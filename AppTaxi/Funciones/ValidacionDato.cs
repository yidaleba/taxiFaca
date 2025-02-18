namespace AppTaxi.Funciones
{
    public class ValidacionDato
    {
        public static bool ValidarTexto(string texto)
        {
            // Lista de símbolos no permitidos
            char[] simbolosNoPermitidos = { '#', '-', '*', '=', '$', '&', '|' };

            // Verificar si el texto contiene alguno de los símbolos no permitidos
            return !texto.Any(c => simbolosNoPermitidos.Contains(c));
        }
    }
}
