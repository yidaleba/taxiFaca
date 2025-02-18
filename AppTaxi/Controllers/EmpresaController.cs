using AppTaxi.Models;
using AppTaxi.Servicios;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AppTaxi.Controllers
{
    public class EmpresaController : Controller
    {
        private readonly API_Vehiculo _vehiculo;
        private readonly API_Horario _horario;
        private readonly API_Propietario _propietario;
        private readonly API_Empresa _empresa;
        private readonly API_Conductor _conductor;

        public EmpresaController(API_Vehiculo vehiculo, API_Horario horario, API_Propietario propietario, API_Empresa empresa, API_Conductor conductor)
        {
            _vehiculo = vehiculo;
            _horario = horario;
            _propietario = propietario;
            _empresa = empresa;
            _conductor = conductor;
        }

        public async Task<IActionResult> Inicio()
        {
            Empresa emp = new Empresa();
            var usuarioJson = TempData["Usuario"] as string;
            var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
            //emp = await _empresa.Lista(new Login { Correo = usuario.Correo, Contrasena = usuario.Contrasena });


            ViewBag.Mensaje = $"Hola {usuario.Nombre}";
            return View();
        }
    }
}
