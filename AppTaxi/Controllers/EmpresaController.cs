using AppTaxi.Funciones;
using AppTaxi.Models;
using AppTaxi.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;

namespace AppTaxi.Controllers
{
    public class EmpresaController : Controller
    {
        private readonly I_Vehiculo _vehiculo;
        private readonly I_Horario _horario;
        private readonly I_Propietario _propietario;
        private readonly I_Empresa _empresa;
        private readonly I_Conductor _conductor;

        public EmpresaController(I_Vehiculo vehiculo, I_Horario horario, I_Propietario propietario, I_Empresa empresa, I_Conductor conductor)
        {
            _vehiculo = vehiculo;
            _horario = horario;
            _propietario = propietario;
            _empresa = empresa;
            _conductor = conductor;
        }



        public async Task<IActionResult> Inicio()
        {
            // Obtener el usuario de la sesión
            var usuarioJson = HttpContext.Session.GetString("Usuario");
            if (string.IsNullOrEmpty(usuarioJson))
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return View();
            }

            var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
            var login = new Models.Login { Correo = usuario.Correo, Contrasena = usuario.Contrasena };

            // Obtener listas de datos
            var empresasTotales = await _empresa.Lista(login);
            var vehiculosTotales = await _vehiculo.Lista(login);
            var horariosTotales = await _horario.Lista(login);

            // Validar listas
            if (empresasTotales == null || empresasTotales.Count() == 0)
            {
                ViewBag.Mensaje = "La lista de empresas está vacía.";
                return View();
            }

            // Obtener la empresa asociada al usuario
            var empresa = empresasTotales.FirstOrDefault(item => item.IdUsuario == usuario.IdUsuario);
            if (empresa == null)
            {
                ViewBag.Mensaje = "No hay empresa asociada a su usuario.";
                return View();
            }

            // Filtrar vehículos de la empresa
            var vehiculosEmpresa = vehiculosTotales?.Where(v => v.IdEmpresa == empresa.IdEmpresa).ToList();
            if (vehiculosEmpresa == null || vehiculosEmpresa.Count() == 0)
            {
                ViewBag.Mensaje = "La lista de vehículos está vacía.";
                return View();
            }

            // Construir la lista de datos iniciales
            var datosIniciales = new List<DatosEmpresa>();
            int i = 1;

            foreach (var vehiculo in vehiculosEmpresa)
            {
                var horario = horariosTotales.FirstOrDefault(h => h.IdVehiculo == vehiculo.IdVehiculo);
                if (horario == null) continue;

                var propietario = await _propietario.Obtener(vehiculo.IdPropietario, login);
                var conductor = await _conductor.Obtener(horario.IdConductor, login);

                datosIniciales.Add(new DatosEmpresa
                {
                    IdDato = i++,
                    Placa = vehiculo.Placa,
                    IdVehiculo = vehiculo.IdVehiculo,
                    NombrePropietario = propietario?.Nombre,
                    Conductor = conductor?.Nombre,
                    Fecha = horario.Fecha,
                    HoraInicio = horario.HoraInicio,
                    HoraFin = horario.HoraFin
                });
            }

            ViewBag.Mensaje = $"Bienvenid@ {usuario.Nombre}";
            return View(datosIniciales);
        } 

        public IActionResult Detalle()
        {
            return View();
        }

    }
}
