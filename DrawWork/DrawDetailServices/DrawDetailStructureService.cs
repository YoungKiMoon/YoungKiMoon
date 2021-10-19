using AssemblyLib.AssemblyModels;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawShapeLib.DrawServices;
using DrawWork.DrawCommonServices;
using DrawWork.DrawModels;
using DrawWork.DrawServices;
using DrawWork.DrawStyleServices;
using DrawWork.ValueServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawDetailServices
{
    public class DrawDetailStructureService
    {

        private Model singleModel;
        private AssemblyModel assemblyData;

        private DrawPublicService drawCommon;

        private ValueService valueService;
        private StyleFunctionService styleService;
        private LayerStyleService layerService;

        private DrawEditingService editingService;
        private DrawShapeServices shapeService;


        private DrawCommonService drawComService;

        private DrawDetailStructureShareService drawShareService;

        public DrawDetailStructureService(AssemblyModel selAssembly, object selModel)
        {
            singleModel = selModel as Model;
            assemblyData = selAssembly;

            drawCommon = new DrawPublicService(selAssembly);

            valueService = new ValueService();
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();


            editingService = new DrawEditingService();
            shapeService = new DrawShapeServices();

            drawComService = new DrawCommonService();
            drawShareService = new DrawDetailStructureShareService();
        }

        public DrawEntityModel DrawDetailRoofStructureAssembly_Side(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {
            // List
            DrawEntityModel drawList = new DrawEntityModel();

            // Reference Point
            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            Point3D shellBottomPoint = GetSumPoint(referencePoint, 0, 0);

            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);
            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankIDHalf = tankID / 2;
            double roofSlope= valueService.GetDegreeOfSlope(assemblyData.RoofCompressionRing[0].RoofSlope);
            double bottomSlope = valueService.GetDegreeOfSlope(assemblyData.BottomInput[0].BottomPlateSlope);

            List<Entity> shellList = new List<Entity>();
            List<Entity> bottomList = new List<Entity>();
            List<Entity> centerLineList = new List<Entity>();


            List<Entity> etcList = new List<Entity>();

            double shellHeight = 0;
            double bottomCenterOpposite = 0;
            double roofCenterOpposite = 0;

            double shellHeightPercentage = 100;

            double shellSideHeight = 0;
            double centerSideHeight = 0;

            Point3D roofCenterLowerPoint = GetSumPoint(shellBottomPoint, 0, 0);
            Point3D roofCenterUpperPoint = GetSumPoint(roofCenterLowerPoint, 0, 0);
            Point3D roofSideLowerPoint= GetSumPoint(shellBottomPoint, 0, 0);
            Point3D roofSideUpperPoint= GetSumPoint(shellBottomPoint, 0, 0);


            Point3D bottomCenterLowerPoint = GetSumPoint(shellBottomPoint, 0, 0);
            Point3D bottomCenterUpperPoint = GetSumPoint(bottomCenterLowerPoint, 0, 0);
            Point3D bottomSideLowerPoint= GetSumPoint(shellBottomPoint, 0, 0);
            Point3D bottomSideUpperPoint = GetSumPoint(shellBottomPoint, 0, 0);

            // CenterLine
            double exLength = 6;
            centerLineList.AddRange( editingService.GetCenterLine(GetSumPoint(roofCenterUpperPoint, 0,0), GetSumPoint(bottomCenterLowerPoint, 0, 0), exLength,scaleValue));

            // Shell
            shellList.AddRange(GetShellAssembly(GetSumPoint(referencePoint,0,0))) ;
            // Bottom : Annular 적용
            bottomList.AddRange(GetBottomAssembly(GetSumPoint(referencePoint, 0, 0)));
            // Roof

            // 
            // 1
            etcList.AddRange(drawComService.GetColumnCenterTopSupport_TopView(GetSumPoint(referencePoint, 0, 0), scaleValue));
            // 2
            etcList.AddRange(drawComService.GetColumnCenterTopSupport(GetSumPoint(referencePoint, 1000, 0), scaleValue));
            // 3



            etcList.AddRange(drawShareService.DrawDetailRafterSideClipDetail(GetSumPoint(referencePoint, 0, tankHeight), 1));

            etcList.AddRange(drawShareService.DrawDetailRafter(GetSumPoint(referencePoint, 0, tankHeight), scaleValue, tankIDHalf, roofSlope));




            drawList.outlineList.AddRange(etcList);

            styleService.SetLayerListEntity(ref shellList, layerService.LayerVirtualLine);
            drawList.outlineList.AddRange(shellList);

            styleService.SetLayerListEntity(ref bottomList, layerService.LayerVirtualLine);
            drawList.outlineList.AddRange(bottomList);

            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            drawList.outlineList.AddRange(centerLineList);

            return drawList;
        }

        public List<Entity> GetBottomAssembly(Point3D refPoint)
        {
            List<Entity> newList = new List<Entity>();
            List<Entity> bottomPlateLeft = new List<Entity>();
            List<Entity> annularPlateLeft= new List<Entity>();


            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);
            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankIDHalf = tankID / 2;
            Point3D tankCenterPoint = GetSumPoint(refPoint, tankIDHalf, 0);




            double bottomSlope = valueService.GetDegreeOfSlope(assemblyData.BottomInput[0].BottomPlateSlope);
            double bottomOD = valueService.GetDoubleValue(assemblyData.BottomInput[0].OD);
            double bottomODHalf = bottomOD / 2;
            double bottomThickness = valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomPlateThickness);
            double bottomThicknessHyp = valueService.GetHypotenuseByWidth(bottomSlope, bottomThickness);

            double bottomLeftAdj = 0;
            double bottomCenterOpposite = 0;
            Point3D bottomLeftStartPoint = new Point3D();
            Point3D bottomCenterPoint = new Point3D();
            Point3D bottomCenterUpperPoint = new Point3D();


            List<Point3D> outPointList = new List<Point3D>();
            if (drawCommon.isAnnular())
            {
                double annularOD = valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateOD);
                double annularID = valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateID);
                double annularThickness = valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateThickness);
                double annularWidth = valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateWidthWidth);
                double annularOutsideProjection = (annularOD - tankID)/2;

                Point3D annularPoint = GetSumPoint(referencePoint, -annularOutsideProjection, 0);
                annularPlateLeft.AddRange(shapeService.GetRectangle(out outPointList, GetSumPoint(annularPoint, 0, 0), annularWidth, annularThickness, 0, 0, 0));


                bottomLeftAdj = valueService.GetAdjacentByHypotenuse(bottomSlope, bottomODHalf);
                bottomCenterOpposite = valueService.GetOppositeByHypotenuse(bottomSlope, bottomODHalf);
                bottomLeftStartPoint = GetSumPoint(tankCenterPoint, -bottomLeftAdj, 0);

                bottomCenterPoint = GetSumPoint(tankCenterPoint, 0, bottomCenterOpposite);
            }
            else
            {

                double bottomCenterOppOfTankID = valueService.GetOppositeByAdjacent(bottomSlope, tankIDHalf);
                bottomCenterPoint = GetSumPoint(tankCenterPoint, 0, bottomCenterOppOfTankID);

                bottomLeftAdj = valueService.GetAdjacentByHypotenuse(bottomSlope, bottomODHalf);
                bottomCenterOpposite = valueService.GetOppositeByHypotenuse(bottomSlope, bottomODHalf);
                bottomLeftStartPoint = GetSumPoint(bottomCenterPoint, -bottomLeftAdj, -bottomCenterOpposite);


            }

            bottomCenterUpperPoint = GetSumPoint(bottomCenterPoint, 0, bottomThicknessHyp);

            Line plateLowerLine = new Line(GetSumPoint(bottomLeftStartPoint, 0, 0), GetSumPoint(bottomCenterPoint, 0, 0));
            Line plateLeftLine = new Line(GetSumPoint(bottomLeftStartPoint, 0, 0), GetSumPoint(bottomLeftStartPoint, 0, bottomThickness));
            plateLeftLine.Rotate(bottomSlope, Vector3D.AxisZ, GetSumPoint(bottomLeftStartPoint, 0, 0));
            Line plateUpperLine = new Line(GetSumPoint(plateLeftLine.EndPoint, 0, 0), GetSumPoint(bottomCenterUpperPoint, 0, 0));


            bottomPlateLeft.Add(plateLowerLine);
            bottomPlateLeft.Add(plateUpperLine);
            bottomPlateLeft.Add(plateLeftLine);

            newList.AddRange(annularPlateLeft);
            newList.AddRange(bottomPlateLeft);
            newList.AddRange(editingService.GetMirrorEntity(Plane.YZ, annularPlateLeft, GetSumPoint(tankCenterPoint, 0, 0), true));
            newList.AddRange(editingService.GetMirrorEntity(Plane.YZ, bottomPlateLeft, GetSumPoint(tankCenterPoint, 0, 0), true));

            return newList;
        }

        public List<Entity> GetShellAssembly(Point3D refPoint)
        {
            List<Entity> newList = new List<Entity>();
            List<Entity> leftList = new List<Entity>();

            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);
            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);
            double tankID= valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankIDHalf = tankID / 2;

            Point3D mirrorPoint = GetSumPoint(referencePoint, tankIDHalf, 0);

            // 좌측 하단이 기준


            List<double> shellWidth=drawCommon.GetShellCourseWidthForDrawing();
            List<double> shellThickness = drawCommon.GetShellCourseThickneeForDrawing();

            List<Point3D> outPointList = new List<Point3D>();
            Point3D shellCurrentPoint = GetSumPoint(referencePoint, 0, 0);
            for(int i=0;i<shellWidth.Count;i++)
            {
                double eachWidth = shellWidth[i];
                double eachThickness = shellThickness[i];
                leftList.AddRange(shapeService.GetRectangle(out outPointList, GetSumPoint(shellCurrentPoint, 0, 0), eachThickness, eachWidth, 0, 0, 2));
                shellCurrentPoint = GetSumPoint(outPointList[1],0,0);
            }

            Line leftID = new Line(GetSumPoint(referencePoint, 0, 0), GetSumPoint(refPoint, 0, tankHeight));


            leftList.Add(leftID);


            newList.AddRange(leftList);
            newList.AddRange(editingService.GetMirrorEntity(Plane.YZ, leftList, GetSumPoint(mirrorPoint, 0, 0), true));

            return newList;
        }


        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }






    }
}
