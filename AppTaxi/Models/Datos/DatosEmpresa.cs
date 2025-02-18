namespace AppTaxi.Models.Datos
{
    public class DatosEmpresa
    {
        public int IdDato { get; set; }
        public string Placa { get; set; }
        public string Propietario { get; set; }
        public string Conductor { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
    }
}
