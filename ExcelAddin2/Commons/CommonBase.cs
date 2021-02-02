using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelAddIn.Commons
{
    public enum EXCELSHEET_LIST
    {
        NotSet = 0,
        SHEET_INFORMATION = 1,
        SHEET_GENERAL = 2,
        SHEET_SHEEL = 3,
        SHEET_ROOF = 4,
        SHEET_BOTTOM = 5,
        SHEET_STRUCTURE = 6,
        SHEET_NOZZLE = 7,
        SHEET_ACCESS = 8,
        SHEET_APPURTENANCES = 9,
    }

    public enum CUSTOMPANE_LIST
    {
        NotSet = 0,
        PROCESS = 1,
        INPUT = 2,
    }

    public enum CUSTOMWINDOW_LIST
    {
        NotSet = 0,
        PROCESS = 1,
        INPUT = 2,
    }

}
