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
                User = System.Web.HttpContext.Current.User;
                int user;
                int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);
                var test = db.tests.First(t => t.id == id);

                if (test.author != user)
                {
                    return Unauthorized();
                }

                List<Models.QuestionSend> questionsSend = new List<QuestionSend>();

                foreach (Models.questions question in test.questions)
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
        public IHttpActionResult ListToDo(int id)
        {
            try
            {
                var test = db.tests.First(t => t.id == id);

                List<Models.QuestionSend> questionsSend = new List<QuestionSend>();

                foreach (Models.questions question in test.questions)
                {
                    List<Models.AnswerSend> answers = new List<AnswerSend>();
                    foreach (Models.answers answer in db.answers.Where(a => a.question == question.id))
                    {
                        answers.Add(new Models.AnswerSend(answer.id, answer.answer, false));
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
        public IHttpActionResult Rate(Models.QuestionsToCheck data)
        {
            try
            {
                User = System.Web.HttpContext.Current.User;
                int user;
                int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

                var results = db.results.Where(r => r.test == data.Id && r.examinee == user);
                if (results.Count() > 0)
                {
                    return Unauthorized();
                }

                int points = 0;

                var test = db.tests.First(t => t.id == data.Id);
                var questions = test.questions;

                int i = 0;
                foreach(Models.QuestionToCheck question in data.Questions)
                {
                    if (questions.ElementAt(i).answers_number == question.Answers.Count())
                    {
                        foreach(int answer in question.Answers)
                        {
                            if (questions.ElementAt(i).answers.ElementAt(answer).correct)
                                points++;
                        }
                    }
                    i++;
                }

                Models.results result = new Models.results();
                result.examinee = user;
                result.result = points;
                result.tests = test;

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
