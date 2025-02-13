namespace APITaxi.Models
{
    public class Empresa
    {
        public int IdEmpresa { get; set; }

        public string Nombre { get; set; }

        public string Nit { get; set; }

        public string Representante { get; set; }

        public string RedPrincipal { get; set; }

        public string RedSecundaria { get; set; }

        public int IdUsuario { get; set; }

        public int Cupos { get; set; }

    }
}
