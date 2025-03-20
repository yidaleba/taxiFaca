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
            try
            {
                // Verificar si la cadena es nula o vacía
                if (string.IsNullOrWhiteSpace(contraseñaEncriptada))
                {
                    throw new ArgumentException("La contraseña encriptada está vacía o es nula.");
                }

                // Depuración: Imprimir la contraseña encriptada
                Console.WriteLine($"Contraseña encriptada recibida: {contraseñaEncriptada}");

                // Limpiar la cadena: eliminar espacios en blanco y caracteres no válidos
                contraseñaEncriptada = contraseñaEncriptada.Trim();

                // Intentar convertir desde Base64
                byte[] bytes = Convert.FromBase64String(contraseñaEncriptada);

                // Convertir a texto y devolver
                return Encoding.UTF8.GetString(bytes);
            }
            catch (FormatException ex)
            {
                throw new FormatException($"Error al convertir desde Base64: {ex.Message} | Entrada: {contraseñaEncriptada}");
            }
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
