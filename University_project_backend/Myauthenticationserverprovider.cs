using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using University_project_backend.Models;

namespace University_project_backend
{
    public class Myauthenticationserverprovider:OAuthAuthorizationServerProvider
    {
       
      public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (Contextclass contextclass = new Contextclass())
            {
                var userdetails = contextclass.user_Details.ToList();            
                    bool isvalid = contextclass.user_Details.Any(x => x.Email_address == context.UserName && x.Password == context.Password && x.Verified == "YES");
                    if(isvalid)
                    {
                        var id = new ClaimsIdentity("Embedded");
                        id.AddClaim(new Claim(ClaimTypes.Role, "user"));
                        id.AddClaim(new Claim("username", "user"));
                        id.AddClaim(new Claim(ClaimTypes.Name, "Preetham Holenarasipura"));
                        
                        var data = new Dictionary<string, string>
                        {
                            {"role","user" }
                        };
                        var properties = new AuthenticationProperties(data);
                        //ClaimsIdentity OAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
                        var ticket = new AuthenticationTicket(id, properties);
                        context.Validated(ticket);
                    }
                    else if(context.UserName=="admin" && context.Password=="admin")
                    {
                    var id = new ClaimsIdentity("Embedded");
                    id.AddClaim(new Claim(ClaimTypes.Role, "admin"));
                    id.AddClaim(new Claim("username", "admin"));
                    id.AddClaim(new Claim(ClaimTypes.Name, "Admin"));
                   
                        var data = new Dictionary<string, string>
                        {
                            {"role","user" }
                        };
                    var properties = new AuthenticationProperties(data);
                    var ticket = new AuthenticationTicket(id, properties);
                    context.Validated(ticket);

                    }
                    else
                    {
                        context.SetError("Invalid username or password");
                    } 
            }
           
        }
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("newtoken", "refreshtoken"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
    }
}