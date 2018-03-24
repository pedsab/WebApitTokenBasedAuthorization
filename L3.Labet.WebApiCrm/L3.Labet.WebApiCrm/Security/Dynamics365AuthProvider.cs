using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L3.Labet.WebApiCrm.Security
{
    public class Dynamics365AuthProvider
    {
        public bool ValidateUser(string username, string password)
        {
            if (username == "username" && password == "password")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetUserRole(string username)
        {
            return string.Empty;
        }
    }
}