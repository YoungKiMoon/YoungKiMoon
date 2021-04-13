using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.Commons
{
    public static class CommonMethod
    {
        public static POSITION_TYPE PositionToEnum(string selPosition)
        {
            POSITION_TYPE retrunValue = POSITION_TYPE.LEFT;
            switch (selPosition)
            {
                case "left":
                    retrunValue = POSITION_TYPE.LEFT;
                    break;
                case "right":
                    retrunValue = POSITION_TYPE.RIGHT;
                    break;
                case "bottom":
                    retrunValue = POSITION_TYPE.BOTTOM;
                    break;
                case "top":
                    retrunValue = POSITION_TYPE.TOP;
                    break;
            }
            return retrunValue;
        }
    }
}
