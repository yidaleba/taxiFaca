using AppTaxi.Models;
using Newtonsoft.Json;
using System.Text;

namespace AppTaxi.Servicios
{
    public class API_Vehiculo : Autenticacion, I_Vehiculo
    {

        public async Task<List<Vehiculo>> Lista(Login login)
        {
            List<Vehiculo> lista = new List<Vehiculo>();
            await Autenticar(login);

            var response = await _httpClient.GetAsync("api/Vehiculo/Lista");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi<List<Vehiculo>>>(json_respuesta);
                lista = resultado.Response;

            }
            return lista;

        }

        public async Task<Vehiculo> Obtener(int IdVehiculo, Login login)
        {
            Vehiculo vehiculo = new Vehiculo();
            await Autenticar(login);

            var response = await _httpClient.GetAsync($"api/Vehiculo/Obtener/{IdVehiculo}");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi<Vehiculo>>(json_respuesta);
                vehiculo = resultado.Response;

            }
            return vehiculo;
        }

        public async Task<bool> Guardar(Vehiculo vehiculo, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var contenido = new StringContent(JsonConvert.SerializeObject(vehiculo), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Vehiculo/Guardar/", contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }

        public async Task<bool> Editar(Vehiculo vehiculo, Login login)
        //public async Task<string> Editar(Vehiculo vehiculo, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var contenido = new StringContent(JsonConvert.SerializeObject(vehiculo), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/Vehiculo/Editar/", contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
            //return response.ToString();
        }

        public async Task<bool> Eliminar(int IdVehiculo, Login login)
        {
            bool Respuesta = false;
            await Autenticar(login);

            var response = await _httpClient.DeleteAsync($"api/Vehiculo/Eliminar/{IdVehiculo}");

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;

            }
            return Respuesta;
        }




    }
}
