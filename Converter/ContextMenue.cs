using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Windows.Forms;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;
using System;
using System.Collections.Generic;
using System.IO;

namespace Converter
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    
    public class ContextMenue : SharpContextMenu{
        private ConverterLogic converterLogic = new ConverterLogic();
        private ConcatenateLogic concatenateLogic = new ConcatenateLogic();
        
        protected override ContextMenuStrip CreateMenu()
        {
            var menu = new ContextMenuStrip();
            
            var PDFConverter = new ToolStripMenuItem("Convert to PDF");
            var ImageConverter = new ToolStripMenuItem("Convert Image to");
            var Concatenate = new ToolStripMenuItem("Concat in PDF");
            
            ImageConverter.DropDownItems.Add("JPG").Click += (s, e) =>
            {
                
            };
            
            ImageConverter.DropDownItems.Add("JPEG").Click += (s, e) =>
            {
                
            };
            
            ImageConverter.DropDownItems.Add("PNG").Click += (s, e) =>
            {
                
            };

            PDFConverter.Click += (s, e) =>
            {
                foreach (var filePath in this.SelectedItemPaths)
                {
                    var PosNum = filePath.LastIndexOf(@"\");
                    var PathFolder = filePath.Substring(0, PosNum+1);
                    
                    converterLogic.ConverterData(filePath, PathFolder);
                }
            };
            
            Concatenate.Click += (s, e) =>
            {
                concatenateLogic.ConcatData(this.SelectedItemPaths);
            };
            
            
            menu.Items.Add(PDFConverter);
            menu.Items.Add(Concatenate);

            if (onlyImg(this.SelectedItemPaths))
            {
                menu.Items.Add(ImageConverter);
            }
            
            return menu;
        }

        
        protected override bool CanShowMenu()
        {
            return true;
        }

        
        private static bool onlyImg(IEnumerable<string> SelectedItemPaths)
        {
            bool isImg = true;
            foreach (var filePath in SelectedItemPaths)
            {
                string extension = Path.GetExtension(filePath).ToLowerInvariant();
                if (!(extension == ".jpg" || extension == ".jpeg" || extension == ".png"))
                {
                    isImg = false;
                }
            }

            return isImg;
        }
    }
}