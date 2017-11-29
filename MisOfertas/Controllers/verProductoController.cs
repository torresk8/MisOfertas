using CapaDTO;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MisOfertas.Controllers
{
    public class verProductoController : Controller
    {
        // GET: Producto
        public ActionResult Index()
        {
            return View();
        }


        //
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


        [HttpPost]
        public ActionResult eliminarProducto(Producto producto)
        {
            NegocioProducto negocioProducto = new NegocioProducto();

            Producto auxProducto = new Producto();

            auxProducto.IdProducto = producto.IdProducto;
            bool resultado = negocioProducto.eliminarProducto(producto.IdProducto);

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
            auxProducto.Marca = producto.Marca;
            auxProducto.Descripcion = producto.Descripcion;
            auxProducto.Precio = producto.Precio;
            auxProducto.Modelo = producto.Modelo; 
            auxProducto.Stock = producto.Stock;
            return View(auxProducto);   
        }

        [HttpPost]
        public ActionResult actualizarProducto(Producto producto)
        {

            NegocioProducto auxNegocioProducto = new NegocioProducto();
    
            Producto auxProducto = new Producto();

            
            bool resultado = auxNegocioProducto.actualizarProducto(producto);


            auxProducto.IdProducto = producto.IdProducto; 
             auxProducto.Marca = producto.Marca;
            auxProducto.Descripcion = producto.Descripcion;
            auxProducto.Precio = producto.Precio;
            auxProducto.Modelo = producto.Modelo;
            auxProducto.Stock = producto.Stock;

            if (resultado == true)
            {
                ModelState.AddModelError("", "Datos Correctos");
            }
            else
            {
                ModelState.AddModelError("", "Error datos invalidos");
            }

            return View(auxProducto);
        }



    }
}