using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.COMMON.Configs
{
    public class GoogleConfigs
    {
        public static string ClientId { get; set; }
        public static string ClientSecret { get; set; }
        public static void GoogleConfigurationSettings(IConfiguration configuration)
        {
            ClientId = configuration["GoogleConfigs:ClientId"] ?? "";
            ClientSecret = configuration["GoogleConfigs:ClientSecret"] ?? "";
        }
    }
}
