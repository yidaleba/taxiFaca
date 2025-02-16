using AppTaxi.Models;
using Newtonsoft.Json;
using System.Text;

namespace AppTaxi.Servicios
{
    public class API_Horario : Autenticacion, I_Horario
    {

        public async Task<List<Horario>> Lista(Login login)
        {
            List<Horario> lista = new List<Horario>();
            await Autenticar(login);

            var response = await _httpClient.GetAsync("api/Horario/Lista");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi<List<Horario>>>(json_respuesta);
                lista = resultado.Response;

            }
            return lista;

        }

        public async Task<Horario> Obtener(int IdHorario, Login login)
        {
            Horario horario = new Horario();
            await Autenticar(login);

            var response = await _httpClient.GetAsync($"api/Horario/Obtener/{IdHorario}");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi<Horario>>(json_respuesta);
                horario = resultado.Response;

            }
            return horario;
        }

        public async Task<bool> Guardar(Horario horario, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var contenido = new StringContent(JsonConvert.SerializeObject(horario), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Horario/Guardar/", contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }

        public async Task<bool> Editar(Horario horario, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var contenido = new StringContent(JsonConvert.SerializeObject(horario), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/Horario/Editar/", contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }

        public async Task<bool> Eliminar(int IdHorario, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var response = await _httpClient.DeleteAsync($"api/Horario/Eliminar/{IdHorario}");

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }




    }
}
