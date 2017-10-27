using CapaConexion;
using CapaDTO;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class NegocioOferta
    {
        private Conexion conexion = new Conexion();
        OracleConnection conn;
        public NegocioOferta()
        {
            conn = conexion.conectar();
        }

        public Oferta retornaOferta(int id)
        {
            Oferta oferta = new Oferta(); 
            conn.Open();

            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand("SELECT * FROM oferta where idOferta=:id", conn);
            cmd.Parameters.Add(new OracleParameter(":id", id));

            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = cmd;
            OracleDataReader dr = cmd.ExecuteReader();

            //byte[] ima = (byte[])cmd.ExecuteScalar();          
            
            

            while (dr.Read())
            {
                

                oferta.IdOferta = dr.GetInt32(0);
                oferta.Nombre = String.Format("{0}", dr[1]);
                oferta.Descripcion = String.Format("{0}", dr[2]);
                oferta.PrecioNormal = dr.GetInt32(3);
                oferta.PrecioOfeta = dr.GetInt32(4);
                oferta.CantidadMin = dr.GetInt32(5);
                oferta.CantidadMax = dr.GetInt32(6);
                oferta.Producto.IdProducto = dr.GetInt32(7);

                OracleBlob blob = dr.GetOracleBlob(8);
                Byte[] Buffer = (Byte[])(dr.GetOracleBlob(8)).Value;
                oferta.Imagen = Buffer;
                oferta.Estado = String.Format("{0}", dr[9]);
            }

            conn.Close();

            return oferta;
        }

        public List<Oferta> retornaOfertaPuublicadaList()
        {
            List<Oferta> list = new List<Oferta>();

            conn.Open();

            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand("SELECT * FROM oferta where estado = :estado", conn);

            cmd.Parameters.Add(":estado", "Publicado");

            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = cmd;
            OracleDataReader dr = cmd.ExecuteReader();


            while (dr.Read())
            {

                Oferta oferta = new Oferta();
                oferta.IdOferta = dr.GetInt32(0);
                oferta.Nombre = String.Format("{0}", dr[1]);
                oferta.Descripcion = String.Format("{0}", dr[2]);
                oferta.PrecioNormal = dr.GetInt32(3);
                oferta.PrecioOfeta = dr.GetInt32(4);
                oferta.CantidadMin = dr.GetInt32(5);
                oferta.CantidadMax = dr.GetInt32(6);

                OracleBlob blob = dr.GetOracleBlob(8);
                Byte[] Buffer = (Byte[])(dr.GetOracleBlob(8)).Value;
                oferta.Imagen = Buffer;
                oferta.Estado = String.Format("{0}", dr[9]);
                list.Add(oferta);
            }

            conn.Close();

            return list;
        }
        public bool insertarOferta(Oferta oferta)
        {
            bool resultado = false;
            oferta.Estado = "No publicado";

            conn.Open();

            OracleCommand cmd = new OracleCommand("INSERT INTO oferta (idOferta, nombre, descripcion, " +
                "precioNormal, precioOferta, cantidadMin, cantidadMax, idProducto, productImage,estado)" +
                     "VALUES(sucuence_oferta.NEXTVAL, :nombre, :descripcion, :precioNormal," +
                     ":precioOferta, :cantidadMin, :cantidadMax, :idProducto, :productImage, :estado)", conn);

            cmd.Parameters.Add(new OracleParameter(":nombre", oferta.Nombre));
            cmd.Parameters.Add(new OracleParameter(":descripcion", oferta.Descripcion));
            cmd.Parameters.Add(new OracleParameter(":precioNormal", oferta.PrecioNormal));
            cmd.Parameters.Add(new OracleParameter(":precioOferta", oferta.PrecioOfeta));
            cmd.Parameters.Add(new OracleParameter(":cantidadMin", oferta.CantidadMin));
            cmd.Parameters.Add(new OracleParameter(":cantidadMax", oferta.CantidadMax));
            cmd.Parameters.Add(new OracleParameter(":idProducto", oferta.Producto.IdProducto));
            cmd.Parameters.Add(new OracleParameter(":productImage", oferta.Imagen));
            cmd.Parameters.Add(new OracleParameter(":estado", oferta.Estado));

            int a = cmd.ExecuteNonQuery();
            conn.Close();
            if (a > 0)
            {
                resultado = true;
            }

            return resultado;

        }


        public bool eliminarOferta(Oferta oferta)
        {
            bool resultado = false;

            conn.Open();
            OracleCommand cmd = new OracleCommand("DELETE from oferta where oferta =:idOferta", conn);

            cmd.Parameters.Add(new OracleParameter(":idOferta", oferta.IdOferta));

            int a = cmd.ExecuteNonQuery();
            conn.Close();
            if (a > 0)
            {
                resultado = true;
            }

            return resultado;

        }

        public List<Oferta> retornaOfertaList()
        {
            List<Oferta> list = new List<Oferta>();
            
            conn.Open();

            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand("SELECT * FROM oferta", conn);
            

            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = cmd;
            OracleDataReader dr = cmd.ExecuteReader();


             while (dr.Read())
             {

                Oferta oferta = new Oferta();
                oferta.IdOferta = dr.GetInt32(0);
                 oferta.Nombre = String.Format("{0}", dr[1]);
                 oferta.Descripcion = String.Format("{0}", dr[2]);
                 oferta.PrecioNormal = dr.GetInt32(3);
                 oferta.PrecioOfeta = dr.GetInt32(4);
                 oferta.CantidadMin = dr.GetInt32(5);
                 oferta.CantidadMax = dr.GetInt32(6);

                 OracleBlob blob = dr.GetOracleBlob(8);
                 Byte[] Buffer = (Byte[])(dr.GetOracleBlob(8)).Value;
                 oferta.Imagen = Buffer;
                 oferta.Estado = String.Format("{0}", dr[9]);                 
                 list.Add(oferta);
             }

            conn.Close();

            return list;
        }

        public bool actualizarOferta(string estado,int id)
        {
            bool resultado = false;

            conn.Open();
            OracleCommand cmd = new OracleCommand("UPDATE oferta SET estado = :estado WHERE idOferta =:idOferta", conn);

            cmd.Parameters.Add(new OracleParameter(":estado", estado));
            cmd.Parameters.Add(new OracleParameter(":idOferta", id));

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
