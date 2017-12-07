
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class NegocioReporte
    {
        public void generarArchivoPlano()
        {
            try
            {

            //Pass the filepath and filename to the StreamWriter Constructor
            StreamWriter sw = new StreamWriter("C:\\Users\\Ariel\\Documents\\Test.csv");

            //Write a line of text
            sw.WriteLine("Hello World!!");

            //Write a second line of text
            sw.WriteLine("From the StreamWriter class");

            //Close the file
            sw.Close();
            }
            catch(Exception e)
            {
            Console.WriteLine("Exception: " + e.Message);
            }
            finally 
            {
            Console.WriteLine("Executing finally block.");
            }
        }


        public MemoryStream archivo()
        {
            string text = "El texto para mi archivo.";
            var stream = new MemoryStream(Encoding.ASCII.GetBytes(text));

            return stream;
        }
    }
}
