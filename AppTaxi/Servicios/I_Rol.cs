using AppTaxi.Models;

namespace AppTaxi.Servicios
{
    public interface I_Rol
    {
        Task<List<Rol>> Lista(Login login);
        Task<Rol> Obtener(int IdRol, Login login);
        Task<bool> Guardar(Rol rol, Login login);
        Task<bool> Editar(Rol rol, Login login);
        Task<bool> Eliminar(int IdRol, Login login);
    }
}
