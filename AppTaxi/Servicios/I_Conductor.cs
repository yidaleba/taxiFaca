using AppTaxi.Models;
namespace AppTaxi.Servicios
{
    public interface I_Conductor
    {
        //Interfaces de Conductor:
        Task<List<Conductor>> Lista(Login login);
        Task<Conductor> Obtener(int IdConductor,Login login);
        
        Task<bool> Guardar(Conductor conductor, Login login);
        Task<bool> Editar(Conductor conductor, Login login);
        Task<bool> Eliminar(int IdConductor, Login login);





    }
}
