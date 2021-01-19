using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelAddIn.Commons
{
    public enum EXCELSHEET_LIST
    {
        NotSet = 0,
        SHEET_MAIN = 1,
        SHEET_BASIC = 2,
        SHEET_SHEEL = 3,
        SHEET_ROOF = 4,
        SHEET_BOTTOM = 5,
        SHEET_ANGLE = 6,
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
