using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestyOnlajn.Models
{
    public class QuestionUpdate
    {
        public int TestId { get; set; }
        public int QuestionId { get; set; }
        public int CorrectAnswers { get; set; }
        public List<AnswerSend> Answers { get; set; }
        public string Text { get; set; }
    }
}