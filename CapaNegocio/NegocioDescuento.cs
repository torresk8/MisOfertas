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
    public class NegocioDescuento
    {
            private Conexion conexion = new Conexion();
            OracleConnection conn;
            public NegocioDescuento()
            {
                conn = conexion.conectar();
            }

            public bool insertarDescuento(Descuento descuento)
            {
                bool resultado = false;
            try
            {
            
                //Solo se ingresa el nombre del procedimiento
                OracleCommand cmd = new OracleCommand("ingreso_descuento", conn);
                
                //decimos que ejecutaremos una consulta tipo procedimiento
                cmd.CommandType = CommandType.StoredProcedure;
                //Se agregan los parametros del procedimiento(variables con los mismos nombre de la bd)
                //            Nombre variable , tipo dato,(si es varchar agregar el largo),(input o outPut).valor = varible recibida por post
                cmd.Parameters.Add("cantidad_d", OracleDbType.Int32, ParameterDirection.Input).Value = descuento.cantidad;
                cmd.Parameters.Add("idRubro_d", OracleDbType.Int32, ParameterDirection.Input).Value = descuento.rubro.IdRubro;
                //Abrimos la conexion
                conn.Open();
                //ejecutamos la consulta si devuelve -1 es porque se inserto correctamente
                int a = cmd.ExecuteNonQuery();
                //cerramos la conexion
                conn.Close();
                //vericamos si se inserto el descuento

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

            public List<Descuento> retornaDescuentoListId(int id)
            {
                 List<Descuento> auxDescuento = new List<Descuento>();
            try
            {
            
                conn.Open();

                OracleCommand cmd = new OracleCommand("SELECT * FROM DESCUENTO WHERE idRubro = :idRubro ORDER BY cantidad ASC", conn);
                
                cmd.Parameters.Add(new OracleParameter(":idRubro", id));
                

                OracleDataAdapter da = new OracleDataAdapter();
                da.SelectCommand = cmd;
                OracleDataReader dr = cmd.ExecuteReader();
                

                 while (dr.Read())
                 {
                    Descuento descuento = new Descuento(); ;

                    descuento.idDescuento = dr.GetInt32(0);
                    descuento.cantidad = dr.GetInt32(1);
                    descuento.rubro.IdRubro = dr.GetInt32(2);

                    auxDescuento.Add(descuento);
            }
             conn.Close();
            }
            catch (Exception ex)
            {

            }

            return auxDescuento;
            }


        public Descuento retornaDescuento(int idRubro, int idDescuento)
        {
            Descuento auxDescuento = new Descuento();
            try
            {

            conn.Open();

            OracleCommand cmd = new OracleCommand("SELECT * FROM DESCUENTO WHERE idRubro = :idRubro AND idDescuento = :idDescuento", conn);

            cmd.Parameters.Add(new OracleParameter(":idRubro", idRubro));
            cmd.Parameters.Add(new OracleParameter(":idDescuento", idDescuento));


            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = cmd;
            OracleDataReader dr = cmd.ExecuteReader();


            while (dr.Read())
            {

                auxDescuento.idDescuento = dr.GetInt32(0);
                auxDescuento.cantidad = dr.GetInt32(1);
                auxDescuento.rubro.IdRubro = dr.GetInt32(2);

            }
            conn.Close();
            }
            catch (Exception ex)
            {

            }
            return auxDescuento;
        }

        public List<Descuento> retornaDescuentoList()
        {
            List<Descuento> auxDescuento = new List<Descuento>();
            try
            {
            
            conn.Open();

            OracleCommand cmd = new OracleCommand("SELECT * FROM DESCUENTO ", conn);



            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = cmd;
            OracleDataReader dr = cmd.ExecuteReader();
            conn.Close();

            while (dr.Read())
            {
                Descuento descuento = new Descuento(); ;

                descuento.idDescuento = dr.GetInt32(0);
                descuento.cantidad = dr.GetInt32(1);
                descuento.rubro.IdRubro = dr.GetInt32(2);

                auxDescuento.Add(descuento);
            }
            }
            catch (Exception ex)
            {

            }
            return auxDescuento;
        }


        public bool eliminarDescuento(int id)
        {
            bool resultado = false;

            conn.Open();
            OracleCommand cmd = new OracleCommand("DELETE from descuento where idDescuento =:idDescuento", conn);

            cmd.Parameters.Add(new OracleParameter(":idDescuento", id));

            int a = cmd.ExecuteNonQuery();
            conn.Close();
            if (a > 0)
            {
                resultado = true;
            }

            return resultado;

        }



        public bool actualizarDescuento(Descuento descuento)
        {
            bool resultado = false;


            conn.Open();

            OracleCommand cmd = new OracleCommand("UPDATE descuento SET  cantidad ='" + descuento.cantidad + "', idRubro='" + descuento.rubro + "' WHERE idDescuento ='" + descuento.idDescuento + "'", conn);

            cmd.Parameters.Add(new OracleParameter(":cantidad", descuento.cantidad));            
            cmd.Parameters.Add(new OracleParameter(":rubro", descuento.rubro));




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



