using AppTaxi.Models;

namespace AppTaxi.Servicios
{
    public interface I_Horario
    {
        Task<List<Horario>> Lista();
        Task<Horario> Obtener(int IdHorario);
        Task<bool> Guardar(Horario horario);
        Task<bool> Editar(Horario horario);
        Task<bool> Eliminar(int IdHorario);
    }
}
