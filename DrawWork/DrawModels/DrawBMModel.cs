using DrawSettingLib.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public class DrawBMModel
    {


        public DrawBMModel()
        {
            DWGName = PAPERMAIN_TYPE.NotSet;
            Page = 1;

            No = "";
            Name = "";
            Material = "";
            Dimension = "";
            Set = "";
            Weight = "";
            Remark = "";

            ExportData = new DrawBMExportModel();
        }

        public PAPERMAIN_TYPE DWGName { get; set; }

        public double Page { get; set; }

        public string No { get; set; }
        public string Name { get; set; }
        public string Material { get; set; }
        public string Dimension { get; set; }

        public string Set { get; set; }
        public string Weight { get; set; }
        public string Remark { get; set; }

        public DrawBMExportModel ExportData { get; set; }


    }
}
