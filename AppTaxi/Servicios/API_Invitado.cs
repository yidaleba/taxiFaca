using AppTaxi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AppTaxi.Servicios
{
    public class API_Invitado : Autenticacion, I_Invitado
    {

        public async Task<Invitado> Placa(string Placa)
        {
            Invitado invitado = new Invitado();
            await Autenticar();

            var contenido = new StringContent(JsonConvert.SerializeObject(Placa), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/Invitado/Placa",contenido);

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi<Invitado>>(json_respuesta);
                invitado = resultado.Response;

            }
            return invitado;
        }

        public async Task<Invitado> Documento(long Documento)
        {
            Invitado invitado = new Invitado();
            await Autenticar();

            var contenido = new StringContent(JsonConvert.SerializeObject(Documento), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/Invitado/Placa", contenido);

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi<Invitado>>(json_respuesta);
                invitado = resultado.Response;

            }
            return invitado;
        }

    }

    
}
