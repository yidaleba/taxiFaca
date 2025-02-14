namespace AppTaxi.Funciones
{
    public class Conversor
    {
        public static string ConvertirPdfABase64(string rutaPdf)
        {
            if (!File.Exists(rutaPdf))
                throw new FileNotFoundException("El archivo PDF no existe", rutaPdf);

            byte[] pdfBytes = File.ReadAllBytes(rutaPdf);
            return Convert.ToBase64String(pdfBytes);
        }

        public static void ConvertirBase64APdf(string base64String, string rutaDestino)
        {
            byte[] pdfBytes = Convert.FromBase64String(base64String);
            File.WriteAllBytes(rutaDestino, pdfBytes);
        }
    }
}
