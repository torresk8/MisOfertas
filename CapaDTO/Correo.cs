using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDTO
{
    public class Correo
    {
        public int IdCorreo { get; set; }
        public string Asunto { get; set; }
        public string Descripcion { get; set; }
        public string Fecha { get; set; }        
        
        public Usuario Usuario = new Usuario();

    }
}
