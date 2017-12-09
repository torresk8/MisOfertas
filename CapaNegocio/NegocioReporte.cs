
using CapaConexion;
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
        public void generarArchivoPlano()
        {
            try
            {

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter("C:\\Users\\Ariel\\Documents\\Test.csv");
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

                string texto = "";
                while (dr.Read())
                {
                    texto += String.Format("{0}", dr[0]) + " ,";
                    texto += String.Format("{0}", dr[1]) + " ,";
                    texto += dr.GetInt32(2) + " ,";
                    texto += String.Format("{0}", dr[3]) + " ,";
                    texto += String.Format("{0}", dr[4]) + " \n";

                }
                conn.Close();
                //Write a line of text
                sw.WriteLine(texto);
               

                //Close the file
                sw.Close();
               // MessageBox.Show("Archivo Descargado", "Mis oferta");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }


        public MemoryStream archivo()
        {
            var stream = new MemoryStream();
            try
            {

                conn.Open();

                OracleCommand cmd = new OracleCommand("SELECT u.nombreUsuario, r.nombre,o.idOferta, o.nombre AS OFERTA, l.fecha "+
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
                
                string texto = "";
                while (dr.Read())
                {                    
                    texto += String.Format("{0}", dr[0])+" ,";
                    texto += String.Format("{0}", dr[1]) + " ,";
                    texto += dr.GetInt32(2) + " ,";
                    texto += String.Format("{0}", dr[3]) + " ,";
                    texto += String.Format("{0}", dr[4]) + " \n";

                }
                conn.Close();
                stream = new MemoryStream(Encoding.ASCII.GetBytes(texto));
            }
            catch(Exception ex)
            {

            }

          

            return stream;
        }
    }
}
