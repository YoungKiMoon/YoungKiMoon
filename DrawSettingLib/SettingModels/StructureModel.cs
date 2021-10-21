using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingModels
{
    public class StructureModel
    {
        public StructureModel()
        {

            Radius = 0;
            LayerList = new List<StructureLayerModel>();
        }

        public double Radius { get; set; }

        public List<StructureLayerModel> LayerList { get; set; }
    }
}
