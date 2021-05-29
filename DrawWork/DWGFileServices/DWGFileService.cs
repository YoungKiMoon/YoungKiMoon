using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DrawWork.DWGFileServices
{
    public class DWGFileService
    {
        public  void StartProcessWithFile()
        {
            var assembly = Assembly.GetExecutingAssembly();
            //Getting names of all embedded resources
            var allResourceNames = assembly.GetManifestResourceNames();
            //Selecting first one. 
            var resourceName = allResourceNames[0];
            var pathToFile = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) +
                              resourceName;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var fileStream = File.Create(pathToFile))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }

            var process = new Process();
            process.StartInfo.FileName = pathToFile;
            process.Start();
        }
        public string FileFilecopy()
        {
            string pathToFile = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\" +
                  "BlockSample.dwg";
            //File.Copy(@"Resources\BLOCK_20210527.dwg", @pathToFile);
            File.WriteAllBytes(@pathToFile, Properties.Resources.BLOCK_20210527);

            Thread.Sleep(200);
            return pathToFile;
        }
        public string StartProcessWithFilePath()
        {
            var assembly = Assembly.GetExecutingAssembly();
            //Getting names of all embedded resources
            var allResourceNames = assembly.GetManifestResourceNames();
            //Selecting first one. 
            var resourceName = allResourceNames[0];
            var pathToFile = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) +
                              resourceName;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var fileStream = File.Create(pathToFile))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }

            var process = new Process();
            process.StartInfo.FileName = pathToFile;
            process.Start();

            return "";
        }
    }
}
