using AppTaxi.Models;

namespace AppTaxi.Servicios
{
    public interface I_Transaccion
    {
        Task<List<Transaccion>> Lista(Login login);
        Task<Transaccion> Obtener(int IdTransaccion, Login login);
        Task<bool> Guardar(Transaccion transaccion, Login login);
        Task<bool> Editar(Transaccion transaccion, Login login);
        Task<bool> Eliminar(int IdTransaccion, Login login);
    }
}
