using API.COMMON.Enum;
using API.DATA;
using API.REPO.IRepository;
using API.SERVICES.IServices;
using API.SERVICES.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.SERVICES.Services
{
    public class LearnedWordServices : ILearnedWordServices
    {
        private readonly IRepository<LearnedWord> repository;
        public LearnedWordServices(IRepository<LearnedWord> repository)
        {
            this.repository = repository;
        }
        public IQueryable<LearnedWordModel> GetAll()
        {
            return repository.GetAll(SelectEnum.Select.NONTRASH)
                              .Include(x => x.Account)
                                    .ThenInclude(x => x.User)
                              .Include(x => x.Word)
                              .Select(x => new LearnedWordModel
                              {
                                  AccountId = x.AccountId,
                                  Correct = x.Correct,
                                  CreateAt = x.CreateAt,
                                  CreateBy = x.CreateBy,
                                  Id = x.Id,
                                  Fullname = x.Account.User.FullName,
                                  WordId = x.WordId,
                                  WordModel = new WordModel
                                  {
                                      Id = x.Word.Id,
                                      En = x.Word.En,
                                      Vi = x.Word.Vi,
                                      Note = x.Word.Note,
                                      TopicId = x.Word.TopicId,
                                      TopicName = x.Word.Topic.Name,
                                      Type = x.Word.Type,
                                      CreateAt = x.Word.CreateAt,
                                      CreateBy = x.Word.CreateBy,
                                  }
                              });
        }

        public IQueryable<LearnedWordModel> Searchs(string q = "")
        {
            q = q.ToLower().Trim();
            return repository.GetAll(SelectEnum.Select.NONTRASH)
                               .Include(x => x.Account)
                                     .ThenInclude(x => x.User)
                               .Include(x => x.Word)
                               .Where(x => x.Account.User.FullName.ToLower().Trim().Contains(q) ||
                                         x.Word.En.ToLower().Trim().Contains(q) ||
                                         x.Word.Type.ToLower().Trim().Equals(q) ||
                                         x.Word.Vi.ToLower().Trim().Contains(q))
                               .Select(x => new LearnedWordModel
                               {
                                   AccountId = x.AccountId,
                                   Correct = x.Correct,
                                   CreateAt = x.CreateAt,
                                   CreateBy = x.CreateBy,
                                   Id = x.Id,
                                   Fullname = x.Account.User.FullName,
                                   WordId = x.WordId,
                                   WordModel = new WordModel
                                   {
                                       Id = x.Word.Id,
                                       En = x.Word.En,
                                       Vi = x.Word.Vi,
                                       Note = x.Word.Note,
                                       TopicId = x.Word.TopicId,
                                       TopicName = x.Word.Topic.Name,
                                       Type = x.Word.Type,
                                       CreateAt = x.Word.CreateAt,
                                       CreateBy = x.Word.CreateBy,
                                   }
                               });
        }
    }
}
