using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using devDept.Eyeshot;
using devDept.Graphics;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Serialization;

using AssemblyLib.AssemblyModels;
using DrawWork.DrawModels;
using DrawWork.ValueServices;
using DrawWork.Commons;
using DrawWork.DrawCustomObjectModels;

namespace DrawWork.DrawServices
{
    public class DrawBoundaryService
    {
        private AssemblyModel assemblyData;

        private ValueService valueService;
        private DrawWorkingPointService workingPointService;

        public Dictionary<TANKBOUNDARY_TYPE, Line> BoundaryDic;


        private DrawPointModel _drawPoint;
        public DrawPointModel drawPoint
        {
            get { return _drawPoint; }
            set { _drawPoint = value; }
        }

        public DrawBoundaryService(AssemblyModel selAssembly)
        {
            assemblyData = selAssembly;

            valueService = new ValueService();
            workingPointService = new DrawWorkingPointService(selAssembly);

            BoundaryDic = new Dictionary<TANKBOUNDARY_TYPE, Line>();

            drawPoint = new DrawPointModel();
        }

        public void SetDrawPoint(DrawPointModel selDrawPoint)
        {
            drawPoint.referencePoint = selDrawPoint.referencePoint;
            drawPoint.currentPoint = selDrawPoint.currentPoint;
        }
        public void CreateBoundary(DrawPointModel selDrawPoint)
        {
            SetDrawPoint(selDrawPoint);
            CreateBoundaryEntity();
        }

        private void CreateBoundaryEntity()
        {
            BoundaryDic = new Dictionary<TANKBOUNDARY_TYPE, Line>();

            //
        }
    }
}
