using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Autodesk.AutoCAD.Interop;

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
            CADApp = (AcadApplication)Marshal.GetActiveObject(_CADProgId); ;
        }

        private static void StartAutoCad()
        {
            var t = Type.GetTypeFromProgID(_CADProgId, true);
            // Create a new instance Autocad.
            var obj = Activator.CreateInstance(t, true);
            // No need for casting with dynamics
            CADApp = obj as AcadApplication;
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
            CADApp.Visible = true;
            if (CADApp == null)
                return false;

            dynamic cc = CADApp.ActiveDocument;
            cc.SendCommand("STP" +"\r");

            return true;
        }
    }
}
