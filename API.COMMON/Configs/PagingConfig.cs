using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.COMMON.Configs
{
    public class PagingConfig
    {
        public static int PageSize { get; set; }
        public static void PagingConfigurationSettings(IConfiguration configuration)
        {
            PageSize = Convert.ToInt32(configuration["PagingConfig:PageSize"] ?? "1");
        }
    }
}
