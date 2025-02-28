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

        //Lista de Objetos
        public List<Vehiculo> Vehiculos { get; set; }
        public List<Conductor> Conductores { get; set; }
        public List<Empresa> Empresas { get; set; }
        public List<Propietario> Propietarios { get; set; }
        public List<Horario> Horarios { get; set; }

        public IFormFile Archivo_1 { get; set; }
        public IFormFile Archivo_2 { get; set; }
        public IFormFile Archivo_3 { get; set; }
        public IFormFile Archivo_4 { get; set; }

    }
}
