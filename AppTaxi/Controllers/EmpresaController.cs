using AppTaxi.Funciones;
using AppTaxi.Models;
using AppTaxi.Servicios;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        private async Task<Usuario> GetUsuarioFromSessionAsync()
        {
            var usuarioJson = HttpContext.Session.GetString("Usuario");
            if (string.IsNullOrEmpty(usuarioJson))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<Usuario>(usuarioJson);
        }

        private Models.Login CreateLogin(Usuario usuario)
        {
            return new Models.Login { Correo = usuario.Correo, Contrasena = usuario.Contrasena };
        }

        public async Task<IActionResult> Inicio()
        {
            var usuario = await GetUsuarioFromSessionAsync();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return View();
            }

            var login = CreateLogin(usuario);

            var empresasTotales = await _empresa.Lista(login);
            var vehiculosTotales = await _vehiculo.Lista(login);
            var horariosTotales = await _horario.Lista(login);

            if (empresasTotales == null || !empresasTotales.Any())
            {
                ViewBag.Mensaje = "La lista de empresas está vacía.";
                return View();
            }

            var empresa = empresasTotales.FirstOrDefault(item => item.IdUsuario == usuario.IdUsuario);
            if (empresa == null)
            {
                ViewBag.Mensaje = "No hay empresa asociada a su usuario.";
                return View();
            }

            var vehiculosEmpresa = vehiculosTotales?.Where(v => v.IdEmpresa == empresa.IdEmpresa).ToList();
            if (vehiculosEmpresa == null || !vehiculosEmpresa.Any())
            {
                ViewBag.Mensaje = "La lista de vehículos está vacía.";
                return View();
            }

            var datosIniciales = new List<DatosEmpresa>();
            int i = 1;

            foreach (var vehiculo in vehiculosEmpresa)
            {
                var horario = horariosTotales.FirstOrDefault(h => h.IdVehiculo == vehiculo.IdVehiculo);
                if (horario == null) continue;

                var propietario = await _propietario.Obtener(vehiculo.IdPropietario, login);
                var conductor = await _conductor.Obtener(horario.IdConductor, login);

                if (vehiculo.Estado)
                {
                    datosIniciales.Add(new DatosEmpresa
                    {
                        IdDato = i++,
                        Foto = conductor.Foto,
                        Placa = vehiculo.Placa,
                        IdVehiculo = vehiculo.IdVehiculo,
                        NombrePropietario = propietario?.Nombre,
                        Conductor = conductor?.Nombre,
                        Fecha = horario.Fecha,
                        HoraInicio = horario.HoraInicio,
                        HoraFin = horario.HoraFin
                    });
                }
            }

            ViewBag.Mensaje = $"Bienvenid@ {usuario.Nombre}";
            return View(datosIniciales);
        }

        public async Task<IActionResult> Detalle(int IdDato)
        {
            var usuario = await GetUsuarioFromSessionAsync();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);

            var empresasTotales = await _empresa.Lista(login);
            var vehiculosTotales = await _vehiculo.Lista(login);
            var horariosTotales = await _horario.Lista(login);

            if (empresasTotales == null || !empresasTotales.Any())
            {
                ViewBag.Mensaje = "La lista de empresas está vacía.";
                return View();
            }

            var empresa = empresasTotales.FirstOrDefault(item => item.IdUsuario == usuario.IdUsuario);
            if (empresa == null)
            {
                ViewBag.Mensaje = "No hay empresa asociada a su usuario.";
                return View();
            }

            var vehiculosEmpresa = vehiculosTotales?.Where(v => v.IdEmpresa == empresa.IdEmpresa).ToList();
            if (vehiculosEmpresa == null || !vehiculosEmpresa.Any())
            {
                ViewBag.Mensaje = "La lista de vehículos está vacía.";
                return View();
            }

            var datosIniciales = new List<DatosEmpresa>();
            int i = 1;

            foreach (var vehiculo in vehiculosEmpresa)
            {
                var horario = horariosTotales.FirstOrDefault(h => h.IdVehiculo == vehiculo.IdVehiculo);
                if (horario == null) continue;

                var propietario = await _propietario.Obtener(vehiculo.IdPropietario, login);
                var conductor = await _conductor.Obtener(horario.IdConductor, login);

                if (vehiculo.Estado)
                {
                    datosIniciales.Add(new DatosEmpresa
                    {
                        IdDato = i++,
                        Foto = conductor.Foto,
                        Placa = vehiculo.Placa,
                        IdVehiculo = vehiculo.IdVehiculo,
                        NombrePropietario = propietario?.Nombre,
                        Conductor = conductor?.Nombre,
                        Fecha = horario.Fecha,
                        HoraInicio = horario.HoraInicio,
                        HoraFin = horario.HoraFin
                    });
                }
            }

            var dato = datosIniciales.FirstOrDefault(item => item.IdDato == IdDato);
            return View(dato);
        }

        public async Task<IActionResult> Vehiculos()
        {
            var usuario = await GetUsuarioFromSessionAsync();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);

            var empresas = await _empresa.Lista(login);
            var vehiculosTotales = await _vehiculo.Lista(login);

            var idEmpresa = empresas.FirstOrDefault(item => item.IdUsuario == usuario.IdUsuario)?.IdEmpresa;
            var vehiculosEmpresa = vehiculosTotales?.Where(v => v.IdEmpresa == idEmpresa && v.Estado).ToList();

            return View(vehiculosEmpresa);
        }

        public async Task<IActionResult> Detalle_Vehiculo(int IdVehiculo)
        {
            var usuario = await GetUsuarioFromSessionAsync();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);

            var vehiculo = await _vehiculo.Obtener(IdVehiculo, login);
            var propietario = await _propietario.Obtener(vehiculo.IdPropietario, login);

            ViewBag.Propietario = propietario?.Nombre;
            return View(vehiculo);
        }

        public async Task<IActionResult> Editar_Vehiculo(int IdVehiculo)
        {
            var usuario = await GetUsuarioFromSessionAsync();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            var vehiculo = await _vehiculo.Obtener(IdVehiculo, login);

            return View(vehiculo);
        }

        [HttpPost]
        public async Task<IActionResult> Guardar_Vehiculo(Vehiculo vehiculo)
        {
            var usuario = await GetUsuarioFromSessionAsync();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            bool respuesta = await _vehiculo.Editar(vehiculo, login);

            if (respuesta)
            {
                return RedirectToAction("Vehiculos");
            }
            else
            {
                ViewBag.Mensaje = "No se pudo Guardar";
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar_Vehiculo(int IdVehiculo)
        {
            var usuario = await GetUsuarioFromSessionAsync();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            var vehiculo = await _vehiculo.Obtener(IdVehiculo, login);
            vehiculo.Estado = false;

            bool respuesta = await _vehiculo.Editar(vehiculo, login);

            if (respuesta)
            {
                return RedirectToAction("Vehiculos");
            }
            else
            {
                ViewBag.Mensaje = "No se pudo Guardar";
                return NoContent();
            }
        }

        public IActionResult Agregar_Vehiculo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear_Vehiculo(Vehiculo vehiculo)
        {
            var usuario = await GetUsuarioFromSessionAsync();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            var empresas = await _empresa.Lista(login);
            var vehiculos = await _vehiculo.Lista(login);

            vehiculo.Estado = true;
            vehiculo.IdEmpresa = empresas.FirstOrDefault(e => e.IdUsuario == usuario.IdUsuario)?.IdEmpresa ?? 0;
            vehiculo.Placa = vehiculo.Placa.ToUpper();

            if (vehiculos.Any(v => v.Placa == vehiculo.Placa))
            {
                ViewBag.Mensaje = "La placa ya está en uso";
                return View("Agregar_Vehiculo");
            }

            bool respuesta = await _vehiculo.Guardar(vehiculo, login);

            if (respuesta)
            {
                var vehiculosGuardados = await _vehiculo.Lista(login);
                var vehiculoGuardado = vehiculosGuardados.FirstOrDefault(v => v.Placa == vehiculo.Placa);
                ViewBag.IdVehiculo = vehiculoGuardado?.IdVehiculo;
                ViewBag.Exito = true;
                return View("Agregar_Vehiculo");
            }
            else
            {
                ViewBag.Mensaje = $"No se pudo Guardar {vehiculo.Placa}";
                return View("Agregar_Vehiculo");
            }
        }
    }
}