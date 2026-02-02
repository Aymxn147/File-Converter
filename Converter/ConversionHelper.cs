using System;
using System.IO;
using System.Windows.Forms;
using iText.Kernel.Pdf;
using iText.Layout;

namespace Converter
{
    public class ConversionHelper
    {
        protected static string GetPdfOutputPath(string destination, string name) {
            return Path.Combine(destination, name + ".pdf");
        }

        protected static string FilterOriginName(string filePath) {
            return Path.GetFileNameWithoutExtension(filePath);
        }
        
        protected static void CreateDoc(string pdfDestinationPath, Action<Document> docAction)
        {
            using var writer = new PdfWriter(pdfDestinationPath);
            using var pdf = new PdfDocument(writer);
            using var doc = new Document(pdf);
            docAction(doc);
        }
        
    }
}