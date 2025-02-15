using AppTaxi.Models;

namespace AppTaxi.Servicios
{
    public interface I_Invitado
    {
        Task<Invitado> Consulta(Consulta consulta);

    }
}
