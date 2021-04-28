using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelAddIn.Commons
{
    public enum EXCELSHEET_LIST
    {
        NotSet,
        
        SHEET_GENERAL,
        SHEET_ROOF,

        SHEET_SHELL,
        SHEET_WINDGIRDER,
        
        SHEET_STRUCTURE,
        SHEET_BOTTOM,
        
        SHEET_APPURTENANCES,
        SHEET_NOZZLE,
        SHEET_WELDING,

        SHEET_DWGLIST,
        SHEET_NOTES,
        SHEET_LEADERLIST,

        SHEET_ANCHOR,
        SHEET_APPURT,
        SHEET_NOZZLESHELL,
        SHEET_NOZZLEROOF,
        SHEET_INTERNALPIPE,

        SHEET_FIREFIGHTING,
        SHEET_SUMP,
        SHEET_COMPRESSIONRING,
        SHEET_CENTERING,
        SHEET_COLUMNSUPT,

        SHEET_RAFTERSUPTCLIP,
        SHEET_COLUMNRAFTER,
        SHEET_FLANGE,
        SHEET_PIPESCH,
        SHEET_HBEAM,

        SHEET_ANGLE,
        SHEET_CHANNEL,
        SHEET_FITTING,
    }

    public enum ROOF_TYPE
    {
        NotSet,
        CRT,
        DRT,
        IFRT,
        EFRTSingle,
        EFRTDouble,
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
