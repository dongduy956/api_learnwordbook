using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.DATA
{
    public class LearnedWord : BaseEntity
    {
        public LearnedWord() { }
        public int AccountId { get; set; }
        public int WordId { get; set; }
        public int Rand { get; set; }
        public string Input { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        [ForeignKey("WordId")]
        public Word Word { get; set; }
        [NotMapped]
        public bool Correct
        {
            get
            {
                return (Rand == 0 && Input.ToLower().Trim().Equals(Word.En.ToLower().Trim())) || (Rand == 1 && Input.ToLower().Trim().Equals(Word.Vi.ToLower().Trim()));
            }
        }
    }
}
