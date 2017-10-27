using MisOfertas.MyRole;
using MisOfertas.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MisOfertas.Controllers
{
   [Authorize]
    public class HomeController : Controller
    {        
        
        public ActionResult Index()
        {
            
           /* var rol = new SiteRole();

            var roles = rol.IsUserInRole("43", "4");
            if(roles == true)
            {
                ViewBag.Message = "Tiene permiso";
                
            }
            else
            {
                ViewBag.Message = "No tiene permiso";
            }
            return null;*/

            return View();

        }
        [Authorize(Roles = "2")]
        public ActionResult About()
        {
            

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}