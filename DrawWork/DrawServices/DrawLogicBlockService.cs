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
using DrawWork.Commons;

namespace DrawWork.DrawServices
{
    public class DrawLogicBlockService
    {
        private AssemblyModel assemblyData;

        private ValueService valueService;
        private DrawWorkingPointService workingPointService;

        private DrawReferenceBlockService refBlockService;

        public DrawLogicBlockService(AssemblyModel selAssembly)
        {
            assemblyData = selAssembly;
            valueService = new ValueService();
            workingPointService = new DrawWorkingPointService(selAssembly);
            refBlockService = new DrawReferenceBlockService(selAssembly);
        }

        public Entity[] DrawBlock_TopAngle(CDPoint selPoint1, ref CDPoint refPoint, ref CDPoint curPoint)
        {

            int refFirstIndex = 0;

            string selSizeTankHeight = assemblyData.GeneralDesignData[refFirstIndex].SizeTankHeight;

            string selAngleType = assemblyData.RoofInput[refFirstIndex].TopAngleType;
            string selAngleSize = assemblyData.RoofInput[refFirstIndex].TopAngleSize;

            AngleSizeModel selAngleModel = refBlockService.GetAngleSizeModel(selAngleSize);
            int maxCourse = valueService.GetIntValue(assemblyData.ShellInput[0].CourseCount) - 1;

            CDPoint drawPoint = new CDPoint();
            CDPoint mirrorPoint = new CDPoint();

            Entity[] angleEntity = null;

            // Type
            switch (selAngleType)
            {
                case "b":
                    drawPoint = GetSumCDPoint(refPoint,
                                              -valueService.GetDoubleValue(selAngleModel.AB)
                                              - valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].MinThk),

                                              -valueService.GetDoubleValue(selAngleModel.AB)
                                              + valueService.GetDoubleValue(selSizeTankHeight));

                    angleEntity = refBlockService.DrawReference_Angle(drawPoint, selAngleModel);
                    break;

                case "d":
                    drawPoint = GetSumCDPoint(refPoint,
                                              -valueService.GetDoubleValue(selAngleModel.AB),

                                              -valueService.GetDoubleValue(selAngleModel.AB)
                                              + valueService.GetDoubleValue(selSizeTankHeight));

                    angleEntity = refBlockService.DrawReference_Angle(drawPoint, selAngleModel);
                    break;

                case "e":
                    drawPoint = GetSumCDPoint(refPoint,
                                              -valueService.GetDoubleValue(selAngleModel.AB)
                                              - valueService.GetDoubleValue(selAngleModel.t),

                                              -valueService.GetDoubleValue(selAngleModel.AB)
                                              + valueService.GetDoubleValue(selSizeTankHeight));

                    angleEntity = refBlockService.DrawReference_Angle(drawPoint, selAngleModel);
                    break;

                case "i":
                    drawPoint = GetSumCDPoint(refPoint,
                                              -valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].MinThk),

