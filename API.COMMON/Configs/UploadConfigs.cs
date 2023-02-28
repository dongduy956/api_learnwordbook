using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.COMMON.Configs
{
   public class UploadConfigs
    {
        public static string Image { get; set; }
        public static void UploadConfigurationSettings(IConfiguration configuration)
        {
            Image = configuration["UploadConfigs:Image"] ?? "";
        }
    }
}
