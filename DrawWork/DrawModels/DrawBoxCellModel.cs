using DrawWork.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public  class DrawBoxCellModel
    {

        public DrawBoxCellModel()
        {
            Status = BoxCell_Type.Empty;
            XValue = 0;
            YValue = 0;
        }

        public BoxCell_Type Status { get; set; }
        
        public double XValue { get; set; }
        public double YValue { get; set; }

    }
}
