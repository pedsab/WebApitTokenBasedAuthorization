﻿using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using static L3.Labet.WebApiCrm.Handlers.MessageHandler;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace L3.Labet.WebApiCrm
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static Security.Dynamics365AuthHelper Dynamics365AuthHelper = new Security.Dynamics365AuthHelper();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configuration.MessageHandlers.Add(new MessageLoggingHandler());
        }
    }
}
