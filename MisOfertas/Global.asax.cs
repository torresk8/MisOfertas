using MisOfertas.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MisOfertas
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public void Aplicaction_PostAutenticateRequest(object sender, EventArgs e)
        {
            if(Request.IsAuthenticated)
            {
                var identity = new identityPersonalizado(HttpContext.Current.User.Identity);

                var principal = new PrincipalPersonalizado(identity);
                HttpContext.Current.User = principal;
            }
        }
    }
}
