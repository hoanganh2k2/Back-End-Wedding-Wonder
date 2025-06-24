using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly TokenDAO _dao;

        public TokenRepository(TokenDAO dao)
        {
            _dao = dao;
        }

        public Task<bool> CreateAsync(Token obj) => _dao.CreateToken(obj);
        public Task DeleteAsync(int id) => throw new NotImplementedException();
        public Task DeleteToken(int userId, string keyName) => _dao.DeleteToken(userId, keyName);
        public Task<Token> GetAsyncById(int id) => throw new NotImplementedException();
        public Task<List<Token>> GetsAsync() => _dao.GetTokens();
        public Task<Token> GetTokenById(int userId, string keyName) => _dao.GetTokenById(userId, keyName);
        public Task UpdateAsync(int id, Token obj) => throw new NotImplementedException();
        public Task SaveRefreshToken(int userId, string refreshToken, int expiration) => _dao.SaveRefreshToken(userId, refreshToken, expiration);
        public Task SaveConfirmEmailToken(int userId, string emailToken, int expiration) => _dao.SaveConfirmEmailToken(userId, emailToken, expiration);
    }
}
