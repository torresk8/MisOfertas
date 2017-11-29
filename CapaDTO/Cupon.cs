using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDTO
{
    public class Cupon
    {
        public int IdCupon { get; set; }
        public int Cantidad { get; set; }
        public int Descuento { get; set; }
        public int LimiteCompra { get; set; }
        public string Rubro { get; set; }
        public Valoracion Valoracion = new Valoracion();
        public Usuario Usuario = new Usuario();



    }
}
