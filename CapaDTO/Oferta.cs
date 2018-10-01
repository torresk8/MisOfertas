using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDTO
{
    public class Oferta
    {
        private int idOferta;
        private string nombre;
        private string descripcion;
        private int precioNormal;
        private int precioOfeta;
        private int cantidadMin;
        private int cantidadMax;
        public Producto Producto = new Producto();
        private byte[] imagen;        
        private string estado;
        public Rubro rubro = new Rubro();
        public Sucursal sucursal = new Sucursal();
        public Descuento descuento = new Descuento();

        public Oferta()
        {

        }

        public int IdOferta { get => idOferta; set => idOferta = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public int PrecioNormal { get => precioNormal; set => precioNormal = value; }
        public int PrecioOfeta { get => precioOfeta; set => precioOfeta = value; }
        public int CantidadMin { get => cantidadMin; set => cantidadMin = value; }
        public int CantidadMax { get => cantidadMax; set => cantidadMax = value; }        
        public byte[] Imagen { get => imagen; set => imagen = value; }        
        public string Estado { get => estado; set => estado = value; }
    }
}
