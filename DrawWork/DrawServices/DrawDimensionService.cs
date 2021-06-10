using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawWork.Commons;
using DrawWork.CutomModels;
using DrawWork.DrawModels;
using DrawWork.ValueServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawServices
{
    public class DrawDimensionService
    {
        ValueService valueService;
        public DrawDimensionService()
        {
            valueService = new ValueService();
        }

        // Mark
        public Dictionary<string, List<Entity>> DrawDimension_NozzleMark( ref Line nozzleLine, bool lineTrim, Point3D drawPoint, NozzleInputModel selNozzle, double selScaleValue)
        {
            //string selPosition = selNozzle.Position;
            //string selLR = selNozzle.LR;
            string textUpperStr = selNozzle.Mark;
            string textLowerStr = selNozzle.Size;
            
            // Mark : Size
            double cirDiameter = GetNozzleCircleDiameter(selScaleValue);
            double cirRadius = cirDiameter / 2;
            double cirTextSize = 2.5;
            double cirTextGapHeight = 2.5;

            // Mark : Size : Scale
            double scaleDiameter = cirDiameter ;
            double scaleRadius = scaleDiameter/2;
            double scaleTextHeight = cirTextSize * selScaleValue;
            double scaleTextGapHeight = cirTextGapHeight * selScaleValue;

            // Text Width
            double upperTextWidthFactor = GetNozzleMarkTextWidthFactor(textUpperStr.Length);
            double lowerTextWidthFactor = GetNozzleMarkTextWidthFactor(textLowerStr.Length);

            Circle circleCenter = new Circle(GetSumPoint(drawPoint, 0, 0), scaleRadius);
            Line lineCenter = new Line(GetSumPoint(drawPoint, -scaleRadius, 0), GetSumPoint(drawPoint, scaleRadius, 0));
            Text textUpper = new Text(GetSumPoint(drawPoint, 0, scaleTextGapHeight), textUpperStr, scaleTextHeight);
            textUpper.Alignment = Text.alignmentType.MiddleCenter;
            textUpper.WidthFactor = upperTextWidthFactor;
            Text textLower = new Text(GetSumPoint(drawPoint, 0, -scaleTextGapHeight), textLowerStr, scaleTextHeight);
            textLower.Alignment = Text.alignmentType.MiddleCenter;
            textLower.WidthFactor = lowerTextWidthFactor;

            // Line Tirm
            if (lineTrim)
            {
                Point3D[] lineInter = nozzleLine.IntersectWith(circleCenter);
                if (lineInter.Length > 0)
                {
                    nozzleLine.TrimBy(lineInter[0], false);
                }
            }

            // Entity
            List<Entity> nozzleMarkList = new List<Entity>();
            nozzleMarkList.Add(circleCenter);
            nozzleMarkList.Add(lineCenter);

            List<Entity> nozzleTextList = new List<Entity>();
            nozzleTextList.Add(textUpper);
            nozzleTextList.Add(textLower);

            Dictionary<string, List<Entity>> customEntity = new Dictionary<string, List<Entity>>();
            customEntity.Add(CommonGlobal.NozzleMark, nozzleMarkList);
            customEntity.Add(CommonGlobal.NozzleText, nozzleTextList);

            return customEntity;
        }

        public Text DrawDimension_NozzleTheta(ref Line nozzleLine, ref Circle labelCircle, string selTheta,double selScaleValue)
        {
            double thetaTextSize = 2.5;
            double thetaTextSizeScale = thetaTextSize * selScaleValue;
            double thetaValue = valueService.GetDoubleValue(selTheta);
            double thetaRadian = Utility.DegToRad(thetaValue);
            double offsetValue = 1 * selScaleValue;
            string angelText = "˚";
            Text labelText = null;
            if (thetaValue > 180) 
            {
                // Line Right
                Line newOffsetLine = (Line)nozzleLine.Offset(offsetValue, Vector3D.AxisZ);
                Point3D[] labelInter = nozzleLine.IntersectWith(labelCircle);
                if (labelInter.Length > 0)
                {
                    thetaRadian = thetaRadian + Utility.DegToRad(90);
                    labelText = new Text(GetSumPoint(labelInter[0], 0, 0), selTheta.Trim() + angelText, thetaTextSizeScale);
                    labelText.Alignment = Text.alignmentType.BottomCenter;
                    labelText.Rotate(-thetaRadian, Vector3D.AxisZ, GetSumPoint(labelInter[0], 0, 0));
                }
            }
            else
            {
                // Line Left
                Line newOffsetLine = (Line)nozzleLine.Offset(-offsetValue,Vector3D.AxisZ);
                Point3D[] labelInter = nozzleLine.IntersectWith(labelCircle);
                if (labelInter.Length > 0)
                {
                    labelText = new Text(GetSumPoint(labelInter[0], 0, 0), selTheta.Trim() + angelText, thetaTextSizeScale);
                    labelText.Alignment = Text.alignmentType.BottomCenter;
                    thetaRadian= Utility.DegToRad(90)- thetaRadian; 
                    labelText.Rotate(thetaRadian, Vector3D.AxisZ, GetSumPoint(labelInter[0], 0, 0));
                }
            }

            return labelText;
        }

        public double GetNozzleCircleDiameter(double selScaleValue)
        {
            double cirDiameter = 11;
            return cirDiameter * selScaleValue;
        }
        private double GetNozzleMarkTextWidthFactor(double stringLength)
        {
            double returnValue = 1;
            if (stringLength == 4)
                returnValue = 0.8;
            else if (stringLength > 4)
                returnValue = 0.55;
            return returnValue;
        }


        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
        private Point3D GetSumPoint(CDPoint selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
    }
}
