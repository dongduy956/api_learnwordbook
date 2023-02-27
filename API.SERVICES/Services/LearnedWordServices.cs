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
        public IQueryable<LearnedWordModel> GetAll(int accountId)
        {
            return GetAll().Where(x => x.AccountId == accountId);
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
                                  Input = x.Input,
                                  Rand = x.Rand,
                                  CreateAt = x.CreateAt,
                                  CreateBy = x.CreateBy,
                                  Correct = x.Correct,
                                  Id = x.Id,
                                  FullName = x.Account.User.FullName,
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

        public IQueryable<LearnedWordModel> GetAllIncorrect(int accountId)
        {
            return GetAll(accountId).ToList().Where(x => !x.Correct).AsQueryable();
        }

        public async Task<bool> InsertRangeAsync(IList<LearnedWordModel> models)
        {
            var learnedWords = new List<LearnedWord>();
            foreach (var model in models)
            {
                learnedWords.Add(new LearnedWord
                {
                    CreateBy = model.CreateBy,
                    AccountId = model.AccountId,
                    Input=model.Input,
                    Rand=model.Rand,
                    WordId = model.WordId,
                });
            }
            
            var result = await repository.InsertRangeAsync(learnedWords);
            if (result)
            {
                for (int i = 0; i < models.Count; i++)
                {
                    models[i].Id = learnedWords[i].Id;
                    models[i].CreateAt = learnedWords[i].CreateAt;
                }
            }
            return result;
        }

        public IQueryable<LearnedWordModel> Search(int accountId,string q = "")
        {
            q = q.ToLower().Trim();
            return repository.GetAll(SelectEnum.Select.NONTRASH)
                               .Where(x=>x.AccountId==accountId)
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
                                   FullName = x.Account.User.FullName,
                                   WordId = x.WordId,
                                   Rand=x.Rand,
                                   Input=x.Input,
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

        public async Task<bool> UpdateRangeAsync(IList<LearnedWordModel> models)
        {
            var list = new List<LearnedWord>();
            foreach (var model in models)
            {
                var result = await repository.GetAsync(model.Id);
                if (result != null)
                {
                    result.Input = model.Input;
                    result.Rand = model.Rand;
                    list.Add(result);
                }
            }
            if (list.Count == 0)
                return false;
            return await repository.UpdateRangeAsync(list);
        }
    }
}
