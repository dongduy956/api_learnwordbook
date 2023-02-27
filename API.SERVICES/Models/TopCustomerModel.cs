using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.SERVICES.Models
{
    public class TopCustomerModel
    {
        public string FullName { get; set; }
        public int NumberOfWords { get; set; }
        public int NumberOfWordsCorrect { get; set; }
        public int NumberOfWordsWrong { get; set; }
    }
}
