using AppTaxi.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AppTaxi.Servicios
{
    public abstract class Autenticacion
    {
        private static string _correo;
        private static string _contrasena;
        private static string _baseUrl;
        protected string _token;
        protected HttpClient _httpClient;

        

        public Autenticacion()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _correo = builder.GetSection("ApiSettings:correo").Value;
            _contrasena = builder.GetSection("ApiSettings:contrasena").Value;
            _baseUrl = builder.GetSection("ApiSettings:baseUrl").Value;

            _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
        }

        protected async Task Autenticar()
        {
            var credenciales = new Login()
            {
                Correo = _correo,
                Contrasena = _contrasena
            };

            var content = new StringContent(JsonConvert.SerializeObject(credenciales), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Autenticacion/Validar", content);
            var json_respuesta = await response.Content.ReadAsStringAsync();

            var resultado = JsonConvert.DeserializeObject<ResultadoCredencial>(json_respuesta);
            _token = resultado.Token;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

    }
}
