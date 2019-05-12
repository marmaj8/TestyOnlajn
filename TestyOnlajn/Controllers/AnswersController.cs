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
    public class AnswersController : ApiController
    {
        Models.TestyOnlineEntities db = new Models.TestyOnlineEntities();

        [HttpPost]
        [Authorize]
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
                    return Content(HttpStatusCode.Unauthorized, "Nie możesz dodawać pytań do tego testu");
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

                if (answer == null)
                    return Content(HttpStatusCode.BadRequest, "Nie istnieje odpoweidź o nr " + id);

                if (answer.questions.tests.UserLogin.Id == user)
                {
                    db.answers.Remove(answer);
                    db.SaveChanges();
                    return Ok();
                }
                else
                    return Content(HttpStatusCode.Unauthorized, "Nie możesz usunąć tej odpowiedzi");
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}
