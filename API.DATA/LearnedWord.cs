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

                var result=false;
                if (Rand == 0)
                    result = Input.Equals(Word.En);
                else
                {                    
                    var vis = Word.Vi.Split(',').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))
                                                .Select(x=>x.Trim());
                    var inputs = Input.Split(',').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))
                                                .Select(x => x.Trim());
                    result = vis.Union(inputs).ToArray().Count() == vis.Count();
                }
                return result;
            }
        }
    }
}
