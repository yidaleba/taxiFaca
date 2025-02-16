using AppTaxi.Models;
using Newtonsoft.Json;
using System.Text;

namespace AppTaxi.Servicios
{
    public class API_Usuario : Autenticacion, I_Usuario
    {

        public async Task<List<Usuario>> Lista(Login login)
        {
            List<Usuario> lista = new List<Usuario>();
            
            await Autenticar(login);
            
            var response = await _httpClient.GetAsync("api/Usuario/Lista");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi<List<Usuario>>>(json_respuesta);
                lista = resultado.Response;

            }
            return lista;

        }

        public async Task<Usuario> Obtener(int IdUsuario, Login login)
        {
            Usuario usuario = new Usuario();
            await Autenticar(login);

            var response = await _httpClient.GetAsync($"api/Usuario/Obtener/{IdUsuario}");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi<Usuario>>(json_respuesta);
                usuario = resultado.Response;

            }
            return usuario;
        }

        public async Task<bool> Guardar(Usuario usuario, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var contenido = new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Usuario/Guardar/", contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }

        public async Task<bool> Editar(Usuario usuario, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var contenido = new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/Usuario/Editar/", contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }

        public async Task<bool> Eliminar(int IdUsuario, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var response = await _httpClient.DeleteAsync($"api/Usuario/Eliminar/{IdUsuario}");

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }




    }
}
