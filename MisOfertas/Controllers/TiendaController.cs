using CapaDTO;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MisOfertas.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        [Authorize(Roles = "gerenteAsociacion")]
        public ActionResult Index()
        {


            NegocioTienda negocioTienda = new NegocioTienda();
            List<ResumenTienda> listResumenTienda = negocioTienda.retornaResumenTiendaList(1);

            NegocioOferta negocioOferta = new NegocioOferta();
            Oferta o = negocioOferta.retornaOferta(Convert.ToInt32(1));

            List<Oferta> list = new List<Oferta>();

            list.Add(o);

            ViewBag.oferta = list;

            ViewBag.listaSucursal = obtenerSucursal(1);

            return View(listResumenTienda);
        }
        [HttpPost]
        public ActionResult Index(string idSucursal, string id)
        {


            NegocioTienda negocioTienda = new NegocioTienda();
            List<ResumenTienda> listResumenTienda = negocioTienda.retornaResumenTiendaList(Convert.ToInt32(idSucursal));

            NegocioOferta negocioOferta = new NegocioOferta();            
            Oferta o = negocioOferta.retornaOferta(Convert.ToInt32(id));

            List<Oferta> list = new List<Oferta>();

            list.Add(o);

            ViewBag.oferta = list;

            ViewBag.listaSucursal = obtenerSucursal(Convert.ToInt32(id));
            return View(listResumenTienda);
        }

        public JsonResult oferta(string id)
        {
            NegocioOferta negocioOferta = new NegocioOferta();
            Oferta o = negocioOferta.retornaOferta(Convert.ToInt32(id));

            List<Oferta> list = new List<Oferta>();

            list.Add(o);

            var listaOferta = list.Where(x => x.IdOferta == Convert.ToInt32(id))
                .Select(a => new
                {

                    id = a.IdOferta,
                    nombre = a.Nombre,
                    descripcion = a.Descripcion,
                    precioNormal = a.PrecioNormal,
                    precioOferta = a.PrecioOfeta,
                    cantidadMin = a.CantidadMin,
                    cantidadMax = a.CantidadMax,
                    idProducto = a.Producto.Nombre,
                    estado = a.Estado,
                    rubro = a.rubro.Nombre,
                    sucursal = a.sucursal.Nombre,


                }
                );

            return Json(listaOferta, JsonRequestBehavior.AllowGet);
        }

        public JsonResult usuarioJson(string id)
        {
            NegocioUsuario negocioUsuario = new NegocioUsuario();
            Usuario usuario = negocioUsuario.retornaUsuario(Convert.ToInt32(id));

            List<Usuario> list = new List<Usuario>();

            list.Add(usuario);

            var listaUsuario = list.Where(x => x.IdUsuario == Convert.ToInt32(id))
                .Select(a => new
                {

                    id = a.IdUsuario,
                    nombre = a.Nombre,
                    nombreUsuario = a.NombreUsuario,
                    rut = a.Rut,
                    direccion = a.Direccion,
                    telefono = a.Telefono,
                    recibirCorreo = a.RecibirCorreo                  

                }
                );

            return Json(listaUsuario, JsonRequestBehavior.AllowGet);
        }

        public List<SelectListItem> obtenerSucursal(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            List<Sucursal> listSucursal;
            NegocioProducto auxNegocio = new NegocioProducto();

            listSucursal = auxNegocio.retornaSucursal();

            bool resultado = false;

            foreach (var li in listSucursal)
            {
                if(li.IdSucursal == id)
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
                    Value = li.IdSucursal.ToString(),
                    Selected = resultado



                });
            }

            return list;
        }


        public ActionResult ReporteTienda()
        {


            NegocioReporte negocioReporte = new NegocioReporte();
            List<Reporte> listReporte = negocioReporte.retornaReporte();
                  
            
            return View(listReporte);
        }

        [Authorize(Roles = "gerenteAsociacion")]
        public ActionResult reporteBi()
        {
            return View();
        }

        [HttpPost]
        public ActionResult reporteBi(string a)
        {

            ServiceReporte.reporteSoapClient reporte = new ServiceReporte.reporteSoapClient();
            reporte.archivoPlano();
            return View();
        }

        public FileResult Download()
        {
            NegocioReporte negocioReporte = new NegocioReporte();
            ServiceReporte.reporteSoapClient reporte = new ServiceReporte.reporteSoapClient();
            reporte.archivoPlano();
            return File(negocioReporte.archivo(reporte.archivoPlano()), "text/plain", "ReporteBi"+DateTime.Now+".csv");
        }
    }
}