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
    [Authorize(Roles = "crearOferta,encargadoTienda")]    
    public class VentasController : Controller
    {
        // GET: Ventas
        public ActionResult Index(string id, string nom, string precio)
        {
            string a = ""; 
            ViewBag.listaSucursal = obtenerSucursal();
            ViewBag.listaDescuento = obtenerDescuento(0);            

            NegocioProducto auxProducto = new NegocioProducto();
            List<Producto> listProducto = auxProducto.retornaProductoList();


            ViewBag.lista = listProducto;            
            Session["idProducto"] = id;
            Session["precio"] = precio;  
            
            return View();
        }


        public ActionResult convertirImagen(string id)
        {
            NegocioOferta auxOferta = new NegocioOferta();
            var imagen = auxOferta.retornaOferta(Convert.ToInt32(id));
            return File(imagen.Imagen, "image/jpeg");
        }

        [HttpPost]
        public ActionResult Index(Oferta oferta, HttpPostedFileBase file,string idProducto,string idRubro, string idSucursal, string idDescuento)
        {

            if(idSucursal != null)
            {
                if (ModelState.IsValid)
                {
                    NegocioOferta auxOferta = new NegocioOferta();

                    if (file != null && file.ContentLength > 0)
                    {
                        byte[] imagenData = null;
                        using (var binaryImagen = new BinaryReader(file.InputStream))
                        {
                            imagenData = binaryImagen.ReadBytes(file.ContentLength);
                        }
                        oferta.Imagen = imagenData;
                    }
                    oferta.Producto.IdProducto = Convert.ToInt32(idProducto);
                    oferta.rubro.IdRubro = Convert.ToInt32(idRubro);
                    oferta.sucursal.IdSucursal = Convert.ToInt32(idSucursal);
                    oferta.PrecioNormal = Convert.ToInt32(Session["precio"]);
                    oferta.Producto.IdProducto = Convert.ToInt32(Session["idProducto"]);

                    NegocioDescuento negocioDescuento = new NegocioDescuento();
                    Descuento descuento = negocioDescuento.retornaDescuento(Convert.ToInt32(idRubro),Convert.ToInt32(idDescuento));
                    oferta.descuento.cantidad = descuento.cantidad;

                    oferta.PrecioOfeta = (oferta.PrecioNormal-((oferta.PrecioNormal * oferta.descuento.cantidad)/100));

                    bool resultado = auxOferta.insertarOferta(oferta);

                    if (resultado == true)
                    {
                        
                        ModelState.Clear();
                        Session["idProducto"] = "";
                        Session["precio"] = "";
                        ModelState.AddModelError("", "Oferta registrada");
                        Session["class"] = "text-success";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error datos invalidos");
                        Session["class"] = "text-danger";
                    }
                    // 
                    }
                }

                ViewBag.listaSucursal = obtenerSucursal();
                ViewBag.listaDescuento = obtenerDescuento(Convert.ToInt32(idRubro));

                NegocioProducto auxProducto = new NegocioProducto();
                List<Producto> listProducto = auxProducto.retornaProductoList();

                ViewBag.lista = listProducto;

                return View();
        }

        public ActionResult Producto(string id,string idTipoProducto)
        {

            

            NegocioProducto auxProducto = new NegocioProducto();
            List<Producto> listProducto = auxProducto.retornaProductoList();

            Producto producto = new Producto();
            Producto prod = auxProducto.retornaProducto(Convert.ToInt32(id));

            ViewBag.lista = listProducto;
            if(id==null)
            {                
                id = "0";
                ViewBag.listaTipoProducto = obtenerTipoProducto(0);
            }
            else
            {

                producto.Nombre = prod.Nombre;
                producto.Precio = prod.Precio;
                producto.IdProducto = prod.IdProducto;
                producto.Descripcion = prod.Descripcion;                
                producto.TipoProducto.IdTipoProducto = Convert.ToInt32(idTipoProducto);
                ViewBag.listaTipoProducto = obtenerTipoProducto(producto.TipoProducto.IdTipoProducto);
            }
                        
            Session["idProducto"] = id;                                

            return View(producto);
        }


        [HttpPost]
        public ActionResult Producto(Producto producto, string idProducto2, string idTipoProducto)
        {
            if (ModelState.IsValid)
            {
                NegocioProducto auxProducto = new NegocioProducto();                         
                bool resultado = false;
                producto.TipoProducto.IdTipoProducto = Convert.ToInt32(idTipoProducto);
                //producto.IdProducto = Convert.ToInt32(idProducto2);
                                             
                resultado = auxProducto.insertarProducto(producto);               

                

                if (resultado == true)
                {
                    ModelState.Clear();
                    ModelState.AddModelError("", "Producto registrado");
                    Session["class"] = "text-success";
                }
                else
                {

                    ModelState.AddModelError("", "Error datos invalidos");
                    Session["class"] = "text-danger";

                }

                ViewBag.listaTipoProducto = obtenerTipoProducto(0);
                
                List<Producto> listProducto = auxProducto.retornaProductoList();

                ViewBag.lista = listProducto;

                // 
            }
            return View();
        }

        public List<SelectListItem> obtenerTipoProducto(int tipoProducto)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            List<TipoProducto> listTipoProducto = new List<TipoProducto>();
            NegocioProducto auxNegocio = new NegocioProducto();

            listTipoProducto = auxNegocio.retornaTipoProducto();

            bool resultado = false; 

            foreach (var li in listTipoProducto)
            {
                if(tipoProducto == li.IdTipoProducto)
                {
                    resultado = true;
                }
                else
                {
                    resultado = false;
                }
                list.Add(new SelectListItem()
                {
                    Text = li.Nombre,
                    Value = li.IdTipoProducto.ToString(),
                    Selected = resultado
                    

                });
            }

            return list;
        }

        public List<SelectListItem> obtenerDescuento(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            List<Descuento> listDescuento = new List<Descuento>();
            NegocioDescuento auxNegocio = new NegocioDescuento();

            listDescuento = auxNegocio.retornaDescuentoListId(id);


            foreach (var li in listDescuento)
            {
                list.Add(new SelectListItem()
                {
                    Text = li.cantidad.ToString()+"%",
                    Value = li.idDescuento.ToString()


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


       

        public List<SelectListItem> obtenerRubro(int idRubro)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            List<Rubro> listRubro;
            NegocioOferta negocioOferta = new NegocioOferta();

            listRubro = negocioOferta.retornaRubroList();

            bool resultado = false;  

            foreach (var li in listRubro)
            {
                if (idRubro == li.IdRubro)
                {
                    resultado = true;

                }
                else
                {
                    resultado = false;
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
  

        

        public ActionResult actualizarOferta(int id)
        {
            
            NegocioOferta auxNegocioOferta = new NegocioOferta();
            Oferta oferta = auxNegocioOferta.retornaOferta(id);
            Oferta auxOferta = new Oferta();

            auxOferta.IdOferta = oferta.IdOferta;
            auxOferta.Nombre = oferta.Nombre;
            auxOferta.Descripcion = oferta.Descripcion;
            auxOferta.PrecioNormal = oferta.PrecioNormal;
            auxOferta.PrecioOfeta = oferta.PrecioOfeta;
            auxOferta.CantidadMin = oferta.CantidadMin;
            auxOferta.CantidadMax = oferta.CantidadMax;
            auxOferta.Estado = oferta.Estado;
            return View(auxOferta);
        }

        [HttpPost]
        public ActionResult actualizarOferta(Oferta oferta)
        {

            NegocioOferta auxNegocioOferta = new NegocioOferta();

            Oferta auxOferta = new Oferta();

            bool resultado = auxNegocioOferta.actualizarOferta(oferta);         

            
            auxOferta.IdOferta = oferta.IdOferta;
            auxOferta.Nombre = oferta.Nombre;
            auxOferta.Descripcion = oferta.Descripcion;
            auxOferta.PrecioNormal = oferta.PrecioNormal;
            auxOferta.PrecioOfeta = oferta.PrecioOfeta;
            auxOferta.CantidadMin = oferta.CantidadMin;
            auxOferta.CantidadMax = oferta.CantidadMax;
            auxOferta.Estado = oferta.Estado;

            if (resultado == true)
            {
                ModelState.Clear();
                ModelState.AddModelError("", "Datos Correctos");
                Session["class"] = "text-success";
            }
            else
            {
                ModelState.AddModelError("", "Error datos invalidos");
                Session["class"] = "text-danger";
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

        [Authorize(Roles = "administrador")]
        public ActionResult crearDescuento()
        {
            ViewBag.listaRubro = obtenerRubro(0);

            return View();
        }

        [HttpPost]
        public ActionResult crearDescuento(Descuento descuento, string idRubro)
        {

            if (ModelState.IsValid)
            {
                NegocioDescuento negocioDescuento = new NegocioDescuento();
                                
                bool resultado = false;

                descuento.rubro.IdRubro = Convert.ToInt32(idRubro);

                resultado = negocioDescuento.insertarDescuento(descuento);

                if (resultado == true)
                {
                    ModelState.Clear();
                    ModelState.AddModelError("", "Descuento registrado");                    
                    Session["class"] = "text-success";
                }
                else
                {

                    ModelState.AddModelError("", "Error datos invalidos");
                    Session["class"] = "text-danger";

                }
                ViewBag.listaRubro = obtenerRubro(0);
                // 
            }

            return View();
        }

        public ActionResult VerProducto()
        {

            NegocioProducto auxNegocioProducto = new NegocioProducto();
            List<Producto> listProducto = auxNegocioProducto.retornaProductoList();

            return View(listProducto);
        }

        [HttpPost]
        public ActionResult VerProducto(Producto producto)
        {

            NegocioProducto auxNegocioProducto = new NegocioProducto();
            List<Producto> listProducto = auxNegocioProducto.retornaProductoList();

            return View(listProducto);
        }

        public ActionResult eliminarProducto(int id)
        {
            NegocioProducto negocioProducto = new NegocioProducto();
            bool resultado = negocioProducto.eliminarProducto(id);

            List<Producto> listProducto = negocioProducto.retornaProductoList();

            return View("VerProducto", listProducto);
        }
   
    

        //

        public ActionResult actualizarProducto(int id)
        {

            NegocioProducto auxNegocioProducto = new NegocioProducto();
            Producto producto = auxNegocioProducto.retornaProducto(id);
            Producto auxProducto = new Producto();

            auxProducto.IdProducto = producto.IdProducto;
            auxProducto.Nombre = producto.Nombre;            
            auxProducto.Descripcion = producto.Descripcion;
            auxProducto.Precio = producto.Precio;            
            ViewBag.listaTipoProducto = obtenerTipoProducto(producto.TipoProducto.IdTipoProducto);
            return View(auxProducto);
        }

        [HttpPost]
        public ActionResult actualizarProducto(Producto producto,string idTipoProducto)
        {

            NegocioProducto auxNegocioProducto = new NegocioProducto();


            producto.TipoProducto.IdTipoProducto = Convert.ToInt32(idTipoProducto);
            bool resultado = auxNegocioProducto.actualizarProducto(producto);

   

            if (resultado == true)
            {
                ModelState.Clear();
                ModelState.AddModelError("", "Datos Correctos");
                Session["class"] = "text-success";
            }
            else
            {
                ModelState.AddModelError("", "Error datos invalidos");
                Session["class"] = "text-danger";
            }
            ViewBag.listaTipoProducto = obtenerTipoProducto(0);
            return View();
        }


        public ActionResult verDescuento()
        {
            
            NegocioDescuento auxNegocoDescuento = new NegocioDescuento();
            List<Descuento> listDescuento = auxNegocoDescuento.retornaDescuentoList();

            return View(listDescuento);
        }

        [HttpPost]
        public ActionResult verDescuento(Descuento descuento)
        {

            NegocioDescuento auxNegocoDescuento = new NegocioDescuento();
            List<Descuento> listDescuento = auxNegocoDescuento.retornaDescuentoList();

            return View(listDescuento);
        }

        public ActionResult eliminarDescuento(int id)
        {
            NegocioDescuento negocioDescuento = new NegocioDescuento();
            bool resultado = negocioDescuento.eliminarDescuento(id);

            List<Descuento> listDescuento = negocioDescuento.retornaDescuentoList();

            return View("verDescuento", listDescuento);
        }

 



        //

        public ActionResult actualizarDescuento(int id)
        {

            NegocioDescuento auxNegocioDescuento = new NegocioDescuento();
            Descuento descuento = auxNegocioDescuento.retornaDescuentoActualizar( id);
            Descuento auxDescuento = new Descuento();

            auxDescuento.idDescuento = descuento.idDescuento;
            auxDescuento.cantidad = descuento.cantidad;
            auxDescuento.rubro.IdRubro = descuento.rubro.IdRubro;

            ViewBag.listaRubro = obtenerRubro(descuento.rubro.IdRubro);

            return View(auxDescuento);
        }

        [HttpPost]
        public ActionResult actualizarDescuento(Descuento descuento, string idRubro)
        {

            NegocioDescuento auxNegocioDescuento = new NegocioDescuento();
            descuento.rubro.IdRubro = Convert.ToInt32(idRubro);

            bool resultado = auxNegocioDescuento.actualizarDescuento(descuento);
            

            if (resultado == true)
            {
                ModelState.Clear();                
                ModelState.AddModelError("", "Datos Correctos");
                Session["class"] = "text-success";
            }
            else
            {
                ModelState.Clear();
                ModelState.AddModelError("", "Error datos invalidos");
                Session["class"] = "text-danger";
            }

            ViewBag.listaRubro = obtenerRubro(0);
            return View();
        }

    }
}