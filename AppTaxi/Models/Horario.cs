namespace APITaxi.Models
{
    public class Horario
    {
        public int IdHorario { get; set; } 
        public DateTime Fecha { get; set; } 
        public TimeSpan HoraInicio { get; set; } 
        public TimeSpan HoraFin { get; set; } 
        public int IdConductor { get; set; } 
        public int IdVehiculo { get; set; } 
    }
}
