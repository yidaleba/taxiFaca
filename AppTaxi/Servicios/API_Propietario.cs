using AppTaxi.Models;
using Newtonsoft.Json;
using System.Text;

namespace AppTaxi.Servicios
{
    public class API_Propietario : Autenticacion, I_Propietario
    {

        public async Task<List<Propietario>> Lista(Login login)
        {
            List<Propietario> lista = new List<Propietario>();
            await Autenticar(login);

            var response = await _httpClient.GetAsync("api/Propietario/Lista");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi<List<Propietario>>>(json_respuesta);
                lista = resultado.Response;

            }
            return lista;

        }

        public async Task<Propietario> Obtener(int IdPropietario, Login login)
        {
            Propietario propietario = new Propietario();
            await Autenticar(login);

            var response = await _httpClient.GetAsync($"api/Propietario/Obtener/{IdPropietario}");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi<Propietario>>(json_respuesta);
                propietario = resultado.Response;

            }
            return propietario;
        }

        public async Task<bool> Guardar(Propietario propietario, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var contenido = new StringContent(JsonConvert.SerializeObject(propietario), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Propietario/Guardar/", contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }

        public async Task<bool> Editar(Propietario propietario, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var contenido = new StringContent(JsonConvert.SerializeObject(propietario), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/Propietario/Editar/", contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }

        public async Task<bool> Eliminar(int IdPropietario, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var response = await _httpClient.DeleteAsync($"api/Propietario/Eliminar/{IdPropietario}");

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }




    }
}
