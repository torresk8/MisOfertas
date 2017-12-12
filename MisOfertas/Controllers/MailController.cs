using CapaDTO;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MisOfertas.Controllers
{
    public class MailController : Controller
    {
        // GET: Mail
        [Authorize(Roles ="encargadoTienda")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Mail mail)
        {

            NegocioOferta auxOferta = new NegocioOferta();
            List<Oferta> listOferta = new List<Oferta>();
            Rubro auxRubro;
            if (Convert.ToInt32(Session["idUsuario"]) != 0)
            {
                NegocioLog negocioLog = new NegocioLog();
                LogUsuario logUsuario = negocioLog.retornaLogUsuario(Convert.ToInt32(Session["idUsuario"]));

                auxRubro = auxOferta.retornaRubro(logUsuario.rubro.IdRubro);

                Session["idRubro"] = auxRubro.IdRubro;
                Session["nombreRubro"] = auxRubro.Nombre;


                listOferta = auxOferta.retornaOfertaPuublicadaList(logUsuario.rubro.IdRubro, 0);
            }

            if (ModelState.IsValid)
            {
                var envioCorreo = new MailAddress("ofertas.noreply@gmail.com", "Reply");


                var pass = "misofertas123";
                var asunto = mail.Asunto;
                var mensaje = mail.Mensaje;

                MailMessage msg = new MailMessage();
                msg.From = envioCorreo;
                
                


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

                    msg.To.Add(para);                    
                    msg.Subject = asunto;                    
                    msg.IsBodyHtml = true;
                    msg.Body += "<!DOCTYPE html> " +
                                "<html>" +
                                "<head>" +

"                            <title></title> " +
                            "</head>" +
                            "<body> ";
                    msg.Body += "<table align='center' border='1' cellpadding='0' cellspacing='0' width='600' style='border-collapse: collapse; border-color:#E6E6E6;'> " +
        "<tr> " +
            "<td align = 'center' bgcolor = '#70bbd9' style = 'padding: 40px 0 30px 0;' > " +

                     "<img src = 'https://about.canva.com/wp-content/uploads/sites/3/2017/02/birthday_banner.png' alt = 'Email' style = 'display: block;' /> " +

                      "</td> " +

                  "</tr> " +

                  "<tr> " +

                      "<td bgcolor = '#ffffff' style = 'padding: 40px 30px 40px 30px;'> " +

                             "<table border = '0' cellpadding = '0' cellspacing = '0' width = '100%'> " +

                                        "<tr> " +


                                            "<td style = 'color: #153643; font-family: Arial, sans-serif; font-size: 24px;'> " +

                                                 "<b> Las Mejores Ofertas Junto a ti </b> " +

                                                "</td> " +

                                            "</tr> " +

                                            "<tr style = 'color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;'> " +

                                                 "<td style = 'padding: 20px 0 30px 0;'> " +
                                                      "Las ofertas de HOY: " +
                        "</td> " +
                    "</tr> "+
                    "<tr style = 'color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;'> "+
 
                         "<td>" +
 
                             "<!--Mostrar ofertas--> "+
  
                              "<table border = '0' cellpadding = '0' cellspacing = '0' width = '100%'> "+
         
                                         "<tr> "+
         
                                             "<td width = '260' valign = 'top'> "+


                                                    "<table border = '0' cellpadding = '0' cellspacing = '0' width = '100%'> " +

                                                               "<tr> " +

                                                                  "<td> " +

                                                                       "<img src = 'imagen/silla.jpeg' alt = '' width = '100%' height = '140' style = 'display: block;'/> " +

                                                                            "</td> " +

                                                                        "</tr> " +

                                                                        "<tr> " +

                                "<div class='container'> " +
            "<div class='row'>";
                    foreach (var oferta in listOferta)
                    {
                        msg.Body += "<div class='col-lg-3 col-md-4 col-sm-6'> " +
                            "<div class='panel panel-default text-center'> " +
                                "<img class='card-img-top' src='data:image/png;base64," + Convert.ToBase64String(oferta.Imagen)+ "' style='width:70px; height:70px'>" +
                                "<h3 class='card-title'> " +
                                    "<a href = '#' ><h2>" + oferta.Nombre + "</h2></a>" +
                                "</h3>" +

                        "<h4 class='card-text'>Precio oferta</h4> " +
                        "<h5>$<label>" + oferta.PrecioOfeta + "</label></h5> " +
                        "<h4 class='card-text'>Precio normal</h4>" +
                        "<h5>$<label>" + oferta.PrecioNormal + "</label></h5> " +
                        "<h4 class='card-text'>Stock disponible</h4> " +                        
                        "<p class='card-text'>" + oferta.Descripcion + "</h2></p> " +
                        "<p>Sucursal: <a class='btn-link col-md-0' data-toggle='modal' data-target='#myModal'>" + oferta.sucursal.Nombre + "</a></p> " +

                        "</div> " +

                         " </div>";
                        
                }
        msg.Body += "</div> "+
    "</div> "+
    "</tr> "+
            
                                           
                                        "</table>"+
                                    "</td> "+
                                    "<td style = 'font-size: 0; line-height: 0;' width='20'> "+
                                        "&nbsp; "+
                                    "</td> "+
                                "</tr> "+
                            "</table> "+
                            "<!--Terminar Mostrar ofertas--> "+
                        "</td> "+
                    "</tr> "+
                "</table> "+
            "</td>"+
        "</tr>"+
        "<tr>"+
            "<td bgcolor = '#ee4c50' >"+
                "<table border='0' cellpadding='0' cellspacing='0' width='100%'>" +
                    "<tr> " +
                        "<td style = 'color: #ffffff; font-family: Arial, sans-serif; font-size: 14px;' >" +
                            "&reg; MisOfertas - JumMarc " + DateTime.Now.Year + "<br/>" +
                        "</td>" +
                    "</tr>" +
                "</table>" +
            "</td>" +
        "</tr>" +
    "</table>";
                    msg.Body += "</body> " +
                                "</html> ";

                   /* using (var message = new MailMessage(envioCorreo, para)
                    {
                        Subject = asunto,
                        Body = msg.Body
                    }
                )*/
                    {
                        smtp.Send(msg);
                    }
                    msg.To.Clear();
                }
                ModelState.Clear();
            }
            else
            {
                ModelState.AddModelError("", "Error No se pudieron enviar los correos");
            }

            return View();
        }

       

        public ActionResult MailBody()
        {
            NegocioOferta auxOferta = new NegocioOferta();
            List<Oferta> listOferta = new List<Oferta>();
            Rubro auxRubro;
            if (Convert.ToInt32(Session["idUsuario"]) != 0)
            {
                NegocioLog negocioLog = new NegocioLog();
                LogUsuario logUsuario = negocioLog.retornaLogUsuario(Convert.ToInt32(Session["idUsuario"]));

                auxRubro = auxOferta.retornaRubro(logUsuario.rubro.IdRubro);

                Session["idRubro"] = auxRubro.IdRubro;
                Session["nombreRubro"] = auxRubro.Nombre;


                listOferta = auxOferta.retornaOfertaPuublicadaList(logUsuario.rubro.IdRubro, 0);
            }
            return View(listOferta);
        }

        public ActionResult convertirImagen(string id)
        {
            NegocioOferta auxOferta = new NegocioOferta();
            var imagen = auxOferta.retornaOferta(Convert.ToInt32(id));
            return File(imagen.Imagen, "image/jpeg");
        }

        public static Image Convertir_Bytes_Imagen(byte[] bytes)
        {
            if (bytes == null) return null;

            MemoryStream ms = new MemoryStream(bytes);
            Bitmap bm = null;
            try
            {
                bm = new Bitmap(ms);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return bm;
        }
    }
}