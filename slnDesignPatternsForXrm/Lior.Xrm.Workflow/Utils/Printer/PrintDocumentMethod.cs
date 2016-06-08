using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace printpdf.Utils
{

    public static class myPrinters
    {
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Name);
    }

    public class PrintDocumentMethod
    {
        PagingImage _qu;
        private Stream IOStream;
        int page = 1;

        public void ChangeDefaultPrinter(String pname)
        {
            myPrinters.SetDefaultPrinter(pname);
        }

        public void Printing(string pname, string path)
        {
            using (IOStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (PrintDocument pd = new PrintDocument())
                {
                    _qu = new PagingImage(path);

                    pd.PrintPage += new PrintPageEventHandler((sender, ev) =>
                    {
                        var img = _qu.GetPage(page);
                        if (img == null)
                        {
                            ev.HasMorePages = false;
                            return;
                        }
                        ev.Graphics.DrawImage(img, ev.Graphics.VisibleClipBounds);
                        page++;
                        ev.HasMorePages = _qu.Max >= page;
                    });
                    pd.PrinterSettings.PrinterName = pname;
                    pd.Print();

                }
            }
        }

    }
}

