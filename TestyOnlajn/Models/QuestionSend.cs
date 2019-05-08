using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestyOnlajn.Models
{
    public class QuestionSend
    {
        int Id { get; }
        string Text { get; }
        int AnswersCount { get; }
        List<AnswerSend> Answers { get; }

        public QuestionSend(int id, string txt, int count, List<AnswerSend> answers)
        {
            Id = id;
            Text = txt;
            AnswersCount = count;
            Answers = answers;
        }
    }
}