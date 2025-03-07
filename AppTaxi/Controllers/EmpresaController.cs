using AppTaxi.Funciones;
using AppTaxi.Models;
using AppTaxi.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;
using NPOI.OpenXmlFormats.Spreadsheet;
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

            // Ejecutar todas las llamadas API en paralelo para reducir el tiempo de espera
            var empresasTask = _empresa.Lista(login);
            var vehiculosTask = _vehiculo.Lista(login);
            var conductoresTask = _conductor.Lista(login);
            var propietariosTask = _propietario.Lista(login);
            var horariosTask = _horario.Lista(login);

            await Task.WhenAll(empresasTask, vehiculosTask, conductoresTask, propietariosTask, horariosTask);

            var empresasTotales = await empresasTask;
            var vehiculosTotales = await vehiculosTask;
            var conductoresTotales = await conductoresTask;
            var propietariosTotales = await propietariosTask;
            var horariosTotales = await horariosTask;

            int IdEmpresa = empresasTotales
                .Where(e => e.IdUsuario == usuario.IdUsuario)
                .Select(e => e.IdEmpresa)
                .FirstOrDefault();

            // Convertimos las listas en diccionarios para acceso rápido
            var vehiculosDict = vehiculosTotales.ToDictionary(v => v.IdVehiculo);
            var conductoresDict = conductoresTotales.ToDictionary(c => c.IdConductor);
            var propietariosDict = propietariosTotales.ToDictionary(p => p.IdPropietario);

            List<DatosEmpresa> DatosIniciales = new List<DatosEmpresa>();
            int i = 1;

            foreach (var h in horariosTotales)
            {
                if (conductoresDict.TryGetValue(h.IdConductor, out var c) && c.IdEmpresa == IdEmpresa && c.Estado)
                {
                    if (vehiculosDict.TryGetValue(h.IdVehiculo, out var v) && propietariosDict.TryGetValue(v.IdPropietario, out var p))
                    {
                        DatosIniciales.Add(new DatosEmpresa
                        {
                            IdDato = i++,
                            Foto = c.Foto,
                            IdVehiculo = h.IdVehiculo,
                            Placa = v.Placa,
                            NombrePropietario = p.Nombre,
                            Conductor = c.Nombre,
                            Fecha = h.Fecha,
                            HoraInicio = h.HoraInicio,
                            HoraFin = h.HoraFin
                        });
                    }
                }
            }

            ViewBag.Mensaje = $"Bienvenid@ {usuario.Nombre}";
            return View(DatosIniciales);
        }


        // Muestra los detalles de un registro específico (vehículo, conductor, horario, etc.).
        public async Task<IActionResult> Detalle(int IdDato)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return View();
            }

            var login = CreateLogin(usuario);

            // Ejecutar todas las llamadas API en paralelo para reducir el tiempo de espera
            var empresasTask = _empresa.Lista(login);
            var vehiculosTask = _vehiculo.Lista(login);
            var conductoresTask = _conductor.Lista(login);
            var propietariosTask = _propietario.Lista(login);
            var horariosTask = _horario.Lista(login);

            await Task.WhenAll(empresasTask, vehiculosTask, conductoresTask, propietariosTask, horariosTask);

            var empresasTotales = await empresasTask;
            var vehiculosTotales = await vehiculosTask;
            var conductoresTotales = await conductoresTask;
            var propietariosTotales = await propietariosTask;
            var horariosTotales = await horariosTask;

            int IdEmpresa = empresasTotales
                .Where(e => e.IdUsuario == usuario.IdUsuario)
                .Select(e => e.IdEmpresa)
                .FirstOrDefault();

            // Convertimos las listas en diccionarios para acceso rápido
            var vehiculosDict = vehiculosTotales.ToDictionary(v => v.IdVehiculo);
            var conductoresDict = conductoresTotales.ToDictionary(c => c.IdConductor);
            var propietariosDict = propietariosTotales.ToDictionary(p => p.IdPropietario);

            List<DatosEmpresa> DatosIniciales = new List<DatosEmpresa>();
            int i = 1;

            foreach (var h in horariosTotales)
            {
                if (conductoresDict.TryGetValue(h.IdConductor, out var c) && c.IdEmpresa == IdEmpresa && c.Estado)
                {
                    if (vehiculosDict.TryGetValue(h.IdVehiculo, out var v) && propietariosDict.TryGetValue(v.IdPropietario, out var p))
                    {
                        DatosIniciales.Add(new DatosEmpresa
                        {
                            IdDato = i++,
                            Foto = c.Foto,
                            IdVehiculo = h.IdVehiculo,
                            Placa = v.Placa,
                            NombrePropietario = p.Nombre,
                            Conductor = c.Nombre,
                            Fecha = h.Fecha,
                            HoraInicio = h.HoraInicio,
                            HoraFin = h.HoraFin
                        });
                    }
                }
            }

            // Obtiene el registro específico por su IdDato.
            var dato = DatosIniciales.FirstOrDefault(item => item.IdDato == IdDato);
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

            int i = 0;
            while (true)
            {
                vehiculosEmpresa[i].Contador = i + 1;
                if (i == vehiculosEmpresa.Count() - 1)
                {
                    break;
                }
                else
                {
                    i++;
                }
            }
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
            ModeloVista modelo = new ModeloVista();
            modelo.Vehiculo = vehiculo;
            return View(modelo);
        }

        // Guarda los cambios realizados en un vehículo.
        [HttpPost]
        public async Task<IActionResult> Guardar_Vehiculo(ModeloVista modelo)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            modelo.Vehiculo.Estado = true;

            // Convertir archivos PDF a Base64
            if (modelo.Archivo_1 != null)
            {
                using (var ms = new MemoryStream())
                {
                    await modelo.Archivo_1.CopyToAsync(ms);
                    modelo.Vehiculo.Soat = Convert.ToBase64String(ms.ToArray());
                }
            }

            if (modelo.Archivo_2 != null)
            {
                using (var ms = new MemoryStream())
                {
                    await modelo.Archivo_2.CopyToAsync(ms);
                    modelo.Vehiculo.TecnicoMecanica = Convert.ToBase64String(ms.ToArray());
                }
            }
            bool respuesta = await _vehiculo.Editar(modelo.Vehiculo, login);

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

            viewModel.Vehiculo.Estado = true;
            viewModel.Vehiculo.IdEmpresa = empresas.FirstOrDefault(e => e.IdUsuario == usuario.IdUsuario)?.IdEmpresa ?? 0;
            viewModel.Vehiculo.Placa = viewModel.Vehiculo.Placa.ToUpper();

            // Validar si la placa ya existe
            if (vehiculos.Any(v => v.Placa == viewModel.Vehiculo.Placa))
            {
                ViewBag.Mensaje = "La placa ya está en uso";
                var propietariosTotales = await _propietario.Lista(login);
                viewModel.Propietarios = propietariosTotales?.Where(p => p.IdEmpresa == viewModel.Vehiculo.IdEmpresa && p.Estado).ToList();

                return View("Agregar_Vehiculo", viewModel);
            }

            // Convertir archivos PDF a Base64
            if (viewModel.Archivo_1 != null)
            {
                using (var ms = new MemoryStream())
                {
                    await viewModel.Archivo_1.CopyToAsync(ms);
                    viewModel.Vehiculo.Soat = Convert.ToBase64String(ms.ToArray());
                }
            }

            if (viewModel.Archivo_2 != null)
            {
                using (var ms = new MemoryStream())
                {
                    await viewModel.Archivo_2.CopyToAsync(ms);
                    viewModel.Vehiculo.TecnicoMecanica = Convert.ToBase64String(ms.ToArray());
                }
            }

            // Guardar el vehículo en la BD
            bool respuesta = await _vehiculo.Guardar(viewModel.Vehiculo, login);

            if (respuesta)
            {
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
            int i = 0;
            while (true)
            {
                conductoresEmpresa[i].Contador = i + 1;
                if (i == conductoresEmpresa.Count() - 1)
                {
                    break;
                }
                else
                {
                    i++;
                }
            }
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
            ModeloVista modelo = new ModeloVista();
            var login = CreateLogin(usuario);
            var conductor = await _conductor.Obtener(IdConductor, login);
            modelo.Conductor = conductor;
            return View(modelo);
        }

        // Guarda los cambios realizados en un conductor.
        [HttpPost]
        public async Task<IActionResult> Guardar_Conductor(ModeloVista modelo)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            modelo.Conductor.Estado = true;
            
            // Convertir archivos PDF a Base64
            if (modelo.Archivo_1 != null)
            {
                using (var ms = new MemoryStream())
                {
                    await modelo.Archivo_1.CopyToAsync(ms);
                    modelo.Conductor.DocumentoCedula = Convert.ToBase64String(ms.ToArray());
                }
            }

            if (modelo.Archivo_2 != null)
            {
                using (var ms = new MemoryStream())
                {
                    await modelo.Archivo_2.CopyToAsync(ms);
                    modelo.Conductor.DocumentoEps = Convert.ToBase64String(ms.ToArray());
                }
            }

            if (modelo.Archivo_3 != null)
            {
                using (var ms = new MemoryStream())
                {
                    await modelo.Archivo_3.CopyToAsync(ms);
                    modelo.Conductor.DocumentoArl = Convert.ToBase64String(ms.ToArray());
                }
            }

            if (modelo.Archivo_4 != null)
            {
                using (var ms = new MemoryStream())
                {
                    await modelo.Archivo_4.CopyToAsync(ms);
                    modelo.Conductor.Foto = Convert.ToBase64String(ms.ToArray());
                }
            }

            bool respuesta = await _conductor.Editar(modelo.Conductor, login);

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
        public async Task<IActionResult> Crear_Conductor(ModeloVista modelo)
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

            modelo.Conductor.Estado = true;
            modelo.Conductor.IdEmpresa = empresas.FirstOrDefault(e => e.IdUsuario == usuario.IdUsuario)?.IdEmpresa ?? 0;

            // Valida si el conductor ya está registrado.
            if (conductores.Any(c => c.NumeroCedula == modelo.Conductor.NumeroCedula))
            {
                ViewBag.Mensaje = "El conductor ya está registrado";
                return View("Agregar_Conductor");
            }

            // Convertir archivos PDF a Base64
            if (modelo.Archivo_1 != null)
            {
                using (var ms = new MemoryStream())
                {
                    await modelo.Archivo_1.CopyToAsync(ms);
                    modelo.Conductor.DocumentoCedula = Convert.ToBase64String(ms.ToArray());
                }
            }
            

            if (modelo.Archivo_2 != null)
            {
                using (var ms = new MemoryStream())
                {
                    await modelo.Archivo_2.CopyToAsync(ms);
                    modelo.Conductor.DocumentoEps = Convert.ToBase64String(ms.ToArray());
                }
            }
            

            if (modelo.Archivo_3 != null)
            {
                using (var ms = new MemoryStream())
                {
                    await modelo.Archivo_3.CopyToAsync(ms);
                    modelo.Conductor.DocumentoArl = Convert.ToBase64String(ms.ToArray());
                }
            }
            

            if (modelo.Archivo_4 != null)
            {
                using (var ms = new MemoryStream())
                {
                    await modelo.Archivo_4.CopyToAsync(ms);
                    modelo.Conductor.Foto = Convert.ToBase64String(ms.ToArray());
                }
            }
            

            // Guarda el conductor.
            bool respuesta = await _conductor.Guardar(modelo.Conductor, login);

            if (respuesta)
            {
                var conductoresGuardados = await _conductor.Lista(login);
                var conductorGuardado = conductoresGuardados.FirstOrDefault(c => c.NumeroCedula == modelo.Conductor.NumeroCedula);
                ViewBag.IdConductor = conductorGuardado?.IdConductor;
                ViewBag.Exito = true;
                return RedirectToAction("Conductores");
            }
            else
            {
                ViewBag.Mensaje = $"No se pudo Guardar {modelo.Conductor.Eps}";
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

            int i = 0;
            while (true)
            {
                propietariosEmpresa[i].Contador = i + 1;
                if (i == propietariosEmpresa.Count() - 1)
                {
                    break;
                }
                else
                {
                    i++;
                }
            }
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
            ModeloVista modelo = new ModeloVista();
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            var propietario = await _propietario.Obtener(IdPropietario, login);
            modelo.Propietario = propietario;
            return View(modelo);
        }

        // Guarda los cambios realizados en un propietario.
        [HttpPost]
        public async Task<IActionResult> Guardar_Propietario(ModeloVista modelo)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            modelo.Propietario.Estado = true;
            if (modelo.Archivo_4 != null)
            {
                using (var ms = new MemoryStream())
                {
                    await modelo.Archivo_4.CopyToAsync(ms);
                    modelo.Propietario.Foto = Convert.ToBase64String(ms.ToArray());
                }
            }
            bool respuesta = await _propietario.Editar(modelo.Propietario, login);

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
        public async Task<IActionResult> Crear_Propietario(ModeloVista modelo)
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

            modelo.Propietario.Estado = true;
            modelo.Propietario.IdEmpresa = empresas.FirstOrDefault(e => e.IdUsuario == usuario.IdUsuario)?.IdEmpresa ?? 0;

            // Valida si el propietario ya está registrado.
            if (propietarios.Any(c => c.NumeroCedula == modelo.Propietario.NumeroCedula))
            {
                ViewBag.Mensaje = "El propietario ya está registrado";
                return View("Agregar_Propietario");
            }
            if (modelo.Archivo_4 != null)
            {
                using (var ms = new MemoryStream())
                {
                    await modelo.Archivo_4.CopyToAsync(ms);
                    modelo.Propietario.Foto = Convert.ToBase64String(ms.ToArray());
                }
            }

            if (modelo.Archivo_1 != null)
            {
                using (var ms = new MemoryStream())
                {
                    await modelo.Archivo_1.CopyToAsync(ms);
                    modelo.Propietario.DocumentoCedula = Convert.ToBase64String(ms.ToArray());
                }
            }
            // Guarda el propietario.
            bool respuesta = await _propietario.Guardar(modelo.Propietario, login);

            if (respuesta)
            {
                var propietariosGuardados = await _propietario.Lista(login);
                var propietarioGuardado = propietariosGuardados.FirstOrDefault(p => p.NumeroCedula == modelo.Propietario.NumeroCedula);
                ViewBag.IdPropietario = modelo.Propietario?.IdPropietario;
                ViewBag.Exito = true;
                return RedirectToAction("Propietarios");
            }
            else
            {
                ViewBag.Mensaje = $"No se pudo Guardar {modelo.Propietario.IdEmpresa}";
                return View("Agregar_Propietario");
            }
        }

        //------------------------- Horario ----------------------------------------------
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

        public async Task<IActionResult> Editar_Horario(int IdHorario)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }
            var login = CreateLogin(usuario);
            ModeloVista modelo = new ModeloVista();

            var vehiculos = await _vehiculo.Lista(login);
            var empresas = await _empresa.Lista(login);
            int IdEmpresa = empresas.Where(e => e.IdUsuario == usuario.IdUsuario).FirstOrDefault().IdEmpresa;
            
            modelo.Vehiculos = vehiculos?.Where(v => v.IdEmpresa == IdEmpresa && v.Estado).ToList();
            modelo.Horario = await _horario.Obtener(IdHorario, login);
            modelo.Conductor = await _conductor.Obtener(modelo.Horario.IdConductor,login);
            return View(modelo);

        }

        [HttpPost]
        public async Task<IActionResult> Guardar_Horario(ModeloVista modelo)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }
            var login = CreateLogin(usuario);

            Horario h = modelo.Horario;
            bool respuesta = await _horario.Editar(h,login);
            if (respuesta)
            {
                int IdConductor = modelo.Conductor.IdConductor;
                return RedirectToAction("Ver_Horario", new { IdConductor = IdConductor });
            }
            else
            {
                ViewBag.Mensaje = "No se pudo Guardar";
                return NoContent();
            }
           
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar_Horario(int IdHorario)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);

            bool respuesta = await _horario.Eliminar(IdHorario,login);
            if(respuesta)
            {
                return RedirectToAction("Conductores");
            }
            else
            {
                ViewBag.Mensaje = "No se pudo Eliminar";
                return NoContent();
            }
        }

        public async Task<IActionResult> Asignar_Horario (int IdConductor)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            ModeloVista modelo = new ModeloVista();

            var vehiculos = await _vehiculo.Lista(login);
            var empresas = await _empresa.Lista(login);
            int IdEmpresa = empresas.Where(e => e.IdUsuario == usuario.IdUsuario).FirstOrDefault().IdEmpresa;

            modelo.Vehiculos = vehiculos?.Where(v => v.IdEmpresa == IdEmpresa && v.Estado).ToList();

            modelo.Conductor = await _conductor.Obtener(IdConductor,login);
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear_Horario(ModeloVista modelo)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);

            // Asignar el IdConductor al horario
            modelo.Horario.IdConductor = modelo.Conductor.IdConductor;

            // Guardar el horario
            bool respuesta = await _horario.Guardar(modelo.Horario, login);

            if (respuesta)
            {
                ViewBag.Mensaje = "Horario guardado correctamente.";
                return RedirectToAction("Ver_Horario", new { IdConductor = modelo.Conductor.IdConductor });
            }
            else
            {
                ViewBag.Mensaje = "No se pudo guardar el horario.";
                return NoContent();
            }
                    }

        [HttpPost]
        public async Task<IActionResult> Crear_RangoHorarios(ModeloVista modelo, DateTime FechaInicio, DateTime FechaFin, TimeSpan HoraInicio, TimeSpan HoraFin, int IdVehiculo)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);

            // Crear la lista de horarios
            var horarios = new List<Horario>();
            for (var fecha = FechaInicio; fecha <= FechaFin; fecha = fecha.AddDays(1))
            {
                horarios.Add(new Horario
                {
                    Fecha = fecha,
                    HoraInicio = HoraInicio,
                    HoraFin = HoraFin,
                    IdVehiculo = IdVehiculo,
                    IdConductor = modelo.Conductor.IdConductor
                });
            }

            // Guardar los horarios
            bool respuesta = true;
            foreach (var horario in horarios)
            {
                respuesta &= await _horario.Guardar(horario, login);
            }

            if (respuesta)
            {
                ViewBag.Mensaje = "Horario guardado correctamente.";
                return RedirectToAction("Ver_Horario", new { IdConductor = modelo.Conductor.IdConductor });
            }
            else
            {
                ViewBag.Mensaje = "No se pudo guardar el horario.";
                return NoContent();
            }
                        
        }

        public async Task<IActionResult> Papelera()
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);

            ModeloVista modeloTotal = new ModeloVista();
            modeloTotal.Conductores = await _conductor.Lista(login);
            modeloTotal.Propietarios = await _propietario.Lista(login);
            modeloTotal.Vehiculos = await _vehiculo.Lista(login);
            modeloTotal.Empresas = await _empresa.Lista(login);

            int IdEmpresa = modeloTotal.Empresas.Where(e => e.IdUsuario == usuario.IdUsuario).FirstOrDefault().IdEmpresa;


            ModeloVista modelo = new ModeloVista();
            modelo.Conductores = modeloTotal.Conductores.Where(item => item.Estado == false && item.IdEmpresa == IdEmpresa).ToList();
            modelo.Propietarios = modeloTotal.Propietarios.Where(item => item.Estado == false && item.IdEmpresa == IdEmpresa).ToList();
            modelo.Vehiculos = modeloTotal.Vehiculos.Where(item => item.Estado == false && item.IdEmpresa == IdEmpresa).ToList();


            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Recuperar_Conductor(int IdConductor)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            var conductor = await _conductor.Obtener(IdConductor, login);
            conductor.Estado = true;

            bool respuesta = await _conductor.Editar(conductor, login);

            if (respuesta)
            {
                return RedirectToAction("Papelera");
            }
            else
            {
                ViewBag.Mensaje = "No se pudo Recuperar";
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Recuperar_Vehiculo(int IdVehiculo)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            var vehiculo = await _vehiculo.Obtener(IdVehiculo, login);
            vehiculo.Estado = true;

            bool respuesta = await _vehiculo.Editar(vehiculo, login);

            if (respuesta)
            {
                return RedirectToAction("Papelera");
            }
            else
            {
                ViewBag.Mensaje = "No se pudo Recuperar";
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Recuperar_Propietario(int IdPropietario)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            var propietario = await _propietario.Obtener(IdPropietario, login);
            propietario.Estado = true;

            bool respuesta = await _propietario.Editar(propietario, login);

            if (respuesta)
            {
                return RedirectToAction("Papelera");
            }
            else
            {
                ViewBag.Mensaje = "No se pudo Recuperar";
                return NoContent();
            }
        }

        
    }

}