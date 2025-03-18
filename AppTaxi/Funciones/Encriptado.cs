using System;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

namespace AppTaxi.Funciones
{
    public class Encriptado
    {


        // Encriptación y desencriptación más simple con AES

        private static readonly byte[] Clave = Encoding.UTF8.GetBytes("AppTaxisFacatativaSecretaria2025"); // Debe ser de 32 caracteres

        public string EncriptarSimple(string contraseña)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Clave;
                aesAlg.GenerateIV(); // Genera un IV aleatorio para cada encriptación

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length); // Almacena el IV al inicio

                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(contraseña);
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public string DesencriptarSimple(string contraseñaEncriptada)
        {
            byte[] fullCipher = Convert.FromBase64String(contraseñaEncriptada);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Clave;

                // Extraer el IV (los primeros 16 bytes)
                byte[] iv = new byte[aesAlg.IV.Length];
                Array.Copy(fullCipher, iv, iv.Length);
                aesAlg.IV = iv;

                using (MemoryStream msDecrypt = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
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
