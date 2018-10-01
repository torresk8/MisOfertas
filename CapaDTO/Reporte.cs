using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDTO
{
    public class Reporte
    {        
        public Sucursal sucursal = new Sucursal();       

        public int CantidadUsuario { get; set; }
        public int CantidadCorreo { get; set; }
        public int CantidadValoracion { get; set; }
        public int CantidadDescuento { get; set; }
        


    }
}
