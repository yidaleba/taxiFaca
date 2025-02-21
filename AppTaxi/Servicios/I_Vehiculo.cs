using AppTaxi.Models;

namespace AppTaxi.Servicios
{
    public interface I_Vehiculo
    {
        Task<List<Vehiculo>> Lista(Login login);
        Task<Vehiculo> Obtener(int IdVehiculo, Login login);
        Task<bool> Guardar(Vehiculo vehiculo, Login login);
        Task<bool> Editar(Vehiculo vehiculo, Login login);
        //Task<string> Editar(Vehiculo vehiculo, Login login);
        Task<bool> Eliminar(int IdVehiculo, Login login);
    }
}
