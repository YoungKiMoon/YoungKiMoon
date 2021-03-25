﻿using System;
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
    public class DrawBlockService
    {
        private AssemblyModel assemblyData;

        private ValueService valueService;

        public DrawBlockService(AssemblyModel selAssembly)
        {
            assemblyData = selAssembly;
            valueService = new ValueService();
        }

        public Entity[] DrawBlock_TopAngle(CDPoint selPoint1,string selAngleType, EqualAngleSizeModel selAngle)
        {
            
            double AB = valueService.GetDoubleValue( selAngle.AB);
            double t = valueService.GetDoubleValue(selAngle.t);
            double R1 = valueService.GetDoubleValue(selAngle.R1);
            double R2 = valueService.GetDoubleValue(selAngle.R2);
            double CD = valueService.GetDoubleValue(selAngle.CD);
            double E = valueService.GetDoubleValue(selAngle.E);

            Point3D drawPoint = new Point3D(selPoint1.X, selPoint1.Y, selPoint1.Z);

            // Type
            switch (selAngleType){
                case "b":
                    drawPoint = GetSumPoint(drawPoint, -AB, -AB);
                    
                    break;

            }

            Line lineA = new Line(GetSumPoint(drawPoint, 0, AB), GetSumPoint(drawPoint,AB, AB));
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

        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
    }
}
