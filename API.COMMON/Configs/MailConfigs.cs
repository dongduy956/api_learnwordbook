using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.COMMON.Configs
{
   public class MailConfigs
    {
        public static string Mail { get; set; }
        public static string Pass { get; set; }

        public static void MailConfigurationSettings(IConfiguration configuration)
        {
            Mail = configuration["MailConfigs:Mail"] ?? "";
            Pass = configuration["MailConfigs:Pass"] ?? "";

        }
    }
}
