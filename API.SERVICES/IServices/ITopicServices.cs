using API.SERVICES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.SERVICES.IServices
{
    public interface ITopicServices
    {
        IQueryable<TopicModel> GetAll();
        IQueryable<TopicModel> Search(string q="");
        Task<bool> InsertAsync(TopicModel model);
        Task<bool> InsertRangeAsync(IList<TopicModel> models);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(int id, TopicModel model);

    }
}
