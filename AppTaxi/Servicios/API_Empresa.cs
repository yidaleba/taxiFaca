using AppTaxi.Models;
using Newtonsoft.Json;
using System.Text;

namespace AppTaxi.Servicios
{
    public class API_Empresa : Autenticacion, I_Empresa
    {

        public async Task<List<Empresa>> Lista(Login login)
        {
            List<Empresa> lista = new List<Empresa>();
            await Autenticar(login);

            var response = await _httpClient.GetAsync("api/Empresa/Lista");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi<List<Empresa>>>(json_respuesta);
                lista = resultado.Response;

            }
            return lista;

        }

        public async Task<Empresa> Obtener(int IdEmpresa, Login login)
        {
            Empresa empresa = new Empresa();
            await Autenticar(login);

            var response = await _httpClient.GetAsync($"api/Empresa/Obtener/{IdEmpresa}");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi<Empresa>>(json_respuesta);
                empresa = resultado.Response;

            }
            return empresa;
        }

        public async Task<bool> Guardar(Empresa empresa, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var contenido = new StringContent(JsonConvert.SerializeObject(empresa), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Empresa/Guardar/", contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }

        public async Task<bool> Editar(Empresa empresa, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var contenido = new StringContent(JsonConvert.SerializeObject(empresa), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/Empresa/Editar/", contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }

        public async Task<bool> Eliminar(int IdEmpresa, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var response = await _httpClient.DeleteAsync($"api/Empresa/Eliminar/{IdEmpresa}");

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }


        

    }
}
