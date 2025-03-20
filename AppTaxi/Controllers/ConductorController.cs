using AppTaxi.Models;
using AppTaxi.Servicios;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AppTaxi.Controllers
{
    public class ConductorController : Controller
    {
        // Servicios inyectados para manejar las operaciones de vehículos, horarios, propietarios, empresas y conductores.
        private readonly I_Vehiculo _vehiculo;
        private readonly I_Horario _horario;
        private readonly I_Propietario _propietario;
        private readonly I_Empresa _empresa;
        private readonly I_Conductor _conductor;
        private readonly I_Usuario _usuario;
        private readonly I_Transaccion _transaccion;

        // Constructor que recibe las dependencias inyectadas.
        public ConductorController(I_Vehiculo vehiculo, I_Horario horario, I_Propietario propietario, I_Empresa empresa, I_Conductor conductor, I_Usuario usuario, I_Transaccion transaccion)
        {
            _vehiculo = vehiculo;
            _horario = horario;
            _propietario = propietario;
            _empresa = empresa;
            _conductor = conductor;
            _usuario = usuario;
            _transaccion = transaccion;
        }

        //------------ Métodos auxiliares ------------

        // Obtiene el usuario actual desde la sesión.
        private Usuario GetUsuarioFromSession()
        {
            var usuarioJson = HttpContext.Session.GetString("Usuario");
            if (string.IsNullOrEmpty(usuarioJson))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<Usuario>(usuarioJson);
        }

        // Crea un objeto Login a partir del usuario actual.
        private Models.Login CreateLogin(Usuario usuario)
        {
            return new Models.Login { Correo = usuario.Correo, Contrasena = usuario.Contrasena };
        }

        private Transaccion Crear_Transaccion(string accion, string modelo)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                Console.WriteLine("No Hay Usuario Registrado");
            }

            var login = CreateLogin(usuario);

            Transaccion transaccion = new Transaccion();

            transaccion.IdUsuario = usuario.IdUsuario;
            transaccion.Modelo = modelo;
            transaccion.Accion = accion;
            transaccion.Fecha = DateTime.Now.Date;
            transaccion.Hora = DateTime.Now.TimeOfDay;
            return transaccion;

        }
        public async Task<IActionResult> Inicio()
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return View();
            }

            var login = CreateLogin(usuario);

            var usuarios = await _usuario.Lista(login);

            return View(usuario);
            
        }
    }
}
