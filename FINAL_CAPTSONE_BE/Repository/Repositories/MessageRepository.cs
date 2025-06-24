using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MessageDAO _dao;

        public MessageRepository(MessageDAO dao)
        {
            _dao = dao;
        }
         
        public Task<bool> CreateAsync(Message obj) => _dao.CreateMessage(obj);
         
        public Task DeleteAsync(int id) => _dao.DeleteMessage(id);
         
        public Task<Message> GetAsyncById(int id) => _dao.GetMessageById(id);
         
        public Task<List<Message>> GetsAsync() => _dao.GetMessages();
         
        public Task<List<Message>> GetConversation(int senderId, int receiverId) => _dao.GetConversation(senderId, receiverId);
         
        public Task<List<MessageDTO>> GetMessagesByConversationId(int conversationId) => _dao.GetMessagesByConversationId(conversationId);
         
        public Task<int?> GetConversationId(int senderId, int receiverId) => _dao.GetConversationId(senderId, receiverId);
         
        public Task UpdateAsync(int id, Message obj) => _dao.UpdateMessage(id, obj);

        public Task<List<int>> GetConversationIdsForUser(int userId) => _dao.GetConversationIdsForUser(userId);

        public async Task<List<UserDTO>> GetReceiversForUserAsync(int userId)
        {
            return await _dao.GetReceiversForUser(userId);
        }

        public Task<List<Message>> SearchMessages(int userId, int conversationId, string key) => _dao.SearchMessages(userId, conversationId, key);

        public Task<int> EnsureConversationExists(int senderId, int receiverId) => _dao.EnsureConversationExists(senderId, receiverId);

        public Task<int> CreateConversation(int senderId, int receiverId) => _dao.CreateConversation(senderId, receiverId);
    }
}
