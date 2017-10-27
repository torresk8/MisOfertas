using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDTO
{
    public class Provincia
    {
        public int IdProvincia { get; set; }
        public string Nombre { get; set; }

        public Region Region = new Region();

    }
}
