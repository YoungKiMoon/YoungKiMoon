using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using xlApp = Microsoft.Office.Interop.Excel.Application;
using xlWin = Microsoft.Office.Interop.Excel.Window;

using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelDataLib.ExcelServices
{
    public class ExcelMacroService
    {
        public ExcelMacroService()
        {

        }


        public string RunAME(string selFile, string ameFile)
        {
            string returnValue = RunMacro(selFile, ameFile);


            return returnValue;
        }

        private string RunMacro(string selFile,string ameFile)
        {
            string returnValue = "";

            xlApp newApp = null;
            Excel.Workbook newWorkbook = null;

            try
            {
                newApp = new xlApp();
                newApp.Visible = false;

                newWorkbook = newApp.Workbooks.Open(selFile);

                newApp.Run("loadAME_All",ameFile);

                newWorkbook.Save();
                newWorkbook.Close(false);
                newApp.Quit();

                releaseObject(newApp);
                releaseObject(newWorkbook);

                returnValue = "";
            }
            catch(Exception ex)
            {
                if (newWorkbook != null)
                    newWorkbook.Close(false);
                if (newApp != null)
                    newApp.Quit();
                releaseObject(newApp);
                releaseObject(newWorkbook);

                Console.WriteLine(ex.Message);
                returnValue = ex.Message;
            }

            return returnValue;
        }

        public  string RunNozzle(string selFile, string fileName)
        {
            string returnValue = "";

            xlApp newApp = null;
            Excel.Workbook newWorkbook = null;

            try
            {
                newApp = new xlApp();
                newApp.Visible = false;

                newWorkbook = newApp.Workbooks.Open(selFile);

                newApp.Run("loadNozzle", fileName);

                newWorkbook.Save();
                newWorkbook.Close(false);
                newApp.Quit();

                releaseObject(newApp);
                releaseObject(newWorkbook);

                returnValue = "";
            }
            catch (Exception ex)
            {
                if (newWorkbook != null)
                    newWorkbook.Close(false);
                if (newApp != null)
                    newApp.Quit();
                releaseObject(newApp);
                releaseObject(newWorkbook);

                Console.WriteLine(ex.Message);
                returnValue = ex.Message;
            }

            return returnValue;
        }
        //~> Release the objects
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
