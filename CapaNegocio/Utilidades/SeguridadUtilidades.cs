using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Utilidades
{
    public class SeguridadUtilidades
    {
        public static String getSha1(String texto)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();            

            byte[] inputBytes = (new UnicodeEncoding()).GetBytes(texto);
            byte[] hash = sha1.ComputeHash(inputBytes);            

            return Convert.ToBase64String(hash);

        }
    }
}
