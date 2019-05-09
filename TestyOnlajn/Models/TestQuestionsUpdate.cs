using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestyOnlajn.Models
{
    public class TestQuestionsUpdate
    {
        public int TestId { get; set; }
        public List<QuestionUpdate> Questions { get; set; }
    }
}