using AppTaxi.Funciones;
using AppTaxi.Models;
using AppTaxi.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
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

        //------------Usuario Loggeado
        private Usuario GetUsuarioFromSession()
        {
            var usuarioJson = HttpContext.Session.GetString("Usuario");
            if (string.IsNullOrEmpty(usuarioJson))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<Usuario>(usuarioJson);
        }

        //---------- Creación del login
        private Models.Login CreateLogin(Usuario usuario)
        {
            return new Models.Login { Correo = usuario.Correo, Contrasena = usuario.Contrasena };
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
            var usuario = GetUsuarioFromSession();
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
            var usuario = GetUsuarioFromSession();
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
            var usuario = GetUsuarioFromSession();
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
            var usuario = GetUsuarioFromSession();
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
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            vehiculo.Estado = true;
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
            var usuario = GetUsuarioFromSession();
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

        public async Task<IActionResult> Agregar_Vehiculo()
        {
            // Obtener el usuario de la sesión
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }
            var login = CreateLogin(usuario);

            var empresas = await _empresa.Lista(login);

            var IdEmpresa = empresas.FirstOrDefault(item => item.IdUsuario == usuario.IdUsuario)?.IdEmpresa;
            //ViewBag.Mensaje = $"Hola {IdEmpresa}";

            var propietariosTotales = await _propietario.Lista(login);
            var propietariosEmpresa = propietariosTotales?.Where(p => p.IdEmpresa == IdEmpresa && p.Estado).ToList();

            // Crear el ViewModel
            var viewModel = new ModeloVista
            {
                Vehiculo = new Vehiculo(), // Inicializar el objeto Vehiculo
                Propietarios = propietariosEmpresa // Obtener la lista de propietarios
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Crear_Vehiculo(ModeloVista viewModel)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            var empresas = await _empresa.Lista(login);
            var vehiculos = await _vehiculo.Lista(login);

            // Asignar valores al vehículo
            viewModel.Vehiculo.Estado = true;
            viewModel.Vehiculo.IdEmpresa = empresas.FirstOrDefault(e => e.IdUsuario == usuario.IdUsuario)?.IdEmpresa ?? 0;
            viewModel.Vehiculo.Placa = viewModel.Vehiculo.Placa.ToUpper();

            // Validar si la placa ya está en uso
            if (vehiculos.Any(v => v.Placa == viewModel.Vehiculo.Placa))
            {
                ViewBag.Mensaje = "La placa ya está en uso";
                var propietariosTotales = await _propietario.Lista(login);
                viewModel.Propietarios = propietariosTotales?.Where(p => p.IdEmpresa == viewModel.Vehiculo.IdEmpresa && p.Estado).ToList();

                return View("Agregar_Vehiculo", viewModel);
            }

            // Guardar el vehículo
            bool respuesta = await _vehiculo.Guardar(viewModel.Vehiculo, login);

            if (respuesta)
            {
                var vehiculosGuardados = await _vehiculo.Lista(login);
                var vehiculoGuardado = vehiculosGuardados.FirstOrDefault(v => v.Placa == viewModel.Vehiculo.Placa);
                ViewBag.IdVehiculo = vehiculoGuardado?.IdVehiculo;
                ViewBag.Exito = true;
                return RedirectToAction("Vehiculos");
            }
            else
            {
                ViewBag.Mensaje = $"No se pudo Guardar {viewModel.Vehiculo.Placa}";
                var propietariosTotales = await _propietario.Lista(login);
                viewModel.Propietarios = propietariosTotales?.Where(p => p.IdEmpresa == viewModel.Vehiculo.IdEmpresa && p.Estado).ToList();
                
                return View("Agregar_Vehiculo", viewModel);
            }
        }

        //---------------------------- Conductores ------------------------------------

        public async Task<IActionResult> Conductores()
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);

            var empresas = await _empresa.Lista(login);

            var IdEmpresa = empresas.FirstOrDefault(item => item.IdUsuario == usuario.IdUsuario)?.IdEmpresa;
            //ViewBag.Mensaje = $"Hola {IdEmpresa}";

            var conductoresTotales = await _conductor.Lista(login);
            var conductoresEmpresa = conductoresTotales?.Where(c => c.IdEmpresa == IdEmpresa && c.Estado).ToList();
            return View(conductoresEmpresa);
        }

        public async Task<IActionResult> Detalle_Conductor(int IdConductor)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);

            var conductor = await _conductor.Obtener(IdConductor, login);
            
            return View(conductor);
        }

        public async Task<IActionResult> Editar_Conductor(int IdConductor)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            var conductor = await _conductor.Obtener(IdConductor, login);

            return View(conductor);
        }

        [HttpPost]
        public async Task<IActionResult> Guardar_Conductor(Conductor conductor)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            conductor.Estado = true;
            bool respuesta = await _conductor.Editar(conductor, login);

            if (respuesta)
            {
                return RedirectToAction("Conductores");
            }
            else
            {
                ViewBag.Mensaje = "No se pudo Guardar";
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar_Conductor(int IdConductor)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            var conductor = await _conductor.Obtener(IdConductor, login);
            conductor.Estado = false;

            bool respuesta = await _conductor.Editar(conductor, login);

            if (respuesta)
            {
                return RedirectToAction("Conductores");
            }
            else
            {
                ViewBag.Mensaje = "No se pudo Guardar";
                return NoContent();
            }
        }

        public IActionResult Agregar_Conductor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear_Conductor(Conductor conductor)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            var empresas = await _empresa.Lista(login);
            var conductores = await _conductor.Lista(login);

            conductor.Estado = true;
            conductor.IdEmpresa = empresas.FirstOrDefault(e => e.IdUsuario == usuario.IdUsuario)?.IdEmpresa ?? 0;

            if (conductores.Any(c => c.NumeroCedula == conductor.NumeroCedula))
            {
                ViewBag.Mensaje = "El conductor ya está registrado";
                return View("Agregar_Conductor");
            }

            bool respuesta = await _conductor.Guardar(conductor, login);
            
            if (respuesta)
            {
                var conductoresGuardados = await _conductor.Lista(login);
                var conductorGuardado = conductoresGuardados.FirstOrDefault(c => c.NumeroCedula == conductor.NumeroCedula);
                ViewBag.IdConductor = conductorGuardado?.IdConductor;
                ViewBag.Exito = true;
                return View("Conductores");
            }
            else
            {
                ViewBag.Mensaje = $"No se pudo Guardar {conductor.IdEmpresa}";
                return View("Agregar_Conductor");
            }
            

        }

        //---------------------------- Propietarios ------------------------------------


        public async Task<IActionResult> Propietarios()
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);

            var empresas = await _empresa.Lista(login);

            var IdEmpresa = empresas.FirstOrDefault(item => item.IdUsuario == usuario.IdUsuario)?.IdEmpresa;
            //ViewBag.Mensaje = $"Hola {IdEmpresa}";

            var propietariosTotales = await _propietario.Lista(login);
            var propietariosEmpresa = propietariosTotales?.Where(p => p.IdEmpresa == IdEmpresa && p.Estado).ToList();
            return View(propietariosEmpresa);
        }

        public async Task<IActionResult> Detalle_Propietario(int IdPropietario)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }
            ModeloVista modelo = new ModeloVista();
            var login = CreateLogin(usuario);

            modelo.Propietario = await _propietario.Obtener(IdPropietario, login);

            var vehiculosTotales = await _vehiculo.Lista(login);
            modelo.Vehiculos = vehiculosTotales?.Where(v => v.IdPropietario == IdPropietario && v.Estado).ToList();

            
            
            return View(modelo);
        }

        public async Task<IActionResult> Editar_Propietario(int IdPropietario)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            var propietario = await _propietario.Obtener(IdPropietario, login);

            return View(propietario);
        }

        [HttpPost]
        public async Task<IActionResult> Guardar_Propietario(Propietario propietario)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            propietario.Estado = true;
            bool respuesta = await _propietario.Editar(propietario, login);

            if (respuesta)
            {
                return RedirectToAction("Propietarios");
            }
            else
            {
                ViewBag.Mensaje = "No se pudo Guardar";
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar_Propietario(int IdPropietario)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            var propietario = await _propietario.Obtener(IdPropietario, login);
            propietario.Estado = false;

            bool respuesta = await _propietario.Editar(propietario, login);

            if (respuesta)
            {
                return RedirectToAction("Propietarios");
            }
            else
            {
                ViewBag.Mensaje = "No se pudo Guardar";
                return NoContent();
            }
        }

        public IActionResult Agregar_Propietario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear_Propietario(Propietario propietario)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            var empresas = await _empresa.Lista(login);
            var propietarios = await _propietario.Lista(login);

            propietario.Estado = true;
            propietario.IdEmpresa = empresas.FirstOrDefault(e => e.IdUsuario == usuario.IdUsuario)?.IdEmpresa ?? 0;

            if (propietarios.Any(c => c.NumeroCedula == propietario.NumeroCedula))
            {
                ViewBag.Mensaje = "El propietario ya está registrado";
                return View("Agregar_Propietario");
            }

            bool respuesta = await _propietario.Guardar(propietario, login);

            if (respuesta)
            {
                var propietariosGuardados = await _propietario.Lista(login);
                var propietarioGuardado = propietariosGuardados.FirstOrDefault(p => p.NumeroCedula == propietario.NumeroCedula);
                ViewBag.IdPropietario = propietario?.IdPropietario;
                ViewBag.Exito = true;
                return View("Propietarios");
            }
            else
            {
                ViewBag.Mensaje = $"No se pudo Guardar {propietario.IdEmpresa}";
                return View("Agregar_Propietario");
            }


        }
    }
}