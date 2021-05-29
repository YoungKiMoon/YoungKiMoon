using ExcelAddIn.Commons;
using ExcelAddIn.ExcelModels;
using ExcelAddIn.Models;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Excel.Worksheet newSelectSheet = GetWorkSheet(selSheetType);

            if (newSelectSheet != null)
            {
                newSelectSheet.Select();
                newSelectSheet.Cells[1, 1].Select();
                Globals.ThisAddIn.ActionActiveSheet = GetActionActiveSheet(selSheetType, newSelectSheet);
            }


        }

        public static Excel.Worksheet GetWorkSheet(EXCELSHEET_LIST selSheetType)
        {
            Excel.Workbook selWorkBook = GetTankWorkbook();
            Excel.Worksheet returnSheet = null;

            if(selWorkBook != null)
            {
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




        #region Information Window
        public static ExcelWorkSheetModel GetActionActiveSheet(EXCELSHEET_LIST selSheetType, Excel.Worksheet selSheet)
        {
            SetInformationWindowArea(false,false);

            ExcelWorkSheetModel newModel = new ExcelWorkSheetModel();

            if (Globals.ThisAddIn.Application == null)
                return newModel;


            newModel.ExcelSheet = selSheet;

            // Each Event : binding
            ROOF_TYPE currentRoofType = GetSheetRoofType();
            switch (selSheetType)
            {
                case EXCELSHEET_LIST.SHEET_ROOF:

                    switch (currentRoofType)
                    {
                        case ROOF_TYPE.CRT:
                            AddInformationTable(GetSheetTable(EXCELSHEET_LIST.SHEET_ANGLE));
                            newModel.ExcelSheet.SelectionChange += ExcelSheetRoof_SelectionChange;
                            break;
                        case ROOF_TYPE.IFRT:
                        case ROOF_TYPE.EFRTSingle:
                            SetInformationWindowArea(true, false);
                            //AddInformationImage(new List<ImageModel> { new ImageModel("Roof_FRTsingleDeck", "Roof_FRTsingleDeck") });
                            break;
                        case ROOF_TYPE.EFRTDouble:
                            SetInformationWindowArea(true, false);
                            //AddInformationImage(new List<ImageModel> { new ImageModel("Roof_FRTdoubleDeck", "Roof_FRTdoubleDeck") });
                            break;
                    }
                    break;

                case EXCELSHEET_LIST.SHEET_SHELL:
                    newModel.ExcelSheet.SelectionChange += ExcelSheetShell_SelectionChange;
                    break;

                case EXCELSHEET_LIST.SHEET_BOTTOM:
                    newModel.ExcelSheet.SelectionChange += ExcelSheetBottom_SelectionChange;
                    break;

                case EXCELSHEET_LIST.SHEET_STRUCTURE:
                    switch (currentRoofType)
                    {
                        case ROOF_TYPE.CRT:
                        case ROOF_TYPE.DRT:
                            newModel.ExcelSheet.SelectionChange += ExcelSheetStructure_SelectionChange;
                            break;
                        case ROOF_TYPE.IFRT:
                        case ROOF_TYPE.EFRTSingle:
                            SetInformationWindowArea(true, false);
                            //AddInformationImage(new List<ImageModel> { new ImageModel("Structure_FRTsingleDeck", "Structure_FRTsingleDeck") });
                            break;
                        case ROOF_TYPE.EFRTDouble:
                            SetInformationWindowArea(true, false);
                            //AddInformationImage(new List<ImageModel> { new ImageModel("Structure_FRTdoubleDeck", "Structure_FRTdoubleDeck") });
                            break;
                    }
                    break;

                case EXCELSHEET_LIST.SHEET_APPURTENANCES:
                    newModel.ExcelSheet.SelectionChange += ExcelSheetAppurtenances_SelectionChange;
                    break;

            }


            return newModel;
        }

        private static void ExcelSheetAppurtenances_SelectionChange(Range Target)
        {
            SetInformationWindowArea(true, false);

            if (Target.Column == 6)
            {
                if (Target.Row == 11)
                {
                    // Name Plate
                    List<ImageModel> newImage = new List<ImageModel>();
                    newImage.Add(new ImageModel("NamePlate", "Name Plate"));

                    AddInformationImage(newImage);
                }
                else if (Target.Row == 23)
                {
                    // Earth Lug
                    AddInformationImage(new List<ImageModel> { new ImageModel("EarthLug", "Earth Lug") });
                }

            }
            else if (Target.Column == 13)
            {
                if (Target.Row == 11)
                {
                    // Settlement
                    AddInformationImage(new List<ImageModel> { new ImageModel("SettlementCheckPiece", "Settlement Check Piece") });
                }
                else if (Target.Row == 12)
                {
                    // Scaffold Cable 
                    AddInformationImage(new List<ImageModel> { new ImageModel("ScaffoldCableSupt", "Scaffold Cable Support") });
                }
            }
        }

        private static void ExcelSheetStructure_SelectionChange(Range Target)
        {
            SetInformationWindowArea(true, false);

            if (Target.Column == 14)
            {
                if (Target.Row == 14)
                {
                    // Centering position
                    List<ImageModel> newImage = new List<ImageModel>();
                    newImage.Add(new ImageModel("CRT_Centering_int", "Internal"));
                    newImage.Add(new ImageModel("CRT_Centering_ext", "External"));

                    AddInformationImage(newImage);
                }
                else if (Target.Row == 22)
                {
                    AddInformationImage(new List<ImageModel> { new ImageModel("ShapeSteel_Angle", "Purlin Size") });
                }

            }
            else if (Target.Column == 6)
            {
                if (Target.Row >=35 && Target.Row <=39)
                {
                    AddInformationImage(new List<ImageModel> { new ImageModel("ShapeSteel_Channel", "Rafter Size") });
                }
            }
        }

        private static void ExcelSheetBottom_SelectionChange(Range Target)
        {
            SetInformationWindowArea(true, false);

            if (Target.Column == 16)
            {
                if (Target.Row == 6)
                {
                    // Anchor Chair
                    List<ImageModel> newImage = new List<ImageModel>();
                    newImage.Add(new ImageModel("AnchorChair", "TYPE I, TYPE II"));
                    AddInformationImage(newImage);
                }
            }
        }

        private static void ExcelSheetShell_SelectionChange(Range Target)
        {
            SetInformationWindowArea(true, false);

            if (Target.Column == 14)
            {
                if (Target.Row >=23 && Target.Row <36)
                {

                    // Stiffening ring
                    List<ImageModel> newImage = new List<ImageModel>();
                    newImage.Add(new ImageModel("Stiffening-ring_detail_c", "Detail c"));
                    newImage.Add(new ImageModel("Stiffening-ring_detail_d", "Detail d"));
                    newImage.Add(new ImageModel("Stiffening-ring_detail_e", "Detail e"));
                    AddInformationImage(newImage);
                }
                else if (Target.Row == 14 || Target.Row == 15 || Target.Row == 16)
                {
                    // Angle
                    //AddInformationImage(new List<ImageModel> { new ImageModel("ShapeSteel_Angle", "Angle") });

                }
            }
        }

        private static void ExcelSheetRoof_SelectionChange(Range Target)
        {
            SetInformationWindowArea(true, false);
            if (Target.Column == 6)
            {
                if (Target.Row == 25)
                {
                    // Compression Ring
                    List<ImageModel> newImage = new List<ImageModel>();
                    newImage.Add(new ImageModel("compressionRing_detail_b", "Detail b"));
                    newImage.Add(new ImageModel("compressionRing_detail_d", "Detail d"));
                    newImage.Add(new ImageModel("CompressionRing_detail_e(new)", "Detail e"));
                    newImage.Add(new ImageModel("compressionRing_detail_i", "Detail i"));
                    newImage.Add(new ImageModel("compressionRing_detail_k", "Detail k"));

                    AddInformationImage(newImage);
                }
                else if (Target.Row == 28 || Target.Row == 31  || Target.Row == 34)
                {
                    // Angle
                    SetInformationWindowArea(true, true);
                    AddInformationImage(new List<ImageModel> { new ImageModel("ShapeSteel_Angle", "Angle") });
                    
                }
            }
        }


        public static void SetInformationWindowArea(bool imageVisible, bool listVisible)
        {
            if (Globals.ThisAddIn.customInputPane != null)
            {
                Globals.ThisAddIn.customInputPane.elementHost1WPF.imageArea.Children.Clear();

                if (imageVisible)
                {
                    Globals.ThisAddIn.customInputPane.elementHost1WPF.imageArea.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    Globals.ThisAddIn.customInputPane.elementHost1WPF.imageArea.Visibility = System.Windows.Visibility.Collapsed;
                }

                if (listVisible)
                {
                    Globals.ThisAddIn.customInputPane.elementHost1WPF.listArea.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    Globals.ThisAddIn.customInputPane.elementHost1WPF.listArea.Visibility = System.Windows.Visibility.Collapsed;
                }

            }


        }

        private static void AddInformationImage(List<ImageModel> selImageList)
        {
            List<ImageModel> newImageList = new List<ImageModel>();
            foreach (ImageModel eachImage in selImageList)
                newImageList.Add(new ImageModel("/ExcelAddIn;component/AssemblyImage/" + eachImage.ImagePath + ".png", eachImage.ImageName));

            if (Globals.ThisAddIn.customInputPane != null)
            {
                if (Globals.ThisAddIn.customInputPane.Visible == false)
                    Globals.ThisAddIn.customInputPane.Visible = true;
                Globals.ThisAddIn.customInputPane.elementHost1WPF.AddImage(newImageList);
            }

        }
        private static void AddInformationTable(ObservableCollection<TableModel> selTable)
        {
            if (Globals.ThisAddIn.customInputPane != null)
            {
                if (Globals.ThisAddIn.customInputPane.Visible == false)
                    Globals.ThisAddIn.customInputPane.Visible = true;
                Globals.ThisAddIn.customInputPane.elementHost1WPF.AddTable(selTable);
            }
        }


        #endregion

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
                Range tempRange1 = roofSheet.Range[roofSheet.Cells[1, 1], roofSheet.Cells[1, 50]];
                tempRange1.EntireColumn.Hidden = true;
                Range roofViewRange = null;

                switch (selRoofType)
                {
                    case ROOF_TYPE.CRT:
                        roofViewRange = roofSheet.Range[roofSheet.Cells[1, 1], roofSheet.Cells[1, 9]];
                        roofViewRange.EntireColumn.Hidden = false;
                        break;
                    case ROOF_TYPE.DRT:
                        roofViewRange = roofSheet.Range[roofSheet.Cells[1, 11], roofSheet.Cells[1, 19]];
                        roofViewRange.EntireColumn.Hidden = false;
                        break;
                    case ROOF_TYPE.IFRT:
                        roofViewRange = roofSheet.Range[roofSheet.Cells[1, 21], roofSheet.Cells[1, 29]];
                        roofViewRange.EntireColumn.Hidden = false;
                        break;
                    case ROOF_TYPE.EFRTSingle:
                        roofViewRange = roofSheet.Range[roofSheet.Cells[1, 31], roofSheet.Cells[1, 39]];
                        roofViewRange.EntireColumn.Hidden = false;
                        break;
                    case ROOF_TYPE.EFRTDouble:
                        roofViewRange = roofSheet.Range[roofSheet.Cells[1, 41], roofSheet.Cells[1, 49]];
                        roofViewRange.EntireColumn.Hidden = false;
                        break;
                }
            }

            // Structure
            Excel.Worksheet structureSheet = GetWorkSheet(EXCELSHEET_LIST.SHEET_STRUCTURE);
            if (structureSheet != null)
            {
                Range tempRange1 = structureSheet.Range[structureSheet.Cells[1, 1], structureSheet.Cells[1, 85]];
                tempRange1.EntireColumn.Hidden = true;
                Range structureViewRange = null;

                switch (selRoofType)
                {
                    case ROOF_TYPE.CRT:
                        structureViewRange = structureSheet.Range[structureSheet.Cells[1, 1], structureSheet.Cells[1, 16]];
                        structureViewRange.EntireColumn.Hidden = false;
                        break;
                    case ROOF_TYPE.DRT:
                        structureViewRange = structureSheet.Range[structureSheet.Cells[1, 18], structureSheet.Cells[1, 33]];
                        structureViewRange.EntireColumn.Hidden = false;
                        break;
                    case ROOF_TYPE.IFRT:
                        structureViewRange = structureSheet.Range[structureSheet.Cells[1, 35], structureSheet.Cells[1, 50]];
                        structureViewRange.EntireColumn.Hidden = false;
                        break;
                    case ROOF_TYPE.EFRTSingle:
                        structureViewRange = structureSheet.Range[structureSheet.Cells[1, 52], structureSheet.Cells[1, 67]];
                        structureViewRange.EntireColumn.Hidden = false;
                        break;
                    case ROOF_TYPE.EFRTDouble:
                        structureViewRange = structureSheet.Range[structureSheet.Cells[1, 69], structureSheet.Cells[1, 85]];
                        structureViewRange.EntireColumn.Hidden = false;
                        break;
                }
            }

            // General
            Excel.Worksheet generalSheet = GetWorkSheet(EXCELSHEET_LIST.SHEET_GENERAL);
            if(generalSheet != null)
            {
                string typeString = "";
                switch (selRoofType)
                {
                    case ROOF_TYPE.CRT:
                        typeString = "CRT";
                        break;
                    case ROOF_TYPE.DRT:
                        typeString = "DRT";
                        break;
                    case ROOF_TYPE.IFRT:
                        typeString = "IFRT";
                        break;
                    case ROOF_TYPE.EFRTSingle:
                        typeString = "EFRT_SingleDeck";
                        break;
                    case ROOF_TYPE.EFRTDouble:
                        typeString = "EFRT_DoubleDeck";
                        break;
                }
                if (typeString != "")
                    generalSheet.Cells[16, 6] = typeString;

            }
        }

        public static ROOF_TYPE GetSheetRoofType()
        {
            ROOF_TYPE returnValue = ROOF_TYPE.NotSet;
            Excel.Worksheet generalSheet = GetWorkSheet(EXCELSHEET_LIST.SHEET_GENERAL);
            if (generalSheet != null)
            {
                string typeString = generalSheet.Cells[16, 6].Value;
                switch (typeString)
                {
                    case "CRT":
                        returnValue = ROOF_TYPE.CRT;
                        break;
                    case "DRT":
                        returnValue = ROOF_TYPE.DRT;
                        break;
                    case "IFRT":
                        returnValue = ROOF_TYPE.IFRT;
                        break;
                    case "EFRT_singleDeck":
                        returnValue = ROOF_TYPE.EFRTSingle;
                        break;
                    case "EFRT_doubleDeck":
                        returnValue = ROOF_TYPE.EFRTDouble;
                        break;
                }
            }
            return returnValue;
        }
        #endregion

        #region Table
        public static ObservableCollection<TableModel> GetSheetTable(EXCELSHEET_LIST selSheet)
        {
            ObservableCollection<TableModel> newTable = new ObservableCollection<TableModel>();

            Excel.Worksheet newSheet = GetWorkSheet(selSheet);
            if (newSheet != null)
            {
                var tempTable = newSheet.Cells[3, 1].Resize[37, 7].Value;
                if(tempTable != null)
                {
                    object[,] tempArray = tempTable as object[,];
                    int maxRow =tempArray.GetLength(0);
                    int maxCol = tempArray.GetLength(1);
                    for(int i=1;i<=maxRow; i++)
                    {
                        TableModel newRow = new TableModel();
                        newRow.COL01 = tempArray[i, 1].ToString();
                        newRow.COL02 = tempArray[i, 2].ToString();
                        newRow.COL03 = tempArray[i, 3].ToString();
                        newRow.COL04 = tempArray[i, 4].ToString();
                        newRow.COL05 = tempArray[i, 5].ToString();
                        newRow.COL06 = tempArray[i, 6].ToString();
                        newRow.COL07 = tempArray[i, 7].ToString();
                        newTable.Add(newRow);
                    }
                }
                
            }

            return newTable;
        }
        #endregion
    }
}
