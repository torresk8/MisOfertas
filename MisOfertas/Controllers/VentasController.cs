using CapaDTO;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MisOfertas.Controllers
{
    [Authorize(Roles = "crearOferta")]    
    public class VentasController : Controller
    {
        // GET: Ventas
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult convertirImagen(string id)
        {
            NegocioOferta auxOferta = new NegocioOferta();
            var imagen = auxOferta.retornaOferta(Convert.ToInt32(id));
            return File(imagen.Imagen, "image/jpeg");
        }

        [HttpPost]
        public ActionResult Index(Oferta oferta, HttpPostedFileBase file,string idProducto)
        {
            if (ModelState.IsValid)
            {
                NegocioOferta auxOferta = new NegocioOferta();

                if (file !=  null && file.ContentLength > 0)
                {
                    byte[] imagenData = null;
                    using (var binaryImagen = new BinaryReader(file.InputStream)) 
                    {
                        imagenData = binaryImagen.ReadBytes(file.ContentLength);
                    }
                    oferta.Imagen = imagenData;               
                }
                oferta.Producto.IdProducto = Convert.ToInt32(idProducto);

                bool resultado = auxOferta.insertarOferta(oferta);

                if (resultado == true)
                {
                    ModelState.AddModelError("", "Datos Correctos");
                }
                else
                {
                    ModelState.AddModelError("", "Error datos invalidos");
                }
                // 
            }
            return View();
        }

        public ActionResult Producto()
        {



            ViewBag.listaTipoProducto = obtenerTipoProducto();            
            ViewBag.listaSucursal = obtenerSucursal();     


            return View();
        }
       
        public List<SelectListItem> obtenerTipoProducto()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            List<TipoProducto> listTipoProducto = new List<TipoProducto>();
            NegocioProducto auxNegocio = new NegocioProducto();

            listTipoProducto = auxNegocio.retornaTipoProducto();


            foreach (var li in listTipoProducto)
            {
                list.Add(new SelectListItem()
                {
                    Text = li.Nombre,
                    Value = li.IdTipoProducto.ToString()
                    

                });
            }

            return list;
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

        [HttpPost]
        public ActionResult Producto(Producto producto,string idTipoProducto, string idSucursal)
        {
            if (ModelState.IsValid)
            {
                NegocioProducto auxProducto = new NegocioProducto();
                int idTipoProduc = Convert.ToInt32(idTipoProducto);
                int idSucur = Convert.ToInt32(idSucursal);
                bool resultado = false;
               
                  producto.TipoProducto.IdTipoProducto = idTipoProduc;
                  producto.Sucursal.IdSucursal = idSucur;
                

                     resultado = auxProducto.insertarProducto(producto);                                

                if (resultado == true)
                {
                    ModelState.AddModelError("", "Datos Correctos");
                    
                    
                }
                else
                {
                    
                    ModelState.AddModelError("", "Error datos invalidos");
                    
                }
               


                ViewBag.listaTipoProducto = obtenerTipoProducto();
                ViewBag.listaSucursal = obtenerSucursal();
                //ModelState.Clear();


                // 
            }
            ModelState.Clear();
            return View();
           
        }

        private void  Limpiar(Producto producto)
        {

            ModelState.Clear();

        }





        public ActionResult VerOferta()
        {
            
            NegocioOferta auxOferta = new NegocioOferta();
            List<Oferta> listOferta = auxOferta.retornaOfertaList();                        
            
            return View(listOferta);
        }

        public Object retornaTabla()
        {
            NegocioOferta auxOferta = new NegocioOferta();
            List<Oferta> listOferta = auxOferta.retornaOfertaList();
            Object Json = new { data = listOferta };
            return Json;
        }


        //muestra el estado de las ofertas
        [HttpPost]
        public ActionResult VerOferta(string estado, int id)
        {
            NegocioOferta auxnegocioOferta = new NegocioOferta();            
            
            
            if (estado == "Publicado")
            {
                auxnegocioOferta.actualizarOfertaEstado("No publicado", id);
            }
            else
            {
                auxnegocioOferta.actualizarOfertaEstado("Publicado", id);
            }
                    

            List<Oferta> listOferta = auxnegocioOferta.retornaOfertaList();

            return View(listOferta);
        }
            

        public ActionResult OfertasPublicadas()
        {
            NegocioOferta auxOferta = new NegocioOferta();
            List<Oferta> listOferta = auxOferta.retornaOfertaPuublicadaList();
            return View(listOferta);
        }

        


        public ActionResult valoracion(int id)
        {
            NegocioOferta aux = new NegocioOferta();
            Oferta oferta =aux.retornaOferta(id);
            Session["idOferta"] = oferta.IdOferta;

            Valoracion valoracion = new Valoracion();
            valoracion.oferta.IdOferta = oferta.IdOferta;
            valoracion.oferta.Nombre = oferta.Nombre;
            valoracion.usuario.IdUsuario = Convert.ToInt32(Session["idUsuario"]);
            valoracion.usuario.NombreUsuario = Session["nombreUsuario"].ToString();

            return View(valoracion);
        }



    
        [HttpPost]
        public ActionResult valoracion(Valoracion valoracion, HttpPostedFileBase file )
        {
            if (ModelState.IsValid)
            {
                NegocioValoracion auxValoacion = new NegocioValoracion();                              

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
                }
                else
                {
                    ModelState.AddModelError("", "Error datos invalidos");
                }
                // 
            }
            return View();
        }

        public ActionResult actualizarOferta(int id)
        {
            
            NegocioOferta auxNegocioOferta = new NegocioOferta();
            Oferta oferta = auxNegocioOferta.retornaOferta(id);
            Oferta auxOferta = new Oferta();

            auxOferta.Nombre = oferta.Nombre;
            auxOferta.Descripcion = oferta.Descripcion;
            auxOferta.PrecioNormal = oferta.PrecioNormal;
            auxOferta.PrecioOfeta = oferta.PrecioOfeta;
            auxOferta.CantidadMax = oferta.CantidadMax;
            auxOferta.CantidadMax = oferta.CantidadMin;
            ViewBag.Prueba = oferta;
            return View(auxOferta);
        }

        [HttpPost]
        public ActionResult actualizarOferta(Oferta oferta)
        {

            NegocioOferta auxNegocioOferta = new NegocioOferta();
            bool resultado = auxNegocioOferta.actualizarOferta(oferta);
            Oferta auxOferta = new Oferta();


            auxOferta.Nombre = oferta.Nombre;
            auxOferta.Descripcion = oferta.Descripcion;
            auxOferta.PrecioNormal = oferta.PrecioNormal;
            auxOferta.PrecioOfeta = oferta.PrecioOfeta;
            auxOferta.CantidadMax = oferta.CantidadMax;
            auxOferta.CantidadMax = oferta.CantidadMin;
           
    

            if (resultado == true)
            {
                ModelState.AddModelError("", "Datos Correctos");
            }
            else
            {
                ModelState.AddModelError("", "Error datos invalidos");
            }

            return View(auxOferta);
        }



        public ActionResult eliminarOferta(int id)
        {
            NegocioOferta negocioOferta = new NegocioOferta();
            bool resultado = negocioOferta.eliminarOferta(id);

            List<Oferta> listOferta = negocioOferta.retornaOfertaList();

            return View("VerOferta",listOferta);
        }


        [HttpPost]
        public ActionResult eliminarOferta(Oferta oferta)
        {
            NegocioOferta negocioOferta = new NegocioOferta();
            
            Oferta auxOferta = new Oferta();

            auxOferta.IdOferta = oferta.IdOferta;
            bool resultado = negocioOferta.eliminarOferta(oferta.IdOferta);

            List<Oferta> listOferta = negocioOferta.retornaOfertaList();

            return View("VerOferta", listOferta);
        }
    }
}