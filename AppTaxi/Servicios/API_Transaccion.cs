using AppTaxi.Models;
using Newtonsoft.Json;
using System.Text;

namespace AppTaxi.Servicios
{
    public class API_Transaccion : Autenticacion, I_Transaccion
    {
        public async Task<List<Transaccion>> Lista(Login login)
        {
            List<Transaccion> lista = new List<Transaccion>();
            await Autenticar(login);

            var response = await _httpClient.GetAsync("api/Transaccion/Lista");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi<List<Transaccion>>>(json_respuesta);
                lista = resultado.Response;
            }
            return lista;
        }

        public async Task<Transaccion> Obtener(int IdTransaccion, Login login)
        {
            Transaccion transaccion = new Transaccion();
            await Autenticar(login);

            var response = await _httpClient.GetAsync($"api/Transaccion/Obtener/{IdTransaccion}");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi<Transaccion>>(json_respuesta);
                transaccion = resultado.Response;
            }
            return transaccion;
        }

        public async Task<bool> Guardar(Transaccion transaccion, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var contenido = new StringContent(JsonConvert.SerializeObject(transaccion), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Transaccion/Guardar/", contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;
            }
            return Respuesta;
        }

        public async Task<bool> Editar(Transaccion transaccion, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var contenido = new StringContent(JsonConvert.SerializeObject(transaccion), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/Transaccion/Editar/", contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;
            }
            return Respuesta;
        }
        
        public async Task<bool> Eliminar(int IdTransaccion, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var response = await _httpClient.DeleteAsync($"api/Transaccion/Eliminar/{IdTransaccion}");

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;
            }
            return Respuesta;
        }
    }
}
