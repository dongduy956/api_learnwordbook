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
    public class TopicServices : ITopicServices
    {
        private readonly IRepository<Topic> repository;
        public TopicServices(IRepository<Topic> repository)
        {
            this.repository = repository;
        }

        public async Task<bool> DeteleAsync(int id)
        {
            var model = await repository.GetAsync(id);
            if (model == null)
                return false;
            var result = await repository.DeleteAsync(model);
            return result;
        }

        public IQueryable<TopicModel> GetAll()
        {
            return repository.GetAll(SelectEnum.Select.NONTRASH)
                              .Select(x => new TopicModel
                              {
                                  Id = x.Id,
                                  CreateAt = x.CreateAt,
                                  CreateBy = x.CreateBy,
                                  Name = x.Name
                              });
        }

        public async Task<bool> InsertAsync(TopicModel model)
        {
            var word = new Topic
            {
                CreateBy = model.CreateBy,
                Name = model.Name
            };
            var result = await repository.InsertAsync(word);
            if (result)
            {
                model.Id = word.Id;
                model.CreateAt = word.CreateAt;
            }
            return result;
        }

        public async Task<bool> InsertRangeAsync(IList<TopicModel> models)
        {
            var topics = new List<Topic>();
            foreach (var model in models)
            {
                topics.Add(new Topic
                {
                    CreateBy = model.CreateBy,
                    Name = model.Name
                });
            }
            var result = await repository.InsertRangeAsync(topics);
            if (result)
            {
                for (int i = 0; i < models.Count; i++)
                {
                    models[i].Id = topics[i].Id;
                    models[i].CreateAt = topics[i].CreateAt;
                }
            }
            return result;
        }

        public IQueryable<TopicModel> Searchs(string q = "")
        {
            q = q.ToLower().Trim();
            return repository.GetAll(SelectEnum.Select.NONTRASH)
                              .Where(x => x.Name.ToLower().Trim().Contains(q))
                              .Select(x => new TopicModel
                              {
                                  Id = x.Id,
                                  CreateAt = x.CreateAt,
                                  CreateBy = x.CreateBy,
                                  Name = x.Name
                              });
        }

        public async Task<bool> UpdateAsync(int id, TopicModel model)
        {
            var topic = await repository.GetAsync(id);
            if (topic == null)
                return false;
            topic.Name = model.Name;
            var result = await repository.UpdateAsync(topic);
            return result;
        }
    }
}
