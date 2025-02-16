using AppTaxi.Models;

namespace AppTaxi.Servicios
{
    public interface I_Horario
    {
        Task<List<Horario>> Lista(Login login);
        Task<Horario> Obtener(int IdHorario, Login login);
        Task<bool> Guardar(Horario horario, Login login);
        Task<bool> Editar(Horario horario, Login login);
        Task<bool> Eliminar(int IdHorario, Login login);
    }
}
