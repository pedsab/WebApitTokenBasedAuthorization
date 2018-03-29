using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace L3.Labet.WebApiCrm.Security
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();

            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            if(WebApiApplication.Dynamics365AuthHelper.ValidateUserAndCacheService(context.UserName, context.Password))
            {
                identity.AddClaim(new Claim("username", context.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, ""));

                context.Validated(identity);
            }
            else
            {
                context.Rejected();

                context.SetError("invalid_grant", "The username or password is incorrect");
            }

            return Task.FromResult<object>(null);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var identity = new ClaimsIdentity(context.Ticket.Identity);

            var ticket = new AuthenticationTicket(identity, context.Ticket.Properties);

            context.Validated(ticket);

            return Task.FromResult<object>(null);
        }
    }
}