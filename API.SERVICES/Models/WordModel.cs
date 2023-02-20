using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace API.SERVICES.Models
{
    public class WordModel:BaseModel
    {
        
        public string En { get; set; }
        public string Vi { get; set; }
        public string Type { get; set; }
        public string? Note { get; set; }
        public int TopicId { get; set; }
        public string? TopicName { get; set; }

    }
}
