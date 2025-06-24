using BusinessObject.Models;
using Repository.IRepositories;
using WeddingWonderAPI.Models.DTOs;

namespace Services
{
    public class TopicManager
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TopicManager(ITopicRepository topicRepository, IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TopicDTO>> GetAllTopicsAsync()
        {
            try
            {
                List<Topic> topics = await _topicRepository.GetsAsync();
                return topics.Select(t => new TopicDTO
                {
                    TopicId = t.TopicId,
                    Name = t.Name
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<TopicDTO?> GetTopicByIdAsync(int id)
        {
            try
            {
                Topic topic = await _topicRepository.GetAsyncById(id);
                if (topic == null) return null;

                return new TopicDTO
                {
                    TopicId = topic.TopicId,
                    Name = topic.Name
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> CreateTopicAsync(TopicCreateUpdateRequestDTO request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Topic topic = new()
                {
                    Name = request.Name
                };

                await _topicRepository.CreateAsync(topic);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateTopicAsync(int id, TopicCreateUpdateRequestDTO request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                Topic existingTopic = await _topicRepository.GetAsyncById(id);
                if (existingTopic == null) return false;

                existingTopic.Name = request.Name;

                await _topicRepository.UpdateAsync(id, existingTopic);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteTopicAsync(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _topicRepository.DeleteAsync(id);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
    }
}