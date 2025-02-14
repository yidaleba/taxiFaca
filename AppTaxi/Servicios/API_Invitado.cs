using AppTaxi.Models;
using Newtonsoft.Json;
using System.Text;

namespace AppTaxi.Servicios
{
    public class API_Invitado : Autenticacion, I_Invitado
    {

        public async Task<List<Invitado>> Lista()
        {
            List<Invitado> lista = new List<Invitado>();
            await Autenticar();

            var response = await _httpClient.GetAsync("api/Invitado/Lista");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi<List<Invitado>>>(json_respuesta);
                lista = resultado.Response;

            }
            return lista;

        }

        public async Task<Invitado> Obtener(int IdInvitado)
        {
            Invitado invitado = new Invitado();
            await Autenticar();

            var response = await _httpClient.GetAsync($"api/Invitado/Obtener/{IdInvitado}");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi<Invitado>>(json_respuesta);
                invitado = resultado.Response;

            }
            return invitado;
        }

        public async Task<bool> Guardar(Invitado invitado)
        {
            bool Respuesta = false;
            await Autenticar();

            var contenido = new StringContent(JsonConvert.SerializeObject(invitado), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Invitado/Guardar/", contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }

        public async Task<bool> Editar(Invitado invitado)
        {
            bool Respuesta = false;
            await Autenticar();

            var contenido = new StringContent(JsonConvert.SerializeObject(invitado), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/Invitado/Editar/", contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }

        public async Task<bool> Eliminar(int IdInvitado)
        {
            bool Respuesta = false;
            await Autenticar();

            var response = await _httpClient.DeleteAsync($"api/Invitado/Eliminar/{IdInvitado}");

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }




    }
}
