using CapaDTO;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace MisOfertas.Controllers
{
    public class MailController : Controller
    {
        // GET: Mail
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Mail mail)
        {

            if (ModelState.IsValid)
            {
                var envioCorreo = new MailAddress("ofertas.noreply@gmail.com", "Reply");


                var pass = "misofertas123";
                var asunto = mail.Asunto;
                var mensaje = mail.Mensaje;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(envioCorreo.Address, pass)
                };



                NegocioUsuario negocio = new NegocioUsuario();
                NegocioEmail negocioEmail = new NegocioEmail();

                Correo correo = new Correo();

                correo.Asunto = asunto;
                correo.Descripcion = mensaje;

                List<Usuario> list = negocio.retornaUsuarioList();

                foreach (var item in list)
                {
                    var para = new MailAddress(item.NombreUsuario, "Recibir");

                    correo.Usuario.IdUsuario = item.IdUsuario;
                    negocioEmail.insertarCorreo(correo);

                    using (var message = new MailMessage(envioCorreo, para)
                    {
                        Subject = asunto,
                        Body = mensaje
                    }
                )
                    {
                        smtp.Send(message);
                    }
                }
                ModelState.Clear();
            }
            else
            {
                ModelState.AddModelError("", "Error No se pudieron enviar los correos");
            }

            return View();
        }

        public ActionResult reporte()
        {
            return View();
        }
        [HttpPost]
        public ActionResult reporte(string a)
        {
            ServiceReporte.WebServiceReporteSoapClient reporte = new ServiceReporte.WebServiceReporteSoapClient();
            reporte.generarArchivo();
            return View();
        }
    }
}