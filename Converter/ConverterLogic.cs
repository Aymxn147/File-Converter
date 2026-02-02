using System;
using System.IO;
using System.Windows.Forms;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.IO.Image;
using iText.Layout.Element;

namespace Converter
{
    public class ConverterLogic : ConversionHelper
    {

        internal void ConverterData(string filePath, string destinationPath){
            string extension = Path.GetExtension(filePath).ToLowerInvariant();

            switch (extension)
            {
                case ".txt":
                    ConvertTextFileToPdf(filePath, destinationPath);
                    break;

                case ".jpg":
                case ".jpeg":
                case ".png":
                    ConvertImageFileToPdf(filePath, destinationPath);
                    break;

                default:
                    ShowSupportedTypesMessage();
                    break;
            }

        }


        private static void ConvertTextFileToPdf(string filePath, string destinationPath)
        {
            try
            {
                var originalName = FilterOriginName(filePath);
                var pdfPath = GetPdfOutputPath(destinationPath, originalName);
                
                
                CreateDoc(pdfPath, doc =>
                {
                    var text = File.ReadAllText(filePath);
                    doc.Add(new Paragraph(text));

                    doc.Close();
                    MessageBox.Show("PDF erstellt: " + pdfPath); 
                });
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler bei Konvertierung:\n" + ex.ToString());
            }
        }
        
        private static void ConvertImageFileToPdf(string filePath, string destinationPath)
        {
            try
            {
                string originalName = FilterOriginName(filePath);
                string pdfPath = GetPdfOutputPath(destinationPath, originalName );
                
                CreateDoc(pdfPath, doc =>
                {
                    var iDATA = ImageDataFactory.Create(filePath);
                    Image image = new Image(iDATA);
                
                    doc.Add(image);

                    doc.Close();
                    MessageBox.Show("PDF erstellt: " + pdfPath);
                });
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler bei Konvertierung:\n" + ex.ToString());
            }
        }
        
        private static void ShowSupportedTypesMessage() {
            string msg = "Fehler bei Konvertierung! Nur Folgende Dateitypen werden unterstützt:\n";
            string[] a = new[] { ".txt", ".png", ".jpg", ".jpeg" };
            
            for (int i = 0; i < a.Length; i++){
                if (i == a.Length - 1){
                    msg += a[i];
                    continue;
                }
                msg += a[i] + ", ";
            }
            MessageBox.Show(msg);
        }
    }
}