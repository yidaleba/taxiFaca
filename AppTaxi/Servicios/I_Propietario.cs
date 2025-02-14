using AppTaxi.Models;

namespace AppTaxi.Servicios
{
    public interface I_Propietario
    {
        Task<List<Propietario>> Lista();
        Task<Propietario> Obtener(int IdPropietario);
        Task<bool> Guardar(Propietario propietario);
        Task<bool> Editar(Propietario propietario);
        Task<bool> Eliminar(int IdPropietario);
    }
}
