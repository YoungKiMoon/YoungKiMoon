using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ExcelDataLib.ExcelModels;
using ExcelDataLib.Utils;

using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelDataLib.ExcelServices
{
    public class ExcelApplicationService : Notifier
    {
        public ExcelApplicationService()
        {

        }

        public ObservableCollection<ExcelWorkSheetModel> GetSheetList(Excel.Workbook selWork)
        {
            ObservableCollection<ExcelWorkSheetModel> newList = new ObservableCollection<ExcelWorkSheetModel>();

            foreach (Excel.Worksheet eachSheet in selWork.Worksheets)
            {
                ExcelWorkSheetModel newModel = new ExcelWorkSheetModel();

                newModel.ExcelSheet = eachSheet;
                newModel.ExcelSheetName = eachSheet.Name;

                newList.Add(newModel);
            }

            return newList;

        }

        public List<List<string>> GetSheetData(string selSheetName, List<Tuple<int, int>> selDataList, bool isSingle)
        {
            List<List<string>> returnList = new List<List<string>>();

            if (isSingle)
            {

            }
            else
            {

            }

            return returnList;
        }

    }
}
