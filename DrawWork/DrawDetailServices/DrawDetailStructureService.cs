using AssemblyLib.AssemblyModels;
using devDept.Eyeshot;
using devDept.Geometry;
using DrawWork.DrawModels;
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
        public DrawDetailStructureService(AssemblyModel selAssembly, object selModel)
        {
            singleModel = selModel as Model;
            assemblyData = selAssembly;
        }

        public DrawEntityModel GetShellHorizontalJoint(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {
            // List
            DrawEntityModel drawList = new DrawEntityModel();

            // Reference Point
            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);



            return drawList;
        }
    }
}
