using System.Web;
using System.Web.Mvc;

namespace L3.Labet.WebApiCrm
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
