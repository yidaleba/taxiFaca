using AppTaxi.Models;

namespace AppTaxi.Servicios
{
    public interface I_Empresa
    {
        Task<List<Empresa>> Lista();
        Task<Empresa> Obtener(int IdEmpresa);
        Task<bool> Guardar(Empresa empresa);
        Task<bool> Editar(Empresa empresa);
        Task<bool> Eliminar(int IdEmpresa);
    }
}
