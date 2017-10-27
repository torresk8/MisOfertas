using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDTO
{
    public class Comuna
    {
        public int idComuna { get; set; }
        public string Nombre { get; set; }

        public Provincia Provincia = new Provincia();

    }
}
