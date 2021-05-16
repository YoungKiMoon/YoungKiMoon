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
using MColor = System.Windows.Media.Color;

//using Color = System.Drawing.Color;

using AssemblyLib.AssemblyModels;

using DrawWork.ValueServices;
using DrawWork.DrawModels;

namespace DrawWork.DrawServices
{
    public class DrawReferenceBlockService
    {
        private AssemblyModel assemblyData;

        private ValueService valueService;
        private DrawWorkingPointService contactService;

        public DrawReferenceBlockService(AssemblyModel selAssembly)
        {
            assemblyData = selAssembly;
            valueService = new ValueService();
            contactService = new DrawWorkingPointService(selAssembly);
        }

        #region Angle
        public AngleSizeModel GetAngleSizeModel(string selAngleSize)
        {
            AngleSizeModel returnValue = null;
            string newAngleSize = valueService.GetAllTrim(selAngleSize);
            foreach (AngleSizeModel eachAngle in assemblyData.AngleIList)
            {
                if (valueService.GetAllTrim(eachAngle.SIZE) == newAngleSize)
                {
                    returnValue=eachAngle;
                    break;
                }
            }
            return returnValue;
        }
        public Entity[] DrawReference_Angle(CDPoint selPoint1, string selAngle)
        {
            AngleSizeModel newAngleSize = GetAngleSizeModel(selAngle);
            return DrawReference_Angle(selPoint1, newAngleSize);
        }
        public Entity[] DrawReference_Angle(CDPoint selPoint1, AngleSizeModel selAngle)
        {


            if (selAngle == null)
                return null;



            double AB = valueService.GetDoubleValue(selAngle.AB);
            double t = valueService.GetDoubleValue(selAngle.t);
            double R1 = valueService.GetDoubleValue(selAngle.R1);
            double R2 = valueService.GetDoubleValue(selAngle.R2);
            double CD = valueService.GetDoubleValue(selAngle.CD);
            double E = valueService.GetDoubleValue(selAngle.E);

            // 좌측 상단을 기준으로 그림
            Point3D drawPoint = new Point3D(selPoint1.X, selPoint1.Y, selPoint1.Z);

            Line lineA = new Line(GetSumPoint(drawPoint, 0, AB), GetSumPoint(drawPoint, AB, AB));
            Line lineAa = new Line(GetSumPoint(drawPoint, 0, AB - t), GetSumPoint(drawPoint, AB - t, AB - t));
            Line lineAt = new Line(GetSumPoint(drawPoint, 0, AB), GetSumPoint(drawPoint, 0, AB - t));
            Line lineB = new Line(GetSumPoint(drawPoint, AB, AB), GetSumPoint(drawPoint, AB, 0));
            Line lineBb = new Line(GetSumPoint(drawPoint, AB - t, AB - t), GetSumPoint(drawPoint, AB - t, 0));
            Line lineBt = new Line(GetSumPoint(drawPoint, AB - t, 0), GetSumPoint(drawPoint, AB, 0));


            List<Entity> customBlockList = new List<Entity>();

            Arc arcFillet1;
            if (Curve.Fillet(lineAt, lineAa, R2, false, false, true, true, out arcFillet1))
                customBlockList.Add(arcFillet1);
            Arc arcFillet2;
            if (Curve.Fillet(lineBb, lineBt, R2, false, false, true, true, out arcFillet2))
                customBlockList.Add(arcFillet2);
            Arc arcFillet3;
            if (Curve.Fillet(lineAa, lineBb, R1, false, false, true, true, out arcFillet3))
                customBlockList.Add(arcFillet3);

            
            customBlockList.Add(lineA);
            customBlockList.Add(lineAa);
            customBlockList.Add(lineAt);
            customBlockList.Add(lineB);
            customBlockList.Add(lineBb);
            customBlockList.Add(lineBt);

            return customBlockList.ToArray();
        }

        public Entity[] DrawReference_CompressionRingI(CDPoint selPoint1)
        {
            int firstIndex = 0;
            double A = valueService.GetDoubleValue(assemblyData.RoofInput[firstIndex].ComRingOutsideProjectionA);
            double B = valueService.GetDoubleValue(assemblyData.RoofInput[firstIndex].ComRingWidthB);
            double C = valueService.GetDoubleValue(assemblyData.RoofInput[firstIndex].ComRingOverlapC);
            double t1 = valueService.GetDoubleValue(assemblyData.RoofInput[firstIndex].ComRingThicknessT1);


            // Roof Slope
            string roofSlopeString = assemblyData.RoofInput[firstIndex].RoofSlopeOne;
            double roofSlopeDegree = valueService.GetDegreeOfSlope(roofSlopeString);

            double outsideBottomX = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, A);
            double outsideBottomY = valueService.GetOppositeByHypotenuse(roofSlopeDegree, A);
            double insideBottomX= valueService.GetAdjacentByHypotenuse(roofSlopeDegree, B);
            double insideBottomY = valueService.GetOppositeByHypotenuse(roofSlopeDegree, B);

