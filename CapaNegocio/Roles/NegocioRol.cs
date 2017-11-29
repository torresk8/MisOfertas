using CapaConexion;
using CapaDTO;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Roles
{
    public class NegocioRol
    {
        private Conexion conexion = new Conexion();
        OracleConnection conn;
        public NegocioRol()
        {
            conn = conexion.conectar();
        }
        private int numRecs;
        public int NumRecs { get => numRecs; set => numRecs = value; }
        public bool IsUserInRole(string idUsuario, string idRol)
        {
            bool userIsInRole = false;
            int idUsu = Convert.ToInt32(idUsuario);
            int idRol1 = Convert.ToInt32(idRol);
            conn.Open();
            
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand("SELECT COUNT(*) FROM permiso " +
                "where idUsuario=:idUsu AND idRol=:idRol", conn);
           cmd.Parameters.Add(new OracleParameter(":idUsu", idUsu));
           cmd.Parameters.Add(new OracleParameter(":idRol", idRol1));

            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = cmd;
            OracleDataReader dr = cmd.ExecuteReader();

            while(dr.Read())
            {
                NumRecs = dr.GetInt32(0);
            }
            
    
                if (NumRecs > 0)
            {
                userIsInRole = true;
            }
                    

            conn.Close();

            return userIsInRole;
        }


        public string[] GetRolesForUser(string userName)
        {            
            conn.Open();
            string permisos = "";

            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand("SELECT r.nombre FROM permiso p  join ROL r on r.IDROL = p.IDROL join usuario u on u.IDUSUARIO = p.IDUSUARIO " +
                "where u.nombreUsuario =:usu", conn);
            cmd.Parameters.Add(new OracleParameter(":usu", userName));            

            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = cmd;
            OracleDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                permisos += dr.GetString(0)+ ",";
            }


            if (permisos.Length > 0)
            {
                // Remove trailing comma.
                permisos = permisos.Substring(0, permisos.Length - 1);
                return permisos.Split(',');
            }


            conn.Close();

            return new string[0];
        }

        /*
        public bool insertarOferta(Oferta oferta)
        {
            bool resultado = false;

            conn.Open();

            OracleCommand cmd = new OracleCommand("INSERT INTO oferta (idOferta, nombre, descripcion, " +
                "precioNormal, precioOferta, cantidadMin, cantidadMax, idProducto, productImage)" +
                     "VALUES(sucuence_oferta.NEXTVAL, :nombre, :descripcion, :precioNormal," +
                     ":precioOferta, :cantidadMin, :cantidadMax, :idProducto, :productImage)", conn);

            cmd.Parameters.Add(new OracleParameter(":nombre", oferta.Nombre));
            cmd.Parameters.Add(new OracleParameter(":descripcion", oferta.Descripcion));
            cmd.Parameters.Add(new OracleParameter(":precioNormal", oferta.PrecioNormal));
            cmd.Parameters.Add(new OracleParameter(":precioOferta", oferta.PrecioOfeta));
            cmd.Parameters.Add(new OracleParameter(":cantidadMin", oferta.CantidadMin));
            cmd.Parameters.Add(new OracleParameter(":cantidadMax", oferta.CantidadMax));
            cmd.Parameters.Add(new OracleParameter(":idProducto", 1));
            cmd.Parameters.Add(new OracleParameter(":productImage", oferta.Imagen));

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

            cmd.Parameters.Add(new OracleParameter(":usuario", oferta.IdOferta));

            int a = cmd.ExecuteNonQuery();
            conn.Close();
            if (a > 0)
            {
                resultado = true;
            }

            return resultado;

        }*/
    }
}
