using AppTaxi.Models;

namespace AppTaxi.Servicios
{
    public interface I_Invitado
    {
        Task<Invitado> Placa(string Placa);
        Task<Invitado> Documento(long Documento);

    }
}