            double thicknessY= valueService.GetAdjacentByHypotenuse(roofSlopeDegree, t1);
            double thickneesX = valueService.GetOppositeByHypotenuse(roofSlopeDegree, t1);

            // 좌측 상단을 기준으로 그림
            Point3D drawPoint = new Point3D(selPoint1.X - outsideBottomX, selPoint1.Y - outsideBottomY, selPoint1.Z);

            Line lineBa = new Line(GetSumPoint(drawPoint, 0, 0), GetSumPoint(drawPoint, insideBottomX, insideBottomY));
            Line lineBb = new Line(GetSumPoint(drawPoint, -thickneesX, thicknessY), GetSumPoint(drawPoint, insideBottomX-thickneesX, insideBottomY+thicknessY));
            Line lineTa = new Line(GetSumPoint(drawPoint, 0, 0), GetSumPoint(drawPoint, -thickneesX, thicknessY));
            Line lineTb = new Line(GetSumPoint(drawPoint, insideBottomX, insideBottomY), GetSumPoint(drawPoint, insideBottomX - thickneesX, insideBottomY + thicknessY));



            List<Entity> customBlockList = new List<Entity>();

            customBlockList.Add(lineBa);
            customBlockList.Add(lineBb);
            customBlockList.Add(lineTa);
            customBlockList.Add(lineTb);

