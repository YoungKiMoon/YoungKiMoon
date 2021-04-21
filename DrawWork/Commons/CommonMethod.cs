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
            POSITION_TYPE returnValue = POSITION_TYPE.LEFT;
            switch (selPosition)
            {
                case "left":
                    returnValue = POSITION_TYPE.LEFT;
                    break;
                case "right":
                    returnValue = POSITION_TYPE.RIGHT;
                    break;
                case "bottom":
                    returnValue = POSITION_TYPE.BOTTOM;
                    break;
                case "top":
                    returnValue = POSITION_TYPE.TOP;
                    break;
            }
            return returnValue;
        }

        public static TANKASS_TYPE TankAssToEnum(string selTankAss)
        {
            TANKASS_TYPE returnValue = TANKASS_TYPE.NotSet;
            switch (selTankAss)
            {
                case "shell":
                    returnValue = TANKASS_TYPE.SHELL;
                    break;
                case "roof":
                    returnValue = TANKASS_TYPE.ROOF;
                    break;
            }
            return returnValue;
        }
    }

    
}
