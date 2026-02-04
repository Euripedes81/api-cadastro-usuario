using Domain.Entities;

namespace Application.Interfaces.IServices
{
    public interface ITokenService
    {
        public string GerarToken(Usuario usuario);
    }
}
