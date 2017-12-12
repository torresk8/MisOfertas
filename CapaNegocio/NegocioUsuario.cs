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
            try
            {
                conn.Open();

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
            }
            catch(Exception ex)
            {

            }

            
                               
            

            return usuario;
        }


        public bool insertarUsuario(Usuario usuario)
        {
            bool resultado = false;

            try
            {               

                var pass = SeguridadUtilidades.getSha1(usuario.Password);
                
                OracleCommand cmd = new OracleCommand("nuevo_usu", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("nombre_u", OracleDbType.Varchar2,ParameterDirection.Input).Value=usuario.Nombre;
                cmd.Parameters.Add("nombreU_u", OracleDbType.Varchar2, ParameterDirection.Input).Value = usuario.NombreUsuario;
                cmd.Parameters.Add("pass_u", OracleDbType.Varchar2, ParameterDirection.Input).Value = usuario.Password;
                cmd.Parameters.Add("rut_u", OracleDbType.Varchar2, ParameterDirection.Input).Value = usuario.Rut;
                cmd.Parameters.Add("direc", OracleDbType.Varchar2, ParameterDirection.Input).Value = usuario.Direccion;
                cmd.Parameters.Add("telefono_u", OracleDbType.Int32, ParameterDirection.Input).Value = usuario.Telefono;
                cmd.Parameters.Add("recibirC", OracleDbType.Varchar2, ParameterDirection.Input).Value = usuario.RecibirCorreo;

                conn.Open();
                int a = cmd.ExecuteNonQuery();
                conn.Close();
                if (a == -1)
                {
                    resultado = true;
                }
            }catch(Exception ex)
            {

            }

            return resultado;

        }

        public bool actualizarUsuario(Usuario usuario)
        {
            bool resultado = false;
            try
            {

                OracleCommand cmd = new OracleCommand("update_usuario", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("nombre_u", OracleDbType.Varchar2, ParameterDirection.Output).Value = usuario.Nombre;
                cmd.Parameters.Add("nombreU_u", OracleDbType.Varchar2, ParameterDirection.Output).Value = usuario.NombreUsuario;
                cmd.Parameters.Add("pass_u", OracleDbType.Varchar2, ParameterDirection.Output).Value = usuario.Password;
                cmd.Parameters.Add("rut_u", OracleDbType.Varchar2, ParameterDirection.Output).Value = usuario.Rut;
                cmd.Parameters.Add("direc", OracleDbType.Varchar2, ParameterDirection.Output).Value = usuario.Direccion;
                cmd.Parameters.Add("telefono_u", OracleDbType.Int32, ParameterDirection.Output).Value = usuario.Telefono;
                cmd.Parameters.Add("recibirC", OracleDbType.Varchar2, ParameterDirection.Output).Value = usuario.RecibirCorreo;

                conn.Open();
                int a = cmd.ExecuteNonQuery();
                conn.Close();
                if (a == -1)
                {
                    resultado = true;
                }
            }catch(Exception ex)
            {

            }


            return resultado;
        }

        public Usuario retornaUsuario(int id)        
        {
            Usuario usuario = new Usuario();
            try
            {

                conn.Open();

                OracleCommand cmd = new OracleCommand();
                cmd = new OracleCommand("SELECT * FROM usuario u " +
                                        "WHERE  u.idUsuario = :idUsuario", conn);

                cmd.Parameters.Add(new OracleParameter(":idUsuario", id));

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
                    usuario.RecibirCorreo = String.Format("{0}", dr[7]);


                }

                conn.Close();
            }catch(Exception ex)
            {

            }

            return usuario;
        }

        public List<Usuario> retornaUsuarioList()
        {
            List<Usuario> list = new List<Usuario>();

            try
            {


                conn.Open();

                OracleCommand cmd = new OracleCommand();
                cmd = new OracleCommand("SELECT * FROM usuario u " +
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
            }catch(Exception ex)
            {

            }
            return list;
        }


        public bool eliminarUsuario(Usuario usuario)
        {
            bool resultado = false;
            try
            {

                conn.Open();
                OracleCommand cmd = new OracleCommand("DELETE from usuario where nombreUsuario =:usuario", conn);


                cmd.Parameters.Add(new OracleParameter(":nombreUsuario", usuario.NombreUsuario));

                int a = cmd.ExecuteNonQuery();
                conn.Close();
                if (a > 0)
                {
                    resultado = true;
                }
            }catch(Exception ex)
            {

            }
            return resultado;

        }


        public bool actualizarPassword(Usuario usuario)
        {
            bool resultado = false;
            try
            {
               
                conn.Open();

                OracleCommand cmd = new OracleCommand("UPDATE usuario SET  password =:password WHERE idUsuario =:id", conn);

                cmd.Parameters.Add(new OracleParameter(":password", usuario.Password));
                cmd.Parameters.Add(new OracleParameter(":id", usuario.IdUsuario));


                int a = cmd.ExecuteNonQuery();
                conn.Close();
                if (a > 0)
                {
                    resultado = true;
                }
            }catch(Exception ex)
            {

            }
            return resultado;

        }
    }
}
