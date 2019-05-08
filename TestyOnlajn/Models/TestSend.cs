using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestyOnlajn.Models
{
    public class TestSend
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get;}
        public string Author { get; }
        public int Questions { get; }
        public int Points { get; }

        public TestSend(int id, string name, string description, string author, int questions = 0, int points = 0)
        {
            Id = id;
            Name = name;
            Description = description;
            Author = author;
            Questions = questions;
            Points = points;
        }
    }
}