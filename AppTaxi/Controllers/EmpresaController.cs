using Microsoft.AspNetCore.Mvc;

namespace AppTaxi.Controllers
{
    public class EmpresaController : Controller
    {
        public IActionResult Inicio()
        {
            return View();
        }
    }
}
