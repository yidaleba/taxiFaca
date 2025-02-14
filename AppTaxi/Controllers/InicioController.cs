using AppTaxi.Models;
using AppTaxi.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace AppTaxi.Controllers
{
    public class InicioController : Controller
    {
        private readonly I_Invitado _invitado;

        public InicioController(I_Invitado invitado) //Como en Windows Forms de Controlador
        {
            _invitado = invitado;
        }

        public IActionResult Index()
        {
            Invitado invitado = new Invitado();

            return View();
        }
    }
}
