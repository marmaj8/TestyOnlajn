using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestyOnlajn.Models
{
    public class QuestionSend
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int AnswersCount { get; set; }
        public List<AnswerSend> Answers { get; set; }

        public QuestionSend(int id, string txt, int count, List<AnswerSend> answers)
        {
            Id = id;
            Text = txt;
            AnswersCount = count;
            Answers = answers;
        }
    }
}