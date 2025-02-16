using AppTaxi.Models;

namespace AppTaxi.Servicios
{
    public interface I_Usuario
    {
        Task<List<Usuario>> Lista(Login login);
        Task<Usuario> Obtener(int IdUsuario, Login login);
        Task<bool> Guardar(Usuario usuario, Login login);
        Task<bool> Editar(Usuario usuario, Login login);
        Task<bool> Eliminar(int IdUsuario, Login login);
    }
}
