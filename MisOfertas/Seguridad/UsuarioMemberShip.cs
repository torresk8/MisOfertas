using CapaDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MisOfertas.Seguridad
{
    public class UsuarioMemberShip : MembershipUser
    {
        private int idUsuario;
        private string nombre;
        private string nombreUsuario;
        private string password;        

        public int IdUsuario { get => idUsuario; set => idUsuario = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string NombreUsuario { get => nombreUsuario; set => nombreUsuario = value; }
        public string Password { get => password; set => password = value; }        

        public UsuarioMemberShip(Usuario usuario)
        {
            idUsuario = usuario.IdUsuario;
            nombre = usuario.Nombre;
            nombreUsuario = usuario.NombreUsuario;
            password = usuario.Password;            
        }
    }
}