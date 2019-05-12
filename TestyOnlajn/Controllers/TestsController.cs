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
    [System.Web.Mvc.RequireHttps]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TestsController : ApiController
    {
        Models.TestyOnlineEntities db = new Models.TestyOnlineEntities();

        [HttpGet]
        [Authorize]
        public IHttpActionResult List()
        {
            try
            {
                User = System.Web.HttpContext.Current.User;
                int user;
                int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);
                Boolean isY;

                List<Models.TestSend> tests = new List<Models.TestSend>();

                foreach (Models.tests test in db.tests)
                {
                    if (user == test.author)
                        isY = true;
                    else
                        isY = false;
                    int points = 0;
                    int result = -1;
                    foreach (Models.questions q in test.questions)
                    {
                        points += q.answers_number;
                    }

                    var results = test.results.Where(r => r.examinee == user);
                    if (results.Count() > 0)
                    {
                        result = results.First().result;
                    }

                    tests.Add(new Models.TestSend(test.id, test.name, test.descript, test.UserLogin.UserName, test.questions.Count(), points, result, isY));
                }

                return Ok(tests);
            }
            catch
            {
                return InternalServerError();
            }
        }

        [HttpPut]
        [Authorize]
        public IHttpActionResult Create(Models.TestCreate data)
        {
            try
            {
                User = System.Web.HttpContext.Current.User;
                Models.tests test = new Models.tests();

                try
                {
                    test.name = data.Name;
                    test.descript = data.Desc;
                }
                catch
                {
                    return Content(HttpStatusCode.BadRequest, "Przesłano wadliwe dane");
                }

                int author;
                int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out author);
                test.author = author;
                
                db.tests.Add(test);
                db.SaveChanges();

                return Ok(test.id);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                return Content(HttpStatusCode.BadRequest, "Wprowadzone dane nie mogą zostać zapisane");
            }
            catch
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateDesc(Models.TestUpdateDesc data)
        {
            try
            {
                Models.tests test = db.tests.First(t => t.id == data.Id);

                if (test == null)
                    return Content(HttpStatusCode.BadRequest, "Nie istnieje test o nr " + data.Id);

                int author;
                int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out author);

                
                if (author == test.author)
                {
                    if (data.Name != null)
                        test.name = data.Name;
                    if (data.Desc != null)
                        test.descript = data.Desc;

                    db.SaveChanges();
                    return Ok();
                }
                else
                    return Content(HttpStatusCode.Unauthorized, "Nie możesz edytować tego testu");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                return Content(HttpStatusCode.BadRequest, "Wprowadzone dane nie mogą zostać zapisane");
            }
            catch
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateQuestions(Models.TestQuestionUpdate data)
        {
            try
            {
                Models.tests test = db.tests.First(t => t.id == data.TestId);

                if (test == null)
                    return Content(HttpStatusCode.BadRequest, "Nie istnieje test o nr " + data.TestId);

                int author;
                int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out author);

                if (author == test.author)
                {
                    List<Models.questions> qDelete;
                    List<Models.answers> aDelete = new List<Models.answers>();

                    qDelete = test.questions.ToList();
                    foreach (Models.questions q in qDelete)
                    {
                        foreach (Models.answers a in q.answers)
                        {
                            aDelete.Add(a);
                        }
                    }
                    foreach (Models.answers a in aDelete)
                    {
                        db.answers.Remove(a);
                    }
                    foreach (Models.questions q in qDelete)
                    {
                        db.questions.Remove(q);
                    }



                    foreach (Models.QuestionUpdate q in data.Questions.OrderBy(q => q.QuestionId))
                    {
                        Models.questions question = new Models.questions();
                        question.answers_number = q.CorrectAnswers;
                        question.question = q.Text;
                        question.answers = new List<Models.answers>();

                        foreach (Models.AnswerSend a in q.Answers.OrderBy(a => a.Id))
                        {
                            Models.answers answer = new Models.answers();
                            answer.answer = a.Text;
                            answer.correct = a.Correct;

                            question.answers.Add(answer);
                        }
                        test.questions.Add(question);
                    }

                    db.SaveChanges();
                    return Ok();
                }
                else
                    return Content(HttpStatusCode.Unauthorized, "Nie możesz edytować tego testu");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                return Content(HttpStatusCode.BadRequest, "Wprowadzone dane są niepoprawne");
            }
            catch
            {
                return InternalServerError();
            }
        }

        [HttpGet]
        [Authorize]
        public IHttpActionResult Get(int id)
        {
            try
            {
                int user;
                int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

                var test = db.tests.First(t => t.id == id);

                if (test == null)
                    return Content(HttpStatusCode.BadRequest, "Nie istnieje test o nr "+id);

                int questions = 0;
                int points = 0;

                Boolean isY;
                if (user == test.author)
                    isY = true;
                else
                    isY = false;
                int result = -1;
                foreach (Models.questions q in test.questions)
                {
                    points += q.answers_number;
                }

                var results = test.results.Where(r => r.examinee == user);
                if (results.Count() > 0)
                {
                    result = results.First().result;
                }

                return Ok(new Models.TestSend(id, test.name, test.descript, test.UserLogin.UserName, questions, points, result, isY));
            }
            catch
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                Models.tests test = db.tests.First(t => t.id == id);
                
                if (test == null)
                    return Content(HttpStatusCode.BadRequest, "Nie istnieje test o nr " + id);

                int author;
                int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out author);

                if (author == test.author)
                {
                    var results = test.results;
                    foreach (Models.results result in results)
                    {
                        db.results.Remove(result);
                    }

                    List<Models.questions> qDelete = new List<Models.questions>();
                    List<Models.answers> aDelete = new List<Models.answers>();

                    foreach (Models.questions question in test.questions)
                    {
                        foreach (Models.answers answer in question.answers)
                        {
                            aDelete.Add(answer);
                        }
                        qDelete.Add(question);
                    }

                    foreach (Models.answers answer in aDelete)
                    {
                        db.answers.Remove(answer);
                    }
                    foreach (Models.questions question in qDelete)
                    {
                        db.questions.Remove(question);
                    }
                    db.tests.Remove(test);
                    db.SaveChanges();
                    return Ok();
                }
                else
                    return Content(HttpStatusCode.Unauthorized, "Nie możesz usuwać tego testu");
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}
