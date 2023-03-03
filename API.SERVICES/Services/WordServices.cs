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
    public class WordServices : IWordServices
    {
        private readonly IRepository<Word> repository;
        public WordServices(IRepository<Word> repository)
        {
            this.repository = repository;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var model = await repository.GetAsync(id);
            if (model == null)
                return false;
            var result = await repository.DeleteAsync(model);
            return result;
        }

        public IQueryable<WordModel> GetAll()
        {
            return repository.GetAll(SelectEnum.Select.NONTRASH)
                              .Include(x => x.Topic)
                              .Select(x => new WordModel
                              {
                                  Id = x.Id,
                                  En = x.En,
                                  Vi = x.Vi,
                                  Note = x.Note,
                                  TopicId = x.TopicId,
                                  TopicName = x.Topic.Name,
                                  Type = x.Type,
                                  CreateAt = x.CreateAt,
                                  CreateBy = x.CreateBy,
                              });
        }

        public async Task<WordModel?> GetAsync(int id)
        {
            var x = await repository.GetAsync(id);
            if (x == null)
                return null;
            return new WordModel
            {
                Id = x.Id,
                En = x.En,
                Vi = x.Vi,
                Note = x.Note,
                TopicId = x.TopicId,
                TopicName = x.Topic.Name,
                Type = x.Type,
                CreateAt = x.CreateAt,
                CreateBy = x.CreateBy,
            };
        }

        public IQueryable<WordModel> GetWordsByTopicId(int topicId)
        {
            return GetAll().Where(x => x.TopicId == topicId);
        }

        public async Task<bool> InsertAsync(WordModel model)
        {
            var word = new Word
            {
                CreateBy = model.CreateBy,
                En = model.En.Trim().ToLower(),
                Vi = model.Vi.Trim().ToLower(),
                Note = model.Note,
                TopicId = model.TopicId,
                Type = model.Type.Trim().ToLower(),
            };
            var result = await repository.InsertAsync(word);
            if (result)
            {
                model.Id = word.Id;
                model.CreateAt = word.CreateAt;
            }
            return result;
        }

        public async Task<bool> InsertRangeAsync(IList<WordModel> models)
        {
            var words = new List<Word>();
            foreach (var model in models)
            {
                words.Add(new Word
                {
                    CreateBy = model.CreateBy,
                    En = model.En.Trim().ToLower(),
                    Vi = model.Vi.Trim().ToLower(),
                    Note = model.Note,
                    TopicId = model.TopicId,
                    Type = model.Type.Trim().ToLower(),
                });
            }
            var result = await repository.InsertRangeAsync(words);
            if (result)
            {
                for (int i = 0; i < models.Count; i++)
                {
                    models[i].Id = words[i].Id;
                    models[i].CreateAt = words[i].CreateAt;
                }
            }
            return result;
        }

        public IQueryable<WordModel> Search(string q = "")
        {
            q = q.ToLower().Trim();
            return repository.GetAll(SelectEnum.Select.NONTRASH)
                              .Include(x => x.Topic)
                              .Where(x => x.En.ToLower().Trim().Contains(q)
                                        || x.Vi.ToLower().Trim().Contains(q)
                                        || x.Type.ToLower().Trim().Equals(q)
                                        || x.Topic.Name.ToLower().Trim().Contains(q))
                              .Select(x => new WordModel
                              {
                                  Id = x.Id,
                                  En = x.En,
                                  Vi = x.Vi,
                                  Note = x.Note,
                                  TopicId = x.TopicId,
                                  TopicName = x.Topic.Name,
                                  Type = x.Type,
                                  CreateAt = x.CreateAt,
                                  CreateBy = x.CreateBy,
                              });
        }

        public async Task<bool> UpdateAsync(int id, WordModel model)
        {
            var word = await repository.GetAsync(id);
            if (word == null)
                return false;
            word.En = model.En.Trim().ToLower();
            word.Vi = model.Vi.Trim().ToLower();
            word.Type = model.Type.Trim().ToLower();
            word.Note = model.Note;
            word.TopicId = model.TopicId;
            var result = await repository.UpdateAsync(word);
            return result;
        }
    }
}
