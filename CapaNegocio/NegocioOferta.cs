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
            try
            {
            
            conn.Open();

            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand("SELECT o.idOferta, o.nombre, o.descripcion, o.precioNormal, o.precioOferta, " +
                                    "o.cantidadMin, o.CantidadMax, o.idProducto, o.productImage, o.estado, " +
                                    "o.idRubro,o.idSucursal, r.nombre, s.nombre , p.nombre " +
                                    "FROM oferta o " +
                                    "INNER JOIN rubro r ON r.idRubro = o.idRubro " +
                                    "INNER JOIN sucursal s ON s.idSucursal = o.idSucursal " +
                                    "INNER JOIN producto p ON p.idProducto = o.idProducto " +
                                    "WHERE o.idOferta=:id", conn);
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
                oferta.rubro.IdRubro = dr.GetInt32(10);
                oferta.sucursal.IdSucursal = dr.GetInt32(11);

                oferta.rubro.Nombre = String.Format("{0}", dr[12]);
                oferta.sucursal.Nombre = String.Format("{0}", dr[13]);
                oferta.Producto.Nombre = String.Format("{0}", dr[14]);
            }

            conn.Close();
            }
            catch (Exception ex)
            {

            }
            return oferta;
        }

        public Rubro retornaRubro(int id)
        {
            Rubro rubro = new Rubro();
            try
            {
            
            conn.Open();

            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand("SELECT * FROM rubro where idRubro=:id", conn);
            cmd.Parameters.Add(new OracleParameter(":id", id));

            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = cmd;
            OracleDataReader dr = cmd.ExecuteReader();

            //byte[] ima = (byte[])cmd.ExecuteScalar();          



            while (dr.Read())
            {

                rubro.IdRubro= dr.GetInt32(0);
                rubro.Nombre = String.Format("{0}", dr[1]);              
            }

            conn.Close();
            }
            catch (Exception ex)
            {

            }

            return rubro;
        }

        public List<Rubro> retornaRubroList()
        {
            List<Rubro> auxRubro = new List<Rubro>();
            try
            {
            
            conn.Open();
            
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand("SELECT * FROM rubro ", conn);
            
            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = cmd;
            OracleDataReader dr = cmd.ExecuteReader();

            //byte[] ima = (byte[])cmd.ExecuteScalar();          



            while (dr.Read())
            {
                Rubro rubro = new Rubro();

                rubro.IdRubro = dr.GetInt32(0);
                rubro.Nombre = String.Format("{0}", dr[1]);

                auxRubro.Add(rubro);
            }

            conn.Close();
            }
            catch (Exception ex)
            {

            }
            return auxRubro;
        }

        public List<Oferta> retornaOfertaPuublicadaList(int idRubro, int idSucursal)
        {
            List<Oferta> list = new List<Oferta>();
            try
            {
            
            conn.Open();

            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand();

            if(idRubro>0)
            {
                cmd = new OracleCommand("SELECT o.idOferta, o.nombre, o.descripcion, o.precioNormal,o.precioOferta, o.cantidadMin, o.cantidadMax, " +
                                    "o.idProducto, o.productImage, o.estado, o.idRubro, s.idSucursal, s.nombre FROM oferta o " +
                                    " INNER JOIN sucursal s ON s.idSucursal = o.idSucursal " +
                                    "where estado = :estado AND idRubro = :idRubro", conn);

                cmd.Parameters.Add(":estado", "Publicado");
                cmd.Parameters.Add(":idRubro", idRubro);
            }
            else
            {
                cmd = new OracleCommand("SELECT o.idOferta, o.nombre, o.descripcion, o.precioNormal,o.precioOferta, o.cantidadMin, o.cantidadMax, " +
                                    "o.idProducto, o.productImage, o.estado, o.idRubro, s.idSucursal, s.nombre FROM oferta o " +
                                    " INNER JOIN sucursal s ON s.idSucursal = o.idSucursal " +
                                    "where estado = :estado AND s.idSucursal = :idSucursal", conn);

                cmd.Parameters.Add(":estado", "Publicado");
                cmd.Parameters.Add(":idSucursal", idSucursal);
            }

            
            
            

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
                oferta.Producto.IdProducto = dr.GetInt32(7);

                OracleBlob blob = dr.GetOracleBlob(8);
                Byte[] Buffer = (Byte[])(dr.GetOracleBlob(8)).Value;
                oferta.Imagen = Buffer;
                oferta.Estado = String.Format("{0}", dr[9]);
                oferta.rubro.IdRubro = dr.GetInt32(10);
                oferta.sucursal.IdSucursal = dr.GetInt32(11);
                oferta.sucursal.Nombre = String.Format("{0}", dr[12]);

                list.Add(oferta);
            }

            conn.Close();
            }
            catch (Exception ex)
            {

            }
            return list;
        }


        public List<Oferta> retornaOfertaPublicadaListPrecioMenor(int precioInicio, int precioFin, string order)
        {
            List<Oferta> list = new List<Oferta>();

            try
            {

                conn.Open();

                DataSet ds = new DataSet();
                OracleCommand cmd = new OracleCommand();
                if (order != "")
                {
                    cmd = new OracleCommand("SELECT o.idOferta, o.nombre, o.descripcion, o.precioNormal,o.precioOferta, o.cantidadMin, o.cantidadMax, " +
                                            "o.idProducto, o.productImage, o.estado, o.idRubro, s.idSucursal, s.nombre FROM oferta o " +
                                            "INNER JOIN sucursal s ON s.idSucursal = o.idSucursal " +
                                            "where estado = :estado ORDER BY precioOferta " + order + "", conn);

                    cmd.Parameters.Add(":estado", "Publicado");
                    //cmd.Parameters.Add(":order", order);
                }
                else
                {
                    cmd = new OracleCommand("SELECT o.idOferta, o.nombre, o.descripcion, o.precioNormal, o.precioOferta, o.cantidadMin, o.cantidadMax, " +
                                            " o.idProducto, o.productImage, o.estado, o.idRubro, s.idSucursal, s.nombre FROM oferta o " +
                                            "INNER JOIN sucursal s ON s.idSucursal = o.idSucursal " +
                                            "where estado = :estado AND precioOferta > :precioInicio AND precioOferta < :precioFin", conn);
                    cmd.Parameters.Add(":estado", "Publicado");
                    cmd.Parameters.Add(":precioInicio", precioInicio);
                    cmd.Parameters.Add(":precioFin", precioFin);
                }



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
                    oferta.Producto.IdProducto = dr.GetInt32(7);

                    OracleBlob blob = dr.GetOracleBlob(8);
                    Byte[] Buffer = (Byte[])(dr.GetOracleBlob(8)).Value;
                    oferta.Imagen = Buffer;
                    oferta.Estado = String.Format("{0}", dr[9]);
                    oferta.rubro.IdRubro = dr.GetInt32(10);
                    oferta.sucursal.IdSucursal = dr.GetInt32(11);
                    oferta.sucursal.Nombre = String.Format("{0}", dr[12]);

                    list.Add(oferta);
                }

                conn.Close();
            }catch(Exception ex)
            {

            }

            return list;
        }


        public bool insertarOferta(Oferta oferta)
        {
            bool resultado = false;
            try
            {         
            

                OracleCommand cmd = new OracleCommand("create_oferta", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("nombre_o", OracleDbType.Varchar2,ParameterDirection.Input).Value= oferta.Nombre;
                cmd.Parameters.Add("descrip", OracleDbType.Varchar2, ParameterDirection.Input).Value = oferta.Descripcion;
                cmd.Parameters.Add("precioN", OracleDbType.Int32, ParameterDirection.Input).Value = oferta.PrecioNormal;
                cmd.Parameters.Add("precioO", OracleDbType.Int32, ParameterDirection.Input).Value = oferta.PrecioOfeta;
                cmd.Parameters.Add("cantidadMi", OracleDbType.Int32, ParameterDirection.Input).Value = oferta.CantidadMin;
                cmd.Parameters.Add("cantidadMa", OracleDbType.Int32, ParameterDirection.Input).Value = oferta.CantidadMax;
                cmd.Parameters.Add("imagen", OracleDbType.Blob, ParameterDirection.Input).Value = oferta.Imagen;
                cmd.Parameters.Add("estad", OracleDbType.Varchar2, ParameterDirection.Input).Value = oferta.Estado = "No publicado"; 
                cmd.Parameters.Add("rubro_o", OracleDbType.Int32, ParameterDirection.Input).Value = oferta.rubro.IdRubro;
                cmd.Parameters.Add("idSu", OracleDbType.Int32, ParameterDirection.Input).Value = oferta.sucursal.IdSucursal;
               

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


        public bool eliminarOferta(int id)
        {
            bool resultado = false;
            try
            {

                conn.Open();
                OracleCommand cmd = new OracleCommand(" delete_oferta", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("idOferta_o", OracleDbType.Int32, ParameterDirection.Input).Value = id;

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

        public List<Oferta> retornaOfertaList()
        {
            List<Oferta> list = new List<Oferta>();
            try
            {
            
            conn.Open();

            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand("SELECT o.idOferta,r.nombre,o.nombre,o.descripcion," +
                "o.precioNormal,o.precioOferta,o.cantidadMin,o.cantidadMax, o.idProducto, o.productImage" +
                ", o.estado, s.nombre as sucursal "+
                "FROM OFERTA O "+
                "INNER JOIN RUBRO r ON r.idRubro = o.IdRubro "+
                "INNER JOIN PRODUCTO p ON p.idProducto = o.idProducto " +
                "INNER JOIN SUCURSAL s ON o.idSucursal = s.idSucursal", conn);
            

            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = cmd;
            OracleDataReader dr = cmd.ExecuteReader();


             while (dr.Read())
             {

                Oferta oferta = new Oferta();
                oferta.IdOferta = dr.GetInt32(0);
                oferta.rubro.Nombre = String.Format("{0}", dr[1]);
                oferta.Nombre = String.Format("{0}", dr[2]);
                 oferta.Descripcion = String.Format("{0}", dr[3]);
                 oferta.PrecioNormal = dr.GetInt32(4);
                 oferta.PrecioOfeta = dr.GetInt32(5);
                 oferta.CantidadMin = dr.GetInt32(6);
                 oferta.CantidadMax = dr.GetInt32(7);
                oferta.Producto.IdProducto = dr.GetInt32(8);
                OracleBlob blob = dr.GetOracleBlob(9);
                 Byte[] Buffer = (Byte[])(dr.GetOracleBlob(9)).Value;
                 oferta.Imagen = Buffer;
                 oferta.Estado = String.Format("{0}", dr[10]);   
                oferta.sucursal.Nombre = String.Format("{0}", dr[11]);

                list.Add(oferta);
             }

            conn.Close();
            }
            catch (Exception ex)
            {

            }

            return list;
        }

        public bool actualizarOfertaEstado(string estado,int id)
        {
            bool resultado = false;
            try
            {

                
                OracleCommand cmd = new OracleCommand(" update_estado_oferta", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("idOferta_o", OracleDbType.Int32, ParameterDirection.Input).Value = id;
                cmd.Parameters.Add("estado_o", OracleDbType.Int32, ParameterDirection.Input).Value = estado;
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

        public bool actualizarOferta(Oferta oferta)
        {
            bool resultado = false;
            try
            {

                OracleCommand cmd = new OracleCommand(" update_oferta", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("idOferta_o", OracleDbType.Int32, ParameterDirection.Input).Value = oferta.IdOferta;
                cmd.Parameters.Add("nombre_o", OracleDbType.Int32, ParameterDirection.Input).Value = oferta.Nombre;
                cmd.Parameters.Add("descripcion_o", OracleDbType.Int32, ParameterDirection.Input).Value = oferta.Descripcion;
                cmd.Parameters.Add("precioNormal_o", OracleDbType.Int32, ParameterDirection.Input).Value = oferta.PrecioNormal;
                cmd.Parameters.Add("precioOferta_o", OracleDbType.Int32, ParameterDirection.Input).Value = oferta.PrecioOfeta;
                cmd.Parameters.Add("cantidadMin_o", OracleDbType.Int32, ParameterDirection.Input).Value = oferta.CantidadMin;
                cmd.Parameters.Add("cantidadMax_o", OracleDbType.Int32, ParameterDirection.Input).Value = oferta.CantidadMax;
                cmd.Parameters.Add("idProducto_p", OracleDbType.Int32, ParameterDirection.Input).Value = oferta.Producto.IdProducto;
                cmd.Parameters.Add("idRubro_o", OracleDbType.Int32, ParameterDirection.Input).Value = oferta.rubro.IdRubro;
                cmd.Parameters.Add("idSucursal_o", OracleDbType.Int32, ParameterDirection.Input).Value = oferta.sucursal.IdSucursal;
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
