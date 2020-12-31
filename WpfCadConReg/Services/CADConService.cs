using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
//using Autodesk.AutoCAD.Interop;


namespace WpfCadCon.Services
{
    public class CADConService
    {
        private const string _CADProgId = "AutoCAD.Application";
        private static dynamic _CADApp = null;
        public static dynamic CADApp
        {
            get { return _CADApp; }
            set { _CADApp = value; }
        }


        private static void GetAutoCAD()
        {
            //CADApp = (AcadApplication)Marshal.GetActiveObject(_CADProgId);
            CADApp = Marshal.GetActiveObject(_CADProgId);
        }

        private static void StartAutoCad()
        {
            var t = Type.GetTypeFromProgID(_CADProgId, true);
            // Create a new instance Autocad.
            var obj = Activator.CreateInstance(t, true);
            // No need for casting with dynamics
            //CADApp = obj as AcadApplication;
            CADApp = null;
        }

        private static void EnsureAutoCadIsRunning()
        {
            if (CADApp == null)
            {
                try
                {
                    GetAutoCAD();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    try
                    {
                        StartAutoCad();
                    }
                    catch(Exception e2x)
                    {
                        Console.WriteLine(e2x.Message);
                    }
                }
            }
        }

        public bool RunProgram()
        {
            EnsureAutoCadIsRunning();
            
            if (CADApp == null)
                return false;

            CADApp.Visible = true;

            //MessageBox.Show("Now running " + CADApp.Name +" version " + CADApp.Version);



            string comSpace = " ";
            dynamic CADDoc = CADApp.ActiveDocument;

            /*
            char comEquivalent = (char)34;  //Quotation Marks
            string dllPath = "C:\\Users\\tree\\source\\cadProject\\Wpfcad\\bin\\Debug\\";
            string dllName = "Wpf.dll";
            dllPath = dllPath.Replace("\\", "/");
            dllPath = "C:/ASUS/";
            CADDoc.SendCommand("(command " + comEquivalent + "_NETLOAD" + comEquivalent + " " +
                                    comEquivalent + dllPath +dllName + comEquivalent + ") ");

            */

            CADDoc.SendCommand("STP" + comSpace);

            return true;
        }
    }
}
