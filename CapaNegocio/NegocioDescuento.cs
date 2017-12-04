using CapaConexion;
using CapaDTO;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
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

                conn.Open();
                OracleCommand cmd = new OracleCommand("INSERT INTO DESCUENTO(idDescuento,cantidad,idRubro)" +
                         "VALUES(secuence_descuento.NEXTVAL,:cantidad,:idRubro)", conn);

                cmd.Parameters.Add(new OracleParameter(":cantidad", descuento.cantidad));
                cmd.Parameters.Add(new OracleParameter(":idRubro", descuento.rubro.IdRubro));

            int a = cmd.ExecuteNonQuery();
                conn.Close();
                if (a > 0)
                {
                    resultado = true;
                }

                return resultado;

            }

            public List<Descuento> retornaDescuentoListId(int id)
            {
                 List<Descuento> auxDescuento = new List<Descuento>();

                conn.Open();

                OracleCommand cmd = new OracleCommand("SELECT * FROM DESCUENTO WHERE idRubro = :idRubro", conn);
                
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
            return auxDescuento;
            }


        public Descuento retornaDescuento(int idRubro, int idDescuento)
        {
            Descuento auxDescuento = new Descuento();

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
            return auxDescuento;
        }

        public List<Descuento> retornaDescuentoList()
        {
            List<Descuento> auxDescuento = new List<Descuento>();

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

            return auxDescuento;
        }


    }
}



