using CapaConexion;
using CapaDTO;
using CapaNegocio.Utilidades;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class NegocioUsuario
    {
        private Conexion conexion = new Conexion();
        OracleConnection conn;


        public NegocioUsuario()
        {
            conn = conexion.conectar();
        }

        public Usuario login(Usuario usuario)
        {
            conn.Open();

            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand();

            //Encriptamos contraseña
            string pass = SeguridadUtilidades.getSha1(usuario.Password);

            cmd = new OracleCommand("SELECT * FROM usuario where nombreUsuario=:user_u and password=:pass_u", conn);
            cmd.Parameters.Add(new OracleParameter(":user_u", usuario.NombreUsuario));
            cmd.Parameters.Add(new OracleParameter(":pass_u", pass));

            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = cmd;
            OracleDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                usuario.IdUsuario = dr.GetInt32(0);
                usuario.Nombre = String.Format("{0}", dr[1]);
                usuario.NombreUsuario = String.Format("{0}", dr[2]);
                usuario.Password = String.Format("{0}", dr[3]);
                usuario.Rut = String.Format("{0}", dr[4]);
                usuario.Direccion = String.Format("{0}", dr[5]);
                usuario.Telefono = dr.GetInt32(6);

            }

            conn.Close();

            return usuario;
        }


        public bool insertarUsuario(Usuario usuario)
        {
            bool resultado = false;

            var pass = SeguridadUtilidades.getSha1(usuario.Password);

            conn.Open();
            OracleCommand cmd = new OracleCommand("INSERT INTO usuario (idUsuario,nombre,nombreUsuario," +
                "                                  password,rut,direccion,telefono)" +
                     "VALUES(sucuence_usu.NEXTVAL,:nombre,:usuario,:pass,:rut,:direccion,:telefono)", conn);

            cmd.Parameters.Add(new OracleParameter(":nombre", usuario.Nombre));
            cmd.Parameters.Add(new OracleParameter(":usuario", usuario.NombreUsuario));
            cmd.Parameters.Add(new OracleParameter(":pass", pass));
            cmd.Parameters.Add(new OracleParameter(":rut", pass));
            cmd.Parameters.Add(new OracleParameter(":direccion", pass));
            cmd.Parameters.Add(new OracleParameter(":telefono", pass));

            int a = cmd.ExecuteNonQuery();
            conn.Close();
            if (a > 0)
            {
                resultado = true;
            }

            return resultado;

        }


        public bool eliminarUsuario(Usuario usuario)
        {
            bool resultado = false;

            conn.Open();
            OracleCommand cmd = new OracleCommand("DELETE from usuario where nombreUsuario =:usuario", conn);

            cmd.Parameters.Add(new OracleParameter(":usuario", usuario.NombreUsuario));

            int a = cmd.ExecuteNonQuery();
            conn.Close();
            if (a > 0)
            {
                resultado = true;
            }

            return resultado;

        }


        public Usuario retornaUsuario(int id)
        {
            Usuario usuario = new Usuario();

            conn.Open();
            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand("SELECT * FROM usuario where idUsuario=:id", conn);
            cmd.Parameters.Add(new OracleParameter(":id", id));

            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = cmd;
            OracleDataReader dr = cmd.ExecuteReader();

            //byte[] ima = (byte[])cmd.ExecuteScalar();          



            while (dr.Read())
            {

                usuario.IdUsuario = dr.GetInt32(0);
                usuario.Nombre = String.Format("{0}", dr[1]);
                usuario.NombreUsuario = String.Format("{0}", dr[2]);
                usuario.Password = String.Format("{0}", dr[3]);
                usuario.Rut = String.Format("{0}", dr[4]);
                usuario.Direccion = String.Format("{0}", dr[5]);
                usuario.Telefono = dr.GetInt32(6);

            }

            conn.Close();

            return usuario;
        }



        public bool actualizarUsuario(Usuario usuario)
        {
            bool resultado = false;


            conn.Open();

            OracleCommand cmd = new OracleCommand("UPDATE producto SET  marca ='" + usuario.Nombre + "', modelo='" + usuario.Modelo + "', descripcion='" + producto.Descripcion + "', " +
                "precio='" + producto.Precio + "', stock='" + producto.Stock + "' WHERE idProducto ='" + producto.IdProducto + "'", conn);

            cmd.Parameters.Add(new OracleParameter(":marca", producto.Marca));
            cmd.Parameters.Add(new OracleParameter(":modelo", producto.Modelo));
            cmd.Parameters.Add(new OracleParameter(":descripcion", producto.Descripcion));
            cmd.Parameters.Add(new OracleParameter(":precio", producto.Precio));
            cmd.Parameters.Add(new OracleParameter(":stock", producto.Stock));



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
