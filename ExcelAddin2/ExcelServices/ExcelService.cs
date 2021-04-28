using ExcelAddIn.Commons;
using ExcelAddIn.ExcelModels;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Excel = Microsoft.Office.Interop.Excel;
using xlApp = Microsoft.Office.Interop.Excel.Application;
using xlWin = Microsoft.Office.Interop.Excel.Window;

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
            string selSheetName = CommonMethod.GetSheetName(selSheetType);
            foreach (Excel.Worksheet eachSheet in selWorkBook.Worksheets)
            {
                string eachSheetName = eachSheet.Name.ToLower();
                if (selSheetName == eachSheetName)
                {
                    eachSheet.Select();
                    break;
                }
            }

            //Globals.ThisAddIn.roofSheet= GetActiveSheetModel("Roof");
        }

        public static Excel.Worksheet GetWorkSheet(EXCELSHEET_LIST selSheetType)
        {
            Excel.Workbook selWorkBook = GetTankWorkbook();
            Excel.Worksheet returnSheet = null;

            string selSheetName = CommonMethod.GetSheetName(selSheetType);
            foreach (Excel.Worksheet eachSheet in selWorkBook.Worksheets)
            {
                string eachSheetName = eachSheet.Name.ToLower();
                if (selSheetName == eachSheetName)
                {
                    returnSheet = eachSheet;
                    break;
                }
            }
            return returnSheet;
        }

        public static ExcelWorkSheetModel GetSelectModel(xlApp selApp, string selWork, string selSheet)
        {
            ExcelWorkSheetModel newModel = new ExcelWorkSheetModel();

            if (selApp != null)
            {
                foreach (Excel.Workbook eachWork in selApp.Workbooks)
                {
                    if (eachWork.Name == selWork)
                    {
                        foreach (Excel.Worksheet eachSheet in eachWork.Worksheets)
                        {
                            if (eachSheet.Name == selSheet)
                            {
                                newModel.ExcelApp = selApp;
                                newModel.ExcelWork = eachWork;
                                newModel.ExcelWorkName = selWork;
                                newModel.ExcelSheet = eachSheet;
                                newModel.ExcelSheetName = selSheet;

                            }
                        }
                    }

                }
            }

            return newModel;
        }

        public static ExcelWorkSheetModel GetActiveSheetModel(string selSheet)
        {
            InformationWindowReset();

            ExcelWorkSheetModel newModel = new ExcelWorkSheetModel();

            if (Globals.ThisAddIn.Application == null)
                return newModel;

            Excel.Worksheet newSheet = null;
            newSheet = GetActiveWorkSheet();
            if (newSheet != null)
                if (newSheet.Name == selSheet)
                {
                    newModel.ExcelSheet = newSheet;
                    newModel.ExcelSheet.SelectionChange += ExcelSheet_SelectionChange;
                }

            return newModel;
        }

        private static void InformationWindowReset()
        {
            if (Globals.ThisAddIn.customInputPane != null)
            {
                Globals.ThisAddIn.customInputPane.elementHost1WPF.imageArea.Visibility = System.Windows.Visibility.Collapsed;
                Globals.ThisAddIn.customInputPane.elementHost1WPF.listArea.Visibility = System.Windows.Visibility.Collapsed;
            }


        }
        private static void ExcelSheet_SelectionChange(Excel.Range Target)
        {
            Globals.ThisAddIn.customInputPane.elementHost1WPF.imageArea.Visibility = System.Windows.Visibility.Collapsed;
            Globals.ThisAddIn.customInputPane.elementHost1WPF.listArea.Visibility = System.Windows.Visibility.Collapsed;

            if (Target.Column == 5 && (Target.Row == 8 || Target.Row == 9))
            {
                // angle
                Globals.ThisAddIn.customInputPane.elementHost1WPF.listArea.Visibility = System.Windows.Visibility.Visible;
                Globals.ThisAddIn.customInputPane.elementHost1WPF.listAreaimgItem.Source = new BitmapImage(new Uri("/ExcelAddIn;component/Resources/angle_image.png", UriKind.Relative));
                Globals.ThisAddIn.customInputPane.elementHost1WPF.listAreaListHedaerItem.Source = new BitmapImage(new Uri("/ExcelAddIn;component/Resources/angle_header.png", UriKind.Relative));
                Globals.ThisAddIn.customInputPane.elementHost1WPF.listAreaListItem.Source = new BitmapImage(new Uri("/ExcelAddIn;component/Resources/angle_list.png",UriKind.Relative));

            }
            else if (Target.Row == 15)
            {
                // abc
                Globals.ThisAddIn.customInputPane.elementHost1WPF.imageArea.Visibility = System.Windows.Visibility.Visible;
                Globals.ThisAddIn.customInputPane.elementHost1WPF.imageAreaimgItem.Source = new BitmapImage(new Uri("/ExcelAddIn;component/Resources/roof_a.png", UriKind.Relative));
            }
            else if (Target.Row == 20)
            {
                // k
                Globals.ThisAddIn.customInputPane.elementHost1WPF.imageArea.Visibility = System.Windows.Visibility.Visible;
                Globals.ThisAddIn.customInputPane.elementHost1WPF.imageAreaimgItem.Source = new BitmapImage(new Uri("/ExcelAddIn;component/Resources/roof_de.png", UriKind.Relative));
            }
            else if (Target.Row == 25)
            {
                // i
                Globals.ThisAddIn.customInputPane.elementHost1WPF.imageArea.Visibility = System.Windows.Visibility.Visible;
                Globals.ThisAddIn.customInputPane.elementHost1WPF.imageAreaimgItem.Source = new BitmapImage(new Uri("/ExcelAddIn;component/Resources/roof_k.png", UriKind.Relative));
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


        #region Roof Type Setting
        public static void ChangeRoofType(ROOF_TYPE selRoofType)
        {
            // Roof
            Excel.Worksheet roofSheet = GetWorkSheet(EXCELSHEET_LIST.SHEET_ROOF);
            if(roofSheet != null)
            {
                Range tempRange1 = roofSheet.Range[roofSheet.Cells[1, 3], roofSheet.Cells[1, 39]];
                tempRange1.EntireColumn.Hidden = true;
                Range roofViewRange = null;

                switch (selRoofType)
                {
                    case ROOF_TYPE.CRT:
                        roofViewRange = roofSheet.Range[roofSheet.Cells[1, 3], roofSheet.Cells[1, 8]];
                        roofViewRange.EntireColumn.Hidden = false;
                        break;
                    case ROOF_TYPE.DRT:
                        roofViewRange = roofSheet.Range[roofSheet.Cells[1, 10], roofSheet.Cells[1, 16]];
                        roofViewRange.EntireColumn.Hidden = false;
                        break;
                    case ROOF_TYPE.IFRT:
                        roofViewRange = roofSheet.Range[roofSheet.Cells[1, 18], roofSheet.Cells[1, 24]];
                        roofViewRange.EntireColumn.Hidden = false;
                        break;
                    case ROOF_TYPE.EFRTSingle:
                        roofViewRange = roofSheet.Range[roofSheet.Cells[1, 26], roofSheet.Cells[1, 32]];
                        roofViewRange.EntireColumn.Hidden = false;
                        break;
                    case ROOF_TYPE.EFRTDouble:
                        roofViewRange = roofSheet.Range[roofSheet.Cells[1, 34], roofSheet.Cells[1, 39]];
                        roofViewRange.EntireColumn.Hidden = false;
                        break;
                }
            }

            // Structure
            Excel.Worksheet structureSheet = GetWorkSheet(EXCELSHEET_LIST.SHEET_STRUCTURE);
            if (structureSheet != null)
            {
                Range tempRange1 = structureSheet.Range[structureSheet.Cells[1, 3], structureSheet.Cells[1, 59]];
                tempRange1.EntireColumn.Hidden = true;
                Range structureViewRange = null;

                switch (selRoofType)
                {
                    case ROOF_TYPE.CRT:
                        structureViewRange = structureSheet.Range[structureSheet.Cells[1, 3], structureSheet.Cells[1, 13]];
                        structureViewRange.EntireColumn.Hidden = false;
                        break;
                    case ROOF_TYPE.DRT:
                        structureViewRange = structureSheet.Range[structureSheet.Cells[1, 15], structureSheet.Cells[1, 25]];
                        structureViewRange.EntireColumn.Hidden = false;
                        break;
                    case ROOF_TYPE.IFRT:
                        structureViewRange = structureSheet.Range[structureSheet.Cells[1, 27], structureSheet.Cells[1, 37]];
                        structureViewRange.EntireColumn.Hidden = false;
                        break;
                    case ROOF_TYPE.EFRTSingle:
                        structureViewRange = structureSheet.Range[structureSheet.Cells[1, 39], structureSheet.Cells[1, 48]];
                        structureViewRange.EntireColumn.Hidden = false;
                        break;
                    case ROOF_TYPE.EFRTDouble:
                        structureViewRange = structureSheet.Range[structureSheet.Cells[1, 50], structureSheet.Cells[1, 59]];
                        structureViewRange.EntireColumn.Hidden = false;
                        break;
                }
            }
        }
        #endregion
    }
}
