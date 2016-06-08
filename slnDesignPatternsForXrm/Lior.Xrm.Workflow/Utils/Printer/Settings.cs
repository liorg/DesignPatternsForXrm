using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace printpdf
{
    public class Settings
    {
        public string AdobeFileName { get; set; }

        public int WaitingRetry { get; set; }

        public int MaxRetry { get; set; }

        public string AdobePath { get; set; }

        public string printerName { get; set; }

        public string GoustScriptPath { get; set; }
    }
}
