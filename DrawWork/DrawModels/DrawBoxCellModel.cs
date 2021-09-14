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

            Status = BoxCell_Type.Block;
            LeftLine = false;
            RightLine = false;
            BottomLine = false;
            TopLine = false;
        }

        public BoxCell_Type Status { get; set; }

        public bool LeftLine { get; set; }
        public bool RightLine { get; set; }
        public bool BottomLine { get; set; }
        public bool TopLine { get; set; }
    }
}
