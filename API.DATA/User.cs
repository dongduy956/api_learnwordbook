using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace API.DATA
{
    public class User:BaseEntity
    {
        public User() { }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Avatar { get; set; }
        public Account? Account { get; set; }
    }
}
