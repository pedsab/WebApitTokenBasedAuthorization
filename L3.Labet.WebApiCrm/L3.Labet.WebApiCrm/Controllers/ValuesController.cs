using L3.Labet.WebApiCrm.Security;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace L3.Labet.WebApiCrm.Controllers
{
    [TokenAuthentication]
    public class ValuesController : ApiController
    {
        OrganizationServiceProxy _organizationService;

        public ValuesController()
        {
            var username = RequestContext.Principal.Identity.Name;
            _organizationService = WebApiApplication.Dynamics365AuthHelper.GetCachedService(username);
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
