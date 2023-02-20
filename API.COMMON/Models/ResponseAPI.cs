using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace API.COMMON.Models
{
   public class ResponseAPI
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string[]? Messages { get; set; }
        public object? Data { get; set; }
    }
}
