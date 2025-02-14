using AppTaxi.Models;

namespace AppTaxi.Servicios
{
    public interface I_Usuario
    {
        Task<List<Usuario>> Lista();
        Task<Usuario> Obtener(int IdUsuario);
        Task<bool> Guardar(Usuario usuario);
        Task<bool> Editar(Usuario usuario);
        Task<bool> Eliminar(int IdUsuario);
    }
}
