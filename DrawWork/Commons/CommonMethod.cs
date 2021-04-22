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

        public static WORKINGPOINT_TYPE WorkingPointToEnum(string selWorkingPoint)
        {
            WORKINGPOINT_TYPE returnValue = WORKINGPOINT_TYPE.NotSet;
            switch (selWorkingPoint)
            {
                // Point : Reference
                case "pointreference":
                    returnValue = WORKINGPOINT_TYPE.PointReference;
                    break;

                // Point : Center
                case "pointcentertopup":
                    returnValue = WORKINGPOINT_TYPE.PointCenterTopUp;
                    break;
                case "pointcentertopdown":
                    returnValue = WORKINGPOINT_TYPE.PointCenterTopDown;
                    break;

                case "pointcenterbottomup":
                    returnValue = WORKINGPOINT_TYPE.PointCenterBottomUp;
                    break;
                case "pointcenterbottomdown":
                    returnValue = WORKINGPOINT_TYPE.PointCenterBottomDown;
                    break;

                case "pointcentertop":
                    returnValue = WORKINGPOINT_TYPE.PointCenterTop;
                    break;
                case "pointcenterbottom":
                    returnValue = WORKINGPOINT_TYPE.PointCenterBottom;
                    break;

                // Point : Bottom
                case "pointleftbottomup":
                    returnValue = WORKINGPOINT_TYPE.PointLeftBottomUp;
                    break;
                case "pointleftbottomdown":
                    returnValue = WORKINGPOINT_TYPE.PointLeftBottomDown;
                    break;
                
                // Point : Roof
                case "pointleftroofup":
                    returnValue = WORKINGPOINT_TYPE.PointLeftRoofUp;
                    break;
                case "pointleftroofdown":
                    returnValue = WORKINGPOINT_TYPE.PointLeftRoofDown;
                    break;

                // Point : Shell
                case "pointleftshelltop":
                    returnValue = WORKINGPOINT_TYPE.PointLeftShellTop;
                    break;
                case "pointleftshelltopadj":
                    returnValue = WORKINGPOINT_TYPE.PointLeftShellTopAdj;
                    break;
                case "pointleftshellbottom":
                    returnValue = WORKINGPOINT_TYPE.PointLeftShellBottom;
                    break;

                case "pointrightshelltop":
                    returnValue = WORKINGPOINT_TYPE.PointRightShellTop;
                    break;
                case "pointrightshelltopadj":
                    returnValue = WORKINGPOINT_TYPE.PointRightShellTopAdj;
                    break;
                case "pointrightshellbottom":
                    returnValue = WORKINGPOINT_TYPE.PointRightShellBottom;
                    break;


                // Adj : Roof
                case "adjcenterroofup":
                    returnValue = WORKINGPOINT_TYPE.AdjCenterRoofUp;
                    break;
                case "adjcenterroofdown":
                    returnValue = WORKINGPOINT_TYPE.AdjCenterRoofDown;
                    break;
                case "adjleftroofup":
                    returnValue = WORKINGPOINT_TYPE.AdjLeftRoofUp;
                    break;
                case "adjleftroofdown":
                    returnValue = WORKINGPOINT_TYPE.AdjLeftRoofDown;
                    break;

                // Adj : Bottom
                case "adjcenterbottomup":
                    returnValue = WORKINGPOINT_TYPE.AdjCenterBottomUp;
                    break;
                case "adjcenterbottomdown":
                    returnValue = WORKINGPOINT_TYPE.AdjCenterBottomDown;
                    break;

                // Adj : Shell
                case "adjleftshell":
                    returnValue = WORKINGPOINT_TYPE.AdjLeftShell;
                    break;
                case "adjrightshell":
                    returnValue = WORKINGPOINT_TYPE.AdjRightShell;
                    break;

            }
            return returnValue;
        }
    }

    
}
