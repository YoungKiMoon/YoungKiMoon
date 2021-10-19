using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawShapeLib.Commons;
using DrawShapeLib.DrawStyleServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawShapeLib.DrawServices
{
    public class Clip_CenterRIng
    {
        public CENTERRING_TYPE type;

        public double A; public double B; public double C; public double D; public double E; public double HoleQty;
        public double A1; public double B1; public double C1; public double D1; public double E1; public double ChamferF1;
        public double SlotHoleHt; public double SlotHoleWd;

        public Clip_CenterRIng(CENTERRING_TYPE Type)
        {
            if (Type == CENTERRING_TYPE.DRT_INT)
            {
                type = CENTERRING_TYPE.DRT_INT;
                A = 200; B = 0; C = 160; D = 40; E = 80; HoleQty = 4;
                A1 = 30; B1 = 40; C1 = 70; D1 = 20; E1 = 15; ChamferF1 = 0; SlotHoleHt = 23; SlotHoleWd = 45;
            }
            else if (Type == CENTERRING_TYPE.CRT_INT)
            {
                type = CENTERRING_TYPE.CRT_INT;
                // slot Hole : 2
                A = 150; B = 68; C = 75; D = 0; E = 0; HoleQty = 2;
                A1 = 7; B1 = 30; C1 = 55; D1 = 70; E1 = 40; ChamferF1 = 15; SlotHoleHt = 23; SlotHoleWd = 45;
                
                // slot Hole : 4
                //A = 200; B = 48; C = 55; D = 90; E = 0; HoleQty = 4;
                //A1 = 7; B1 = 30; C1 = 55; D1 = 70; E1 = 40; ChamferF1 = 15; SlotHoleHt = 23; SlotHoleWd = 45;
            }
        }
    }


    public class DrawCommonService
    {
        private StyleFunctionService styleService;
        private LayerStyleService layerService;


        public DrawCommonService()
        {
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();
        }

        private Line SetInterSectLIneCut(Line refLine, Circle withIntersect, bool isCutFlip=false)
        {
            Point3D[] intersectPoint = refLine.IntersectWith(withIntersect);

            if (isCutFlip) refLine.StartPoint = intersectPoint[0];
            else refLine.EndPoint = intersectPoint[0];

            return (Line)refLine.Clone();
        }

        public List<Entity> GetColumnRib(Point3D refPoint, double scaleValue)
        {

            // refPoint : Bottom Center

            List<Entity> entityList = new List<Entity>();
            List<Entity> centerlinesList = new List<Entity>();
            List<Entity> hiddenlinesList = new List<Entity>();
            List<Entity> outlinesList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            //////////////////////////////////////////////////////////
            // Input CAD DATA
            //////////////////////////////////////////////////////////

            double pipeOD = 168.3; // PipeOD
            double ribLength = 300; // A
            double ribWidth = 700; // A1
            double ribTopFlat = 50; // D1
            double ribChamfer = 15; // @1
            double sleeveWidth = 500; // B1
            double RibTopWidth = 200; // C1
            double radius = 15; // F1
            double sleeveThk = 10; // H1
            double distanceSleeve = 3; // E1

            double radiusOuter = pipeOD / 2;

            /////////////////////


            ////////////////////////////////
            /// Draw 
            ////////////////////////////////
            double ribBottomLength = ribLength - sleeveThk;
            List<Line> guideBottomBox = GetRectangle(GetSumPoint(referencePoint, -ribBottomLength, 0), sleeveWidth, ribBottomLength, RECTANGLUNVIEW.TOP);
            List<Line> guideTopBox = GetRectangle(GetSumPoint(referencePoint, -ribBottomLength, 0), RibTopWidth, ribLength, RECTANGLUNVIEW.LEFT);

            //Point3D centerRIngStarPoint = GetSumPoint(referencePoint, -radiusOuter, 0);
            //outlinesList.AddRange(GetRectangle(CopyPoint(centerRIngStarPoint), centerRingThk, radiusOuter));

            //List<Line> guideRibBox = GetRectangleLT(GetSumPoint(centerRIngStarPoint, distanceRib, 0), ribHeight, ribLength);

            ////guideRibBox[0].TrimBy(GetSumPoint(guideRibBox[0].EndPoint, -ribTop,0), true);
            //guideRibBox[0].TrimAt(ribLength - ribTop, true);
            //guideRibBox[3].TrimAt(ribTop, false);
            //Line ribDiagonal = new Line(guideRibBox[3].EndPoint, guideRibBox[0].StartPoint);

            //Line ribChamferLine = GetChamferLine(guideRibBox[2], guideRibBox[1], ribChamfer);
            //guideRibBox[2].TrimBy(ribChamferLine.StartPoint, false);
            //guideRibBox[1].TrimBy(ribChamferLine.EndPoint, true);


            //outlinesList.AddRange(guideRibBox);

            //outlinesList.AddRange(ribList);
            outlinesList.AddRange(new Entity[] {
                //circleOD, centerHole, //ribLeftLine,ribRightLine
            });

            centerlinesList.AddRange(new Entity[] {
                //circleHoleCenter,
            });

            hiddenlinesList.AddRange(guideTopBox);
            hiddenlinesList.AddRange(guideBottomBox);
            hiddenlinesList.AddRange(new Entity[] {
                // , guideTopBox,
            });

            styleService.SetLayerListEntity(ref centerlinesList, layerService.LayerCenterLine);
            styleService.SetLayerListEntity(ref hiddenlinesList, layerService.LayerHiddenLine);
            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

            entityList.AddRange(centerlinesList);
            entityList.AddRange(hiddenlinesList);
            entityList.AddRange(outlinesList);

            return entityList;
        }

        public List<Entity> GetColumnBaseSupport_SideView(Point3D refPoint, double scaleValue)
        {

            // refPoint : Bottom Center

            List<Entity> entityList = new List<Entity>();
            List<Entity> centerlinesList = new List<Entity>();
            List<Entity> hiddenlinesList = new List<Entity>();
            List<Entity> outlinesList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            //////////////////////////////////////////////////////////
            // Input CAD DATA
            //////////////////////////////////////////////////////////

            double pipeOD = 168.3; // PipeOD
            double ribLength = 300; // A
            double ribWidth = 700; // A1
            double ribTopFlat = 50; // D1
            double ribChamfer = 15; // @1
            double sleeveWidth = 500; // B1
            double RibTopWidth = 200; // C1
            double radius = 15; // F1
            double sleeveThk = 10; // H1
            double distanceSleeve = 3; // E1
            
            double distanceRib = sleeveThk+ distanceSleeve; // E1+H1
            double radiusOuter = pipeOD / 2;
            double pipeheight = ribWidth + RibTopWidth;  // +Ribtopwidth = 임의갑 추가높이


            ////////////////////////////////
            /// Draw 
            ////////////////////////////////

            entityList.AddRange( GetColumnPipe(CopyPoint(referencePoint), pipeOD, ribWidth + RibTopWidth, VIEWPOSITION.TOP) ); 
            //outlinesList.AddRange(GetRectangle(CopyPoint(centerRIngStarPoint), centerRingThk, radiusOuter));

            //List<Line> guideRibBox = GetRectangleLT(GetSumPoint(centerRIngStarPoint, distanceRib, 0), ribHeight, ribLength);

            
            // Draw : Rib
            List<Entity> ribList = GetColumnRib(GetSumPoint(referencePoint, -(distanceRib),0), scaleValue);




            //outlinesList.AddRange(ribList);
            outlinesList.AddRange(new Entity[] {
                //circleOD, centerHole, //ribLeftLine,ribRightLine
            });

            centerlinesList.AddRange(new Entity[] {
                //circleHoleCenter,
            });

            hiddenlinesList.AddRange(new Entity[] {
                //circlePipeOuter, circlePipeInner
            });

            styleService.SetLayerListEntity(ref centerlinesList, layerService.LayerCenterLine);
            styleService.SetLayerListEntity(ref hiddenlinesList, layerService.LayerHiddenLine);
            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

            entityList.AddRange(centerlinesList);
            entityList.AddRange(hiddenlinesList);
            entityList.AddRange(outlinesList);

            return entityList;
        }

        public List<Entity> GetColumnCenterTopSupport_TopView(Point3D refPoint, double scaleValue)
        {

            // refPoint : Bottom Center of PipeTop(Center)

            List<Entity> centerlinesList = new List<Entity>();
            List<Entity> outlinesList = new List<Entity>();
            List<Entity> hiddenlinesList = new List<Entity>();
            List<Entity> entityList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            //////////////////////////////////////////////////////////
            // Input CAD DATA
            //////////////////////////////////////////////////////////

            double centerHoleDia = 24; // Hole 24
            double pipeOD = 168.3; // PipeOD
            double distanceRib = 30; // A1
            double distanceHoleCenter = 400; //B
            double ClipLength = 180; // C
            double ClipThk = 12; // G1
            double ribLength = 420; // G
            double ribThk = 10; // F1
            double pipeThk = 7.11; // SCH.40S : THK
            double clipCount = 10; // 임시값
            double pipeRibCount = 8; // 임시값

            double pipeRadius = pipeOD/2; // PipeOD
            double radiusOuter = pipeRadius + ribLength + distanceRib; // CenterRing RadiusOD
            double gapClipCP = distanceHoleCenter - ClipLength/2; 

            /////////////////////


            ////////////////////////////////
            /// Draw 
            ////////////////////////////////
            // Draw : circle
            Circle centerHole = new Circle(CopyPoint(referencePoint), centerHoleDia / 2);
            Circle circleOD = new Circle(CopyPoint(referencePoint), radiusOuter);
            
            Circle circleHoleCenter = new Circle(CopyPoint(referencePoint), distanceHoleCenter);

            Circle circlePipeOuter = new Circle(CopyPoint(referencePoint), pipeRadius);
            Circle circlePipeInner = new Circle(CopyPoint(referencePoint), pipeRadius -pipeThk);

            // Draw : Clip
            List<Line> clipList = GetRectangle(GetSumPoint(referencePoint, 0, gapClipCP), ClipLength, ClipThk);
            List<Entity> clipEntityList = new List<Entity>();
            foreach (Line eachLine in clipList) {
                clipEntityList.Add(eachLine);
            }
            outlinesList.AddRange(GetRepeatRotateEntity(CopyPoint(referencePoint), 1, clipCount, clipEntityList, 0 ));


            // Draw : Rib (HiddenLine)
            Line guideRibLine = new Line(GetSumPoint(referencePoint, 0, radiusOuter), CopyPoint(referencePoint));
            List<Entity> ribList = new List<Entity>();
            
            Line ribLineR = (Line)guideRibLine.Offset(ribThk / 2, Vector3D.AxisZ);
            Line ribLineL = (Line)guideRibLine.Offset(-(ribThk / 2), Vector3D.AxisZ);

            ribList.Add(SetInterSectLIneCut(ribLineR, circleOD, true));
            ribList.Add(SetInterSectLIneCut(ribLineL, circleOD, true));

            //ribList.AddRange(new Entity[] { ribLineR, ribLineL });




            hiddenlinesList.AddRange(GetRepeatRotateEntity(CopyPoint(referencePoint), pipeRadius, pipeRibCount, ribList, 2, 360));


            //outlinesList.AddRange(ribList);
            outlinesList.AddRange(new Entity[] {
                circleOD, centerHole, //ribLeftLine,ribRightLine
            });

            centerlinesList.AddRange(new Entity[] {
                circleHoleCenter, 
            });

            hiddenlinesList.AddRange(new Entity[] {
                circlePipeOuter, circlePipeInner
            });

            styleService.SetLayerListEntity(ref centerlinesList, layerService.LayerCenterLine);
            styleService.SetLayerListEntity(ref hiddenlinesList, layerService.LayerHiddenLine);
            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

            entityList.AddRange(centerlinesList);
            entityList.AddRange(hiddenlinesList);
            entityList.AddRange(outlinesList);

            return entityList;
        }

        public List<Entity> GetColumnCenterTopSupport(Point3D refPoint, double scaleValue)
        {

            // refPoint : Bottom Center of PipeTop(Center)

            List<Entity> outlinesList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            //////////////////////////////////////////////////////////
            // Input CAD DATA
            //////////////////////////////////////////////////////////

            double pipeOD = 168.3; // PipeOD
            double distanceRib = 30; // A1
            double ribTop = 50; // B1
            double centerRingThk = 22; // C1
            double ribChamfer = 15; // E1
            double ribLength = 420; // G
            double ribHeight = 420; // H
            double radiusOuter = (pipeOD/2) + ribLength + distanceRib; // CenterRing RadiusOD

            /////////////////////


            ////////////////////////////////
            /// Draw 
            ////////////////////////////////

            Point3D centerRIngStarPoint = GetSumPoint(referencePoint, -radiusOuter, 0);
            outlinesList.AddRange( GetRectangle(CopyPoint(centerRIngStarPoint), centerRingThk, radiusOuter,RECTANGLUNVIEW.RIGHT) );

            List<Line> guideRibBox = GetRectangleLT(GetSumPoint(centerRIngStarPoint, distanceRib, 0), ribHeight, ribLength);

            //guideRibBox[0].TrimBy(GetSumPoint(guideRibBox[0].EndPoint, -ribTop,0), true);
            guideRibBox[0].TrimAt(ribLength-ribTop, true);
            guideRibBox[3].TrimAt(ribTop, false);
            Line ribDiagonal = new Line(guideRibBox[3].EndPoint, guideRibBox[0].StartPoint);

            Line ribChamferLine = GetChamferLine(guideRibBox[2], guideRibBox[1], ribChamfer);
            guideRibBox[2].TrimBy(ribChamferLine.StartPoint, false);
            guideRibBox[1].TrimBy(ribChamferLine.EndPoint, true);


            outlinesList.AddRange(guideRibBox);

            //outlinesList.AddRange(rightBoxList);
            outlinesList.AddRange(new Entity[] {
                ribDiagonal, ribChamferLine,
                //RBoxBottomLine, RBoxRightLine, RBoxTopLine,
            });


            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

            return outlinesList;
        }

        public List<Entity> GetCenterColumn(Point3D refPoint, Clip_CenterRIng clipInfo, double INT_C, double INT_B, double FlangeThk, double SlopeRadian = 0, Point3D DomeCenterPoint = null)
        {
            // INT_C INT_B : CnterRing Type(INT). Data
            // INT_C : CenterRIng C - Flange Top Length(Outer)
            // INT_B : CenterRIng B - Flange Height(Inner)
            // FlangeThk : CenterRing Flange Thk(t1)
            // DomeCenterPoint : CenterPoint of Dome

            List<Entity> outlinesList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            //////////////////////////////////////////////////////////
            // Input CAD DATA
            //////////////////////////////////////////////////////////

            //double tankOD = 10000;
            //double outDiameter = 800;
            double slopeRadian = SlopeRadian;

            CENTERRING_TYPE CenterRingType = clipInfo.type;
            double distanceA = clipInfo.A;
            double distanceB = clipInfo.B;
            double distanceC = clipInfo.C;
            double distanceD = clipInfo.D;
            double distanceE = clipInfo.E;
            double holeQty = clipInfo.HoleQty;
            double distanceA1 = clipInfo.A1;
            double distanceB1 = clipInfo.B1;
            double distanceC1 = clipInfo.C1;
            double distanceD1 = clipInfo.D1;
            double distanceE1 = clipInfo.E1;
            double chamferF1 = clipInfo.ChamferF1;
            double slotHoleHt = clipInfo.SlotHoleHt;
            double slotHoleWd = clipInfo.SlotHoleWd;

            double columnHeight = 16322.5;
            double columnPipeGap = 500;
            double outerDiameter = 1200;

            /////////////////////
            //double domeRadius = tankOD / 2;
            //double radiusOuter = outDiameter / 2;


            ////////////////////////////////
            /// Draw 
            ////////////////////////////////
            List<double> cut_YList = new List<double> { 3000, 5000, 9000, 14000 };

            outlinesList.AddRange(GetColumnPipeCut(referencePoint, outerDiameter, columnHeight, columnPipeGap, cut_YList));
                
        

            //outlinesList.AddRange(rightBoxList);
            outlinesList.AddRange(new Entity[] {
                //chamferBottom, chamferTop,
                //RBoxBottomLine, RBoxRightLine, RBoxTopLine,
            });


            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

            return outlinesList;
        }

        public List<Entity> GetRingTypeCenterRing_TopView(Point3D refPoint, CENTERRING_TYPE type, double OutDiameter,
            double DistanceB, double DistanceC, double DistanceD, double ThkT2, double RafterThk, double scaleValue)
        {
            List<Entity> outlinesList = new List<Entity>();
            List<Entity> hiddenLinesList = new List<Entity>();
            List<Entity> virtualLinesList = new List<Entity>();
            List<Line> centerlinesList = new List<Line>();
            List<Entity> flangeList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            //////////////////////////////////////////////////////////
            // Input CAD DATA
            //////////////////////////////////////////////////////////

            double outDiameter = OutDiameter;
            double distanceB = DistanceB;
            double distanceC = DistanceC;
            double distanceD = DistanceD;
            double thkT2 = ThkT2;
            double rafterThk = RafterThk;
            
            double minRafterDistance = 500;  // Rafter 최소간격
            double addNum = 10; // Rafter 간격 증가단위


            // Set Radius
            double radiusOuter = outDiameter / 2;
            double radiusWebInner = radiusOuter - thkT2;

            // Set Distance Rafter  :  O.D / 3  10단위 UP (Min. 500)
            double rafterDistance = GetMinAddNum( (outDiameter/3), minRafterDistance, addNum );
            double rafterDistanceHalf = rafterDistance / 2;

            // Set Radius of ID / E
            double radiusInner = 0;
            double halfDistanceE = 0;
            if (type == CENTERRING_TYPE.CRT_INT || type == CENTERRING_TYPE.DRT_INT)
            {
                radiusInner = radiusOuter - distanceD;
                halfDistanceE = radiusOuter + distanceC;
            }
            else
            {
                radiusInner = radiusOuter - distanceC;
                halfDistanceE = radiusOuter + distanceB;
            }


            //////////////////////////////////////////////////////////
            // Draw 
            //////////////////////////////////////////////////////////

            // Draw : Circle (OutLine)
            Circle circleOuter = new Circle(referencePoint, halfDistanceE);
            Circle circleInnerr = new Circle(referencePoint, radiusInner);

            // Draw : Hidden circle
            Circle circleOD = new Circle(referencePoint, radiusOuter);
            Circle circleWeb = new Circle(referencePoint, radiusWebInner);

            // Draw Rafter
            Point3D TopLineTopSP = GetSumPoint(referencePoint, -radiusOuter, rafterDistanceHalf);
            Point3D LeftLineLeftSP = GetSumPoint(referencePoint, -rafterDistanceHalf, radiusOuter);

            List<Point3D> nullPoint = new List<Point3D>();
            List<Entity> topRafter = GetRectangleK(TopLineTopSP, rafterThk, OutDiameter, out nullPoint, true, 0, 0, 0, new bool[] {true,false,true,false } );
            topRafter.AddRange(GetMirrorEntity(Plane.XZ, topRafter, referencePoint.X, referencePoint.Y, true));
            outlinesList.AddRange(topRafter);

            List<Entity> leftRafter = GetRectangleK(LeftLineLeftSP, OutDiameter, rafterThk, out nullPoint, true, 0, 0, 0, new bool[] { false, true, false, true });
            leftRafter.AddRange(GetMirrorEntity(Plane.YZ, leftRafter, referencePoint.X, referencePoint.Y, true));
            outlinesList.AddRange(leftRafter);



            /*
            List<Entity> repeatEntityList = new List<Entity>();
            Point3D testEntityStartPoint = GetSumPoint(referencePoint, 0, radiusOuterOD);
            double gap = 10;
            Line testLine1 = new Line(GetSumPoint(testEntityStartPoint, -gap, outerThk), GetSumPoint(testEntityStartPoint, -gap, -outerThk));
            Line testLine2 = new Line(GetSumPoint(testEntityStartPoint, 0, outerThk), GetSumPoint(testEntityStartPoint, 0, -outerThk));
            Line testLine3 = new Line(GetSumPoint(testEntityStartPoint, gap, outerThk), GetSumPoint(testEntityStartPoint, gap, -outerThk));
            Line testLine4 = new Line(GetSumPoint(testEntityStartPoint, gap / 2, outerThk * 2), GetSumPoint(testEntityStartPoint, gap / 2, outerThk));
            repeatEntityList.AddRange(new Entity[] { testLine1,
                testLine2, testLine3, testLine4
            });

            // Draw : Rotate Repeat entity
            outlinesList.AddRange(GetRepeatRotateEntity(referencePoint, radiusOuterOD, rafterCount, repeatEntityList, 3));
            /**/


            hiddenLinesList.AddRange(new Entity[] {
                circleOD, circleWeb
            });

            //hiddenLinesList.AddRange(repeatEntityList);

            outlinesList.AddRange(new Entity[] {
                circleOuter, circleInnerr,
            });



            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref hiddenLinesList, layerService.LayerHiddenLine);
            outlinesList.AddRange(outlinesList);
            outlinesList.AddRange(hiddenLinesList);


            return outlinesList;
        }
        
        public List<Entity> GetClip_CenterRingSide(Point3D refPoint, Clip_CenterRIng clipInfo, double INT_C, double INT_B, double FlangeThk, double SlopeRadian = 0, Point3D DomeCenterPoint = null)
        {
            // INT_C INT_B : CnterRing Type(INT). Data
            // INT_C : CenterRIng C - Flange Top Length(Outer)
            // INT_B : CenterRIng B - Flange Height(Inner)
            // FlangeThk : CenterRing Flange Thk(t1)
            // DomeCenterPoint : CenterPoint of Dome

            List<Entity> outlinesList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            //////////////////////////////////////////////////////////
            // Input CAD DATA
            //////////////////////////////////////////////////////////

            //double tankOD = 10000;
            //double outDiameter = 800;
            double slopeRadian = SlopeRadian;

            CENTERRING_TYPE CenterRingType = clipInfo.type;
            double distanceA = clipInfo.A;
            double distanceB = clipInfo.B;
            double distanceC = clipInfo.C;
            double distanceD = clipInfo.D;
            double distanceE = clipInfo.E;
            double holeQty = clipInfo.HoleQty;
            double distanceA1 = clipInfo.A1;
            double distanceB1 = clipInfo.B1;
            double distanceC1 = clipInfo.C1;
            double distanceD1 = clipInfo.D1;
            double distanceE1 = clipInfo.E1;
            double chamferF1 = clipInfo.ChamferF1;
            double slotHoleHt = clipInfo.SlotHoleHt;
            double slotHoleWd = clipInfo.SlotHoleWd;


            /////////////////////
            //double domeRadius = tankOD / 2;
            //double radiusOuter = outDiameter / 2;


            ////////////////////////////////
            /// Draw 
            ////////////////////////////////

            // Set RightBox
            double extendLenght = 0;
            double chamferLength = distanceE1;
            if (CenterRingType == CENTERRING_TYPE.CRT_INT)
            {
                extendLenght = distanceB1; // B1 아님  Test용
                chamferLength = chamferF1;
            }
            List<Line> rightBoxList = GetRectangleLT(GetSumPoint(referencePoint, -(INT_C + extendLenght), 0), INT_B, INT_C + extendLenght);

            // Draw : Chamfer
            Line chamferBottom = GetChamferLine(rightBoxList[0], rightBoxList[1], chamferLength);
            Line chamferTop = GetChamferLine(rightBoxList[2], rightBoxList[1], chamferLength);

            // Draw : RightBoxLine
            Line RBoxBottomLine = new Line(CopyPoint(rightBoxList[0].StartPoint), CopyPoint(chamferBottom.StartPoint));
            Line RBoxRightLine = new Line(CopyPoint(chamferTop.EndPoint), CopyPoint(chamferBottom.EndPoint));
            Line RBoxTopLine = new Line(CopyPoint(rightBoxList[2].StartPoint), CopyPoint(chamferTop.StartPoint));


            // Draw : LeftBox
            if (CenterRingType == CENTERRING_TYPE.CRT_INT)
            {
                Point3D guideRoofStartPoint = GetSumPoint(referencePoint, -INT_C, FlangeThk);

                Line guideRoofLine = new Line( guideRoofStartPoint, GetSumPoint(guideRoofStartPoint, -(distanceA * 2), 0) ); //  distanceA(RafterWidth) * 2 : 적당한 길이의 임의 값
                guideRoofLine.Rotate(slopeRadian, Vector3D.AxisZ, guideRoofStartPoint);
                
                // Draw LeftBox (Top Left Bottom Lines)
                Line LeftTopLine = (Line)guideRoofLine.Offset(-distanceA1, Vector3D.AxisZ);
                Point3D[] intersectTop = LeftTopLine.IntersectWith(RBoxTopLine);

                LeftTopLine.TrimBy(intersectTop[0], true);
                RBoxTopLine.TrimBy(intersectTop[0], true);

                // guideLine RafterTopPoint
                Line guideRafterCut = new Line(GetSumPoint(guideRoofStartPoint, -distanceB1, 0), 
                                               GetSumPoint(guideRoofStartPoint, -distanceB1, -distanceA)); // distanceA (임의값 길게)

                Point3D[] intersectRafterTop = guideRoofLine.IntersectWith(guideRafterCut);

                Line guideSlotHoleCenterLine = new Line( CopyPoint(intersectRafterTop[0]), 
                                                    GetSumPoint(intersectRafterTop[0], -(distanceE1+(distanceD1/2)), 0));

                guideSlotHoleCenterLine.Rotate(slopeRadian, Vector3D.AxisZ, intersectRafterTop[0]);
                Point3D slotHoleCenter = ((Line)guideSlotHoleCenterLine.Offset(-distanceA / 2, Vector3D.AxisZ)).EndPoint;
                //Line testLine = (Line)guideSlotHoleCenterLine.Offset(-distanceA / 2, Vector3D.AxisZ);
                List<Entity> slotHoles = GetSlotHoles(slotHoleCenter, slotHoleHt / 2, distanceD, slotHoleWd, distanceD1); // todo : slotholeWD Edit
                foreach (Entity eachEntity in slotHoles) { eachEntity.Rotate(slopeRadian, Vector3D.AxisZ, slotHoleCenter); }
                outlinesList.AddRange(slotHoles);

                int indexTopLeftHole = 0;
                if (holeQty == 4) indexTopLeftHole = 2; // todo : entity4개 * 슬롯 인덱스 두개 찾아서 중간점 
                Point3D topLeftHoleCenter = ((Circle)slotHoles[indexTopLeftHole]).Center;  //  LeftArc RArc TopLine BLine

                // guideLine LeftCut
                Line guideLeftCut = new Line(GetSumPoint(topLeftHoleCenter, -distanceC1, distanceA), 
                                             GetSumPoint(topLeftHoleCenter, -distanceC1, -distanceA)); // distacneA : 임의값

                Point3D[] LeftTopLineEndPoint = LeftTopLine.IntersectWith(guideLeftCut);
                LeftTopLine.TrimBy(LeftTopLineEndPoint[0], false);
                RBoxBottomLine.StartPoint.X = guideLeftCut.StartPoint.X;

                Line LeftLine = new Line(CopyPoint(LeftTopLineEndPoint[0]), CopyPoint(RBoxBottomLine.StartPoint));

                outlinesList.AddRange(new Entity[] {
                    //guideRoofLine, 
                    LeftTopLine,  LeftLine,
                    //guideRafterCut, guideSlotHoleCenterLine,
                    //guideLeftCut,
                });

            }
            else if( CenterRingType == CENTERRING_TYPE.DRT_INT ) 
            {
                // Get RafterTopPoint : for Slot Holes WorkPoint
                double rafterTopPointX = (GetSumPoint(referencePoint, -(INT_C+distanceA1), 0)).X;
                //double rafterTopPointY = GetArcSidePointHeight()
                //Point3D rafterTopPoint = 

                // Get WorkPoint : for Slot Holes
                //Point3D SHworkPoint = 


            }
            List<Point3D> leftBoxPoint = null;
            double leftBoxLength = distanceA1 + (distanceB1 * 2) + distanceC1;
            //List<Entity> leftGuideBox = GetRectangleK(GetSumPoint(rightBoxPoint[3], -leftBoxLength, distanceD1), INT_C, leftBoxLength, out leftBoxPoint, true);



            //outlinesList.AddRange(rightBoxList);
            outlinesList.AddRange(new Entity[] {
                chamferBottom, chamferTop,
                RBoxBottomLine, RBoxRightLine, RBoxTopLine,
            });


            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

            return outlinesList;
        }

        public List<Entity> GetCenterRing_SideView(Point3D refPoint, CENTERRING_TYPE type, double OutDiameter, double SlopeRadian,
            double HeightA, double DistanceB, double DistanceC, double DistanceD, double ThkT1, double ThkT2, double TankOD, out Point3D WorkPoint)
        {
            // slopeRadian : Roof Slope - Radian
            // domeRadius : Tank OD 1/2

            List<Entity> outlinesList = new List<Entity>();
            List<Entity> virtualLinesList = new List<Entity>();
            List<Line> centerlinesList = new List<Line>();
            List<Entity> flangeList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            //////////////////////////////////////////////////////////
            // Input CAD DATA
            //////////////////////////////////////////////////////////

            double tankOD = TankOD;
            double slopeRadian = SlopeRadian;
            double outDiameter = OutDiameter;
            double heightA = HeightA;
            double distanceB = DistanceB;
            double distanceC = DistanceC;
            double distanceD = DistanceD;
            double thkT1 = ThkT1;
            double thkT2 = ThkT2;


            /////////////////////
            double domeRadius = tankOD / 2;
            double radiusOuter = outDiameter / 2;

            double radiusInner = 0;
            double halfDistanceE = 0;

            Point3D workPoint = null;
            Point3D domeCenterPoint = null;

            // set Radius of ID / E
            if (type == CENTERRING_TYPE.CRT_INT || type == CENTERRING_TYPE.DRT_INT)
            {
                radiusInner = radiusOuter - distanceD;
                halfDistanceE = radiusOuter + distanceC;
            }
            else
            {
                radiusInner = radiusOuter - distanceC;
                halfDistanceE = radiusOuter + distanceB;
            }


            ///////////////////////////
            // Get W.P
            ///////////////////////////

            // CRT W.P
            if (type == CENTERRING_TYPE.CRT_INT || type == CENTERRING_TYPE.CRT_EXT)
            {
                Line roofLine = new Line(CopyPoint(referencePoint), GetSumPoint(referencePoint, -outDiameter, 0));
                roofLine.Rotate(slopeRadian, Vector3D.AxisZ, roofLine.StartPoint);

                // guideWP Line  : CRT_INT = E  ,  CRT_EXT = OD
                double shiftX = halfDistanceE;
                if (type == CENTERRING_TYPE.CRT_EXT) { shiftX = radiusOuter; }
                Line intersectForVLine = new Line(GetSumPoint(referencePoint, -shiftX, 0),
                                                  GetSumPoint(referencePoint, -shiftX, -outDiameter));

                Point3D[] tempWP = roofLine.IntersectWith(intersectForVLine);
                workPoint = tempWP[0];

                /**/
                virtualLinesList.AddRange(new Entity[] {
                roofLine, intersectForVLine
                }); /**/
            }
            else // DRT W.P
            {
                domeCenterPoint = GetSumPoint(referencePoint, 0, -domeRadius);
                Arc roofArc = new Arc(domeCenterPoint, domeRadius, Utility.DegToRad(90), Utility.DegToRad(110));
                //Circle guideCircle = new Circle(domeCenterPoint, domeRadius);
                // guideWP Line  : DRT_INT = E  ,  DRT_EXT = OD
                double shiftX = halfDistanceE;
                if (type == CENTERRING_TYPE.DRT_EXT) { shiftX = radiusOuter; }
                Line intersectForVLine = new Line(GetSumPoint(referencePoint, -shiftX, 0),
                                                  GetSumPoint(referencePoint, -shiftX, -outDiameter));

                Point3D[] tempWP = roofArc.IntersectWith(intersectForVLine);
                workPoint = tempWP[0];

                /**/
                virtualLinesList.AddRange(new Entity[] {
                roofArc, intersectForVLine,
                }); /**/
            }
            // Out WorkPoint
            WorkPoint = workPoint; 

            // Get CenterRing_Flange
            flangeList = GetCenterRing_Flange(workPoint, type,
                            heightA, distanceB, distanceC, distanceD, thkT1, thkT2,
                            TankOD, SlopeRadian, OutDiameter);

            // Get CenterRing_Flange
            //flangeList = GetCenterRing_Flange(workPoint, type,
            //                heightA, distanceB, distanceC, distanceD, thkT1, thkT2, slopeRadian, domeRadius, domeCenterPoint);

            // Draw CenterPlate
            Point3D centerPlateStartPoint = null;
            if (type == CENTERRING_TYPE.CRT_INT || type == CENTERRING_TYPE.DRT_INT)
            {
                centerPlateStartPoint = GetSumPoint(workPoint, (distanceC + distanceD), -thkT1);
                centerlinesList.AddRange(GetRectangle(CopyPoint(centerPlateStartPoint), thkT1, radiusInner));
                centerPlateStartPoint = GetSumPoint(workPoint, (distanceC + distanceD), -heightA);
                centerlinesList.AddRange(GetRectangle(CopyPoint(centerPlateStartPoint), thkT1, radiusInner));
            }
            else
            {
                centerPlateStartPoint = GetSumPoint(workPoint, distanceC, (heightA - thkT1));
                centerlinesList.AddRange(GetRectangle(CopyPoint(centerPlateStartPoint), thkT1, radiusInner));

            }

            styleService.SetLayerListEntity(ref flangeList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref virtualLinesList, layerService.LayerVirtualLine);
            styleService.SetLayerListLine(ref centerlinesList, layerService.LayerOutLine);

            outlinesList.AddRange(virtualLinesList);
            outlinesList.AddRange(centerlinesList);
            outlinesList.AddRange(flangeList);


            return outlinesList;
        }

        public List<Entity> GetCenterRing_Flange(Point3D refPoint, CENTERRING_TYPE type,
            double HeightA, double DistanceB, double DistanceC, double DistanceD, double ThkT1, double ThkT2, 
            double TankOD, double SlopeRadian, double OutDiameter)
        {

            List<Entity> outlinesList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            //////////////////////////////////////////////////////////
            // Input CAD DATA
            //////////////////////////////////////////////////////////

            double tankOD = TankOD;
            double slopeRadian = SlopeRadian;
            double outDiameter = OutDiameter;
            double heightA = HeightA;
            double distanceB = DistanceB;
            double distanceC = DistanceC;
            double distanceD = DistanceD;
            double thkT1 = ThkT1;
            double thkT2 = ThkT2;


            /////////////////////
            double domeRadius = tankOD / 2;
            double radiusOuter = outDiameter / 2;


            // Draw Flange & Web
            double flangeTopLength = 0;

            if (type == CENTERRING_TYPE.CRT_INT || type == CENTERRING_TYPE.DRT_INT)
            {
                flangeTopLength = distanceC + distanceD;
                outlinesList.AddRange(GetRectangle(GetSumPoint(referencePoint, 0, -thkT1), thkT1, flangeTopLength)); // Upper Flange
                outlinesList.AddRange(GetRectangle(GetSumPoint(referencePoint, distanceC, (-heightA + thkT1)), distanceB, thkT2)); // Web Flange
                outlinesList.AddRange(GetRectangle(GetSumPoint(referencePoint, 0, -heightA), thkT1, flangeTopLength)); // Low Flange
            }
            else
            {
                flangeTopLength = distanceB + distanceC;
                List<Line> upperFlangeList = GetRectangle(GetSumPoint(referencePoint, -distanceB, (heightA - thkT1)), thkT1, flangeTopLength); // Upper Flange

                // Draw : Web
                Line verticalLeftLine = new Line(CopyPoint(referencePoint), GetSumPoint(upperFlangeList[0].StartPoint, distanceB, 0));
                Line verticalRightLine = (Line)verticalLeftLine.Offset(thkT2, Vector3D.AxisZ);

                ICurve intersectEntity = null;

                // Draw : Low Flange
                if (type == CENTERRING_TYPE.CRT_EXT)
                {
                    // Draw Low Flange
                    List<Line> lowFlangeList = GetRectangle(GetSumPoint(referencePoint, -distanceB, 0), thkT1, flangeTopLength);
                    foreach (Line eachLine in lowFlangeList) { eachLine.Rotate(slopeRadian, Vector3D.AxisZ, lowFlangeList[0].MidPoint); }
                    outlinesList.AddRange(lowFlangeList);

                    // Index 2 : lowFlange TopLine
                    intersectEntity = lowFlangeList[2];
                }

                if (type == CENTERRING_TYPE.DRT_EXT)
                {
                    // Get center Point
                    double workPointAngle = Math.Asin(radiusOuter / domeRadius);
                    double workPointHeight = domeRadius * Math.Cos(workPointAngle);
                    Point3D domeCenterPoint = GetSumPoint(referencePoint, radiusOuter, -workPointHeight);
                    
                    // Draw Low Flange
                    double WorkPointRadian = Math.Asin((domeCenterPoint.X - referencePoint.X) / domeRadius);
                    double degToRad90 = Utility.DegToRad(90);
                    double totalWorkPointRadian = WorkPointRadian + degToRad90;
                    // L = R@(Radian) , @ = L/R    @ = distanceB/ domeR
                    double radianDistanceB = distanceB / domeRadius;
                    double radianDistanceC = distanceC / domeRadius;

                    Arc lowFlangeBottom = new Arc(domeCenterPoint, domeRadius,
                                                      (totalWorkPointRadian + radianDistanceB),
                                                      (totalWorkPointRadian + -radianDistanceC));

                    Arc lowFlangeTop = (Arc)lowFlangeBottom.Offset(-thkT1, Vector3D.AxisZ);

                    Line lowFlangeLeftLine = new Line(lowFlangeTop.StartPoint, lowFlangeBottom.StartPoint);
                    Line lowFlangeRightLine = new Line(lowFlangeTop.EndPoint, lowFlangeBottom.EndPoint);

                    outlinesList.AddRange(new Entity[] {
                        lowFlangeBottom, lowFlangeTop,
                        lowFlangeLeftLine, lowFlangeRightLine,
                    });

                    intersectEntity = lowFlangeTop;
                }
                // intersect FlangeTopLine with Web
                /*
                Point3D[] interSectLeft = lowFlangeList[2].IntersectWith(verticalLeftLine);
                Point3D[] interSectRight = lowFlangeList[2].IntersectWith(verticalRightLine);
                /**/

                Point3D[] interSectLeft = verticalLeftLine.IntersectWith(intersectEntity);
                Point3D[] interSectRight = verticalRightLine.IntersectWith(intersectEntity);

                // Trim of Web : bottom Vertical Line
                verticalLeftLine.TrimBy(interSectLeft[0], true);
                verticalRightLine.TrimBy(interSectRight[0], true);

                outlinesList.AddRange(upperFlangeList);
                outlinesList.AddRange(new Entity[] { verticalLeftLine, verticalRightLine, }); // Vertical Line
            }


            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

            return outlinesList;
        }


        //  Not Use 추후에 사용할수 있는지 검토
        public Point3D GetWorkPointOfCenterRIng(Point3D referencePoint, CENTERRING_TYPE type, double outDiameter, double slopeRadian,
                       double distanceB, double distanceC, out Point3D domeCenterPoint, double domeRadius = 0)
        {
            List<Entity> virtualLinesList = new List<Entity>();

            Point3D workPoint = null;
            double radiusOuter = outDiameter / 2;
            double halfDistanceE = 0;

            if (type == CENTERRING_TYPE.CRT_INT || type == CENTERRING_TYPE.DRT_INT)
            {
                halfDistanceE = radiusOuter + distanceC;
            }
            else
            {
                halfDistanceE = radiusOuter + distanceB;
            }

            // CRT W.P
            if (type == CENTERRING_TYPE.CRT_INT || type == CENTERRING_TYPE.CRT_EXT)
            {
                domeCenterPoint = null;
                Line roofLine = new Line(CopyPoint(referencePoint), GetSumPoint(referencePoint, -outDiameter, 0));
                roofLine.Rotate(slopeRadian, Vector3D.AxisZ, roofLine.StartPoint);

                // guideWP Line  : CRT_INT = E  ,  CRT_EXT = OD
                double shiftX = halfDistanceE;
                if (type == CENTERRING_TYPE.CRT_EXT) { shiftX = radiusOuter; }
                Line intersectForVLine = new Line(GetSumPoint(referencePoint, -shiftX, 0),
                                                  GetSumPoint(referencePoint, -shiftX, -outDiameter));

                Point3D[] tempWP = roofLine.IntersectWith(intersectForVLine);
                workPoint = tempWP[0];

                /**/
                virtualLinesList.AddRange(new Entity[] {
                roofLine, intersectForVLine
                }); /**/

            }
            else // DRT W.P
            {
                domeCenterPoint = GetSumPoint(referencePoint, 0, -domeRadius);
                Arc roofArc = new Arc(domeCenterPoint, domeRadius, Utility.DegToRad(90), Utility.DegToRad(110));
                //Circle guideCircle = new Circle(domeCenterPoint, domeRadius);
                // guideWP Line  : DRT_INT = E  ,  DRT_EXT = OD
                double shiftX = halfDistanceE;
                if (type == CENTERRING_TYPE.DRT_EXT) { shiftX = radiusOuter; }
                Line intersectForVLine = new Line(GetSumPoint(referencePoint, -shiftX, 0),
                                                  GetSumPoint(referencePoint, -shiftX, -outDiameter));

                Point3D[] tempWP = roofArc.IntersectWith(intersectForVLine);
                workPoint = tempWP[0];
            }

            return workPoint;
        }

        //public List<Entity> GetCenterRing_Flange(Point3D refPoint, CENTERRING_TYPE type,
        //    double heightA, double distanceB, double distanceC, double distanceD, double thkT1, double thkT2,
        //    double slopeRadian, double domeRadius = 0, Point3D domeCenterPoint = null)
        //{

        //    List<Entity> outlinesList = new List<Entity>();
        //    Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

        //    //////////////////////////////////////////////////////////
        //    // CAD DATA
        //    //////////////////////////////////////////////////////////
        //    double flangeTopLength = 0;


        //    // Draw Flange & Web
        //    if (type == CENTERRING_TYPE.CRT_INT || type == CENTERRING_TYPE.DRT_INT)
        //    {
        //        flangeTopLength = distanceC + distanceD;
        //        outlinesList.AddRange(GetRectangle(GetSumPoint(referencePoint, 0, -thkT1), thkT1, flangeTopLength)); // Upper Flange
        //        outlinesList.AddRange(GetRectangle(GetSumPoint(referencePoint, distanceC, (-heightA + thkT1)), distanceB, thkT2)); // Web Flange
        //        outlinesList.AddRange(GetRectangle(GetSumPoint(referencePoint, 0, -heightA), thkT1, flangeTopLength)); // Low Flange
        //    }
        //    else
        //    {
        //        flangeTopLength = distanceB + distanceC;
        //        List<Line> upperFlangeList = GetRectangle(GetSumPoint(referencePoint, -distanceB, (heightA - thkT1)), thkT1, flangeTopLength); // Upper Flange

        //        // Draw : Web
        //        Line verticalLeftLine = new Line(CopyPoint(referencePoint), GetSumPoint(upperFlangeList[0].StartPoint, distanceB, 0));
        //        Line verticalRightLine = (Line)verticalLeftLine.Offset(thkT2, Vector3D.AxisZ);

        //        ICurve intersectEntity = null;

        //        // Draw : Low Flange
        //        if (type == CENTERRING_TYPE.CRT_EXT)
        //        {
        //            // Draw Low Flange
        //            List<Line> lowFlangeList = GetRectangle(GetSumPoint(referencePoint, -distanceB, 0), thkT1, flangeTopLength);
        //            foreach (Line eachLine in lowFlangeList) { eachLine.Rotate(slopeRadian, Vector3D.AxisZ, lowFlangeList[0].MidPoint); }
        //            outlinesList.AddRange(lowFlangeList);

        //            // Index 2 : lowFlange TopLine
        //            intersectEntity = lowFlangeList[2];
        //        }

        //        if (type == CENTERRING_TYPE.DRT_EXT)
        //        {
        //            // Draw Low Flange
        //            double WorkPointRadian = Math.Asin((domeCenterPoint.X - referencePoint.X) / domeRadius);
        //            double degToRad90 = Utility.DegToRad(90);
        //            double totalWorkPointRadian = WorkPointRadian + degToRad90;
        //            // L = R@(Radian) , @ = L/R    @ = distanceB/ domeR
        //            double radianDistanceB = distanceB / domeRadius;
        //            double radianDistanceC = distanceC / domeRadius;

        //            Arc lowFlangeBottom = new Arc(domeCenterPoint, domeRadius,
        //                                              (totalWorkPointRadian + radianDistanceB),
        //                                              (totalWorkPointRadian + -radianDistanceC));

        //            Arc lowFlangeTop = (Arc)lowFlangeBottom.Offset(-thkT1, Vector3D.AxisZ);

        //            Line lowFlangeLeftLine = new Line(lowFlangeTop.StartPoint, lowFlangeBottom.StartPoint);
        //            Line lowFlangeRightLine = new Line(lowFlangeTop.EndPoint, lowFlangeBottom.EndPoint);

        //            outlinesList.AddRange(new Entity[] {
        //                lowFlangeBottom, lowFlangeTop,
        //                lowFlangeLeftLine, lowFlangeRightLine,
        //            });

        //            intersectEntity = lowFlangeTop;
        //        }
        //        // intersect FlangeTopLine with Web
        //        /*
        //        Point3D[] interSectLeft = lowFlangeList[2].IntersectWith(verticalLeftLine);
        //        Point3D[] interSectRight = lowFlangeList[2].IntersectWith(verticalRightLine);
        //        /**/

        //        Point3D[] interSectLeft = verticalLeftLine.IntersectWith(intersectEntity);
        //        Point3D[] interSectRight = verticalRightLine.IntersectWith(intersectEntity);

        //        // Trim of Web : bottom Vertical Line
        //        verticalLeftLine.TrimBy(interSectLeft[0], true);
        //        verticalRightLine.TrimBy(interSectRight[0], true);

        //        outlinesList.AddRange(upperFlangeList);
        //        outlinesList.AddRange(new Entity[] { verticalLeftLine, verticalRightLine, }); // Vertical Line
        //    }


        //    styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

        //    return outlinesList;
        //}



        public List<Entity> GetDrtEXTRafter(Point3D refWorkPoint, double headGap, double centerRingThk, double heightA, double headHLength,
                                            double beamThk, double domeRadius, Point3D ArcCenterPoint, double rafterAngle)
        {
            // headHeight : Rafter Head Y축 높이
            // headHLength :Rafter Head X축 길이 ( @ : (OD+B) +30 )
            // rafterAngel : WorkPoint 부터 왼쪽 fafter 끝까지의 각도, 혹은 호의 길이로 각도 넣기
            //               호의 길이로 넣을때는 Rafter Body + Head 전체 길이
            //
            //               ArcLength(L)  L = R@, @(radian) = L/R
            // rafterLength : rafterAngelRadian = rafterLength/domeRadius;
            //                rafterAngel = Utility.DegToRad(rafterAngelRadian);


            List<Entity> outlinesList = new List<Entity>();
            Point3D workPoint = GetSumPoint(refWorkPoint, 0, 0);
            Point3D arcCenterPoint = GetSumPoint(ArcCenterPoint, 0, 0);


            //////////////////////////////////////////////////////////
            // CAD DATA
            //////////////////////////////////////////////////////////
            double headHeight = heightA - (headGap * 2) - (centerRingThk * 2);
            double rafterAddWidth = 0;  // centerRing 과 Rafter 연결부의 단차를 줄이기 위함 ( Dome의 R값이 10000 이하는 단차가 생김, 10 추가하면 맞아짐)
            double RafterWidth = heightA + rafterAddWidth;
            // Get WorkPoint angle
            Line getAngleLine = new Line(ArcCenterPoint, workPoint);
            double radianWP = getAngleLine.Direction.Angle;
            double angleWP = Utility.RadToDeg(radianWP);
            /**/

            // Get Rafter Head GuideLine
            Line guideHeadVRLine = new Line(GetSumPoint(workPoint, 0, -headHeight), GetSumPoint(workPoint, 0, headHeight * 2));
            Line guideHeadVLLine = new Line(GetSumPoint(guideHeadVRLine.StartPoint, -headHLength, 0), GetSumPoint(guideHeadVRLine.EndPoint, -headHLength, 0));

            // Get Guide Arc
            Arc guideArcBottom = new Arc(arcCenterPoint, domeRadius, Utility.DegToRad(90), Utility.DegToRad(angleWP + rafterAngle));
            Arc guideArcBottomOffset = (Arc)guideArcBottom.Offset(beamThk, Vector3D.AxisZ);
            Arc guideArcCenterRingBottm = (Arc)guideArcBottom.Offset(centerRingThk, Vector3D.AxisZ);
            Arc guideArcHeadBottom = (Arc)guideArcCenterRingBottm.Offset(headGap, Vector3D.AxisZ);

            Arc guideArcTop = (Arc)guideArcBottom.Offset(RafterWidth, Vector3D.AxisZ);
            Arc guideArcTopOffset = (Arc)guideArcTop.Offset(-beamThk, Vector3D.AxisZ);



            // intersectPoint
            Point3D[] GheadRightVLineEndPoint = guideHeadVRLine.IntersectWith(guideArcHeadBottom);
            Point3D[] GheadLeftVLineBottomInnerPoint = guideHeadVLLine.IntersectWith(guideArcHeadBottom);
            Point3D[] GheadLeftVLineBottomOffsetPoint = guideHeadVLLine.IntersectWith(guideArcBottomOffset);
            Point3D[] GheadLeftVLineBottomPoint = guideHeadVLLine.IntersectWith(guideArcBottom);
            Point3D[] GheadLeftVLineTopPoint = guideHeadVLLine.IntersectWith(guideArcTop);
            Point3D[] GheadLeftVLineTopOffsetPoint = guideHeadVLLine.IntersectWith(guideArcTopOffset);
            Point3D headRightVLineEndPoint = CopyPoint(GheadRightVLineEndPoint[0]);
            Point3D headLeftVLineBottomInnerPoint = CopyPoint(GheadLeftVLineBottomInnerPoint[0]);
            Point3D headLeftVLineBottomOffsetPoint = CopyPoint(GheadLeftVLineBottomOffsetPoint[0]);
            Point3D headLeftVLineBottomPoint = CopyPoint(GheadLeftVLineBottomPoint[0]);
            Point3D headLeftVLineTopPoint = CopyPoint(GheadLeftVLineTopPoint[0]);
            Point3D headLeftVLineTopOffsetPoint = CopyPoint(GheadLeftVLineTopOffsetPoint[0]);

            // Draw Rafter Head Line
            Line headRightLine = new Line(headRightVLineEndPoint, GetSumPoint(headRightVLineEndPoint, 0, headHeight));
            //double headRLineLength = headRightLine.Length();
            Line headTopLine = new Line(headRightLine.EndPoint, GetSumPoint(headRightLine.EndPoint, -headHLength, 0));
            Line headLeftVTopLine = new Line(headTopLine.EndPoint, headLeftVLineTopPoint); //////
            
            Line headLeftVBottomLine = new Line(headLeftVLineBottomInnerPoint, headLeftVLineBottomPoint);
            Arc headBottomArc = new Arc(arcCenterPoint, headLeftVLineBottomInnerPoint, headRightVLineEndPoint);


            // Draw Rafter Body
            Arc rafterBottomArc = new Arc(arcCenterPoint, headLeftVLineBottomPoint, guideArcBottom.EndPoint);
            Arc rafterBottomOffsetArc = new Arc(arcCenterPoint, headLeftVLineBottomOffsetPoint, guideArcBottomOffset.EndPoint);

            Arc rafterTopArc = new Arc(arcCenterPoint, headLeftVLineTopPoint, guideArcTop.EndPoint);
            Arc rafterTopOffsetArc = new Arc(arcCenterPoint, headLeftVLineTopOffsetPoint, guideArcTopOffset.EndPoint);

            Line leftLine = new Line(guideArcTop.EndPoint, guideArcBottom.EndPoint);
            //double leftLineLength = leftLine.Length();

            outlinesList.AddRange(new Entity[] {
                headRightLine, headTopLine, headLeftVTopLine, headLeftVBottomLine, headBottomArc,
                rafterBottomArc, rafterBottomOffsetArc, leftLine,
                rafterTopArc, rafterTopOffsetArc,
                //guideHeadVRLine,
                //guideHeadVLLine, 
                //guideArcBottom, guideArcBottomInner, guideArcHeadBottom
                //guideArcTop, 
                // guideArcTopOffset
            });

            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

            return outlinesList;
        }

        public List<Entity> GetDrtEXTCenterRing(Point3D refPoint, double outDiameter, double diaInnerThk, double diaOuterThk, double heightA, 
                double DomeRadius,double plateThkHorizontal, double plateThkVertial, out Point3D WorkingPoint, out Point3D ArcCenterPoint)
        {

            List<Entity> outlinesList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            //////////////////////////////////////////////////////////
            // CAD DATA
            //////////////////////////////////////////////////////////
            ///

            // Todo : OD , ID   1/2 로 수정해서 적용할 것

            double radiusOD = outDiameter / 2;
            double radiusID = radiusOD - diaInnerThk;
            double sideDiameter = outDiameter + diaOuterThk;
            
            double enumA = heightA;
            double enumB = diaOuterThk;
            double enumC = diaInnerThk;
            double enumE = radiusOD + enumB;

            double cRightAngle2 = Utility.RadToDeg(Math.Asin(enumC / (2 * DomeRadius))) *2;


            // Get Arc
            Circle guideCircle = new Circle(referencePoint, DomeRadius);
            Line guideLineWP_InterSect = new Line(GetSumPoint(referencePoint, -radiusOD, 0), GetSumPoint(referencePoint, -radiusOD, DomeRadius * 2));
            Point3D[] tempWP = guideLineWP_InterSect.IntersectWith(guideCircle);
            //Point3D WorkPoint = tempWP[0];
            WorkingPoint = tempWP[0];
            Point3D workPoint = WorkingPoint;

            Line getAngleLine = new Line(referencePoint, tempWP[0]);
            double radianWP = getAngleLine.Direction.Angle;
            double angleWP = (Utility.RadToDeg(radianWP));
            double angleWPofVetical = angleWP - 90;
            
            /*
            //double bottomArcHeight = GetArcHeight(DomeRadius, angleWPofVetical);
            double workingPointY = GetArcSidePointHeight(DomeRadius, angleWPofVetical*2);

            // Line newTestLine = new Line(referencePoint, )
            double arcCenterY = -workingPointY;  //- (DomeRadius + bottomArcHeight);
            Point3D arcCenterPoint = GetSumPoint(referencePoint, 0, arcCenterY);
            /**/
            ArcCenterPoint = CopyPoint(referencePoint);
            Point3D arcCenterPoint = ArcCenterPoint;
            Arc guideArcBottom = new Arc(ArcCenterPoint, DomeRadius, Utility.DegToRad(90), radianWP);
            Arc guideArcTop = (Arc)guideArcBottom.Offset(plateThkHorizontal, Vector3D.AxisZ);

            /*
            // Draw Vertical Plate
            Point3D workPoint = GetSumPoint(tempWP[0], 0, arcCenterY);
            WorkingPoint = workPoint;
            /**/
            Line guideVeticalLeftLine = new Line(workPoint, GetSumPoint(workPoint, 0, enumA - plateThkHorizontal));
            Line guideVeticalRightLine = new Line(GetSumPoint(guideVeticalLeftLine.StartPoint, plateThkVertial,0), 
                                                  GetSumPoint(guideVeticalLeftLine.EndPoint, plateThkVertial,0));

            Point3D[] intersectArcTopLeft = guideVeticalLeftLine.IntersectWith(guideArcTop);
            Point3D[] intersectArcTopRight = guideVeticalRightLine.IntersectWith(guideArcTop);

            Line verticalLeftLine = new Line(guideVeticalLeftLine.EndPoint, intersectArcTopLeft[0]);
            Line verticalRightLine = new Line(guideVeticalRightLine.EndPoint, intersectArcTopRight[0]);
            //double leftLineLength = verticalLeftLine.Length();
            
            // Draw Top Plate
            Point3D topPlateStartPoint = GetSumPoint(verticalLeftLine.StartPoint, -enumB, 0);
            List<Line> topPlateList = GetRectangle(topPlateStartPoint, plateThkHorizontal, enumE, RECTANGLUNVIEW.RIGHT);
            
            Line indiameterVLine = new Line(GetSumPoint(workPoint, enumC, enumA),
                                            GetSumPoint(workPoint, enumC, enumA-plateThkHorizontal));


            // Draw Bottom Plate (Arc)
            double ArcRightAngleRadian = enumC / DomeRadius;    // ArcLength(L)  L = R@, @(radian) = L/R
            double ArcLeftAngleRadian = enumB / DomeRadius;     // Asin(CHL/2R)*2
            double arcRightAngle = Utility.RadToDeg(ArcRightAngleRadian);
            double arcLeftAngle = Utility.RadToDeg(ArcLeftAngleRadian);



            Arc arcBottomRight = new Arc(arcCenterPoint, DomeRadius, radianWP, Utility.DegToRad(angleWP - arcRightAngle));
            Arc arcBottomLeft = new Arc(arcCenterPoint, DomeRadius, radianWP, Utility.DegToRad(angleWP + arcLeftAngle));

            Arc arcTopRight = (Arc)arcBottomRight.Offset(-plateThkHorizontal, Vector3D.AxisZ);
            Arc arcTopLeft = (Arc)arcBottomLeft.Offset(plateThkHorizontal, Vector3D.AxisZ);

            Line diagonalLeft = new Line(arcTopLeft.EndPoint, arcBottomLeft.EndPoint);
            Line diagonalRight = new Line(arcTopRight.EndPoint, arcBottomRight.EndPoint);


            outlinesList.AddRange(topPlateList);
            outlinesList.AddRange(new Entity[] {
                guideArcBottom, 
                //guideArcTop,
                //guideVeticalLeftLine, guideVeticalRightLine,
                guideCircle,
                guideLineWP_InterSect,
                verticalLeftLine, verticalRightLine,

                indiameterVLine,

                arcBottomRight,arcBottomLeft, arcTopRight, arcTopLeft,
                diagonalLeft, diagonalRight
            });

            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

            return outlinesList;
        }

        public List<Entity> GetCRTRafter(Point3D refPoint, double width, double length, double HThk, double holeRadius, RECTANGLUNVIEW direction = RECTANGLUNVIEW.LEFT)
        {

            List<Entity> outlinesList = new List<Entity>();
            List<Line> doubleLineRectangleList = new List<Line>();
            List<Entity> holeList = new List<Entity>();

            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            //////////////////////////////////////////////////////////
            // CAD DATA
            //////////////////////////////////////////////////////////
            double intersectCutAngle = 15;
            double intersectCutRemain = width / 2.5;

            double holeSideGap = 40;
            double holeBetweenGap = 70;
            double holeWidthGap = 100;  // input data??   홀의 위아래 CP간격

            //////////////////////////////////////////////////////////
            // Draw 
            //////////////////////////////////////////////////////////
            doubleLineRectangleList = GetDoubleLineRectangle(referencePoint, width, length, HThk, direction);
            
            Line guideHoleLine = null;
            Point3D HoleCenterPoint = null;
            double holeCenterPointShift = holeSideGap + (holeBetweenGap / 2);
            switch (direction)
            {
                case RECTANGLUNVIEW.LEFT:
                    guideHoleLine = doubleLineRectangleList[1];
                    HoleCenterPoint = GetSumPoint(guideHoleLine.MidPoint, -holeCenterPointShift, 0);
                    break;

                case RECTANGLUNVIEW.RIGHT:
                    guideHoleLine = doubleLineRectangleList[2];
                    HoleCenterPoint = GetSumPoint(guideHoleLine.MidPoint, holeCenterPointShift, 0);
                    break;
            }

            holeList = GetHoles(HoleCenterPoint, holeRadius, holeBetweenGap, holeWidthGap);

            outlinesList.AddRange(doubleLineRectangleList);
            outlinesList.AddRange(holeList);
            outlinesList.AddRange(new Entity[] {

            });

            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

            return outlinesList;
        }
        public List<Entity> GetCRTRafterTopView(Point3D refPoint, double length, double height, double VThk, double HThk, double holeRadius, RECTANGLUNVIEW unView = RECTANGLUNVIEW.LEFT, bool isTopview = false)
        {

            List<Entity> outlinesList = new List<Entity>();
            List<Line> doubleLineRectangleList = new List<Line>();
            List<Entity> holeList = new List<Entity>();

            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            //////////////////////////////////////////////////////////
            // CAD DATA
            //////////////////////////////////////////////////////////
            double intersectCutAngle = 15;
            double intersectCutRemain = height / 2.5;

            double holeSideGap = 40;
            double holeBetweenGap = 70;
            double holeWidthGap = 100;  // input data??   홀의 위아래 CP간격

            //////////////////////////////////////////////////////////
            // Draw 
            //////////////////////////////////////////////////////////
            doubleLineRectangleList = GetDoubleLineRectangle(referencePoint, height, length, HThk, unView);

            Line guideHoleLine = null;
            Point3D HoleCenterPoint = null;
            double holeCenterPointShift = holeSideGap + (holeBetweenGap / 2);
            switch (unView)
            {
                case RECTANGLUNVIEW.LEFT:
                    guideHoleLine = doubleLineRectangleList[1];
                    HoleCenterPoint = GetSumPoint(guideHoleLine.MidPoint, -holeCenterPointShift, 0);
                    break;

                case RECTANGLUNVIEW.RIGHT:
                    guideHoleLine = doubleLineRectangleList[2];
                    HoleCenterPoint = GetSumPoint(guideHoleLine.MidPoint, holeCenterPointShift, 0);
                    break;
            }

            holeList = GetHoles(HoleCenterPoint, holeRadius, holeBetweenGap, holeWidthGap);

            outlinesList.AddRange(doubleLineRectangleList);
            outlinesList.AddRange(holeList);
            outlinesList.AddRange(new Entity[] {

            });

            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

            return outlinesList;
        }

        public List<Entity> GetDripRing(Point3D refPoint)
        {

            List<Entity> allOutLineList = new List<Entity>();
            List<Entity> outlinesList = new List<Entity>();
            List<Line> concreteBoxleList = new List<Line>();
            List<Line> virtualPlateList = new List<Line>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            //////////////////////////////////////////////////////////
            // CAD DATA
            //////////////////////////////////////////////////////////
            double concreteWidth = 25.2;
            double concreteLength = 40;

            double topPlateWidth = 13.9;
            double topPlateLength = 1.5;
            double topPlateXGap = 9.3;   

            double bottomPlateWidth = 2;
            double bottomPlateLength = 15.3;
            double bottomPlateXGap = 3.6;

            double splineGap = 3;

            double DripRIngWidth = 0.5;
            double topDripRIngLength = 29;
            double BottomDripRIngLength = 3;


            //////////////////////////////////////////////////////////
            // Draw 
            //////////////////////////////////////////////////////////

            // Draw Boxes
            Point3D bottomPlateStartPoint = GetSumPoint(refPoint, bottomPlateXGap, concreteWidth); ;
            List<Line> bottomPlateList = GetRectangle(bottomPlateStartPoint, bottomPlateWidth, bottomPlateLength, RECTANGLUNVIEW.LEFT);
            virtualPlateList.AddRange(bottomPlateList);

            Point3D topPlateStartPoint = GetSumPoint(bottomPlateStartPoint, topPlateXGap, bottomPlateWidth); ;
            List<Line> topPlateList = GetRectangle(topPlateStartPoint, topPlateWidth, topPlateLength, RECTANGLUNVIEW.TOP);
            virtualPlateList.AddRange(topPlateList);

            concreteBoxleList = GetRectangle(refPoint, concreteWidth, concreteLength, RECTANGLUNVIEW.LEFT);
            outlinesList.AddRange(concreteBoxleList);


            // Draw Spline
            List<Point3D> splinePointList = new List<Point3D>();

            Line splineGuideLine = new Line(concreteBoxleList[2].StartPoint, concreteBoxleList[0].StartPoint);
            Line splineTopGuide = new Line(splineGuideLine.StartPoint, splineGuideLine.MidPoint);
            Line splineBottomGuide = new Line(splineGuideLine.MidPoint, splineGuideLine.EndPoint);

            splinePointList.Add(GetSumPoint(splineTopGuide.StartPoint, 0, 0));
            splinePointList.Add(GetSumPoint(splineTopGuide.MidPoint, -splineGap, 0));
            splinePointList.Add(GetSumPoint(splineTopGuide.EndPoint, 0, 0));
            splinePointList.Add(GetSumPoint(splineBottomGuide.MidPoint, splineGap, 0));
            splinePointList.Add(GetSumPoint(splineBottomGuide.EndPoint, 0, 0));

            Curve bottomSpline = Curve.CubicSplineInterpolation(splinePointList);
            outlinesList.Add(bottomSpline);


            // Draw Drip Ring
            Point3D topDripRIngStartPoint = GetSumPoint(bottomPlateList[0].EndPoint, 0, 0); ;
            List<Line> topDripRIngList = GetRectangle(topDripRIngStartPoint, DripRIngWidth, topDripRIngLength);
            outlinesList.AddRange(topDripRIngList);

            Point3D bottomDripRIngStartPoint = GetSumPoint(topDripRIngList[0].EndPoint, 0, 0); ;
            List<Line> bottomDripRIngList = GetRectangleLT(bottomDripRIngStartPoint, BottomDripRIngLength, DripRIngWidth);
            outlinesList.AddRange(bottomDripRIngList);


            outlinesList.AddRange(new Entity[] {

            });

            styleService.SetLayerListLine(ref virtualPlateList, layerService.LayerVirtualLine);
            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

            allOutLineList.AddRange(virtualPlateList);
            allOutLineList.AddRange(outlinesList);

            return allOutLineList;
        }

        // GetRepeatRotateLineBetweenCircle (보류 : 만들다가 멈춤)
        #region
        /*
        public List<Entity> GetRepeatRotateLineBetweenCircle(Point3D refPoint, double innerRadius, double outerRadius)
        {

            // intersectCount : index번호로 사용,
            // intersect로 잘라내야할 entity 갯수(현재는 Line만 Cut)  (guideCircle 바깥쪽만 남기고 안쪽은 잘라냄)
            // Line의 Start/EndPoint :  Start(원의 중심부터 먼곳에서)  End(Center쪽으로)

            List<Entity> outLineList = new List<Entity>();
            Point3D referencePoint = CopyPoint(refPoint);

            // Data
            double eachAngel = 360.0 / count;
            double countLIst = entityList.Count;


            Circle guideCircle = new Circle(referencePoint, radius);

            List<Line> intersectLineLIst = new List<Line>();

            // Intersect and Line Trim
            for (int i = 0; i < intersectCount; i++)
            {
                intersectLineLIst.Add((Line)entityList[i].Clone());

                Point3D[] intersectPoint = intersectLineLIst[i].IntersectWith(guideCircle);
                intersectLineLIst[i].EndPoint = intersectPoint[0];

                entityList[i] = intersectLineLIst[i];
            }


            // Rotate
            for (int k = 0; k < count; k++)
            {
                for (int j = 0; j < countLIst; j++)
                {
                    // 시작각도는 좌우 대칭이 되도록 중앙이 아닌 반절 각도로 시작
                    if (k == 0) { entityList[j].Rotate(Utility.DegToRad(eachAngel / 2), Vector3D.AxisZ, referencePoint); }
                    else { entityList[j].Rotate(Utility.DegToRad(eachAngel), Vector3D.AxisZ, referencePoint); }

                    outLineList.Add((Entity)entityList[j].Clone());
                }
            }

            return outLineList;
        }
        /**/
        #endregion






        /// <summary>
        /// Functions : Make Basic Entity

        public List<Entity> GetRepeatRotateEntity(Point3D refPoint, double radius, double count, List<Entity> entityList, double intersectCount, double Rotate = 360)
        {
            // Rotate : 0 - 무회전
            //          360 - 시작각도가 좌우 대칭되도록 조정
            //          그외 - 각도입력 회전

            // intersectCount : index번호로 사용,
            // intersect로 잘라내야할 entity 갯수(현재는 Line만 Cut)  (guideCircle 바깥쪽만 남기고 안쪽은 잘라냄)
            // Line의 Start/EndPoint :  Start(원의 중심부터 먼곳에서)  End(Center쪽으로)

            List<Entity> outLineList = new List<Entity>();
            Point3D referencePoint = CopyPoint(refPoint);

            // Data
            double eachAngel = 360.0 / count;
            double countLIst = entityList.Count;


            Circle guideCircle = new Circle(referencePoint, radius);

            List<Line> intersectLineLIst = new List<Line>();

            // Intersect and Line Trim
            for (int i = 0; i < intersectCount; i++)
            {
                intersectLineLIst.Add((Line)entityList[i].Clone());

                Point3D[] intersectPoint = intersectLineLIst[i].IntersectWith(guideCircle);
                intersectLineLIst[i].EndPoint = intersectPoint[0];

                entityList[i] = intersectLineLIst[i];
            }


            // Rotate
            for (int k = 0; k < count; k++)
            {
                for(int j=0; j< countLIst; j++ )
                {
                    // 시작각도는 좌우 대칭이 되도록 중앙이 아닌 반절 각도로 시작
                    if (k == 0) {
                        double radian = 0;

                        if (Rotate == 0) { }
                        else if(Rotate == 360) radian = Utility.DegToRad(eachAngel / 2);
                        else radian = Utility.DegToRad(Rotate);

                        entityList[j].Rotate(radian, Vector3D.AxisZ, referencePoint); 
                    }
                    else { entityList[j].Rotate(Utility.DegToRad(eachAngel), Vector3D.AxisZ, referencePoint); }

                    outLineList.Add((Entity)entityList[j].Clone());
                }
            }

            return outLineList;
        }

        private Line GetChamferLine(Line HorizontalLine, Line VerticalLine, double ChamferLength, bool isTrim = false)
        {
            // HorizontalLine : Start(Left) End(Right)
            // VerticalLine : Start(Top), End(Bottom)

            // Return Line (HorizontalLinePoint, VerticalLinePoint)

            Line ChamferLine = (Line)HorizontalLine.Clone(); // 임시값으로 넣어줌
            double cornerIndex = 0; // 0 : BL, 1 BR, 2 TR, 3 TL

            if (HorizontalLine.EndPoint == VerticalLine.EndPoint) cornerIndex = 1;
            else if (HorizontalLine.EndPoint == VerticalLine.StartPoint) cornerIndex = 2;
            else if (HorizontalLine.StartPoint == VerticalLine.StartPoint) cornerIndex = 3;


            if (cornerIndex == 0 || cornerIndex == 3)
            {
                ChamferLine.StartPoint = GetSumPoint(HorizontalLine.StartPoint, ChamferLength, 0);

                if (cornerIndex == 0) ChamferLine.EndPoint = GetSumPoint(VerticalLine.EndPoint, 0, ChamferLength);
                else ChamferLine.EndPoint = GetSumPoint(VerticalLine.StartPoint, 0, -ChamferLength);
            }
            else
            { // index 1, 2
                ChamferLine.StartPoint = GetSumPoint(HorizontalLine.EndPoint, -ChamferLength, 0);

                if (cornerIndex == 1) ChamferLine.EndPoint = GetSumPoint(VerticalLine.EndPoint, 0, ChamferLength);
                else ChamferLine.EndPoint = GetSumPoint(VerticalLine.StartPoint, 0, -ChamferLength);
            }

            return ChamferLine;
        }

        public List<Entity> GetSpline(Line guideLine, double splineHeight, SHAPEDIRECTION startArcDirection,
                                      bool isHorizontal=false, bool isSingleArc = false)
        {
            // single Arc : S 자 모양
            // double Arc : 3 자 모양
            // default direction : Vertical-Left, Horizontal-Top

            List<Entity> splineList = new List<Entity>();

            
            double singletype = 1;
            double direction = 1;

            double heightX = 0;
            double heightY = 0;
            double lineRadian = 0;

            bool isSlope = false;
            Line newGuideLine = null;


            if (isSingleArc) { singletype = -1; }
            if (startArcDirection == SHAPEDIRECTION.RIGHT || startArcDirection == SHAPEDIRECTION.BOTTOM) 
               { direction = -1; }

            if (isHorizontal) { heightY = splineHeight; }
            else { heightX = splineHeight; }


            // Slope Check
            if (isHorizontal)
            {
                if (guideLine.StartPoint.Y != guideLine.EndPoint.Y)
                {
                    // todo : 0 도로 맞추기
                    isSlope = true;

                    Vector3D lineDirection = guideLine.Direction;


                    lineRadian = lineDirection.Angle;
                    guideLine.Rotate(-lineRadian, Vector3D.AxisZ, guideLine.MidPoint);
                }
            }
            else 
            {
                if (guideLine.StartPoint.X != guideLine.EndPoint.X)
                {
                    // todo :  270 or 90 도로 맞추고 되돌리기
                    isSlope = true;

                    Vector3D lineDirection = guideLine.Direction;


                    lineRadian = lineDirection.Angle;
                    guideLine.Rotate(-lineRadian, Vector3D.AxisZ, guideLine.MidPoint);
                }
            }
            
            newGuideLine = (Line)guideLine.Clone();

            // Draw Spline
            List<Point3D> splinePointList = new List<Point3D>();

            Line splineTopGuide = new Line(newGuideLine.StartPoint, newGuideLine.MidPoint);
            Line splineBottomGuide = new Line(newGuideLine.MidPoint, newGuideLine.EndPoint);

            splinePointList.Add(GetSumPoint(splineTopGuide.StartPoint, 0, 0));
            splinePointList.Add(GetSumPoint(splineTopGuide.MidPoint, -heightX * direction, heightY * direction));
            splinePointList.Add(GetSumPoint(splineTopGuide.EndPoint, 0, 0));
            splinePointList.Add(GetSumPoint(splineBottomGuide.MidPoint, -heightX * singletype * direction, heightY * singletype * direction));
            splinePointList.Add(GetSumPoint(splineBottomGuide.EndPoint, 0, 0));

            Curve spline = Curve.CubicSplineInterpolation(splinePointList);

            if (isSlope) spline.Rotate(lineRadian, Vector3D.AxisZ, newGuideLine.MidPoint);
            splineList.Add(spline);

            return splineList;
        }

        public List<Entity> GetManyCircles(Point3D refPoint, double Radius, List<Point3D> pointList)
        {
            List<Entity> circleList = new List<Entity>();

            foreach (Point3D eachPoint in pointList)
            {
                Circle eachCircle = new Circle(eachPoint, Radius);
                circleList.Add(eachCircle);
            }

            styleService.SetLayerListEntity(ref circleList, layerService.LayerOutLine);
            return circleList;
        }

        public List<Entity> GetTriangle(Point3D refPoint, double length, double angle = 0)
        {
            Point3D RefPoint = GetSumPoint(refPoint, 0, 0);
            List<Entity> triangleList = new List<Entity>();

            Line bottomLine = new Line(GetSumPoint(RefPoint, 0, 0), GetSumPoint(RefPoint, length, 0));

            Line leftLine = new Line(GetSumPoint(RefPoint, 0, 0), GetSumPoint(RefPoint, length, 0));
            leftLine.Rotate(Utility.DegToRad(60), Vector3D.AxisZ, RefPoint);

            Line rightLine = new Line(GetSumPoint(RefPoint, 0, 0), GetSumPoint(bottomLine.EndPoint, 0, 0));
            rightLine.Rotate(Utility.DegToRad(-60), Vector3D.AxisZ, bottomLine.EndPoint);

            triangleList.AddRange(new Line[] {
                bottomLine, leftLine, rightLine
            });

            if (angle != 0)
            {
                foreach (Line eachLine in triangleList)
                {
                    eachLine.Rotate(Utility.DegToRad(angle), Vector3D.AxisZ, RefPoint);
                }
            }

            styleService.SetLayerListEntity(ref triangleList, layerService.LayerOutLine);
            return triangleList;
        }

        public List<Entity> GetColumnPipe(Point3D refPoint, double outerDiameter, double height, VIEWPOSITION viewPosition, bool isInnerView=false, double thk=0)
        {
            // refPoint : bottom Center
            //drawPosition 0: ViewUp, 1: ViewDown, 2: VIewUp&Down

            List<Entity> EntityList = new List<Entity>();
            List<Entity> outlinesList = new List<Entity>();
            List<Entity> splinesList = new List<Entity>();
            List<Line> rectangleList = new List<Line>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            double arcHeight = outerDiameter / 20;  // OuterDiameter * 0.1 / 2(위,아래)

            ////////////////////////////////////////////// ///////////////////////////////////////////
            double radiusOD = outerDiameter / 2;
            Point3D drawBottomCenterPoint = GetSumPoint(referencePoint, -radiusOD, 0);

            rectangleList = GetRectangle(drawBottomCenterPoint, height, outerDiameter);
            Line leftLine = rectangleList[3];
            Line rightLine = rectangleList[1];
            Line guideLineTop = rectangleList[2];
            Line guideLineBottom = rectangleList[0];

            if (viewPosition == VIEWPOSITION.TOP || viewPosition == VIEWPOSITION.TOP_BOTTOM)
            {
                if (viewPosition == VIEWPOSITION.TOP) { outlinesList.Add(guideLineBottom); }

                Line guideLineTopLeft = new Line(guideLineTop.StartPoint, guideLineTop.MidPoint);
                Line guideLineTopRight = new Line(guideLineTop.MidPoint, guideLineTop.EndPoint);

                Arc TopLeftUp = new Arc(guideLineTopLeft.StartPoint,
                                        GetSumPoint(guideLineTopLeft.MidPoint, 0, arcHeight),
                                        guideLineTopLeft.EndPoint,
                                        false);

                Arc TopLeftDown = new Arc(guideLineTopLeft.StartPoint,
                                        GetSumPoint(guideLineTopLeft.MidPoint, 0, -arcHeight),
                                        guideLineTopLeft.EndPoint,
                                        false);

                Arc TopRightUp = new Arc(guideLineTopRight.StartPoint,
                                        GetSumPoint(guideLineTopRight.MidPoint, 0, arcHeight),
                                        guideLineTopRight.EndPoint,
                                        false);

                splinesList.AddRange(new Entity[] {
                    TopLeftUp, TopLeftDown, TopRightUp
                });
            }

            if (viewPosition == VIEWPOSITION.BOTTOM || viewPosition == VIEWPOSITION.TOP_BOTTOM)
            {
                if (viewPosition == VIEWPOSITION.BOTTOM) { outlinesList.Add(guideLineTop); }

                Line guideLineBottomLeft = new Line(guideLineBottom.StartPoint, guideLineBottom.MidPoint);
                Line guideLineBottomRight = new Line(guideLineBottom.MidPoint, guideLineBottom.EndPoint);

                Arc BottomLeftDown = new Arc(guideLineBottomLeft.StartPoint,
                                        GetSumPoint(guideLineBottomLeft.MidPoint, 0, -arcHeight),
                                        guideLineBottomLeft.EndPoint,
                                        false);

                Arc BottomRightUp = new Arc(guideLineBottomRight.StartPoint,
                                        GetSumPoint(guideLineBottomRight.MidPoint, 0, arcHeight),
                                        guideLineBottomRight.EndPoint,
                                        false);

                Arc BottomRightDown = new Arc(guideLineBottomRight.StartPoint,
                                        GetSumPoint(guideLineBottomRight.MidPoint, 0, -arcHeight),
                                        guideLineBottomRight.EndPoint,
                                        false);

                splinesList.AddRange(new Entity[] {
                    BottomLeftDown, BottomRightUp, BottomRightDown
                });
            }


            //////////////////////////////////////////////////////////
            // Draw Add
            //////////////////////////////////////////////////////////


            outlinesList.AddRange(new Entity[] {
                leftLine, rightLine,
            });

            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref splinesList, layerService.LayerDimension);

            EntityList.AddRange(outlinesList);
            EntityList.AddRange(splinesList);


            return EntityList;
        }

        public List<Entity> GetColumnPipeCut(Point3D refPoint, double OuterDiameter, double Height, double Gap, List<double> cut_YList)
        {
            // refPoint : bottom Center
            // gap : cut Y1 (distance:gap) cut Y2
            // cut_YList : bottom -> Top  ( 중간의 Y축 기준의 중간값들   ex) h:10 ,  cut_YList = {3,8}

            List<Entity> outlinesList = new List<Entity>();
            List<Line> rectangleList = new List<Line>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            double halfGap = Gap / 2;

            // guide Center Line
            Line guideLine = new Line(referencePoint, GetSumPoint(refPoint, 0, Height));


            // set cut Point
            List<Point3D> cutYPointList = new List<Point3D>();
            cutYPointList.Add(GetSumPoint(referencePoint, 0, 0)); // bottom Point
            foreach (double eachY in cut_YList)
            {
                cutYPointList.Add(GetSumPoint(referencePoint, 0, eachY)); // add Mid Point
            }
            cutYPointList.Add(GetSumPoint(referencePoint, 0, Height)); // top Point

            int countYPoint = cutYPointList.Count();


            // Draw : bottom Pipe
            outlinesList.AddRange(GetColumnPipe(cutYPointList[0], OuterDiameter, cut_YList[0], VIEWPOSITION.TOP));

            // Draw : Top Pipe
            double heightTopPipe = Height - halfGap - cut_YList[cut_YList.Count - 1];
            outlinesList.AddRange(GetColumnPipe(GetSumPoint(cutYPointList[countYPoint - 2], 0, halfGap), OuterDiameter, heightTopPipe, VIEWPOSITION.BOTTOM));

            // Draw : Mid Pipe
            for (int i = 1; i < countYPoint - 2; i++)
            {
                double eachHeightPipe = Height - halfGap - cut_YList[cut_YList.Count - 1];
                outlinesList.AddRange(GetColumnPipe(GetSumPoint(cutYPointList[countYPoint - 2], 0, halfGap), OuterDiameter, heightTopPipe, VIEWPOSITION.BOTTOM));
            }


            //////////////////////////////////////////////////////////
            // Draw Add
            //////////////////////////////////////////////////////////

            outlinesList.AddRange(new Entity[] {
                guideLine,
            });
            /**/
            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerHiddenLine);

            return outlinesList;
        }

        private List<Entity> GetAngleList(Point3D refPoint, double width, double length, double thk, SHAPEDIRECTION shapeDirection = SHAPEDIRECTION.RIGHTTOP, double radius = 0)
        {

            List<Entity> angleList = new List<Entity>();

            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);


            //앵글 외곽부분
            Line angleOutsideTop = new Line(GetSumPoint(referencePoint, 0, 0), GetSumPoint(referencePoint, width, 0));
            Line angleOutsideLeft = new Line(GetSumPoint(referencePoint, 0, 0), GetSumPoint(referencePoint, 0, -length));

            //앵글 내부부분
            Line angleInsideTop = new Line(GetSumPoint(referencePoint, thk, -thk), GetSumPoint(referencePoint, width, -thk));
            Line angleInsideTopLeft = new Line(GetSumPoint(angleOutsideTop.EndPoint, 0, 0), GetSumPoint(angleInsideTop.EndPoint, 0, 0));
            Line angleInsideLeft = new Line(GetSumPoint(referencePoint, thk, -thk), GetSumPoint(referencePoint, thk, -length));
            Line angleInsideLeftBottom = new Line(GetSumPoint(angleOutsideLeft.EndPoint, 0, 0), GetSumPoint(angleInsideLeft.EndPoint, 0, 0));


            if (radius != 0)
            {
                //연결되는 부분 부드럽게 처리
                if (Curve.Fillet(angleInsideTop, angleInsideTopLeft, radius, true, false, true, true, out Arc arcFillet01))
                { angleList.Add(arcFillet01); }

                if (Curve.Fillet(angleInsideLeft, angleInsideTop, radius, false, true, true, true, out Arc arcFillet02))
                { angleList.Add(arcFillet02); }

                if (Curve.Fillet(angleInsideLeftBottom, angleInsideLeft, radius, true, false, true, true, out Arc arcFillet03))
                { angleList.Add(arcFillet03); }
            }

            angleList.AddRange(new Line[] { angleOutsideTop, angleOutsideLeft, angleInsideTop, angleInsideLeft,
                                            angleInsideTopLeft,angleInsideLeftBottom


            });

            double angleToRatate = 0;


            switch (shapeDirection)
            {

                case SHAPEDIRECTION.RIGHTTOP:
                    angleToRatate = 0;
                    break;

                case SHAPEDIRECTION.RIGHTBOTTOM:
                    angleToRatate = 90;
                    break;

                case SHAPEDIRECTION.LEFTBOTTOM:
                    angleToRatate = 180;
                    break;

                case SHAPEDIRECTION.LEFTTOP:
                    angleToRatate = 270;
                    break;

            }

            foreach (Entity eachEntity in angleList)
            {
                eachEntity.Rotate(Utility.DegToRad(angleToRatate), Vector3D.AxisZ);
            }

            return angleList;
        }

        private List<Entity> GetHbeam(Point3D refPoint, double width, double length, double thk, double radius, double seamThk = 0)
        {
            //switch(dLinePositoin)

            //seam의 두께가 따로 입력되지 않은 경우 thk값으로 같이 사용
            if (seamThk == 0) { seamThk = thk; }

            List<Entity> HbeamLineList = new List<Entity>();

            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);


            //H빔 상단부분
            Line beamTopLineOn = new Line(GetSumPoint(referencePoint, 0, width), GetSumPoint(referencePoint, length, width));
            Line beamTopLineUnderLeft = new Line(GetSumPoint(referencePoint, 0, width - thk), GetSumPoint(referencePoint, (length - seamThk) / 2, width - thk));
            Line beamTopLineUnderRight = new Line(GetSumPoint(referencePoint, (length + seamThk) / 2, width - thk), GetSumPoint(referencePoint, length, width - thk));
            Line beamTopLeftLine = new Line(GetSumPoint(beamTopLineOn.StartPoint, 0, 0), GetSumPoint(beamTopLineUnderLeft.StartPoint, 0, 0));
            Line beamTopRightLine = new Line(GetSumPoint(beamTopLineOn.EndPoint, 0, 0), GetSumPoint(beamTopLineUnderRight.EndPoint, 0, 0));

            //H빔 하단부분
            Line beamBottomLineOnLeft = new Line(GetSumPoint(referencePoint, 0, thk), GetSumPoint(referencePoint, (length - seamThk) / 2, thk));
            Line beamBottomLineOnRight = new Line(GetSumPoint(referencePoint, (length + seamThk) / 2, thk), GetSumPoint(referencePoint, length, thk));
            Line beamBottomLineUnder = new Line(GetSumPoint(referencePoint, 0, 0), GetSumPoint(referencePoint, length, 0));
            Line beamBottomLeftLine = new Line(GetSumPoint(beamBottomLineOnLeft.StartPoint, 0, 0), GetSumPoint(beamBottomLineUnder.StartPoint, 0, 0));
            Line beamBottomRightLine = new Line(GetSumPoint(beamBottomLineOnRight.EndPoint, 0, 0), GetSumPoint(beamBottomLineUnder.EndPoint, 0, 0));

            //H빔 중간 기둥
            Line SeamLeftLine = new Line(GetSumPoint(beamTopLineUnderLeft.EndPoint, 0, 0), GetSumPoint(beamBottomLineOnLeft.EndPoint, 0, 0));
            Line SeamRightLine = new Line(GetSumPoint(beamTopLineUnderRight.StartPoint, 0, 0), GetSumPoint(beamBottomLineOnRight.StartPoint, 0, 0));


            //연결되는 부분 부드럽게 처리
            if (Curve.Fillet(beamTopLineUnderLeft, SeamLeftLine, radius, false, false, true, true, out Arc arcFillet01))
            { HbeamLineList.Add(arcFillet01); }

            if (Curve.Fillet(beamTopLineUnderRight, SeamRightLine, radius, false, true, true, true, out Arc arcFillet02))
            { HbeamLineList.Add(arcFillet02); }

            if (Curve.Fillet(beamBottomLineOnLeft, SeamLeftLine, radius, true, false, true, true, out Arc arcFillet03))
            { HbeamLineList.Add(arcFillet03); }

            if (Curve.Fillet(beamBottomLineOnRight, SeamRightLine, radius, true, true, true, true, out Arc arcFillet04))
            { HbeamLineList.Add(arcFillet04); }

            HbeamLineList.AddRange(new Line[] { beamTopLineOn, beamTopLineUnderLeft, beamTopLineUnderRight, beamTopLeftLine, beamTopRightLine,
                                                beamBottomLineOnLeft, beamBottomLineOnRight,beamBottomLineUnder, beamBottomLeftLine, beamBottomRightLine,
                                                SeamLeftLine, SeamRightLine,
            });


            styleService.SetLayerListEntity(ref HbeamLineList, layerService.LayerOutLine);
            //drawList.outlineList.AddRange(outlinesList);

            return HbeamLineList;

        }

        private List<Entity> GetSlotHoles(Point3D refPoint, double holeRadius, double widthGap, double slotHoleLength, double lengthGap)
        {
            //refPoint는 홀과 홀 사이의 중앙인 점을 입력해주시면됩니다
            //lengthGap=0인경우는 2개, lengthGap에 값이 입력된 경우는 홀이 4개로 그려집니다 :)

            //  return Index       2 3
            //                     0 1

            List<Entity> slotHoleList = new List<Entity>();
            //List<Entity> holeList = new List<Entity>();
            List<Entity> mergeList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Point3D circleCenterPoint = GetSumPoint(referencePoint, lengthGap / 2 * (2 * i - 1), widthGap / 2 * (1 - 2 * j));

                    //Circle Hole = new Circle(circleCenterPoint, holeRadius);
                    Arc SlotLeftArc = new Arc(GetSumPoint(circleCenterPoint, -slotHoleLength / 2, holeRadius),
                                                GetSumPoint(circleCenterPoint, -holeRadius - slotHoleLength / 2, 0),
                                                GetSumPoint(circleCenterPoint, -slotHoleLength / 2, -holeRadius), false);
                    Arc SlotRightArc = new Arc(GetSumPoint(circleCenterPoint, slotHoleLength / 2, holeRadius),
                                                GetSumPoint(circleCenterPoint, holeRadius + slotHoleLength / 2, 0),
                                                GetSumPoint(circleCenterPoint, slotHoleLength / 2, -holeRadius), false);
                    Line SlotTopLine = new Line(GetSumPoint(SlotRightArc.StartPoint, 0, 0), GetSumPoint(SlotLeftArc.StartPoint, 0, 0));
                    Line SlotBottomLine = new Line(GetSumPoint(SlotRightArc.EndPoint, 0, 0), GetSumPoint(SlotLeftArc.EndPoint, 0, 0));


                    //holeList.Add(Hole);
                    slotHoleList.AddRange(new Entity[] { SlotLeftArc, SlotRightArc, SlotTopLine, SlotBottomLine });
                }
            }

            //styleService.SetLayerListEntity(ref holeList, layerService.LayerVirtualLine);
            styleService.SetLayerListEntity(ref slotHoleList, layerService.LayerOutLine);

            //mergeList.AddRange(holeList);
            //smergeList.AddRange(slotHoleList);

            return slotHoleList;

        }

        private List<Entity> GetHoles(Point3D refPoint, double holeRadius, double lengthGap, double widthGap = 0, double angle = 0)
        {
            //refPoint는 홀과 홀 사이의 중앙
            //기본적으로 2개의 홀을 그리는 함수이나,
            //widthGap 입력된 경우는 홀이 4인것으로 인식해서 4개가 그려집니다 :)

            List<Entity> holeList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            //widthGap이 있는데 각도가 입력된 경우에는 각도를 임의로 0으로 초기화합니다.
            if (widthGap != 0 && angle != 0) { angle = 0; }

            double radian = Utility.DegToRad(angle);


            Line guideLine = new Line(GetSumPoint(referencePoint, lengthGap / 2, widthGap / 2),
                                        GetSumPoint(referencePoint, -lengthGap / 2, widthGap / 2));

            guideLine.Rotate(radian, Vector3D.AxisZ);

            Circle topHole = new Circle(guideLine.StartPoint, holeRadius);
            Circle bottomHole = new Circle(guideLine.EndPoint, holeRadius);


            //widthGap 입력받아 2개의 원을 추가로 그려주는 합수입니다.
            if (widthGap != 0)
            {
                Circle LTopHole = new Circle(GetSumPoint(referencePoint, lengthGap / 2, -widthGap / 2), holeRadius);
                Circle LBottomHole = new Circle(GetSumPoint(referencePoint, -lengthGap / 2, -widthGap / 2), holeRadius);

                holeList.Add(LTopHole);
                holeList.Add(LBottomHole);
            }

            holeList.AddRange(new Entity[] {topHole, bottomHole,
                });

            //2개의 원이 그려질 때, 각도를 입력받는 경우에는 플레이트와 별개로 회전합니다.
            //즉, 플레이트와 수평이 아닌 경우에만 각도를 입력해주시면 됩니다.
            //Circle TopHole = new Circle(GetSumPoint(referencePoint, lengthGap / 2 , widthGap / 2 ), holeRadius);
            //Circle BottomHole = new Circle(GetSumPoint(referencePoint, lengthGap / 2, -widthGap / 2), holeRadius);

            //TopHole.Rotate(radian, Vector3D.AxisZ, referencePoint);
            //BottomHole.Rotate(radian, Vector3D.AxisZ, referencePoint);

            //로테이션 함수를 쓰지 않고 바로 회전시키는 경우입니다.
            //Circle TopHole = new Circle(GetSumPoint(referencePoint, lengthGap/ 2 - widthGap/2*Math.Sin(radian) , widthGap / 2 * Math.Cos(radian)), holeRadius);
            //Circle BottomHole = new Circle(GetSumPoint(referencePoint, lengthGap/2 + widthGap / 2 * Math.Sin(radian), -widthGap / 2 * Math.Cos(radian)), holeRadius);





            styleService.SetLayerListEntity(ref holeList, layerService.LayerOutLine);

            return holeList;

        }

        private List<Line> GetDoubleLineRectangle(Point3D refPoint, double width, double length, double thk, RECTANGLUNVIEW unview = RECTANGLUNVIEW.NONE)
        {
            //switch(dLinePositoin)


            List<Line> outlineList = new List<Line>();
            List<Line> doubleLineList = new List<Line>();

            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            //doubleLineList = GetRectangle(referencePoint, width, length, unview);
            doubleLineList = GetRectangle(referencePoint, width, length);
            Line BottomSecondLine = new Line(GetSumPoint(doubleLineList[0].StartPoint, 0, thk), GetSumPoint(doubleLineList[0].EndPoint, 0, thk));
            Line TopSecondLine = new Line(GetSumPoint(doubleLineList[2].StartPoint, 0, -thk), GetSumPoint(doubleLineList[2].EndPoint, 0, -thk));

            switch (unview)
            {
                case RECTANGLUNVIEW.LEFT:
                    outlineList.AddRange(new Line[] {
                             doubleLineList[0], doubleLineList[1],doubleLineList[2],
                    });
                    break;
                case RECTANGLUNVIEW.RIGHT:
                    outlineList.AddRange(new Line[] {
                             doubleLineList[0], doubleLineList[2],doubleLineList[3],
                    });
                    break;
            }

            outlineList.AddRange(new Line[] { 
               BottomSecondLine, TopSecondLine
            });

            
            styleService.SetLayerListLine(ref outlineList, layerService.LayerOutLine);

            /* Index List
             * 0 : bottom ,  1: second OutLine, 2: third OutLine
             * 3 : bottom InnerLine, 4 : top InnerLIne
            /**/

            return outlineList;

        }

        public Line GetExtendLine(Line selLine, double Length, bool endPoint = true)
        {
            
            Line tempLine = new Line(selLine.StartPoint, selLine.EndPoint);

            if (endPoint)
            {
                Vector3D direction = new Vector3D(selLine.StartPoint, selLine.EndPoint);

                direction.Normalize();
                tempLine.EndPoint = tempLine.EndPoint + direction * Length;

                return tempLine;
            }
            else
            {
                Vector3D direction = new Vector3D(selLine.EndPoint, selLine.StartPoint);

                direction.Normalize();
                tempLine.StartPoint = tempLine.StartPoint + direction * Length;

                return tempLine;
            }
        }

        public double GetArcHeight(double Radius, double Angle)
        {
            return Radius - (Radius * (Math.Cos(Utility.DegToRad(Angle / 2))));
        }

        public double GetArcSidePointHeight(double Radius, double Angle)
        {
            return Radius * (Math.Cos(Utility.DegToRad(Angle / 2)));
        }


        public List<Line> GetRectangle(Point3D startPoint, double width, double length, RECTANGLUNVIEW unViewPosition = RECTANGLUNVIEW.NONE)
        {
            // startPoint = LeftBottom point
            List<Line> rectangleLineList = new List<Line>();

            if (unViewPosition != RECTANGLUNVIEW.BOTTOM)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, 0), GetSumPoint(startPoint, length, 0)));
            if (unViewPosition != RECTANGLUNVIEW.RIGHT)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, length, width), GetSumPoint(startPoint, length, 0)));
            if (unViewPosition != RECTANGLUNVIEW.TOP)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, width), GetSumPoint(startPoint, length, width)));
            if (unViewPosition != RECTANGLUNVIEW.LEFT)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, width), GetSumPoint(startPoint, 0, 0)));

            return rectangleLineList;
        }

        public List<Line> GetRectangleLT(Point3D startPoint, double width, double length, RECTANGLUNVIEW unViewPosition = RECTANGLUNVIEW.NONE)
        {
            // startPoint = LeftTop point
            List<Line> rectangleLineList = new List<Line>();

            if (unViewPosition != RECTANGLUNVIEW.BOTTOM)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, -width), GetSumPoint(startPoint, length, -width)));
            if (unViewPosition != RECTANGLUNVIEW.RIGHT)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, length, 0), GetSumPoint(startPoint, length, -width)));
            if (unViewPosition != RECTANGLUNVIEW.TOP)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, 0), GetSumPoint(startPoint, length, 0)));
            if (unViewPosition != RECTANGLUNVIEW.LEFT)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, 0), GetSumPoint(startPoint, 0, -width)));

            return rectangleLineList;
        }

        public List<Entity> GetRectangleK(Point3D selPoint, double Width, double Length, out List<Point3D> selOutputPointList, 
            bool isTopLeft=false, double rotateRadian = 0, double rotateCenterNum = 0, double selTranslateNumber = 0,
            bool[] selVisibleLine = null)
        {
            selOutputPointList = new List<Point3D>();
            List<Entity> newList = new List<Entity>();

            // selPoint
            // default : Bottom Left

            // switch : Top Left
            double switchPoint = 1;
            if (isTopLeft) { switchPoint = -1; }

            // Reference Point : Bottom Left(A)   -> B:BR -> C:TR -> D:TL
            Point3D A = GetSumPoint(selPoint, 0, 0);
            Point3D B = GetSumPoint(selPoint, Length, 0);
            Point3D C = GetSumPoint(selPoint, Length, Width * switchPoint);
            Point3D D = GetSumPoint(selPoint, 0, Width * switchPoint);

            // Reference Point : Bottom Left(A)   -> B:BR -> C:TR -> D:TL
            //Point3D A = GetSumPoint(selPoint, 0, 0);
            //Point3D B = GetSumPoint(selPoint, Length, 0);
            //Point3D C = GetSumPoint(selPoint, Length, Width);
            //Point3D D = GetSumPoint(selPoint, 0, Width);

            if (isTopLeft) {
                Point3D tempPoint = null;

                tempPoint = CopyPoint(A);
                A = CopyPoint(D);
                D = CopyPoint(tempPoint);

                tempPoint = CopyPoint(B);
                B = CopyPoint(C);
                C = CopyPoint(tempPoint);
            }

            // Line : Bottom , Right, Top, Left
            Line lineA = new Line(GetSumPoint(A, 0, 0), GetSumPoint(B, 0, 0));
            Line lineB = new Line(GetSumPoint(C, 0, 0), GetSumPoint(B, 0, 0));
            Line lineC = new Line(GetSumPoint(D, 0, 0), GetSumPoint(C, 0, 0));
            Line lineD = new Line(GetSumPoint(D, 0, 0), GetSumPoint(A, 0, 0));

            newList.AddRange(new Line[] { lineA, lineB, lineC, lineD });

            // 1,2,3,4 = A, B, C, D  각 포인트를 기준으로 회전
            if (rotateRadian != 0)
            {
                Point3D WPRotate = GetSumPoint(A, 0, 0);
                if (rotateCenterNum == 1)
                    WPRotate = GetSumPoint(B, 0, 0);
                else if (rotateCenterNum == 2)
                    WPRotate = GetSumPoint(C, 0, 0);
                else if (rotateCenterNum == 3)
                    WPRotate = GetSumPoint(D, 0, 0);

                foreach (Entity eachEntity in newList)
                    eachEntity.Rotate(rotateRadian, Vector3D.AxisZ, WPRotate);
            }

            // 
            if (selTranslateNumber > 0)
            {
                Point3D WPTranslate = new Point3D();
                if (selTranslateNumber == 1)
                    WPTranslate = GetSumPoint(B, 0, 0);
                else if (selTranslateNumber == 2)
                    WPTranslate = GetSumPoint(C, 0, 0);
                else if (selTranslateNumber == 3)
                    WPTranslate = GetSumPoint(D, 0, 0);
                SetTranslate(ref newList, GetSumPoint(selPoint, 0, 0), WPTranslate);

            }

            // Bottom, Right, Top, Left 
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

        public List<Entity> GetMirrorEntity(Plane selPlane, List<Entity> selList, double X, double Y, bool copyCmd = false)
        {
            // double X, Y  : Plane 의 중심점( Point(X,Y) )

            Plane pl1 = selPlane;
            pl1.Origin.X = X;
            pl1.Origin.Y = Y;
            Mirror customMirror = new Mirror(pl1);
            List<Entity> mirrorList = new List<Entity>();
            if (copyCmd)
            {
                foreach (Entity eachEntity in selList)
                {
                    Entity newEntity = (Entity)eachEntity.Clone();
                    newEntity.TransformBy(customMirror);
                    mirrorList.Add(newEntity);
                }
            }
            else
            {
                foreach (Entity eachEntity in selList)
                {
                    eachEntity.TransformBy(customMirror);
                }
            }


            return mirrorList;
        }

        public void SetTranslate(ref List<Entity> selList, Point3D refPoint, Point3D currentPoint)
        {
            double distanceY = refPoint.Y - currentPoint.Y;
            double distanceX = refPoint.X - currentPoint.X;
            Vector3D tempMovement = new Vector3D(distanceX, distanceY);
            foreach (Entity eachEntity in selList)
                eachEntity.Translate(tempMovement);
        }


        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
        private Point3D CopyPoint(Point3D selPoint)
        {
            return new Point3D(selPoint.X, selPoint.Y, selPoint.Z);
        }


        private double GetMinAddNum(double selX, double minB, double addNum)
        {
            // 최소단위 숫자 만들기  X는 최소 B보다는 크고 단위가 10단위 100단위로 증가
            // selX 입력값, minB 최소값, addNum 증가할 단위값(10, 100, 1000 등)
            double returnX = Math.Ceiling(selX);
            
            if (returnX < minB) returnX = minB;
            else
            {
                double rafterDistanceRemain = returnX % addNum;
                if (rafterDistanceRemain > 0)
                {
                    returnX = returnX + (addNum - rafterDistanceRemain);
                }
            }
            return returnX;
        }
    }
}
