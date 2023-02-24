using API.SERVICES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.SERVICES.IServices
{
    public interface ILearnedWordServices
    {
        IQueryable<LearnedWordModel> GetAll(int accountId);
        IQueryable<LearnedWordModel> GetAllIncorrect(int accountId);
        IQueryable<LearnedWordModel> Search(int accountId,string q="");
        Task<bool> InsertRangeAsync(IList<LearnedWordModel> models);
        Task<bool> UpdateRangeAsync(IList<LearnedWordModel> models);
    }
}
