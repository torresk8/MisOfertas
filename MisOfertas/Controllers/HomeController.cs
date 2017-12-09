using CapaDTO;
using CapaNegocio;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MisOfertas.MyRole;
using MisOfertas.Seguridad;
using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MisOfertas.Controllers
{
   //[Authorize]
    public class HomeController : Controller
    {        
        
        public ActionResult Index()
        {

            NegocioOferta auxOferta = new NegocioOferta();
            List<Oferta> listOferta;
            Rubro auxRubro;
            if (Convert.ToInt32(Session["idUsuario"]) !=0)
            {
                NegocioLog negocioLog = new NegocioLog();
                LogUsuario logUsuario = negocioLog.retornaLogUsuario(Convert.ToInt32(Session["idUsuario"]));

                auxRubro = auxOferta.retornaRubro(logUsuario.rubro.IdRubro);

                Session["idRubro"] = auxRubro.IdRubro;
                Session["nombreRubro"] = auxRubro.Nombre;


                listOferta = auxOferta.retornaOfertaPuublicadaList(logUsuario.rubro.IdRubro, 0);
            }
            else
            {
                
                auxRubro = auxOferta.retornaRubro(1);

                Session["idRubro"] = auxRubro.IdRubro;
                Session["nombreRubro"] = auxRubro.Nombre;


                listOferta = auxOferta.retornaOfertaPuublicadaList(1, 0);
            }


            ViewBag.listarRubro = obtenerRubro(Convert.ToInt32(Session["idRubro"]));
            ViewBag.listaRangoPrecio = obtenerRangoPrecio();
            ViewBag.listaSucursal = obtenerSucursal();


            return View(listOferta);

        }    
        
        [HttpPost]
        public ActionResult Index(string idRubro, string precio,string sucursal)
        {

            NegocioOferta auxOferta = new NegocioOferta();
            Rubro auxRubro = auxOferta.retornaRubro(Convert.ToInt32(idRubro));

            Session["idRubro"] = auxRubro.IdRubro;
            Session["nombreRubro"] = auxRubro.Nombre;

            List<Oferta> listOferta;

            int precioInicio = 0;
            int precioFin = 0;
            bool menorPrecio = false;
            bool mayorPrecio = false;
            string order = "";
            

            if (Convert.ToInt32(precio) > 0)
            {

                switch (Convert.ToInt32(precio))
                {
                    case 1:

                        menorPrecio = true;
                        order = "ASC";
                        break;
                    case 2:

                        mayorPrecio = true;
                        order = "DESC";
                        break;
                    case 3:
                        precioInicio = 0;
                        precioFin = 10000;
                        break;
                    case 4:
                        precioInicio = 10000;
                        precioFin = 50000;
                        break;
                    case 5:
                        precioInicio = 50000;
                        precioFin = 100000;
                        break;
                    case 6:
                        precioInicio = 100000;
                        precioFin = 500000;
                        break;                    
                }

                if (menorPrecio == true)
                {
                    listOferta = auxOferta.retornaOfertaPublicadaListPrecioMenor(Convert.ToInt32(precioInicio), Convert.ToInt32(precioFin),order);
                }
                else
                {
                    if(mayorPrecio == true)
                    {
                        listOferta = auxOferta.retornaOfertaPublicadaListPrecioMenor(Convert.ToInt32(precioInicio), Convert.ToInt32(precioFin), order);
                    }
                    else
                    {
                        // listOferta = auxOferta.retornaOfertaPublicadaListPrecio(Convert.ToInt32(precioInicio), Convert.ToInt32(precioFin));
                        listOferta = auxOferta.retornaOfertaPublicadaListPrecioMenor(Convert.ToInt32(precioInicio), Convert.ToInt32(precioFin), order);
                    }
                }

                
            }
            else
            {
                
                listOferta = auxOferta.retornaOfertaPuublicadaList(Convert.ToInt32(idRubro), Convert.ToInt32(sucursal));

                //Generacion de log para el seguimiento usuario

                if(Convert.ToInt32(Session["idUsuario"]) != 0)
                {
                    NegocioLog negocioLog = new NegocioLog();
                    LogUsuario logUsuario = new LogUsuario();

                    logUsuario.rubro.IdRubro = Convert.ToInt32(idRubro);
                    logUsuario.usuario.IdUsuario = Convert.ToInt32(Session["idUsuario"]);

                    negocioLog.insertarLog(logUsuario);
                }
               

            }


            ViewBag.listaRangoPrecio = obtenerRangoPrecio();
            ViewBag.listaSucursal = obtenerSucursal();
            ViewBag.listarRubro = obtenerRubro(Convert.ToInt32(Session["idRubro"]));

            return View(listOferta);
        }

        public ActionResult About()
        {
            

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Cupon()
        {
            NegocioCupon negocioCupon = new NegocioCupon();
            Puntaje auxPuntaje = negocioCupon.retornaPuntaje(Convert.ToInt32(Session["idUsuario"]));          

                return View(auxPuntaje);
        }

  
        public ActionResult GenerarCupon(string puntaje)
        {
            int cantidad = Convert.ToInt32(puntaje);
            string rubro = "";
            int tope = 0;
            NegocioCupon negocioCupon = new NegocioCupon();

            if (cantidad>0 && cantidad<=100)
                {
                    rubro = "Alimentos";
                    tope = 100000;
                    crearPdf(5, rubro,tope);
                negocioCupon.limpiarPuntaje(Convert.ToInt32(Session["idUsuario"]));
                 }

            if (cantidad > 101 && cantidad <= 500)
            {
                rubro = "Alimentos, Electrónica y Línea Blanca";
                tope = 150000;
                crearPdf(10, rubro, tope);
                negocioCupon.limpiarPuntaje(Convert.ToInt32(Session["idUsuario"]));
            }

            if (cantidad > 101 && cantidad <= 500)
            {
                rubro = "Alimentos, Electrónica, Línea Blanca, Ropa";
                tope = 300000;
                crearPdf(15, rubro, tope);
                negocioCupon.limpiarPuntaje(Convert.ToInt32(Session["idUsuario"]));
            }
            Puntaje p = negocioCupon.retornaPuntaje(Convert.ToInt32(Session["idUsuario"]));



            return RedirectToAction("Cupon", "Home",p);
        }

        public Document crearPdf(int descuento, string rubro,int tope)
        {
            Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 15);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();


            //TEXTO DEL HEADING
            Chunk chunk = new Chunk("MIS OFERTAS", FontFactory.GetFont("Arial", 20, Font.BOLDITALIC, BaseColor.DARK_GRAY));
            pdfDoc.Add(chunk);

            //Horizontal Line
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line);

            //Table
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100;
            //0=Left, 1=Centre, 2=Right
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            //IMAGEN DEL CODIGO
            PdfPCell cell = new PdfPCell();
            cell.Border = 0;

            // Codigo qr

            QRCodeGenerator qr = new QRCodeGenerator();
            //Texto que tendra el codigo
            string code = "Usuario: " + Session["nombreUsuario"] + "\nFecha:" + DateTime.Now + "\nRubro: "+rubro+"\nDescuento: "+descuento+ "% \nTope: "+tope+"\nCodigo: "+ Session["nombreUsuario"] + DateTime.Now;
            QRCodeData qrData = qr.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrData);


            System.Web.UI.WebControls.Image imageQr = new System.Web.UI.WebControls.Image();
            imageQr.Height = 150;
            imageQr.Width = 150;

            using (System.Drawing.Bitmap bitmap = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    imageQr.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                    string base64 = Convert.ToBase64String(byteImage);
                    byte[] imageBytes = Convert.FromBase64String(base64);
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageBytes);
                    image.ScaleAbsolute(200, 150);
                    cell.AddElement(image);
                }

            }
            table.AddCell(cell);
            //Cell no 2
            chunk = new Chunk("\nFecha:  " + String.Format("{0:dd/MM/yyyy}", DateTime.Now)+ "\nNombre: " + Session["nombre"] + "\nEmail:  " + Session["nombreUsuario"] + "\nRubro: " + rubro + "\nDescuento: " + descuento + "% \nTope: " + tope, FontFactory.GetFont("Arial", 15, Font.NORMAL, BaseColor.BLACK));
            cell = new PdfPCell();
            cell.Border = 0;
            cell.AddElement(chunk);
            table.AddCell(cell);

            //Add table to document
            pdfDoc.Add(table);

            //Horizontal Line
            line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line);

            //creador pdf importante
            pdfWriter.CloseStream = false;
            pdfDoc.Close();

            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=CuponDescuento"+Session["nombreUsuario"]+ String.Format("{0:dd/MM/yyyy}", DateTime.Now) + ".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();

        
            return pdfDoc;
        }

        public List<SelectListItem> obtenerRangoPrecio()
        {
            var lista = new List<SelectListItem>(){

                 new SelectListItem()
                {
                    Text = "Menor precio",
                    Value = "1"
                },
                  new SelectListItem()
                {
                    Text = "Mayor precio",
                    Value = "2"
                },

                new SelectListItem()
                {
                    Text = "0 - 10000",
                    Value = "3"
                },
                new SelectListItem()
                {
                    Text = "10000 - 50000",
                    Value = "4"
                },
                new SelectListItem()
                {
                    Text = "50000 - 100000",
                    Value = "5"
                },
                new SelectListItem()
                {
                    Text = "100000 - 500000",
                    Value = "6"
                }
            };



            return lista;
        }

        public List<SelectListItem> obtenerSucursal()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            List<Sucursal> listSucursal;
            NegocioProducto auxNegocio = new NegocioProducto();

            listSucursal = auxNegocio.retornaSucursal();

            foreach (var li in listSucursal)
            {
                list.Add(new SelectListItem()
                {
                    Text = li.Nombre,
                    Value = li.IdSucursal.ToString()



                });
            }

            return list;
        }

        public List<SelectListItem> obtenerRubro(int idRubro)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            List<Rubro> listRubro;
            NegocioOferta auxNegocio = new NegocioOferta();

            listRubro = auxNegocio.retornaRubroList();

            bool resultado = false;

            foreach (var li in listRubro)
            {
                if (idRubro == li.IdRubro)
                {
                    resultado = true;                    
                }
                else
                {
                    if(li.IdRubro == 1)
                    {
                        resultado = true;
                    }
                    else
                    {
                        resultado = false;
                    }
                }

                list.Add(new SelectListItem()
                {
                    Text = li.Nombre,
                    Value = li.IdRubro.ToString(),
                    Selected = resultado                


                });
            }

            return list;
        }

        public List<SelectListItem> obtenerCalificacion()
        {
            var lista = new List<SelectListItem>(){

                 new SelectListItem()
                {
                    Text = "Excelente",
                    Value = "Excelente"
                },
                  new SelectListItem()
                {
                    Text = "Buena",
                    Value = "Buena"
                },

                new SelectListItem()
                {
                    Text = "Mala",
                    Value = "Mala"
                }
               
            };

            return lista;
        }



        public ActionResult convertirImagen(string id)
        {
            NegocioOferta auxOferta = new NegocioOferta();
            var imagen = auxOferta.retornaOferta(Convert.ToInt32(id));
            return File(imagen.Imagen, "image/jpeg");
        }

        [Authorize]
        public ActionResult Valoracion()
        {
            NegocioOferta aux = new NegocioOferta();
            Oferta oferta = aux.retornaOferta(Convert.ToInt32(Session["idOferta"]));
             
            Valoracion valoracion = new Valoracion();
           
                valoracion.oferta.IdOferta = oferta.IdOferta;
                valoracion.oferta.Nombre = oferta.Nombre;
                valoracion.usuario.IdUsuario = Convert.ToInt32(Session["idUsuario"]);
                valoracion.usuario.NombreUsuario = Session["nombreUsuario"].ToString();
                ViewBag.listaCalificacion = obtenerCalificacion();

            // Registramos el seguimiento del usuario

            NegocioLog negocioLog = new NegocioLog();
            LogUsuario logUsuario = new LogUsuario();

            logUsuario.rubro.IdRubro = Convert.ToInt32(oferta.rubro.IdRubro);
            logUsuario.usuario.IdUsuario = Convert.ToInt32(Session["idUsuario"]);

            negocioLog.insertarLog(logUsuario);        
            
            
            return View(valoracion);
        }

        [HttpPost]
        public ActionResult Valoracion(Valoracion valoracion, HttpPostedFileBase file, string calificacion)
        {
            if (ModelState.IsValid)
            {
                NegocioValoracion auxValoacion = new NegocioValoracion();
                NegocioCupon negocioCupon = new NegocioCupon();

                if (file != null && file.ContentLength > 0)
                {
                    byte[] imagenData = null;
                    using (var binaryImagen = new BinaryReader(file.InputStream))
                    {
                        imagenData = binaryImagen.ReadBytes(file.ContentLength);
                    }
                    valoracion.Boleta = imagenData;
                }
                valoracion.oferta.IdOferta = Convert.ToInt32(Session["idOferta"]);
                valoracion.usuario.IdUsuario = Convert.ToInt32(Session["idUsuario"]);
                valoracion.Calificacion = calificacion;

                bool resultado = auxValoacion.insertarValoracion(valoracion);

                if (resultado == true)
                {
                    ModelState.Clear();
                    ModelState.AddModelError("", "Datos Correctos");
                    negocioCupon.insertarPuntaje(valoracion.usuario.IdUsuario);
                    
                }
                else
                {
                    ModelState.AddModelError("", "Error datos invalidos");
                }
                // 
                
            }
            ViewBag.listaCalificacion = obtenerCalificacion();
            return View();
        }
        [Authorize]
        public ActionResult VerValoracion(string id)
        {
            Session["idOferta"] = id;

            NegocioValoracion auxValoracion = new NegocioValoracion();
            List<Valoracion> listaValoracion = auxValoracion.retornaValoracionList(Convert.ToInt32(id));
            
            return View(listaValoracion);            
        }

        public ActionResult convertirBoleta(string id)
        {
            NegocioValoracion negocioValoracion = new NegocioValoracion();
            var imagen= negocioValoracion.retornaValoracion(Convert.ToInt32(id));

            return File(imagen.Boleta, "image/jpeg");
        }
    }
}