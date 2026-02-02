using System.IO;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.IO.Image;
using iText.Kernel.Utils;
using iText.Layout.Element;

namespace Converter


{
    public class ConcatenateLogic : ConversionHelper
    {
        internal void ConcatData(IEnumerable<string> SelectedItemPaths)
        {
            string destinationFolder = string.Empty;
            var parts = new List<byte[]>(); 
            
            foreach (var filePath in SelectedItemPaths)
            {
                try
                {
                    var posNum = filePath.LastIndexOf(@"\");

                    // Zielordner nur einmal setzen (vom ersten File)
                    if (string.IsNullOrEmpty(destinationFolder))
                        destinationFolder = filePath.Substring(0, posNum + 1);

                    string extension = Path.GetExtension(filePath).ToLowerInvariant();

                    using var ms = new MemoryStream();
                    using (var writer = new PdfWriter(ms))
                    using (var pdf = new PdfDocument(writer))
                    using (var doc = new Document(pdf))
                    {
                        if (isText(extension))
                        {
                            var text = File.ReadAllText(filePath);
                            doc.Add(new Paragraph(text));
                        }
                        else if (isPic(extension))
                        {
                            var iDATA = ImageDataFactory.Create(filePath);
                            Image image = new Image(iDATA);
                            doc.Add(image);
                        }
                        else
                        {
                            ShowSupportedTypesMessage();
                            continue;
                        }
                    } // schließt doc/pdf/writer → PDF fertig in ms

                    parts.Add(ms.ToArray());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler bei Datei '{filePath}':\n{ex}", "Konvertierungsfehler");
                }
            }

            try {
                if (parts.Count == 0)
                {
                    MessageBox.Show("Keine gültigen Dateien zum Zusammenführen gefunden.");
                    return;
                }

                // WICHTIG: Hier Dateiname bauen, nicht nur Ordner!
                var destinationPath = Path.Combine(destinationFolder, "merged.pdf");
                MergeDocs(destinationPath, parts);
                MessageBox.Show("Merge abgeschlossen:\n" + destinationPath);
            }
            catch (Exception ex){
                MessageBox.Show("Fehler beim Zusammenführen:\n" + ex, "Merge-Fehler");
            }
        }

        

        private static void MergeDocs(string pdfDestinationPath, List<byte[]> parts)
        {
            using var writer = new PdfWriter(pdfDestinationPath);
            using var pdf = new PdfDocument(writer);
            PdfMerger merger = new PdfMerger(pdf);

            foreach (var part in parts)
            {
                var src = new PdfDocument(new PdfReader(new MemoryStream(part)));
                merger.Merge(src, 1, src.GetNumberOfPages());
            }
            parts.Clear();
        }
        
        
        private static void ShowSupportedTypesMessage() {
            string msg = "Fehler bei Konvertierung! Nur Folgende Dateitypen werden unterstützt:\n";
            string[] a = new[] { ".txt", ".png", ".jpg", ".jpeg", ".html" };
            
            for (int i = 0; i < a.Length; i++){
                if (i == a.Length - 1){
                    msg += a[i];
                    continue;
                }
                msg += a[i] + ", ";
            }
            MessageBox.Show(msg);
        }
        
        private bool isText(string extension)
        {
            return extension == ".html" || extension == ".txt";
        }
        
        private bool isPic(string extension)
        {
            return extension == ".png" || extension == ".jpg" || extension == ".jpeg";
        }
    }
}
