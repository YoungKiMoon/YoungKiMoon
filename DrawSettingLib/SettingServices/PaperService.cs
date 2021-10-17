using DrawSettingLib.SettingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingServices
{
    public class PaperService
    {
        public PaperService()
        {

        }


        public List<PaperModel> GetPaperModel_StructureTyep1(string selTest)
        {
            List<PaperModel> newList = new List<PaperModel>();


            if (selTest == "1")
            {
                newList.AddRange(GetPaperModel_StructureTyep1())
            }
            else if (selTest == "2")
            {

            }



            return newList;
        }

        public List<PaperModel> GetPaperModel_StructureTyep1()
        {
            List<PaperModel> newList = new List<PaperModel>();

            PaperModel newPaperMode = new PaperModel();
            newPaperMode.Page = 1;
            newPaperMode.RowDef = 12;
            newPaperMode.ColumnDef = 12;


            newList.Add(newPaperMode);


            return newList;
        }
    }
}
