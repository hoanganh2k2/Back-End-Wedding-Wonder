using BusinessObject.DTOs;
using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<List<MessageDTO>> GetMessagesByConversationId(int conversationId);
         
        Task<int?> GetConversationId(int senderId, int receiverId);
         
        Task<List<Message>> GetConversation(int senderId, int receiverId);

        Task<int> EnsureConversationExists(int senderId, int receiverId);

        Task<int> CreateConversation(int senderId, int receiverId);

        Task<List<int>> GetConversationIdsForUser(int userId);

        Task<List<UserDTO>> GetReceiversForUserAsync(int userId);

        Task<List<Message>> SearchMessages(int userId, int conversationId, string key);
    }
}
