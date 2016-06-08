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
    public class PagingImage
    {
        Image img;
        int _pages;
        string _filename;

        public PagingImage(string filename)
        {
            _filename = filename;
            img = Bitmap.FromFile(filename);
            _pages = img.GetFrameCount(FrameDimension.Page);
            img.Dispose();
        }

        public int Max
        {
            get
            {
                return _pages;
            }
        }

        public Image GetPage(int page)
        {
            if (page < 1 || page > _pages) return null;
            img = Bitmap.FromFile(_filename);
            img.SelectActiveFrame(FrameDimension.Page, page - 1);
            Image ret = new Bitmap(img);
            img.Dispose();
            return ret;
        }
    }

    
}
