using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace compliments_complaints_service.Controllers.Models
{
    public class PostData
    {
        public string Form { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
