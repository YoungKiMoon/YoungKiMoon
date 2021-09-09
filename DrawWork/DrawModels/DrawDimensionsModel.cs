using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public class DrawDimensionsModel
    {
        public DrawDimensionsModel()
        {
            No = "";
            L = "";
            W = "";
            X = "";
            Thk = "";

            Qty = "";
            PartNo = "";
        }

        public string No { get; set; }
        public string L { get; set; }
        public string W { get; set; }
        public string X { get; set; }
        public string Thk { get; set; }
        public string Qty { get; set; }
        public string PartNo { get; set; }
    }
}
