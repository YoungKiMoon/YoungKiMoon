using ExcelAddIn.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelAddIn.ExcelServices
{
    public static class ExcelService
    {
        public static void ChangeSheet(EXCELSHEET_LIST selSheetType)
        {
            Excel.Workbook selWorkBook = GetTankWorkbook();
            if (selWorkBook != null)
                SelectSheet(selWorkBook, selSheetType);

        }

        private static Excel.Workbook GetTankWorkbook()
        {
            Excel.Workbook selWorkBook = null;

            if (Globals.ThisAddIn.Application.Workbooks.Count > 0)
            {
                foreach (Excel.Workbook eachWork in Globals.ThisAddIn.Application.Workbooks)
                {
                    //
                }

                // Temp
                selWorkBook = GetActiveWorkBook();
            }

            return selWorkBook;
        }
        private static void SelectSheet(Excel.Workbook selWorkBook, EXCELSHEET_LIST selSheetType)
        {
            foreach(Excel.Worksheet eachSheet in selWorkBook.Worksheets)
            {
                string eachSheetName = eachSheet.Name;
                bool selectValue = false;
                switch (selSheetType)
                {
                    case EXCELSHEET_LIST.SHEET_MAIN:
                        if (eachSheetName.Contains("Main"))
                            selectValue = true;
                        break;
                    case EXCELSHEET_LIST.SHEET_BASIC:
                        if (eachSheetName.Contains("Basic"))
                            selectValue = true;
                        break;
                    case EXCELSHEET_LIST.SHEET_SHEEL:
                        if (eachSheetName.Contains("Shell"))
                            selectValue = true;
                        break;
                    case EXCELSHEET_LIST.SHEET_ROOF:
                        if (eachSheetName.Contains("Roof"))
                            selectValue = true;
                        break;
                    case EXCELSHEET_LIST.SHEET_BOTTOM:
                        if (eachSheetName.Contains("Bottom"))
                            selectValue = true;
                        break;

                    default:
                        break;
                }
                if (selectValue)
                {
                    eachSheet.Select();
                    return;
                }
            }
        }



        #region Excel Basic Function
        public static Excel.Worksheet GetActiveWorkSheet()
        {
            if (Globals.ThisAddIn.Application.ActiveWorkbook != null)
            {
                return Globals.ThisAddIn.Application.ActiveSheet as Excel.Worksheet;
            }
            else
            {
                return null;
            }

        }
        public static Excel.Workbook GetActiveWorkBook()
        {
            if (Globals.ThisAddIn.Application.ActiveWorkbook != null)
            {
                return Globals.ThisAddIn.Application.ActiveWorkbook as Excel.Workbook;
            }
            else
            {
                return null;
            }

        }
        #endregion
    }
}
