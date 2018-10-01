using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDTO
{
    public class ResumenTienda
    {
        public int IdValoracion { get; set; }

        public DateTime Fecha { get; set; }

        public Oferta oferta = new Oferta();

        public Usuario usuario = new Usuario();

        public string Calificacion { get; set; }

        public string comentario { get; set; }


    }
}
