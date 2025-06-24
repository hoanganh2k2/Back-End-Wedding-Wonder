using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface ITokenRepository : IRepository<Token>
    {
        Task<Token> GetTokenById(int userId, string keyName);
        Task DeleteToken(int userId, string keyName);
        Task SaveRefreshToken(int userId, string refreshToken, int expiration);
        Task SaveConfirmEmailToken(int userId, string emailToken, int expiration);
    }
}
