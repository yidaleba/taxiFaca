using AppTaxi.Models;
using Newtonsoft.Json;
using System.Text;

namespace AppTaxi.Servicios
{
    public class API_Rol : Autenticacion, I_Rol
    {

        public async Task<List<Rol>> Lista(Login login)
        {
            List<Rol> lista = new List<Rol>();
            await Autenticar(login);

            var response = await _httpClient.GetAsync("api/Rol/Lista");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi<List<Rol>>>(json_respuesta);
                lista = resultado.Response;

            }
            return lista;

        }

        public async Task<Rol> Obtener(int IdRol, Login login)
        {
            Rol rol = new Rol();
            await Autenticar(login);

            var response = await _httpClient.GetAsync($"api/Rol/Obtener/{IdRol}");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi<Rol>>(json_respuesta);
                rol = resultado.Response;

            }
            return rol;
        }

        public async Task<bool> Guardar(Rol rol, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var contenido = new StringContent(JsonConvert.SerializeObject(rol), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Rol/Guardar/", contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }

        public async Task<bool> Editar(Rol rol, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var contenido = new StringContent(JsonConvert.SerializeObject(rol), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/Rol/Editar/", contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }

        public async Task<bool> Eliminar(int IdRol, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var response = await _httpClient.DeleteAsync($"api/Rol/Eliminar/{IdRol}");

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }




    }
}
