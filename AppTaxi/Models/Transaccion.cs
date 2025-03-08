namespace AppTaxi.Models
{
    public class Transaccion
    {
        public int IdAccion { get; set; }
        public int IdUsuario { get; set; }
        public string Accion { get; set; }
        public string Modelo { get; set; }

        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
    }
}
