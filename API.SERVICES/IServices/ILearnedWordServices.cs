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
        IQueryable<LearnedWordModel> GetAll();
        IQueryable<LearnedWordModel> Searchs(string q="");
    }
}
