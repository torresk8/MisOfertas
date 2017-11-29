using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDTO
{
    public class Valoracion
    {
        public int IdValoracion { get; set; }
        public string Calificacion { get; set; }
        public byte[] Boleta { get; set; }
        public string fecha { get; set; }
        public Oferta oferta = new Oferta();
        public Usuario usuario = new Usuario();

        

    }
}
