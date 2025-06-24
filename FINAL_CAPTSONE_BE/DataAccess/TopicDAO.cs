using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class TopicDAO
    {
        private readonly WeddingWonderDbContext _context;

        public TopicDAO(WeddingWonderDbContext context)
        {
            _context = context;
        }

        public async Task<List<Topic>> GetAllTopics()
        {
            try
            {
                return await _context.Topics.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Topic> GetTopicById(int id)
        {
            try
            {
                return await _context.Topics.FindAsync(id) ?? throw new Exception($"Topic with id {id} not found.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> CreateTopic(Topic topic)
        {
            try
            {
                await _context.Topics.AddAsync(topic);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateTopic(int id, Topic topic)
        {
            try
            {
                var existingTopic = await GetTopicById(id);
                existingTopic.Name = topic.Name;
                existingTopic.UpdatedAt = DateTime.Now;

                _context.Topics.Update(existingTopic);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteTopic(int id)
        {
            try
            {
                var topic = await GetTopicById(id);
                _context.Topics.Remove(topic);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}