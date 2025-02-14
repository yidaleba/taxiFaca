
using AppTaxi.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AppTaxi.Servicios
{
    public class API_Conductor:I_Conductor
    {
        private static string _correo;
        private static string _contrasena;
        private static string _baseUrl;
        private static string _token;

        public API_Conductor()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            _correo = builder.GetSection("ApiSettings:correo").Value;
            _contrasena = builder.GetSection("ApiSettings:contrasena").Value;
            _baseUrl = builder.GetSection("ApiSettings:baseUrl").Value;
        }

        public async Task Autenticar()
        {
            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseUrl);
            var credenciales = new Login()
            {
                Correo = _correo,
                Contrasena = _contrasena
            };

            var content = new StringContent(JsonConvert.SerializeObject(credenciales),Encoding.UTF8,"application/json");
            var response = await cliente.PostAsync("api/Autenticacion/Validar",content);
            var json_respuesta = await response.Content.ReadAsStringAsync();

            var resultado = JsonConvert.DeserializeObject<ResultadoCredencial>(json_respuesta);
            _token = resultado.Token;
        }

        public async Task<bool> Editar(Conductor conductor)
        {
            bool Respuesta = false;
            await Autenticar();

            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseUrl);
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var contenido = new StringContent(JsonConvert.SerializeObject(conductor), Encoding.UTF8, "application/json");

            var response = await cliente.PutAsync("api/Conductor/Editar/", contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }

        public async Task<bool> Eliminar(int IdConductor)
        {
            bool Respuesta = false;
            await Autenticar();

            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseUrl);
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

         

            var response = await cliente.DeleteAsync($"api/Conductor/Eliminar/{IdConductor}");

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }

        public async Task<bool> Guardar(Conductor conductor)
        {
            bool Respuesta = false;
            await Autenticar();

            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseUrl);
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var contenido = new StringContent(JsonConvert.SerializeObject(conductor), Encoding.UTF8, "application/json");

            var response = await cliente.PostAsync("api/Conductor/Guardar/",contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }

        public async Task<List<Conductor>> Lista()
        {
            List<Conductor> lista = new List<Conductor>();
            await Autenticar();

            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseUrl);
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await cliente.GetAsync("api/Conductor/Lista");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi<List<Conductor>>>(json_respuesta);
                lista = resultado.Response;

            }
            return lista;
        }

        public async Task<Conductor> Obtener(int IdConductor)
        {
            Conductor conductor = new Conductor();
            await Autenticar();

            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseUrl);
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await cliente.GetAsync($"api/Conductor/Obtener/{IdConductor}");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi<Conductor>>(json_respuesta);
                conductor = resultado.Response;

            }
            return conductor;
        }
    }
}
