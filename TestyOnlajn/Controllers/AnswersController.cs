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
    public class AnswersController : ApiController
    {
        Models.TestyOnlineEntities db = new Models.TestyOnlineEntities();

        [HttpPost]
        [Authorize]
        //public int Create(int question, string txt, Boolean correct = false)
        public IHttpActionResult Create(Models.AnswerSend data)
        {
            try
            {
                User = System.Web.HttpContext.Current.User;
                int user;
                int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);
                if (db.questions.First(q => q.id == data.Id).tests.UserLogin.Id == user)
                {
                    Models.answers answer = new Models.answers();
                    answer.question = data.Id;
                    answer.answer = data.Text;
                    answer.correct = data.Correct;
                    db.answers.Add(answer);
                    db.SaveChanges();
                    return Ok();
                }
                else
                    return Unauthorized();
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
                User = System.Web.HttpContext.Current.User;
                int user;
                int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

                Models.answers answer = db.answers.First(q => q.id == id);
                if (answer.questions.tests.UserLogin.Id == user)
                {
                    db.answers.Remove(answer);
                    db.SaveChanges();
                    return Ok();
                }
                else
                    return Unauthorized();
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}
