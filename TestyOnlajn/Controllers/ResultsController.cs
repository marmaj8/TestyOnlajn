using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;
using System.Web.Http.Cors;

namespace TestyOnlajn.Controllers
{
    [System.Web.Mvc.RequireHttps]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ResultsController : ApiController
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

                if (test == null)
                    return Content(HttpStatusCode.BadRequest, "Nie istnieje test o nr " + id);

                if (test.UserLogin.Id == user)
                {
                    List<Models.ResultsSend> results = new List<Models.ResultsSend>();

                    foreach(Models.results r in test.results)
                    {
                        Models.ResultsSend result = new Models.ResultsSend();
                        result.Examinee = r.UserLogin.UserName;
                        result.Result = r.result;

                        results.Add(result);
                    }

                    return Ok(results);
                }
                else
                    return Content(HttpStatusCode.Unauthorized, "Nie możesz edytować tego testu");
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}
