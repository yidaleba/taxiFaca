namespace AppTaxi.Models
{
    public class Vehiculo
    {
        public int Contador { get; set; }
        public int IdVehiculo { get; set; }

        public string Placa { get; set; }

        public bool Estado { get; set; }

        public string Soat { get; set; }

        public string TecnicoMecanica { get; set; }

        public int IdPropietario { get; set; }

        public int IdEmpresa { get; set; }
    }
}
