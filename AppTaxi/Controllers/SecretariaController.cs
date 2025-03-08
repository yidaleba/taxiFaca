using AppTaxi.Models;
using AppTaxi.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;
using System.Transactions;

namespace AppTaxi.Controllers
{
    public class SecretariaController : Controller
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
        public SecretariaController(I_Vehiculo vehiculo, I_Horario horario, I_Propietario propietario, I_Empresa empresa, I_Conductor conductor, I_Usuario usuario, I_Transaccion transaccion)
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

        //------------ Acciones principales ------------
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
        public async Task<IActionResult> Empresas()
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return View();
            }

            var login = CreateLogin(usuario);

            // Obtener la lista de empresas
            var empresasTotales = await _empresa.Lista(login);

            // Crear lista de tareas para obtener los usuarios en paralelo
            var tareasUsuarios = empresasTotales.Select(emp => _usuario.Obtener(emp.IdUsuario, login));

            // Ejecutar todas las llamadas en paralelo
            var usuarios = await Task.WhenAll(tareasUsuarios);

            // Construir el modelo con los resultados
            ModeloVista modelo = new ModeloVista
            {
                Empresas = empresasTotales,
                Usuarios = usuarios.ToList()
            };

            int i = 0;
            while (true)
            {
                modelo.Empresas[i].Contador = i + 1 ;
                if (i == modelo.Empresas.Count() - 1)
                {
                    break; 
                }
                else
                {
                    i++;
                }
            }


            return View(modelo);
        }

        public async Task<IActionResult> Detalle_Empresa(int IdEmpresa)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return View();
            }

            var login = CreateLogin(usuario);

            ModeloVista modelo = new ModeloVista();
            modelo.Empresa = await _empresa.Obtener(IdEmpresa, login);

            modelo.Usuario = await _usuario.Obtener(modelo.Empresa.IdUsuario, login);
            
            return View(modelo);
        }

        //----------------------------- Vista de Objetos registrados por Empresa
        public async Task<IActionResult> Conductores(int IdEmpresa)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return View();
            }

            var login = CreateLogin(usuario);
            ModeloVista modelo = new ModeloVista();

            var conductoresTotales = await _conductor.Lista(login);
            var conductoresEmpresa = conductoresTotales?.Where(c => c.IdEmpresa == IdEmpresa).ToList();
            if(conductoresEmpresa.Count > 0)
            {
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

            }
            modelo.Conductores = conductoresEmpresa;
            modelo.Empresa = await _empresa.Obtener(IdEmpresa, login);
            return View(modelo);
        }

        public async Task<IActionResult> Propietarios (int IdEmpresa)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return View();
            }

            var login = CreateLogin(usuario);

            var propietariosTotales = await _propietario.Lista(login);
            var propietariosEmpresa = propietariosTotales?.Where(p => p.IdEmpresa == IdEmpresa).ToList();
            ModeloVista modelo = new ModeloVista();
            if(propietariosEmpresa.Count() > 0)
            {
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
            }
                       
            modelo.Propietarios = propietariosEmpresa;
            modelo.Empresa = await _empresa.Obtener(IdEmpresa, login);
            return View(modelo);
        }

        public async Task<IActionResult> Vehiculos(int IdEmpresa)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return View();
            }

            var login = CreateLogin(usuario);

            var vehiculosTotales = await _vehiculo.Lista(login);
            var vehiculosEmpresa = vehiculosTotales?.Where(p => p.IdEmpresa == IdEmpresa).ToList();
            ModeloVista modelo = new ModeloVista();
            if(vehiculosEmpresa.Count() > 0)
            {

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
            }
            
            modelo.Vehiculos = vehiculosEmpresa;
            modelo.Empresa = await _empresa.Obtener(IdEmpresa, login);
            return View(modelo);
        }


        public async Task<IActionResult> Detalle_Conductor(int IdConductor)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return View();
            }

            var login = CreateLogin(usuario);
            var conductor = await _conductor.Obtener(IdConductor, login);

            return View(conductor);
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

            // Obtiene el propietario y los vehículos asociados.
            modelo.Propietario = await _propietario.Obtener(IdPropietario, login);

            var vehiculosTotales = await _vehiculo.Lista(login);
            modelo.Vehiculos = vehiculosTotales?.Where(v => v.IdPropietario == IdPropietario && v.Estado).ToList();

            return View(modelo);
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

            // Obtiene el vehículo y el propietario asociado.
            var vehiculo = await _vehiculo.Obtener(IdVehiculo, login);
            var propietario = await _propietario.Obtener(vehiculo.IdPropietario, login);

            ViewBag.Propietario = propietario?.Nombre;
            return View(vehiculo);
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


        public async Task<IActionResult> Vista_Agregar_Empresa()
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);
            ModeloVista modelo = new ModeloVista();

            var usuariosTotales = await _usuario.Lista(login);
            var empresasRegistradas = await _empresa.Lista(login);

            // Filtrar usuarios que no están en empresasRegistradas y que tengan IdRol = 1
            modelo.Usuarios = usuariosTotales
                .Where(u => u.IdRol == 1 && !empresasRegistradas.Any(e => e.IdUsuario == u.IdUsuario))
                .ToList();

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Agregar_Empresa(ModeloVista modelo)
        {
            var usuario = GetUsuarioFromSession();
            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }

            var login = CreateLogin(usuario);

            Empresa empresa = modelo.Empresa;

            bool respuesta = await _empresa.Guardar(empresa, login);

            if(respuesta)
            {
                Transaccion t = Crear_Transaccion("Guardar", "Empresa");
                bool guardar = await _transaccion.Guardar(t, login);
                return RedirectToAction("Empresas","Secretaria");
            }
            else
            {
                ViewBag.Mensaje = "No se Pudo Guardar";
                return View("Vista_Agregar_Empresa");
            }
        }

    }
}
