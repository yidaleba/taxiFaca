using AppTaxi.Funciones;
using AppTaxi.Models;
using AppTaxi.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;

namespace AppTaxi.Controllers
{
    public class InicioController : Controller
    {
        private readonly I_Invitado _invitado;
        private readonly I_Usuario _usuario;

        public InicioController(I_Invitado invitado, I_Usuario usuario) //Como en Windows Forms de Controlador
        {
            _invitado = invitado;
            _usuario = usuario;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Consultar(Consulta consulta)
        {
            ViewBag.Mensaje = "";
            Invitado inv = new Invitado();
            if (string.IsNullOrEmpty(consulta.Placa) && consulta.Documento == 0)
            {
                ViewBag.Mensaje = "Se debe digitar los campos solicitados";
                return View("Index"); 
            }

            if (consulta.Placa != null && consulta.Placa.Length != 6)
            {
                ViewBag.Mensaje = "Placa no Admitida";
                return View("Index");
            }

            inv = await _invitado.Consulta(consulta);

            if (inv == null)
            {
                ViewBag.Mensaje = "No se encontraron datos para la consulta.";
                return View("Index"); 
            }
            return View(inv);
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Autenticar(Login login)
        {
            List<Usuario> lista = new List<Usuario>();
            Usuario usuario = new Usuario();
            ViewBag.Mensaje = "";
            if (string.IsNullOrEmpty(login.Correo) || string.IsNullOrEmpty(login.Contrasena))
            {
                ViewBag.Mensaje = "Se debe digitar los campos solicitados";
                return View("Login");
            }

            if(!ValidacionDato.ValidarTexto(login.Correo) || !ValidacionDato.ValidarTexto(login.Contrasena))
            {
                ViewBag.Mensaje = "Error, intenta ingresar caracteres no permitidos";
                return View("Login");
            }
            
            lista = await _usuario.Lista(login);

            if (lista != null && lista.Any())
            {
                usuario = lista.Where(item => item.Correo == login.Correo && item.Contrasena == item.Contrasena).FirstOrDefault();
                ViewBag.Mensaje = $"Bienvenido {usuario.Nombre}";
                HttpContext.Session.SetString("Usuario", JsonConvert.SerializeObject(usuario));

                switch (usuario.IdRol)
                {
                    case 1:
                        return RedirectToAction("Inicio", "Empresa");
                    case 2:
                        return RedirectToAction("Inicio", "Secretaria");
                    case 1004:
                        return RedirectToAction("Inicio", "Admin");
                    default:
                        return View("Login");
                }
            }
            else
            {
                string msg = "Usuario o Contraseña incorrecta";
                ViewBag.Mensaje = msg;
                return View("Login");
            }
  
        }

        public IActionResult CerrarSesion()
        {
            // Limpiar todos los datos de la sesión
            HttpContext.Session.Clear();

            // Opcional: Redirigir a la página de inicio o login
            return RedirectToAction("Login");
        }




    }
}
