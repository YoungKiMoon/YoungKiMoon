using StrengthCalLib.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrengthCalLib.CalMatchingModels
{
    public class RoofTypeModel
    {

        public string Roof { get; set; }
        public string TABAS { get; set; }
        public string AMEtxt01 { get; set; }
        public string AMEtxt02 { get; set; }

        public SearchDirection_Type fromEnd { get; set; }
        public RoofTypeModel()
        {
            Roof = "";
            TABAS = "";
            AMEtxt01 = "";
            AMEtxt02 = "";
            fromEnd = SearchDirection_Type.FRONT;
        }
    }
}
