using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.COMMON.Configs
{
   public class JwtConfigs
    {
        public static int AccessTokenTime { get; set; }
        public static int RefreshTokenTime { get; set; }
        public static string SecretKey { get; set; }
        public static void JwtConfigurationSettings(IConfiguration configuration)
        {
            AccessTokenTime = Convert.ToInt32(configuration["JwtConfigs:AccessTokenTime"] ?? "1");
            RefreshTokenTime = Convert.ToInt32(configuration["JwtConfigs:RefreshTokenTime"] ?? "1");
            SecretKey = configuration["JwtConfigs:SecretKey"] ?? "";
        }
    }
}
