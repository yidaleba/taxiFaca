using AppTaxi.Models;
using AppTaxi.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;

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

        // Constructor que recibe las dependencias inyectadas.
        public SecretariaController(I_Vehiculo vehiculo, I_Horario horario, I_Propietario propietario, I_Empresa empresa, I_Conductor conductor, I_Usuario usuario)
        {
            _vehiculo = vehiculo;
            _horario = horario;
            _propietario = propietario;
            _empresa = empresa;
            _conductor = conductor;
            _usuario = usuario;
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


        //Detalles de los objetos:


    }
}
