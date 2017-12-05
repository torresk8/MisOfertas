using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDTO
{
    public class LogUsuario
    {
        public int idLogUsuario { get; set; }

        public Usuario usuario = new Usuario();

        public Rubro rubro = new Rubro();

        public DateTime Fecha { get; set; }
    }
}
