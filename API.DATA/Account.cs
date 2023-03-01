using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.DATA
{
    public class Account : BaseEntity
    {
        public Account()
        {
            LoginSessions = new HashSet<LoginSession>();
            LearnedWords = new HashSet<LearnedWord>();
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsLock { get; set; }
        public int UserId { get; set; }
        public string? Code { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public ICollection<LoginSession> LoginSessions { get; set; }
        public ICollection<LearnedWord> LearnedWords { get; set; }

    }
}
