using CapaConexion;
using CapaDTO;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class NegocioEmail
    {
        private Conexion conexion = new Conexion();
        OracleConnection conn;
        public NegocioEmail()
        {
            conn = conexion.conectar();
        }

        public bool insertarCorreo(Correo correo)
        {
            bool resultado = false;
            

            conn.Open();
            OracleCommand cmd = new OracleCommand("INSERT INTO correo (idCorreo,asunto,descripcion," +
                                                  "fecha,idUsuario)" +
                                                " VALUES(secuence_correo.nextval,:asunto,:descripcion,sysdate,:idUsuario)", conn);

            cmd.Parameters.Add(new OracleParameter(":asunto", correo.Asunto));
            cmd.Parameters.Add(new OracleParameter(":descripcion", correo.Descripcion));
            cmd.Parameters.Add(new OracleParameter(":idUsuario", correo.Usuario.IdUsuario));         

            int a = cmd.ExecuteNonQuery();

            conn.Close();
            if (a > 0)
            {
                resultado = true;
            }

            return resultado;

        }
    }
}
