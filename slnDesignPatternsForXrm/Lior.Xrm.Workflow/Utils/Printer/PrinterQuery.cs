using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace printpdf.Utils
{
    public class PrinterUtil
    {
   
        public static void Restart(){
            ServiceController service = new ServiceController("Spooler");
            if ((service.Status.Equals(ServiceControllerStatus.Stopped)) || (service.Status.Equals(ServiceControllerStatus.StopPending)))
            {
                service.Start();
            }
            else
            {
                service.Stop();

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running);
            }
        }
    
        public static bool HasJobs()
        {
            bool hasJob = false;
            Console.WriteLine("Retrieving printer queue information using WMI");
            Console.WriteLine("==================================");
            //Query printer queue
            System.Management.ObjectQuery oq = new System.Management.ObjectQuery("SELECT * FROM Win32_PrintJob");
            using (ManagementObjectSearcher query1 = new ManagementObjectSearcher(oq))
            {
                using (ManagementObjectCollection queryCollection1 = query1.Get())
                {
                    hasJob = queryCollection1 != null && queryCollection1.Count > 0;
                    //foreach (ManagementObject mo in queryCollection1)
                    //{
                    //    foreach (var prop in mo.Properties)
                    //    {
                    //        if (mo != null && mo[prop.Name] != null)
                    //        {
                    //            Console.WriteLine("Printer|" + prop.Name + ": " + mo[prop.Name] != null ? mo[prop.Name].ToString() : "");

                    //            Console.WriteLine("==================================");
                    //        }
                    //    } if (mo != null)
                    //    {
                    //        Console.WriteLine("Printer Driver : " + mo["DriverName"].ToString());
                    //        Console.WriteLine("Document Name : " + mo["Document"].ToString());
                    //        Console.WriteLine("Document Owner : " + mo["Owner"].ToString());
                    //        Console.WriteLine("==================================");
                    //    }
                    //}
                }
            }
            return hasJob;
        }
    }

}

