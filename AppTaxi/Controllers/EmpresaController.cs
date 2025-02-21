﻿using AppTaxi.Funciones;
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
            // Obtener el usuario de la sesión
            var usuarioJson = HttpContext.Session.GetString("Usuario");
            if (string.IsNullOrEmpty(usuarioJson))
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return View();
            }

            var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
            var login = new Models.Login { Correo = usuario.Correo, Contrasena = usuario.Contrasena };

            // Obtener listas de datos
            var empresasTotales = await _empresa.Lista(login);
            var vehiculosTotales = await _vehiculo.Lista(login);
            var horariosTotales = await _horario.Lista(login);

            // Validar listas
            if (empresasTotales == null || empresasTotales.Count() == 0)
            {
                ViewBag.Mensaje = "La lista de empresas está vacía.";
                return View();
            }

            // Obtener la empresa asociada al usuario
            var empresa = empresasTotales.FirstOrDefault(item => item.IdUsuario == usuario.IdUsuario);
            if (empresa == null)
            {
                ViewBag.Mensaje = "No hay empresa asociada a su usuario.";
                return View();
            }

            // Filtrar vehículos de la empresa
            var vehiculosEmpresa = vehiculosTotales?.Where(v => v.IdEmpresa == empresa.IdEmpresa).ToList();
            if (vehiculosEmpresa == null || vehiculosEmpresa.Count() == 0)
            {
                ViewBag.Mensaje = "La lista de vehículos está vacía.";
                return View();
            }

            // Construir la lista de datos iniciales
            var datosIniciales = new List<DatosEmpresa>();
            int i = 1;

            foreach (var vehiculo in vehiculosEmpresa)
            {
                var horario = horariosTotales.FirstOrDefault(h => h.IdVehiculo == vehiculo.IdVehiculo);
                if (horario == null) continue;

                var propietario = await _propietario.Obtener(vehiculo.IdPropietario, login);
                var conductor = await _conductor.Obtener(horario.IdConductor, login);
                if(vehiculo.Estado == true)
                {
                    datosIniciales.Add(new DatosEmpresa
                    {
                        IdDato = i++,
                        Foto = conductor.Foto,
                        Placa = vehiculo.Placa,
                        IdVehiculo = vehiculo.IdVehiculo,
                        NombrePropietario = propietario?.Nombre,
                        Conductor = conductor?.Nombre,
                        Fecha = horario.Fecha,
                        HoraInicio = horario.HoraInicio,
                        HoraFin = horario.HoraFin
                    });
                }
            }

            ViewBag.Mensaje = $"Bienvenid@ {usuario.Nombre}";

            return View(datosIniciales);
        }

        public async Task<IActionResult> Detalle(int IdDato)
        {
            var usuarioJson = HttpContext.Session.GetString("Usuario");
            if (string.IsNullOrEmpty(usuarioJson))
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }
            var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
            var login = new Models.Login { Correo = usuario.Correo, Contrasena = usuario.Contrasena };

            // Obtener listas de datos
            var empresasTotales = await _empresa.Lista(login);
            var vehiculosTotales = await _vehiculo.Lista(login);
            var horariosTotales = await _horario.Lista(login);

            // Validar listas
            if (empresasTotales == null || empresasTotales.Count() == 0)
            {
                ViewBag.Mensaje = "La lista de empresas está vacía.";
                return View();
            }

            // Obtener la empresa asociada al usuario
            var empresa = empresasTotales.FirstOrDefault(item => item.IdUsuario == usuario.IdUsuario);
            if (empresa == null)
            {
                ViewBag.Mensaje = "No hay empresa asociada a su usuario.";
                return View();
            }

            // Filtrar vehículos de la empresa
            var vehiculosEmpresa = vehiculosTotales?.Where(v => v.IdEmpresa == empresa.IdEmpresa).ToList();
            if (vehiculosEmpresa == null || vehiculosEmpresa.Count() == 0)
            {
                ViewBag.Mensaje = "La lista de vehículos está vacía.";
                return View();
            }

            // Construir la lista de datos iniciales
            var datosIniciales = new List<DatosEmpresa>();
            int i = 1;

            foreach (var vehiculo in vehiculosEmpresa)
            {
                var horario = horariosTotales.FirstOrDefault(h => h.IdVehiculo == vehiculo.IdVehiculo);
                if (horario == null) continue;

                var propietario = await _propietario.Obtener(vehiculo.IdPropietario, login);
                var conductor = await _conductor.Obtener(horario.IdConductor, login);

                if (vehiculo.Estado == true)
                {
                    datosIniciales.Add(new DatosEmpresa
                    {
                        IdDato = i++,
                        Foto = conductor.Foto,
                        Placa = vehiculo.Placa,
                        IdVehiculo = vehiculo.IdVehiculo,
                        NombrePropietario = propietario?.Nombre,
                        Conductor = conductor?.Nombre,
                        Fecha = horario.Fecha,
                        HoraInicio = horario.HoraInicio,
                        HoraFin = horario.HoraFin
                    });
                }


            }
            DatosEmpresa Dato = datosIniciales.Where(item => item.IdDato == IdDato).FirstOrDefault();
            return View(Dato);

        }

        public async Task<IActionResult> Vehiculos()
        {
            var usuarioJson = HttpContext.Session.GetString("Usuario");
            if (string.IsNullOrEmpty(usuarioJson))
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }
            var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
            var login = new Models.Login { Correo = usuario.Correo, Contrasena = usuario.Contrasena };

            List<Empresa> empresas = await _empresa.Lista(login);
            List<Vehiculo> vehiculos_totales = await _vehiculo.Lista(login);
            List<Vehiculo> vehiculos_empresas = new List<Vehiculo>();

            int IdEmpresa = empresas.Where(item => item.IdUsuario == usuario.IdUsuario).FirstOrDefault().IdEmpresa;
            foreach (Vehiculo v in vehiculos_totales)
            {
                if (v.IdEmpresa == IdEmpresa && v.Estado == true)
                {
                    vehiculos_empresas.Add(v);
                }
            }

            return View(vehiculos_empresas);
        }

        public async Task<IActionResult> Detalle_Vehiculo(int IdVehiculo)
        {
            var usuarioJson = HttpContext.Session.GetString("Usuario");
            if (string.IsNullOrEmpty(usuarioJson))
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }
            var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
            var login = new Models.Login { Correo = usuario.Correo, Contrasena = usuario.Contrasena };

            Vehiculo vehiculo = await _vehiculo.Obtener(IdVehiculo, login);
            ViewBag.Mensaje = "";
            Propietario propietario = await _propietario.Obtener(vehiculo.IdPropietario,login);
            ViewBag.Propietario = propietario.Nombre;
            return View(vehiculo);
        }

        public async Task<IActionResult> Editar_Vehiculo (int IdVehiculo)
        {
            var usuarioJson = HttpContext.Session.GetString("Usuario");
            if (string.IsNullOrEmpty(usuarioJson))
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }
            var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
            var login = new Models.Login { Correo = usuario.Correo, Contrasena = usuario.Contrasena };
            

            Vehiculo vehiculo = await _vehiculo.Obtener (IdVehiculo, login);

            return View(vehiculo);
        }

        [HttpPost]
        public async Task<IActionResult> Guardar_Vehiculo(Vehiculo vehiculo)
        {
            var usuarioJson = HttpContext.Session.GetString("Usuario");
            if (string.IsNullOrEmpty(usuarioJson))
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }
            var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
            var login = new Models.Login { Correo = usuario.Correo, Contrasena = usuario.Contrasena };

            bool respuesta = await _vehiculo.Editar(vehiculo, login);
            //string respuesta = await _vehiculo.Editar(vehiculo, login);

            if (respuesta)
            {
                return RedirectToAction("Vehiculos");
            }
            else
            {
                ViewBag.Mensaje = "No se pudo Guardar";
                //return View("Detalle_Vehiculo",vehiculo.IdVehiculo);
                return NoContent();
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar_Vehiculo(int IdVehiculo)
        {
            var usuarioJson = HttpContext.Session.GetString("Usuario");
            if (string.IsNullOrEmpty(usuarioJson))
            {
                ViewBag.Mensaje = "Usuario no autenticado.";
                return RedirectToAction("Login", "Inicio");
            }
            var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
            var login = new Models.Login { Correo = usuario.Correo, Contrasena = usuario.Contrasena};

            Vehiculo v = await _vehiculo.Obtener(IdVehiculo,login);
            v.Estado = false;
            bool respuesta = await _vehiculo.Editar(v, login);
            //string respuesta = await _vehiculo.Editar(vehiculo, login);

            if (respuesta)
            {
                return RedirectToAction("Vehiculos");
            }
            else
            {
                ViewBag.Mensaje = "No se pudo Guardar";
                //return View("Detalle_Vehiculo",vehiculo.IdVehiculo);
                return NoContent();
            }
        }





    }
}
