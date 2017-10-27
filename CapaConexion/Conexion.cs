using CapaDTO;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaConexion
{
    public class Conexion
    {
        private string cadenaConexion;
        public Conexion()
        {
            cadenaConexion = "DATA SOURCE=localhost:1521/xe;USER ID=OFERTAS ; Password= 141516sk8";
        }

        public OracleConnection conectar()
        {
            OracleConnection conn = new OracleConnection(cadenaConexion);
            return conn;
        }

    }
}
