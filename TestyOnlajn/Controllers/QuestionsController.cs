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
        
        [HttpGet]
        [Authorize]
        public IHttpActionResult List(int id)
        {
            try
            {
                List<Models.QuestionSend> questionsSend = new List<QuestionSend>();

                foreach (Models.questions question in db.questions.Where(q => q.test == id))
                {
                    List<Models.AnswerSend> answers = new List<AnswerSend>();
                    foreach (Models.answers answer in db.answers.Where(a => a.question == question.id))
                    {
                        answers.Add(new Models.AnswerSend(answer.id, answer.answer, answer.correct));
                    }
                    questionsSend.Add(new Models.QuestionSend(question.id, question.question, question.answers_number, answers));
                }

                return Ok(questionsSend);
            }
            catch
            {
                return InternalServerError();
            }
        }

        [HttpGet]
        [Authorize]
        public IHttpActionResult Get(int test, int number)
        {
            try
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

                return Ok(new QuestionSend(question.id, question.question, question.answers_number, answers));
            }
            catch
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult Rate(int id, string answers)
        {
            try
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

                return Ok(points);
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}