                                               valueService.GetDoubleValue(selSizeTankHeight));
                    angleEntity = refBlockService.DrawReference_CompressionRingI(drawPoint);
                    break;

                case "k":
                    drawPoint = GetSumCDPoint(refPoint,
                                              0,

                                               valueService.GetDoubleValue(selSizeTankHeight));
                    angleEntity = refBlockService.DrawReference_CompressionRingK(drawPoint);
                    break;

            }



            switch (selAngleType)
            {
                case "e":
                    mirrorPoint = GetSumCDPoint(drawPoint, valueService.GetDoubleValue(selAngleModel.AB), 0);
                    Plane pl1 = Plane.YZ;
                    pl1.Origin.X = mirrorPoint.X;
                    pl1.Origin.Y = mirrorPoint.Y;
                    Mirror customMirror = new Mirror(pl1);
                    foreach (Entity eachEntity in angleEntity)
                        eachEntity.TransformBy(customMirror);

                    break;

            }


            return angleEntity;
        }

        public Entity[] DrawBlock_Structure(CDPoint selPoint1)
        {
            // Sturcutre Type
            // Type
            DrawStructureService StructureDivService = new DrawStructureService();
            StructureDivService.SetStructureData(SingletonData.TankType, assemblyData.StructureInput[0].SupportingType, assemblyData.RoofInput[0].TopAngleType);


            int firstIndex = 0;
            Point3D drawPoint = new Point3D(selPoint1.X, selPoint1.Y, selPoint1.Z);
            CDPoint refPoint = new CDPoint() { X = drawPoint.X, Y = drawPoint.Y };
            CDPoint curPoint = (CDPoint)refPoint.Clone();

            List<Entity> customBlockList = new List<Entity>();

            // Roof Slope
            string roofSlopeString = assemblyData.RoofInput[firstIndex].RoofSlopeOne;
            double roofSlopeDegree = valueService.GetDegreeOfSlope(roofSlopeString);

            StructureColumnBaseSupportModel eachColumnBase = assemblyData.StructureColumnBaseSupportOutput[firstIndex];
            double bA = valueService.GetDoubleValue(eachColumnBase.A);
            double bB = valueService.GetDoubleValue(eachColumnBase.B);
            double bC = valueService.GetDoubleValue(eachColumnBase.C);
            double bD = valueService.GetDoubleValue(eachColumnBase.D);
            double bE = valueService.GetDoubleValue(eachColumnBase.E);
            double bF = valueService.GetDoubleValue(eachColumnBase.F);
            double bG = valueService.GetDoubleValue(eachColumnBase.G);
            double bH = valueService.GetDoubleValue(eachColumnBase.H);
            double bI = valueService.GetDoubleValue(eachColumnBase.I);
            double bJ = valueService.GetDoubleValue(eachColumnBase.J);

            // Basic
            PipeModel centerPipe = assemblyData.StructureColumnPipeOutput[0]; // Center
            double centerPipeOD = valueService.GetDoubleValue(centerPipe.OD);
            double centerPipeODHalf = centerPipeOD / 2;
            double centerRadius = 0; // Center
            CDPoint centerColumnPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, centerRadius, ref refPoint, ref curPoint);

            // Column Center
            double centerLeftWidthHalf = 0;
            double boltHoleHeight = 0;
            double boltHoleWidth = 0;
            boltHoleHeight = valueService.GetDoubleValue(assemblyData.StructureClipSlotHoleOutput[firstIndex].ht);
            boltHoleWidth = valueService.GetDoubleValue(assemblyData.StructureClipSlotHoleOutput[firstIndex].wd);

            // Centering
            CDPoint centeringClipRight = new CDPoint();

            if (StructureDivService.columnType == "column")
            {
                #region Column Side
                for (int i = 0; i < assemblyData.StructureGirderInput.Count; i++)
                {
                    #region Column Basic
                    HBeamModel eachHBeam = assemblyData.StructureColumnHBeamOutput[i];
                    StructureColumnSideModel eachColumnSide = assemblyData.StructureColumnSideOutput[i];
                    PipeModel eachPipe = assemblyData.StructureColumnPipeOutput[i + 1]; // Start = 2 Column

                    string Size = eachColumnSide.A1A2;
                    double B = valueService.GetDoubleValue(eachColumnSide.B);
                    string C = eachColumnSide.C;
                    double D = valueService.GetDoubleValue(eachColumnSide.D);
                    double E = valueService.GetDoubleValue(eachColumnSide.E);
                    double F = valueService.GetDoubleValue(eachColumnSide.F);
                    double G = valueService.GetDoubleValue(eachColumnSide.G);
                    double H = valueService.GetDoubleValue(eachColumnSide.H);

                    double pipeOD = valueService.GetDoubleValue(eachPipe.OD);
                    double pipeODHalf = pipeOD / 2;

                    double radius = valueService.GetDoubleValue(assemblyData.StructureGirderInput[i].GirderInRadius);

                    CDPoint eachColumnPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, radius, ref refPoint, ref curPoint);
                    #endregion

                    #region HBream
                    CDPoint eachHBeamPoint = (CDPoint)eachColumnPoint.Clone();
                    eachHBeamPoint.Y = eachHBeamPoint.Y - B;
                    Entity[] eachHBeamEntity = refBlockService.DrawReference_HBeam(eachHBeamPoint, eachHBeam);
                    customBlockList.AddRange(eachHBeamEntity);
                    #endregion

                    #region Side Top Support
                    eachColumnPoint.Y = eachHBeamPoint.Y - valueService.GetDoubleValue(eachHBeam.A);
                    drawPoint.X = eachColumnPoint.X;
                    drawPoint.Y = eachColumnPoint.Y;

                    Line lineTopPad1 = new Line(GetSumPoint(drawPoint, -H - F - pipeODHalf, 0), GetSumPoint(drawPoint, H + F + pipeODHalf, 0));
                    Line lineTopPad2 = new Line(GetSumPoint(drawPoint, -H - F - pipeODHalf, -D), GetSumPoint(drawPoint, H + F + pipeODHalf, -D));
                    Line lineTopPad3 = new Line(GetSumPoint(drawPoint, -H - F - pipeODHalf, 0), GetSumPoint(drawPoint, -H - F - pipeODHalf, -D));
                    Line lineTopPad4 = new Line(GetSumPoint(drawPoint, H + F + pipeODHalf, 0), GetSumPoint(drawPoint, H + F + pipeODHalf, -D));
                    customBlockList.AddRange(new Line[] { lineTopPad1, lineTopPad2, lineTopPad3, lineTopPad4 });

                    Line lineTopLeft1 = new Line(GetSumPoint(drawPoint, -F - pipeODHalf, -D), GetSumPoint(drawPoint, -F - pipeODHalf, -D - G));
                    Line lineTopLeft2 = new Line(GetSumPoint(drawPoint, -pipeODHalf, -D), GetSumPoint(drawPoint, -pipeODHalf, -D - E));
                    Line lineTopLeft3 = new Line(GetSumPoint(drawPoint, -G - pipeODHalf, -D - E), GetSumPoint(drawPoint, -pipeODHalf, -D - E));
                    Line lineTopLeft4 = new Line(GetSumPoint(drawPoint, -G - pipeODHalf, -D - E), GetSumPoint(drawPoint, -F - pipeODHalf, -D - G));
                    customBlockList.AddRange(new Line[] { lineTopLeft1, lineTopLeft2, lineTopLeft3, lineTopLeft4 });


                    Line lineTopRight1 = new Line(GetSumPoint(drawPoint, F + pipeODHalf, -D), GetSumPoint(drawPoint, F + pipeODHalf, -D - G));
                    Line lineTopRight2 = new Line(GetSumPoint(drawPoint, pipeODHalf, -D), GetSumPoint(drawPoint, pipeODHalf, -D - E));
                    Line lineTopRight3 = new Line(GetSumPoint(drawPoint, G + pipeODHalf, -D - E), GetSumPoint(drawPoint, pipeODHalf, -D - E));
                    Line lineTopRight4 = new Line(GetSumPoint(drawPoint, G + pipeODHalf, -D - E), GetSumPoint(drawPoint, F + pipeODHalf, -D - G));
                    customBlockList.AddRange(new Line[] { lineTopRight1, lineTopRight2, lineTopRight3, lineTopRight4 });
                    #endregion

                    #region Support Buttom Support

                    CDPoint eachColumnBasePoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, radius, ref refPoint, ref curPoint);
                    drawPoint.X = eachColumnBasePoint.X;
                    drawPoint.Y = eachColumnBasePoint.Y;

                    CDPoint eachColumnBasePointLeft = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, radius + pipeODHalf + bD + bC + bJ, ref refPoint, ref curPoint);
                    CDPoint eachColumnBasePointRight = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, radius - pipeODHalf - bD - bC - bJ, ref refPoint, ref curPoint);
                    //Line basePad1 = new Line(new Point3D(eachColumnBasePointLeft.X, eachColumnBasePointLeft.Y), new Point3D(eachColumnBasePointRight.X, eachColumnBasePointRight.Y));                    
                    Line basePad2 = new Line(GetSumPoint(eachColumnBasePointLeft, 0, 0), GetSumPoint(eachColumnBasePointLeft, 0, bA));
                    Line basePad3 = new Line(GetSumPoint(eachColumnBasePointLeft, 0, bA), GetSumPoint(eachColumnBasePointRight, 0, bA));
                    Line basePad4 = new Line(GetSumPoint(eachColumnBasePointRight, 0, 0), GetSumPoint(eachColumnBasePointRight, 0, bA));
                    customBlockList.AddRange(new Line[] { basePad2, basePad3, basePad4 });

                    CDPoint eachColumnBasePointLeft2 = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, radius + pipeODHalf + bD + bC + bI, ref refPoint, ref curPoint);
                    CDPoint eachColumnBasePointRight2 = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, radius - pipeODHalf - bD - bC - bI, ref refPoint, ref curPoint);
                    //Line basePadPad1 = new Line(new Point3D(eachColumnBasePointLeft2.X, eachColumnBasePointLeft2.Y + bA), new Point3D(eachColumnBasePointRight2.X, eachColumnBasePointRight2.Y + bA));
                    Line basePadPad2 = new Line(GetSumPoint(eachColumnBasePointLeft2, 0, bA), GetSumPoint(eachColumnBasePointLeft2, 0, bA + bB));
                    Line basePadPad3 = new Line(GetSumPoint(eachColumnBasePointLeft2, 0, bA + bB), GetSumPoint(eachColumnBasePointRight2, 0, bA + bB));
                    Line basePadPad4 = new Line(GetSumPoint(eachColumnBasePointRight2, 0, bA), GetSumPoint(eachColumnBasePointRight2, 0, bA + bB));
                    customBlockList.AddRange(new Line[] { basePadPad2, basePadPad3, basePadPad4 });

                    CDPoint eachColumnBasePointLeft3 = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, radius + pipeODHalf + bD + bC, ref refPoint, ref curPoint);
                    CDPoint eachColumnBasePointRight3 = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, radius - pipeODHalf - bD - bC, ref refPoint, ref curPoint);
                    Line baseLeft1 = new Line(GetSumPoint(eachColumnBasePointLeft3, 0, bA + bB), GetSumPoint(eachColumnBasePointLeft3, 0, bA + bB + bF));
                    Line baseRight1 = new Line(GetSumPoint(eachColumnBasePointRight3, 0, bA + bB), GetSumPoint(eachColumnBasePointRight3, 0, bA + bB + bF));
                    customBlockList.AddRange(new Line[] { baseLeft1, baseRight1 });

                    CDPoint eachColumnBasePointLeft4 = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, radius + pipeODHalf + bC + bG, ref refPoint, ref curPoint);
                    CDPoint eachColumnBasePointRight4 = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, radius - pipeODHalf - bC - bG, ref refPoint, ref curPoint);
                    Line baseLeftLeft1 = new Line(GetSumPoint(eachColumnBasePointLeft4, 0, bA + bB), GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC - bG, bA + bB + bE - bH));
                    //Line baseLeftLeft2 = new Line(GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC - bG, +bA + bB + bE - bH), GetSumPoint(eachColumnBasePoint, 0, bA + bB + bE - bH));
                    Line baseLeftLeft3 = new Line(GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC, +bA + bB + bE - bH), GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC, +bA + bB + bE));
                    Line baseLeftLeft4 = new Line(GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC, +bA + bB + bE), GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC - bF, +bA + bB + bE));
                    Line baseLeftLeft5 = new Line(GetSumPoint(eachColumnBasePointLeft3, 0, +bA + bB + bF), GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC - bF, +bA + bB + bE));
                    customBlockList.AddRange(new Line[] { baseLeftLeft1, baseLeftLeft3, baseLeftLeft4, baseLeftLeft5 });

                    Line baseLeftRight1 = new Line(GetSumPoint(eachColumnBasePointRight4, 0, bA + bB), GetSumPoint(eachColumnBasePoint, pipeODHalf + bC + bG, bA + bB + bE - bH));
                    //Line baseLeftRight2 = new Line(GetSumPoint(eachColumnBasePoint, pipeODHalf + bC + bG, +bA + bB + bE - bH), GetSumPoint(eachColumnBasePoint, 0, bA + bB + bE - bH));
                    Line baseLeftRight3 = new Line(GetSumPoint(eachColumnBasePoint, pipeODHalf + bC, +bA + bB + bE - bH), GetSumPoint(eachColumnBasePoint, pipeODHalf + bC, +bA + bB + bE));
                    Line baseLeftRight4 = new Line(GetSumPoint(eachColumnBasePoint, pipeODHalf + bC, +bA + bB + bE), GetSumPoint(eachColumnBasePoint, pipeODHalf + bC + bF, +bA + bB + bE));
                    Line baseLeftRight5 = new Line(GetSumPoint(eachColumnBasePointRight3, 0, +bA + bB + bF), GetSumPoint(eachColumnBasePoint, pipeODHalf + bC + bF, +bA + bB + bE));
                    customBlockList.AddRange(new Line[] { baseLeftRight1, baseLeftRight3, baseLeftRight4, baseLeftRight5 });

                    Line baseHorizon1 = new Line(GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC - bG, +bA + bB + bE - bH), GetSumPoint(eachColumnBasePoint, pipeODHalf + bC + bG, bA + bB + bE - bH));
                    customBlockList.Add(baseHorizon1);
                    #endregion

                    #region Column
                    Line columnLeft = new Line(GetSumPoint(eachColumnPoint, -pipeODHalf, -D - E), GetSumPoint(eachColumnBasePoint, -pipeODHalf, +bA + bB + bE - bH));
                    Line columnRight = new Line(GetSumPoint(eachColumnPoint, pipeODHalf, -D - E), GetSumPoint(eachColumnBasePoint, pipeODHalf, +bA + bB + bE - bH));
                    customBlockList.AddRange(new Line[] { columnLeft, columnRight });
                    #endregion
                }
                #endregion

                #region Column Center

                StructureColumnCenterModel centerTopSupport = assemblyData.StructureColumnCenterOutput[firstIndex];
                double tsSize = valueService.GetDoubleValue(centerTopSupport.COLUMN);
                double tsA = valueService.GetDoubleValue(centerTopSupport.A);
                double tsB = valueService.GetDoubleValue(centerTopSupport.B);
                double tsC = valueService.GetDoubleValue(centerTopSupport.C);
                double tsD = valueService.GetDoubleValue(centerTopSupport.D);
                double tsE = valueService.GetDoubleValue(centerTopSupport.E);
                double tsF = valueService.GetDoubleValue(centerTopSupport.F);
                double tsG = valueService.GetDoubleValue(centerTopSupport.G);
                double tsH = valueService.GetDoubleValue(centerTopSupport.H);
                double tsI = valueService.GetDoubleValue(centerTopSupport.I);

                StructureColumnRafterModel centerFirstRafter = assemblyData.StructureColumnRafterOutput[firstIndex];
                double rA = valueService.GetDoubleValue(centerFirstRafter.A);
                double rB = valueService.GetDoubleValue(centerFirstRafter.B);
                double rC = valueService.GetDoubleValue(centerFirstRafter.C);
                double rD = valueService.GetDoubleValue(centerFirstRafter.D);
                double rE = valueService.GetDoubleValue(centerFirstRafter.E);
                double rBoltHoleOnShell = valueService.GetDoubleValue(centerFirstRafter.BoltHoleOnShell);
                double rBoltHoleOnColumn = valueService.GetDoubleValue(centerFirstRafter.BoltHoleOnColumn);
                double rBoltHoleOnCenter = valueService.GetDoubleValue(centerFirstRafter.BoltHoleOnCenter);
                double rBoltHoleDia = valueService.GetDoubleValue(centerFirstRafter.BoltHoleDia);

                // WP : Left Square Center
                centerLeftWidthHalf = tsB;
                CDPoint centerTopRoofLeftB = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, centerRadius + centerLeftWidthHalf, ref refPoint, ref curPoint);

                double centerRafterHeightHalf = rA / 2;
                double raHeightHalf = valueService.GetOppositeByWidth(roofSlopeString, centerRafterHeightHalf);
                double raSlopeWidthHalf = Math.Sqrt(centerRafterHeightHalf * centerRafterHeightHalf + raHeightHalf * raHeightHalf);
                CDPoint centerTopLeftSquare = new CDPoint(); // B Center
                centerTopLeftSquare.X = centerTopRoofLeftB.X;
                centerTopLeftSquare.Y = centerTopRoofLeftB.Y - raSlopeWidthHalf;

                // WP : Left Pad 
                double centerLeftWidthODHalf = centerPipeODHalf + tsG + 30;// 30 값 고정
                CDPoint centerTopRoofLeft = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, centerRadius + centerLeftWidthODHalf, ref refPoint, ref curPoint);

                // Square : 평행 방향이동 : 아래쪽
                double centerRafterHeight = rA + 20; // MinValue : 20
                double raHeight = valueService.GetOppositeByWidth(roofSlopeString, centerRafterHeight);
                double raSlopeWidth = Math.Sqrt(centerRafterHeight * centerRafterHeight + raHeight * raHeight);
                CDPoint centerTopLeft = new CDPoint();
                centerTopLeft.X = centerTopRoofLeft.X;
                centerTopLeft.Y = centerTopRoofLeft.Y - raSlopeWidth;

                // Center Top Support : Square
                Line lineSquare1 = new Line(GetSumPoint(centerTopLeftSquare, -tsC / 2, tsE), GetSumPoint(centerTopLeftSquare, tsC / 2, tsE));
                Line lineSquare2 = new Line(GetSumPoint(centerTopLeftSquare, -tsC / 2, tsE), new Point3D(centerTopLeftSquare.X - tsC / 2, centerTopLeft.Y));
                Line lineSquare3 = new Line(GetSumPoint(centerTopLeftSquare, tsC / 2, tsE), new Point3D(centerTopLeftSquare.X + tsC / 2, centerTopLeft.Y));
                customBlockList.AddRange(new Line[] { lineSquare1, lineSquare2, lineSquare3 });


                // Center Top Support : Bolt Hole : 직각 방향 이동 : 90도 아래
                double ellipseWidth = tsD / 2 * Math.Cos(roofSlopeDegree);
                double ellipseHeight = tsD / 2 * Math.Sin(roofSlopeDegree);
                Point3D centerEllipseLeft = GetSumPoint(centerTopLeftSquare, -ellipseWidth, -ellipseHeight);
                Point3D centerEllipseRight = GetSumPoint(centerTopLeftSquare, ellipseWidth, ellipseHeight);


                CompositeCurve leftBolt1 = GetBoltHoleHorizontal(centerEllipseLeft, boltHoleWidth, boltHoleHeight);
                leftBolt1.Rotate(roofSlopeDegree, Vector3D.AxisZ, centerEllipseLeft);
                CompositeCurve leftBolt2 = GetBoltHoleHorizontal(centerEllipseRight, boltHoleWidth, boltHoleHeight);
                leftBolt2.Rotate(roofSlopeDegree, Vector3D.AxisZ, centerEllipseRight);
                customBlockList.AddRange(new CompositeCurve[] { leftBolt1, leftBolt2 });

                Circle leftBoltcircle1 = new Circle(centerEllipseLeft, boltHoleHeight / 2);
                Circle leftBoltcircle2 = new Circle(centerEllipseRight, boltHoleHeight / 2);
                customBlockList.AddRange(new Circle[] { leftBoltcircle1, leftBoltcircle2 });

                // Center Top Support : Pad
                double padHeight = 44; // 임시로 적용
                double padPushWidth = 30;
                double triEdge = 50;
                double smallTri = 15;

                Line topPad1 = new Line(GetSumPoint(centerTopLeft, 0, 0), GetSumPoint(centerTopLeft, centerLeftWidthODHalf, 0));
                Line topPad2 = new Line(GetSumPoint(centerTopLeft, 0, -padHeight), GetSumPoint(centerTopLeft, centerLeftWidthODHalf, -padHeight));
                Line topPad3 = new Line(GetSumPoint(centerTopLeft, 0, 0), GetSumPoint(centerTopLeft, 0, -padHeight));
                customBlockList.AddRange(new Line[] { topPad1, topPad2, topPad3 });

                Line topTriLeft1 = new Line(GetSumPoint(centerTopLeft, padPushWidth, -padHeight), GetSumPoint(centerTopLeft, padPushWidth, -padHeight - triEdge));
                Line topTriLeft2 = new Line(GetSumPoint(centerTopLeft, padPushWidth + tsG - smallTri, -padHeight), GetSumPoint(centerTopLeft, padPushWidth + tsG, -padHeight - smallTri));
                Line topTriLeft3 = new Line(GetSumPoint(centerTopLeft, padPushWidth + tsG, -padHeight - smallTri), GetSumPoint(centerTopLeft, padPushWidth + tsG, -padHeight - tsH));
                Line topTriLeft4 = new Line(GetSumPoint(centerTopLeft, padPushWidth + tsG - triEdge, -padHeight - tsH), GetSumPoint(centerTopLeft, padPushWidth + tsG, -padHeight - tsH)); // center column top
                Line topTriLeft5 = new Line(GetSumPoint(centerTopLeft, padPushWidth + tsG - triEdge, -padHeight - tsH), GetSumPoint(centerTopLeft, padPushWidth, -padHeight - triEdge));

                customBlockList.AddRange(new Line[] { topTriLeft1, topTriLeft2, topTriLeft3, topTriLeft4, topTriLeft5 });



                // Support Buttom Support
                CDPoint centerColumnBasePoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, centerRadius, ref refPoint, ref curPoint);
                drawPoint.X = centerColumnBasePoint.X;
                drawPoint.Y = centerColumnBasePoint.Y;

                CDPoint centerColumnBasePointLeft = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, centerRadius + centerPipeODHalf + bD + bC + bJ, ref refPoint, ref curPoint);
                Line centerPad2 = new Line(GetSumPoint(centerColumnBasePointLeft, 0, 0), GetSumPoint(centerColumnBasePointLeft, 0, bA));
                Line centerPad3 = new Line(GetSumPoint(centerColumnBasePointLeft, 0, bA), GetSumPoint(centerColumnBasePoint, 0, bA));
                Line centerPad4 = new Line(GetSumPoint(centerColumnBasePoint, 0, 0), GetSumPoint(centerColumnBasePoint, 0, bA));
                customBlockList.AddRange(new Line[] { centerPad2, centerPad3, centerPad4 });

                CDPoint centerColumnBasePointLeft2 = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, centerRadius + centerPipeODHalf + bD + bC + bI, ref refPoint, ref curPoint);
                Line centerPadPad2 = new Line(GetSumPoint(centerColumnBasePointLeft2, 0, bA), GetSumPoint(centerColumnBasePointLeft2, 0, bA + bB));
                Line centerPadPad3 = new Line(GetSumPoint(centerColumnBasePointLeft2, 0, bA + bB), GetSumPoint(centerColumnBasePoint, 0, bA + bB));
                Line centerPadPad4 = new Line(GetSumPoint(centerColumnBasePoint, 0, bA), GetSumPoint(centerColumnBasePoint, 0, bA + bB));
                customBlockList.AddRange(new Line[] { centerPadPad2, centerPadPad3, centerPadPad4 });

                CDPoint centerColumnBasePointLeft3 = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, centerRadius + centerPipeODHalf + bD + bC, ref refPoint, ref curPoint);
                Line centerLeft1 = new Line(GetSumPoint(centerColumnBasePointLeft3, 0, bA + bB), GetSumPoint(centerColumnBasePointLeft3, 0, bA + bB + bF));
                customBlockList.AddRange(new Line[] { centerLeft1 });

                CDPoint centerColumnBasePointLeft4 = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, centerRadius + centerPipeODHalf + bC + bG, ref refPoint, ref curPoint);
                Line centerLeftLeft1 = new Line(GetSumPoint(centerColumnBasePointLeft4, 0, bA + bB), GetSumPoint(centerColumnBasePoint, -centerPipeODHalf - bC - bG, bA + bB + bE - bH));
                Line centerLeftLeft2 = new Line(GetSumPoint(centerColumnBasePoint, -centerPipeODHalf - bC - bG, +bA + bB + bE - bH), GetSumPoint(centerColumnBasePoint, 0, bA + bB + bE - bH));
                Line centerLeftLeft3 = new Line(GetSumPoint(centerColumnBasePoint, -centerPipeODHalf - bC, +bA + bB + bE - bH), GetSumPoint(centerColumnBasePoint, -centerPipeODHalf - bC, +bA + bB + bE));
                Line centerLeftLeft4 = new Line(GetSumPoint(centerColumnBasePoint, -centerPipeODHalf - bC, +bA + bB + bE), GetSumPoint(centerColumnBasePoint, -centerPipeODHalf - bC - bF, +bA + bB + bE));
                Line centerLeftLeft5 = new Line(GetSumPoint(centerColumnBasePointLeft3, 0, +bA + bB + bF), GetSumPoint(centerColumnBasePoint, -centerPipeODHalf - bC - bF, +bA + bB + bE));
                customBlockList.AddRange(new Line[] { centerLeftLeft1, centerLeftLeft2, centerLeftLeft3, centerLeftLeft4, centerLeftLeft5 });

                // Column  : Column Center Top Support 아래쪽 이랑 Base Support 연결
                Line centerColumnLeft = new Line(GetSumPoint(centerTopLeft, padPushWidth + tsG, -padHeight - tsH), GetSumPoint(centerColumnBasePoint, -centerPipeODHalf, +bA + bB + bE - bH));
                customBlockList.AddRange(new Line[] { centerColumnLeft });

                #endregion
            }
            else if (StructureDivService.columnType == "centering")
            {

                StructureCenterRingInputModel centeringInput = assemblyData.StructureCenterRingInput[firstIndex];
                StructureCenteringModel centeringOutput = assemblyData.StructureCenteringOutput[firstIndex];
                double centeringOD = valueService.GetDoubleValue(centeringInput.OD);
                double centeringID = valueService.GetDoubleValue(centeringInput.ID);
                double centeringA = valueService.GetDoubleValue(centeringOutput.A);
                double centeringB = valueService.GetDoubleValue(centeringOutput.B);
                double centeringC = valueService.GetDoubleValue(centeringOutput.C);
                double centeringD = valueService.GetDoubleValue(centeringOutput.D);
                double centeringE = valueService.GetDoubleValue(centeringOutput.E);
                double centeringT1 = valueService.GetDoubleValue(centeringOutput.t1);
                double centeringT2 = valueService.GetDoubleValue(centeringOutput.t2);
                //Cal
                
                centeringID = (centeringOD - (centeringD * 2)) / 2;
                centeringE = (centeringOD + (centeringC * 2)) / 2;
                double centeringIDHalf = centeringOD / 2;

                if (StructureDivService.centeringInEx == "internal")
                {
                    #region CenterRing


                    CDPoint centeringWP = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, centerRadius + centeringE, ref refPoint, ref curPoint);

                    Line cLine01 = new Line(GetSumPoint(centeringWP, 0, 0), GetSumPoint(centeringWP, centeringE, 0));
                    Line cLine02 = new Line(GetSumPoint(centeringWP, 0, -centeringT1), GetSumPoint(centeringWP, centeringE, -centeringT1));
                    Line cLine03 = new Line(GetSumPoint(centeringWP, 0, -centeringA + centeringT1), GetSumPoint(centeringWP, centeringE, -centeringA + centeringT1));
                    Line cLine04 = new Line(GetSumPoint(centeringWP, 0, -centeringA), GetSumPoint(centeringWP, centeringE, -centeringA));

                    Line cLineV01 = new Line(GetSumPoint(centeringWP, 0, 0), GetSumPoint(centeringWP, 0, -centeringT1));
                    Line cLineV02 = new Line(GetSumPoint(centeringWP, centeringC + centeringD, 0), GetSumPoint(centeringWP, centeringC + centeringD, -centeringT1));
                    Line cLineV03 = new Line(GetSumPoint(centeringWP, 0, -centeringA + centeringT1), GetSumPoint(centeringWP, 0, -centeringA));
                    Line cLineV04 = new Line(GetSumPoint(centeringWP, centeringC + centeringD, -centeringA + centeringT1), GetSumPoint(centeringWP, centeringC + centeringD, -centeringA));

                    Line cLineLongV01 = new Line(GetSumPoint(centeringWP, centeringC, -centeringT1), GetSumPoint(centeringWP, centeringC, -centeringA + centeringT1));
                    Line cLineLongV02 = new Line(GetSumPoint(centeringWP, centeringC + centeringT2, -centeringT1), GetSumPoint(centeringWP, centeringC + centeringT2, -centeringA + centeringT1));

                    customBlockList.AddRange(new Line[] { cLine01, cLine02, cLine03, cLine04, cLineV01, cLineV02, cLineV03, cLineV04, cLineLongV01, cLineLongV02 });
                    #endregion
                    #region Clip Center Side
                    StructureClipCenteringSideModel centeringSideClip = assemblyData.StructureClipCenteringSideOutput[firstIndex];
                    double cClipA = valueService.GetDoubleValue(centeringSideClip.A);
                    double cClipB = valueService.GetDoubleValue(centeringSideClip.B);
                    double cClipC = valueService.GetDoubleValue(centeringSideClip.C);
                    double cClipD = valueService.GetDoubleValue(centeringSideClip.D);
                    double cClipHoleQty = valueService.GetDoubleValue(centeringSideClip.HoleQty);
                    double cClipA1 = valueService.GetDoubleValue(centeringSideClip.A1);
                    double cClipB1 = valueService.GetDoubleValue(centeringSideClip.B1);
                    double cClipC1 = valueService.GetDoubleValue(centeringSideClip.C1);
                    double cClipD1 = valueService.GetDoubleValue(centeringSideClip.D1);
                    double cClipE1 = valueService.GetDoubleValue(centeringSideClip.E1);
                    double cClipF1 = valueService.GetDoubleValue(centeringSideClip.F1);

                    // Gap : A1 : 7
                    double centeringGapVertiY = valueService.GetHypotenuseByWidth(roofSlopeDegree, cClipA1);
                    double centeringGapHeightY = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cClipA1);
                    double centeringGapWidthX = valueService.GetOppositeByHypotenuse(roofSlopeDegree, cClipA1);


                    double centeringclipB1MiddelX = valueService.GetAdjacentByHeight(roofSlopeDegree, centeringT1);
                    double centeringclipGapX = valueService.GetAdjacentByHeight(roofSlopeDegree, cClipA1);
                    double centeringclipGapXX = valueService.GetHypotenuseByWidth(roofSlopeDegree, centeringclipGapX);
                    CDPoint centeringClipRightReal = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, centerRadius + centeringE + centeringclipB1MiddelX, ref refPoint, ref curPoint);
                    Point3D centeringClipRightRealPoint = GetSumPoint(centeringClipRightReal, centeringclipGapXX, 0);


                    double firstWidthX = valueService.GetOppositeByWidth(roofSlopeDegree, cClipC);
                    double middleWidth = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cClipD1 + cClipE1 - firstWidthX);
                    CDPoint centeringClipLeft = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, centerRadius + centeringE + middleWidth + cClipC1 + cClipB1, ref refPoint, ref curPoint);
                    Point3D centeringClipLeftPoint = GetSumPoint(centeringClipLeft, 0, -centeringGapVertiY);


                    Line slopeLine01 = new Line(centeringClipRightRealPoint, centeringClipLeftPoint);
                    Line clipLine01 = new Line(centeringClipRightRealPoint, GetSumPoint(centeringWP, +centeringC - cClipF1, -centeringT1));
                    Line clipLine02 = new Line(GetSumPoint(centeringWP, +centeringC - cClipF1, -centeringT1), GetSumPoint(centeringWP, +centeringC, -centeringT1 - cClipF1));
                    Line clipLine03 = new Line(GetSumPoint(centeringWP, +centeringC, -centeringA + centeringT1 + cClipF1), GetSumPoint(centeringWP, +centeringC, -centeringT1 - cClipF1));
                    Line clipLine04 = new Line(GetSumPoint(centeringWP, +centeringC, -centeringA + centeringT1 + cClipF1), GetSumPoint(centeringWP, +centeringC - cClipF1, -centeringA + centeringT1));

                    Point3D centeringClipLeftDownPoint = new Point3D(centeringClipLeftPoint.X, GetSumPoint(centeringWP, 0, -centeringA + centeringT1).Y);
                    Line clipLine05 = new Line(centeringClipLeftDownPoint, GetSumPoint(centeringWP, +centeringC - cClipF1, -centeringA + centeringT1));
                    Line clipLine06 = new Line(centeringClipLeftDownPoint, centeringClipLeftPoint);
                    customBlockList.AddRange(new Line[] { slopeLine01, clipLine01, clipLine02, clipLine03, clipLine04, clipLine05, clipLine06 });

                    // B1 만큼 위치
                    centeringClipRight = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, centerRadius + centeringE + cClipB1, ref refPoint, ref curPoint);

                    if (cClipHoleQty == 2)
                    {
                        // 직각 방향 이동: 아래쪽
                        double clipSideHoleFirstSlope1 = cClipC;
                        double clipSideHoleFirstWidth1 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope1);//  clipSideHoleFirstSlope1 * Math.Cos(roofSlopeDegree);
                        double clipSideHoleFirstHeight1 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope1);// clipSideHoleFirstSlope1 * Math.Sin(roofSlopeDegree);
                        Point3D clipSideHoleFirstPoint1 = GetSumPoint(centeringClipRight, clipSideHoleFirstWidth1, -clipSideHoleFirstHeight1);
                        double clipSideHoleFirstSlope2 = cClipE1;
                        double clipSideHoleFirstWidth2 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope2); //clipSideHoleFirstSlope2 * Math.Cos(roofSlopeDegree);
                        double clipSideHoleFirstHeight2 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope2); //clipSideHoleFirstSlope2 * Math.Sin(roofSlopeDegree);
                        Point3D clipSideHoleFirstPoint2 = GetSumPoint(clipSideHoleFirstPoint1, -clipSideHoleFirstWidth2, -clipSideHoleFirstHeight2);
                        double clipSideHoleFirstSlope3 = cClipD1;
                        double clipSideHoleFirstWidth3 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope3); //clipSideHoleFirstSlope3 * Math.Cos(roofSlopeDegree);
                        double clipSideHoleFirstHeight3 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope3); //clipSideHoleFirstSlope3 * Math.Sin(roofSlopeDegree);
                        Point3D clipSideHoleFirstPoint3 = GetSumPoint(clipSideHoleFirstPoint2, -clipSideHoleFirstWidth3, -clipSideHoleFirstHeight3);

                        CompositeCurve clipSideBoltFirst1 = GetBoltHoleHorizontal(clipSideHoleFirstPoint2, boltHoleWidth, boltHoleHeight);
                        clipSideBoltFirst1.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleFirstPoint2);
                        CompositeCurve clipSideBoltFirst2 = GetBoltHoleHorizontal(clipSideHoleFirstPoint3, boltHoleWidth, boltHoleHeight);
                        clipSideBoltFirst2.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleFirstPoint3);
                        customBlockList.AddRange(new CompositeCurve[] { clipSideBoltFirst1, clipSideBoltFirst2 });

                        Circle ClipSideBoltFirstCicle1 = new Circle(clipSideHoleFirstPoint2, boltHoleHeight / 2);
                        Circle ClipSideBoltFirstCicle2 = new Circle(clipSideHoleFirstPoint3, boltHoleHeight / 2);
                        customBlockList.AddRange(new Circle[] { ClipSideBoltFirstCicle1, ClipSideBoltFirstCicle2 });
                    }
                    else if (cClipHoleQty == 4)
                    {
                        // 직각 방향 이동: 아래쪽
                        double clipSideHoleFirstSlope1 = cClipC;
                        double clipSideHoleFirstWidth1 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope1);//  clipSideHoleFirstSlope1 * Math.Cos(roofSlopeDegree);
                        double clipSideHoleFirstHeight1 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope1);// clipSideHoleFirstSlope1 * Math.Sin(roofSlopeDegree);
                        Point3D clipSideHoleFirstPoint1 = GetSumPoint(centeringClipRight, clipSideHoleFirstWidth1, -clipSideHoleFirstHeight1);
                        double clipSideHoleFirstSlope2 = cClipE1;
                        double clipSideHoleFirstWidth2 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope2); //clipSideHoleFirstSlope2 * Math.Cos(roofSlopeDegree);
                        double clipSideHoleFirstHeight2 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope2); //clipSideHoleFirstSlope2 * Math.Sin(roofSlopeDegree);
                        Point3D clipSideHoleFirstPoint2 = GetSumPoint(clipSideHoleFirstPoint1, -clipSideHoleFirstWidth2, -clipSideHoleFirstHeight2);
                        double clipSideHoleFirstSlope3 = cClipD1;
                        double clipSideHoleFirstWidth3 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope3); //clipSideHoleFirstSlope3 * Math.Cos(roofSlopeDegree);
                        double clipSideHoleFirstHeight3 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope3); //clipSideHoleFirstSlope3 * Math.Sin(roofSlopeDegree);
                        Point3D clipSideHoleFirstPoint3 = GetSumPoint(clipSideHoleFirstPoint2, -clipSideHoleFirstWidth3, -clipSideHoleFirstHeight3);

                        CompositeCurve clipSideBoltFirst1 = GetBoltHoleHorizontal(clipSideHoleFirstPoint2, boltHoleWidth, boltHoleHeight);
                        clipSideBoltFirst1.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleFirstPoint2);
                        CompositeCurve clipSideBoltFirst2 = GetBoltHoleHorizontal(clipSideHoleFirstPoint3, boltHoleWidth, boltHoleHeight);
                        clipSideBoltFirst2.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleFirstPoint3);
                        customBlockList.AddRange(new CompositeCurve[] { clipSideBoltFirst1, clipSideBoltFirst2 });

                        Circle ClipSideBoltFirstCicle1 = new Circle(clipSideHoleFirstPoint2, boltHoleHeight / 2);
                        Circle ClipSideBoltFirstCicle2 = new Circle(clipSideHoleFirstPoint3, boltHoleHeight / 2);
                        customBlockList.AddRange(new Circle[] { ClipSideBoltFirstCicle1, ClipSideBoltFirstCicle2 });

                        // 직각 방향 이동: 아래쪽
                        Point3D clipSideHoleFirstPoint11 = GetSumPoint(centeringClipRight, +valueService.GetOppositeByHypotenuse(roofSlopeDegree, cClipC + cClipD),
                                                                                                -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cClipC + cClipD));
                        Point3D clipSideHoleFirstPoint21 = GetSumPoint(clipSideHoleFirstPoint11, -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cClipE1),
                                                                                                 -valueService.GetOppositeByHypotenuse(roofSlopeDegree, cClipE1));
                        Point3D clipSideHoleFirstPoint31 = GetSumPoint(clipSideHoleFirstPoint21, -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cClipD1),
                                                                                                 -valueService.GetOppositeByHypotenuse(roofSlopeDegree, cClipD1));

                        CompositeCurve clipSideBoltFirst11 = GetBoltHoleHorizontal(clipSideHoleFirstPoint21, boltHoleWidth, boltHoleHeight);
                        clipSideBoltFirst11.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleFirstPoint21);
                        CompositeCurve clipSideBoltFirst21 = GetBoltHoleHorizontal(clipSideHoleFirstPoint31, boltHoleWidth, boltHoleHeight);
                        clipSideBoltFirst21.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleFirstPoint3);
                        customBlockList.AddRange(new CompositeCurve[] { clipSideBoltFirst11, clipSideBoltFirst21 });

                        Circle ClipSideBoltFirstCicle11 = new Circle(clipSideHoleFirstPoint21, boltHoleHeight / 2);
                        Circle ClipSideBoltFirstCicle21 = new Circle(clipSideHoleFirstPoint31, boltHoleHeight / 2);
                        customBlockList.AddRange(new Circle[] { ClipSideBoltFirstCicle11, ClipSideBoltFirstCicle21 });
                    }
                    #endregion
                }
                else
                {
                    //external
                    #region CenterRing

                    CDPoint centeringWP = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofUp, centerRadius + centeringIDHalf, ref refPoint, ref curPoint);

                    Line cLine01 = new Line(GetSumPoint(centeringWP, -centeringB, centeringA ), GetSumPoint(centeringWP, +centeringIDHalf, centeringA ));
                    Line cLine02 = new Line(GetSumPoint(centeringWP, -centeringB, centeringA - centeringT1), GetSumPoint(centeringWP, +centeringIDHalf, centeringA -centeringT1));
                    Line cLine03 = new Line(GetSumPoint(centeringWP, -centeringB, centeringA ), GetSumPoint(centeringWP, -centeringB, centeringA - centeringT1));
                    Line cLine04 = new Line(GetSumPoint(centeringWP, +centeringC, centeringA ), GetSumPoint(centeringWP, +centeringC, centeringA - centeringT1));

                    double centeringPadVertiY = valueService.GetHypotenuseByWidth(roofSlopeDegree, centeringT1);
                    double centeringPadHeightY = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, centeringT1);
                    double centeringPadWidthX = valueService.GetOppositeByHypotenuse(roofSlopeDegree, centeringT1);

                    double centeringVertiHeightY = valueService.GetOppositeByWidth(roofSlopeDegree, centeringT2);


                    Line cLineLongV01 = new Line(GetSumPoint(centeringWP, 0, centeringPadVertiY), GetSumPoint(centeringWP, 0, centeringA - centeringT1));
                    Line cLineLongV02 = new Line(GetSumPoint(centeringWP, centeringT2, centeringVertiHeightY + centeringPadVertiY), GetSumPoint(centeringWP, centeringT2, centeringA - centeringT1));

                    double centeringPadBottomBX = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, centeringB);
                    double centeringPadBottomCX = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, centeringC);

                    CDPoint centeringPadBottonLeft = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofUp, centerRadius + centeringIDHalf + centeringPadBottomBX, ref refPoint, ref curPoint);
                    CDPoint centeringPadBottonRight = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofUp, centerRadius + centeringIDHalf - centeringPadBottomCX, ref refPoint, ref curPoint);

                    Line cLineV01 = new Line(GetSumPoint(centeringPadBottonLeft, -centeringPadWidthX, centeringPadHeightY), GetSumPoint(centeringPadBottonRight, -centeringPadWidthX, centeringPadHeightY));
                    Line cLineV02 = new Line(GetSumPoint(centeringPadBottonRight, 0, 0), GetSumPoint(centeringPadBottonRight, -centeringPadWidthX, centeringPadHeightY));
                    Line cLineV03 = new Line(GetSumPoint(centeringPadBottonLeft, 0, 0), GetSumPoint(centeringPadBottonLeft,-centeringPadWidthX, centeringPadHeightY));
                    Line cLineV04 = new Line(GetSumPoint(centeringPadBottonLeft, 0,0), GetSumPoint(centeringPadBottonRight, 0,0));


                    customBlockList.AddRange(new Line[] { cLine01, cLine02, cLine03, cLine04, cLineV01, cLineV02, cLineV03, cLineV04, cLineLongV01, cLineLongV02 });
                    #endregion

                    #region Rafter : Centering : external
                    StructureCenteringRafterModel centeringRafter = assemblyData.StructureCenteringRaterOutput[firstIndex];
                    double cRafterA = valueService.GetDoubleValue(centeringRafter.A);
                    double cRafterA1 = valueService.GetDoubleValue(centeringRafter.A1);
                    double cRafterB1 = valueService.GetDoubleValue(centeringRafter.B1);

                    CDPoint centeringRafterLeftDown = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointLeftRoofUp, ref refPoint, ref curPoint);
                    CDPoint centeringRafterLeftUP = GetSumCDPoint(centeringRafterLeftDown, -valueService.GetOppositeByHypotenuse(roofSlopeDegree, cRafterA), valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cRafterA));
                    CDPoint centeringRafterRightDown = GetSumCDPoint(centeringWP, -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cRafterA1 + centeringB), -valueService.GetOppositeByHypotenuse(roofSlopeDegree, cRafterA1 + centeringB));
                    CDPoint centeringRafterRightUP = GetSumCDPoint(workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofUp, centerRadius + centeringE + cRafterA1, ref refPoint, ref curPoint),
                                                                   0, valueService.GetHypotenuseByWidth(roofSlopeDegree,cRafterA));
                    CDPoint centeringRafterRightRightUp = GetSumCDPoint(centeringWP, -centeringB-cRafterA1, centeringA - centeringT1 - cRafterB1);
                    CDPoint centeringRafterRightRightRightUp = GetSumCDPoint(centeringWP, 0, centeringA - centeringT1 - cRafterB1);

                    double centeringRafeterBottomVerti = valueService.GetHypotenuseByWidth(roofSlopeDegree, centeringT1) + valueService.GetHypotenuseByWidth(roofSlopeDegree, cRafterB1);
                    double centeringRafeterBottomVerti2 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, centeringRafeterBottomVerti);
                    CDPoint centeringRafterRightRightRightDown = GetSumCDPoint(centeringWP,0, centeringRafeterBottomVerti);
                    CDPoint centeringRafterRightRightDown = GetSumCDPoint(centeringRafterRightDown, -valueService.GetOppositeByHypotenuse(roofSlopeDegree, centeringRafeterBottomVerti2) ,
                                                                                                             +valueService.GetAdjacentByHypotenuse(roofSlopeDegree, centeringRafeterBottomVerti2));
                    Line rLine01 = new Line(GetSumPoint(centeringRafterLeftDown,0,0),GetSumPoint(centeringRafterLeftUP,0,0));
                    Line rLine02 = new Line(GetSumPoint(centeringRafterLeftUP, 0, 0), GetSumPoint(centeringRafterRightUP, 0, 0));
                    Line rLine03 = new Line(GetSumPoint(centeringRafterRightUP, 0, 0), GetSumPoint(centeringRafterRightRightUp, 0, 0));
                    Line rLine04 = new Line(GetSumPoint(centeringRafterRightRightUp, 0, 0), GetSumPoint(centeringRafterRightRightRightUp, 0, 0));
                    Line rLine05 = new Line(GetSumPoint(centeringRafterRightRightRightUp, 0, 0), GetSumPoint(centeringRafterRightRightRightDown, 0, 0));
                    Line rLine06 = new Line(GetSumPoint(centeringRafterRightRightRightDown, 0, 0), GetSumPoint(centeringRafterRightRightDown, 0, 0));
                    Line rLine07 = new Line(GetSumPoint(centeringRafterRightRightDown, 0, 0), GetSumPoint(centeringRafterRightDown, 0, 0));
                    Line rLine08 = new Line(GetSumPoint(centeringRafterRightDown, 0, 0), GetSumPoint(centeringRafterLeftDown, 0, 0));

                    customBlockList.AddRange(new Line[] { rLine01, rLine02, rLine03, rLine04, rLine05, rLine06, rLine07, rLine08 });
                    #endregion
                }

            }


            if (!(StructureDivService.columnType == "centering" && StructureDivService.centeringInEx == "external"))
            {
                // Clip Shell Side : Colum type, Centering Type
                #region Support Clip Shell Side
                StructureColumnClipShellSideModel eachSupportClip = assemblyData.StructureColumnClipShellSideOutput[firstIndex];
                double scA = valueService.GetDoubleValue(eachSupportClip.A);
                double scB = valueService.GetDoubleValue(eachSupportClip.B);
                double scC = valueService.GetDoubleValue(eachSupportClip.C);
                double scD = valueService.GetDoubleValue(eachSupportClip.D);
                double scE = valueService.GetDoubleValue(eachSupportClip.E);
                double scF = valueService.GetDoubleValue(eachSupportClip.F);
                double scG = valueService.GetDoubleValue(eachSupportClip.G);

                StructureColumnRafterModel lastRafter = assemblyData.StructureColumnRafterOutput[assemblyData.StructureColumnRafterOutput.Count - 1];
                double lrA = valueService.GetDoubleValue(lastRafter.A);
                double lrB = valueService.GetDoubleValue(lastRafter.B);
                double lrC = valueService.GetDoubleValue(lastRafter.C);
                double lrD = valueService.GetDoubleValue(lastRafter.D);
                double lrE = valueService.GetDoubleValue(lastRafter.E);
                double lrBoltHoleOnShell = valueService.GetDoubleValue(lastRafter.BoltHoleOnShell);
                double lrBoltHoleOnColumn = valueService.GetDoubleValue(lastRafter.BoltHoleOnColumn);
                double lrBoltHoleOnCenter = valueService.GetDoubleValue(lastRafter.BoltHoleOnCenter);
                double lrBoltHoleDia = valueService.GetDoubleValue(lastRafter.BoltHoleDia);

                int refFirstIndex = 0;

                string selSizeTankHeight = assemblyData.GeneralDesignData[refFirstIndex].SizeTankHeight;
                Point3D leftTankTop = GetSumPoint(refPoint, 0, valueService.GetDoubleValue(selSizeTankHeight));
                int maxCourse = valueService.GetIntValue(assemblyData.ShellInput[refFirstIndex].CourseCount) - 1;

                // Rafter : Bolt
                double rafterSideBoltWidth = valueService.GetDoubleValue(eachSupportClip.F1);
                double rafterSideBoltGap = valueService.GetDoubleValue(eachSupportClip.G1);

                double shellClipTopGap = valueService.GetDoubleValue(eachSupportClip.B1);
                double shellClipPadWidth = valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].MinThk);
                double shellClipPadInto = valueService.GetDoubleValue(eachSupportClip.C1);
                double shellClipTriTopGap = valueService.GetDoubleValue(eachSupportClip.D1);
                double shellClipTriBottomGap = valueService.GetDoubleValue(eachSupportClip.H1);
                double shellClipTriTopEndGap = valueService.GetDoubleValue(eachSupportClip.E1);

                Line shellClipPad1 = new Line(GetSumPoint(leftTankTop, 0, -shellClipTopGap), GetSumPoint(leftTankTop, 0, -shellClipTopGap - scF));
                Line shellClipPad2 = new Line(GetSumPoint(leftTankTop, shellClipPadWidth, -shellClipTopGap), GetSumPoint(leftTankTop, shellClipPadWidth, -shellClipTopGap - scF));
                Line shellClipPad3 = new Line(GetSumPoint(leftTankTop, 0, -shellClipTopGap), GetSumPoint(leftTankTop, shellClipPadWidth, -shellClipTopGap));
                Line shellClipPad4 = new Line(GetSumPoint(leftTankTop, 0, -shellClipTopGap - scF), GetSumPoint(leftTankTop, shellClipPadWidth, -shellClipTopGap - scF));
                customBlockList.AddRange(new Line[] { shellClipPad1, shellClipPad2, shellClipPad3, shellClipPad4 });

                //CDPoint leftTankRoofTop = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftRoofDown, ref refPoint, ref curPoint);


                double leftTankRoofTopGapWidth = valueService.GetDoubleValue(eachSupportClip.A1);
                // Gap : Slope
                double leftTankRoofTopGapY = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, leftTankRoofTopGapWidth);
                double leftTankRoofTopGapX = valueService.GetOppositeByHypotenuse(roofSlopeDegree, leftTankRoofTopGapWidth);
                // 수직 방향 이동 : 아래쪽
                CDPoint leftTankRoofTop1 = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftRoofDown, shellClipTriTopGap, ref refPoint, ref curPoint);
                leftTankRoofTop1.X = leftTankRoofTop1.X + leftTankRoofTopGapX;
                leftTankRoofTop1.Y = leftTankRoofTop1.Y - leftTankRoofTopGapY;

                // 중간 : Slope 길이 -> Width로 변환
                double shellBoltWidth = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, rafterSideBoltWidth + rafterSideBoltGap);

                // 평행 방향 이동 : 아래쪽
                CDPoint leftTankRoofTop2 = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftRoofDown, shellClipTriTopGap + shellBoltWidth + shellClipTriTopEndGap, ref refPoint, ref curPoint);
                double leftTankRoofTopGapHeight = valueService.GetHypotenuseByWidth(roofSlopeDegree, leftTankRoofTopGapWidth);
                leftTankRoofTop2.Y = leftTankRoofTop2.Y - leftTankRoofTopGapHeight;

                // Compressionring
                double t1 = valueService.GetDoubleValue(assemblyData.RoofInput[firstIndex].ComRingThicknessT1);
                double thickVertiY = valueService.GetHypotenuseByWidth(roofSlopeDegree, t1);
                double thickneesY = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, t1);
                double thickneesX = valueService.GetOppositeByHypotenuse(roofSlopeDegree, t1);
                if (StructureDivService.topAngleType == "compressionring")
                {
                    scB = scA - t1;
                    scD = scB / 2;
                    scE = scD - leftTankRoofTopGapWidth;

                    //기준 포인트
                    leftTankRoofTop1.Y = leftTankRoofTop1.Y - thickVertiY;
                    leftTankRoofTop2.Y = leftTankRoofTop2.Y - thickVertiY;

                    //Rafter
                    lrA = lrA - t1;
                }

                Line shellClipTri1 = new Line(GetSumPoint(leftTankTop, shellClipPadWidth, -shellClipTopGap - shellClipPadInto), GetSumPoint(leftTankRoofTop1, 0, 0));
                Line shellClipTri2 = new Line(GetSumPoint(leftTankRoofTop1, 0, 0), GetSumPoint(leftTankRoofTop2, 0, 0));
                Line shellClipTri3 = new Line(GetSumPoint(leftTankRoofTop2, 0, 0), GetSumPoint(leftTankRoofTop2, 0, -scC));
                Line shellClipTri4 = new Line(GetSumPoint(leftTankRoofTop2, 0, -scC), GetSumPoint(leftTankTop, shellClipPadWidth + shellClipTriBottomGap, -shellClipTopGap - scF + shellClipPadInto));
                Line shellClipTri5 = new Line(GetSumPoint(leftTankTop, shellClipPadWidth, -shellClipTopGap - scF + shellClipPadInto), GetSumPoint(leftTankTop, shellClipPadWidth + shellClipTriBottomGap, -shellClipTopGap - scF + shellClipPadInto));
                customBlockList.AddRange(new Line[] { shellClipTri1, shellClipTri2, shellClipTri3, shellClipTri4, shellClipTri5 });


                // Clip Shell Side : Bolt Hole
                if (lrBoltHoleOnShell == 2)
                {
                    // 직각 방향 이동: 아래쪽
                    double clipSideHoleFirstSlope1 = lrA / 2;
                    double clipSideHoleFirstWidth1 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope1);//  clipSideHoleFirstSlope1 * Math.Cos(roofSlopeDegree);
                    double clipSideHoleFirstHeight1 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope1);// clipSideHoleFirstSlope1 * Math.Sin(roofSlopeDegree);
                    Point3D clipSideHoleFirstPoint1 = GetSumPoint(leftTankRoofTop1, clipSideHoleFirstWidth1, -clipSideHoleFirstHeight1);
                    double clipSideHoleFirstSlope2 = rafterSideBoltWidth;
                    double clipSideHoleFirstWidth2 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope2); //clipSideHoleFirstSlope2 * Math.Cos(roofSlopeDegree);
                    double clipSideHoleFirstHeight2 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope2); //clipSideHoleFirstSlope2 * Math.Sin(roofSlopeDegree);
                    Point3D clipSideHoleFirstPoint2 = GetSumPoint(clipSideHoleFirstPoint1, clipSideHoleFirstWidth2, +clipSideHoleFirstHeight2);
                    double clipSideHoleFirstSlope3 = rafterSideBoltGap;
                    double clipSideHoleFirstWidth3 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope3); //clipSideHoleFirstSlope3 * Math.Cos(roofSlopeDegree);
                    double clipSideHoleFirstHeight3 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope3); //clipSideHoleFirstSlope3 * Math.Sin(roofSlopeDegree);
                    Point3D clipSideHoleFirstPoint3 = GetSumPoint(clipSideHoleFirstPoint2, clipSideHoleFirstWidth3, +clipSideHoleFirstHeight3);

                    CompositeCurve clipSideBoltFirst1 = GetBoltHoleHorizontal(clipSideHoleFirstPoint2, boltHoleWidth, boltHoleHeight);
                    clipSideBoltFirst1.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleFirstPoint2);
                    CompositeCurve clipSideBoltFirst2 = GetBoltHoleHorizontal(clipSideHoleFirstPoint3, boltHoleWidth, boltHoleHeight);
                    clipSideBoltFirst2.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleFirstPoint3);
                    customBlockList.AddRange(new CompositeCurve[] { clipSideBoltFirst1, clipSideBoltFirst2 });

                    Circle ClipSideBoltFirstCicle1 = new Circle(clipSideHoleFirstPoint2, boltHoleHeight / 2);
                    Circle ClipSideBoltFirstCicle2 = new Circle(clipSideHoleFirstPoint3, boltHoleHeight / 2);
                    customBlockList.AddRange(new Circle[] { ClipSideBoltFirstCicle1, ClipSideBoltFirstCicle2 });
                }
                else if (lrBoltHoleOnShell == 4)
                {
                    // 직각 방향 이동: 아래쪽
                    double clipSideHoleSecondSlope1 = lrD;
                    double clipSideHoleSecondWidth1 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleSecondSlope1);// clipSideHoleSecondSlope1 * Math.Cos(roofSlopeDegree);
                    double clipSideHoleSecondHeight1 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleSecondSlope1);// clipSideHoleSecondSlope1 * Math.Sin(roofSlopeDegree);
                    Point3D clipSideHoleSecondPoint1 = GetSumPoint(leftTankRoofTop1, clipSideHoleSecondWidth1, -clipSideHoleSecondHeight1);
                    double clipSideHoleSecondlope2 = rafterSideBoltWidth;
                    double clipSideHoleSecondWidth2 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleSecondlope2); //clipSideHoleSecondlope2* Math.Cos(roofSlopeDegree);
                    double clipSideHoleSecondHeight2 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleSecondlope2); //clipSideHoleSecondlope2* Math.Sin(roofSlopeDegree);
                    Point3D clipSideHoleSecondPoint2 = GetSumPoint(clipSideHoleSecondPoint1, clipSideHoleSecondWidth2, +clipSideHoleSecondHeight2);
                    double clipSideHoleSecondSlope3 = rafterSideBoltGap;
                    double clipSideHoleSecondWidth3 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleSecondSlope3); //clipSideHoleSecondSlope3 * Math.Cos(roofSlopeDegree);
                    double clipSideHoleSecondHeight3 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleSecondSlope3); //clipSideHoleSecondSlope3 * Math.Sin(roofSlopeDegree);
                    Point3D clipSideHoleSecondPoint3 = GetSumPoint(clipSideHoleSecondPoint2, clipSideHoleSecondWidth3, +clipSideHoleSecondHeight3);

                    CompositeCurve clipSideBoltSecond1 = GetBoltHoleHorizontal(clipSideHoleSecondPoint2, boltHoleWidth, boltHoleHeight);
                    clipSideBoltSecond1.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleSecondPoint2);
                    CompositeCurve clipSideBoltSecond2 = GetBoltHoleHorizontal(clipSideHoleSecondPoint3, boltHoleWidth, boltHoleHeight);
                    clipSideBoltSecond2.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleSecondPoint3);
                    customBlockList.AddRange(new CompositeCurve[] { clipSideBoltSecond1, clipSideBoltSecond2 });

                    // 직각 방향 이동: 아래쪽
                    double clipSideHoleSecondSlope4 = lrD + lrE;
                    double clipSideHoleSecondWidth4 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleSecondSlope4); //clipSideHoleSecondSlope4 * Math.Cos(roofSlopeDegree);
                    double clipSideHoleSecondHeight4 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleSecondSlope4);// clipSideHoleSecondSlope4 * Math.Sin(roofSlopeDegree);
                    Point3D clipSideHoleSecondPoint4 = GetSumPoint(leftTankRoofTop1, clipSideHoleSecondWidth4, -clipSideHoleSecondHeight4);
                    double clipSideHoleSecondlope5 = rafterSideBoltWidth;
                    double clipSideHoleSecondWidth5 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleSecondlope5); //clipSideHoleSecondlope5 * Math.Cos(roofSlopeDegree);
                    double clipSideHoleSecondHeight5 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleSecondlope5); //clipSideHoleSecondlope5 * Math.Sin(roofSlopeDegree);
                    Point3D clipSideHoleSecondPoint5 = GetSumPoint(clipSideHoleSecondPoint4, clipSideHoleSecondWidth5, +clipSideHoleSecondHeight5);
                    double clipSideHoleSecondSlope6 = rafterSideBoltGap;
                    double clipSideHoleSecondWidth6 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleSecondSlope6); //clipSideHoleSecondSlope6 * Math.Cos(roofSlopeDegree);
                    double clipSideHoleSecondHeight6 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleSecondSlope6); //clipSideHoleSecondSlope6 * Math.Sin(roofSlopeDegree);
                    Point3D clipSideHoleSecondPoint6 = GetSumPoint(clipSideHoleSecondPoint5, clipSideHoleSecondWidth6, +clipSideHoleSecondHeight6);

                    CompositeCurve clipSideBoltSecond3 = GetBoltHoleHorizontal(clipSideHoleSecondPoint5, boltHoleWidth, boltHoleHeight);
                    clipSideBoltSecond3.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleSecondPoint5);
                    CompositeCurve clipSideBoltSecond4 = GetBoltHoleHorizontal(clipSideHoleSecondPoint6, boltHoleWidth, boltHoleHeight);
                    clipSideBoltSecond4.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleSecondPoint6);
                    customBlockList.AddRange(new CompositeCurve[] { clipSideBoltSecond3, clipSideBoltSecond4 });


                    Circle ClipSideBoltSecondCicle1 = new Circle(clipSideHoleSecondPoint2, boltHoleHeight / 2);
                    Circle ClipSideBoltSecondCicle2 = new Circle(clipSideHoleSecondPoint3, boltHoleHeight / 2);
                    Circle ClipSideBoltSecondCicle3 = new Circle(clipSideHoleSecondPoint5, boltHoleHeight / 2);
                    Circle ClipSideBoltsecondCicle4 = new Circle(clipSideHoleSecondPoint6, boltHoleHeight / 2);
                    customBlockList.AddRange(new Circle[] { ClipSideBoltSecondCicle1, ClipSideBoltSecondCicle2, ClipSideBoltSecondCicle3, ClipSideBoltsecondCicle4 });
                }

                    #endregion

            
                if (StructureDivService.columnType == "column")
                {
                    #region Rafter

                    CDPoint rafterEndPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftRoofDown, shellClipTriTopGap, ref refPoint, ref curPoint);

                    // 수직방향 이동 : 오른쪽으로
                    CDPoint rafterStartPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, centerRadius + centerLeftWidthHalf, ref refPoint, ref curPoint);
                    double rafterStartPointtSlope = rafterSideBoltGap / 2 + rafterSideBoltWidth;
                    double rafterStartPointtWidth = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, rafterStartPointtSlope);
                    double rafterStartPointtHeight = valueService.GetOppositeByHypotenuse(roofSlopeDegree, rafterStartPointtSlope);

                    Point3D rafterStartPoint2 = GetSumPoint(rafterStartPoint, rafterStartPointtWidth, rafterStartPointtHeight);


                    Point3D rafterTempColumnPoint = new Point3D();
                    for (int i = 0; i < assemblyData.StructureColumnRafterOutput.Count; i++)
                    {
                        Point3D rafterStartColumnPoint = new Point3D();
                        if (i == 0)
                        {
                            double rafterWidthHalfHalf = valueService.GetDoubleValue(assemblyData.StructureColumnRafterOutput[i].A) / 2;
                            double rafterO = valueService.GetOppositeByWidth(roofSlopeDegree, rafterWidthHalfHalf);
                            double rafterXXX = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, rafterO);
                            double rafterYYY = valueService.GetOppositeByHypotenuse(roofSlopeDegree, rafterO);
                            rafterStartColumnPoint.X = rafterStartPoint2.X - rafterXXX;
                            rafterStartColumnPoint.Y = rafterStartPoint2.Y - rafterYYY;
                        }
                        else
                        {
                            rafterStartColumnPoint.X = rafterTempColumnPoint.X;
                            rafterStartColumnPoint.Y = rafterTempColumnPoint.Y;
                        }

                        double rafterEachRadius = valueService.GetDoubleValue(assemblyData.StructureRafterInput[i].RafterInRadius);
                        CDPoint rafterCurrentColumnPointTemp = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, rafterEachRadius, ref refPoint, ref curPoint);
                        Point3D rafterCurrentColumnPoint = GetSumPoint(rafterCurrentColumnPointTemp, 0, 0);

                        if (i == assemblyData.StructureColumnRafterOutput.Count - 1)
                        {
                            //rafterStartColumnPoint.X = rafterCurrentColumnPoint.X;
                            //rafterStartColumnPoint.Y = rafterCurrentColumnPoint.Y;
                            rafterCurrentColumnPoint.X = rafterEndPoint.X;
                            rafterCurrentColumnPoint.Y = rafterEndPoint.Y;

                        }

                        // Draw Structure

                        // 직각 방향 이동 : 아래로
                        double rafterWidth = valueService.GetDoubleValue(assemblyData.StructureColumnRafterOutput[i].A);
                        double rafterStartSlope = rafterWidth;
                        double rafterStartWidth = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, rafterStartSlope);
                        double rafterStartHeight = valueService.GetOppositeByHypotenuse(roofSlopeDegree, rafterStartSlope);
                        Point3D rafterStartColumnPoint2 = GetSumPoint(rafterStartColumnPoint, rafterStartHeight, -rafterStartWidth);

                        double rafterCurrentSlope = rafterWidth;
                        double rafterCurrentWidth = rafterStartWidth;
                        double rafterCurrentHeight = rafterStartHeight;
                        Point3D rafterCurrentColumnPoint2 = GetSumPoint(rafterCurrentColumnPoint, rafterCurrentHeight, -rafterCurrentWidth);



                        Line rafterSquare1 = new Line(rafterStartColumnPoint, rafterCurrentColumnPoint);
                        Line rafterSquare2 = new Line(rafterStartColumnPoint, rafterStartColumnPoint2);
                        Line rafterSquare3 = new Line(rafterCurrentColumnPoint, rafterCurrentColumnPoint2);
                        Line rafterSquare4 = new Line(rafterStartColumnPoint2, rafterCurrentColumnPoint2);
                        if (StructureDivService.topAngleType == "compressionring")
                        {
                            if (i == assemblyData.StructureColumnRafterOutput.Count - 1)
                            {
                                double A = valueService.GetDoubleValue(assemblyData.RoofInput[firstIndex].ComRingOutsideProjectionA);
                                double B = valueService.GetDoubleValue(assemblyData.RoofInput[firstIndex].ComRingWidthB);
                                double compressWidth = B - A - valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].MinThk) + 50;// 50이 정해 짐
                                double comPressAdjacent = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, compressWidth);

                                CDPoint leftCompressPointCD = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftRoofDown, comPressAdjacent, ref refPoint, ref curPoint);
                                Point3D leftCompressPoint = new Point3D(leftCompressPointCD.X, leftCompressPointCD.Y);
                                Point3D leftCompressPoint2 = GetSumPoint(leftCompressPoint, +thickneesX, -thickneesY);

                                rafterCurrentColumnPoint.Y = rafterCurrentColumnPoint.Y - thickVertiY;

                                rafterSquare1 = new Line(rafterStartColumnPoint, leftCompressPoint);
                                Line rafterCompre1 = new Line(leftCompressPoint, leftCompressPoint2);
                                Line rafterCompre2 = new Line(rafterCurrentColumnPoint, leftCompressPoint2);
                                rafterSquare2 = new Line(rafterStartColumnPoint, rafterStartColumnPoint2);
                                rafterSquare3 = new Line(rafterCurrentColumnPoint, rafterCurrentColumnPoint2);
                                rafterSquare4 = new Line(rafterStartColumnPoint2, rafterCurrentColumnPoint2);

                                customBlockList.Add(rafterCompre1);
                                customBlockList.Add(rafterCompre2);
                            }


                        }

                        customBlockList.AddRange(new Line[] { rafterSquare1, rafterSquare2, rafterSquare3, rafterSquare4 });

                        // Center Line
                        double rafterWidthHalf = valueService.GetDoubleValue(assemblyData.StructureColumnRafterOutput[i].A) / 2;
                        double rafterStartSlope2 = rafterWidthHalf;
                        double rafterStartWidth2 = rafterStartWidth / 2;
                        double rafterStartHeight2 = rafterStartHeight / 2;
                        Point3D rafterStartColumnPoint3 = GetSumPoint(rafterStartColumnPoint, rafterStartHeight2, -rafterStartWidth2);

                        double rafterCurrentSlope2 = rafterWidthHalf;
                        double rafterCurrentWidth2 = rafterStartWidth / 2;
                        double rafterCurrentHeight2 = rafterStartHeight / 2;
                        Point3D rafterCurrentColumnPoint4 = GetSumPoint(rafterCurrentColumnPoint, rafterCurrentHeight2, -rafterCurrentWidth2);

                        Line rafterSquareCenter1 = new Line(rafterStartColumnPoint3, rafterCurrentColumnPoint4);
                        customBlockList.Add(rafterSquareCenter1);

                        // Current -> Start
                        rafterTempColumnPoint.X = rafterCurrentColumnPoint.X;
                        rafterTempColumnPoint.Y = rafterCurrentColumnPoint.Y;
                    }

                    #endregion
                }
                else if (StructureDivService.columnType == "centering")
                {
                    if (StructureDivService.centeringInEx == "internal")
                    {
                        #region Rafter : Centering
                        StructureCenteringRafterModel centeringRafter = assemblyData.StructureCenteringRaterOutput[firstIndex];
                        double cRafterA = valueService.GetDoubleValue(centeringRafter.A);
                        double cRafterB = valueService.GetDoubleValue(centeringRafter.B);
                        double cRafterC = valueService.GetDoubleValue(centeringRafter.C);
                        double cRafterD = valueService.GetDoubleValue(centeringRafter.D);
                        double cRafterHoleQty = valueService.GetDoubleValue(centeringRafter.HoleQty);
                        double cRafterA1 = valueService.GetDoubleValue(centeringRafter.A1);
                        double cRafterB1 = valueService.GetDoubleValue(centeringRafter.B1);
                        double cRafterC1 = valueService.GetDoubleValue(centeringRafter.C1);
                        double cRafterD1 = valueService.GetDoubleValue(centeringRafter.D1);
                        double cRafterE = valueService.GetDoubleValue(centeringRafter.E);



                        CDPoint leftCenteringRafterEndRef = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftRoofDown, shellClipTriTopGap, ref refPoint, ref curPoint);
                        Point3D leftCenteringRafterEnd01 = GetSumPoint(leftCenteringRafterEndRef, 0, 0);
                        Point3D leftCenteringRafterEnd02 = GetSumPoint(leftCenteringRafterEndRef, +valueService.GetOppositeByHypotenuse(roofSlopeDegree, cRafterB),
                                                                                                  -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cRafterB));
                        Point3D leftCenteringRafterEndMiddle = GetSumPoint(leftCenteringRafterEndRef, +valueService.GetOppositeByHypotenuse(roofSlopeDegree, cRafterB / 2),
                                                                                                      -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cRafterB / 2));

                        CDPoint rightCenterigRafterStartRef = (CDPoint)centeringClipRight.Clone();
                        Point3D rightCenteringRafterStart01 = GetSumPoint(rightCenterigRafterStartRef, 0, 0);
                        Point3D rightCenteringRafterStart02 = GetSumPoint(rightCenterigRafterStartRef, +valueService.GetOppositeByHypotenuse(roofSlopeDegree, cRafterB),
                                                                                                       -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cRafterB));
                        Point3D rightCenteringRafterStartMiddle = GetSumPoint(rightCenterigRafterStartRef, +valueService.GetOppositeByHypotenuse(roofSlopeDegree, cRafterB / 2),
                                                                                                           -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cRafterB / 2));


                        Line centeringRafterSquare1 = new Line(leftCenteringRafterEnd01, leftCenteringRafterEnd02);
                        Line centeringRafterSquare2 = new Line(rightCenteringRafterStart01, rightCenteringRafterStart02);
                        Line centeringRafterSquare3 = new Line(leftCenteringRafterEnd01, rightCenteringRafterStart01);
                        Line centeringRafterSquare4 = new Line(leftCenteringRafterEnd02, rightCenteringRafterStart02);
                        Line centeringRafterMiddle = new Line(leftCenteringRafterEndMiddle, rightCenteringRafterStartMiddle);
                        customBlockList.AddRange(new Line[] { centeringRafterSquare1, centeringRafterSquare2, centeringRafterSquare3, centeringRafterSquare4, centeringRafterMiddle });

                        #endregion

                    }
                }
            }
            
            return customBlockList.ToArray();
        }


        public Entity[] DrawBlock_Bottom(CDPoint selPoint1)
        {
            int firstIndex = 0;
            Point3D drawPoint = new Point3D(selPoint1.X, selPoint1.Y, selPoint1.Z);
            CDPoint refPoint = new CDPoint() { X = drawPoint.X, Y = drawPoint.Y };
            CDPoint curPoint = (CDPoint)refPoint.Clone();

            List<Entity> customBlockList = new List<Entity>();

            // Shell
            double bottomThk = valueService.GetDoubleValue(assemblyData.ShellOutput[0].MinThk);

            // Roof Slope
            string bottomSlopeString = assemblyData.BottomInput[firstIndex].BottomSlope;
            double bottomSlopeDegree = valueService.GetDegreeOfSlope(bottomSlopeString);

            BottomInputModel bottomBase = assemblyData.BottomInput[firstIndex];
            double bottomThickness = valueService.GetDoubleValue(bottomBase.BottomThickness);
            double annularThickness = valueService.GetDoubleValue(bottomBase.AnnularPlateThickness);
            double annularThickWidth = valueService.GetDoubleValue(bottomBase.AnnularPlateWidth);

            double overLap = 70;

            double outsideProjection = 0;
            double weldingLeg = 9;
            if (true)
            {
                //Annulal
                outsideProjection = 60 + weldingLeg;
            }
            else
            {
                outsideProjection = 30 + weldingLeg;
            }



            return customBlockList.ToArray();
        }


        private CompositeCurve GetBoltHoleHorizontal(Point3D newCenter, double boltWidth, double boltRadius)
        {

            double boltHeight = boltRadius / 2;
            Point3D newCenterLeft = GetSumPoint(newCenter, -boltWidth / 2 + boltHeight, 0);
            Point3D newCenterRight = GetSumPoint(newCenter, +boltWidth / 2 - boltHeight, 0);

            Arc arcarc1 = new Arc(Plane.XY, newCenterLeft, boltHeight, GetSumPoint(newCenterLeft, 0, boltHeight), GetSumPoint(newCenterLeft, 0, -boltHeight), false);
            Arc arcarc2 = new Arc(Plane.XY, newCenterRight, boltHeight, GetSumPoint(newCenterRight, 0, boltHeight), GetSumPoint(newCenterRight, 0, -boltHeight), true);
            Line ll1 = new Line(arcarc1.StartPoint, arcarc2.EndPoint);
            Line ll2 = new Line(arcarc1.EndPoint, arcarc2.StartPoint);

            CompositeCurve newCom = new CompositeCurve(arcarc1, arcarc2, ll1, ll2);
            return newCom;
        }



        public Entity[] DrawBlock_WindGirder(CDPoint selPoint1, ref CDPoint refPoint, ref CDPoint curPoint)
        {

            int refFirstIndex = 0;
            string selStiffenerType = assemblyData.WindGirderInput[refFirstIndex].StiffenerType;
            int selwindGirderCount = valueService.GetIntValue(assemblyData.WindGirderInput[refFirstIndex].Qty);

            List<Entity> windGirderEntity = new List<Entity>();

            // stiffenerType
            if (selStiffenerType.Length > 1)
            {
                selStiffenerType = selStiffenerType.Replace("Detail", "");
                selStiffenerType = selStiffenerType.Replace(" ", "");
            }
            selStiffenerType = selStiffenerType.ToLower();

            List<string[]> windGirderList = new List<string[]>();
            windGirderList.Add(new string[] { assemblyData.WindGirderInput[refFirstIndex].Elevation1, assemblyData.WindGirderInput[refFirstIndex].Size1 });
            windGirderList.Add(new string[] { assemblyData.WindGirderInput[refFirstIndex].Elevation2, assemblyData.WindGirderInput[refFirstIndex].Size2 });
            windGirderList.Add(new string[] { assemblyData.WindGirderInput[refFirstIndex].Elevation3, assemblyData.WindGirderInput[refFirstIndex].Size3 });

            // Type
            switch (selStiffenerType)
            {
                case "c":

                    for (int i = 0; i < selwindGirderCount; i++)
                    {
                        string[] eachWindGirder = windGirderList[i];
                        AngleSizeModel selAngleModel = refBlockService.GetAngleSizeModel(eachWindGirder[1]);

                        // Left
                        CDPoint adjPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftShell, eachWindGirder[0], ref refPoint, ref curPoint);
                        CDPoint drawPoint = GetSumCDPoint(adjPoint, -valueService.GetDoubleValue(selAngleModel.AB), 0);
                        Point3D rotatePoint = GetSumPoint(adjPoint, 0, 0);
                        Entity[] angleEntity = refBlockService.DrawReference_Angle(drawPoint, selAngleModel);
                        if (angleEntity != null)
                            foreach (Entity eachEntity in angleEntity)
                                eachEntity.Rotate(UtilityEx.DegToRad(90), Vector3D.AxisZ, rotatePoint);

                        windGirderEntity.AddRange(angleEntity);

                        // right : Mirror
                        //if (angleEntity != null)
                        //{
                        //    CDPoint mirrorRefPoint = workingPointService.ContactPoint("centerlinebottompoint",  ref refPoint, ref curPoint);
                        //    Plane pl1 = Plane.YZ;
                        //    pl1.Origin.X = mirrorRefPoint.X;
                        //    pl1.Origin.Y = mirrorRefPoint.Y;
                        //    Mirror customMirror = new Mirror(pl1);
                        //    foreach (Entity eachEntity in angleEntity)
                        //    {
                        //        Entity newEachEntity = (Entity)eachEntity.Clone();
                        //        newEachEntity.TransformBy(customMirror);
                        //        windGirderEntity.Add(newEachEntity);
                        //    }
                        //}


                    }

                    break;
                case "d":

                    for (int i = 0; i < selwindGirderCount; i++)
                    {
                        string[] eachWindGirder = windGirderList[i];
                        AngleSizeModel selAngleModel = refBlockService.GetAngleSizeModel(eachWindGirder[1]);

                        // Left
                        CDPoint adjPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftShell, eachWindGirder[0], ref refPoint, ref curPoint);
                        CDPoint drawPoint = GetSumCDPoint(adjPoint, -valueService.GetDoubleValue(selAngleModel.AB), -valueService.GetDoubleValue(selAngleModel.AB));
                        Entity[] angleEntity = refBlockService.DrawReference_Angle(drawPoint, selAngleModel);

                        List<Entity> leftAngleList = new List<Entity>();
                        leftAngleList.AddRange(angleEntity);

                        // Left : Double
                        if (angleEntity != null)
                        {
                            CDPoint mirrorRefPoint = drawPoint;
                            Plane pl1 = Plane.YZ;
                            pl1.Origin.X = mirrorRefPoint.X;
                            pl1.Origin.Y = mirrorRefPoint.Y;
                            Mirror customMirror = new Mirror(pl1);
                            foreach (Entity eachEntity in angleEntity)
                            {
                                Entity newEachEntity = (Entity)eachEntity.Clone();
                                newEachEntity.TransformBy(customMirror);
                                leftAngleList.Add(newEachEntity);
                            }
                        }

                        windGirderEntity.AddRange(leftAngleList);


                        // right : Mirror
                        //if (angleEntity != null)
                        //{
                        //    CDPoint mirrorRefPoint = workingPointService.ContactPoint("centerlinebottompoint", ref refPoint, ref curPoint);
                        //    Plane pl1 = Plane.YZ;
                        //    pl1.Origin.X = mirrorRefPoint.X;
                        //    pl1.Origin.Y = mirrorRefPoint.Y;
                        //    Mirror customMirror = new Mirror(pl1);
                        //    foreach (Entity eachEntity in leftAngleList)
                        //    {
                        //        Entity newEachEntity = (Entity)eachEntity.Clone();
                        //        newEachEntity.TransformBy(customMirror);
                        //        windGirderEntity.Add(newEachEntity);
                        //    }
                        //}


                    }

                    break;

            }





            return windGirderEntity.ToArray();
        }






        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
        private Point3D GetSumPoint(CDPoint selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
        private CDPoint GetSumCDPoint(CDPoint selPoint1, double X, double Y, double Z = 0)
        {
            return new CDPoint(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }




    }
}

