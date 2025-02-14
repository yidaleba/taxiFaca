using BCrypt.Net;

namespace AppTaxi.Funciones
{
    public class Encriptado
    {
        public string EncriptarContraseña(string contraseña)
        {
            return BCrypt.Net.BCrypt.HashPassword(contraseña);
        }

        public bool VerificarContraseña(string contraseña, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(contraseña, hash);
        }
    }
}
