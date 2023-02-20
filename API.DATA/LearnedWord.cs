using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.DATA
{
   public class LearnedWord:BaseEntity
    {
        public LearnedWord() { }
        public int AccountId { get; set; }
        public int WordId { get; set; }
        public bool Correct { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        [ForeignKey("WordId")]
        public Word Word { get; set; }
    }
}
