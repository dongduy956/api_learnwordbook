using API.SERVICES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.SERVICES.IServices
{
    public interface IWordServices
    {
        Task<WordModel?> GetAsync(int id);
        IQueryable<WordModel> GetAll();
        IQueryable<WordModel> GetWordsByTopicId(int topicId);
        IQueryable<WordModel> Searchs(string q="");
        Task<bool> InsertAsync(WordModel model);
        Task<bool> InsertRangeAsync(IList<WordModel> models);
        Task<bool> DeteleAsync(int id);
        Task<bool> UpdateAsync(int id, WordModel model);

    }
}
