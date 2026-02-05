using Application.DTO.Create;
using Application.DTO.Responses;
using Domain.Entities;

namespace Application.Extensions
{
    internal static class UsuarioMapping
    {
        public static UsuarioResponseDTO MapToResponseDTO(this Usuario usuario)
        {
            return new UsuarioResponseDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = new PerfilUsuarioResponseDTO
                {
                    Id = usuario.PerfilUsuarioId,
                    Nome = usuario.PerfilUsuario!.Nome
                },
                Inativo = usuario.Inativo

            };
        }

        public static Usuario MapToEntity(this UsuarioDTO usuarioDTO)
        {
            return new Usuario
            {
                Nome = usuarioDTO.Nome,
                Email = usuarioDTO.Email,
                Senha = usuarioDTO.Senha,
                PerfilUsuarioId = usuarioDTO.Perfil!.Id,
                Inativo = usuarioDTO.Inativo
            };
        }
    }
}
