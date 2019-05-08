using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace TestyOnlajn.Provider
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated(); //   
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (var db = new Models.TestyOnlineEntities())
            {
                if (db != null)
                {
                    var users = db.UserLogin.ToList();
                    if (users != null)
                    {
                        var user = users.Find(u => u.UserEmail == context.UserName && u.UserPassword == context.Password);
                        //if (!string.IsNullOrEmpty(users.Where(u => u.UserEmail == context.UserName && u.UserPassword == context.Password).FirstOrDefault().UserName))
                        if (user != null)
                        {
                            //identity.AddClaim(new Claim("Age", "16"));
                            identity.AddClaim(new Claim("Id", user.Id.ToString()));

                            var props = new AuthenticationProperties(new Dictionary<string, string>
                            {
                                {
                                    "userdisplayname", user.UserName
                                },
                                {
                                     "role", user.UserRole
                                }
                             });

                            var ticket = new AuthenticationTicket(identity, props);
                            context.Validated(ticket);
                        }
                        else
                        {
                            context.SetError("invalid_grant", "Provided email and password is incorrect");
                            context.Rejected();
                        }
                    }
                }
                else
                {
                    context.SetError("invalid_grant", "Provided email and password is incorrect");
                    context.Rejected();
                }
                return;
            }
        }
    }
}