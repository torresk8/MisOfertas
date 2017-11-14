using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDTO
{
    public class Descuento
    {
        public int idDescuento { get; set; }
        public int cantidad { get; set; }
        public Rubro rubro = new Rubro();
        public Descuento()
        {

        }

    }
}
