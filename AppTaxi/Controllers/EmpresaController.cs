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
            List<Vehiculo> vehiculos_totales = new List<Vehiculo>();
            List<Vehiculo> vehiculos_empresa = new List<Vehiculo>();
            List<Horario> horarios_totales = new List<Horario>();
            List<DatosEmpresa> datos_iniciales = new List<DatosEmpresa>();
            List<Empresa> empresas_totales = new List<Empresa>();  // Todas las Empresas

            Empresa empresa = new Empresa(); //Obtiene la empresa Logeada

            var usuarioJson = TempData["Usuario"] as string; //Obtiene los datos del usuario logeado
            var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson); //Deserializa el objeto en usuario

            Models.Login login = new Models.Login() 
            { 
                Correo = usuario.Correo, 
                Contrasena = usuario.Contrasena 
            };

            empresas_totales = await _empresa.Lista(login);
            vehiculos_totales = await _vehiculo.Lista(login);
            horarios_totales = await _horario.Lista(login);

            if (empresas_totales == null && !empresas_totales.Any())
            {
                ViewBag.Mensaje = "La lista Empresa está vacía";
                return View();
            }
            
            empresa = empresas_totales.Where(item => item.IdUsuario == usuario.IdUsuario).FirstOrDefault();
            
            if (empresa == null)
            {
                ViewBag.Mensaje = "No hay empresa asociada a su usuario";
                return View();
            }

            //ViewBag.Mensaje = $"Bienvenido {usuario.Nombre} encargado de {empresa.Nombre}";  //Prueba si el objeto empresa se llena

            if (vehiculos_totales == null && !vehiculos_totales.Any())
            {
                ViewBag.Mensaje = "La lista Vehiculo está vacía";
                return View();
            }
            //ViewBag.Mensaje = $"Bienvenido {usuario.Nombre} encargado de {vehiculos_totales.FirstOrDefault().Placa}";//Prueba si la lista Vehiculos se llena
            
            foreach(Vehiculo v in vehiculos_totales)
            {
                if (v.IdEmpresa == empresa.IdEmpresa)
                {
                    vehiculos_empresa.Add(v);
                }
            }
            int i = 1;
            foreach (Vehiculo v in vehiculos_empresa)
            {
                Propietario p = new Propietario();
                Conductor c = new Conductor();
                Horario h = new Horario();
                DatosEmpresa dE = new DatosEmpresa();

                h = horarios_totales.Where(item => item.IdVehiculo == v.IdVehiculo).FirstOrDefault();
                p = await _propietario.Obtener(v.IdPropietario,login);
                c = await _conductor.Obtener(h.IdConductor,login);

                dE.IdDato = i;
                dE.Placa = v.Placa;
                dE.IdVehiculo = v.IdVehiculo;
                dE.NombrePropietario = p.Nombre;
                dE.Conductor = c.Nombre;
                dE.Fecha = h.Fecha;
                dE.HoraInicio = h.HoraInicio;
                dE.HoraFin = h.HoraFin;
                
                datos_iniciales.Add(dE);
                i++;
            }

            //ViewBag.Mensaje = $"Añadidos {datos_iniciales.Count()} registros";//Prueba si la lista Vehiculos de empresa se llena
            ViewBag.Mensaje = $"Bienvenid@ {usuario.Nombre}";


            return View(datos_iniciales);
        }
    }
}
