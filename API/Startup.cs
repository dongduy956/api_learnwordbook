using API.COMMON.Configs;
using API.Helpers;
using API.Middlewares;
using API.REPO;
using API.SERVICES.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            #region register config
            PagingConfigs.PagingConfigurationSettings(Configuration);
            AccountConfigs.AccountConfigurationSettings(Configuration);
            JwtConfigs.JwtConfigurationSettings(Configuration);
            UploadConfigs.UploadConfigurationSettings(Configuration);
            MailConfigs.MailConfigurationSettings(Configuration);
            GoogleConfigs.GoogleConfigurationSettings(Configuration);
            #endregion
            #region register jwt
            var secretBytes = Encoding.UTF8.GetBytes(JwtConfigs.SecretKey);
            var tokenParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
                ClockSkew = TimeSpan.Zero

            };
            services.AddSingleton(tokenParameters);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = tokenParameters;
                    opt.Events = new JwtBearerEvents();
                    opt.Events.OnTokenValidated = async (context) =>
                        {
                            var jwtServices = context.HttpContext.RequestServices.GetService<IJwtServices>();
                            var jwtToken = context.SecurityToken as JwtSecurityToken;
                            if (!(jwtServices.IsTokenLive(jwtToken.RawData)))
                            {
                                context.HttpContext.Response.Headers.Remove("Authorization");
                                context.Fail("Invalid Token");
                            }
                        };
                });
            #endregion

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
            IServicesCollectionExtensions.AddServicesRepositories(services);
            services.AddDbContext<LearnWordBookContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("LearnWordBookContext"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(opt =>
            {
                opt.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            });
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //Auto create folder Images in wwwroot
            if (!Directory.Exists(Path.Combine(env.ContentRootPath + "//wwwroot//", UploadConfigs.Image)))
                Directory.CreateDirectory(Path.Combine(env.ContentRootPath + "//wwwroot//", UploadConfigs.Image));
            app.UseStaticFiles();

            app.UseMiddleware<GlobalExceptonHandlingMiddlewares>();
        }
    }
}
