using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.SERVICES.Models
{
   public class JwtResponse:JwtRequest
    {
        public object User { get; set; }
        public int ExpiredTime { get; set; }
        public string Avatar { get; set; }
    }
}
