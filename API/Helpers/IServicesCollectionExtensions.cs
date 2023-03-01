using API.REPO.IRepository;
using API.REPO.Repository;
using API.SERVICES.IServices;
using API.SERVICES.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class IServicesCollectionExtensions
    {
        public static void AddServicesRepositories(IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IWordServices, WordServices>();
            services.AddScoped<ITopicServices, TopicServices>();
            services.AddScoped<ILearnedWordServices, LearnedWordServices>();
            services.AddScoped<IStatisticalServices, StatisticalServices>();
            services.AddScoped<IJwtServices, JwtServices>();
            services.AddScoped<ILoginSessionServices, LoginSessionServices>();
            services.AddScoped<IAccountServices, AccountServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IMailServices, MailServices>();
            services.AddHttpContextAccessor();
        }
    }
}
