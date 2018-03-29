using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L3.Labet.WebApiCrm.DynamicsCRM
{
    public class Dynamics365Helper
    {
        private OrganizationServiceProxy _organizationService;

        public Dynamics365Helper(OrganizationServiceProxy organizationService)
        {
            _organizationService = organizationService;
        }
    }
}