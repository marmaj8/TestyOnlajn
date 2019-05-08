using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestyOnlajn.Models
{
    public class AnswerSend
    {
        int Id { get; }
        string Text { get; }
        Boolean Correct { get; }

        public AnswerSend(int id, string txt, Boolean correct = false)
        {
            Id = id;
            Text = txt;
            Correct = correct;
        }
    }
}