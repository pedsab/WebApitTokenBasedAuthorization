using L3.Labet.WebApiCrm.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace L3.Labet.WebApiCrm
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API works only with XML
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = true;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            
            // Web API configuration and services
            log4net.Config.XmlConfigurator.Configure();
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
