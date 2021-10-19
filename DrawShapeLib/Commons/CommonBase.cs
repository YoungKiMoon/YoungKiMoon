using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawShapeLib.Commons
{
    // Entity 
    public enum Temp_Enum
    {
        NotSet = 0,
        OUT_LINE = 1,
        CENTER_LINE = 2,
        DIM_LINE = 3,
        DIM_TEXT = 4,
        DIM_LINE_EXT = 5,
        LEADER_LINE = 6,
        LEADER_TEXT = 7,
        NOZZLE_LINE = 8,
        NOZZLE_MARK = 9,
    }

    public enum RECTANGLUNVIEW { NONE, LEFT, RIGHT, TOP, BOTTOM, }

    public enum VIEWPOSITION { NONE, LEFT, RIGHT, TOP, BOTTOM, TOP_BOTTOM, LEFT_RIGHT, }

    public enum SHAPEDIRECTION { NONE, LEFT, RIGHT, TOP, BOTTOM, LEFTTOP, LEFTBOTTOM, RIGHTTOP, RIGHTBOTTOM, }

    public enum CENTERRING_TYPE { CRT_INT, CRT_EXT, DRT_INT, DRT_EXT, }

}
