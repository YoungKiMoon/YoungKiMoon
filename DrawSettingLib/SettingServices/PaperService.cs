using DrawSettingLib.Commons;
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

        // Only : Column Type
        public List<PaperModel> GetPaperModel_StructureColumnType(double selColumnCount)
        {
            List<PaperModel> newList = new List<PaperModel>();


            if (selColumnCount == 1)
            {
                newList.AddRange(GetPaperModel_StructureColumn1());
            }
            else if (selColumnCount == 2)
            {
                newList.AddRange(GetPaperModel_StructureColumn2());
            }
            else if (selColumnCount == 3 && selColumnCount == 4)
            {
                newList.AddRange(GetPaperModel_StructureColumn34());
            }
            else if (selColumnCount == 5)
            {
                newList.AddRange(GetPaperModel_StructureColumn5());
            }
            else if (selColumnCount == 6)
            {
                newList.AddRange(GetPaperModel_StructureColumn6());
            }



            return newList;
        }



        private List<PaperModel> GetPaperModel_StructureColumn1()
        {
            List<PaperModel> newList = new List<PaperModel>();

            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 1, 1, 2));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 2, 4, 4));


            return newList;
        }
        private List<PaperModel> GetPaperModel_StructureColumn2()
        {
            List<PaperModel> newList = new List<PaperModel>();

            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 1, 1, 2));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 2, 4, 4));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 3, 4, 3));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 4, 4, 3));

            return newList;
        }
        private List<PaperModel> GetPaperModel_StructureColumn34()
        {
            List<PaperModel> newList = new List<PaperModel>();

            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 1, 1, 1));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 2, 1, 1));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 3, 4, 4));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 4, 4, 4));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 5, 4, 3));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 6, 4, 3));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 7, 4, 3));

            return newList;
        }
        private List<PaperModel> GetPaperModel_StructureColumn5()
        {
            List<PaperModel> newList = new List<PaperModel>();

            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 1, 1, 1));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 2, 1, 1));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 3, 4, 4));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 4, 4, 4));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 5, 4, 4));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 6, 4, 4));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 7, 4, 3));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 8, 4, 3));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 9, 4, 3));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 10, 4, 3));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 11, 4, 3));

            return newList;
        }
        private List<PaperModel> GetPaperModel_StructureColumn6()
        {
            List<PaperModel> newList = new List<PaperModel>();

            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 1, 1, 1));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 2, 1, 1));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 3, 4, 4));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 4, 4, 4));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 5, 4, 4));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 6, 4, 4));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 7, 4, 3));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 8, 4, 3));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 9, 4, 3));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 10, 4, 3));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 11, 4, 3));
            newList.Add(GetPaperModel(PAPERMAIN_TYPE.DetailOfRoofStructure, 12, 4, 3));


            return newList;
        }



        private PaperModel GetPaperModel(PAPERMAIN_TYPE dwgName, double page, double rowDef, double columnDef)
        {

            PaperModel newPaperMode = new PaperModel();
            newPaperMode.DWGName = dwgName;
            newPaperMode.Page = page;
            newPaperMode.RowDef = rowDef;
            newPaperMode.ColumnDef = columnDef;



            return newPaperMode;
        }

    }
}
