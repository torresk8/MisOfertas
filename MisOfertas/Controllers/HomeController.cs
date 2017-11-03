using CapaDTO;
using CapaNegocio;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MisOfertas.MyRole;
using MisOfertas.Seguridad;
using System;
using System.Collections.Generic;
using System.IO;
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

            NegocioOferta auxOferta = new NegocioOferta();
            Rubro auxRubro = auxOferta.retornaRubro(Convert.ToInt32(1));

            Session["idRubro"] = auxRubro.IdRubro;
            Session["nombreRubro"] = auxRubro.Nombre;
            List<Oferta> listOferta = auxOferta.retornaOfertaPuublicadaList(Convert.ToInt32(1));
            return View(listOferta);

        }    
        
        [HttpPost]
        public ActionResult Index(string idRubro)
        {

            NegocioOferta auxOferta = new NegocioOferta();
            Rubro auxRubro = auxOferta.retornaRubro(Convert.ToInt32(idRubro));

            Session["idRubro"] = auxRubro.IdRubro;
            Session["nombreRubro"] = auxRubro.Nombre;
            List<Oferta> listOferta = auxOferta.retornaOfertaPuublicadaList(Convert.ToInt32(idRubro));
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

  
        public ActionResult GenerarCupon(Puntaje puntaje)
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
            Image image = Image.GetInstance(Server.MapPath("~/Content/Upload/CodigoQR.png"));
            image.ScaleAbsolute(200, 150);
            cell.AddElement(image);
            table.AddCell(cell);
            //Cell no 2
            chunk = new Chunk("Nombre: ,\nEmail: , \nProducto: , \nFecha: ", FontFactory.GetFont("Arial", 15, Font.NORMAL, BaseColor.BLACK));
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
            Response.AddHeader("content-disposition", "attachment;filename=MisOfertasProducto.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();
        


            NegocioCupon negocioCupon = new NegocioCupon();
            negocioCupon.limpiarPuntaje(Convert.ToInt32(Session["idUsuario"]));
            puntaje = negocioCupon.retornaPuntaje(Convert.ToInt32(Session["idUsuario"]));


            return RedirectToAction("Index", "Home");
        }

        public ActionResult OfertasPublicadas()
        {                     
            NegocioOferta auxOferta = new NegocioOferta();
            Rubro auxRubro = auxOferta.retornaRubro(Convert.ToInt32(1));

            Session["idRubro"] = auxRubro.IdRubro;
            Session["nombreRubro"] = auxRubro.Nombre;
            List<Oferta> listOferta = auxOferta.retornaOfertaPuublicadaList(Convert.ToInt32(1));
            return View(listOferta);
        }
        [HttpPost]
        public ActionResult OfertasPublicadas(string idRubro)
        {

            
            NegocioOferta auxOferta = new NegocioOferta();
            Rubro auxRubro = auxOferta.retornaRubro(Convert.ToInt32(idRubro));

            Session["idRubro"] = auxRubro.IdRubro;
            Session["nombreRubro"] = auxRubro.Nombre;

            List<Oferta> listOferta = auxOferta.retornaOfertaPuublicadaList(Convert.ToInt32(idRubro));
            return View(listOferta);
        }

       

        public ActionResult convertirImagen(string id)
        {
            NegocioOferta auxOferta = new NegocioOferta();
            var imagen = auxOferta.retornaOferta(Convert.ToInt32(id));
            return File(imagen.Imagen, "image/jpeg");
        }

        public ActionResult Valoracion(int id)
        {
            NegocioOferta aux = new NegocioOferta();
            Oferta oferta = aux.retornaOferta(id);
            Session["idOferta"] = oferta.IdOferta;

            Valoracion valoracion = new Valoracion();
            valoracion.oferta.IdOferta = oferta.IdOferta;
            valoracion.oferta.Nombre = oferta.Nombre;
            valoracion.usuario.IdUsuario = Convert.ToInt32(Session["idUsuario"]);
            valoracion.usuario.NombreUsuario = Session["nombreUuario"].ToString();

            return View(valoracion);
        }

        [HttpPost]
        public ActionResult Valoracion(Valoracion valoracion, HttpPostedFileBase file)
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

                bool resultado = auxValoacion.insertarValoracion(valoracion);

                if (resultado == true)
                {
                    ModelState.AddModelError("", "Datos Correctos");
                    negocioCupon.insertarPuntaje(valoracion.usuario.IdUsuario);
                    ModelState.Clear();
                }
                else
                {
                    ModelState.AddModelError("", "Error datos invalidos");
                }
                // 
                
            }            
            return View();
        }

        public ActionResult VerValoracion(string id)
        {           
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