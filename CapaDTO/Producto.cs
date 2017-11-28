using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDTO
{
    public class Producto
    {
        private int idProducto;
        private string nombre;
        private string descripcion;
        private int precio;
        public TipoProducto TipoProducto = new TipoProducto();                
        public int IdTipoProducto { get; set; }
        public int IdSucursal { get; set; }
        public int Stock { get; set; }

        public Producto()
        {

        }

        public int IdProducto { get => idProducto; set => idProducto = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public int Precio { get => precio; set => precio = value; }
        
        
    }
}
