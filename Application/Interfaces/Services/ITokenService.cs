using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ITokenService
    {
        public string GerarToken(Usuario usuario);
    }
}
