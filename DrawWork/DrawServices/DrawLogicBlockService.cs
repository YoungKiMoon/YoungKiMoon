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
    public class DrawLogicBlockService
    {
        private AssemblyModel assemblyData;

        private ValueService valueService;
        private DrawContactPointService contactPointService;

        private DrawReferenceBlockService refBlockService;

        public DrawLogicBlockService(AssemblyModel selAssembly)
        {
            assemblyData = selAssembly;
            valueService = new ValueService();
            contactPointService = new DrawContactPointService(selAssembly);
            refBlockService = new DrawReferenceBlockService(selAssembly);
        }

        public Entity[] DrawBlock_TopAngle(CDPoint selPoint1,ref CDPoint refPoint, ref CDPoint curPoint)
        {

            int refFirstIndex = 0;
            string selAngleType = assemblyData.RoofInput[refFirstIndex].TopAngleType;
            string selAngleSize = assemblyData.RoofInput[refFirstIndex].TopAngleSize;

            AngleSizeModel selAngleModel = refBlockService.GetAngleSizeModel(selAngleSize);
            int maxCourse = valueService.GetIntValue(assemblyData.ShellInput[0].CourseCount) - 1;

            CDPoint drawPoint = new CDPoint();
            CDPoint mirrorPoint = new CDPoint();

            // Type
            switch (selAngleType)
            {
                case "b":
                    drawPoint = GetSumCDPoint(refPoint, 
                                              - valueService.GetDoubleValue(selAngleModel.AB)
                                              - valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].MinThk), 

                                              - valueService.GetDoubleValue(selAngleModel.AB) 
                                              + valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeTankHeight));

                    break;

                case "d":
                    drawPoint = GetSumCDPoint(refPoint, 
                                              - valueService.GetDoubleValue(selAngleModel.AB), 

                                              - valueService.GetDoubleValue(selAngleModel.AB)
                                              + valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeTankHeight));

                    break;

                case "e":
                    drawPoint = GetSumCDPoint(refPoint,
                                              - valueService.GetDoubleValue(selAngleModel.AB)
                                              - valueService.GetDoubleValue(selAngleModel.t),

                                              - valueService.GetDoubleValue(selAngleModel.AB)
                                              + valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeTankHeight));

                    break;

            }

            Entity[] angleEntity = refBlockService.DrawReference_Angle(drawPoint, selAngleModel);

            switch (selAngleType)
            {
                case "e":
                    mirrorPoint = GetSumCDPoint(drawPoint, valueService.GetDoubleValue(selAngleModel.AB),0);
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

        public Entity[] DrawBlock_ColumnSupportSide(CDPoint selPoint1)
        {

            int firstIndex = 0;
            Point3D drawPoint = new Point3D(selPoint1.X, selPoint1.Y, selPoint1.Z);
            CDPoint refPoint = new CDPoint() { X=drawPoint.X,Y=drawPoint.Y};
            CDPoint curPoint = (CDPoint)refPoint.Clone();

            List<Entity> customBlockList = new List<Entity>();

            // Roof Slope
            string roofSlopeString = assemblyData.RoofInput[firstIndex].RoofSlopeOne;
            double roofSlopeDegree = valueService.GetAtanOfSlope(roofSlopeString);

            // Rafter : Bolt
            double rafterSideBoltWidth = 40;
            double rafterSideBoltGap = 70;

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


            #region Column Side
            for (int i= 0; i < assemblyData.StructureGirderInput.Count; i++)
            {
                #region Column Basic
                HBeamModel eachHBeam = assemblyData.StructureColumnHBeamOutput[i];
                StructureColumnSideModel eachColumnSide = assemblyData.StructureColumnSideOutput[i];
                PipeModel eachPipe = assemblyData.StructureColumnPipeOutput[i+1]; // Start = 2 Column

                string Size = eachColumnSide.A1A2;
                double B = valueService.GetDoubleValue(eachColumnSide.B);
                string C = eachColumnSide.C;
                double D = valueService.GetDoubleValue(eachColumnSide.D);
                double E = valueService.GetDoubleValue(eachColumnSide.E);
                double F = valueService.GetDoubleValue(eachColumnSide.F);
                double G = valueService.GetDoubleValue(eachColumnSide.G);
                double H = valueService.GetDoubleValue(eachColumnSide.H);

                double pipeOD = valueService.GetDoubleValue( eachPipe.OD);
                double pipeODHalf = pipeOD / 2;

                double radius = -valueService.GetDoubleValue(assemblyData.StructureGirderInput[i].GirderInRadius);

                CDPoint eachColumnPoint = contactPointService.ContactPoint("centerroofpoint", radius.ToString(), ref refPoint,ref curPoint);
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

                CDPoint eachColumnBasePoint = contactPointService.ContactPoint("centerbottompoint", radius.ToString(), ref refPoint, ref curPoint);
                drawPoint.X = eachColumnBasePoint.X;
                drawPoint.Y = eachColumnBasePoint.Y;

                CDPoint eachColumnBasePointLeft = contactPointService.ContactPoint("centerbottompoint", (radius - pipeODHalf - bD - bC - bJ).ToString(), ref refPoint, ref curPoint);
                CDPoint eachColumnBasePointRight = contactPointService.ContactPoint("centerbottompoint", (radius + pipeODHalf + bD + bC + bJ).ToString(), ref refPoint, ref curPoint);
                //Line basePad1 = new Line(new Point3D(eachColumnBasePointLeft.X, eachColumnBasePointLeft.Y), new Point3D(eachColumnBasePointRight.X, eachColumnBasePointRight.Y));                    
                Line basePad2 = new Line(GetSumPoint(eachColumnBasePointLeft,0,0), GetSumPoint(eachColumnBasePointLeft,0,bA));
                Line basePad3 = new Line(GetSumPoint(eachColumnBasePointLeft,0,bA), GetSumPoint(eachColumnBasePointRight,0,bA));
                Line basePad4 = new Line(GetSumPoint(eachColumnBasePointRight,0,0), GetSumPoint(eachColumnBasePointRight,0,bA));
                customBlockList.AddRange(new Line[] { basePad2, basePad3, basePad4 });

                CDPoint eachColumnBasePointLeft2 = contactPointService.ContactPoint("centerbottompoint", (radius - pipeODHalf - bD - bC - bI).ToString(), ref refPoint, ref curPoint);
                CDPoint eachColumnBasePointRight2 = contactPointService.ContactPoint("centerbottompoint", (radius + pipeODHalf + bD + bC + bI).ToString(), ref refPoint, ref curPoint);
                //Line basePadPad1 = new Line(new Point3D(eachColumnBasePointLeft2.X, eachColumnBasePointLeft2.Y + bA), new Point3D(eachColumnBasePointRight2.X, eachColumnBasePointRight2.Y + bA));
                Line basePadPad2 = new Line(GetSumPoint(eachColumnBasePointLeft2, 0, bA), GetSumPoint(eachColumnBasePointLeft2, 0, bA + bB));
                Line basePadPad3 = new Line(GetSumPoint(eachColumnBasePointLeft2, 0, bA + bB), GetSumPoint(eachColumnBasePointRight2, 0, bA + bB));
                Line basePadPad4 = new Line(GetSumPoint(eachColumnBasePointRight2, 0, bA), GetSumPoint(eachColumnBasePointRight2, 0, bA + bB));
                customBlockList.AddRange(new Line[] {  basePadPad2, basePadPad3, basePadPad4 });

                CDPoint eachColumnBasePointLeft3 = contactPointService.ContactPoint("centerbottompoint", (radius - pipeODHalf - bD - bC ).ToString(), ref refPoint, ref curPoint);
                CDPoint eachColumnBasePointRight3 = contactPointService.ContactPoint("centerbottompoint", (radius + pipeODHalf + bD + bC).ToString(), ref refPoint, ref curPoint);
                Line baseLeft1 = new Line(GetSumPoint(eachColumnBasePointLeft3, 0, bA + bB), GetSumPoint(eachColumnBasePointLeft3, 0, bA + bB + bF));
                Line baseRight1 = new Line(GetSumPoint(eachColumnBasePointRight3, 0, bA + bB), GetSumPoint(eachColumnBasePointRight3, 0, bA + bB + bF));
                customBlockList.AddRange(new Line[] { baseLeft1, baseRight1 });

                CDPoint eachColumnBasePointLeft4 = contactPointService.ContactPoint("centerbottompoint", (radius - pipeODHalf - bC -bG).ToString(), ref refPoint, ref curPoint);
                CDPoint eachColumnBasePointRight4 = contactPointService.ContactPoint("centerbottompoint", (radius + pipeODHalf + bC +bG).ToString(), ref refPoint, ref curPoint);
                Line baseLeftLeft1 = new Line(GetSumPoint(eachColumnBasePointLeft4, 0, bA + bB), GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC - bG, bA + bB + bE - bH));
                //Line baseLeftLeft2 = new Line(GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC - bG, +bA + bB + bE - bH), GetSumPoint(eachColumnBasePoint, 0, bA + bB + bE - bH));
                Line baseLeftLeft3 = new Line(GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC, +bA + bB + bE - bH), GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC, +bA + bB + bE));
                Line baseLeftLeft4 = new Line(GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC, +bA + bB + bE), GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC - bF, +bA + bB + bE));
                Line baseLeftLeft5 = new Line(GetSumPoint(eachColumnBasePointLeft3, 0, +bA + bB + bF), GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC - bF, +bA + bB + bE));
                customBlockList.AddRange(new Line[] { baseLeftLeft1,  baseLeftLeft3, baseLeftLeft4, baseLeftLeft5 });

                Line baseLeftRight1 = new Line(GetSumPoint(eachColumnBasePointRight4, 0, bA + bB), GetSumPoint(eachColumnBasePoint, pipeODHalf + bC + bG, bA + bB + bE - bH));
                //Line baseLeftRight2 = new Line(GetSumPoint(eachColumnBasePoint, pipeODHalf + bC + bG, +bA + bB + bE - bH), GetSumPoint(eachColumnBasePoint, 0, bA + bB + bE - bH));
                Line baseLeftRight3 = new Line(GetSumPoint(eachColumnBasePoint, pipeODHalf + bC, +bA + bB + bE - bH), GetSumPoint(eachColumnBasePoint, pipeODHalf + bC, +bA + bB + bE));
                Line baseLeftRight4 = new Line(GetSumPoint(eachColumnBasePoint, pipeODHalf + bC, +bA + bB + bE), GetSumPoint(eachColumnBasePoint, pipeODHalf + bC + bF, +bA + bB + bE));
                Line baseLeftRight5 = new Line(GetSumPoint(eachColumnBasePointRight3, 0, +bA + bB + bF), GetSumPoint(eachColumnBasePoint, pipeODHalf + bC + bF, +bA + bB + bE));
                customBlockList.AddRange(new Line[] { baseLeftRight1,  baseLeftRight3, baseLeftRight4, baseLeftRight5 });

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

            // Basic
            PipeModel centerPipe = assemblyData.StructureColumnPipeOutput[0]; // Center
            double centerPipeOD = valueService.GetDoubleValue(centerPipe.OD);
            double centerPipeODHalf = centerPipeOD / 2;
            double centerRadius = 0; // Center
            CDPoint centerColumnPoint = contactPointService.ContactPoint("centerroofpoint", centerRadius.ToString(), ref refPoint, ref curPoint);

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

            double boltHoleHeight = valueService.GetDoubleValue(assemblyData.StructureClipSlotHoleOutput[firstIndex].ht);
            double boltHoleWidth = valueService.GetDoubleValue(assemblyData.StructureClipSlotHoleOutput[firstIndex].wd);

            // WP : Left Square Center
            double centerLeftWidthHalf = tsB;
            CDPoint centerTopRoofLeftB = contactPointService.ContactPoint("centerroofpoint", (centerRadius - centerLeftWidthHalf).ToString(), ref refPoint, ref curPoint);

            double centerRafterHeightHalf = rA / 2;
            double raHeightHalf = valueService.GetSlopeOfHeight(roofSlopeString, centerRafterHeightHalf);
            double raSlopeWidthHalf = Math.Sqrt(centerRafterHeightHalf * centerRafterHeightHalf + raHeightHalf * raHeightHalf);
            CDPoint centerTopLeftSquare = new CDPoint(); // B Center
            centerTopLeftSquare.X = centerTopRoofLeftB.X;
            centerTopLeftSquare.Y = centerTopRoofLeftB.Y - raSlopeWidthHalf;

            // WP : Left Pad 
            double centerLeftWidthODHalf = centerPipeODHalf + tsG + 30;// 30 값 고정
            CDPoint centerTopRoofLeft = contactPointService.ContactPoint("centerroofpoint", (centerRadius - centerLeftWidthODHalf).ToString(), ref refPoint, ref curPoint);

            // Square : 평행 방향이동 : 아래쪽
            double centerRafterHeight = rA + 20; // MinValue : 20
            double raHeight = valueService.GetSlopeOfHeight(roofSlopeString, centerRafterHeight);
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
            Line topTriLeft2 = new Line(GetSumPoint(centerTopLeft, padPushWidth + tsG - smallTri, -padHeight), GetSumPoint(centerTopLeft, padPushWidth + tsG , -padHeight - smallTri));
            Line topTriLeft3 = new Line(GetSumPoint(centerTopLeft, padPushWidth + tsG, -padHeight - smallTri), GetSumPoint(centerTopLeft, padPushWidth + tsG, -padHeight - tsH));
            Line topTriLeft4 = new Line(GetSumPoint(centerTopLeft, padPushWidth + tsG-triEdge, -padHeight - tsH), GetSumPoint(centerTopLeft, padPushWidth + tsG, -padHeight - tsH)); // center column top
            Line topTriLeft5 = new Line(GetSumPoint(centerTopLeft, padPushWidth + tsG - triEdge, -padHeight - tsH), GetSumPoint(centerTopLeft, padPushWidth, -padHeight - triEdge));

            customBlockList.AddRange(new Line[] { topTriLeft1, topTriLeft2, topTriLeft3, topTriLeft4, topTriLeft5 });



            // Support Buttom Support
            CDPoint centerColumnBasePoint = contactPointService.ContactPoint("centerbottompoint", centerRadius.ToString(), ref refPoint, ref curPoint);
            drawPoint.X = centerColumnBasePoint.X;
            drawPoint.Y = centerColumnBasePoint.Y;

            CDPoint centerColumnBasePointLeft = contactPointService.ContactPoint("centerbottompoint", (centerRadius - centerPipeODHalf - bD - bC - bJ).ToString(), ref refPoint, ref curPoint); 
            Line centerPad2 = new Line(GetSumPoint(centerColumnBasePointLeft, 0, 0), GetSumPoint(centerColumnBasePointLeft, 0, bA));
            Line centerPad3 = new Line(GetSumPoint(centerColumnBasePointLeft, 0, bA), GetSumPoint(centerColumnBasePoint, 0, bA));
            Line centerPad4 = new Line(GetSumPoint(centerColumnBasePoint, 0, 0), GetSumPoint(centerColumnBasePoint, 0, bA));
            customBlockList.AddRange(new Line[] { centerPad2, centerPad3, centerPad4 });

            CDPoint centerColumnBasePointLeft2 = contactPointService.ContactPoint("centerbottompoint", (centerRadius - centerPipeODHalf - bD - bC - bI).ToString(), ref refPoint, ref curPoint);
            Line centerPadPad2 = new Line(GetSumPoint(centerColumnBasePointLeft2, 0, bA), GetSumPoint(centerColumnBasePointLeft2, 0, bA + bB));
            Line centerPadPad3 = new Line(GetSumPoint(centerColumnBasePointLeft2, 0, bA + bB), GetSumPoint(centerColumnBasePoint, 0, bA + bB));
            Line centerPadPad4 = new Line(GetSumPoint(centerColumnBasePoint, 0, bA), GetSumPoint(centerColumnBasePoint, 0, bA + bB));
            customBlockList.AddRange(new Line[] { centerPadPad2, centerPadPad3, centerPadPad4 });

            CDPoint centerColumnBasePointLeft3 = contactPointService.ContactPoint("centerbottompoint", (centerRadius - centerPipeODHalf - bD - bC).ToString(), ref refPoint, ref curPoint);
            Line centerLeft1 = new Line(GetSumPoint(centerColumnBasePointLeft3, 0, bA + bB), GetSumPoint(centerColumnBasePointLeft3, 0, bA + bB + bF));
            customBlockList.AddRange(new Line[] { centerLeft1 });

            CDPoint centerColumnBasePointLeft4 = contactPointService.ContactPoint("centerbottompoint", (centerRadius - centerPipeODHalf - bC - bG).ToString(), ref refPoint, ref curPoint);
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


            // Bolt Hole 관련 Top Angle이랑 처리 안됨
            #region Support Clip Shell Side
            StructureColumnClipShellSideModel eachSupportClip = assemblyData.StructureColumnClipShellSideOutput[firstIndex];
            double scA = valueService.GetDoubleValue(eachSupportClip.A);
            double scB = valueService.GetDoubleValue(eachSupportClip.B);
            double scC = valueService.GetDoubleValue(eachSupportClip.C);
            double scD = valueService.GetDoubleValue(eachSupportClip.D);
            double scE = valueService.GetDoubleValue(eachSupportClip.E);
            double scF = valueService.GetDoubleValue(eachSupportClip.F);
            double scG = valueService.GetDoubleValue(eachSupportClip.G);

            StructureColumnRafterModel lastRafter = assemblyData.StructureColumnRafterOutput[assemblyData.StructureColumnRafterOutput.Count-1];
            double lrA = valueService.GetDoubleValue(lastRafter.A);
            double lrB = valueService.GetDoubleValue(lastRafter.B);
            double lrC = valueService.GetDoubleValue(lastRafter.C);
            double lrD = valueService.GetDoubleValue(lastRafter.D);
            double lrE = valueService.GetDoubleValue(lastRafter.E);
            double lrBoltHoleOnShell = valueService.GetDoubleValue(lastRafter.BoltHoleOnShell);
            double lrBoltHoleOnColumn = valueService.GetDoubleValue(lastRafter.BoltHoleOnColumn);
            double lrBoltHoleOnCenter = valueService.GetDoubleValue(lastRafter.BoltHoleOnCenter);
            double lrBoltHoleDia = valueService.GetDoubleValue(lastRafter.BoltHoleDia);

            Point3D leftTankTop = GetSumPoint(refPoint, 0, valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeTankHeight));

            double shellClipTopGap = 30;
            double shellClipPadWidth = 10;
            double shellClipPadInto = 25;
            double shellClipTriTopGap = 70;
            double shellClipTriBottomGap = 50;
            double shellClipTriTopEndGap = 55;


            Line shellClipPad1 = new Line(GetSumPoint(leftTankTop, 0, -shellClipTopGap), GetSumPoint(leftTankTop, 0, -shellClipTopGap - scF));
            Line shellClipPad2 = new Line(GetSumPoint(leftTankTop, shellClipPadWidth, -shellClipTopGap), GetSumPoint(leftTankTop, shellClipPadWidth, -shellClipTopGap - scF));
            Line shellClipPad3 = new Line(GetSumPoint(leftTankTop, 0, -shellClipTopGap), GetSumPoint(leftTankTop, shellClipPadWidth, -shellClipTopGap));
            Line shellClipPad4 = new Line(GetSumPoint(leftTankTop, 0, -shellClipTopGap - scF), GetSumPoint(leftTankTop, shellClipPadWidth, -shellClipTopGap - scF));
            customBlockList.AddRange(new Line[] { shellClipPad1, shellClipPad2, shellClipPad3, shellClipPad4 });

            CDPoint leftTankRoofTop = contactPointService.ContactPoint("leftroofpoint", ref refPoint, ref curPoint);


            // Gap : Slope
            double leftTankRoofTopGapWidth = 7;
            double leftTankRoofTopGapHeight = valueService.GetSlopeOfHeight(roofSlopeString, leftTankRoofTopGapWidth);
            double leftTankRoofTopGapSlope = Math.Sqrt(leftTankRoofTopGapWidth * leftTankRoofTopGapWidth + leftTankRoofTopGapHeight * leftTankRoofTopGapHeight);

            // 평행 방향 이동 : 아래쪽
            CDPoint leftTankRoofTop1 = contactPointService.ContactPoint("leftroofpoint", shellClipTriTopGap.ToString(), ref refPoint, ref curPoint);
            leftTankRoofTop1.Y = leftTankRoofTop1.Y - leftTankRoofTopGapSlope;

            // 중간 : Slope 길이 -> Width로 변환
            double shellBoltWidth = (rafterSideBoltWidth + rafterSideBoltGap)*Math.Cos(roofSlopeDegree);

            // 평행 방향 이동 : 아래쪽
            CDPoint leftTankRoofTop2 = contactPointService.ContactPoint("leftroofpoint", (shellClipTriTopGap+ shellBoltWidth+ shellClipTriTopEndGap).ToString(), ref refPoint, ref curPoint);
            leftTankRoofTop2.Y = leftTankRoofTop2.Y - leftTankRoofTopGapSlope;

            Line shellClipTri1 = new Line(GetSumPoint(leftTankTop, shellClipPadWidth, -shellClipTopGap - shellClipPadInto), GetSumPoint(leftTankRoofTop1, 0, 0));
            Line shellClipTri2 = new Line(GetSumPoint(leftTankRoofTop1, 0, 0), GetSumPoint(leftTankRoofTop2, 0, 0));
            Line shellClipTri3 = new Line(GetSumPoint(leftTankRoofTop2, 0, 0), GetSumPoint(leftTankRoofTop2, 0, -scC));
            Line shellClipTri4 = new Line(GetSumPoint(leftTankRoofTop2, 0, -scC), GetSumPoint(leftTankTop, shellClipPadWidth + shellClipTriBottomGap, -shellClipTopGap - scF + shellClipPadInto));
            Line shellClipTri5 = new Line(GetSumPoint(leftTankTop, shellClipPadWidth, -shellClipTopGap - scF+ shellClipPadInto), GetSumPoint(leftTankTop, shellClipPadWidth+ shellClipTriBottomGap, -shellClipTopGap - scF+ shellClipPadInto));
            customBlockList.AddRange(new Line[] { shellClipTri1, shellClipTri2, shellClipTri3, shellClipTri4, shellClipTri5 });


            // Clip Shell Side : Bolt Hole
            if (lrBoltHoleOnShell == 2)
            {
                // 직각 방향 이동: 아래쪽
                double clipSideHoleFirstSlope1 = lrA / 2;
                double clipSideHoleFirstWidth1 = clipSideHoleFirstSlope1 * Math.Cos(roofSlopeDegree);
                double clipSideHoleFirstHeight1 = clipSideHoleFirstSlope1 * Math.Sin(roofSlopeDegree);
                Point3D clipSideHoleFirstPoint1 = GetSumPoint(leftTankRoofTop1, clipSideHoleFirstHeight1, -clipSideHoleFirstWidth1);
                double clipSideHoleFirstSlope2 = rafterSideBoltWidth;
                double clipSideHoleFirstWidth2 = clipSideHoleFirstSlope2 * Math.Cos(roofSlopeDegree);
                double clipSideHoleFirstHeight2 = clipSideHoleFirstSlope2 * Math.Sin(roofSlopeDegree);
                Point3D clipSideHoleFirstPoint2 = GetSumPoint(clipSideHoleFirstPoint1, clipSideHoleFirstWidth2, +clipSideHoleFirstHeight2);
                double clipSideHoleFirstSlope3 = rafterSideBoltGap;
                double clipSideHoleFirstWidth3 = clipSideHoleFirstSlope3 * Math.Cos(roofSlopeDegree);
                double clipSideHoleFirstHeight3 = clipSideHoleFirstSlope3 * Math.Sin(roofSlopeDegree);
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
                double clipSideHoleSecondWidth1 = clipSideHoleSecondSlope1 * Math.Cos(roofSlopeDegree);
                double clipSideHoleSecondHeight1 = clipSideHoleSecondSlope1 * Math.Sin(roofSlopeDegree);
                Point3D clipSideHoleSecondPoint1 = GetSumPoint(leftTankRoofTop1, clipSideHoleSecondHeight1, -clipSideHoleSecondWidth1);
                double clipSideHoleSecondlope2 = rafterSideBoltWidth;
                double clipSideHoleSecondWidth2 = clipSideHoleSecondlope2 * Math.Cos(roofSlopeDegree);
                double clipSideHoleSecondHeight2 = clipSideHoleSecondlope2 * Math.Sin(roofSlopeDegree);
                Point3D clipSideHoleSecondPoint2 = GetSumPoint(clipSideHoleSecondPoint1, clipSideHoleSecondWidth2, +clipSideHoleSecondHeight2);
                double clipSideHoleSecondSlope3 = rafterSideBoltGap;
                double clipSideHoleSecondWidth3 = clipSideHoleSecondSlope3 * Math.Cos(roofSlopeDegree);
                double clipSideHoleSecondHeight3 = clipSideHoleSecondSlope3 * Math.Sin(roofSlopeDegree);
                Point3D clipSideHoleSecondPoint3 = GetSumPoint(clipSideHoleSecondPoint2, clipSideHoleSecondWidth3, +clipSideHoleSecondHeight3);

                CompositeCurve clipSideBoltSecond1 = GetBoltHoleHorizontal(clipSideHoleSecondPoint2, boltHoleWidth, boltHoleHeight);
                clipSideBoltSecond1.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleSecondPoint2);
                CompositeCurve clipSideBoltSecond2 = GetBoltHoleHorizontal(clipSideHoleSecondPoint3, boltHoleWidth, boltHoleHeight);
                clipSideBoltSecond2.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleSecondPoint3);
                customBlockList.AddRange(new CompositeCurve[] { clipSideBoltSecond1, clipSideBoltSecond2 });

                // 직각 방향 이동: 아래쪽
                double clipSideHoleSecondSlope4 = lrD +lrE;
                double clipSideHoleSecondWidth4 = clipSideHoleSecondSlope4 * Math.Cos(roofSlopeDegree);
                double clipSideHoleSecondHeight4 = clipSideHoleSecondSlope4 * Math.Sin(roofSlopeDegree);
                Point3D clipSideHoleSecondPoint4 = GetSumPoint(leftTankRoofTop1, clipSideHoleSecondHeight4, -clipSideHoleSecondWidth4);
                double clipSideHoleSecondlope5 = rafterSideBoltWidth;
                double clipSideHoleSecondWidth5 = clipSideHoleSecondlope5 * Math.Cos(roofSlopeDegree);
                double clipSideHoleSecondHeight5 = clipSideHoleSecondlope5 * Math.Sin(roofSlopeDegree);
                Point3D clipSideHoleSecondPoint5 = GetSumPoint(clipSideHoleSecondPoint4, clipSideHoleSecondWidth5, +clipSideHoleSecondHeight5);
                double clipSideHoleSecondSlope6 = rafterSideBoltGap;
                double clipSideHoleSecondWidth6 = clipSideHoleSecondSlope6 * Math.Cos(roofSlopeDegree);
                double clipSideHoleSecondHeight6 = clipSideHoleSecondSlope6 * Math.Sin(roofSlopeDegree);
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

            #region Rafter

            CDPoint rafterEndPoint = contactPointService.ContactPoint("leftroofpoint", shellClipTriTopGap.ToString(), ref refPoint, ref curPoint);

            // 수직방향 이동 : 오른쪽으로
            CDPoint rafterStartPoint= contactPointService.ContactPoint("centerroofpoint", (centerRadius - centerLeftWidthHalf).ToString(), ref refPoint, ref curPoint);
            double rafterStartPointtSlope = rafterSideBoltGap/2 + rafterSideBoltWidth;
            double rafterStartPointtWidth = rafterStartPointtSlope * Math.Cos(roofSlopeDegree);
            double rafterStartPointtHeight = rafterStartPointtSlope * Math.Sin(roofSlopeDegree);
            Point3D rafterStartPoint2 = GetSumPoint(rafterStartPoint, rafterStartPointtWidth, rafterStartPointtHeight);


            Point3D rafterTempColumnPoint = new Point3D();
            for (int i = 0; i < assemblyData.StructureColumnRafterOutput.Count; i++)
            {
                Point3D rafterStartColumnPoint = new Point3D();
                if (i == 0)
                {
                    rafterStartColumnPoint.X = rafterStartPoint2.X;
                    rafterStartColumnPoint.Y = rafterStartPoint2.Y;
                }
                else
                {
                    rafterStartColumnPoint.X = rafterTempColumnPoint.X;
                    rafterStartColumnPoint.Y = rafterTempColumnPoint.Y;
                }

                double rafterEachRadius = -valueService.GetDoubleValue(assemblyData.StructureRafterInput[i].RafterInRadius);
                CDPoint rafterCurrentColumnPointTemp = contactPointService.ContactPoint("centerroofpoint", rafterEachRadius.ToString(), ref refPoint, ref curPoint);
                Point3D rafterCurrentColumnPoint = GetSumPoint(rafterCurrentColumnPointTemp, 0, 0);

                if (i== assemblyData.StructureColumnRafterOutput.Count - 1)
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
                double rafterStartWidth = rafterStartSlope * Math.Cos(roofSlopeDegree);
                double rafterStartHeight = rafterStartSlope * Math.Sin(roofSlopeDegree);
                Point3D rafterStartColumnPoint2 = GetSumPoint(rafterStartColumnPoint, rafterStartHeight, -rafterStartWidth);

                double rafterCurrentSlope = rafterWidth;
                double rafterCurrentWidth = rafterCurrentSlope * Math.Cos(roofSlopeDegree);
                double rafterCurrentHeight = rafterCurrentSlope * Math.Sin(roofSlopeDegree);
                Point3D rafterCurrentColumnPoint2 = GetSumPoint(rafterCurrentColumnPoint, rafterCurrentHeight, -rafterCurrentWidth);


                Line rafterSquare1 = new Line(rafterStartColumnPoint, rafterCurrentColumnPoint);
                Line rafterSquare2 = new Line(rafterStartColumnPoint, rafterStartColumnPoint2);
                Line rafterSquare3 = new Line(rafterCurrentColumnPoint, rafterCurrentColumnPoint2);
                Line rafterSquare4 = new Line(rafterStartColumnPoint2, rafterCurrentColumnPoint2);
                customBlockList.AddRange(new Line[] { rafterSquare1, rafterSquare2, rafterSquare3, rafterSquare4 });

                // Center Line
                double rafterWidthHalf = valueService.GetDoubleValue(assemblyData.StructureColumnRafterOutput[i].A)/2;
                double rafterStartSlope2 = rafterWidthHalf;
                double rafterStartWidth2 = rafterStartSlope2 * Math.Cos(roofSlopeDegree);
                double rafterStartHeight2 = rafterStartSlope2 * Math.Sin(roofSlopeDegree);
                Point3D rafterStartColumnPoint3 = GetSumPoint(rafterStartColumnPoint, rafterStartHeight2, -rafterStartWidth2);

                double rafterCurrentSlope2 = rafterWidthHalf;
                double rafterCurrentWidth2 = rafterCurrentSlope2 * Math.Cos(roofSlopeDegree);
                double rafterCurrentHeight2 = rafterCurrentSlope2 * Math.Sin(roofSlopeDegree);
                Point3D rafterCurrentColumnPoint4 = GetSumPoint(rafterCurrentColumnPoint, rafterCurrentHeight2, -rafterCurrentWidth2);

                Line rafterSquareCenter1 = new Line(rafterStartColumnPoint3, rafterCurrentColumnPoint4);
                customBlockList.Add(rafterSquareCenter1);

                // Current -> Start
                rafterTempColumnPoint.X = rafterCurrentColumnPoint.X;
                rafterTempColumnPoint.Y = rafterCurrentColumnPoint.Y;
            }

            #endregion

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



        public Entity[] DrawBlock_WindGirder(CDPoint selPoint1,ref CDPoint refPoint, ref CDPoint curPoint)
        {

            int refFirstIndex = 0;
            string selStiffenerType = assemblyData.WindGirderInput[refFirstIndex].StiffenerType;
            int selwindGirderCount= valueService.GetIntValue( assemblyData.WindGirderInput[refFirstIndex].Qty);

            List<Entity> windGirderEntity = new List<Entity>();

            // stiffenerType
            if (selStiffenerType.Length > 1)
            {
                selStiffenerType = selStiffenerType.Replace("Detail", "");
                selStiffenerType = selStiffenerType.Replace(" ", "");
            }
            selStiffenerType=selStiffenerType.ToLower();

            List<string[]> windGirderList = new List<string[]>();
            windGirderList.Add(new string[] { assemblyData.WindGirderInput[refFirstIndex].Elevation1, assemblyData.WindGirderInput[refFirstIndex].Size1 });
            windGirderList.Add(new string[] { assemblyData.WindGirderInput[refFirstIndex].Elevation2, assemblyData.WindGirderInput[refFirstIndex].Size2 });
            windGirderList.Add(new string[] { assemblyData.WindGirderInput[refFirstIndex].Elevation3, assemblyData.WindGirderInput[refFirstIndex].Size3 });

            // Type
            switch (selStiffenerType)
            {
                case "c":

                    for (int i = 0;i< selwindGirderCount; i++)
                    {
                        string[] eachWindGirder= windGirderList[i];
                        AngleSizeModel selAngleModel = refBlockService.GetAngleSizeModel(eachWindGirder[1]);

                        // Left
                        CDPoint adjPoint = contactPointService.ContactPoint("leftshelladj", eachWindGirder[0], ref refPoint, ref curPoint);
                        CDPoint drawPoint = GetSumCDPoint(adjPoint, -valueService.GetDoubleValue(selAngleModel.AB), 0);
                        Point3D rotatePoint = GetSumPoint(adjPoint, 0,0);
                        Entity[] angleEntity = refBlockService.DrawReference_Angle(drawPoint, selAngleModel);
                        if (angleEntity != null)
                            foreach (Entity eachEntity in angleEntity)
                                eachEntity.Rotate(UtilityEx.DegToRad(90), Vector3D.AxisZ, rotatePoint);

                        windGirderEntity.AddRange(angleEntity);

                        // right : Mirror
                        //if (angleEntity != null)
                        //{
                        //    CDPoint mirrorRefPoint = contactPointService.ContactPoint("centerlinebottompoint",  ref refPoint, ref curPoint);
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
                        CDPoint adjPoint = contactPointService.ContactPoint("leftshelladj", eachWindGirder[0], ref refPoint, ref curPoint);
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
                        //    CDPoint mirrorRefPoint = contactPointService.ContactPoint("centerlinebottompoint", ref refPoint, ref curPoint);
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

