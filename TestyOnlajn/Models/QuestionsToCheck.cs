using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestyOnlajn.Models
{
    public class QuestionsToCheck
    {
        public int Id { get; set; }
        public List<QuestionToCheck> Questions { get; set; }
    }
}