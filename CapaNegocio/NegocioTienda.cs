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
    public class NegocioTienda
    {
        private Conexion conexion = new Conexion();
        OracleConnection conn;

        public NegocioTienda()
        {
            conn = conexion.conectar();
        }

        public List<ResumenTienda> retornaResumenTiendaList(int idSucural)
        {
            List<ResumenTienda> listaResumen = new List<ResumenTienda>();

            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand("select v.idValoracion, v.fecha, o.nombre AS Oferta, " +
                                    " u.nombreUsuario AS Usuario, v.calificacion, v.comentario,  o.idOferta,u.idUsuario " +
                                     "FROM valoracion v " +
                                     "INNER JOIN oferta o ON o.idOferta = v.idOferta " +
                                     "INNER JOIN usuario u ON u.idUsuario = v.idUsuario " +
                                     "WHERE o.idSucursal = :idSucursal", conn);


            cmd.Parameters.Add(new OracleParameter(":idSucursal", idSucural));

            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = cmd;
            OracleDataReader dr = cmd.ExecuteReader();



            while (dr.Read())
            {
                ResumenTienda resumen = new ResumenTienda();

                resumen.IdValoracion = dr.GetInt32(0);
                resumen.Fecha = Convert.ToDateTime(String.Format("{0}", dr[1]));
                resumen.oferta.Nombre = String.Format("{0}", dr[2]);
                resumen.usuario.NombreUsuario = String.Format("{0}", dr[3]);
                resumen.Calificacion = String.Format("{0}", dr[4]);
                resumen.comentario = String.Format("{0}", dr[5]);
                resumen.oferta.IdOferta = dr.GetInt32(6);
                resumen.usuario.IdUsuario = dr.GetInt32(7);


                listaResumen.Add(resumen);
            }

            conn.Close();

            return listaResumen;
        }
    }
}
