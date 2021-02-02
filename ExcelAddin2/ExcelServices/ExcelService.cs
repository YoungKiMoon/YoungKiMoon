using ExcelAddIn.Commons;
using ExcelAddIn.ExcelModels;
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
            foreach (Excel.Worksheet eachSheet in selWorkBook.Worksheets)
            {
                string eachSheetName = eachSheet.Name;
                bool selectValue = false;
                switch (selSheetType)
                {
                    case EXCELSHEET_LIST.SHEET_INFORMATION:
                        if (eachSheetName.Contains("Information"))
                            selectValue = true;
                        break;
                    case EXCELSHEET_LIST.SHEET_GENERAL:
                        if (eachSheetName.Contains("General"))
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
                    break;
                }

            }

            Globals.ThisAddIn.roofSheet= GetActiveSheetModel("Roof");
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
            Globals.ThisAddIn.customInputPane.elementHost1WPF.imageArea.Visibility = System.Windows.Visibility.Collapsed;
            Globals.ThisAddIn.customInputPane.elementHost1WPF.listArea.Visibility = System.Windows.Visibility.Collapsed;

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
    }
}
