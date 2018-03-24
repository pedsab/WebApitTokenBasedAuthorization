using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace L3.Labet.WebApiCrm.Security
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        private static ConcurrentDictionary<string, AuthenticationTicket> _tickets = new ConcurrentDictionary<string, AuthenticationTicket>();

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var refreshTokenKey = Guid.NewGuid().ToString();

            var tokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary)
            {
                IssuedUtc = context.Ticket.Properties.IssuedUtc,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30) // expira em 30 minutos
            };

            var ticket = new AuthenticationTicket(context.Ticket.Identity, tokenProperties);

            _tickets.TryAdd(refreshTokenKey, ticket);

            context.SetToken(refreshTokenKey);

            await Task.FromResult<object>(null);
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            if (_tickets.TryRemove(context.Token, out AuthenticationTicket ticket))
            {
                context.SetTicket(ticket);
            }

            await Task.FromResult<object>(null);
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }
        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
    }
}