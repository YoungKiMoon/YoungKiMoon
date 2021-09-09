using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.Commons
{
    public enum PAPERMAIN_TYPE
    {
        NotSet = 0,
        GA1,
        GA2,
        ORIENTATION,
        ShellPlateArrangement,
        CourseShellPlate,
        BottomPlateArrangement,
        BottomPlateCuttingPlan,
        RoofPlateArrangement,
        RoofPlateCuttingPlan,
        DETAIL,
    }

    public enum PAPERSUB_TYPE
    {
        NotSet = 0,

        HORIZONTALJOINT,

        ComRing,
        TopRingCuttingPlan,
        ComRingCuttingPlan,


        AnchorChair,
        AnchorDetail,

        TopAngleJoint,
        WindGirderJoint,

        SectionDD,

        VertJointDetail,

        DimensionsForCutting,
        ToleranceLimit,
        ShellPlateChordLength,


        ONECOURSESHELLPLATE,
        BOTTOMPLATEJOINT,

        ShellPlateArrangement,

        RoofArrange,

        NamePlateBracket,
        EarthLug,
        SettlementCheckPiece,

        // Bottom
        BottomPlateArrangement,
        BottomPlateJointAnnularDetail,
        BottomPlateJointDetail,
        BottomPlateWeldingDetailC,
        BottomPlateWeldingDetailD,
        BottomPlateWeldingDetailBB,
        BackingStripWeldingDetail,
        BottomPlateShellJointDetail,

        BottomPlateCuttingPlan,
        AnnularPlateCuttingPlan,
        BackingStrip,

        // Roof
        RoofPlateArrangement,
        RoofCompressionRingJointDetail,
        RoofCompressionWeldingDetail,
        RoofPlateWeldingDetailD,
        RoofPlateWeldingDetailC,
        RoofPlateWeldingDetailDD,

        RoofPlateCuttingPlan,
        RoofCompressionRingCuttingPlan,




    }


}
