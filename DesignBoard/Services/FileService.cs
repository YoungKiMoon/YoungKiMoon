using DesignBoard.Commons;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DesignBoard.Services
{
    public class FileService
    {



        private List<string> GetOpenInformation(OpenFile_Type selType)
        {
            List<string> returnValue = new List<string>();
            switch (selType)
            {
                case OpenFile_Type.AMETankData:
                    returnValue.Add("AMETank DATA LOAD");
                    returnValue.Add("AMETank File Type (*.doc)|*.doc;");
                    break;
                case OpenFile_Type.EngData:
                    returnValue.Add("EXISTING DATA LOAD : Engineering Data");
                    returnValue.Add("TABAS File Type (*.xlsm)|*.xlsm;");
                    break;
                case OpenFile_Type.NozzleData:
                    returnValue.Add("Nozzle DATA LOAD");
                    returnValue.Add("Nozzle File Type (*.xlsx)|*.xlsx;");
                    break;
                case OpenFile_Type.FireFighting:
                    returnValue.Add("FireFighting DATA LOAD");
                    returnValue.Add("FireFighting File Type (*.xlsm)|*.xlsm;");
                    break;
                case OpenFile_Type.WaterSpray:
                    returnValue.Add("WATER SPRAY INFORM");
                    returnValue.Add("WaterSpray File Type (*.xlsx)|*.xlsx;");
                    break;
                case OpenFile_Type.FoamSystem:
                    returnValue.Add("FOAM SYSTEM INFORM");
                    returnValue.Add("FoamSystem File Type (*.xlsx)|*.xlsx;");
                    break;
            }


            return returnValue;
        }
        public string GetFile(OpenFile_Type selType )
        {
            string returnValue = "";
            List<string> openInfo = GetOpenInformation(selType);
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = openInfo[0];
            ofd.Filter = openInfo[1];
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
            {
                foreach (string eachFile in ofd.FileNames)
                {
                    //selView.currentNoticeList.ImagePath.Name = System.IO.Path.GetFileName(eachFile);
                    //selView.currentNoticeList.ImagePath.Path = eachFile;
                    returnValue=eachFile;
                    break;
                }

            }

            return returnValue;
        }
    }
}
