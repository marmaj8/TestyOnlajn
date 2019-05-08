using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Cors;

namespace TestyOnlajn.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TestsController : ApiController
    {
        Models.TestyOnlineEntities db = new Models.TestyOnlineEntities();

        [HttpGet]
        [Authorize]
        public List<Models.TestSend> List()
        {
            List<Models.TestSend> tests = new List<Models.TestSend>();

            foreach (Models.tests test in db.tests)
            {
                tests.Add(new Models.TestSend(test.id, test.name, test.descript, test.UserLogin.UserName));
            }

            return tests;
        }

        [HttpPost]
        [Authorize]
        public int Create(string name, string desc)
        {
            User = System.Web.HttpContext.Current.User;
            Models.tests test = new Models.tests();
            test.name = name;
            test.descript = desc;
            //test.author = db.UserLogin.First(u => u.UserEmail == User.Identity.Name).Id;

            int author;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out author);
            test.author = author;

            db.tests.Add(test);
            db.SaveChanges();

            return test.id;
        }

        [HttpPost]
        [Authorize]
        public string Update(int id, string name, string desc)
        {
            Models.tests test = db.tests.First(t => t.id == id);
            
            int author;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out author);

            if (author == test.author)
            {
                if (name != null)
                    test.name = name;
                if (desc != null)
                    test.descript = desc;

                db.SaveChanges();

                return "Zmodyfikowano Opis Testu";
            }
            return "Nie możesz edytowac tego testu";
        }

        [HttpGet]
        //[Authorize]
        public Models.TestSend Get(int id)
        {
            var test = db.tests.First(t => t.id == id);
            int questions = 0;
            int points = 0;

            foreach(Models.questions question in db.questions.Where(q => q.test == id))
            {
                int answers = question.answers_number;
                foreach (Models.answers answer in db.answers.Where(a => a.question == question.id))
                {
                    if (answer.correct == true)
                    {
                        points++;
                        answers--;
                        if (answers == 0)
                            break;
                    }
                }
                questions++;
            }

            return new Models.TestSend(id, test.name, test.descript, test.UserLogin.UserName, questions, points);
        }

        [HttpPost]
        [Authorize]
        public string Delete(int id)
        {
            Models.tests test = db.tests.First(t => t.id == id);

            int author;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out author);

            if (author == test.author)
            {
                db.tests.Remove(test);
                db.SaveChanges();

                return "Usunięto test nr " + id;
            }
            return "Nie możesz usunąć tego testu";
        }
    }
}
