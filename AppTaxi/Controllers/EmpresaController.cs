using AppTaxi.Models;
using AppTaxi.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;

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

        public async Task<IActionResult> Inicio()
        {

            List<Empresa> empresas = new List<Empresa>();  //Obtiene todas las empresas

            Empresa empresa = new Empresa(); //Datos de la empresa

            var usuarioJson = TempData["Usuario"] as string;
            var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
            empresas = await _empresa.Lista(new Models.Login(){ Correo = usuario.Correo, Contrasena = usuario.Contrasena });


            if (empresas != null && empresas.Any())
            {
                //ViewBag.Mensaje = $"Hola {empresas.FirstOrDefault().Nit}";        //Impresion de Prueba
                empresa = empresas.Where(item => item.IdUsuario == usuario.IdUsuario).FirstOrDefault();
                if(empresa != null)
                {
                    ViewBag.Mensaje = $"Bienvenido {usuario.Nombre} encargado de {empresa.Nit}";
                }
                else
                {
                    ViewBag.Mensaje = "No hay empresa asociada a su usuario";
                }

            }
            
            return View();
        }
    }
}
