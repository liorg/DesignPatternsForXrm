using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malam.Toto.Crm.Workflow.Utils
{
    public class FileUtils
    {

        public FileUtils()
        {

        }

        public string CopyToTemp(string tempFolder, Tuple<string,string> yourBase64String)
        {
            string folderDate = DateTime.Now.ToString("yyyyMMdd");
            var path = System.IO.Path.Combine(tempFolder, folderDate);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var pathFolder = System.IO.Path.Combine(path, yourBase64String.Item1);
            File.WriteAllBytes(pathFolder, Convert.FromBase64String(yourBase64String.Item2));
            return pathFolder;
        }
       
    }
}
