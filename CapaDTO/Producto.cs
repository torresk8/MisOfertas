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
        private string marca;
        private string modelo;
        private int stock;
        private string descripcion;
        private int precio;
        public TipoProducto TipoProducto = new TipoProducto();
        public Sucursal Sucursal = new Sucursal();
        public int IdTipoProducto { get; set; }
        public int IdSucursal { get; set; }

        public Producto()
        {

        }

        public int IdProducto { get => idProducto; set => idProducto = value; }
        public string Marca { get => marca; set => marca = value; }
        public string Modelo { get => modelo; set => modelo = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public int Precio { get => precio; set => precio = value; }
        public int Stock { get => stock; set => stock = value; }
    }
}
