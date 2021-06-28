using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CalculationLib.FilesServices
{
    public class TextFileService
    {
        public TextFileService()
        {

        }

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
    }
}
