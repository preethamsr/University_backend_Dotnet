using Microsoft.Owin;
using Owin;
using System;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http;
using System.Web.Http.Cors;

[assembly: OwinStartup(typeof(University_project_backend.Startup1))]

namespace University_project_backend
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            HttpConfiguration config = new HttpConfiguration();
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            var provider = new Myauthenticationserverprovider();
            OAuthAuthorizationServerOptions OAuthOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(30),
                Provider = provider,
                RefreshTokenProvider = new OAuthCustomRefreshTokenProvider()

            };
            app.UseOAuthAuthorizationServer(OAuthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            
            config.EnableCors(cors);
            WebApiConfig.Register(config);
        }
    }
}
