using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingModels
{
    public class StructureLayerModel
    {
        public StructureLayerModel()
        {
            Number = 0;
            Radius = 0;
            StartAngle = 0;

            ColumnList = new List<StructureColumnModel>();
            GirderList = new List<StructureGirderModel>();
            RafterList = new List<StructureRafterModel>();
        }

        public double Number { get; set; }

        public double Radius { get; set; }
        public double StartAngle { get; set; }

        public List<StructureColumnModel> ColumnList { get; set; }

        public List<StructureGirderModel> GirderList { get; set; }

        public List<StructureRafterModel> RafterList { get; set; }
    }
}
