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
using DrawWork.ValueServices;
using DrawWork.Commons;
using DrawWork.DrawShapes;

namespace DrawWork.DrawServices
{
    public class DrawShapeServices
    {
        private ValueService valueService;
        private DrawEditingService editingService;

        public DrawBreakSymbols breakSymbols { get; set; }
        public DrawShellCourses shellCourses { get; set; }
        public DrawShapeServices()
        {
            valueService = new ValueService();
            editingService = new DrawEditingService();

            breakSymbols = new DrawBreakSymbols();
            shellCourses = new DrawShellCourses();
        }

        public List<Entity> GetRectangle(out List<Point3D> selOutputPointList,Point3D selPoint, double selWidth,double selHeight,double selRotate, double selRotateCenter, double selTranslateNumber,bool[] selVisibleLine=null)
        {
            selOutputPointList = new List<Point3D>();
            List<Entity> newList = new List<Entity>();

            // Reference Point : Top Left
            Point3D A = GetSumPoint(selPoint, 0, 0);
            Point3D B = GetSumPoint(selPoint, selWidth, 0);
            Point3D C = GetSumPoint(selPoint, selWidth, -selHeight);
            Point3D D = GetSumPoint(selPoint, 0, -selHeight);

            Line lineA = new Line(GetSumPoint(A,0,0), GetSumPoint(B,0,0));
            Line lineB = new Line(GetSumPoint(B,0,0), GetSumPoint(C,0,0));
            Line lineC = new Line(GetSumPoint(C,0,0),GetSumPoint( D,0,0));
            Line lineD = new Line(GetSumPoint(D, 0, 0), GetSumPoint(A, 0, 0));

            newList.AddRange(new Line[] { lineA, lineB, lineC, lineD });
            if (selRotate != 0)
            {
                Point3D WPRotate = GetSumPoint(A, 0, 0);
                if (selRotateCenter == 1)
                    WPRotate = GetSumPoint(B, 0, 0);
                else if (selRotateCenter == 2)
                    WPRotate = GetSumPoint(C, 0, 0);
                else if (selRotateCenter == 3)
                    WPRotate = GetSumPoint(D, 0, 0);

                foreach (Entity eachEntity in newList)
                    eachEntity.Rotate(selRotate, Vector3D.AxisZ, WPRotate);
            }
            if (selTranslateNumber > 0)
            {
                Point3D WPTranslate = new Point3D();
                if (selTranslateNumber == 1)
                    WPTranslate = GetSumPoint(B, 0, 0);
                else if (selTranslateNumber == 2)
                    WPTranslate = GetSumPoint(C, 0, 0);
                else if (selTranslateNumber == 3)
                    WPTranslate = GetSumPoint(D, 0, 0);
                editingService.SetTranslate(ref newList, GetSumPoint(selPoint, 0, 0), WPTranslate);

            }
            if (selVisibleLine != null)
            {
                if (selVisibleLine[0] == false)
                    newList.Remove(lineA);
                if (selVisibleLine[1] == false)
                    newList.Remove(lineB);
                if (selVisibleLine[2] == false)
                    newList.Remove(lineC);
                if (selVisibleLine[3] == false)
                    newList.Remove(lineD);
            }

            return newList;
        }


        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
    }
}
