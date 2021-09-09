using DrawSettingLib.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingModels
{
    public class PaperAreaListModel
    {
        public PaperAreaListModel()
        {
            AreaList = new List<PaperAreaModel>();
        }

        public  List<PaperAreaModel> AreaList { get; set; }

        public PaperAreaModel GetAreaModel(PAPERMAIN_TYPE paperName, PAPERSUB_TYPE paperSubName )
        {
            PaperAreaModel returnModel = new PaperAreaModel();
            foreach(PaperAreaModel eachModel in AreaList)
            {
                if(eachModel.Name==paperName && eachModel.SubName == paperSubName)
                {
                    returnModel = eachModel;
                }
            }

            return returnModel;
        }
    }
}
