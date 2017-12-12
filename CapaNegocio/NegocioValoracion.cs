using CapaConexion;
using CapaDTO;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class NegocioValoracion
    {


        private Conexion conexion = new Conexion();
        OracleConnection conn;
        public NegocioValoracion()
        {
            conn = conexion.conectar();
        }

        public Valoracion retornaValoracion(int id)
        {
            Valoracion valoracion = new Valoracion();
            try
            {

                conn.Open();

                DataSet ds = new DataSet();
                OracleCommand cmd = new OracleCommand();
                cmd = new OracleCommand("SELECT * FROM valoracion where idOferta=:id", conn);
                cmd.Parameters.Add(new OracleParameter(":id", id));

                OracleDataAdapter da = new OracleDataAdapter();
                da.SelectCommand = cmd;
                OracleDataReader dr = cmd.ExecuteReader();

                //byte[] ima = (byte[])cmd.ExecuteScalar();          



                while (dr.Read())
                {


                    valoracion.IdValoracion = dr.GetInt32(0);
                    valoracion.Calificacion = String.Format("{0}", dr[5]);
                    // valoracion.Boleta = String.Format("{0}", dr[3]);

                    OracleBlob blob = dr.GetOracleBlob(1);
                    Byte[] Buffer = (Byte[])(dr.GetOracleBlob(1)).Value;
                    valoracion.Boleta = Buffer;

                    valoracion.fecha = String.Format("{0}", dr[2]);
                    valoracion.oferta.IdOferta = dr.GetInt32(3);
                    valoracion.usuario.IdUsuario = dr.GetInt32(4);
                    valoracion.Comentario = String.Format("{0}", dr[6]);


                }

                conn.Close();
            }
            catch (Exception ex)
            {

            }

            return valoracion;
        }



        public bool insertarValoracion(Valoracion valoracion)
        {
            bool resultado = false;
            try
            {

                conn.Open();

                OracleCommand cmd = new OracleCommand("INSERT INTO valoracion(idValoracion, calificacion, boleta, " +
                    "fecha, idOferta, idUsuario,comentario)" +
                         "VALUES(sucuence_valoracion.NEXTVAL, :calificacion, :boleta, sysdate," +
                         ":idOferta, :idUsuario,:comentario)", conn);

                cmd.Parameters.Add(new OracleParameter(":calificacion", valoracion.Calificacion));
                cmd.Parameters.Add(new OracleParameter(":boleta ", valoracion.Boleta));
                cmd.Parameters.Add(new OracleParameter(":idOferta", valoracion.oferta.IdOferta));
                cmd.Parameters.Add(new OracleParameter(":idUsuario", valoracion.usuario.IdUsuario));
                cmd.Parameters.Add(new OracleParameter(":comentario", valoracion.Comentario));

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


        public bool eliminarValoracion(Valoracion valoracion)
        {
            bool resultado = false;
            try
            {

                conn.Open();
                OracleCommand cmd = new OracleCommand("DELETE from valoracion where valoracion =:idValoracion", conn);

                cmd.Parameters.Add(new OracleParameter(":idValoracion", valoracion.IdValoracion));

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

        public List<Valoracion> retornaValoracionList(int id)
        {
            List<Valoracion> list = new List<Valoracion>();
            try
            {

                conn.Open();

                DataSet ds = new DataSet();
                OracleCommand cmd = new OracleCommand();
                cmd = new OracleCommand("SELECT v.idValoracion,v.boleta,v.fecha,o.nombre,u.nombreUsuario,v.calificacion,v.comentario,v.idOferta, u.nombre " +
                                        "FROM valoracion v " +
                                        "INNER JOIN usuario u ON u.idUsuario = v.idUsuario " +
                                        " INNER JOIN oferta o ON v.idOferta = o.idOferta " +
                                        "WHERE v.IdOferta = :idOferta", conn);

                cmd.Parameters.Add(":idOferta", id);

                OracleDataAdapter da = new OracleDataAdapter();
                da.SelectCommand = cmd;
                OracleDataReader dr = cmd.ExecuteReader();


                while (dr.Read())
                {

                    Valoracion valoracion = new Valoracion();

                    valoracion.IdValoracion = dr.GetInt32(0);
                    //carga imagen
                    OracleBlob blob = dr.GetOracleBlob(1);
                    Byte[] Buffer = (Byte[])(dr.GetOracleBlob(1)).Value;
                    valoracion.Boleta = Buffer;

                    valoracion.fecha = String.Format("{0:dd/MM/yyyy}", dr[2]);
                    valoracion.oferta.Nombre = String.Format("{0}", dr[3]);
                    valoracion.usuario.NombreUsuario = String.Format("{0}", dr[4]);
                    valoracion.Calificacion = String.Format("{0}", dr[5]);
                    valoracion.Comentario = String.Format("{0}", dr[6]);
                    valoracion.oferta.IdOferta = dr.GetInt32(7);
                    valoracion.usuario.Nombre = String.Format("{0}", dr[8]);

                    list.Add(valoracion);
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
