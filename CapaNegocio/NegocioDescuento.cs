﻿using CapaConexion;
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
            try
            {
            
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


    }
}



