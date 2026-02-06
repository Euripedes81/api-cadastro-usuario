using Api.Responses;
using Application.Common;
using Application.DTO.Create;
using Application.DTO.Responses;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Tags("Usuários")]
    [ApiController]
    [Route("v{version:apiVersion}/usuarios")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioAppService _usuarioService;
        public UsuarioController(IUsuarioAppService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Obtém um usuario.
        /// </summary>
        /// <param name="id">ID do usuario</param> 
        /// <response code="200">Ok</response> 
        /// <returns>Retorna um usuario.</returns>
        /// <remarks>Obtémum usuario.</remarks>
        [HttpGet("{id}")]       
        [ProducesResponseType(typeof(SuccessResponse<UsuarioResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetId([FromRoute] int id)
        {
            if (User.IsInRole("Usuario") && User.FindFirst(ClaimTypes.NameIdentifier)?.Value != id.ToString())
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    new ErrorResponse(MessageResponse.NaoPermitido, StatusCodes.Status403Forbidden.ToString())
               );
            }

            var result = await _usuarioService.ObterPorIdAsync(id);

            if (!result.IsSuccess)
            {
                return result.ErrorCode switch
                {
                    ApplicationErrors.UsuarioNaoEncontrado =>
                        NotFound(new ErrorResponse(
                            MessageResponse.UsuarioNaoEncontrado,
                            StatusCodes.Status404NotFound.ToString()
                        )),

                    _ =>
                        StatusCode(
                            StatusCodes.Status500InternalServerError,
                            new ErrorResponse(MessageResponse.ErroInternoServidor)
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
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(typeof(SuccessResponseList<UsuarioResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]        
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {       
            var result = await _usuarioService.ObterTodosAsync();

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
                            new ErrorResponse(MessageResponse.ErroInternoServidor)
                        )
                };
            }

            return Ok(new SuccessResponseList<UsuarioResponseDTO>(result.Data!.ToList()));
         
        }

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
        [ProducesResponseType(typeof(LoginResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostLogin([FromBody] LoginDTO loginCreateDTO)
        {
            var result = await _usuarioService.FazerLoginAsync(loginCreateDTO);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
           
            return result.ErrorCode switch
            {
                ApplicationErrors.CredenciaisInvalidas =>
                    Unauthorized(new ProblemDetails
                    {
                        Title = MessageResponse.CredenciaisInvalidas,
                        Status = StatusCodes.Status401Unauthorized,
                        Detail = MessageResponse.EmailSenhaInvalidos
                    }),

                _ =>
                    StatusCode(
                        StatusCodes.Status500InternalServerError,
                        new ProblemDetails
                        {
                            Title = MessageResponse.ErroInternoServidor,
                            Status = StatusCodes.Status500InternalServerError
                        })
            };          
        }

        /// <summary>
        /// Cria um usuário.
        /// </summary>        
        /// /// <param name="usuarioCreateDTO"></param>
        /// <response code="201">Created</response>
        /// <response code="401">Unauthorized</response>
        /// <returns>Cria um usuario.</returns>       
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(typeof(SuccessResponse<UsuarioResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse  ), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] UsuarioDTO usuarioCreateDTO)
        {
            var result = await _usuarioService.AdicionarAsync(usuarioCreateDTO);
         
            if (result.IsSuccess)
            {               
                return CreatedAtAction(
                   nameof(GetId),
                   new { id = result.Data },
                   new SuccessResponse<GenericResponseDTO>(new GenericResponseDTO { Id = result.Data! })
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
                        new ErrorResponse(MessageResponse.ErroInternoServidor)
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
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(typeof(SuccessResponse<GenericResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UsuarioDTO usuarioUpdateDTO)
        {
            var result = await _usuarioService.AtualizarAsync(id, usuarioUpdateDTO);

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
                        new ErrorResponse(MessageResponse.ErroInternoServidor)
                    )
            };
        }

        /// <summary>
        /// Remove um usuário.
        /// </summary>       
        /// <param name="id"></param>      
        /// <returns>Retorna no content.</returns>
        /// <remarks>Remove um usuário.</remarks>          
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _usuarioService.RemoverAsync(id);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return result.ErrorCode switch
            {
                ApplicationErrors.UsuarioNaoEncontrado =>
                    NotFound(new ErrorResponse(
                        MessageResponse.UsuarioNaoEncontrado,
                        StatusCodes.Status404NotFound.ToString()
                    )),
                ApplicationErrors.AcessoNegado =>
                    StatusCode(
                         StatusCodes.Status403Forbidden,
                        new ErrorResponse(MessageResponse.NaoPermitido, StatusCodes.Status403Forbidden.ToString())                     
                    ),
                _ =>
                    StatusCode(
                        StatusCodes.Status500InternalServerError,
                        new ErrorResponse(MessageResponse.ErroInternoServidor)
                    )
            };           
        }
    }
}