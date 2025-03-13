using Tesseract;
using System;

namespace AppTaxi.Servicios
{
    public class ValidacionDocumentos
    {
        private readonly string _tessDataPath;

        public ValidacionDocumentos()
        {
            // Ruta donde está el archivo "spa.traineddata"
            _tessDataPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Tesseract");
        }

        public string ProcesarImagenConOCR(string imagePath)
        {
            using var engine = new TesseractEngine(_tessDataPath, "spa", EngineMode.Default);
            using var img = Pix.LoadFromFile(imagePath);
            using var page = engine.Process(img);

            return page.GetText(); // Retorna el texto extraído
        }

        public bool Contiene(string texto, string[] palabras, char operador)
        {
            if (operador == 'Y')
            {
                return palabras.All(palabra => texto.Contains(palabra, StringComparison.OrdinalIgnoreCase));
            }
            else if (operador == 'O')
            {
                return palabras.Any(palabra => texto.Contains(palabra, StringComparison.OrdinalIgnoreCase));
            }

            throw new ArgumentException("El operador debe ser 'Y' o 'O'.");
        }
    }
}
