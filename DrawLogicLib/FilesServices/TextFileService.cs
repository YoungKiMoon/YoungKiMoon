using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DrawLogicLib.FilesServices
{
    public class TextFileService
    {

        public string GetTextFileString(string selFilePath)
        {
            try
            {
                if (File.Exists(selFilePath))
                {
                    return File.ReadAllText(selFilePath);
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("File Read Error" + "\r\n" + "\r\n" + ex.Message);
                Console.WriteLine("File Read Error" + "\r\n" + "\r\n" + ex.Message);
                return "";
            }
        }
        public string[] GetTextFileArray(string selFilePath)
        {
            try
            {
                if (File.Exists(selFilePath))
                {
                    return File.ReadAllLines(selFilePath);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("File Read Error" + "\r\n" + "\r\n" + ex.Message);
                Console.WriteLine("File Read Error" + "\r\n" + "\r\n" + ex.Message);
                return null;
            }
        }

        public string[] ReadResourceFile(string selFilePath)
        {

            string newStrAll = GetEmbeddedResource("DrawLogicLib.DrawLogicFiles.", selFilePath);
            string newStr = newStrAll.Replace("\t", "");
            string[] newArray = newStr.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return newArray;
            
        }

        public string GetEmbeddedResource(string namespacename, string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = namespacename + filename;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }

    }
}
