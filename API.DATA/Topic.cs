using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.DATA
{
    public class Topic : BaseEntity
    {
        public Topic()
        {
            Words = new HashSet<Word>();
        }
        public string Name { get; set; }
        public ICollection<Word> Words { get; set; }
    }
}
