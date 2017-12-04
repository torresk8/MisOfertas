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
            cmd.Parameters.Add(new OracleParameter(":rut", usuario.Rut));
            cmd.Parameters.Add(new OracleParameter(":direccion", usuario.Direccion));
            cmd.Parameters.Add(new OracleParameter(":telefono", usuario.Telefono));

            int a= cmd.ExecuteNonQuery();
            conn.Close();
            if (a > 0)
            {
                 resultado = true;
            }

            return resultado;

        }

        public List<Usuario> retornaUsuarioList()
        {
            List<Usuario> list = new List<Usuario>();
            conn.Open();
            
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand("SELECT * FROM usuario u "+
                                    "WHERE  u.RECIBIRCORREO = 'Si' AND u.IDUSUARIO <> ALL (SELECT p.IDUSUARIO FROM permiso p)", conn);


            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = cmd;
            OracleDataReader dr = cmd.ExecuteReader();


            while (dr.Read())
            {
                Usuario usuario = new Usuario();

                usuario.IdUsuario = dr.GetInt32(0);
                usuario.NombreUsuario = String.Format("{0}", dr[2]);


                list.Add(usuario);
            }

            conn.Close();

            return list;
        }


        public bool eliminarUsuario(Usuario usuario)
        {
            bool resultado = false;

            conn.Open();
            OracleCommand cmd = new OracleCommand("DELETE from usuario where nombreUsuario =:usuario", conn);

<<<<<<< HEAD
            OracleCommand cmd = new OracleCommand("UPDATE usuario SET  nombre ='" + usuario.Nombre + "', nombreUsuario='" + usuario.NombreUsuario + "', rut='" + usuario.Rut + "', " +
                "direccion='" + usuario.Direccion + "', telefono='" + usuario.Telefono+ "', correo='" + usuario.Correo + "' WHERE idUsuario ='" + usuario.IdUsuario + "'", conn);

            cmd.Parameters.Add(new OracleParameter(":nombre", usuario.Nombre));
            cmd.Parameters.Add(new OracleParameter(":nombreUsuario", usuario.NombreUsuario));
            cmd.Parameters.Add(new OracleParameter(":rut", usuario.Rut));
            cmd.Parameters.Add(new OracleParameter(":direccion", usuario.Direccion));
            cmd.Parameters.Add(new OracleParameter(":telefono", usuario.Telefono));
            cmd.Parameters.Add(new OracleParameter(":correo", usuario.Correo));



            int a = cmd.ExecuteNonQuery();
            conn.Close();
            if (a > 0)
            {
                resultado = true;
            }

            return resultado;

        }


        public bool actualizarPassword(Usuario usuario)
        {
            bool resultado = false;


            conn.Open();

            OracleCommand cmd = new OracleCommand("UPDATE usuario SET  password ='" + usuario.Password +  "' WHERE idUsuario ='" + usuario.IdUsuario + "'", conn);

            cmd.Parameters.Add(new OracleParameter(":nombre", usuario.Password));
        


=======
            cmd.Parameters.Add(new OracleParameter(":usuario", usuario.NombreUsuario));
>>>>>>> parent of 1d927bb... m

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
