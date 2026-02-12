using System.Web;
using System.Web.Mvc;

namespace Initilal_YV_Assesment2
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
