namespace AppTaxi.Models
{
    public class ModeloVista
    {

        //Objetos:

        public Vehiculo Vehiculo { get; set; } 
        public Conductor Conductor { get; set; }
        public Empresa Empresa { get; set; }
        public Propietario Propietario { get; set; }
        public Horario Horario { get; set; }
        public Usuario Usuario { get; set; }
        public Transaccion Transaccion { get; set; }

        //Lista de Objetos:
        public List<int> Contador { get; set; }
        public List<Vehiculo> Vehiculos { get; set; }
        public List<Conductor> Conductores { get; set; }
        public List<Empresa> Empresas { get; set; }
        public List<Propietario> Propietarios { get; set; }
        public List<Horario> Horarios { get; set; }
        public List<Usuario> Usuarios { get; set; }
        public List<Transaccion> Transacciones { get; set; }

        //Archivos: 
        public IFormFile Archivo_1 { get; set; }
        public IFormFile Archivo_2 { get; set; }
        public IFormFile Archivo_3 { get; set; }
        public IFormFile Archivo_4 { get; set; }

    }
}
