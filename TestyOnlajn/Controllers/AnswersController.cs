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
        public int Create(int question, string txt, Boolean correct = false)
        {
            User = System.Web.HttpContext.Current.User;
            int user;
            int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);
            if (db.questions.First(q => q.id == question).tests.UserLogin.Id == user)
            {
                Models.answers answer = new Models.answers();
                answer.question = question;
                answer.answer = txt;
                answer.correct = correct;
                db.answers.Add(answer);
                db.SaveChanges();
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

            Models.answers answer = db.answers.First(q => q.id == id);
            if (answer.questions.tests.UserLogin.Id == user)
            {
                db.answers.Remove(answer);
                db.SaveChanges();

                return "Usunięto odpowiedź nr " + id;
            }
            return "Nie możesz usunąć tej odpowiedzi";
        }
    }
}
