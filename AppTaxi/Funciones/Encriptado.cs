using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AppTaxi.Funciones
{
    public class Encriptado
    {
        // Función existente para obtener SHA256
        public static string GetSHA256(string contrasena)
        {
            SHA256 sha256 = SHA256.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(contrasena));
            for (int i = 0; i < stream.Length; i++)
                sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        // Función existente para generar sugerencia de contraseña
        public static string GenerarContrasena(string Documento, string Nombre)
        {
            string nombreProcesado = Nombre.Replace(" ", "").Substring(0, Math.Min(6, Nombre.Length));
            string docProcesado = Documento.Substring(Documento.Length - 3);
            return nombreProcesado + docProcesado;
        }


    }
}
