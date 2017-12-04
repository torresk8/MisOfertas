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
    public class NegocioCupon
    {
        private Conexion conexion = new Conexion();
        OracleConnection conn;
        public NegocioCupon()
        {
            conn = conexion.conectar();
        }

        public bool insertarPuntaje(int id)
        {
            bool resultado = false;            

            conn.Open();
            OracleCommand cmd = new OracleCommand("INSERT INTO PUNTAJE(idPuntaje,cantidad,idUsuario)" +
                     "VALUES(secuence_cupon.NEXTVAL,10,:idUsuario)", conn);

            cmd.Parameters.Add(new OracleParameter(":idUsuario", id));            

            int a = cmd.ExecuteNonQuery();
            conn.Close();
            if (a > 0)
            {
                resultado = true;
            }

            return resultado;

        }

        public Puntaje retornaPuntaje(int id)
        {
            Puntaje auxPuntaje = new Puntaje();

            conn.Open();

            OracleCommand cmd = new OracleCommand("SELECT SUM(cantidad) as Puntaje FROM puntaje " +
                                                  "WHERE idUsuario = :idUsuario", conn);

            cmd.Parameters.Add("idUsuario", id);

            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = cmd;
            OracleDataReader dr = cmd.ExecuteReader();


            while (dr.Read())
            {
                if(String.Format("{0}", dr[0]) == "")
                {
                    auxPuntaje.Cantidad = 0;
                }else
                {
                    auxPuntaje.Cantidad = dr.GetInt32(0);
                }

                

            }

            conn.Close();

            return auxPuntaje;
        }

        public bool limpiarPuntaje(int id)
        {
            bool resultado = false;

            conn.Open();
            OracleCommand cmd = new OracleCommand("DELETE FROM puntaje WHERE idUsuario =:idUsuario", conn);

            cmd.Parameters.Add(new OracleParameter(":idUsuario", id));

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
