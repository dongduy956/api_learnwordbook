using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.COMMON.Configs
{
    public class AccountConfigs
    {
        public static string DefaultPassword { get; set; }
        public static void AccountConfigurationSettings(IConfiguration configuration)
        {
            DefaultPassword = configuration["AccountConfigs:DefaultPassword"] ?? "12345";
        }
    }
}
