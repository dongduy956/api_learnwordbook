using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable enable
namespace API.SERVICES.Models
{
    public class LoginSessionModel : BaseModel
    {
        public string TokenId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string IPAddress { get; set; }
        public DateTime Expired { get; set; }
        public bool IsRevoked { get; set; }
        public int AccountId { get; set; }       
    }
}
