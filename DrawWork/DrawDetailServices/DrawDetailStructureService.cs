using AssemblyLib.AssemblyModels;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
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


        private ValueService valueService;
        private StyleFunctionService styleService;
        private LayerStyleService layerService;

        private DrawEditingService editingService;

        public DrawDetailStructureService(AssemblyModel selAssembly, object selModel)
        {
            singleModel = selModel as Model;
            assemblyData = selAssembly;


            valueService = new ValueService();
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();


            editingService = new DrawEditingService();
        }

        public DrawEntityModel DrawDetailRoofStructureAssembly(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {
            // List
            DrawEntityModel drawList = new DrawEntityModel();

            // Reference Point
            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            Point3D shellBottomPoint = GetSumPoint(referencePoint, 0, 0);

            List<Entity> shellList = new List<Entity>();
            List<Entity> bottomList = new List<Entity>();
            List<Entity> centerLineList = new List<Entity>();

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



            return newList;
        }


        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
    }
}
