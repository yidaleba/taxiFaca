using Newtonsoft.Json;

namespace AppTaxi.Models
{
    public class ResultadoApi<T>
    {
        public string Mensaje { get; set; }
        public T Response { get; set; }
    }
}