            return customBlockList.ToArray();
        }
        public Entity[] DrawReference_CompressionRingK(CDPoint selPoint1)
        {
            int firstIndex = 0;
            double A = valueService.GetDoubleValue(assemblyData.RoofInput[firstIndex].ComRingWidthThicknedA);
            double B = valueService.GetDoubleValue(assemblyData.RoofInput[firstIndex].ComRingDistanceB);
            double C = valueService.GetDoubleValue(assemblyData.RoofInput[firstIndex].ComRingThicknessC);
            double D = valueService.GetDoubleValue(assemblyData.RoofInput[firstIndex].ComRingWidthD);
            double t1 = valueService.GetDoubleValue(assemblyData.RoofInput[firstIndex].ComRingShellThicknessT1);


            int maxCourse = valueService.GetIntValue(assemblyData.ShellInput[0].CourseCount) - 1;
            double maxCourseThk = valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].MinThk);

            //
            double t1Width = (t1 - maxCourseThk) / 2;

            // Angle Slope
            double angleSlopeDegree = valueService.GetDegreeOfSlope(1,4);
            double thicknessY = valueService.GetAdjacentByHeight(angleSlopeDegree, t1Width);

            // 우측 상단을 기준으로 그림
            Point3D drawPoint = new Point3D(selPoint1.X + t1Width, selPoint1.Y, selPoint1.Z);

            Line lineTa = new Line(GetSumPoint(drawPoint, 0, 0), GetSumPoint(drawPoint, -t1, 0));
            Line lineTHa = new Line(GetSumPoint(drawPoint, -t1, 0), GetSumPoint(drawPoint, -t1, -A + thicknessY));
            Line lineTHb = new Line(GetSumPoint(drawPoint, 0, 0), GetSumPoint(drawPoint, 0, -A + thicknessY));
            Line lineTHaL = new Line(GetSumPoint(drawPoint, -t1, -A + thicknessY), GetSumPoint(drawPoint, -t1Width - maxCourseThk, -A));
            Line lineTHbR = new Line(GetSumPoint(drawPoint, 0, -A + thicknessY), GetSumPoint(drawPoint, -t1Width, -A));

            Line lineTb = new Line(GetSumPoint(drawPoint, -t1Width, -A), GetSumPoint(drawPoint, -t1Width-maxCourseThk, -A));

            Line lineCa = new Line(GetSumPoint(drawPoint, -t1, -B), GetSumPoint(drawPoint, -t1 - D, -B));
            Line lineCb = new Line(GetSumPoint(drawPoint, -t1, -B - C), GetSumPoint(drawPoint, -t1 - D, -B - C));
            Line lineCh = new Line(GetSumPoint(drawPoint, -t1-D, -B - C), GetSumPoint(drawPoint, -t1 - D, -B ));



            List<Entity> customBlockList = new List<Entity>();

            customBlockList.Add(lineTa);
            customBlockList.Add(lineTHa);
            customBlockList.Add(lineTHb);
            customBlockList.Add(lineTHaL);
            customBlockList.Add(lineTHbR);
            customBlockList.Add(lineTb);

            customBlockList.Add(lineCa);
            customBlockList.Add(lineCb);
            customBlockList.Add(lineCh);

            return customBlockList.ToArray();
        }
        #endregion

        #region HBeam
        public HBeamModel GetHbeamModel(string selHBeamSize)
        {
            HBeamModel returnValue = null;
            string newHBeamSize = valueService.GetAllTrim(selHBeamSize);
            foreach (HBeamModel eachHBeam in assemblyData.HBeamList)
            {
                if (valueService.GetAllTrim(eachHBeam.SIZE) == newHBeamSize)
                {
                    returnValue = eachHBeam;
                    break;
                }
            }
            return returnValue;
        }
        public Entity[] DrawReference_HBeam(CDPoint selPoint1, string selBeam)
        {
            HBeamModel newHBeamSize = GetHbeamModel(selBeam);
            return DrawReference_HBeam(selPoint1, newHBeamSize);
        }
        public Entity[] DrawReference_HBeam(CDPoint selPoint1, HBeamModel selBeam)
        {
            double A = valueService.GetDoubleValue(selBeam.A);
            double B = valueService.GetDoubleValue(selBeam.B);
            double t1 = valueService.GetDoubleValue(selBeam.t1);
            double t2 = valueService.GetDoubleValue(selBeam.t2);
            double R = valueService.GetDoubleValue(selBeam.R);

            Point3D drawPoint = new Point3D(selPoint1.X, selPoint1.Y, selPoint1.Z);

            List<Entity> customBlockList = new List<Entity>();

            Line lineTopB = new Line(GetSumPoint(drawPoint, -B / 2, 0), GetSumPoint(drawPoint, B / 2, 0));
            Line lineTopB2 = new Line(GetSumPoint(drawPoint, -B / 2, 0), GetSumPoint(drawPoint, -B / 2, -t2));
            Line lineTopB3 = new Line(GetSumPoint(drawPoint, -B / 2, -t2), GetSumPoint(drawPoint, -t1 / 2, -t2));
            Line lineTopB4 = new Line(GetSumPoint(drawPoint, B / 2, 0), GetSumPoint(drawPoint, B / 2, -t2));
            Line lineTopB5 = new Line(GetSumPoint(drawPoint, t1 / 2, -t2), GetSumPoint(drawPoint, B / 2, -t2));

            Line lineA1 = new Line(GetSumPoint(drawPoint, -t1 / 2, -t2), GetSumPoint(drawPoint, -t1 / 2, -A + t2));
            Line lineA2 = new Line(GetSumPoint(drawPoint, t1 / 2, -t2), GetSumPoint(drawPoint, t1 / 2, -A + t2));

            Line lineBottomB = new Line(GetSumPoint(drawPoint, -B / 2, -A), GetSumPoint(drawPoint, B / 2, -A));
            Line lineBottomB2 = new Line(GetSumPoint(drawPoint, -B / 2, -A), GetSumPoint(drawPoint, -B / 2, -A + t2));
            Line lineBottomB3 = new Line(GetSumPoint(drawPoint, -B / 2, -A + t2), GetSumPoint(drawPoint, -t1 / 2, -A + t2));
            Line lineBottomB4 = new Line(GetSumPoint(drawPoint, B / 2, -A), GetSumPoint(drawPoint, B / 2, -A + t2));
            Line lineBottomB5 = new Line(GetSumPoint(drawPoint, t1 / 2, -A + t2), GetSumPoint(drawPoint, B / 2, -A + t2));

            Arc arcFillet1;
            if (Curve.Fillet(lineTopB3, lineA1, R, false, false, true, true, out arcFillet1))
                customBlockList.Add(arcFillet1);
            Arc arcFillet2;
            if (Curve.Fillet(lineTopB5, lineA2, R, false, true, true, true, out arcFillet2))
                customBlockList.Add(arcFillet2);

            Arc arcFillet3;
            if (Curve.Fillet(lineBottomB3, lineA1, R, true, false, true, true, out arcFillet3))
                customBlockList.Add(arcFillet3);
            Arc arcFillet4;
            if (Curve.Fillet(lineBottomB5, lineA2, R, true, true, true, true, out arcFillet4))
                customBlockList.Add(arcFillet4);

            customBlockList.Add(lineTopB);
            customBlockList.Add(lineTopB2);
            customBlockList.Add(lineTopB3);
            customBlockList.Add(lineTopB4);
            customBlockList.Add(lineTopB5);

            customBlockList.Add(lineA1);
            customBlockList.Add(lineA2);

            customBlockList.Add(lineBottomB);
            customBlockList.Add(lineBottomB2);
            customBlockList.Add(lineBottomB3);
            customBlockList.Add(lineBottomB4);
            customBlockList.Add(lineBottomB5);

            return customBlockList.ToArray();
        }
        #endregion






        #region Get Sum Point
        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
        private Point3D GetSumPoint(CDPoint selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
        #endregion
    }
}
