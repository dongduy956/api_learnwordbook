using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace API.SERVICES.Models
{
   public class BaseModel
    {
        public int Id { get; set; }
        public DateTime CreateAt { get; set; }
        public string? CreateBy { get; set; }
    }
}
