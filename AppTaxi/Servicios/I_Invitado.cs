using AppTaxi.Models;

namespace AppTaxi.Servicios
{
    public interface I_Invitado
    {
        Task<List<Invitado>> Lista();
        Task<Invitado> Obtener(int IdInvitado);
        Task<bool> Guardar(Invitado invitado);
        Task<bool> Editar(Invitado invitado);
        Task<bool> Eliminar(int IdInvitado);
    }
}
