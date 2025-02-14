using AppTaxi.Models;

namespace AppTaxi.Servicios
{
    public interface I_Rol
    {
        Task<List<Rol>> Lista();
        Task<Rol> Obtener(int IdRol);
        Task<bool> Guardar(Rol rol);
        Task<bool> Editar(Rol rol);
        Task<bool> Eliminar(int IdRol);
    }
}
