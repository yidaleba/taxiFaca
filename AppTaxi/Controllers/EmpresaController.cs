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
        // Servicios inyectados para manejar las operaciones de vehículos, horarios, propietarios, empresas y conductores.
        private readonly I_Vehiculo _vehiculo;
        private readonly I_Horario _horario;
        private readonly I_Propietario _propietario;
        private readonly I_Empresa _empresa;
        private readonly I_Conductor _conductor;

        // Constructor que recibe las dependencias inyectadas.
        public EmpresaController(I_Vehiculo vehiculo, I_Horario horario, I_Propietario propietario, I_Empresa empresa, I_Conductor conductor)
        {
            _vehiculo = vehiculo;
            _horario = horario;
            _propietario = propietario;
            _empresa = empresa;
            _conductor = conductor;
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

        //------------ Acciones principales ------------

        // Muestra la página de inicio con los datos de la empresa, vehículos, horarios y conductores asociados.
        public async Task<IActionResult> Inicio()
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return View();
            }

            var login = CreateLogin(usuario);

            // Obtiene las listas de empresas, vehículos y horarios.
            var empresasTotales = await _empresa.Lista(login);
            var vehiculosTotales = await _vehiculo.Lista(login);
            var horariosTotales = await _horario.Lista(login);

            // Valida si hay empresas registradas.
            if (empresasTotales == null || !empresasTotales.Any())
            {
                ViewBag.Mensaje = "La lista de empresas está vacía.";
                return View();
            }

            // Obtiene la empresa asociada al usuario actual.
            var empresa = empresasTotales.FirstOrDefault(item => item.IdUsuario == usuario.IdUsuario);
            if (empresa == null)
            {
                ViewBag.Mensaje = "No hay empresa asociada a su usuario.";
                return View();
            }

            // Filtra los vehículos asociados a la empresa.
            var vehiculosEmpresa = vehiculosTotales?.Where(v => v.IdEmpresa == empresa.IdEmpresa).ToList();
            if (vehiculosEmpresa == null || !vehiculosEmpresa.Any())
            {
                ViewBag.Mensaje = "La lista de vehículos está vacía.";
                return View();
            }

            // Construye la lista de datos iniciales para mostrar en la vista.
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

        // Muestra los detalles de un registro específico (vehículo, conductor, horario, etc.).
        public async Task<IActionResult> Detalle(int IdDato)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);

            // Obtiene las listas de empresas, vehículos y horarios.
            var empresasTotales = await _empresa.Lista(login);
            var vehiculosTotales = await _vehiculo.Lista(login);
            var horariosTotales = await _horario.Lista(login);

            // Valida si hay empresas registradas.
            if (empresasTotales == null || !empresasTotales.Any())
            {
                ViewBag.Mensaje = "La lista de empresas está vacía.";
                return View();
            }

            // Obtiene la empresa asociada al usuario actual.
            var empresa = empresasTotales.FirstOrDefault(item => item.IdUsuario == usuario.IdUsuario);
            if (empresa == null)
            {
                ViewBag.Mensaje = "No hay empresa asociada a su usuario.";
                return View();
            }

            // Filtra los vehículos asociados a la empresa.
            var vehiculosEmpresa = vehiculosTotales?.Where(v => v.IdEmpresa == empresa.IdEmpresa).ToList();
            if (vehiculosEmpresa == null || !vehiculosEmpresa.Any())
            {
                ViewBag.Mensaje = "La lista de vehículos está vacía.";
                return View();
            }

            // Construye la lista de datos iniciales para mostrar en la vista.
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

            // Obtiene el registro específico por su IdDato.
            var dato = datosIniciales.FirstOrDefault(item => item.IdDato == IdDato);
            return View(dato);
        }

        // Muestra la lista de vehículos asociados a la empresa del usuario.
        public async Task<IActionResult> Vehiculos()
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);

            // Obtiene las empresas y vehículos asociados al usuario.
            var empresas = await _empresa.Lista(login);
            var vehiculosTotales = await _vehiculo.Lista(login);

            // Filtra los vehículos asociados a la empresa del usuario.
            var idEmpresa = empresas.FirstOrDefault(item => item.IdUsuario == usuario.IdUsuario)?.IdEmpresa;
            var vehiculosEmpresa = vehiculosTotales?.Where(v => v.IdEmpresa == idEmpresa && v.Estado).ToList();

            return View(vehiculosEmpresa);
        }

        // Muestra los detalles de un vehículo específico.
        public async Task<IActionResult> Detalle_Vehiculo(int IdVehiculo)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);

            // Obtiene el vehículo y el propietario asociado.
            var vehiculo = await _vehiculo.Obtener(IdVehiculo, login);
            var propietario = await _propietario.Obtener(vehiculo.IdPropietario, login);

            ViewBag.Propietario = propietario?.Nombre;
            return View(vehiculo);
        }

        // Muestra el formulario para editar un vehículo.
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

        // Guarda los cambios realizados en un vehículo.
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

        // Desactiva un vehículo (cambia su estado a false).
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

        // Muestra el formulario para agregar un nuevo vehículo.
        public async Task<IActionResult> Agregar_Vehiculo()
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }
            var login = CreateLogin(usuario);

            // Obtiene las empresas y propietarios asociados al usuario.
            var empresas = await _empresa.Lista(login);
            var IdEmpresa = empresas.FirstOrDefault(item => item.IdUsuario == usuario.IdUsuario)?.IdEmpresa;

            var propietariosTotales = await _propietario.Lista(login);
            var propietariosEmpresa = propietariosTotales?.Where(p => p.IdEmpresa == IdEmpresa && p.Estado).ToList();

            // Crea el ViewModel con el vehículo y la lista de propietarios.
            var viewModel = new ModeloVista
            {
                Vehiculo = new Vehiculo(), // Inicializa el objeto Vehiculo
                Propietarios = propietariosEmpresa // Asigna la lista de propietarios
            };

            return View(viewModel);
        }

        // Guarda un nuevo vehículo en la base de datos.
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

            // Asigna valores al vehículo.
            viewModel.Vehiculo.Estado = true;
            viewModel.Vehiculo.IdEmpresa = empresas.FirstOrDefault(e => e.IdUsuario == usuario.IdUsuario)?.IdEmpresa ?? 0;
            viewModel.Vehiculo.Placa = viewModel.Vehiculo.Placa.ToUpper();

            // Valida si la placa ya está en uso.
            if (vehiculos.Any(v => v.Placa == viewModel.Vehiculo.Placa))
            {
                ViewBag.Mensaje = "La placa ya está en uso";
                var propietariosTotales = await _propietario.Lista(login);
                viewModel.Propietarios = propietariosTotales?.Where(p => p.IdEmpresa == viewModel.Vehiculo.IdEmpresa && p.Estado).ToList();

                return View("Agregar_Vehiculo", viewModel);
            }

            // Guarda el vehículo.
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

        // Muestra la lista de conductores asociados a la empresa del usuario.
        public async Task<IActionResult> Conductores()
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);

            // Obtiene las empresas y conductores asociados al usuario.
            var empresas = await _empresa.Lista(login);
            var IdEmpresa = empresas.FirstOrDefault(item => item.IdUsuario == usuario.IdUsuario)?.IdEmpresa;

            var conductoresTotales = await _conductor.Lista(login);
            var conductoresEmpresa = conductoresTotales?.Where(c => c.IdEmpresa == IdEmpresa && c.Estado).ToList();
            return View(conductoresEmpresa);
        }

        // Muestra los detalles de un conductor específico.
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

        // Muestra el formulario para editar un conductor.
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

        // Guarda los cambios realizados en un conductor.
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

        // Desactiva un conductor (cambia su estado a false).
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

        // Muestra el formulario para agregar un nuevo conductor.
        public IActionResult Agregar_Conductor()
        {
            return View();
        }

        // Guarda un nuevo conductor en la base de datos.
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

            // Valida si el conductor ya está registrado.
            if (conductores.Any(c => c.NumeroCedula == conductor.NumeroCedula))
            {
                ViewBag.Mensaje = "El conductor ya está registrado";
                return View("Agregar_Conductor");
            }

            // Guarda el conductor.
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

        // Muestra la lista de propietarios asociados a la empresa del usuario.
        public async Task<IActionResult> Propietarios()
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);

            // Obtiene las empresas y propietarios asociados al usuario.
            var empresas = await _empresa.Lista(login);
            var IdEmpresa = empresas.FirstOrDefault(item => item.IdUsuario == usuario.IdUsuario)?.IdEmpresa;

            var propietariosTotales = await _propietario.Lista(login);
            var propietariosEmpresa = propietariosTotales?.Where(p => p.IdEmpresa == IdEmpresa && p.Estado).ToList();
            return View(propietariosEmpresa);
        }

        // Muestra los detalles de un propietario específico.
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

            // Obtiene el propietario y los vehículos asociados.
            modelo.Propietario = await _propietario.Obtener(IdPropietario, login);

            var vehiculosTotales = await _vehiculo.Lista(login);
            modelo.Vehiculos = vehiculosTotales?.Where(v => v.IdPropietario == IdPropietario && v.Estado).ToList();

            return View(modelo);
        }

        // Muestra el formulario para editar un propietario.
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

        // Guarda los cambios realizados en un propietario.
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

        // Desactiva un propietario (cambia su estado a false).
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

        // Muestra el formulario para agregar un nuevo propietario.
        public IActionResult Agregar_Propietario()
        {
            return View();
        }

        // Guarda un nuevo propietario en la base de datos.
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

            // Valida si el propietario ya está registrado.
            if (propietarios.Any(c => c.NumeroCedula == propietario.NumeroCedula))
            {
                ViewBag.Mensaje = "El propietario ya está registrado";
                return View("Agregar_Propietario");
            }

            // Guarda el propietario.
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

        public async Task<IActionResult> Ver_Horario(int IdConductor)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            ModeloVista modelo = new ModeloVista();
            modelo.Conductor = await _conductor.Obtener(IdConductor, login);
            var Horarios = await _horario.Lista(login);
            modelo.Vehiculos = await _vehiculo.Lista(login);
                       
            modelo.Horarios = Horarios?.Where(h => h.IdConductor == IdConductor).ToList();
            

            return View(modelo);
        }
    }
}