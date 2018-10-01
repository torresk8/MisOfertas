
using CapaConexion;
using CapaDTO;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class NegocioReporte
    {

        private Conexion conexion = new Conexion();
        OracleConnection conn;
        public NegocioReporte()
        {
            conn = conexion.conectar();
        }
        public string generarArchivoPlano()
        {
            string texto = "";
            try
            {

                conn.Open();

                OracleCommand cmd = new OracleCommand("SELECT u.nombreUsuario, r.nombre,o.idOferta, o.nombre AS OFERTA, l.fecha " +
                                                       "FROM log_usuario l " +
                                                       "INNER JOIN usuario u ON u.idUsuario = l.idUsuario " +
                                                       "INNER JOIN rubro r ON r.idRubro = l.idRubro " +
                                                       "INNER JOIN oferta o ON o.idRubro = r.idRubro " +
                                                       "INNER JOIN producto p ON p.idProducto = o.idProducto " +
                                                       "WHERE o.estado = 'Publicado' " +
                                                       "ORDER BY fecha ASC ", conn);

                OracleDataAdapter da = new OracleDataAdapter();
                da.SelectCommand = cmd;
                OracleDataReader dr = cmd.ExecuteReader();


                while (dr.Read())
                {
                    texto += String.Format("{0}", dr[0]) + " ,";
                    texto += String.Format("{0}", dr[1]) + " ,";
                    texto += dr.GetInt32(2) + " ,";
                    texto += String.Format("{0}", dr[3]) + " ,";
                    texto += String.Format("{0}", dr[4]) + " \n";

                }
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
            return texto;
        }


        public MemoryStream archivo(string texto)
        {
            var stream = new MemoryStream();
            try
            {
                stream = new MemoryStream(Encoding.ASCII.GetBytes(texto));
            }
            catch (Exception ex)
            {

            }



            return stream;
        }

        public List<Reporte> retornaReporte()
        {
            List<Reporte> list = new List<Reporte>();
            try
            {
                conn.Open();
                
                OracleCommand cmd = new OracleCommand();
                cmd = new OracleCommand("SELECT  s.nombre,(select count(idUsuario) from usuario u WHERE  u.IDUSUARIO <> ALL (SELECT p.IDUSUARIO FROM permiso p)) AS CantidadUsuario,"+
                                        "(select count(idCorreo) from correo) as cantidadCorreo, COUNT(v.idValoracion) as CantidadValoracion,"+
                                        "(select count(d.idRubro) from descuento d) as CantidadDescuento "+
                                        "FROM oferta o "+
                                         "INNER JOIN sucursal s ON s.idSucursal = o.idSucursal "+
                                        "INNER JOIN valoracion v ON v.idOferta = o.idOferta "+
                                        "group by  s.nombre", conn);


                OracleDataAdapter da = new OracleDataAdapter();
                da.SelectCommand = cmd;
                OracleDataReader dr = cmd.ExecuteReader();


                while (dr.Read())
                {
                    Reporte reporte = new Reporte();

                    reporte.sucursal.Nombre = String.Format("{0}", dr[0]);
                    reporte.CantidadUsuario = dr.GetInt32(1);
                    reporte.CantidadCorreo = dr.GetInt32(2);
                    reporte.CantidadValoracion = dr.GetInt32(3);
                    reporte.CantidadDescuento = dr.GetInt32(4);

                    list.Add(reporte);
                }

                conn.Close();
            }
            catch (Exception ex)
            {

            }
            return list;
        }
    }
}
