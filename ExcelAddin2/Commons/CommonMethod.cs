using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelAddIn.Commons
{
    public class CommonMethod
    {

        public static string GetSheetName(EXCELSHEET_LIST selList)
        {
            string returnValue = "";
            switch (selList)
            {
                case EXCELSHEET_LIST.SHEET_GENERAL:
                    returnValue = "general";
                    break;
                case EXCELSHEET_LIST.SHEET_ROOF:
                    returnValue = "roof";
                    break;

                case EXCELSHEET_LIST.SHEET_SHELL:
                    returnValue = "shell";
                    break;
                case EXCELSHEET_LIST.SHEET_WINDGIRDER:
                    returnValue = "windgirder";
                    break;

                case EXCELSHEET_LIST.SHEET_STRUCTURE:
                    returnValue = "structure";
                    break;
                case EXCELSHEET_LIST.SHEET_BOTTOM:
                    returnValue = "bottom";
                    break;

                case EXCELSHEET_LIST.SHEET_APPURTENANCES:
                    returnValue = "appurtenances";
                    break;
                case EXCELSHEET_LIST.SHEET_NOZZLE:
                    returnValue = "nozzle";
                    break;
                case EXCELSHEET_LIST.SHEET_WELDING:
                    returnValue = "welding";
                    break;

                case EXCELSHEET_LIST.SHEET_DWGLIST:
                    returnValue = "dwglist";
                    break;
                case EXCELSHEET_LIST.SHEET_NOTES:
                    returnValue = "notes";
                    break;
                case EXCELSHEET_LIST.SHEET_LEADERLIST:
                    returnValue = "leaderlist";
                    break;

                case EXCELSHEET_LIST.SHEET_ANCHOR:
                    returnValue = "anchor";
                    break;
                case EXCELSHEET_LIST.SHEET_APPURT:
                    returnValue = "appurt";
                    break;
                case EXCELSHEET_LIST.SHEET_NOZZLESHELL:
                    returnValue = "nozzle_shell";
                    break;
                case EXCELSHEET_LIST.SHEET_NOZZLEROOF:
                    returnValue = "nozzle_roof";
                    break;
                case EXCELSHEET_LIST.SHEET_INTERNALPIPE:
                    returnValue = "internalpipe";
                    break;

                case EXCELSHEET_LIST.SHEET_FIREFIGHTING:
                    returnValue = "firefighting";
                    break;
                case EXCELSHEET_LIST.SHEET_SUMP:
                    returnValue = "sump";
                    break;
                case EXCELSHEET_LIST.SHEET_COMPRESSIONRING:
                    returnValue = "compressionring";
                    break;
                case EXCELSHEET_LIST.SHEET_CENTERING:
                    returnValue = "centering";
                    break;
                case EXCELSHEET_LIST.SHEET_COLUMNSUPT:
                    returnValue = "column_supt";
                    break;

                case EXCELSHEET_LIST.SHEET_RAFTERSUPTCLIP:
                    returnValue = "raftersuptclip";
                    break;
                case EXCELSHEET_LIST.SHEET_COLUMNRAFTER:
                    returnValue = "column_rafter";
                    break;
                case EXCELSHEET_LIST.SHEET_FLANGE:
                    returnValue = "flange";
                    break;
                case EXCELSHEET_LIST.SHEET_PIPESCH:
                    returnValue = "pipe_sch";
                    break;
                case EXCELSHEET_LIST.SHEET_HBEAM:
                    returnValue = "h_beam";
                    break;

                case EXCELSHEET_LIST.SHEET_ANGLE:
                    returnValue = "angle";
                    break;
                case EXCELSHEET_LIST.SHEET_CHANNEL:
                    returnValue = "channel";
                    break;
                case EXCELSHEET_LIST.SHEET_FITTING:
                    returnValue = "fiting";
                    break;
            }

            return returnValue;
        }
    }
}
