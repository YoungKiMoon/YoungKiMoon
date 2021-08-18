using devDept.Geometry;
using DrawWork.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public class DrawPlateModel
    {
        public Point3D CenterPoint { get; set; }
        public double Radius { get; set; }


        public double Number { get; set; }


        public List<Point3D> Points { get; set; }
        public Plate_Type ShapeType { get; set; }
        public bool IsAdjustment { get; set; }
        public PERPENDICULAR_TYPE PlateDirection { get; set; }
        public  ORIENTATION_TYPE ArcDirection { get; set; }

        public double RectWidth { get; set; }
        public double RectLength { get; set; }

        public DrawPlateModel()
        {
            // 중심 점
            CenterPoint = null;
            Radius = 0;

            // Default
            Number = 0;


            // 0 : 직선, 1 : 직선/Arc, 2 : 직선/ARC, 3 : 직선
            Points = new List<Point3D>();
            // 사각형, 사각형 호, 호 
            ShapeType = Plate_Type.Rectangle;
            // 사각형 변형 유무
            IsAdjustment = false;
            // Plate 방향
            PlateDirection = PERPENDICULAR_TYPE.Horizontal;
            // Arc 방향
            ArcDirection = ORIENTATION_TYPE.NotSet;

            // Plate Size
            RectWidth = 0;
            RectLength = 0;


        }
    }
}
