using AppTaxi.Models;
namespace AppTaxi.Servicios
{
    public interface I_Conductor
    {
        //Interfaces de Conductor:
        Task<List<Conductor>> Lista();
        Task<Conductor> Obtener(int IdConductor);
        Task<bool> Guardar(Conductor conductor);
        Task<bool> Editar(Conductor conductor);
        Task<bool> Eliminar(int IdConductor);





    }
}
