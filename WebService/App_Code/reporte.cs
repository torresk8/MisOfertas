﻿using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;



/// <summary>
/// Descripción breve de reporte
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
// [System.Web.Script.Services.ScriptService]
public class reporte : System.Web.Services.WebService {

    public reporte () {

        //Elimine la marca de comentario de la línea siguiente si utiliza los componentes diseñados 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string archivoPlano()
    {
        NegocioReporte negocioReporte = new NegocioReporte();
        return negocioReporte.generarArchivoPlano();
        
        

    }

}
