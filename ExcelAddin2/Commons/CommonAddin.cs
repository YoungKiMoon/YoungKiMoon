using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelAddIn.Commons
{
    public class  CommonAddin
    {
        public static int GetAddinApplicationHWND()
        {
            return Globals.ThisAddIn.Application.Hwnd;
        }
    }
}
