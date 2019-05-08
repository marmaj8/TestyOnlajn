using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TestyOnlajn.Controllers
{
    public class TestsController : ApiController
    {
        Models.TestyOnlineEntities db = new Models.TestyOnlineEntities();

        [HttpGet]
        [Authorize]
        public List<Models.TestSend> GetTests()
        {
            List<Models.TestSend> tests = new List<Models.TestSend>();

            foreach (Models.tests test in db.tests)
            {
                tests.Add(new Models.TestSend(test.name, test.descript, test.UserLogin.UserName));
            }

            return tests;
        }
    }
}
