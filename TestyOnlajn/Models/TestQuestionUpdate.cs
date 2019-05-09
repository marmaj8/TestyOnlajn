using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestyOnlajn.Models
{
    public class TestQuestionUpdate
    {
        public int TestId { get; set; }
        public List<Models.QuestionUpdate> Questions { get; set; }
    }
}