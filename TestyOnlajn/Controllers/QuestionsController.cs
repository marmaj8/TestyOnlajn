using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Cors;

namespace TestyOnlajn.Models
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class QuestionsController : ApiController
    {
        Models.TestyOnlineEntities db = new Models.TestyOnlineEntities();

        [HttpPost]
        [Authorize]
        public int Create(int test, string txt, int answers)
        {
            User = System.Web.HttpContext.Current.User;
            int user;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);
            if ( db.tests.First(t => t.id == test).UserLogin.Id == user)
            {
                Models.questions question = new Models.questions();
                question.test = test;
                question.question = txt;
                question.answers_number = answers;

                db.questions.Add(question);
                db.SaveChanges();

                return question.id;
            }
            return -1;

        }

        [HttpPost]
        [Authorize]
        public string Delete(int id)
        {
            User = System.Web.HttpContext.Current.User;
            int user;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

            Models.questions question = db.questions.First(q => q.id == id);
            if (question.tests.UserLogin.Id == user)
            {
                db.questions.Remove(question);
                db.SaveChanges();

                return "Usunięto pytanie nr " + id;
            }
            return "Nie możesz usunąć tego pytania";
        }

        [HttpPost]
        [Authorize]
        public string Update(int id, string txt, int answers)
        {
            User = System.Web.HttpContext.Current.User;
            int user;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

            Models.questions question = db.questions.First(q => q.id == id);
            if (question.tests.UserLogin.Id == user)
            {
                if (txt != null)
                    question.question = txt;
                if (answers != null)
                    question.answers_number = answers;
                db.SaveChanges();

                return "Edytowano pytanie nr " + id;
            }
            return "Nie możesz edytować tego pytania";
        }

        [HttpGet]
        [Authorize]
        public List<Models.QuestionSend> List(int test)
        {
            List<Models.QuestionSend> questionsSend = new List<QuestionSend>();

            foreach (Models.questions question in db.questions.Where(q => q.test == test))
            {
                List<Models.AnswerSend> answers = new List<AnswerSend>();
                foreach (Models.answers answer in db.answers.Where(a => a.question == question.id))
                {
                    answers.Add(new Models.AnswerSend(answer.id, answer.answer, answer.correct));
                }
                questionsSend.Add(new Models.QuestionSend(question.id, question.question, question.answers_number, answers));
            }

            return questionsSend;
        }

        [HttpGet]
        [Authorize]
        public QuestionSend Get(int test, int number)
        {
            var questions = db.questions.Where(q => q.test == test);
            if (number >= questions.Count() || number <= 0)
            {
                throw new Exception("Nieprawidlowy numer pytania");
            }
            var question = questions.ElementAt(number);

            List<Models.AnswerSend> answers = new List<AnswerSend>();
            foreach (Models.answers answer in db.answers.Where(a => a.question == question.id))
            {
                answers.Add(new Models.AnswerSend(answer.id, answer.answer, answer.correct));
            }

            return new QuestionSend(question.id, question.question, question.answers_number, answers);
        }

        [HttpPost]
        [Authorize]
        public int Rate(int id, string answers)
        {
            User = System.Web.HttpContext.Current.User;
            int user;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);


            int points = 0;

            var questions = db.tests.First(t => t.id == id).questions;

            answers = answers.ToLower();
            string[] answers_list = answers.Split(':');

            int i = 0;
            foreach (string ans in answers_list)
            {
                foreach(char c in ans)
                {
                    if (questions.ElementAt(i).answers.ElementAt(c - 97).correct)
                        points++;
                }
            }

            Models.results result = new Models.results();
            result.result = points;
            result.test = id;
            result.examinee = user;

            db.results.Add(result);
            db.SaveChanges();

            return points;
        }
    }
}
