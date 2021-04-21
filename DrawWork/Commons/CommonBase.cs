﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.Commons
{
    public enum ENTITY_TYPE
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

    public enum POSITION_TYPE 
    {
        NotSet = 0,
        LEFT = 1,
        RIGHT = 2,
        BOTTOM = 3,
        TOP = 4,
    }

    public enum TANKASS_TYPE
    {
        NotSet = 0,
        SHELL = 1,
        ROOF = 2,

    }


}
