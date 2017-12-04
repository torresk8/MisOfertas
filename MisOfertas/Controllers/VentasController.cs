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
                        ModelState.AddModelError("", "Datos Correctos");
                        ModelState.Clear();
                        Session["idProducto"] = "";
                        Session["precio"] = "";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error datos invalidos");
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

        public ActionResult Producto(string id)
        {

            

            NegocioProducto auxProducto = new NegocioProducto();
            List<Producto> listProducto = auxProducto.retornaProductoList();

            Producto producto = new Producto();
            Producto prod = auxProducto.retornaProducto(Convert.ToInt32(id));

            ViewBag.lista = listProducto;
            if(id==null)
            {                
                id = "0";
                ViewBag.listaTipoProducto = obtenerTipoProducto();
            }
            else
            {

                producto.Nombre = prod.Nombre;
                producto.Precio = prod.Precio;
                producto.IdProducto = prod.IdProducto;
                producto.Descripcion = prod.Descripcion;
                producto.Stock = prod.Stock;
                producto.TipoProducto.IdTipoProducto = prod.TipoProducto.IdTipoProducto;
                ViewBag.listaTipoProducto = obtenerTipoProducto();
            }
                        
            Session["idProducto"] = id;                                

            return View(producto);
        }


        [HttpPost]
        public ActionResult Producto(Producto producto, string idTipoProducto, string idProducto)
        {
            if (ModelState.IsValid)
            {
                NegocioProducto auxProducto = new NegocioProducto();
                int idTipoProduc = Convert.ToInt32(idTipoProducto);                
                bool resultado = false;

                producto.TipoProducto.IdTipoProducto = idTipoProduc;
                producto.IdProducto = Convert.ToInt32(idProducto);
                
                if(producto.IdProducto>0)
                {
                    resultado = auxProducto.actualizarProducto(producto);
                }
                else
                {
                    resultado = auxProducto.insertarProducto(producto);
                }


                

                if (resultado == true)
                {
                    ModelState.AddModelError("", "Datos Correctos");
                    ModelState.Clear();
                }
                else
                {

                    ModelState.AddModelError("", "Error datos invalidos");

                }

                ViewBag.listaTipoProducto = obtenerTipoProducto();
                
                List<Producto> listProducto = auxProducto.retornaProductoList();

                ViewBag.lista = listProducto;

                // 
            }
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


       

        public List<SelectListItem> obtenerRubro()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            List<Rubro> listRubro;
            NegocioOferta negocioOferta = new NegocioOferta();

            listRubro = negocioOferta.retornaRubroList();

            foreach (var li in listRubro)
            {
                list.Add(new SelectListItem()
                {
                    Text = li.Nombre,
                    Value = li.IdRubro.ToString()


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

            auxOferta.Nombre = oferta.Nombre;
            return View(auxOferta);
        }

        [HttpPost]
        public ActionResult actualizarOferta(Oferta oferta)
        {

            NegocioOferta auxNegocioOferta = new NegocioOferta();
            bool resultado = auxNegocioOferta.actualizarOferta(oferta);
            Oferta auxOferta = new Oferta();

            auxOferta.Nombre = oferta.Nombre;
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
            ViewBag.listaRubro = obtenerRubro();

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
                    ModelState.AddModelError("", "Datos Correctos");
                    ModelState.Clear();
                }
                else
                {

                    ModelState.AddModelError("", "Error datos invalidos");

                }
                ViewBag.listaRubro = obtenerRubro();
                // 
            }

            return View();
        }

    }
}