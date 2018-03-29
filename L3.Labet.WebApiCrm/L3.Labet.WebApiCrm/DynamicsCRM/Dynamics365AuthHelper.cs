using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace L3.Labet.WebApiCrm.Security
{
    public class Dynamics365AuthHelper
    {
        private string _connectionString;
        private string _dynamics365Url;
        private string _username;
        private string _password;

        private static ConcurrentDictionary<string, OrganizationServiceProxy> _cachedServices = new ConcurrentDictionary<string, OrganizationServiceProxy>();

        private OrganizationServiceProxy _organizationService;
        public OrganizationServiceProxy OrganizationService
        {
            get
            {
                if (_organizationService != null && _organizationService.IsAuthenticated)
                    return _organizationService;

                _organizationService = GetOrganizationService();

                return _organizationService;
            }
            set
            {
                _organizationService = value;
            }
        }

        public Dynamics365AuthHelper()
        {
            ReadConfigurationFile();

            OrganizationService = GetOrganizationService();
        }

        public bool ValidateUserAndCacheService(string username, string password)
        {
            var builder = new System.Data.Common.DbConnectionStringBuilder();
            builder.ConnectionString = _connectionString;

            builder.Add("RequireNewInstance", true);
            builder["Username"] = username;
            builder["Password"] = password;

            var conn = new CrmServiceClient(builder.ConnectionString);
            conn.OrganizationServiceProxy.Timeout = new TimeSpan(24, 0, 0);

            if (conn.IsReady)
            {
                _cachedServices[username] = conn.OrganizationServiceProxy;
            }

            return conn.IsReady;
        }

        public OrganizationServiceProxy GetCachedService(string username)
        {
            if (_cachedServices.ContainsKey(username))
                return _cachedServices[username];

            return null;
        }

        private OrganizationServiceProxy GetOrganizationService()
        {
            var conn = new CrmServiceClient(_connectionString);

            var organizationServiceProxy = conn.OrganizationServiceProxy;

            if (organizationServiceProxy == null)
                throw new Exception($"{conn.LastCrmError}\n\n{conn.LastCrmException}\n\n{_dynamics365Url}");

            organizationServiceProxy.Timeout = new TimeSpan(24, 0, 0);

            return organizationServiceProxy;
        }

        private Guid GetSystemUserIdByName(string username)
        {
            try
            {
                var qe = new QueryExpression("systemuser");
                qe.ColumnSet.AllColumns = false;
                qe.Criteria.AddCondition("domainname", ConditionOperator.Equal, username);

                var ec = _organizationService.RetrieveMultiple(qe).Entities;

                if (ec.Count > 0)
                    return ec.First().Id;

                return Guid.Empty;
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }

        private void ReadConfigurationFile()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["Dynamics365ConnectionString"]?.ConnectionString;

            var connStringBuilder = new System.Data.Common.DbConnectionStringBuilder();
            connStringBuilder.ConnectionString = _connectionString;

            _dynamics365Url = connStringBuilder.ContainsKey("Url") ? connStringBuilder["Url"].ToString() : string.Empty;
            _username = connStringBuilder.ContainsKey("Username") ? connStringBuilder["Username"].ToString() : string.Empty;
            _password = connStringBuilder.ContainsKey("Password") ? connStringBuilder["Password"].ToString() : string.Empty;
        }
    }
}