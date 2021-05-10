using DrawWork.CommandModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DrawWork.FileServices
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
                MessageBox.Show("File Read Error" +"\r\n" + "\r\n" + ex.Message);
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
                MessageBox.Show("File Read Error" + "\r\n" + "\r\n" + ex.Message);
                return null;
            }
        }

        
    }
}
