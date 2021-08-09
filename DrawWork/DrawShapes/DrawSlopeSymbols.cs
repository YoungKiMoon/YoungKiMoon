using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawWork.Commons;
using DrawWork.DrawModels;
using DrawWork.DrawSacleServices;
using DrawWork.DrawServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DrawWork.DrawShapes
{
    public class DrawSlopeSymbols
    {
        private DrawScaleService scaleService;
        private DrawEditingService editingService;
        public DrawSlopeSymbols()
        {
            scaleService = new DrawScaleService();
            editingService = new DrawEditingService();
        }

        public List<Entity> GetSlopeSymbol( DrawSlopeLeaderModel selLeaderModel, double selScale)
        {
            List<Entity> newEntityList = new List<Entity>();

            double scaleLeaderLength= scaleService.GetOriginValueOfScale(selScale, selLeaderModel.leaderLength);
            double scaleHeightOneSize = scaleService.GetOriginValueOfScale(selScale, selLeaderModel.heightOneSize);
            double scaleTextGap = scaleService.GetOriginValueOfScale(selScale, selLeaderModel.textGap);
            double scaleTextHeight = scaleService.GetOriginValueOfScale(selScale, selLeaderModel.textHeight);

            double angleRadian = selLeaderModel.leaderAngleRadian;

            double scaleWidthSize = (selLeaderModel.widthValue * scaleHeightOneSize) / selLeaderModel.heightValue;
            

            string heightText = selLeaderModel.heightValue.ToString();
            string widthText = selLeaderModel.widthValue.ToString();

            Line slopeLineSlope = null;
            Line slopeLineHeight = null;
            Line slopeLineWidth = null;
            Text textHeight = null;
            Text textWidth = null;

            Point3D insertPoint = selLeaderModel.insertPoint;

            Line leaderLine = new Line(GetSumPoint(insertPoint, 0, 0), GetSumPoint(insertPoint, scaleLeaderLength, 0));
            leaderLine.Rotate(angleRadian, Vector3D.AxisZ, GetSumPoint(insertPoint, 0, 0));
            //editingService.SetExtendLine(ref leaderLine, scaleLeaderLength);
            
            Point3D slopePoint = GetSumPoint(leaderLine.EndPoint,0,0);

            switch (selLeaderModel.position)
            {
                case ORIENTATION_TYPE.BOTTOMLEFT:
                    slopeLineHeight = new Line(GetSumPoint(slopePoint, 0, 0), GetSumPoint(slopePoint, -scaleHeightOneSize, 0));
                    slopeLineWidth = new Line(GetSumPoint(slopeLineHeight.EndPoint, 0, 0), GetSumPoint(slopeLineHeight.EndPoint, 0, -scaleWidthSize));
                    slopeLineSlope = new Line(GetSumPoint(slopePoint, 0, 0), GetSumPoint(slopeLineWidth.EndPoint, 0, 0));
                    textHeight = new Text(GetSumPoint(slopeLineHeight.MidPoint, 0, scaleTextGap), heightText, scaleTextHeight) {Alignment=Text.alignmentType.BaselineCenter };
                    textWidth = new Text(GetSumPoint(slopeLineWidth.MidPoint, -scaleTextGap, 0), widthText, scaleTextHeight) {Alignment=Text.alignmentType.MiddleRight };
                    break;
                case ORIENTATION_TYPE.BOTTOMRIGHT:
                    break;
                case ORIENTATION_TYPE.TOPLEFT:
                    break;
                case ORIENTATION_TYPE.TOPRIGHT:
                    break;
            }

            newEntityList.Add(leaderLine);
            newEntityList.Add(slopeLineHeight);
            newEntityList.Add(slopeLineWidth);
            newEntityList.Add(slopeLineSlope);
            newEntityList.Add(textHeight);
            newEntityList.Add(textWidth);

            return newEntityList;
        }

        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
    }
}
