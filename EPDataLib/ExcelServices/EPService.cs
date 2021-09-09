using EPDataLib.ExcelModels;
using EPDataLib.Utils;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace EPDataLib.ExcelServices
{
    public class EPService
    {
        public EPService()
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
        }

        public bool RunSample(string selFilePath)
        {
            if (File.Exists(selFilePath))
            {
                Console.WriteLine("File not found");
                return false;
            }

            FileInfo existingFile = new FileInfo(selFilePath);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                int wsCount = package.Workbook.Worksheets.Count;
                ExcelWorksheet tempSheet = package.Workbook.Worksheets[0];
  

            }
            
            return true;
        }




        public ObservableCollection<EPWorkSheetModel> GetSheetList(string selFilePath)
        {
            

            ObservableCollection<EPWorkSheetModel> newList = new ObservableCollection<EPWorkSheetModel>();
            if (!File.Exists(selFilePath))
            {
                Console.WriteLine("File not found");
                return newList;
            }

            FileInfo existingFile = new FileInfo(selFilePath);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {

                foreach(ExcelWorksheet eachSheet in package.Workbook.Worksheets)
                {
                    EPWorkSheetModel newModel = new EPWorkSheetModel();
                    if (eachSheet.Dimension != null)
                    {
                        newModel.RowCount = eachSheet.Dimension.Rows;
                        newModel.ColumnCount = eachSheet.Dimension.Columns;
                        ExcelRange curRange = eachSheet.Cells[1, 1, newModel.RowCount, newModel.ColumnCount];
                        var curArray = curRange.Value;
                        newModel.DataArray = curArray;
                    }
                    newModel.ExcelSheetName = eachSheet.Name;
                    newList.Add(newModel);
                }

            }



            return newList;

        }


        public bool CheckTABASExcel(ExcelWorksheets selEWorksheets)
        {
            bool returnValue = false;

            foreach (ExcelWorksheet eachSheet in selEWorksheets)
            {
                if (eachSheet.Name.Contains("AMEdata"))
                {
                    returnValue = true;
                    break;
                }
            }

            return returnValue;
        }




        public void GetSheetData(object selAssembly, ObservableCollection<EPWorkSheetModel> selExcelData)
        {
            if (selAssembly != null)
            {
                foreach (PropertyInfo eachPro in selAssembly.GetType().GetProperties())
                {
                    dynamic newModelObject = eachPro.GetValue(selAssembly, null);
                    ObservableCollection<object> newModelCollection = new ObservableCollection<object>(((ICollection)newModelObject).Cast<object>());

                    if (IsSingleExcelData(newModelCollection))
                    {
                        GetExcelSingleData(selExcelData, newModelCollection);
                    }
                    else
                    {
                        //if (eachPro.Name == "NozzleInputModel")
                        //{
                        //    double a = 1;
                        //}


                        GetExcelMultiData(selExcelData, newModelCollection);
                        IList cc = ((IList)newModelObject);
                        cc.Clear();
                        foreach (var eachVar in newModelCollection)
                            cc.Add(eachVar);

                        eachPro.SetValue(selAssembly, newModelObject, null);



                    }

                }
            }
        }



        private bool IsSingleExcelData(ObservableCollection<object> selModelCollection)
        {
            bool returnValue = false;
            foreach (object selModel in selModelCollection)
                foreach (PropertyInfo eachPro in selModel.GetType().GetProperties())
                {
                    var eachModelValue = eachPro.GetValue(selModel, null);
                    if (eachModelValue is string)
                    {
                        string newEachModelValue = eachModelValue as string;
                        if (newEachModelValue.ToLower().StartsWith("single"))
                        {
                            returnValue = true;
                            break;
                        }
                    }
                }
            return returnValue;
        }

        private void GetExcelSingleData(ObservableCollection<EPWorkSheetModel> selExcelData, ObservableCollection<object> selModelCollection)
        {
            foreach (object selModel in selModelCollection)
                foreach (PropertyInfo eachModelInfo in selModel.GetType().GetProperties())
                {
                    var eachModelValue = eachModelInfo.GetValue(selModel, null);
                    if (eachModelValue is string)
                    {

                        string newEachModelValue = eachModelValue as string;
                        string transValue = "";
                        if (newEachModelValue.ToLower().Replace(" ", "").Contains("|"))
                        {
                            string[] eachAddress = newEachModelValue.Split(new char[] { '|' });

                            // SheetName, Row, Col
                            if (eachAddress[2] != "" && eachAddress[3] != "" && eachAddress[4] != "")
                            {
                                foreach (EPWorkSheetModel eachSheet in selExcelData)
                                {
                                    if (eachSheet.ExcelSheetName == eachAddress[2])
                                    {
                                        if (eachSheet.DataArray != null)
                                        {
                                            int selRow = Convert.ToInt32(eachAddress[3]);
                                            int selCol = Convert.ToInt32(eachAddress[4]);
                                            selRow--;
                                            selCol--;
                                            if (eachSheet.RowCount > selRow && eachSheet.ColumnCount > selCol)
                                            {
                                                var tempTransValue = eachSheet.DataArray[selRow, selCol];
                                                if (tempTransValue != null)
                                                {
                                                    transValue = Convert.ToString(tempTransValue);
                                                }
                                                else
                                                {
                                                    transValue = "";
                                                }

                                                break;
                                            }

                                        }

                                    }
                                }
                            }
                        }
                        eachModelInfo.SetValue(selModel, transValue, null);
                    }
                }

        }

        private void GetExcelMultiData(ObservableCollection<EPWorkSheetModel> selExcelData, ObservableCollection<object> selModelCollection)
        {
            object refModel = null;
            object newModel = null;

            string SheetName = "";
            int RowCountCommnad = 0;
            // 모두가 같은 시트라는 전제
            List<Tuple<int, int>> rowcolList = new List<Tuple<int, int>>();
            foreach (object selModel in selModelCollection)
            {
                foreach (PropertyInfo eachModelInfo in selModel.GetType().GetProperties())
                {
                    var eachModelValue = eachModelInfo.GetValue(selModel, null);
                    if (eachModelValue is string)
                    {
                        string newEachModelValue = eachModelValue as string;
                        if (newEachModelValue.ToLower().Replace(" ", "").Contains("|"))
                        {
                            string[] eachAddress = newEachModelValue.Split(new char[] { '|' });

                            if (eachAddress[1] != "")
                                if (eachAddress[1] != "Auto")
                                    RowCountCommnad = Convert.ToInt32(eachAddress[1]);

                            // SheetName, Row, Col
                            if (eachAddress[2] != "" && eachAddress[3] != "" && eachAddress[4] != "")
                            {
                                SheetName = eachAddress[2];
                                int selRow = Convert.ToInt32(eachAddress[3]);
                                int selCol = Convert.ToInt32(eachAddress[4]);
                                selRow--;
                                selCol--;
                                rowcolList.Add(new Tuple<int, int>(selRow, selCol));
                            }
                        }
                        // Default Value
                        eachModelInfo.SetValue(selModel, "", null);
                    }
                    else
                    {
                        rowcolList.Add(new Tuple<int, int>(0, 0));
                    }
                }
                refModel = CloneService.CloneObject(selModel);
            }


            selModelCollection.Clear();


            foreach (EPWorkSheetModel eachSheet in selExcelData)
            {
                if (eachSheet.ExcelSheetName == SheetName)
                {
                    if (eachSheet.DataArray != null)
                    {
                        bool runSign = true;
                        bool addRow = false;
                        int rowCount = 0;

                        while (runSign)
                        {
                            addRow = true;
                            List<string> newRow = new List<string>();
                            foreach (Tuple<int, int> eachRow in rowcolList)
                            {
                                if (eachRow.Item1 == 0 && eachRow.Item2 == 0)
                                {
                                    //newRow.Add("!@#$%");
                                    newRow.Add("");
                                }
                                else
                                {
                                    // add Row
                                    int currentRow = eachRow.Item1 + rowCount;
                                    int currentCol = eachRow.Item2;
                                    string transValue = "";
                                    if (eachSheet.RowCount > currentRow && eachSheet.ColumnCount > currentCol)
                                    {
                                        var tempTransValue = eachSheet.DataArray[currentRow, currentCol];
                                        if (tempTransValue != null)
                                        {
                                            transValue = Convert.ToString(tempTransValue);
                                        }
                                    }
                                    newRow.Add(transValue);
                                }
                            }

                            if (string.Join("", newRow) == "")
                            {
                                runSign = false;
                                addRow = false;
                            }

                            // rowCount
                            rowCount++;



                            // RowCount Command
                            if (RowCountCommnad > 0)
                            {
                                addRow = true;
                                if (rowCount == RowCountCommnad)
                                    runSign = false;
                                else
                                    runSign = true;
                            }

                            // First : Default
                            if (rowCount == 0)
                                addRow = true;

                            if (addRow)
                            {
                                newModel = CloneService.CloneObject(refModel);
                                int i = 0;
                                foreach (PropertyInfo eachPro in newModel.GetType().GetProperties())
                                {
                                    //if(newRow[i]!= "!@#$%")
                                    //    eachPro.SetValue(newModel, newRow[i], null);
                                    if (eachPro.PropertyType.Name != "Double")
                                        eachPro.SetValue(newModel, newRow[i], null);
                                    i++;
                                    if (newRow.Count == i)
                                        break;
                                }
                                selModelCollection.Add(newModel);
                            }

                            // Exit Roof
                            if (rowCount >= eachSheet.RowCount)
                                runSign = false;


                        }

                    }

                    break;
                }
            }

        }





        // 저장
        public bool SetSaveData(string selFilePath)
        {
            bool returnValue = false;
            if (!File.Exists(selFilePath))
            {
                Console.WriteLine("File not found");
                return returnValue;
            }

            FileInfo existingFile = new FileInfo(selFilePath);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                Thread.Sleep(1000);
                foreach (ExcelWorksheet eachSheet in package.Workbook.Worksheets)
                {
                    if (eachSheet.Name == "DrawingList")
                    {
                        
                        eachSheet.Cells[9, 3].Value =  DateTime.Now.ToString("hh:mm:ss");
                        //FileInfo cc = new FileInfo(@"C:\Users\tree\Desktop\CAD\TABAS\20210719 GA보완\aa.xlsm");
                        //package.Workbook.FullCalcOnLoad = true;
//                        package.SaveAs(cc);
                        package.Save();
                        returnValue = true;
                        break;
                    }
                }
                package.Dispose();
            }
            return returnValue;
        }

        public string GetLoadData(string selFilePath)
        {
            string returnValue = "";
            if (!File.Exists(selFilePath))
            {
                Console.WriteLine("File not found");
                return returnValue;
            }

            FileInfo existingFile = new FileInfo(selFilePath);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                Thread.Sleep(1000);
                foreach (ExcelWorksheet eachSheet in package.Workbook.Worksheets)
                {
                    if (eachSheet.Name == "DrawingList")
                    {

                         // DateTime.Now.ToString("hh:mm:ss");
                        //FileInfo cc = new FileInfo(@"C:\Users\tree\Desktop\CAD\TABAS\20210719 GA보완\aa.xlsm");
                        //package.Workbook.FullCalcOnLoad = true;
                        //                        package.SaveAs(cc);
                        //package.Save();

                        returnValue = eachSheet.Cells[9, 3].Value.ToString();
                        break;
                    }
                }
                package.Dispose();
            }
            return returnValue;
        }

    }
}
