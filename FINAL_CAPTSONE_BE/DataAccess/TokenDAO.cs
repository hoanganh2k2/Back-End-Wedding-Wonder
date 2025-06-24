using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class TokenDAO
    {
        private readonly WeddingWonderDbContext context;

        public TokenDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Token>> GetTokens()
        {
            try
            {
                return await context.Tokens
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<Token> GetTokenById(int userId, string keyName)
        {
            try
            {
                return await context.Tokens
                    .FirstOrDefaultAsync(t => t.UserId == userId && t.KeyName == keyName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CreateToken(Token token)
        {
            try
            {
                await context.Tokens.AddAsync(token);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task DeleteToken(int userId, string keyName)
        {
            try
            {
                Token? tokenToDelete = await context.Tokens.FirstOrDefaultAsync(t => t.UserId == userId && t.KeyName == keyName);
                if (tokenToDelete != null)
                {
                    context.Tokens.Remove(tokenToDelete);
                }
                else
                {
                    throw new Exception("Token not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task SaveRefreshToken(int userId, string refreshToken, int expiration)
        {
            try
            {
                Token? token = await context.Tokens
                    .FirstOrDefaultAsync(t => t.UserId == userId && t.KeyName == "RefreshToken");
                if (token != null)
                {
                    token.KeyValue = refreshToken;
                    token.Expiration = DateTime.Now.AddMinutes(expiration);
                    context.Tokens.Update(token);
                }
                else
                {
                    token = new Token
                    {
                        UserId = userId,
                        KeyName = "RefreshToken",
                        KeyValue = refreshToken,
                        Expiration = DateTime.Now.AddMinutes(expiration)
                    };
                    await context.Tokens.AddAsync(token);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task SaveConfirmEmailToken(int userId, string emailToken, int expiration)
        {
            try
            {
                Token? token = await context.Tokens
                    .FirstOrDefaultAsync(t => t.UserId == userId && t.KeyName == "EmailToken");
                if (token != null)
                {
                    token.KeyValue = emailToken;
                    token.Expiration = DateTime.Now.AddMinutes(expiration);
                    context.Tokens.Update(token);
                }
                else
                {
                    token = new Token
                    {
                        UserId = userId,
                        KeyName = "EmailToken",
                        KeyValue = emailToken,
                        Expiration = DateTime.Now.AddMinutes(expiration)
                    };
                    await context.Tokens.AddAsync(token);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
