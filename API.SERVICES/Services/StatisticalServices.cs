using API.SERVICES.IServices;
using API.SERVICES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.SERVICES.Services
{
    public class StatisticalServices : IStatisticalServices
    {
        private readonly ILearnedWordServices learnedWordServices;
        public StatisticalServices(ILearnedWordServices learnedWordServices)
        {
            this.learnedWordServices = learnedWordServices;
        }
        public IList<TopCustomerModel> GetTopCustomer()
        {
            var listTop = new List<TopCustomerModel>();
            var listUser = new List<int>();
            var learnedWords = learnedWordServices.GetAll();
            foreach (var learnedWord in learnedWords)
            {
                if (!listUser.Contains(learnedWord.AccountId))
                    listUser.Add(learnedWord.AccountId);
            }
            foreach (var user in listUser)
            {
                var learnedWordsByUser = learnedWordServices.GetAll(user).ToList();
                listTop.Add(new TopCustomerModel
                {
                    FullName = learnedWordsByUser.First().FullName,
                    NumberOfWords = learnedWordsByUser.Count(),
                    NumberOfWordsCorrect = learnedWordsByUser.Where(x => x.Correct).Count(),
                    NumberOfWordsWrong = learnedWordsByUser.Where(x => !x.Correct).Count()
                });

            }
            return listTop.OrderByDescending(x => x.NumberOfWords)
                          .OrderBy(x => x.NumberOfWordsWrong).Take(10).ToList();
        }
    }
}
