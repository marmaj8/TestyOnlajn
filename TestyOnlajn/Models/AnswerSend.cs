using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestyOnlajn.Models
{
    public class AnswerSend
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Boolean Correct { get; set; }

        public AnswerSend(int id, string txt, Boolean correct = false)
        {
            Id = id;
            Text = txt;
            Correct = correct;
        }
    }
}