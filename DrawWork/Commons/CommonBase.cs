﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.Commons
{
    // Entity 
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

    // Postion
    public enum POSITION_TYPE 
    {
        NotSet = 0,
        LEFT = 1,
        RIGHT = 2,
        BOTTOM = 3,
        TOP = 4,
    }

    // Tank Assembly
    public enum TANKASS_TYPE
    {
        NotSet = 0,
        SHELL = 1,
        ROOF = 2,

    }

    // Tnak Boundary
    public enum TANKBOUNDARY_TYPE
    {
        NotSet = 0,

        Center = 1,

        LeftShell = 2,
        RightShell = 3,

        TopShell = 4,
        BottomShell = 5,

        LeftRoofUp = 6,
        LeftRoofDown = 7,
        RightRoofUp = 8,
        RightRoofDown = 9,

        LeftBottomUp = 10,
        LeftBottomDown = 11,
        RighBottomUp= 12,
        RighBottomDown = 13,
    }



    // Line Type
    public enum LINE_TYPE
    {
        NotSet = 0,
        OutLine = 1,
        CenterLine = 2,

        DimLine = 3,
        DimText = 4,
        DimLineText = 5,
        DimArrow = 6,

        LeaderLine= 7,
        LeaderText = 8,
        LeaderArrow = 9,

        NozzleLine = 10,
        NozzleMark = 11,
        NozzleText = 12,
    }



    // Working Point
    public enum WORKINGPOINT_TYPE 
    {
        NotSet = 0,

        // Point : Reference
        PointReference =100,

        // Point : Center
        PointCenterTopUp = 1,
        PointCenterTopDown = 2,

        PointCenterBottomUp = 3,
        PointCenterBottomDown = 4,

        PointCenterTop = 5,
        PointCenterBottom = 6,

        // Point : Botom
        PointLeftBottomUp = 7,
        PointLeftBottomDown = 8,

        // Point : Roof
        PointLeftRoofUp = 9,
        PointLeftRoofDown = 10,

        // Point : Shell
        PointLeftShellTop = 11,
        PointLeftShellTopAdj = 12,
        PointLeftShellBottom = 13,

        PointRightShellTop = 14,
        PointRightShellTopAdj = 15,
        PointRightShellBottom = 16,

        // Adj : Roof
        AdjCenterRoofUp = 17,
        AdjCenterRoofDown = 18,
        AdjLeftRoofUp = 19,
        AdjLeftRoofDown = 20,

        // Adj : Bottom
        AdjCenterBottomUp = 21,
        AdjCenterBottomDown = 22,

        // Adj : Shell
        AdjLeftShell = 23,
        AdjRightShell = 24,
    }


}
