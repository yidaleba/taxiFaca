using AppTaxi.Models;

namespace AppTaxi.Servicios
{
    public interface I_Propietario
    {
        Task<List<Propietario>> Lista(Login login);
        Task<Propietario> Obtener(int IdPropietario, Login login);
        Task<bool> Guardar(Propietario propietario, Login login);
        Task<bool> Editar(Propietario propietario, Login login);
        Task<bool> Eliminar(int IdPropietario, Login login);
    }
}
