using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Extensions.DependencyInjection;

[assembly: OwinStartup(typeof(University_project_backend.Startup1))]

namespace University_project_backend
{
    public class Startup1
    {
     public void ConfigurationService(ServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.)
        }
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            HttpConfiguration config = new HttpConfiguration();
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            var provider = new Myauthenticationserverprovider();
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider=provider
            };
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            
            config.EnableCors(cors);
            WebApiConfig.Register(config);
        }
    }
}
