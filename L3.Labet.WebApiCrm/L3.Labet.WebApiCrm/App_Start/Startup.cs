using System;
using System.Threading.Tasks;
using System.Web.Http;
using L3.Labet.WebApiCrm.Security;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(L3.Labet.WebApiCrm.App_Start.Startup))]

namespace L3.Labet.WebApiCrm.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/oauth2/token"),
                AccessTokenExpireTimeSpan = new TimeSpan(0, 30, 0),
                Provider = new AuthorizationServerProvider(),
                RefreshTokenProvider = new RefreshTokenProvider()
            };

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
        }
    }
}
