using AppTaxi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AppTaxi.Servicios
{
    public class API_Invitado : Autenticacion, I_Invitado
    {

        public async Task<Invitado> Consulta(Consulta consulta)
        {
            Invitado invitado = new Invitado();
            await Autenticar();
            
            if (!string.IsNullOrWhiteSpace(consulta.Placa))
            {
                var contenido = new StringContent(JsonConvert.SerializeObject(consulta.Placa), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"api/Invitado/Placa", contenido);

                if (response.IsSuccessStatusCode)
                {
                    var json_respuesta = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<ResultadoApi<Invitado>>(json_respuesta);
                    invitado = resultado.Response;

                }
            }
            else if (consulta.Documento != 0)
            {
                var contenido = new StringContent(JsonConvert.SerializeObject(consulta.Documento), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"api/Invitado/Documento", contenido);

                if (response.IsSuccessStatusCode)
                {
                    var json_respuesta = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<ResultadoApi<Invitado>>(json_respuesta);
                    invitado = resultado.Response;

                }
            }

            return invitado;
        }


    }

    
}
