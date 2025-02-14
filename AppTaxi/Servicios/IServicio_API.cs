using APITaxi.Models;
namespace AppTaxi.Servicios
{
    public interface IServicio_API
    {
        //Interfaces de Conductor:
        Task<List<Conductor>> Lista();
        Task<Conductor> Obtener(int IdConductor);
        Task<bool> Guardar(Conductor conductor);
        Task<bool> Editar(Conductor conductor);
        Task<bool> Eliminar(int IdConductor);





    }
}
