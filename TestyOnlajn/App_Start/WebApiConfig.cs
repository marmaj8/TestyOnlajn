using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace TestyOnlajn
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Konfiguracja i usługi składnika Web API

            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Trasy składnika Web API
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "GetTest",
                routeTemplate: "api/Get/{id}/",
                defaults: new { controller = "Tests", action = "Get" }
            );
            config.Routes.MapHttpRoute(
                name: "ListTests",
                routeTemplate: "api/Get",
                defaults: new { controller = "Tests", action = "List" }
            );
            config.Routes.MapHttpRoute(
                name: "CreateTest",
                routeTemplate: "api/Create",
                defaults: new { controller = "Tests", action = "Create" }
            );
            config.Routes.MapHttpRoute(
                name: "UpdateTestDesc",
                routeTemplate: "api/Update",
                defaults: new { controller = "Tests", action = "UpdateDesc" }
            );
            config.Routes.MapHttpRoute(
                name: "UpdateQuestions",
                routeTemplate: "api/UpdateQuestions",
                defaults: new { controller = "Tests", action = "UpdateQuestions" }
            );
            config.Routes.MapHttpRoute(
                name: "DeleteTest",
                routeTemplate: "api/Delete/{id}",
                defaults: new { controller = "Tests", action = "Delete" }
            );
            config.Routes.MapHttpRoute(
                name: "RateTest",
                routeTemplate: "api/Rate",
                defaults: new { controller = "Questions", action = "Rate" }
            );
            config.Routes.MapHttpRoute(
                name: "Examin",
                routeTemplate: "api/Examin/{id}",
                defaults: new { controller = "Questions", action = "ListToDo" }
            );
            config.Routes.MapHttpRoute(
                name: "ListQuestions",
                routeTemplate: "api/Questions/{id}",
                defaults: new { controller = "Questions", action = "List" }
            );
            config.Routes.MapHttpRoute(
                name: "Results",
                routeTemplate: "api/Results/{id}",
                defaults: new { controller = "Results", action = "List" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


        }
    }
}
