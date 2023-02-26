using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.SERVICES.Models
{
   public class LearnedWordModel:BaseModel
    {
        public int AccountId { get; set; }
        public string? FullName { get; set; }
        public int WordId { get; set; }
        public WordModel? WordModel  { get; set; }
        public int Rand { get; set; }
        public string Input { get; set; }
        public bool Correct { get; set; }
    }
}
