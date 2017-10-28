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
            conn.Open();

            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand("SELECT * FROM valoracion where idValoracion=:id", conn);
            cmd.Parameters.Add(new OracleParameter(":id", id));

            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = cmd;
            OracleDataReader dr = cmd.ExecuteReader();

            //byte[] ima = (byte[])cmd.ExecuteScalar();          



            while (dr.Read())
            {


                valoracion.IdValoracion = dr.GetInt32(0);
                valoracion.Calificacion = String.Format("{0}", dr[1]);
                // valoracion.Boleta = String.Format("{0}", dr[3]);

                OracleBlob blob = dr.GetOracleBlob(2);
                Byte[] Buffer = (Byte[])(dr.GetOracleBlob(2)).Value;
                valoracion.Boleta = Buffer;

                valoracion.fecha = String.Format("{0}", dr[3]);
                valoracion.Oferta.IdOferta = dr.GetInt32(4);
                valoracion.Usuario.IdUsuario = dr.GetInt32(5);


            }

            conn.Close();

            return valoracion;
        }



        public bool insertarValoracion(Valoracion valoracion)
        {
            bool resultado = false;

            conn.Open();

            OracleCommand cmd = new OracleCommand("INSERT INTO valoracion (idValoraion, calificacion, boleta, " +
                "fecha, idOferta, idUsuario)" +
                     "VALUES(1, :calificacion, :boleta, :fecha," +
                     ":idOferta, :idUsuario)", conn);

            cmd.Parameters.Add(new OracleParameter(":calificacion", valoracion.Calificacion));
            cmd.Parameters.Add(new OracleParameter(":boleta ", valoracion.Boleta));
            cmd.Parameters.Add(new OracleParameter(":fecha", valoracion.fecha));
            cmd.Parameters.Add(new OracleParameter(":idOferta", valoracion.Oferta.IdOferta));
            cmd.Parameters.Add(new OracleParameter(":idUsuario", valoracion.Usuario.IdUsuario));

            int a = cmd.ExecuteNonQuery();
            conn.Close();
            if (a > 0)
            {
                resultado = true;
            }

            return resultado;

        }


        public bool eliminarValoracion(Valoracion valoracion)
        {
            bool resultado = false;

            conn.Open();
            OracleCommand cmd = new OracleCommand("DELETE from valoracion where valoracion =:idValoracion", conn);

            cmd.Parameters.Add(new OracleParameter(":idValoracion", valoracion.IdValoracion));

            int a = cmd.ExecuteNonQuery();
            conn.Close();
            if (a > 0)
            {
                resultado = true;
            }

            return resultado;

        }

        public List<Valoracion> retornaValoracionList()
        {
            List<Valoracion> list = new List<Valoracion>();

            conn.Open();

            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand("SELECT * FROM valoracion", conn);


            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = cmd;
            OracleDataReader dr = cmd.ExecuteReader();


            while (dr.Read())
            {

                Valoracion valoracion = new Valoracion();
                valoracion.IdValoracion = dr.GetInt32(0);
                valoracion.Calificacion = String.Format("{0}", dr[1]);

                OracleBlob blob = dr.GetOracleBlob(8);
                Byte[] Buffer = (Byte[])(dr.GetOracleBlob(8)).Value;
                valoracion.Boleta = Buffer;

                valoracion.fecha = String.Format("{0}", dr[2]);
                valoracion.Oferta.IdOferta = dr.GetInt32(3);
                valoracion.Usuario.IdUsuario = dr.GetInt32(4);

                list.Add(valoracion);
            }

            conn.Close();

            return list;
        }
    }
}
