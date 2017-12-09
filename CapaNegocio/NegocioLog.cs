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
    public class NegocioLog
    {
        private Conexion conexion = new Conexion();
        OracleConnection conn;

        public NegocioLog()
        {
            conn = conexion.conectar();
        }

        public bool insertarLog(LogUsuario logUsuario)
        {
            bool resultado = false;

            try
            {            

            conn.Open();
            OracleCommand cmd = new OracleCommand("INSERT INTO log_usuario(idLogUsuario,idUsuario,idRubro,fecha) " +
                                                " VALUES(sucuence_log_usuario.nextval, :idUsuario, :idRubro, SYSDATE)", conn);

            cmd.Parameters.Add(new OracleParameter(":idUsuario", logUsuario.usuario.IdUsuario));
            cmd.Parameters.Add(new OracleParameter(":idRubro", logUsuario.rubro.IdRubro));            

            int a = cmd.ExecuteNonQuery();

            conn.Close();
            if (a > 0)
            {
                resultado = true;
            }
            }
            catch (Exception ex)
            {

            }
            return resultado;

        }

        public LogUsuario retornaLogUsuario(int idUsuario)
        {
            LogUsuario logUsuario = new LogUsuario();
            try
            {
            
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand("SELECT l.idLogUsuario,l.idUsuario, l.idRubro "+
                                     "FROM log_usuario l "+
                                      "INNER JOIN usuario u ON u.idUsuario = l.idUsuario "+
                                      "WHERE l.idUsuario = :idUsuario "+
                                      "ORDER BY l.idLogUsuario DESC ", conn);


            cmd.Parameters.Add(new OracleParameter(":idUsuario", idUsuario));

            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = cmd;
            OracleDataReader dr = cmd.ExecuteReader();

            

            while (dr.Read())
            {

                logUsuario.idLogUsuario = dr.GetInt32(0);
                logUsuario.usuario.IdUsuario = dr.GetInt32(1);
                logUsuario.rubro.IdRubro = dr.GetInt32(2);

                break;
            }

            conn.Close();
            }
            catch (Exception ex)
            {

            }
            return logUsuario;
        }
    }
}
