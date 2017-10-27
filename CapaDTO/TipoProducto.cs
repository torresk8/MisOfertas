using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDTO
{
    public class TipoProducto
    {
        private int idTipoProducto;
        private string nombre;

        public TipoProducto()
        {

        }

        public int IdTipoProducto { get => idTipoProducto; set => idTipoProducto = value; }
        public string Nombre { get => nombre; set => nombre = value; }
    }
}
