using AppTaxi.Models;

namespace AppTaxi.Servicios
{
    public interface I_Vehiculo
    {
        Task<List<Vehiculo>> Lista();
        Task<Vehiculo> Obtener(int IdVehiculo);
        Task<bool> Guardar(Vehiculo vehiculo);
        Task<bool> Editar(Vehiculo vehiculo);
        Task<bool> Eliminar(int IdVehiculo);
    }
}
