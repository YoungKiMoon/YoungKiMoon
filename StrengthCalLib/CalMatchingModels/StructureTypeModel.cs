using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrengthCalLib.CalMatchingModels
{
    public class StructureTypeModel
    {
        public string AME { get; set; }
        public string TABAS { get; set; }
        public string RoofType { get; set; }
        public string Position { get; set; }
        public StructureTypeModel()
        {
            AME = "";
            TABAS = "";
            RoofType = "";
            Position = "";
        }
    }
}
