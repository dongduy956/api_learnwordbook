using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable enable
namespace API.DATA
{
    public class LoginSession : BaseEntity
    {
        public string TokenId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string IPAddress { get; set; }
        public DateTime Expired { get; set; }
        public bool IsRevoked { get; set; }
        [NotMapped]
        public bool IsExpired
        {
            get
            {
                return DateTime.UtcNow > Expired;
            }
        }

        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
    }
}
