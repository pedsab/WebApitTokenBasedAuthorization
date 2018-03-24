using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace L3.Labet.WebApiCrm.Handlers
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var result = new HttpActionResult();
            result.Request = context.Request;

            if (context.Exception is UnauthorizedAccessException)
            {
                context.Result = GetExceptionResult(context, HttpStatusCode.Unauthorized);
            }
            else
            {
                context.Result = GetExceptionResult(context, HttpStatusCode.BadRequest);
            }
        }

        private IHttpActionResult GetExceptionResult(ExceptionHandlerContext context, HttpStatusCode statusCode)
        {
            var result = new HttpActionResult();
            result.Request = context.Request;

            var returnJsonString = JsonConvert.SerializeObject(new
            {
                error = statusCode.ToString().ToLower(),
                error_description = context.Exception.Message
            });

            result.Response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(returnJsonString),
                ReasonPhrase = statusCode.ToString()
            };

            result.Response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return result;
        }
    }

    public class HttpActionResult : IHttpActionResult
    {
        public HttpRequestMessage Request { get; set; }
        public HttpResponseMessage Response { get; set; }

        public HttpActionResult()
        {
        }

        public HttpActionResult(HttpRequestMessage request, HttpResponseMessage response)
        {
            Request = request;
            Response = response;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Response);
        }
    }
}