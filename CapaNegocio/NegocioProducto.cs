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
    public class NegocioProducto
    {
        private Conexion conexion = new Conexion();
        OracleConnection conn;
        public NegocioProducto()
        {
            conn = conexion.conectar();
        }

        public Producto retornaProducto(int id)
        {
            Producto producto = new Producto();
            conn.Open();

            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand("SELECT * FROM producto where idProducto=:id", conn);
            cmd.Parameters.Add(new OracleParameter(":id", id));

            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = cmd;
            OracleDataReader dr = cmd.ExecuteReader();     

            
            while (dr.Read())
            {

                producto.IdProducto = dr.GetInt32(0);
                producto.Nombre = String.Format("{0}", dr[1]);
                producto.Descripcion = String.Format("{0}", dr[2]);
                producto.Precio = dr.GetInt32(3);
                producto.TipoProducto.IdTipoProducto = dr.GetInt32(4);                

           }

            conn.Close();

            return producto;
        }

        public List<TipoProducto> retornaTipoProducto()
        {
            List<TipoProducto> list = new List<TipoProducto>();
            conn.Open();

            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand("SELECT * FROM tipoProducto", conn);
            

            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = cmd;
            OracleDataReader dr = cmd.ExecuteReader();


            while (dr.Read())
            {
                TipoProducto tipoProducto = new TipoProducto();
                tipoProducto.IdTipoProducto = dr.GetInt32(0);
                tipoProducto.Nombre = String.Format("{0}", dr[1]);                
                list.Add(tipoProducto);
            }

            conn.Close();

            return list;
        }

        public List<Producto> retornaProductoList()
        {
            List<Producto> list = new List<Producto>();
            conn.Open();

            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand("SELECT * FROM producto", conn);


            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = cmd;
            OracleDataReader dr = cmd.ExecuteReader();


            while (dr.Read())
            {
                Producto Producto = new Producto();
                Producto.IdProducto = dr.GetInt32(0);
                Producto.Nombre = String.Format("{0}", dr[1]);
                Producto.Descripcion = String.Format("{0}", dr[2]);
                Producto.Precio = dr.GetInt32(3);
                Producto.TipoProducto.IdTipoProducto = dr.GetInt32(4);

                list.Add(Producto);
            }

            conn.Close();

            return list;
        }

        public List<Sucursal> retornaSucursal()
        {
            List<Sucursal> list = new List<Sucursal>();
            conn.Open();

            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand("SELECT * FROM sucursal", conn);


            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = cmd;
            OracleDataReader dr = cmd.ExecuteReader();


            while (dr.Read())
            {
                Sucursal sucursal = new Sucursal();

                sucursal.IdSucursal = dr.GetInt32(0);
                sucursal.Nombre = String.Format("{0}", dr[1]);
                sucursal.Direccion = String.Format("{0}", dr[2]);
                sucursal.Telefono = dr.GetInt32(3);
                sucursal.Comuna.idComuna = dr.GetInt32(4);

                list.Add(sucursal);
            }

            conn.Close();

            return list;
        }

        public bool insertarProducto(Producto producto)
        {
            bool resultado = false;

            conn.Open();

            OracleCommand cmd = new OracleCommand("INSERT INTO producto(idProducto,nombre," +
                "descripcion,precio,idTipoProducto) VALUES (sucuence_producto.NEXTVAL," +
                ":nombre,:descripcion,:precio,:idTipoP)", conn);

            cmd.Parameters.Add(new OracleParameter(":nombre", producto.Nombre));
            cmd.Parameters.Add(new OracleParameter(":descripcion", producto.Descripcion));
            cmd.Parameters.Add(new OracleParameter(":precio", producto.Precio));
            cmd.Parameters.Add(new OracleParameter(":idTipoP", producto.TipoProducto.IdTipoProducto));            

            int a = cmd.ExecuteNonQuery();
            conn.Close();
            if (a > 0)
            {
                resultado = true;
            }

            return resultado;

        }


        public bool eliminarProducto(Producto producto)
        {
            bool resultado = false;

            conn.Open();
            OracleCommand cmd = new OracleCommand("DELETE from producto where oferta =:idProducto", conn);

            cmd.Parameters.Add(new OracleParameter(":idProducto", producto.IdProducto));

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
