using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace API.DATA
{
    public class Word:BaseEntity
    {
        public Word()
        {
            LearnedWords = new HashSet<LearnedWord>();
        }
        public string En { get; set; }
        public string Vi { get; set; }
        public string Type { get; set; }
        public string? Note { get; set; }
        public int TopicId { get; set; }
        [ForeignKey("TopicId")]
        public Topic Topic { get; set; }
        public ICollection<LearnedWord> LearnedWords { get; set; }

    }
}
