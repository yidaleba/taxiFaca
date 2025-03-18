using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AppTaxi.Funciones
{
    public class Encriptado
    {
        public string EncriptarSimple(string contraseña)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(contraseña);
            return Convert.ToBase64String(bytes);
        }

        public string DesencriptarSimple(string contraseñaEncriptada)
        {
            byte[] bytes = Convert.FromBase64String(contraseñaEncriptada);
            return Encoding.UTF8.GetString(bytes);
        }

        // Generar una sugerencia de contraseña combinando nombre y documento
        public string GenerarContrasena(string Documento, string Nombre)
        {
            string nombreProcesado = Nombre.Replace(" ", "").Substring(0, Math.Min(6, Nombre.Length));
            string docProcesado = Documento.Substring(Documento.Length - 3);
            return nombreProcesado + docProcesado;
        }
    }
}
