using AppTaxi.Models;
using AppTaxi.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
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
            
            lista = await _usuario.Lista(login);

            if (lista != null && lista.Any())
            {
                usuario = lista.Where(item => item.Correo == login.Correo && item.Contrasena == item.Contrasena).FirstOrDefault();
                string msg = $"Bienvenido {usuario.Nombre}";
                ViewBag.Mensaje = msg;
            }
            else
            {
                string msg = "Usuario o Contraseña incorrecta";
                ViewBag.Mensaje = msg;
            }
            


            return View("Login");
        }




    }
}
