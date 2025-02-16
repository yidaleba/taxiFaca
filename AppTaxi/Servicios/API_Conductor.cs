
using AppTaxi.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AppTaxi.Servicios
{
    public class API_Conductor:Autenticacion,I_Conductor
    {
        
        public async Task<bool> Editar(Conductor conductor, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var contenido = new StringContent(JsonConvert.SerializeObject(conductor), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/Conductor/Editar/", contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }

        public async Task<bool> Eliminar(int IdConductor, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

         

            var response = await _httpClient.DeleteAsync($"api/Conductor/Eliminar/{IdConductor}");

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }

        public async Task<bool> Guardar(Conductor conductor, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var contenido = new StringContent(JsonConvert.SerializeObject(conductor), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Conductor/Guardar/",contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }

        public async Task<List<Conductor>> Lista(Login login)
        {
            List<Conductor> lista = new List<Conductor>();
            await Autenticar(login);
            var response = await _httpClient.GetAsync("api/Conductor/Lista");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi<List<Conductor>>>(json_respuesta);
                lista = resultado.Response;

            }
            return lista;
        }

        public async Task<Conductor> Obtener(int IdConductor, Login login)
        {
            Conductor conductor = new Conductor();
            await Autenticar(login);

            
            var response = await _httpClient.GetAsync($"api/Conductor/Obtener/{IdConductor}");

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
