using Api.Responses;
using Application.DTO.Create;
using Application.DTO.Responses;
using Application.Interfaces.IServices;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Tags("Usuários")]
    [ApiController]
    [Route("v{version:apiVersion}/usuarios")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioAppService _service;
        public UsuarioController(IUsuarioAppService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtém um usuario.
        /// </summary>
        /// <param name="id">E-mail do usuario</param> 
        /// <response code="200">Ok</response> 
        /// <returns>Retorna um usuario.</returns>
        /// <remarks>Obtémum usuario.</remarks>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<UsuarioResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetId([FromRoute] int id)
        {
            var result = await _service.ObterPorIdAsync(id);

            if (!result.IsSuccess)
            {
                return result.ErrorCode switch
                {
                    ApplicationErrors.UsuarioNaoEncontrado =>
                        NotFound(new ErrorResponse(
                            MessageResponse.UsuarioNaoEncontrado,
                            result.ErrorCode
                        )),

                    _ =>
                        StatusCode(
                            StatusCodes.Status500InternalServerError,
                            new ErrorResponse("Erro interno no servidor")
                        )
                };
            }

            return Ok(new SuccessResponse<UsuarioResponseDTO>(result.Data!));
        }

        /// <summary>
        /// Obtém usuários.
        /// </summary>      
        /// <response code="200">Ok</response> 
        /// <returns>Retorna usuários.</returns>
        /// <remarks>Obtém usuários.</remarks>
        //[HttpGet]
        //[ProducesResponseType(typeof(SuccessResponseList<UsuarioResponseDTO>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ErrorResponse<StatusCodeResponse>), StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(typeof(ErrorResponse<StatusCodeResponse>), StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> Get()
        //{
        //    ICollection<UsuarioResponseDTO> usuarios = await _service.ObterTodosAsync();

        //    if (!string.IsNullOrEmpty(usuarios.ToList()[0].Mensagem))
        //    {
        //        return StatusCode(
        //            usuarios.ToList()[0].Code,
        //            new ErrorResponse<StatusCodeResponse>(
        //                    StatusCodeResponseCreator.Create(
        //                        message: usuarios.ToList()[0].Mensagem,
        //                        code: usuarios.ToList()[0].Code
        //                )));
        //    }

        //    return Ok(new SuccessResponseList<UsuarioResponseDTO>(usuarios.ToList()));
        //}

        /// <summary>
        /// Cria um token para o usuário.
        /// </summary>        
        /// /// <param name="loginCreateDTO"></param>
        /// <response code="200">Ok</response>
        /// <response code="401">Unauthorized</response>
        /// <returns>Retorna um token jwt.</returns>
        /// <remarks>Cria um token quando o usuário é autenticado.</remarks>   
        [HttpPost("logins")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SuccessResponse<UsuarioResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse<StatusCodeResponse>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse<StatusCodeResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostLogin([FromBody] LoginDTO loginCreateDTO)
        {
            var result = await _service.FazerLoginAsync(loginCreateDTO);
            if (!string.IsNullOrEmpty(result.Mensagem))
            {
                return StatusCode(
                    result.Code,
                    new ErrorResponse<StatusCodeResponse>(
                            StatusCodeResponseCreator.Create(
                                message: result.Mensagem,
                                code: result.Code
                        )));
            }

            return Ok(new SuccessResponse<LoginResponseDTO>(result));
        }

        /// <summary>
        /// Cria um usuário.
        /// </summary>        
        /// /// <param name="usuarioCreateDTO"></param>
        /// <response code="200">Ok</response>
        /// <response code="401">Unauthorized</response>
        /// <returns>Cria um usuario.</returns>       
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SuccessResponse<UsuarioResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse  ), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UsuarioDTO usuarioCreateDTO)
        {
            var result = await _service.AdicionarAsync(usuarioCreateDTO);
         
            if (result.IsSuccess)
            {
                return CreatedAtAction(
                    nameof(GetId),
                    new { id = result.Data },
                    new { id = result.Data }
                );
            }

            return result.ErrorCode switch
            {
                ApplicationErrors.EmailJaExiste =>
                    Conflict(new ErrorResponse(
                        MessageResponse.EmailJaExiste
                    )),

                _ =>
                    StatusCode(
                        StatusCodes.Status500InternalServerError,
                        new ErrorResponse("Erro interno no servidor")
                    )
            };
        }


        /// <summary>
        /// Atualiza um usuário.
        /// </summary>   
        /// <param name="id"></param>
        /// <param name="usuarioUpdateDTO"></param>
        /// <response code="200">Ok</response>
        /// <response code="401">Unauthorized</response>
        /// <returns>Atualiza um usuario.</returns>        
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<GenericResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UsuarioDTO usuarioUpdateDTO)
        {
            var result = await _service.AtualizarAsync(id, usuarioUpdateDTO);

            if (result.IsSuccess)
            {
                return Ok(new SuccessResponse<GenericResponseDTO>(new GenericResponseDTO { Id = result.Data! }));                
            }

            return result.ErrorCode switch
            {
                ApplicationErrors.UsuarioNaoEncontrado =>
                    NotFound(new ErrorResponse(
                        MessageResponse.UsuarioNaoEncontrado,
                        result.ErrorCode
                    )),

                ApplicationErrors.EmailJaExiste =>
                    Conflict(new ErrorResponse(
                        MessageResponse.EmailJaExiste,
                        result.ErrorCode
                    )),

                _ =>
                    StatusCode(
                        StatusCodes.Status500InternalServerError,
                        new ErrorResponse("Erro interno no servidor")
                    )
            };

        }

    }
}