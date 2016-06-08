using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using printpdf;
using System.Runtime.InteropServices;

namespace printpdf.Utils
{

    public enum TypeActionPrint { GoustScript, AdobePrint };
    public static class UtilPrinters
    {
        static int _retry = 0;
        static void PrintPdf(Settings settings, string file2Print, Action<string> log, TypeActionPrint typeActionPrint = TypeActionPrint.GoustScript)
        {
            Log(log, "Start PRINTING " + file2Print);
            if (typeActionPrint == TypeActionPrint.AdobePrint)
            {
                PrintPdfByAdobeReader(settings, file2Print, log);
            }
            else
            {
                PrintPdfByPostScript(settings, file2Print, log);//preffer
            }
            // 
            Log(log, "End PRINTING " + file2Print);

        }

        static void PrintPdfByPostScript(Settings settings, string file2Print, Action<string> log)
        {
            // http://www.ghostscript.com/download/gsdnld.html
            Log(log, "Start Print Pdf By PostScript " + file2Print);

            ProcessStartInfo psInfo = new ProcessStartInfo();
            Log(log, "Print Pdf By PostScript " + settings.printerName);
            psInfo.Arguments = String.Format(" -dPrinted -dBATCH -dNOPAUSE -dNOSAFER -q -dNumCopies=1 -sDEVICE=ljet4 -sOutputFile=\"\\\\spool\\{0}\" \"{1}\"", settings.printerName, file2Print);
            psInfo.FileName = settings.GoustScriptPath; //@"C:\Program Files\gs\gs9.10\bin\gswin64c.exe";
            psInfo.UseShellExecute = false;
            Process process = Process.Start(psInfo);
            Log(log, "End Print Pdf By PostScript " + file2Print);
        }

        static void PrintPdfByAdobeReader(Settings settings, string file2Print, Action<string> log)
        {
            Log(log, "Start Print Pdf By AdobeReader " + file2Print);
            var pNewProcess = new Process();
            Log(log, "Configure Process StartInfo Printer ");
            ConfigureProcessStartInfo(pNewProcess, file2Print, settings, log);
            Log(log, "End Print Pdf By AdobeReader " + file2Print);

        }

        public static void Print(Settings settings, string file2Print, Action<string> log)
        {
            bool hasJob = PrinterUtil.HasJobs();
            if (hasJob)
            {
                do
                {
                    Thread.Sleep(settings.WaitingRetry);
                    _retry++;
                    hasJob = PrinterUtil.HasJobs();
                }
                while (hasJob && _retry <= settings.MaxRetry);
            }

            hasJob = PrinterUtil.HasJobs();
            if (hasJob) throw new ArgumentException("printer has job and over the retry");


            if (Path.GetExtension(file2Print).ToLower() == ".pdf")
                PrintPdf(settings, file2Print, log);
            else
                PrintImages(settings, file2Print, log);
        }

       static void PrintImages(Settings settings, string file2Print, Action<string> log)
        {

            PrintDocumentMethod p = new PrintDocumentMethod();

            p.Printing(settings.printerName, file2Print);
        }

        static void ConfigureProcessStartInfo(Process pNewProcess, string file2Print, Settings settings, Action<string> log)
        {

            pNewProcess.StartInfo.WorkingDirectory = settings.AdobePath;
            pNewProcess.StartInfo.FileName = settings.AdobePath + "\\" + settings.AdobeFileName;
            pNewProcess.StartInfo.Arguments = "/n /s /h /t \"" + @file2Print + "\" \"" + settings.printerName + "\"";
            pNewProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pNewProcess.StartInfo.CreateNoWindow = false;
            pNewProcess.StartInfo.UseShellExecute = false;
        }

        static void Log(Action<string> log, string s)
        {
            if (log != null)
            {
                log(s);
            }
            Console.WriteLine(s);
        }
    }
}
