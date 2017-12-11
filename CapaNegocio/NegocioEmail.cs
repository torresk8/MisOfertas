using CapaConexion;
using CapaDTO;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
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
            try
            {

                OracleCommand cmd = new OracleCommand("ingreso_correo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("asunto_c", OracleDbType.Varchar2, ParameterDirection.Input).Value = correo.Asunto;
                cmd.Parameters.Add("descripcion_c", OracleDbType.Varchar2, ParameterDirection.Input).Value = correo.Descripcion;
                cmd.Parameters.Add("fecha_c", OracleDbType.Int32, ParameterDirection.Input).Value = correo.Fecha;
                cmd.Parameters.Add("idUsuario_c", OracleDbType.Int32, ParameterDirection.Input).Value = correo.IdCorreo;
             
                conn.Open();
                int a = cmd.ExecuteNonQuery();
                conn.Close();
                if (a == -1)
                {
                resultado = true;
            }
            }
            catch (Exception ex)
            {

            }
            return resultado;

        }
    }
}
