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
        DetailOfRoofStructure
    }

    public enum PAPERSUB_TYPE
    {
        NotSet = 0,

        EmptyArea,

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



        //Strcuture Int
        RoofStructureOrientation,
        RoofStructureAssembly,
        RafterDetail,
        RafterSideClipDetail,
        CenterRingDetail,
        RafterCenterClipDetail,
        PurlinDetail,
        SectionAA,
        RibPlateDetail,

        //Structure Ext
        CenterRingRafterDetail,
        DetailB,
        SectionCC,
        ViewCC,
        RafterAndReinfPadCrossDetail,

        //Structure Column
        CenterColumnDetail,
        DetailF,
        DrainDetail,
        SectionBB,
        DetailC,

        SectionEE,
        DetailD,
        GirderBracketDetail,
        Rafter,
        InnerRafter,

        MidRafter,
        OutterRafter,
        G1Girder,
        DetailA,
        BracketDetail,

        Table1,
        Table2,
        Table3,
        Table4,
        Girder,




    }


}
