using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestyOnlajn.Models
{
    public class TestSend
    {
        public string Name { get; }
        public string Description { get;}
        public string Author { get; }

        public TestSend(string name, string description, string author)
        {
            Name = name;
            Description = description;
            Author = author;
        }
    }
}