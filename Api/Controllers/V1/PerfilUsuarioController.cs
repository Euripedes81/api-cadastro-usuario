using Api.Responses;
using Application.Common;
using Application.DTO.Responses;
using Application.Interfaces.Services.Generics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Tags("Perfis de Usuários")]
    [ApiController]
    [Route("v{version:apiVersion}/perfis")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public class PerfilUsuarioController : ControllerBase
    {
        
        private readonly IReaderAppService<PerfilUsuarioResponseDTO> _perfilUsuarioAppService;
        public PerfilUsuarioController(IReaderAppService<PerfilUsuarioResponseDTO> perfilUsuarioAppService)
        {
            _perfilUsuarioAppService = perfilUsuarioAppService;
        }

        /// <summary>
        /// Obtém um perfil de usuário.
        /// </summary>
        /// <param name="id">ID do perfil de usuário</param> 
        /// <response code="200">Ok</response> 
        /// <returns>Retorna um perfil de usuário.</returns>
        /// <remarks>Obtém um perfil de usuário.</remarks>
        [HttpGet("{id}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        [ProducesResponseType(typeof(SuccessResponse<PerfilUsuarioResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetId([FromRoute] int id)
        {
            var result = await _perfilUsuarioAppService.ObterPorIdAsync(id);

            if (!result.IsSuccess)
            {
                return result.ErrorCode switch
                {
                    ApplicationErrors.PerfilUsuarioNaoEncontrado =>
                        NotFound(new ErrorResponse(
                            MessageResponse.PerfilUsuarioNaoEncontrado,
                            result.ErrorCode
                        )),

                    _ =>
                        StatusCode(
                            StatusCodes.Status500InternalServerError,
                            new ErrorResponse(MessageResponse.ErroInternoServidor)
                        )
                };
            }

            return Ok(new SuccessResponse<PerfilUsuarioResponseDTO>(result.Data!));
        }

        /// <summary>
        /// Obtém perfis de usuários.
        /// </summary>      
        /// <response code="200">Ok</response> 
        /// <returns>Retorna perfis de usuários.</returns>
        /// <remarks>Obtém perfis de usuários.</remarks>
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(typeof(SuccessResponseList<PerfilUsuarioResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {

            var result = await _perfilUsuarioAppService.ObterTodosAsync();

            if (!result.IsSuccess)
            {
                return result.ErrorCode switch
                {
                    ApplicationErrors.PerfilUsuarioNaoEncontrado =>
                        NotFound(new ErrorResponse(
                            MessageResponse.PerfilUsuarioNaoEncontrado,
                            result.ErrorCode
                        )),

                    _ =>
                        StatusCode(
                            StatusCodes.Status500InternalServerError,
                            new ErrorResponse(MessageResponse.ErroInternoServidor)
                        )
                };
            }

            return Ok(new SuccessResponseList<PerfilUsuarioResponseDTO>([.. result.Data!]));
        }
    }
}
