using AppTaxi.Models;
using AppTaxi.Servicios;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;

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
            
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Consultar(Consulta consulta)
        {
            Invitado invitado = new Invitado();

            if (consulta.Placa != null || consulta.Documento != 0)
            {
                invitado = await _invitado.Consulta(consulta);
            }
            else
            {
                ViewBag.Mensaje = "Se debe digitar los campos solicitados";
            }

            return View(invitado);
        }
    }
}
