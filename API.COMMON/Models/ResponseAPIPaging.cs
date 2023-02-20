using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.COMMON.Models
{
    public class ResponseAPIPaging : ResponseAPI
    {
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
    }
}
