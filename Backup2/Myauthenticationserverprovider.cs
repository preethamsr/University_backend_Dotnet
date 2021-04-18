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
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            using (Contextclass contextclass = new Contextclass())
            {
               
                var user_details = contextclass.user_Details.ToList();
                if(user_details!=null)
                {
                    var temp = context;
                    bool isvalid = contextclass.user_Details.Any(x => x.Email_address == context.UserName && x.Password == context.Password && x.Verified=="YES");
                    if (isvalid)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role,"user"));
                        identity.AddClaim(new Claim("username", "user"));
                        identity.AddClaim(new Claim(ClaimTypes.Name, "Preetham Holenarasipuradeveraju"));
                        context.Validated(identity);
                    }
                  //  bool adminnvalid = contextclass.user_Details.Any(x => x.Email_address == context.UserName && x.Password == context.Password && x.Verified == "YES");
                    //else if(adminnvalid)
                    else if(context.UserName=="admin" && context.Password=="admin")
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                        identity.AddClaim(new Claim("username", "admin"));
                        identity.AddClaim(new Claim(ClaimTypes.Name, "Admin"));
                        context.Validated(identity);
                    }
                    else
                    {
                        context.SetError("Invalid grant username or password is wrong");
                    }
                }
            }
                      
            
        }
        public override async Task  GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            var temp= context.Request.Headers.GetValues("authroization");
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            
                if (context.ClientId == "12345")
                {
                    context.Validated(identity);
                }
            
        }
    }
}