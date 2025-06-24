using BusinessObject.Models;
using DataAccess;
using Repositories.IRepository;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        private readonly TopicDAO _dao;

        public TopicRepository(TopicDAO dao)
        {
            _dao = dao;
        }


 
        public Task<bool> CreateAsync(Topic topic) => _dao.CreateTopic(topic);
        public Task<Topic> GetAsyncById(int id)
        =>_dao.GetTopicById(id);


        public Task<List<Topic>> GetsAsync()
         => _dao.GetAllTopics();
        Task IRepository<Topic>.UpdateAsync(int id, Topic obj)
        => _dao.UpdateTopic(id, obj);

        Task IRepository<Topic>.DeleteAsync(int id)
        => _dao.DeleteTopic(id);
    }
}