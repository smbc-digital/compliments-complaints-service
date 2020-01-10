using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace compliments_complaints_service.Controllers.Models
{
    public class Answer
    {
        public string QuestionId { get; set; }
        public string Response { get; set; }
    }
}
