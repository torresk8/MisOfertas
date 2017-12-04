using CapaConexion;
using CapaDTO;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CapaNegocio.Roles;

namespace MisOfertas.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Usuario usuario)
        {
            if (Membership.ValidateUser(usuario.NombreUsuario, usuario.Password))
            {
                FormsAuthentication.RedirectFromLoginPage(usuario.NombreUsuario, false);
                NegocioUsuario auxNegocio = new NegocioUsuario();
                Usuario u = auxNegocio.login(usuario);
                Session["idUsuario"] = u.IdUsuario;
                Session["usuario"] = u.Nombre;
                Session["nombreUuario"] = u.NombreUsuario;
                Session["nombre"] = u.Nombre;                                              
            }
            else
            {
                ModelState.AddModelError("", "Error datos invalidos");
            }
            return View(usuario);
        }

        public ActionResult LoginIn()
        {
            if (Session["idUsuario"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        public ActionResult eliminarUsuario()
        {
            return View();
        }

        [HttpPost]
        public ActionResult eliminarUsuario(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                NegocioUsuario auxUsuario = new NegocioUsuario();
                bool resultado = auxUsuario.eliminarUsuario(usuario);

                if (resultado == true)
                {                    
                    ModelState.AddModelError("", "Usuario eliminado" + usuario.Nombre);
                }
                else
                {
                    ModelState.AddModelError("", "Error datos invalidos" + usuario.Nombre + "" + usuario.Password);
                }
                // 
            }
            return View();
        }

        public ActionResult RegistrarUsuario()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarUsuario(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                bool resultado = false;
                NegocioUsuario auxUsuario = new NegocioUsuario();
                if(usuario.Password == usuario.ConfirmarPassword && usuario.Password!="" && usuario.ConfirmarPassword!="")
                {
                    resultado = auxUsuario.insertarUsuario(usuario);
                }
               

                if (resultado == true)
                {
                    ModelState.AddModelError("", "Datos Correctos");
                    Session["idUsuario"] = usuario.IdUsuario;
                    Session["usuario"] = usuario.Nombre;
                    Session["password"] = usuario.Password;

                    ModelState.Clear();
                }                 
            }
            else
            {
                usuario.NombreUsuario = "";
                ModelState.AddModelError("", "Error datos invalidos");
                
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Usuario usuario)
        {
            if(Membership.ValidateUser(usuario.NombreUsuario,usuario.Password))
            {
                FormsAuthentication.RedirectFromLoginPage(usuario.NombreUsuario, false);
                return null;
            }
            else
            {
                ModelState.AddModelError("", "Error datos invalidos");
            }
            /*
            if (ModelState.IsValid)
            {
                NegocioUsuario auxUsuario = new NegocioUsuario();
                auxUsuario.login(usuario);
                
                if (usuario !=null)
                {
                    Session["idUsuario"] = usuario.IdUsuario;
                    Session["usuario"] = usuario.Nombre;
                    Session["password"] = usuario.Password;
                    return RedirectToAction("LoginIn");
                }
                else
                {
                    ModelState.AddModelError("", "Error datos invalidos" + usuario.Nombre + "" + usuario.Password);
                }
                // 
            }*/

            return View(usuario);
        }

        public ActionResult cerrarSession()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }


        public ActionResult actualizarUsuario(int id)
        {

            NegocioUsuario auxNegocioUsuario = new NegocioUsuario();
            Usuario auxUSuario = new Usuario();
            Usuario usuario = auxNegocioUsuario.retornaUsuario(id);


            auxUSuario.IdUsuario = usuario.IdUsuario;
            auxUSuario.Nombre = usuario.Nombre;
            auxUSuario.NombreUsuario = usuario.NombreUsuario;
            auxUSuario.Rut = usuario.Rut;
            auxUSuario.Direccion = usuario.Direccion;
            auxUSuario.Telefono = usuario.Telefono;
            auxUSuario.Correo = usuario.Correo;
            return View(auxUSuario);
        }

        [HttpPost]
        public ActionResult actualizarUsuario(Usuario usuario)
        {

            NegocioUsuario auxNegocioUsuario = new NegocioUsuario();

            Usuario auxUSuario = new Usuario();


            bool resultado = auxNegocioUsuario.actualizarUsuario(usuario);



            auxUSuario.IdUsuario = usuario.IdUsuario;
            auxUSuario.Nombre = usuario.Nombre;
            auxUSuario.NombreUsuario = usuario.NombreUsuario;
            auxUSuario.Rut = usuario.Rut;
            auxUSuario.Direccion = usuario.Direccion;
            auxUSuario.Telefono = usuario.Telefono;
            auxUSuario.Correo = usuario.Correo;

            if (resultado == true)
            {
                ModelState.AddModelError("", "Datos Correctos");
            }
            else
            {
                ModelState.AddModelError("", "Error datos invalidos");
            }

            return View(auxUSuario);
        }


    }
}