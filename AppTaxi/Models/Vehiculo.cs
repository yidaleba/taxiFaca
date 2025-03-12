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


        public DateTime VenceSoat { get; set; }

        public DateTime VenceTecnicoMecanica { get; set; }

        public string ObtenerEstadoDocumento(DateTime? fechaVencimiento)
        {
            if (fechaVencimiento == null)
            {
                return "<span class='badge bg-danger'>No registrado</span>";
            }

            DateTime fechaActual = DateTime.Now;
            DateTime unMesDespues = fechaActual.AddMonths(1);

            if (fechaVencimiento < fechaActual)
            {
                return "<span class='badge bg-danger'>Vencida</span>";
            }
            else if (fechaVencimiento >= fechaActual && fechaVencimiento <= unMesDespues)
            {
                return "<span class='badge bg-warning text-dark'>Próxima a vencer</span>";
            }
            else
            {
                return "<span class='badge bg-success'>Activa</span>";
            }
        }
    }
}
