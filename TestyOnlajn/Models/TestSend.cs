using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestyOnlajn.Models
{
    public class TestSend
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public int Questions { get; set; }
        public int Points { get; set; }
        public int Result { get; set; }
        public Boolean IsYour { get; set; }

        public TestSend(int id, string name, string description, string author, int questions, int points, int result, Boolean isY)
        {
            Id = id;
            Name = name;
            Description = description;
            Author = author;
            Questions = questions;
            Points = points;
            Result = result;
            IsYour = isY;
        }
    }
}