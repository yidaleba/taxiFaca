using AppTaxi.Models;

namespace AppTaxi.Servicios
{
    public interface I_Empresa
    {
        Task<List<Empresa>> Lista(Login login);
        Task<Empresa> Obtener(int IdEmpresa, Login login);
        Task<bool> Guardar(Empresa empresa, Login login);
        Task<bool> Editar(Empresa empresa, Login login);
        Task<bool> Eliminar(int IdEmpresa, Login login);
    }
}
