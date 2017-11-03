using CapaNegocio.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace MisOfertas.Seguridad
{
    public class PrincipalPersonalizado : IPrincipal
    {       

        public bool IsInRole(string role)
        {
            NegocioRol auxRol = new NegocioRol();
            return auxRol.IsUserRole(role);
        }

        public IIdentity Identity { get; private set; }

        public identityPersonalizado personalizado
        {
            get { return (identityPersonalizado)Identity; }
            set { Identity = value; }
        }

        public PrincipalPersonalizado(identityPersonalizado identity)
        {
            Identity = identity;                 
        }

    }
}