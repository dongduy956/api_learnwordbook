using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable enable
namespace API.SERVICES.Models
{
   public class AccountModel:BaseModel
    {
        public string Username { get; set; }
        public string? Password { get; set; }
        public int UserId { get; set; }
    }
}
