namespace AppTaxi.Models
{
    public class ReporteSeleccion
    {
        // Conductor
        public bool IdConductor { get; set; }
        public bool FotoConductor { get; set; }
        public bool NombreConductor { get; set; }
        public bool NumeroCedulaConductor { get; set; }
        public bool TelefonoConductor { get; set; }
        public bool CorreoConductor { get; set; }
        public bool DireccionConductor { get; set; }
        public bool CiudadConductor { get; set; }
        public bool CelularConductor { get; set; }
        public bool EstadoConductor { get; set; }
        public bool GrupoSanguineoConductor { get; set; }
        public bool EpsConductor { get; set; }
        public bool ArlConductor { get; set; }
        public bool IdEmpresaConductor { get; set; }

        // Empresa
        public bool IdEmpresa { get; set; }
        public bool NombreEmpresa { get; set; }
        public bool NitEmpresa { get; set; }
        public bool RepresentanteEmpresa { get; set; }
        public bool RedPrincipalEmpresa { get; set; }
        public bool RedSecundariaEmpresa { get; set; }
        public bool IdUsuarioEmpresa { get; set; }
        public bool CuposEmpresa { get; set; }

        // Horario
        public bool IdHorario { get; set; }
        public bool FechaHorario { get; set; }
        public bool HoraInicioHorario { get; set; }
        public bool HoraFinHorario { get; set; }
        public bool IdConductorHorario { get; set; }
        public bool IdVehiculoHorario { get; set; }

        // Transacción
        public bool IdAccion { get; set; }
        public bool IdUsuarioTransaccion { get; set; }
        public bool AccionTransaccion { get; set; }
        public bool ModeloTransaccion { get; set; }
        public bool FechaTransaccion { get; set; }
        public bool HoraTransaccion { get; set; }

        // Vehículo
        public bool IdVehiculo { get; set; }
        public bool PlacaVehiculo { get; set; }
        public bool EstadoVehiculo { get; set; }
        public bool SoatVehiculo { get; set; }
        public bool TecnicoMecanicaVehiculo { get; set; }
        public bool IdPropietarioVehiculo { get; set; }
        public bool IdEmpresaVehiculo { get; set; }
        public bool VenceSoat { get; set; }
        public bool VenceTecnicoMecanica { get; set; }

        // Usuario
        public bool IdUsuario { get; set; }
        public bool CorreoUsuario { get; set; }
        public bool ContrasenaUsuario { get; set; }
        public bool FotoUsuario { get; set; }
        public bool NombreUsuario { get; set; }
        public bool TelefonoUsuario { get; set; }
        public bool DireccionUsuario { get; set; }
        public bool CiudadUsuario { get; set; }
        public bool CelularUsuario { get; set; }
        public bool EstadoUsuario { get; set; }
        public bool IdRolUsuario { get; set; }

        // Rol
        public bool IdRol { get; set; }
        public bool DescripcionRol { get; set; }

        // Propietario
        public bool IdPropietario { get; set; }
        public bool FotoPropietario { get; set; }
        public bool NombrePropietario { get; set; }
        public bool NumeroCedulaPropietario { get; set; }
        public bool TelefonoPropietario { get; set; }
        public bool CorreoPropietario { get; set; }
        public bool DireccionPropietario { get; set; }
        public bool CiudadPropietario { get; set; }
        public bool CelularPropietario { get; set; }
        public bool EstadoPropietario { get; set; }
        public bool DocumentoCedulaPropietario { get; set; }
        public bool IdEmpresaPropietario { get; set; }
    }

}
