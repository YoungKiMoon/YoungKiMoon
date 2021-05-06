using DrawWork.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DataServices
{
    public class ExcelDataService
    {
        public ExcelDataService()
        {

        }

        public void SampleSheet()
        {
            List<Tuple<int, int>> rangeList = new List<Tuple<int, int>>();

            rangeList.Add(new Tuple<int, int>(1,4) );
            List<List<string>> newData = GetSheetData("General", rangeList, EXCELDATAMODEL_TYPE.SingleRowData);
        }

        public List<List<string>> GetSheetData(string selSheetName, List<Tuple<int, int>> selDataList, EXCELDATAMODEL_TYPE selDataType)
        {
            List<List<string>> returnList = new List<List<string>>();

            if (selDataType == EXCELDATAMODEL_TYPE.SingleRowData)
            {

            }

            return returnList;
        }
    }
}
