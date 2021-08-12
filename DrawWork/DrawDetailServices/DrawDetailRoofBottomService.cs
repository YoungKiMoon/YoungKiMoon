using AssemblyLib.AssemblyModels;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawWork.Commons;
using DrawWork.DrawModels;
using DrawWork.DrawSacleServices;
using DrawWork.DrawServices;
using DrawWork.DrawShapes;
using DrawWork.DrawStyleServices;
using DrawWork.ValueServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawDetailServices
{
    public class DrawDetailRoofBottomService
    {
        private Model singleModel;
        private AssemblyModel assemblyData;

        private DrawService drawService;


        private ValueService valueService;
        private StyleFunctionService styleService;
        private LayerStyleService layerService;

        private DrawShellCourses dsService;
        private DrawBreakSymbols shapeService;

        public DrawDetailRoofBottomService(AssemblyModel selAssembly, object selModel)
        {
            singleModel = selModel as Model;

            assemblyData = selAssembly;

            drawService = new DrawService(selAssembly);

            valueService = new ValueService();
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();

            dsService = new DrawShellCourses();
            shapeService = new DrawBreakSymbols();
        }


        public List<Entity> GetBottomPlateJoint(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue, out Dictionary<string, List<Entity>> selDim)
        {
            List<Entity> newList = new List<Entity>();
            selDim = new Dictionary<string, List<Entity>>();




            return newList;
        }



        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }

    }
}
