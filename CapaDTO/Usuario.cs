using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDTO
{
    public class Usuario
    {
        private int idUsuario;
        [Required]
        private string rut;
        [Required]
        private string nombre;   
        [Required]
        private string nombreUsuario;
        [Required]
        private string password;
        [Required]
        private string confirmarPassword;
        [Required]
        private string direccion;
        private int telefono;
        private string correo;

        public Usuario()
        {

        }

        public string Rut { get => rut; set => rut = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string NombreUsuario { get => nombreUsuario; set => nombreUsuario = value; }
        public string Password { get => password; set => password = value; }
        public string ConfirmarPassword { get => confirmarPassword; set => confirmarPassword = value; }
        public string Direccion { get => direccion; set => direccion = value; }
        public int Telefono { get => telefono; set => telefono = value; }
        public int IdUsuario { get => idUsuario; set => idUsuario = value; }
        public string Correo { get => correo; set => correo = value; }
    }
}
