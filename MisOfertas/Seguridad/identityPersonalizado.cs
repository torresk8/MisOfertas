using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace MisOfertas.Seguridad
{
    public class identityPersonalizado : IIdentity
    {
        public string Name
        {
            get { return nombreUsuario; }
        }

        public int IdUsuario
        {
            get { return idUsuario; }
        }
        public string AuthenticationType
        {
            get { return Identity.AuthenticationType; }
        }

        public bool IsAuthenticated
        {
            get { return Identity.IsAuthenticated; }
        }

        public int idUsuario { get; set; }
        public string nombre { get; set; }
        public string nombreUsuario { get; set; }
        public string password { get; set; }       
        public IIdentity Identity { get; set; }

        public identityPersonalizado(IIdentity identity)
        {
            this.Identity = identity;
            var us = Membership.GetUser(identity.Name) as UsuarioMemberShip;

            idUsuario = us.IdUsuario;
            nombre = us.Nombre;
            nombreUsuario = us.NombreUsuario;
            password = us.Password;            


        }

    }
}