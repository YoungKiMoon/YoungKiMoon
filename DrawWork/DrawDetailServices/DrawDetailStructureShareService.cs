using AssemblyLib.AssemblyModels;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawSettingLib.SettingServices;
using DrawShapeLib.DrawServices;
using DrawWork.Commons;
using DrawWork.DrawCommonServices;
using DrawWork.DrawModels;
using DrawWork.DrawServices;
using DrawWork.DrawStyleServices;
using DrawWork.ValueServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawDetailServices
{
    public class DrawDetailStructureShareService
    {

        private Model singleModel;
        private AssemblyModel assemblyData;

        private DrawPublicService drawCommon;

        private ValueService valueService;
        private StyleFunctionService styleService;
        private LayerStyleService layerService;

        private DrawEditingService editingService;
        private DrawShapeServices shapeService;


        private DrawCommonService drawComService;
        private DrawReferenceBlockService refBlockService;
        private DrawWorkingPointService workingPointService;


        public PaperAreaService areaService;





        public DrawDetailStructureShareService(AssemblyModel selAssembly, object selModel)
        {
            singleModel = selModel as Model;
            assemblyData = selAssembly;

            drawCommon = new DrawPublicService(selAssembly);
            refBlockService = new DrawReferenceBlockService(selAssembly);
            workingPointService = new DrawWorkingPointService(selAssembly);

            valueService = new ValueService();
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();


            editingService = new DrawEditingService();
            shapeService = new DrawShapeServices();

            drawComService = new DrawCommonService();


            areaService = new PaperAreaService();

        }


        #region Lee Hye Kyeong


        // Rafter
        public List<Entity> DrawDetailRafter_Test(Point3D refPoint, double scaleValue)
        {
            List<Entity> newList = new List<Entity>();
            if (SingletonData.TankType == TANK_TYPE.CRT)
            {
                // Internal
                newList = RafterCenteringTypeRafter2(refPoint, scaleValue);

                // External
                newList = RafterCenteringTypeRafter1(refPoint, scaleValue);
            }
            else if (SingletonData.TankType == TANK_TYPE.DRT)
            {
                //newList = RafterCenteringTypeRafter1(refPoint, scaleValue);
            }
            return newList;
        }

        // CRT : Rafter : Internal : Column
        public List<Entity> RafterCenteringTypeRafter2(Point3D refPoint, double scaleValue)
        {
            List<Entity> outlinesList = new List<Entity>();

            Point3D workingPoint = GetSumPoint(refPoint, 0, 0);

            //for clip info_1
            double clipLength = 300;
            double clipBottomLength = 50;
            double padThk = 6;
            double padWidth = 350;
            double value_B1 = 30; //working Point to pad Start Point
            double value_C1 = 25; //pad Start Point to clip start Point
            double clipRightWidth_C = 200;
            double roofTOrafter = 7;  //roof to clip Distance 
            double holeTOclip = 55; //slothole to clip right line

            double clipAngle = Utility.DegToRad(10); ; //Angle의 경우 나중에 rad값으로 받기로 함!!

            //for loof info : 임의값
            double roofLenth = 1300;
            double roofwidth = 7;

            //for slothole info
            double slotholeHT = 23.0 / 2;
            double widthGap_E = 90;
            double widthMargin_D = 55;
            double slotholeWD = 45.0 / 2;
            double lengthGap_G1 = 70;
            double lengthmargin_F1 = 40;// rafter의 끝에서 첫번째 홀 중심까지의 거리

            //for rafter info
            double rafterWidth_A = 200;
            double rafterLength = 1000; //임의값
            double value_A1 = 70;    // workingPoint부터 rafter까지의 x축 거리
            double value_byA1 = value_A1 * Math.Tan(clipAngle); // workingPoint부터 rafter까지의 y축 거리

            //for column info
            double columnLength_t = 6;   //현재 있는 값은 전부 동일함
            double columnWidth = 420; //임의값


            //for moving roof start point
            double move_x = -6;
            double move_y = 0;

            //for purlin 
            double purlinLenth = 65;
            double purlinThk = 6;

            Point3D roofStartPoint = GetSumPoint(refPoint, move_x, move_y);

            //roof Line
            List<Line> roof = GetRectangle(GetSumPoint(roofStartPoint, 0, 0), roofwidth, roofLenth, RECTANGLUNVIEW.RIGHT);
            foreach (Entity eachEntity in roof)
            {
                eachEntity.Rotate(clipAngle, Vector3D.AxisZ, GetSumPoint(roofStartPoint, 0, 0));
            }
            outlinesList.AddRange(roof);

            //angle Line
            List<Entity> angle = GetAngleList(GetSumPoint(workingPoint, -columnLength_t, 0), 75, 75, 7, SHAPEDIRECTION.LEFTTOP, 4);// 앵글 수치는 임의값
            outlinesList.AddRange(angle);

            //column Line
            List<Line> column = GetRectangle(GetSumPoint(workingPoint, -columnLength_t, -10 - columnWidth), columnWidth, columnLength_t, RECTANGLUNVIEW.NONE);//y축으로 내려오는 10은 앵글이 있는 경우 고정값
            outlinesList.AddRange(column);
            //Pad Line
            List<Line> pad = GetRectangle(GetSumPoint(workingPoint, 0, -value_B1 - padWidth), padWidth, padThk, RECTANGLUNVIEW.NONE);
            outlinesList.AddRange(pad);

            //rafter And Holes
            List<Line> rafter = GetDoubleLineRectangle(GetSumPoint(roofStartPoint, value_A1, -rafterWidth_A + value_byA1), rafterWidth_A, rafterLength, 10, RECTANGLUNVIEW.NONE); //thk는 임의값

            //Left
            List<Entity> leftSlotHoles = GetSlotHoles(GetSumPoint(rafter[3].MidPoint, lengthmargin_F1 + lengthGap_G1 / 2, 0), slotholeHT, widthGap_E, slotholeWD, lengthGap_G1);
            List<Entity> leftHoles = GetHoles(GetSumPoint(rafter[3].MidPoint, lengthmargin_F1 + lengthGap_G1 / 2, 0), slotholeHT, lengthGap_G1, widthGap_E);

            //Right
            List<Entity> rightSlotHoles = GetSlotHoles(GetSumPoint(rafter[1].MidPoint, -(lengthmargin_F1 + lengthGap_G1 / 2), 0), slotholeHT, widthGap_E, slotholeWD, lengthGap_G1);
            List<Entity> rightHoles = GetHoles(GetSumPoint(rafter[1].MidPoint, -(lengthmargin_F1 + lengthGap_G1 / 2), 0), slotholeHT, lengthGap_G1, widthGap_E);

            //Angle
            Line purlinGuideLine = new Line(GetSumPoint(rafter[3].MidPoint, 0, 0), GetSumPoint(rafter[1].MidPoint, 0, 0));
            List<Entity> purlin = GetAngleList(GetSumPoint(purlinGuideLine.MidPoint, purlinLenth, purlinLenth / 2), purlinLenth, purlinLenth, purlinThk, SHAPEDIRECTION.LEFTTOP, purlinThk * 0.70);

            List<Entity> rafterNslot = new List<Entity>();
            rafterNslot.AddRange(rafter);
            rafterNslot.AddRange(leftSlotHoles);
            rafterNslot.AddRange(leftHoles);
            rafterNslot.AddRange(rightSlotHoles);
            rafterNslot.AddRange(rightHoles);
            rafterNslot.AddRange(purlin);

            foreach (Entity eachEntity in rafterNslot)
            {
                //rater의 회전기준점 : 라프타를 구성하는 사각형의 좌측 상단
                eachEntity.Rotate(clipAngle, Vector3D.AxisZ, GetSumPoint(rafter[2].StartPoint, 0, 0));
            }
            outlinesList.AddRange(rafterNslot);

            ///////////////////////////////////////////////////
            ///////////////////LEFT CLIP///////////////////////
            ///////////////////////////////////////////////////
            //일단 인터섹션으로 인터셉트  
            Circle forClipDistance = new Circle(rafter[2].StartPoint, roofTOrafter);
            Point3D[] clipSlideStartPoint = rafter[3].IntersectWith(forClipDistance);
            Circle underHole = (Circle)leftHoles[0];
            Line clipRightGuideLine = new Line(GetSumPoint(underHole.Center, holeTOclip, 0), GetSumPoint(underHole.Center, holeTOclip, 500));

            //clip Line
            Line clipTopLinkLine = new Line(GetSumPoint(workingPoint, padThk, -value_B1 - value_C1), GetSumPoint(clipSlideStartPoint[0], 0, 0));
            //Using intersect//
            Line clipTopslideLineGuide = new Line(GetSumPoint(clipTopLinkLine.EndPoint, 0, 0), GetSumPoint(clipTopLinkLine.EndPoint, 500, 0));
            clipTopslideLineGuide.Rotate(clipAngle, Vector3D.AxisZ, clipTopLinkLine.EndPoint);
            Point3D[] clipSlideEndPoint = clipTopslideLineGuide.IntersectWith(clipRightGuideLine);

            Line clipTopslideLine = new Line(GetSumPoint(clipTopLinkLine.EndPoint, 0, 0), GetSumPoint(clipSlideEndPoint[0], 0, 0));
            Line clipRightLine = new Line(GetSumPoint(clipTopslideLine.EndPoint, 0, 0), GetSumPoint(clipTopslideLine.EndPoint, 0, -clipRightWidth_C));
            Line clipBottomLine = new Line(GetSumPoint(clipTopLinkLine.StartPoint, 0, -clipLength), GetSumPoint(clipTopLinkLine.StartPoint, clipBottomLength, -clipLength));
            Line clipBottomslideLine = new Line(GetSumPoint(clipBottomLine.EndPoint, 0, 0), GetSumPoint(clipRightLine.EndPoint, 0, 0));


            ////////////////////////////////////////////////////
            ///////////////////RIGHT CLIP///////////////////////
            //////일단 구색맞추기용으로 막코드입니다 .. T_T/////
            ////////////////////////////////////////////////////

            Line forRightClipSlide = (Line)roof[0].Offset(roofTOrafter, Vector3D.AxisZ);
            /// 
            Circle forRightClipDistance = new Circle(rafter[2].EndPoint, roofTOrafter);
            Point3D[] rightClipSlideStartPoint = rafter[1].IntersectWith(forRightClipDistance);
            Point3D rightClipSlidePoint = GetSumPoint(rightClipSlideStartPoint[0], 18 * Math.Cos(clipAngle), 18 * Math.Sin(clipAngle)); //20은 임의값

            Circle LunderHole = (Circle)rightHoles[3];
            Line rightClipLeftGuideLine = new Line(GetSumPoint(LunderHole.Center, -holeTOclip, 100), GetSumPoint(LunderHole.Center, -holeTOclip, -500));

            Point3D[] rightClipInterceptLine = rightClipLeftGuideLine.IntersectWith(forRightClipSlide);

            ///////////////////CENTERING//////////////////////////////////
            Line rightFinishLine = new Line(GetSumPoint(roof[1].EndPoint, 0, 25), GetSumPoint(roof[1].EndPoint, 0, -rafterWidth_A * 1.6)); //y축 수치는 임의값
            //roof is trim by FinishLine
            Point3D[] roofOnFinish = rightFinishLine.IntersectWith(roof[0]);
            roof[0].TrimBy(roofOnFinish[0], false);

            List<Line> rightTopCentering = GetRectangle(GetSumPoint(rafter[2].EndPoint, 30 * Math.Cos(clipAngle), 30 * Math.Sin(clipAngle) - padThk * 1.5), padThk * 1.5, 300, RECTANGLUNVIEW.RIGHT);
            outlinesList.AddRange(rightTopCentering);
            //topCentering is trim by FinishLine
            Point3D[] topCenteringOnFinish0 = rightFinishLine.IntersectWith(rightTopCentering[0]);
            rightTopCentering[0].TrimBy(topCenteringOnFinish0[0], false);
            Point3D[] topCenteringOnFinish1 = rightFinishLine.IntersectWith(rightTopCentering[1]);
            rightTopCentering[1].TrimBy(topCenteringOnFinish0[0], false);
            Line TopCenteringMid = (Line)rightTopCentering[2].Offset(-150, Vector3D.AxisZ);

            //bottomCentering 
            Line bottomCenteringTop = (Line)rightTopCentering[1].Offset(rafterWidth_A * 1.15, Vector3D.AxisZ);
            Line bottomCenteringBottom = (Line)rightTopCentering[0].Offset(rafterWidth_A * 1.15, Vector3D.AxisZ);
            Line bottomCenteringLeft = new Line(bottomCenteringTop.StartPoint, bottomCenteringBottom.StartPoint);
            Line BottomCenteringMid = (Line)bottomCenteringLeft.Offset(-150, Vector3D.AxisZ);

            //clip Line
            rightClipLeftGuideLine.TrimBy(rightClipInterceptLine[0], true);
            Line rightClipSlideLine = new Line(rightClipSlidePoint, rightClipInterceptLine[0]);

            Line rightClipTopLine = new Line(GetSumPoint(rightClipSlideLine.StartPoint, 0, 0), GetSumPoint(rightTopCentering[2].EndPoint, 0, 0));
            Line rightClipBottomLine = new Line(GetSumPoint(bottomCenteringTop.StartPoint, 0, 0), GetSumPoint(bottomCenteringTop.StartPoint, -500, 0));

            Point3D[] rightClipBottom = rightClipBottomLine.IntersectWith(rightClipLeftGuideLine);

            rightClipLeftGuideLine.TrimBy(rightClipBottom[0], false);
            rightClipBottomLine.TrimBy(rightClipBottom[0], false);
            /// 
            /// 
            ///////////////Almost Value Is Random///////////////
            ////////////////////////////////////////////////////

            //Centering 
            List<Line> Centering = GetRectangle(GetSumPoint(bottomCenteringTop.StartPoint, 50, 0), rafterWidth_A * 1.15 - padThk * 1.5, padThk * 1.5);
            outlinesList.AddRange(Centering);



            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////


            //for Rafter
            double thk = 6;



            outlinesList.AddRange(new Entity[] {
                //About Left Clip
                clipTopLinkLine, clipTopslideLine, clipRightLine, clipBottomLine, clipBottomslideLine,
                
                //About Right Clip
                rightFinishLine, TopCenteringMid,
                bottomCenteringTop, bottomCenteringBottom, bottomCenteringLeft, BottomCenteringMid,
                rightClipLeftGuideLine, rightClipSlideLine,
                rightClipTopLine, rightClipBottomLine

            });


            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);
            return outlinesList;
        }

        // CRT : Rafter : External : Centering
        public List<Entity> RafterCenteringTypeRafter1(Point3D refPoint, double scaleValue)
        {
            List<Entity> outlinesList = new List<Entity>();


            double rafterSize_A = 200; //실제로는 rafter width임
            double rafterTOcentering_A1 = 30;
            double recieveAlpha = 30; //센터링으로부터 받아야 하는 알파값
            double cuttingLength_B1 = 15;
            double clipAngle = Utility.DegToRad(10);

            //for loof info : 임의값
            double roofLenth = 600;
            double roofwidth = 7;

            double move_x = -6;
            double move_y = 0;

            //for Rafter
            double thk = 6;

            //centering으로부터 받아야 하는 값
            Point3D getCenteringWorkingPoint = null;
            Point3D getCenteringTopPoint = null;


            //삼각함수 값은 workingPoint를roof의 좌측 상단으로 잡기 위함
            Point3D workingPoint = GetSumPoint(refPoint, 0, 0);
            Point3D centeringWorkingPoint = GetSumPoint(workingPoint, 300 * Math.Cos(clipAngle), 300 * Math.Sin(clipAngle)); //임의의 점. 나중에 받는값으로 바꿔야함
            Point3D roofStartPoint = GetSumPoint(workingPoint, roofwidth * Math.Sin(clipAngle), -roofwidth * Math.Cos(clipAngle));

            //angle Line
            List<Entity> angle = GetAngleList(GetSumPoint(roofStartPoint, -move_x, -move_y),
                                              75, 75, 7, SHAPEDIRECTION.LEFTTOP, 4);// 앵글 수치는 임의값
            outlinesList.AddRange(angle);

            //roof Line
            List<Line> roof = GetRectangle(GetSumPoint(roofStartPoint, 0, 0), roofwidth, roofLenth, RECTANGLUNVIEW.NONE);
            foreach (Entity eachEntity in roof)
            {
                eachEntity.Rotate(clipAngle, Vector3D.AxisZ, GetSumPoint(roofStartPoint, 0, 0));
            }
            outlinesList.AddRange(roof);


            ////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////
            // 임시로 그려놓은 centering :)
            List<Line> underCentering = GetRectangle(GetSumPoint(centeringWorkingPoint, 0, 0), roofwidth, roofLenth / 4, RECTANGLUNVIEW.NONE);
            foreach (Entity eachEntity in underCentering)
            {
                eachEntity.Rotate(clipAngle, Vector3D.AxisZ, GetSumPoint(underCentering[0].StartPoint, 0, 0));
            }
            outlinesList.AddRange(underCentering);
            Line CenteringVLine = new Line(GetSumPoint(underCentering[2].MidPoint, 0, 0), GetSumPoint(underCentering[2].MidPoint, 0, rafterSize_A * 0.95));
            outlinesList.Add(CenteringVLine);
            List<Line> topCentering = GetRectangle(GetSumPoint(CenteringVLine.EndPoint, -roofLenth / 8, 0), roofwidth, roofLenth / 4, RECTANGLUNVIEW.NONE);
            outlinesList.AddRange(topCentering);
            ////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////


            //for Rafter
            //UnderLine : centering Working Point를 받아 A1만큼의 값을 가진 원을 그려서 중첩되는 점을 찾아 점을 통해 라인을 그림
            Circle forBottomRafter = new Circle(underCentering[0].StartPoint, rafterTOcentering_A1);

            Point3D[] roofOnRafterPoint = forBottomRafter.IntersectWith(roof[2]);
            Line rafterUnderLine = new Line(GetSumPoint(workingPoint, 0, 0), GetSumPoint(roofOnRafterPoint[1], 0, 0));
            Line rafterUnderInnerLine = (Line)rafterUnderLine.Offset(-thk, Vector3D.AxisZ);
            Line rafterLeftLine = new Line(GetSumPoint(workingPoint, 0, 0), GetSumPoint(workingPoint, -rafterSize_A * Math.Sin(clipAngle), rafterSize_A * Math.Cos(clipAngle)));
            Line rafterRightBottomLine = new Line(GetSumPoint(roofOnRafterPoint[1], 0, 0), GetSumPoint(roofOnRafterPoint[1], -cuttingLength_B1 * Math.Sin(clipAngle), cuttingLength_B1 * Math.Cos(clipAngle)));

            Line rafterRightBottomHLineGuide = new Line(GetSumPoint(rafterRightBottomLine.EndPoint, 0, 0),
                                                   GetSumPoint(rafterRightBottomLine.EndPoint, (rafterTOcentering_A1 + recieveAlpha) * 2, 0));
            rafterRightBottomHLineGuide.Rotate(clipAngle, Vector3D.AxisZ, rafterRightBottomHLineGuide.StartPoint);

            //TopLine ~ rigtLine
            //Top centering[3].StartPoint로부터 A1만큼 떨어진 선을 그리고 교점을 찾은 후 선을 그림
            Line forTopRafterGuide = new Line(GetSumPoint(topCentering[3].StartPoint, -rafterTOcentering_A1, 100),
                                              GetSumPoint(topCentering[3].StartPoint, -rafterTOcentering_A1, -100)); //+-100은 임의값
            Line rafterTopLine = (Line)roof[2].Offset(-rafterSize_A, Vector3D.AxisZ);
            Line rafterTopInnerLine = (Line)rafterTopLine.Offset(thk, Vector3D.AxisZ);
            Point3D[] topRafterIntersectionPoint = forTopRafterGuide.IntersectWith(rafterTopLine);

            Line rafterTopRight = new Line(GetSumPoint(topRafterIntersectionPoint[0], 0, 0), GetSumPoint(topRafterIntersectionPoint[0], 0, -cuttingLength_B1));
            Line rafterTopHRight = new Line(GetSumPoint(rafterTopRight.EndPoint, 0, 0),
                                            GetSumPoint(rafterTopRight.EndPoint, rafterTOcentering_A1 + recieveAlpha, 0));
            Point3D[] topRightRafterIntersectionPoint = rafterTopInnerLine.IntersectWith(rafterTopRight);

            rafterTopLine.TrimBy(topRafterIntersectionPoint[0], false);
            rafterTopInnerLine.TrimBy(topRightRafterIntersectionPoint[0], false);


            Line rafterRight = new Line(GetSumPoint(rafterTopHRight.EndPoint, 0, 0), GetSumPoint(rafterTopHRight.EndPoint, 0, -rafterSize_A));

            Point3D[] RightRafterIntersectionPoint = rafterRight.IntersectWith(rafterRightBottomHLineGuide);
            rafterRight.TrimBy(RightRafterIntersectionPoint[0], false);
            Line rafterRightBottomHLine = new Line(GetSumPoint(rafterRightBottomLine.EndPoint, 0, 0), GetSumPoint(RightRafterIntersectionPoint[0], 0, 0));

            outlinesList.AddRange(new Entity[] {
                rafterUnderLine, rafterUnderInnerLine,
                rafterTopLine,
                rafterTopInnerLine,
                rafterTopRight,
                rafterUnderLine, rafterUnderInnerLine,
                rafterLeftLine,
                rafterRightBottomLine,
                rafterRightBottomHLine,
                rafterRight,

                rafterTopHRight,
            });

            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);
            return outlinesList;
        }




        public DrawEntityModel Rafter(Point3D refPoint, object selModel, double scaleValue, NColumnRafterModel padModel,
                                        double rotationPoint, double rotateValue)
        {
            DrawEntityModel drawList = new DrawEntityModel();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            List<Entity> newList = new List<Entity>();

            List<Entity> rafterList = new List<Entity>();
            List<Entity> rafterNslot = new List<Entity>();

            double A = valueService.GetDoubleValue(padModel.A);    //rafter Lenth
            double A1 = valueService.GetDoubleValue(padModel.A1);   //woking Point to rafter : NOT USED
            double B = valueService.GetDoubleValue(padModel.B);    //rafter Width
            double B1 = valueService.GetDoubleValue(padModel.B1);   //workingPoint부터 rafter까지의 x축 거리
            double C = valueService.GetDoubleValue(padModel.C);    //centering Point to rafter : NOT USED  //rafter to hole : NOT USED
            double C1 = valueService.GetDoubleValue(padModel.C1);   //hole to hole length gap
            double D = valueService.GetDoubleValue(padModel.D);    //hole to hole width gap
            double E = valueService.GetDoubleValue(padModel.E);    //SHELL ID/4 : NOT USED

            double slotholeHT = 23.0;
            double slotholeWD = 45.0;

            //AEMAE
            double t = 6;   //Thk of doubleLine
            double clipAngle = valueService.GetDegreeOfSlope(assemblyData.RoofCompressionRing[0].RoofSlope);


            Point3D workingPoint = GetSumPoint(referencePoint, 0, 0);

            //rafter And Holes
            List<Line> rafter = GetDoubleLineRectangle(GetSumPoint(workingPoint, 0, 0), B, A, t, RECTANGLUNVIEW.NONE);
            rafterList.AddRange(rafter);

            //Left
            List<Entity> leftSlotHoles = GetSlotHoles(GetSumPoint(rafter[3].MidPoint, B1 + C1 / 2, 0), slotholeHT, slotholeWD, C1, D);
            List<Entity> leftHoles = GetHoles(GetSumPoint(rafter[3].MidPoint, B1 + C1 / 2, 0), slotholeHT, C1, D);

            //Right
            List<Entity> rightSlotHoles = GetSlotHoles(GetSumPoint(rafter[1].MidPoint, -(B1 + C1 / 2), 0), slotholeHT, slotholeWD, C1, D);
            List<Entity> rightHoles = GetHoles(GetSumPoint(rafter[1].MidPoint, -(B1 + C1 / 2), 0), slotholeHT, C1, D);

            rafterNslot.AddRange(leftSlotHoles);
            rafterNslot.AddRange(leftHoles);
            rafterNslot.AddRange(rightSlotHoles);
            rafterNslot.AddRange(rightHoles);


            newList.AddRange(rafterList);
            newList.AddRange(rafterNslot);

            // Rotate


            foreach (Entity eachEntity in rafterNslot)
            {
                //rater의 회전기준점 : 라프타를 구성하는 사각형의 좌측 상단
                eachEntity.Rotate(clipAngle, Vector3D.AxisZ, GetSumPoint(rafter[2].StartPoint, 0, 0));
            }
            drawList.outlineList.AddRange(rafterNslot);


            //styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);
            return drawList;
        }


        public List<Entity> RafterNew(out List<Point3D> selOutputPointList, Point3D refPoint,double selLength , double selRotate, double selRotateCenter, double selTranslateNumber, bool[] selVisibleLine = null, 
                                    NColumnRafterModel padModel=null)
        {
            selOutputPointList = new List<Point3D>();
            List<Entity> newList = new List<Entity>();
            List<Entity> slotHoleList = new List<Entity>();


            // Model Data
            double A = valueService.GetDoubleValue(padModel.A);    //rafter Lenth
            double A1 = valueService.GetDoubleValue(padModel.A1);   //woking Point to rafter : NOT USED
            double B = valueService.GetDoubleValue(padModel.B);    //rafter Width
            double B1 = valueService.GetDoubleValue(padModel.B1);   //workingPoint부터 rafter까지의 x축 거리
            double C = valueService.GetDoubleValue(padModel.C);    //centering Point to rafter : NOT USED  //rafter to hole : NOT USED
            double C1 = valueService.GetDoubleValue(padModel.C1);   //hole to hole length gap
            double D = valueService.GetDoubleValue(padModel.D);    //hole to hole width gap
            double E = valueService.GetDoubleValue(padModel.E);    //SHELL ID/4 : NOT USED
            double holeDia = valueService.GetDoubleValue(padModel.BoltHoleDia);

            double selHeight = A;

            double t = 6;   //Thk of doubleLine

            // Reference Point : Top Left
            Point3D pointA = GetSumPoint(refPoint, 0, 0);
            Point3D pointB = GetSumPoint(refPoint, selLength, 0);
            Point3D pointC = GetSumPoint(refPoint, selLength, -selHeight);
            Point3D pointD = GetSumPoint(refPoint, 0, -selHeight);

            // Line
            Line lineA = new Line(GetSumPoint(pointA, 0, 0), GetSumPoint(pointB, 0, 0));
            Line lineB = new Line(GetSumPoint(pointB, 0, 0), GetSumPoint(pointC, 0, 0));
            Line lineC = new Line(GetSumPoint(pointC, 0, 0), GetSumPoint(pointD, 0, 0));
            Line lineD = new Line(GetSumPoint(pointD, 0, 0), GetSumPoint(pointA, 0, 0));

            // Inner Line : HBeam,Channel Tpye에 따라서 달라짐
            Line lineAinner = (Line)lineA.Offset(t, Vector3D.AxisZ);
            Line lineCinner = (Line)lineC.Offset(-t, Vector3D.AxisZ);

            newList.AddRange(new Line[] { lineA, lineB, lineC, lineD, lineAinner, lineCinner });

            // Slot Hole : Center Line 처리 필요함
            slotHoleList.AddRange(GetHoles(GetSumPoint(lineD.MidPoint, B1 + C1 / 2, 0), holeDia, C1, D));
            slotHoleList.AddRange(GetHoles(GetSumPoint(lineD.MidPoint, B1 + C1 / 2, 0), holeDia, C1, D));
            slotHoleList.AddRange(GetHoles(GetSumPoint(lineB.MidPoint, -(B1 + C1 / 2), 0), holeDia, C1, D));
            slotHoleList.AddRange(GetHoles(GetSumPoint(lineB.MidPoint, -(B1 + C1 / 2), 0), holeDia, C1, D));

            if (selRotate != 0)
            {
                Point3D WPRotate = GetSumPoint(pointA, 0, 0);
                if (selRotateCenter == 1)
                    WPRotate = GetSumPoint(pointB, 0, 0);
                else if (selRotateCenter == 2)
                    WPRotate = GetSumPoint(pointC, 0, 0);
                else if (selRotateCenter == 3)
                    WPRotate = GetSumPoint(pointD, 0, 0);

                foreach (Entity eachEntity in newList)
                    eachEntity.Rotate(selRotate, Vector3D.AxisZ, WPRotate);
            }
            // 나중에 맨 앞으로 옮겨야 함
            if (selTranslateNumber > 0)
            {
                Point3D WPTranslate = new Point3D();
                if (selTranslateNumber == 1)
                    WPTranslate = GetSumPoint(pointB, 0, 0);
                else if (selTranslateNumber == 2)
                    WPTranslate = GetSumPoint(pointC, 0, 0);
                else if (selTranslateNumber == 3)
                    WPTranslate = GetSumPoint(pointD, 0, 0);
                editingService.SetTranslate(ref newList, GetSumPoint(refPoint, 0, 0), WPTranslate);

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

            selOutputPointList.Add(lineA.StartPoint);
            selOutputPointList.Add(lineA.EndPoint);
            selOutputPointList.Add(lineC.StartPoint);
            selOutputPointList.Add(lineC.EndPoint);

            return newList;
        }


        public DrawEntityModel RafterSideClipDetail(Point3D refPoint,Point3D roofPoint, object selModel, double scaleValue, NRafterSupportClipShellSideModel padModel)
        {
            DrawEntityModel drawList = new DrawEntityModel();
            List<Entity> outlinesList = new List<Entity>();
            Point3D workingPoint = GetSumPoint(refPoint, 0, 0);

            double A = valueService.GetDoubleValue(padModel.A);  //rafterWidth
            double A1 = valueService.GetDoubleValue(padModel.A1);   //roof to clip Distance 
            double B = valueService.GetDoubleValue(padModel.B);  //clip To rafter hole center : NOT USED
            double B1 = valueService.GetDoubleValue(padModel.B1);  //working Point to pad Start Point
            double C = valueService.GetDoubleValue(padModel.C); //clipRightWidth
            double C1 = valueService.GetDoubleValue(padModel.C1);  //pad Start Point to clip start Point
            double D = valueService.GetDoubleValue(padModel.D);  //rafter to hole : NOT USED
            double D1 = valueService.GetDoubleValue(padModel.D1);  //workingPoint부터 rafter까지의 x축 거리
            double E = valueService.GetDoubleValue(padModel.E);  //hole to hole width gap
            double E1 = valueService.GetDoubleValue(padModel.E1);  //slothole to clip right line
            double F = valueService.GetDoubleValue(padModel.F); //Pad Width\
            double F1 = valueService.GetDoubleValue(padModel.F1); //rafter to hole
            double G = valueService.GetDoubleValue(padModel.G);  //clip left width
            double G1 = valueService.GetDoubleValue(padModel.G1);  //hole to hole length gap
            double H1 = valueService.GetDoubleValue(padModel.H1);  //clipBottomLength

            double slotholeHT = 23.0;
            double slotholeWD = 45.0;

            double t = 6;   //현재 있는 값은 t2, t3등도 전부 동일함


            //FIXED??

            //AEMAE
            double clipAngle = valueService.GetDegreeOfSlope(assemblyData.RoofCompressionRing[0].RoofSlope); //Angle의 경우 나중에 rad값으로 받기로 함!!
            double roofLenth = 600;                  //for loof info : 임의값
            double roofwidth = 7;                    //for loof info : 임의값
            double rafterLength = 236.7;                //for rafter info : 임의값

            //for moving roof start point
            double move_x = -6;
            double move_y = 0;

            double drawType = 1;

            Point3D topViewWorkingPoint = GetSumPoint(refPoint, 0, 0);
            Point3D frontViewWorkingPoint = GetSumPoint(refPoint, 0, 0);

            Point3D clipStartPoint = null;

            //Point3D roofStartPoint = GetSumPoint(refPoint, move_x, move_y);
            Point3D roofStartPoint = roofPoint;
            //roof Line
            //roof Line
            List<Line> roof = GetRectangle(GetSumPoint(roofStartPoint, 0, 0), roofwidth, roofLenth, RECTANGLUNVIEW.NONE);
            List<Line> rafter = GetOffsetDoubleLineRectangle(GetSumPoint(workingPoint, D1 , D1 * Math.Tan(clipAngle)), A, 
                rafterLength, t, A1, out clipStartPoint); //thk는 임의값

            foreach (Entity eachEntity in roof)
            {
                eachEntity.Rotate(clipAngle, Vector3D.AxisZ, GetSumPoint(roofStartPoint, 0, 0));
            }

            foreach (Entity eachEntity in rafter)
            {
                //회전기준점 라프타의 좌측상단
                eachEntity.Rotate(clipAngle, Vector3D.AxisZ, GetSumPoint(rafter[2].StartPoint, 0, 0));
            }
            // clipstartPoint = clipStartPoint;
            Point3D rafterStartPoint = GetSumPoint(workingPoint, D1, D1 * Math.Tan(clipAngle));
            Line rafterLine = new Line(GetSumPoint(rafterStartPoint, 0, 0), GetSumPoint(rafterStartPoint, 100, 0));
            rafterLine.Rotate(clipAngle, Vector3D.AxisZ, GetSumPoint(rafterStartPoint, 0, 0));
            Point3D rafterStartPointOffsetPoint = GetSumPoint(((Line)rafterLine.Offset(A1, Vector3D.AxisZ)).StartPoint, 0, 0);

            //pad
            List<Line> pad = GetRectangle(GetSumPoint(workingPoint, 0, -B1 - F), F, t, RECTANGLUNVIEW.NONE);
            //rafter And Holes
            List<Entity> slotHoles = GetSlotHoles(GetSumPoint(rafter[3].MidPoint, F1 + G1 / 2, 0), slotholeHT, slotholeWD, G1, E);
            List<Entity> holes = GetHoles(GetSumPoint(rafter[3].MidPoint, F1 + G1 / 2, 0), slotholeHT, G1, E);

            List<Entity> slotNholes = new List<Entity>();
            slotNholes.AddRange(slotHoles);
            slotNholes.AddRange(holes);

            foreach (Entity eachEntity in slotNholes)
            {
                //회전기준점 : 라프타를 구성하는 사각형의 좌측 상단
                eachEntity.Rotate(clipAngle, Vector3D.AxisZ, GetSumPoint(rafter[2].StartPoint, 0, 0));
            }

            //Using intersect for ClipDistance//
            Circle forClipDistance = new Circle(rafter[3].StartPoint, A1);
            Point3D[] clipSlideStartPoint = rafter[3].IntersectWith(forClipDistance);
            Circle underHole = (Circle)holes[3];
            Line clipRightGuideLine = new Line(GetSumPoint(underHole.Center, E1, 0), GetSumPoint(underHole.Center, E1, 500));

            //clip Line
            Line clipTopLinkLine = new Line(GetSumPoint(workingPoint, t, -B1 - C1), GetSumPoint(rafterStartPointOffsetPoint, 0, 0));
            //Using intersect//
            Line clipTopslideLineGuide = new Line(GetSumPoint(clipTopLinkLine.EndPoint, 0, 0), GetSumPoint(clipTopLinkLine.EndPoint, 500, 0));
            clipTopslideLineGuide.Rotate(clipAngle, Vector3D.AxisZ, clipTopLinkLine.EndPoint);
            Point3D[] clipSlideEndPoint = clipTopslideLineGuide.IntersectWith(clipRightGuideLine);

            Line clipTopslideLine = new Line(GetSumPoint(clipTopLinkLine.EndPoint, 0, 0), GetSumPoint(clipSlideEndPoint[0], 0, 0));
            Line clipRightLine = new Line(GetSumPoint(clipTopslideLine.EndPoint, 0, 0), GetSumPoint(clipTopslideLine.EndPoint, 0, -C));
            Line clipBottomLine = new Line(GetSumPoint(clipTopLinkLine.StartPoint, 0, -G), GetSumPoint(clipTopLinkLine.StartPoint, H1, -G));
            Line clipBottomslideLine = new Line(GetSumPoint(clipBottomLine.EndPoint, 0, 0), GetSumPoint(clipRightLine.EndPoint, 0, 0));

            //////////////////////CALCULATE DISTANCE for OTHER VIEW////////////////////////
            //////////////////////The order of the holes represents ///////////////////////
            /////////////////the order from the pad based on the TOP VIEW./////////////////
            Circle firstHole = (Circle)holes[0];
            Circle secondHole = (Circle)holes[1];
            Circle thirdHole = (Circle)holes[2];
            Circle fourthHole = (Circle)holes[3];


            double padTOhole1 = firstHole.Center.X - (workingPoint.X + t + t);
            double padTOhole2 = secondHole.Center.X - (workingPoint.X + t + t);
            double padTOhole3 = thirdHole.Center.X - (workingPoint.X + t + t);
            double padTOhole4 = fourthHole.Center.X - (workingPoint.X + t + t);

            double clipRightLineStrartHeight = clipRightLine.StartPoint.Y - clipBottomLine.EndPoint.Y;
            double clipRightLineEndHeight = clipRightLine.EndPoint.Y - clipBottomLine.EndPoint.Y;


            //////////////////////TOPVIEW/////////////////////
            double padHeight_t = 130;
            double clipThk_k1 = 12;

            List<Line> padTop = GetRectangle(GetSumPoint(topViewWorkingPoint, 0, -padHeight_t / 2), padHeight_t, t, RECTANGLUNVIEW.NONE);
            List<Line> clipTop = GetRectangle(GetSumPoint(topViewWorkingPoint, t, 0), clipThk_k1, G, RECTANGLUNVIEW.NONE);

            Line columnOuterLine = new Line(GetSumPoint(topViewWorkingPoint, 0, -padHeight_t), GetSumPoint(topViewWorkingPoint, 0, padHeight_t));
            Line columnInnerLine = new Line(GetSumPoint(topViewWorkingPoint, -t, -padHeight_t),
                                            GetSumPoint(topViewWorkingPoint, -t, padHeight_t));
            Line HoleCenterLine1 = new Line(GetSumPoint(topViewWorkingPoint, t + padTOhole1, clipThk_k1 * 1.5),
                                            GetSumPoint(topViewWorkingPoint, t + padTOhole1, -clipThk_k1 * 0.5));
            Line HoleCenterLine2 = new Line(GetSumPoint(topViewWorkingPoint, t + padTOhole2, clipThk_k1 * 1.5),
                                            GetSumPoint(topViewWorkingPoint, t + padTOhole2, -clipThk_k1 * 0.5));
            Line HoleCenterLine3 = new Line(GetSumPoint(topViewWorkingPoint, t + padTOhole3, clipThk_k1 * 1.5),
                                            GetSumPoint(topViewWorkingPoint, t + padTOhole3, -clipThk_k1 * 0.5));
            Line HoleCenterLine4 = new Line(GetSumPoint(topViewWorkingPoint, t + padTOhole4, clipThk_k1 * 1.5),
                                            GetSumPoint(topViewWorkingPoint, t + padTOhole4, -clipThk_k1 * 0.5));

            ////////////////////FRONTVIEW///////////////////

            List<Line> padFront = GetRectangle(GetSumPoint(frontViewWorkingPoint, -padHeight_t, 0), F, padHeight_t, RECTANGLUNVIEW.TOP);
            List<Line> clipFront = GetRectangle(GetSumPoint(frontViewWorkingPoint, -padHeight_t / 2, (F - G) / 2), clipRightLineStrartHeight, clipThk_k1, RECTANGLUNVIEW.NONE);
            Line clipFrontStartLine = new Line(GetSumPoint(frontViewWorkingPoint, (-padHeight_t / 2) * 1.2, (F + G) / 2),
                                                  GetSumPoint(frontViewWorkingPoint, -padHeight_t / 2 + clipThk_k1 * 2, (F + G) / 2));
            Line clipRightEndLine = new Line(GetSumPoint(frontViewWorkingPoint, -padHeight_t / 2, clipRightLineEndHeight),
                                                  GetSumPoint(frontViewWorkingPoint, -padHeight_t / 2 + clipThk_k1, clipRightLineEndHeight));
            Line clipFrontTopLeftLine = new Line(GetSumPoint(frontViewWorkingPoint, -padHeight_t, F),
                                                  GetSumPoint(frontViewWorkingPoint, -padHeight_t / 2, F));
            Line clipFrontTopRightLine = new Line(GetSumPoint(frontViewWorkingPoint, -padHeight_t / 2 + clipThk_k1, F),
                                                  GetSumPoint(frontViewWorkingPoint, 0, F));


            if (drawType == 1)
            {
                //clip
                drawList.outlineList.AddRange(pad);
                //drawList.outlineList.AddRange(rafter);
                drawList.outlineList.AddRange(slotHoles);

                drawList.outlineList.AddRange(new Entity[] {

                clipTopLinkLine, clipTopslideLine, clipRightLine,
                clipBottomLine, clipBottomslideLine,


            });
            }
            else if (drawType == 2)
            {
                //Top View
                drawList.outlineList.AddRange(padTop);
                drawList.outlineList.AddRange(clipTop);

                drawList.outlineList.AddRange(new Entity[] {

                columnOuterLine, columnInnerLine, HoleCenterLine1, HoleCenterLine2, HoleCenterLine3, HoleCenterLine4,

            });
            }
            else if (drawType == 3)
            {
                //Front View
                drawList.outlineList.AddRange(padFront);
                drawList.outlineList.AddRange(clipFront);

                drawList.outlineList.AddRange(new Entity[] {

                clipFrontStartLine, clipRightEndLine, clipFrontTopLeftLine, clipFrontTopRightLine
            });
            }


            //styleService.SetLayerListEntity(ref outlineList, layerService.LayerOutLine);
            return drawList;

        }




        private List<Line> GetOffsetDoubleLineRectangle(Point3D refPoint, double width, double length, double thk, double roofTOclip, out Point3D clipstartPoint)
        {
            //ref Point는 좌측상단으로 넣으나 기존 함수와 배열 순서는 같음!

            List<Line> outlineList = new List<Line>();

            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            //doubleLineList = GetRectangle(referencePoint, width, length, unview);

            /* Index List
             * 0 : bottom ,  1: second OutLine, 2: third OutLine
             * 3 : bottom InnerLine, 4 : top InnerLIne
            /**/
            Line topLine = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(refPoint, length, 0));
            Line TopSecondLine = (Line)topLine.Offset(thk, Vector3D.AxisZ);
            Line BottomSecondLine = (Line)topLine.Offset(width - thk, Vector3D.AxisZ);
            Line bottomLine = (Line)topLine.Offset(width, Vector3D.AxisZ);

            Line forOutPoint = (Line)topLine.Offset(roofTOclip, Vector3D.AxisZ);
            clipstartPoint = forOutPoint.StartPoint;

            Line rightLine = new Line(GetSumPoint(topLine.EndPoint, 0, 0), GetSumPoint(bottomLine.EndPoint, 0, 0));
            Line leftLine = new Line(GetSumPoint(topLine.StartPoint, 0, 0), GetSumPoint(bottomLine.StartPoint, 0, 0));


            //Line BottomSecondLine = new Line(GetSumPoint(doubleLineList[0].StartPoint, 0, thk), GetSumPoint(doubleLineList[0].EndPoint, 0, thk));
            //Line TopSecondLine = new Line(GetSumPoint(doubleLineList[2].StartPoint, 0, -thk), GetSumPoint(doubleLineList[2].EndPoint, 0, -thk));


            outlineList.AddRange(new Line[] {
               bottomLine,rightLine, topLine, leftLine,
                TopSecondLine,BottomSecondLine
            });

            return outlineList;

        }
            private List<Entity> GetHoles(Point3D refPoint, double holeRadius, double lengthGap, double widthGap = 0)
        {
            //refPoint는 홀과 홀 사이의 중앙인 점을 입력해주시면됩니다
            //lengthGap=0인경우는 2개, lengthGap에 값이 입력된 경우는 홀이 4개로 그려집니다 :)

            List<Entity> holeList = new List<Entity>();
            Point3D workingPoint = GetSumPoint(refPoint, 0, 0);


            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Point3D circleCenterPoint = GetSumPoint(workingPoint, lengthGap / 2 * (2 * i - 1), widthGap / 2 * (1 - 2 * j));

                    Circle hole = new Circle(circleCenterPoint, holeRadius / 2);

                    holeList.AddRange(new Entity[] { hole });
                }
            }

            styleService.SetLayerListEntity(ref holeList, layerService.LayerOutLine);

            return holeList;

        }

        private List<Entity> GetSlotHoles(Point3D refPoint, double holeRadius, double slotHoleLength, double lengthGap, double widthGap = 0)
        {
            //refPoint는 홀과 홀 사이의 중앙인 점을 입력해주시면됩니다
            //lengthGap=0인경우는 2개, lengthGap에 값이 입력된 경우는 홀이 4개로 그려집니다 :)

            List<Entity> slotHoleList = new List<Entity>();
            Point3D workingPoint = GetSumPoint(refPoint, 0, 0);

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Point3D circleCenterPoint = GetSumPoint(workingPoint, lengthGap / 2 * (2 * i - 1), widthGap / 2 * (1 - 2 * j));

                    Arc SlotLeftArc = new Arc(GetSumPoint(circleCenterPoint, (-slotHoleLength + holeRadius) / 2, holeRadius / 2),
                                                GetSumPoint(circleCenterPoint, -slotHoleLength / 2, 0),
                                                GetSumPoint(circleCenterPoint, (-slotHoleLength + holeRadius) / 2, -holeRadius / 2), false);
                    Arc SlotRightArc = new Arc(GetSumPoint(circleCenterPoint, (slotHoleLength - holeRadius) / 2, holeRadius / 2),
                                                GetSumPoint(circleCenterPoint, slotHoleLength / 2, 0),
                                                GetSumPoint(circleCenterPoint, (slotHoleLength - holeRadius) / 2, -holeRadius / 2), false);
                    Line SlotTopLine = new Line(GetSumPoint(SlotRightArc.StartPoint, 0, 0), GetSumPoint(SlotLeftArc.StartPoint, 0, 0));
                    Line SlotBottomLine = new Line(GetSumPoint(SlotRightArc.EndPoint, 0, 0), GetSumPoint(SlotLeftArc.EndPoint, 0, 0));

                    slotHoleList.AddRange(new Entity[] { SlotLeftArc, SlotRightArc, SlotTopLine, SlotBottomLine });
                }
            }

            styleService.SetLayerListEntity(ref slotHoleList, layerService.LayerOutLine);
            return slotHoleList;

        }










        #endregion




        #region Jang
        private DrawEntityModel testDraw(Point3D refPoint, double outerDiameter, double scaleValue)
        {

            DrawEntityModel drawList = new DrawEntityModel();

            List<DrawOnePlateModel> onePlateList = new List<DrawOnePlateModel>();
            List<Entity> outlinesList = new List<Entity>();
            List<Entity> plateList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            ///////////////////////
            ///  CAD Data Setting
            ///////////////////////


            ////////////////////////////////////////////// ///////////////////////////////////////////


            //

            //////////////////////////////////////////////////////////
            // Draw BendingPlate
            //////////////////////////////////////////////////////////



            outlinesList.AddRange(new Entity[] {

            });

            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(outlinesList);

            return drawList;
        }


        // 되는 것
        public DrawEntityModel testDrawCenterRingTop(Point3D refPoint, double scaleValue)
        {

            DrawEntityModel drawList = new DrawEntityModel();

            List<DrawOnePlateModel> onePlateList = new List<DrawOnePlateModel>();
            List<Entity> centerTopSupportList = new List<Entity>();
            List<Entity> columnList = new List<Entity>();
            List<Entity> columnRIb = new List<Entity>();
            List<Entity> outlinesList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            ///////////////////////
            ///  CAD Data Setting
            ///////////////////////



            centerTopSupportList = drawComService.GetColumnCenterTopSupport(referencePoint, scaleValue);
            centerTopSupportList = drawComService.GetColumnCenterTopSupport_TopView(referencePoint, scaleValue);

            //columnList = drawComService.GetColumnBaseSupport_SideView(referencePoint, scaleValue);


            outlinesList.AddRange(new Entity[] {

            });

            //styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

            drawList.outlineList.AddRange(outlinesList);
            drawList.outlineList.AddRange(centerTopSupportList);
            drawList.outlineList.AddRange(columnList);
            drawList.outlineList.AddRange(columnRIb);

            return drawList;
        }

        // 되는 것 : 
        public  DrawEntityModel testDrawCenterColumn(Point3D refPoint, double scaleValue)
        {

            DrawEntityModel drawList = new DrawEntityModel();

            List<DrawOnePlateModel> onePlateList = new List<DrawOnePlateModel>();
            List<Entity> centerTopSupportList = new List<Entity>();
            List<Entity> columnList = new List<Entity>();
            List<Entity> columnRIb = new List<Entity>();
            List<Entity> outlinesList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            ///////////////////////
            ///  CAD Data Setting
            ///////////////////////
            double INT_OD = 800;
            double INT_A = 250;
            double INT_B = 230;
            double INT_C = 55;
            double INT_D = 100;

            double EXT_A = 200;
            double EXT_B = 100;
            double EXT_C = 10;
            double EXT_D = 0;

            double t1 = 10;
            double t2 = 10;

            double slopeRadian = Utility.DegToRad(10);
            double tankOD = 10000;

            Point3D FlangeWorkPoint = null;
            ////////////////////////////////////////////// ///////////////////////////////////////////
            //centerRingList = shapeDrawService.GetCenterRing_SideView(referencePoint, CENTERRING_TYPE.CRT_INT, INT_OD, slopeRadian, INT_A, INT_B, INT_C, INT_D, t1, t2, tankOD, out workPoint);
            //centerRingList = shapeDrawService.GetCenterRing_SideView(referencePoint, CENTERRING_TYPE.DRT_INT, INT_OD, slopeRadian, INT_A, INT_B, INT_C, INT_D, t1, t2, tankOD, out FlangeWorkPoint);
            //centerRingList = shapeDrawService.GetCenterRing_SideView(referencePoint, CENTERRING_TYPE.CRT_EXT, INT_OD, slopeRadian, EXT_A, EXT_B, EXT_C, EXT_D, t1, t2, tankOD);
            //centerRingList = shapeDrawService.GetCenterRing_SideView(referencePoint, CENTERRING_TYPE.DRT_EXT, INT_OD, slopeRadian, EXT_A, EXT_B, EXT_C, EXT_D, t1, t2, tankOD);


            Point3D domeCenterPoint = GetSumPoint(referencePoint, 0, -(tankOD / 2));
            //Point3D clip_CenterRingWorkPoint = GetSumPoint(FlangeWorkPoint, INT_C, -t1);

            //// CRT_INT  or DRT_INT
            Clip_CenterRIng clipInfo = new Clip_CenterRIng(DrawShapeLib.Commons.CENTERRING_TYPE.CRT_INT);
            //Clip_CenterRIng clipInfo = new Clip_CenterRIng(CENTERRING_TYPE.DRT_INT);

            columnList = drawComService.GetCenterColumn(referencePoint, clipInfo, INT_C, INT_B, t1, slopeRadian, domeCenterPoint);


            outlinesList.AddRange(new Entity[] {

            });

            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

            drawList.outlineList.AddRange(outlinesList);
            drawList.outlineList.AddRange(centerTopSupportList);
            drawList.outlineList.AddRange(columnList);
            drawList.outlineList.AddRange(columnRIb);

            return drawList;
        }


        // Column : Center side : TopView
        public DrawEntityModel DrawColumnCenterSideTopView(Point3D refPoint, double scaleValue)
        {

            DrawEntityModel drawList = new DrawEntityModel();

            List<DrawOnePlateModel> onePlateList = new List<DrawOnePlateModel>();
            List<Entity> centerRingList = new List<Entity>();
            List<Entity> outlinesList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            ///////////////////////
            ///  CAD Data Setting
            ///////////////////////

            ////////////////////////////////////////////// ///////////////////////////////////////////
            //centerRingList = shapeDrawService.GetRingTypeCenterRing_TopView(referencePoint, CENTERRING_TYPE.CRT_INT, 800, 230, 55, 100, 10, 10, scaleValue );
            centerRingList = drawComService.GetRingTypeCenterRing_TopView(referencePoint, DrawShapeLib.Commons.CENTERRING_TYPE.DRT_INT, 800, 230, 55, 100, 10, 10, scaleValue);
            //centerRingList = shapeDrawService.GetRingTypeCenterRing_TopView(referencePoint, CENTERRING_TYPE.CRT_EXT, 800, 100, 100, 0, 10, 10, scaleValue);
            //centerRingList = shapeDrawService.GetRingTypeCenterRing_TopView(referencePoint, CENTERRING_TYPE.DRT_EXT, 800, 100, 100, 0, 10, 10, scaleValue);


            outlinesList.AddRange(new Entity[] {

            });

            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

            drawList.outlineList.AddRange(outlinesList);
            drawList.outlineList.AddRange(centerRingList);

            return drawList;
        }

        // Centering + Centering Clip
        public  DrawEntityModel testDrawCenterRing(Point3D refPoint, double scaleValue)
        {

            DrawEntityModel drawList = new DrawEntityModel();

            List<DrawOnePlateModel> onePlateList = new List<DrawOnePlateModel>();
            List<Entity> centerRingList = new List<Entity>();
            List<Entity> clip_centerRingList = new List<Entity>();
            List<Entity> outlinesList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            ///////////////////////
            ///  CAD Data Setting
            ///////////////////////
            double INT_OD = 800;
            double INT_A = 250;
            double INT_B = 230;
            double INT_C = 55;
            double INT_D = 100;

            double EXT_A = 200;
            double EXT_B = 100;
            double EXT_C = 10;
            double EXT_D = 0;

            double t1 = 10;
            double t2 = 10;

            double slopeRadian = Utility.DegToRad(10);
            double tankOD = 10000;

            Point3D FlangeWorkPoint = null;
            ////////////////////////////////////////////// ///////////////////////////////////////////
            // centerRingList = shapeDrawService.GetCenterRing_SideView(referencePoint, CENTERRING_TYPE.CRT_INT, INT_OD, slopeRadian, INT_A, INT_B, INT_C, INT_D, t1, t2, tankOD, out workPoint);
            centerRingList = drawComService.GetCenterRing_SideView(referencePoint, DrawShapeLib.Commons.CENTERRING_TYPE.DRT_INT, INT_OD, slopeRadian, INT_A, INT_B, INT_C, INT_D, t1, t2, tankOD, out FlangeWorkPoint);
            //centerRingList = shapeDrawService.GetCenterRing_SideView(referencePoint, CENTERRING_TYPE.CRT_EXT, INT_OD, slopeRadian, EXT_A, EXT_B, EXT_C, EXT_D, t1, t2, tankOD);
            //centerRingList = shapeDrawService.GetCenterRing_SideView(referencePoint, CENTERRING_TYPE.DRT_EXT, INT_OD, slopeRadian, EXT_A, EXT_B, EXT_C, EXT_D, t1, t2, tankOD);


            Point3D domeCenterPoint = GetSumPoint(referencePoint, 0, -(tankOD / 2));
            Point3D clip_CenterRingWorkPoint = GetSumPoint(FlangeWorkPoint, INT_C, -t1);

            // CRT_INT  or DRT_INT
            Clip_CenterRIng clipInfo = new Clip_CenterRIng(DrawShapeLib.Commons.CENTERRING_TYPE.CRT_INT);
            //Clip_CenterRIng clipInfo = new Clip_CenterRIng(CENTERRING_TYPE.DRT_INT);
            clip_centerRingList = drawComService.GetClip_CenterRingSide(clip_CenterRingWorkPoint, clipInfo, INT_C, INT_B, t1, slopeRadian, domeCenterPoint);


            outlinesList.AddRange(new Entity[] {

            });

            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

            drawList.outlineList.AddRange(outlinesList);
            drawList.outlineList.AddRange(centerRingList);
            drawList.outlineList.AddRange(clip_centerRingList);

            return drawList;
        }






        // 2021-10-22


        public DrawEntityModel DrawColumnBaseSupport(Point3D refPoint, object selModel, double scaleValue, NColumnBaseSupportModel padModel, PipeModel pipeModel)
        {
            DrawEntityModel drawList = new DrawEntityModel();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);
            List<Entity> newList = new List<Entity>();



            double A = valueService.GetDoubleValue(padModel.A);


            // refPoint : Bottom Center

            List<Entity> entityList = new List<Entity>();
            List<Entity> centerlinesList = new List<Entity>();
            List<Entity> hiddenlinesList = new List<Entity>();
            List<Entity> outlinesList = new List<Entity>();

            //////////////////////////////////////////////////////////
            // Input CAD DATA
            //////////////////////////////////////////////////////////
            // column_BaseSupport Data

            double pipeOD = valueService.GetDoubleValue(padModel.OD);// PipeOD
            double pipeThk = 7.11; // Pipe THK : SCH.40S
            double ribLength = valueService.GetDoubleValue(padModel.A); // A
            double ribWidth = valueService.GetDoubleValue(padModel.A1); // A1
            double sleeveLength = valueService.GetDoubleValue(padModel.B);//pipeOD + 26; // B
            double baseTopPlateLength = valueService.GetDoubleValue(padModel.C); // C
            double baseTopPlateThk = valueService.GetDoubleValue(padModel.I1); // I1
            double ribTopFlat = valueService.GetDoubleValue(padModel.D1); // D1
            double ribChamfer = 15; // @1
            double sleeveWidth = valueService.GetDoubleValue(padModel.B1); // B1 = a1-c1 500 // data shee 오류  200으로 나옴
            double RibTopWidth = valueService.GetDoubleValue(padModel.C); // C1
            double radius = valueService.GetDoubleValue(padModel.F1); // F1
            double sleeveThk = valueService.GetDoubleValue(padModel.H1); // H1
            double baseBottomPlateLength = 700; // valueService.GetDoubleValue(padModel.E); // E
            double baseBottomPlateThk = 12; // D BTM plate thk.
            double gapEachAngle = valueService.GetDoubleValue(padModel.F); // F
            double angleLenght = 75; // J1  - string 일부 읽기
            double angleThk = 6; // J1
            double angleWidth = valueService.GetDoubleValue(padModel.K1); // K1
            double distanceSleeve = valueService.GetDoubleValue(padModel.E1); // E1

            double drainLength = 24;
            double drainRadius = 5;


            double distanceRib = sleeveThk + distanceSleeve; // E1+H1
            double radiusOuter = pipeOD / 2;
            double pipeInnerRadius = radiusOuter - pipeThk;

            double pipeHeight = ribWidth + RibTopWidth;  // +Ribtopwidth = 임의갑 추가높이


            ////////////////////////////////
            /// Draw 
            ////////////////////////////////

            double basePlateHeight = baseTopPlateThk + baseBottomPlateThk;
            double distanceWorkPoint = (distanceRib + radiusOuter); // Gap Sleeve for PipeCenterPoint

            // Draw : Rib
            List<Entity> ribList = GetColumnRib(GetSumPoint(referencePoint, -(distanceRib + radiusOuter), basePlateHeight), distanceWorkPoint, scaleValue, padModel);
            outlinesList.AddRange(ribList);
            // mirror Rib(right)
            outlinesList.AddRange(editingService.GetMirrorEntity(Plane.YZ, ribList, referencePoint.X, referencePoint.Y, true));

            // Draw : Sleeve
            List<Entity> sleeveList = GetColumnSleeve(GetSumPoint(referencePoint, -(distanceRib + radiusOuter), basePlateHeight), distanceWorkPoint,
                                                                   scaleValue, padModel);
            outlinesList.AddRange(sleeveList);
            // mirror Sleeve(right)
            outlinesList.AddRange(editingService.GetMirrorEntity(Plane.YZ, sleeveList, referencePoint.X, referencePoint.Y, true));



            // Draw : Base Plate // baseTopPlateLength, baseBottomPlateLength
            // Draw : base TopPlate
            // todo : Chanage Calculate length
            Line guideBaseTopLength = new Line(CopyPoint(referencePoint), GetSumPoint(referencePoint, baseTopPlateLength, 0));
            guideBaseTopLength.Rotate(Utility.DegToRad(45), Vector3D.AxisZ, referencePoint);
            /**/
            double baseTopDiagonalLength = guideBaseTopLength.EndPoint.X - guideBaseTopLength.StartPoint.X;
            List<Line> baseTopPlate = GetRectangle(
                GetSumPoint(referencePoint, -baseTopDiagonalLength, baseBottomPlateThk), baseTopPlateThk, baseTopDiagonalLength * 2);
            outlinesList.AddRange(baseTopPlate);

            // Draw : base BottomPlate
            // todo : Chanage Calculate length
            Line guideBaseBottomLength = new Line(CopyPoint(referencePoint), GetSumPoint(referencePoint, baseBottomPlateLength, 0));
            guideBaseBottomLength.Rotate(Utility.DegToRad(45), Vector3D.AxisZ, referencePoint);
            /**/
            double baseBottomDiagonalLength = guideBaseBottomLength.EndPoint.X - guideBaseBottomLength.StartPoint.X;
            List<Line> baseBottomPlate = GetRectangle(
                GetSumPoint(referencePoint, -baseBottomDiagonalLength, 0), baseBottomPlateThk, baseBottomDiagonalLength * 2);
            outlinesList.AddRange(baseBottomPlate);




            // Draw : CenterLine



            outlinesList.AddRange(new Entity[] {
                //pipeInnerLeft, pipeInnerRight,
            });

            centerlinesList.AddRange(new Entity[] {

            });

            hiddenlinesList.AddRange(new Entity[] {
               // sleeveTopHiddenLn, sleeveVeticalHiddenLn,
            });

            styleService.SetLayerListEntity(ref centerlinesList, layerService.LayerCenterLine);
            styleService.SetLayerListEntity(ref hiddenlinesList, layerService.LayerHiddenLine);
            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

            newList.AddRange(centerlinesList);
            newList.AddRange(hiddenlinesList);
            newList.AddRange(outlinesList);

            drawList.outlineList.AddRange(newList);

            return drawList;
        }

        public DrawEntityModel DrawColumnCenterTopSupport(Point3D refPoint, object selModel, double scaleValue, NColumnCenterTopSupportModel padModel, PipeModel pipeModel)
        {
            DrawEntityModel drawList = new DrawEntityModel();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);


            // refPoint : Column Height Top Center

            List<Entity> outlinesList = new List<Entity>();
            List<Entity> topPlateList = new List<Entity>();
            List<Entity> topRIbList = new List<Entity>();

            //////////////////////////////////////////////////////////
            // Input CAD DATA
            //////////////////////////////////////////////////////////

            double pipeOD = valueService.GetDoubleValue(pipeModel.OD); // PipeOD
            double distanceRib = valueService.GetDoubleValue(padModel.A1); // A1
            double ribTop = valueService.GetDoubleValue(padModel.B1); // B1
            double centerRingThk = valueService.GetDoubleValue(padModel.C1);// C1
            double ribChamfer = valueService.GetDoubleValue(padModel.E1); // E1
            double ribLength = valueService.GetDoubleValue(padModel.G); // G
            double ribHeight = valueService.GetDoubleValue(padModel.H); // H
            double radiusOuter = (pipeOD / 2) + ribLength + distanceRib; // CenterRing RadiusOD

            /////////////////////


            ////////////////////////////////
            /// Draw 
            ////////////////////////////////

            Point3D centerRIngStarPoint = GetSumPoint(referencePoint, -radiusOuter, -centerRingThk);
            topPlateList.AddRange(GetRectangle(CopyPoint(centerRIngStarPoint), centerRingThk, radiusOuter, RECTANGLUNVIEW.RIGHT));

            List<Line> guideRibBox = GetRectangleLT(GetSumPoint(centerRIngStarPoint, distanceRib, 0), ribHeight, ribLength);

            //guideRibBox[0].TrimBy(GetSumPoint(guideRibBox[0].EndPoint, -ribTop,0), true);
            guideRibBox[0].TrimAt(ribLength - ribTop, true);
            guideRibBox[3].TrimAt(ribTop, false);
            Line ribDiagonal = new Line(guideRibBox[3].EndPoint, guideRibBox[0].StartPoint);

            Line ribChamferLine = GetChamferLine(guideRibBox[2], guideRibBox[1], ribChamfer);
            guideRibBox[2].TrimBy(ribChamferLine.StartPoint, false);
            guideRibBox[1].TrimBy(ribChamferLine.EndPoint, true);


            topRIbList.AddRange(guideRibBox);

            //outlinesList.AddRange(rightBoxList);
            topRIbList.AddRange(new Entity[] {
                ribDiagonal, ribChamferLine,
                //RBoxBottomLine, RBoxRightLine, RBoxTopLine,
            });

            // mirror topPlate(right)
            topPlateList.AddRange(editingService.GetMirrorEntity(Plane.YZ, topPlateList, referencePoint.X, referencePoint.Y, true));
            outlinesList.AddRange(topPlateList);

            // mirror topRib(right)
            topRIbList.AddRange(editingService.GetMirrorEntity(Plane.YZ, topRIbList, referencePoint.X, referencePoint.Y, true));
            outlinesList.AddRange(topRIbList);


            styleService.SetLayerListEntity(ref topPlateList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref topRIbList, layerService.LayerOutLine);

            drawList.outlineList.AddRange(outlinesList);

            return drawList;

        }


        public List<Entity> GetColumnRib(Point3D refPoint, double distanceWorkPoint, double scaleValue, NColumnBaseSupportModel padModel)
        {

            // refPoint : sleeveThk+ distanceSleeve + CenterPipe

            List<Entity> entityList = new List<Entity>();
            List<Entity> centerlinesList = new List<Entity>();
            List<Entity> hiddenlinesList = new List<Entity>();
            List<Entity> outlinesList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            //////////////////////////////////////////////////////////
            // Input CAD DATA
            //////////////////////////////////////////////////////////

            double pipeOD = 168.3; // PipeOD
            double ribLength = valueService.GetDoubleValue(padModel.A);// 300; // A
            double ribWidth = valueService.GetDoubleValue(padModel.A1);//700; // A1
            double ribTopFlat = valueService.GetDoubleValue(padModel.D1);//50; // D1
            double ribChamfer = 15; // @1
            double sleeveWidth = valueService.GetDoubleValue(padModel.B1);// 500; // B1
            double RibTopWidth = valueService.GetDoubleValue(padModel.C1);// 200; // C1
            double radius = valueService.GetDoubleValue(padModel.F1);// 15; // F1
            double sleeveThk = valueService.GetDoubleValue(padModel.H1);// 10; // H1
            double distanceSleeve = valueService.GetDoubleValue(padModel.E1);// 3; // E1

            /////////////////////


            ////////////////////////////////
            /// Draw 
            ////////////////////////////////
            double ribBottomLength = ribLength - sleeveThk;
            List<Point3D> topBoxPointList = new List<Point3D>();
            List<Entity> guideTopBox = GetRectangleK(GetSumPoint(referencePoint, -ribBottomLength, sleeveWidth), RibTopWidth, ribLength, out topBoxPointList);
            List<Line> guideBottomBox = GetRectangle(GetSumPoint(referencePoint, -ribBottomLength, 0), sleeveWidth, ribBottomLength, RECTANGLUNVIEW.TOP);

            // Draw Arc : CenterPoint of Arc
            //Point3D arcCenterPoint = CopyPoint( topBoxPointList[1] );
            Arc guideRibArc = new Arc(topBoxPointList[1], radius, Utility.DegToRad(90), Utility.DegToRad(270));


            // Draw : chamfer  index0:bottom  index1: right
            Line ribChamferLine = GetChamferLine(guideBottomBox[0], guideBottomBox[1], ribChamfer, true);

            // Trim : TopLine, LeftLine, RightLine
            topBoxPointList[3] = GetSumPoint(topBoxPointList[2], -ribTopFlat, 0);
            guideBottomBox[2].StartPoint = GetSumPoint(guideBottomBox[2].EndPoint, 0, ribTopFlat);
            Point3D[] ribBottomRightTop = guideBottomBox[1].IntersectWith(guideRibArc);
            guideBottomBox[1].StartPoint = CopyPoint(ribBottomRightTop[0]);
            Arc ribArc = new Arc(topBoxPointList[1], guideRibArc.StartPoint, ribBottomRightTop[0]);

            // Draw : Rib TopLine
            Line ribTopLine = new Line(topBoxPointList[3], topBoxPointList[2]);
            Line ribTopRightLine = new Line(topBoxPointList[2], GetSumPoint(topBoxPointList[1], 0, radius));

            // Draw : slope Line
            Line ribDiagonalLine = new Line(topBoxPointList[3], guideBottomBox[2].StartPoint);


            //outlinesList.AddRange(guideRibBox);

            //outlinesList.AddRange(ribList);
            outlinesList.AddRange(new Entity[] {
                ribChamferLine, ribArc,
                ribTopLine, ribTopRightLine, ribDiagonalLine
                //circleOD, centerHole, //ribLeftLine,ribRightLine
            });

            centerlinesList.AddRange(new Entity[] {
                //circleHoleCenter,
            });

            //hiddenlinesList.AddRange(guideTopBox);
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

        public List<Entity> GetColumnSleeve(Point3D refPoint, double distanceWorkPoint, double scaleValue, NColumnBaseSupportModel padModel)
        {
            //  double SleeveWidth, double SleeveLength, double SleeveThk,
            // refPoint : sleeveThk+ distanceSleeve + CenterPipe

            List<Entity> entityList = new List<Entity>();
            List<Entity> centerlinesList = new List<Entity>();
            List<Entity> hiddenlinesList = new List<Entity>();
            List<Entity> outlinesList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            //////////////////////////////////////////////////////////
            // Input CAD DATA
            //////////////////////////////////////////////////////////

            double sleeveWidth = valueService.GetDoubleValue(padModel.B1); //SleeveWidth; // B1
            double sleeveLength = valueService.GetDoubleValue(padModel.B); //SleeveLength; // B
            double sleeveThk = valueService.GetDoubleValue(padModel.H1);// SleeveThk; // H1
            double DistanceSleeve = valueService.GetDoubleValue(padModel.E1); // E1
            /////////////////////


            ////////////////////////////////
            /// Draw 
            ////////////////////////////////
            List<Point3D> sleevePoint = null;
            outlinesList.AddRange(GetRectangleK(CopyPoint(referencePoint), sleeveWidth, sleeveThk, out sleevePoint));


            outlinesList.Add(new Line(sleevePoint[3], GetSumPoint(sleevePoint[3], DistanceSleeve, 0)));

            outlinesList.AddRange(new Entity[] {

            });

            centerlinesList.AddRange(new Entity[] {
                //circleHoleCenter,
            });

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


        public List<Entity> GetRectangleK(Point3D selPoint, double Width, double Length, out List<Point3D> selOutputPointList,
                    bool isTopLeft = false, double rotateRadian = 0, double rotateCenterNum = 0, double selTranslateNumber = 0,
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

            if (isTopLeft)
            {
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
                editingService.SetTranslate(ref newList, GetSumPoint(selPoint, 0, 0), WPTranslate);

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

            // Out : PointList
            selOutputPointList.AddRange(new Point3D[] { A, B, C, D });

            return newList;
        }


        public DrawEntityModel DrawColumnSideTopSupport(Point3D refPoint, object selModel, double scaleValue,NColumnSideTopSupportModel padModel, PipeModel pipeModel)
        {
            DrawEntityModel drawList = new DrawEntityModel();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);


            // refPoint : Column Height Top Center

            List<Entity> outlinesList = new List<Entity>();
            List<Entity> topPlateList = new List<Entity>();
            List<Entity> topRIbList = new List<Entity>();

            //////////////////////////////////////////////////////////
            // Input CAD DATA
            //////////////////////////////////////////////////////////

            double pipeOD = valueService.GetDoubleValue( pipeModel.OD); // 168.3; // PipeOD
            double distanceRib = valueService.GetDoubleValue(padModel.D1); // D1
            double ribTop = valueService.GetDoubleValue(padModel.E1); // E1
            double centerRingThk = valueService.GetDoubleValue(padModel.A1);// A1
            double ribChamfer = valueService.GetDoubleValue(padModel.G1); // G1
            double ribLength = valueService.GetDoubleValue(padModel.C1); // C1
            double ribHeight = valueService.GetDoubleValue(padModel.B1); // B1
            double radiusOuter = (pipeOD / 2) + ribLength + distanceRib; // CenterRing RadiusOD

            /////////////////////


            ////////////////////////////////
            /// Draw 
            ////////////////////////////////

            Point3D centerRIngStarPoint = GetSumPoint(referencePoint, -radiusOuter, -centerRingThk);
            topPlateList.AddRange(GetRectangle(CopyPoint(centerRIngStarPoint), centerRingThk, radiusOuter, RECTANGLUNVIEW.RIGHT));

            List<Line> guideRibBox = GetRectangleLT(GetSumPoint(centerRIngStarPoint, distanceRib, 0), ribHeight, ribLength);

            //guideRibBox[0].TrimBy(GetSumPoint(guideRibBox[0].EndPoint, -ribTop,0), true);
            guideRibBox[0].TrimAt(ribLength - ribTop, true);
            guideRibBox[3].TrimAt(ribTop, false);
            Line ribDiagonal = new Line(guideRibBox[3].EndPoint, guideRibBox[0].StartPoint);

            Line ribChamferLine = GetChamferLine(guideRibBox[2], guideRibBox[1], ribChamfer);
            guideRibBox[2].TrimBy(ribChamferLine.StartPoint, false);
            guideRibBox[1].TrimBy(ribChamferLine.EndPoint, true);


            topRIbList.AddRange(guideRibBox);

            //outlinesList.AddRange(rightBoxList);
            topRIbList.AddRange(new Entity[] {
                ribDiagonal, ribChamferLine,
                //RBoxBottomLine, RBoxRightLine, RBoxTopLine,
            });

            // mirror topPlate(right)
            topPlateList.AddRange(editingService.GetMirrorEntity(Plane.YZ, topPlateList, referencePoint.X, referencePoint.Y, true));
            outlinesList.AddRange(topPlateList);

            // mirror topRib(right)
            topRIbList.AddRange(editingService.GetMirrorEntity(Plane.YZ, topRIbList, referencePoint.X, referencePoint.Y, true));
            outlinesList.AddRange(topRIbList);


            styleService.SetLayerListEntity(ref topPlateList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref topRIbList, layerService.LayerOutLine);

            drawList.outlineList.AddRange(outlinesList);

            return drawList;

        }


        public List<Entity> GetColumnPipeCut(Point3D refPoint, double OuterDiameter, double Height, double topSupportThk, double baseHeight, double scaleValue)
        {
            // refPoint : bottom Center
            // gap : cut Y1 (distance:gap) cut Y2
            // PipeCutCount = 2,3,4,5

            List<Entity> EntityList = new List<Entity>();
            List<Entity> outlinesList = new List<Entity>();
            List<Entity> CenterLineList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            double scaleHeight = 330; // 도면에 표현될 크기
            double scale = Height / scaleHeight;  //  Pipe 길이를 도면크기로 만들 스케일 Size

            double Gap = 8 * scale;
            double cutLength3 = 120 * scale;   // 3등분
            double cutLength4 = 56 * scale;   // 4등분
            double cutLength5_Center = 64 * scale;  // 5등분 가운데
            double cutLength5_Side = 40 * scale;   // 5등분 위, 아래

            // set Datae
            double halfGap = Gap / 2;
            double PipeCutCount = 0;
            double pipeRadius = OuterDiameter / 2;

            // Count : pipecut
            if (Height <= 6000) PipeCutCount = 2;
            else if (Height <= 12000) PipeCutCount = 3;
            else if (Height <= 18000) PipeCutCount = 4;
            else if (Height <= 24000) PipeCutCount = 5;


            // Guide Info
            List<double> cut_YList = new List<double>();

            double pipeBottomHeight = 0;
            double pipeMidHeight = 0;
            double pipeTop = Height - topSupportThk; // clipHeight = clipHeight+Thk
            double halfHeight = Height / 2;




            Point3D pipeBottomPoint = GetSumPoint(referencePoint, 0, baseHeight);
            Point3D pipeMidPoint = GetSumPoint(referencePoint, 0, halfHeight);
            Point3D pipeTopPoint = GetSumPoint(referencePoint, 0, pipeTop);

            Line guideLine = new Line(pipeBottomPoint, pipeTopPoint);


            // set cut Point
            switch (PipeCutCount)
            {
                case 2:
                    cut_YList.Add(0);
                    break;
                case 3:
                    cut_YList.Add(cutLength3);
                    break;
                case 4:
                    cut_YList.Add(0);
                    cut_YList.Add(cutLength4);
                    break;
                case 5:
                    cut_YList.Add(cutLength5_Center);
                    cut_YList.Add(cutLength5_Side);
                    break;
            }



            int CutCount = cut_YList.Count();

            Point3D EachTopPoint = CopyPoint(pipeMidPoint);
            Point3D EachBottomPoint = CopyPoint(pipeMidPoint);

            for (int i = 0; i < CutCount; i++)
            {
                double eachHalfHeight = cut_YList[i] / 2;

                if (i == 0)
                {
                    EachTopPoint = GetSumPoint(pipeMidPoint, 0, eachHalfHeight);
                    EachBottomPoint = GetSumPoint(pipeMidPoint, 0, -eachHalfHeight);

                    if (cut_YList[i] == 0)
                    {
                        // ReSet : 다음 Pipe의 간격 조절 위해서
                        EachTopPoint = GetSumPoint(pipeMidPoint, 0, -halfGap);
                        EachBottomPoint = GetSumPoint(pipeMidPoint, 0, halfGap);
                    }
                    else
                    {
                        outlinesList.AddRange(GetColumnPipe(EachBottomPoint, OuterDiameter, cut_YList[i], VIEWPOSITION.TOP_BOTTOM));
                        outlinesList.Add(new Line(GetSumPoint(pipeMidPoint, -pipeRadius, 0),
                                                  GetSumPoint(pipeMidPoint, pipeRadius, 0)));
                    }
                }
                else
                {
                    Point3D upPipeBottomPoint = GetSumPoint(EachTopPoint, 0, Gap);
                    Point3D downPipeBottomPoint = GetSumPoint(EachBottomPoint, 0, -(Gap + cut_YList[i]));
                    // Draw : Cut Pipe
                    outlinesList.AddRange(GetColumnPipe(upPipeBottomPoint, OuterDiameter, cut_YList[i], VIEWPOSITION.TOP_BOTTOM));
                    outlinesList.AddRange(GetColumnPipe(downPipeBottomPoint, OuterDiameter, cut_YList[i], VIEWPOSITION.TOP_BOTTOM));

                    // Draw : Mid Horizontal Line
                    outlinesList.Add(new Line(GetSumPoint(upPipeBottomPoint, -pipeRadius, eachHalfHeight),
                                              GetSumPoint(upPipeBottomPoint, pipeRadius, eachHalfHeight)));
                    outlinesList.Add(new Line(GetSumPoint(downPipeBottomPoint, -pipeRadius, eachHalfHeight),
                                              GetSumPoint(downPipeBottomPoint, pipeRadius, eachHalfHeight)));

                    // ReSet : Next Point
                    EachTopPoint = GetSumPoint(EachTopPoint, 0, Gap + cut_YList[i]);
                    EachBottomPoint = GetSumPoint(EachBottomPoint, 0, -(Gap + cut_YList[i]));
                }
            }


            // 

            // Draw : bottom Pipe
            double bottomPipeHeight = (EachBottomPoint.Y - Gap) - pipeBottomPoint.Y;
            outlinesList.AddRange(GetColumnPipe(pipeBottomPoint, OuterDiameter, bottomPipeHeight, VIEWPOSITION.TOP));


            // Draw : Top Pipe
            double topPipeHeight = pipeTopPoint.Y - (EachTopPoint.Y + Gap);
            Point3D TopPipeBottom = GetSumPoint(EachTopPoint, 0, Gap);
            outlinesList.AddRange(GetColumnPipe(TopPipeBottom, OuterDiameter, topPipeHeight, VIEWPOSITION.BOTTOM));



            //////////////////////////////////////////////////////////
            // Draw Add
            //////////////////////////////////////////////////////////

            outlinesList.AddRange(new Entity[] {

            });

            CenterLineList.AddRange(new Entity[] {
                guideLine
            });

            /**/
            styleService.SetLayerListEntity(ref CenterLineList, layerService.LayerCenterLine);
            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerHiddenLine);
            EntityList.AddRange(CenterLineList);
            EntityList.AddRange(outlinesList);


            return EntityList;
        }

        public List<Entity> GetColumnPipe(Point3D refPoint, double outerDiameter, double height, VIEWPOSITION viewPosition, bool isInnerView = false, double thk = 0)
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

        #endregion




        #region Lee Ju
        public List<Entity> GetCenterColumn_BB(Point3D refPoint, double scaleValue)
        {
            List<Entity> outlinesList = new List<Entity>();

            Point3D referencePoint = GetSumPoint(new Point3D(refPoint.X, refPoint.Y), 0, 0);
            double startPoint_plus = 50;
            double rectangle_Outer = 800;
            double rectangle_inner = 700;

            double rectangle_Hide_inner = 750 / 2;

            double circle_Length_1 = 120;
            double circle_Length_2 = 124;
            double circle_Length_3 = 130;
            double circle_Length_4 = 134;

            double liner_Length = 485;
            double liner_Offset_Length = 10;

            double angle = 90;
            double degrees360 = 360;

            Point3D startPoint = GetSumPoint(referencePoint, 0, 0);
            Point3D startPointPlus = GetSumPoint(startPoint, startPoint_plus, startPoint_plus);
            Point3D centerPoint = GetSumPoint(startPoint, rectangle_Outer / 2, rectangle_Outer / 2);

            Point3D filletPoint = GetSumPoint(centerPoint, -rectangle_Hide_inner, rectangle_Hide_inner);

            List<Line> rectangleOuter = GetRectangle(startPoint, rectangle_Outer, rectangle_Outer);
            List<Line> rectangleInner = GetRectangle(startPointPlus, rectangle_inner, rectangle_inner);
            Circle center_Circle_1 = new Circle(centerPoint, circle_Length_1);
            Circle center_Circle_2 = new Circle(centerPoint, circle_Length_2);
            Circle center_Circle_3 = new Circle(centerPoint, circle_Length_3);
            Circle center_Circle_4 = new Circle(centerPoint, circle_Length_4);

            /*Fillet참고*/
            outlinesList.AddRange(GetAngleList(filletPoint, 75, 75, 6, SHAPEDIRECTION.RIGHTTOP, 5));
            outlinesList.AddRange(GetAngleList(GetSumPoint(filletPoint, 0, -rectangle_Hide_inner * 2), 75, 75, 6, SHAPEDIRECTION.RIGHTBOTTOM, 5));
            outlinesList.AddRange(GetAngleList(GetSumPoint(filletPoint, rectangle_Hide_inner * 2, -rectangle_Hide_inner * 2), 75, 75, 6, SHAPEDIRECTION.LEFTBOTTOM, 5));
            outlinesList.AddRange(GetAngleList(GetSumPoint(filletPoint, rectangle_Hide_inner * 2, 0), 75, 75, 6, SHAPEDIRECTION.LEFTTOP, 5));

            //필렛 예시문 private List<Entity> GetAngleList(Point3D refPoint, double width, double length, double thk, SHAPEDIRECTION shapeDirection = SHAPEDIRECTION.LEFTTOP, double radius = 0)



            for (int i = 0; i < degrees360; i++)
            {
                /*
                Line inner_Rotate_Line001 = new Line(GetSumPoint(centerPoint, circle_Length_4, 7), GetSumPoint(centerPoint, 620 - circle_Length_4, 0));
                inner_Rotate_Line001.Rotate(Utility.DegToRad((46) + angle * i), Vector3D.AxisZ, centerPoint);
                Line inner_Rotate_Line002 = new Line(GetSumPoint(centerPoint, circle_Length_4, -7), GetSumPoint(centerPoint, 620 - circle_Length_4, 0));
                inner_Rotate_Line002.Rotate(Utility.DegToRad((44) + angle * i), Vector3D.AxisZ, centerPoint);

                Line inner_Rotate_Line_Top = new Line(inner_Rotate_Line001.EndPoint, inner_Rotate_Line002.EndPoint);
                */

                Line inner_Rotate_Line1 = new Line(GetSumPoint(centerPoint, circle_Length_4, liner_Offset_Length), GetSumPoint(centerPoint, liner_Length, liner_Offset_Length));
                Line inner_Rotate_Line2 = (Line)inner_Rotate_Line1.Offset(liner_Offset_Length * 2, Vector3D.AxisZ);
                inner_Rotate_Line1.Rotate(Utility.DegToRad((45) + angle * i), Vector3D.AxisZ, centerPoint);
                inner_Rotate_Line2.Rotate(Utility.DegToRad((45) + angle * i), Vector3D.AxisZ, centerPoint);



                outlinesList.AddRange(new Entity[]{
                   //inner_Rotate_Line001, inner_Rotate_Line002, inner_Rotate_Line_Top,
                   inner_Rotate_Line1, inner_Rotate_Line2,
                });
            };


            //Line[] rectangle_min = GetRectangleV3(centerPoint, ,);

            /*
            Point3D rotatePoint = CopyPoint(mPlateTop.EndPoint);
            Line tPlateIntersectLine1 = new Line(CopyPoint(mPlateTop.StartPoint), GetSumPoint(mPlateTop.StartPoint, totalPlateLength, 0));.
            Line tPlateIntersectLine2 = (Line)tPlateIntersectLine1.Offset(-plateWidth, Vector3D.AxisZ);
            tPlateIntersectLine1.Rotate(Utility.DegToRad(-30), Vector3D.AxisZ, rotatePoint);
            */


            outlinesList.AddRange(new Entity[]{

                //바깥네모
                rectangleOuter[0], rectangleOuter[1], rectangleOuter[2], rectangleOuter[3],
                //안팎네모
                rectangleInner[0], rectangleInner[1], rectangleInner[2], rectangleInner[3],
                //중앙원
                center_Circle_1, center_Circle_2, center_Circle_3, center_Circle_4,
                //모서리앵글
                /*
                right_Angle_1[0], right_Angle_1[1], right_Angle_1[2], right_Angle_1[3],
                right_Angle_2[0], right_Angle_2[1], right_Angle_2[2], right_Angle_2[3],
                */

            });

            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

            return outlinesList;
        }

        public List<Entity> GetCenterColumn_Detail_C(Point3D refPoint, double scaleValue)
        {
            List<Entity> outlinesList = new List<Entity>();

            Point3D referencePoint = GetSumPoint(new Point3D(refPoint.X, refPoint.Y), 0, 0);

            double detail_Under_Length = 10;
            double detail_Under_Width = 200;

            double detail_Over_Length = 6.1;
            double detail_Over_Width = 150;

            double arc_Radius = 15;
            double arc_Width = 15;
            double arc_Line_Width = 80;

            double detail_Under_Bottom_Right_slant = 2;

            double detail_Over_Bottom_Right_slant = 1;
            double detail_Over_Right_Top_slant = 3;

            double detail_distance_Length = 3.05;




            Point3D centerPoint = GetSumPoint(referencePoint, 0, 0);

            /*Under Rectangle*/
            Point3D detail_Under_Bottom_L_Point = GetSumPoint(centerPoint, 0, 0);
            Point3D detail_Under_Bottom_R_Point = GetSumPoint(detail_Under_Bottom_L_Point, detail_Under_Length, detail_Under_Bottom_Right_slant);
            Point3D detail_Under_Right_Top_Point = GetSumPoint(detail_Under_Bottom_R_Point, 0, detail_Under_Width);
            Point3D detail_Under_Left_Top_Point = GetSumPoint(detail_Under_Right_Top_Point, -detail_Under_Length, 0);

            //Under Line 바텀 ==========> Line detail_Under_Bottom_Line = new Line(detail_Under_Bottom_L_Point, detail_Under_Bottom_R_Point);
            Line detail_Under_Right_Line = new Line(detail_Under_Bottom_R_Point, detail_Under_Right_Top_Point);
            Line detail_Under_Top_Line = new Line(detail_Under_Right_Top_Point, detail_Under_Left_Top_Point);
            Line detail_Under_Left_Line = new Line(detail_Under_Left_Top_Point, detail_Under_Bottom_L_Point);



            /*Over Contents*/
            //Over Point
            Point3D detail_Over_Up_Fix_Point = GetSumPoint(detail_Under_Right_Top_Point, detail_distance_Length, detail_Over_Width / 2);
            Point3D detail_Over_Down_Fix_Point = GetSumPoint(detail_Under_Right_Top_Point, detail_distance_Length, -detail_Over_Width / 2);
            /*Over Rectangle*/
            Point3D detail_Over_Bottom_L_Point = GetSumPoint(detail_Over_Down_Fix_Point, 0, 0);
            Point3D detail_Over_Bottom_R_Point = GetSumPoint(detail_Over_Bottom_L_Point, detail_Over_Length, 0);
            Point3D detail_Over_Right_Top_Point = GetSumPoint(detail_Over_Bottom_R_Point, 0, detail_Over_Width);
            Point3D detail_Over_Left_Top_Point = GetSumPoint(detail_Over_Up_Fix_Point, 0, 0);

            //Over Line 바텀 ==========> Line detail_Over_Bottom_Line = new Line(detail_Over_Bottom_L_Point, detail_Over_Bottom_R_Point);
            Line detail_Over_Right_Line = new Line(detail_Over_Bottom_R_Point, detail_Over_Right_Top_Point);
            //Line detail_Over_Top_Line = new Line(detail_Over_Right_Top_Point, detail_Over_Left_Top_Point);
            Line detail_Over_Left_Line = new Line(detail_Over_Left_Top_Point, detail_Over_Bottom_L_Point);

            //Arc Line 
            Point3D arc_Under_Point = GetSumPoint(detail_Under_Right_Top_Point, 0, arc_Width);
            Point3D arc_Over_Point = GetSumPoint(arc_Under_Point, 0, arc_Line_Width);

            Point3D arc_First_Point = GetSumPoint(arc_Under_Point, 0, 0);
            Point3D arc_Second_Point = GetSumPoint(detail_Under_Right_Top_Point, -arc_Radius, 0);
            Point3D arc_Third_Point = GetSumPoint(detail_Under_Right_Top_Point, 0, -arc_Width);

            Line arc_Line = new Line(arc_Under_Point, arc_Over_Point);


            Arc arc_Hide_Semicircle = new Arc(arc_First_Point, arc_Second_Point, arc_Third_Point, false);//리스트에는 빼서 보이지않게 하고 밑의 아크만 보이게하기(이 아크는 단순히 인터셉트위드 교차선 용도)
            Point3D[] intersect_Arc_Point = detail_Under_Left_Line.IntersectWith(arc_Hide_Semicircle);
            Arc arc_Semicircle = new Arc(arc_First_Point, arc_Second_Point, intersect_Arc_Point[0], false);

            //Line intersect_Arc_Line = new Line(intersect_Arc_Point[0], detail_Under_Right_Top_Point);



            //Point3D arctest
            //Line testLine = new Line(arc_Under_Point, detail_Under_Right_Top_Point);
            //testLine.Rotate(Utility.DegToRad(138.15), Vector3D.AxisZ, detail_Under_Right_Top_Point);

            //Circle testCircle = new Circle(,);









            outlinesList.AddRange(new Entity[]{

                //detail_Under_Bottom_Line,
                detail_Under_Right_Line, detail_Under_Top_Line, detail_Under_Left_Line,
                //detail_Over_Bottom_Line,
                detail_Over_Right_Line, detail_Over_Left_Line,

                arc_Line, arc_Semicircle,


            });

            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

            return outlinesList;
        }

        public List<Entity> GetCenterColumn_Detail_D(Point3D refPoint, double scaleValue)
        {
            List<Entity> outlinesList = new List<Entity>();



            return outlinesList;
        }


        public List<Entity> GetCenterColumn_EE(Point3D refPoint, double scaleValue)
        {

            List<Entity> outlinesList = new List<Entity>();
            Point3D referencePoint = CopyPoint(refPoint);

            double section_Undefined_Width = 800;
            double section_Seam_Width = 500;
            double detail_Length = 3;
            double detail_over_Right_Length = 6.1;
            double detail_over_Length = 10;

            //under_Bottom
            double under_Bottom_Width = 8;
            double under_Top_Width = 12;
            double under_Bottom_Undefined_Length1 = 750;
            double under_Bottom_Undefined_Length2 = 440;
            double under_Bottom_Undefined_Length3 = 400;
            double under_Bottom_Undefined_Length23 = 40;

            //over_Left
            double over_Bottom_Left_Width = 30;
            double over_Width = 700;
            double over_Seam_Width = over_Width - over_Bottom_Left_Width;
            double over_Length = 320;
            double over_Top_Length = 50;
            double over_Seam_Length = over_Length - over_Top_Length;
            double over_Arc_Radius = 15;
            double over_Seam_Rigth_Top_Width = 200;
            double over_Seam_Right_Top_Arc_Width = over_Seam_Rigth_Top_Width - over_Arc_Radius;
            double over_Chamfer = 20;






            //under_Line
            Point3D under_Bottom_L_Point1 = GetSumPoint(referencePoint, 0, 0);
            Point3D under_Bottom_R_Point1 = GetSumPoint(under_Bottom_L_Point1, under_Bottom_Undefined_Length1, 0);
            Point3D under_Top_L_Point1 = GetSumPoint(under_Bottom_L_Point1, 0, under_Bottom_Width);
            Point3D under_Top_R_Point1 = GetSumPoint(under_Top_L_Point1, under_Bottom_Undefined_Length1, 0);
            Line under_Bottom_Line1 = new Line(under_Bottom_L_Point1, under_Bottom_R_Point1);
            Line under_Top_Line1 = new Line(under_Top_L_Point1, under_Top_R_Point1);

            Point3D under_Bottom_L_Point2 = GetSumPoint(under_Top_L_Point1, under_Bottom_Undefined_Length1 / 4, 0);
            Point3D under_Bottom_R_Point2 = GetSumPoint(under_Bottom_L_Point2, under_Bottom_Undefined_Length2, 0);
            Point3D under_Top_L_Point2 = GetSumPoint(under_Bottom_L_Point2, 0, under_Top_Width);
            Point3D under_Top_R_Point2 = GetSumPoint(under_Top_L_Point2, under_Bottom_Undefined_Length2, 0);
            Line under_Left_Line1 = new Line(under_Bottom_L_Point2, under_Top_L_Point2);
            Line under_Bottom_Line2 = new Line(under_Bottom_L_Point2, under_Bottom_R_Point2);
            Line under_Top_Line2 = new Line(under_Top_L_Point2, under_Top_R_Point2);

            Point3D under_Bottom_L_Point3 = GetSumPoint(under_Top_L_Point2, under_Bottom_Undefined_Length23, 0);
            Point3D under_Bottom_R_Point3 = GetSumPoint(under_Bottom_L_Point3, under_Bottom_Undefined_Length3, 0);
            Point3D under_Top_L_Point3 = GetSumPoint(under_Bottom_L_Point3, 0, under_Top_Width);
            Point3D under_Top_R_Point3 = GetSumPoint(under_Top_L_Point3, under_Bottom_Undefined_Length3, 0);
            Line under_Left_Line2 = new Line(under_Bottom_L_Point3, under_Top_L_Point3);
            Line under_Bottom_Line3 = new Line(under_Bottom_L_Point3, under_Bottom_R_Point3);
            Line under_Top_Line3 = new Line(under_Top_L_Point3, under_Top_R_Point3);

            //over_Line
            Point3D over_Bottom_L_Point1 = GetSumPoint(under_Top_L_Point3, detail_Length, 0);
            Point3D over_Top_L_Point1 = GetSumPoint(over_Bottom_L_Point1, 0, over_Bottom_Left_Width);
            Point3D over_Top_Diagonal = GetSumPoint(over_Top_L_Point1, over_Seam_Length, over_Seam_Width);
            Point3D over_Top_R_Point1 = GetSumPoint(over_Top_Diagonal, over_Top_Length, 0);
            Point3D over_Right_Seam_Point1 = GetSumPoint(over_Top_R_Point1, 0, -over_Seam_Right_Top_Arc_Width);//start
            Point3D over_Right_Seam_Radius_Point = GetSumPoint(over_Right_Seam_Point1, 0, -over_Arc_Radius);//center
            Point3D arc_End_Point = GetSumPoint(over_Right_Seam_Radius_Point, 0, -over_Arc_Radius);//End
            Point3D arc_Mid_Point = GetSumPoint(over_Right_Seam_Radius_Point, -over_Arc_Radius, 0);//Mid
            Point3D over_Left_Seam_Point1 = GetSumPoint(over_Right_Seam_Radius_Point, -detail_over_Length, 0);
            Point3D over_Bottom_L_Point2 = GetSumPoint(over_Left_Seam_Point1, 0, -section_Seam_Width);
            Point3D over_Bottom_R_Point1 = GetSumPoint(over_Bottom_L_Point2, detail_over_Length, 0);
            Point3D over_Right_Top_Point1 = GetSumPoint(over_Bottom_R_Point1, 0, section_Seam_Width);
            Point3D over_Bottom_L_Chamfer_Point = GetSumPoint(over_Bottom_L_Point2, -over_Chamfer, 0);
            Point3D over_Bottom_R_Chamfer_Point = GetSumPoint(over_Bottom_L_Point2, 0, over_Chamfer);
            Point3D over_Bottom_L_Point3 = GetSumPoint(over_Bottom_R_Point1, detail_Length, 0);
            Point3D over_Bottom_R_Point2 = GetSumPoint(over_Bottom_L_Point3, detail_over_Right_Length, 0);
            Point3D over_Top_R_Point2 = GetSumPoint(over_Bottom_R_Point2, 0, section_Undefined_Width);
            Point3D over_Top_L_Point2 = GetSumPoint(over_Bottom_L_Point3, 0, section_Undefined_Width);


            Line over_Bottom_Left_Line1 = new Line(over_Bottom_L_Point1, over_Top_L_Point1);
            Line over_Diagonal_Line = new Line(over_Top_L_Point1, over_Top_Diagonal);
            Line over_Header_Line = new Line(over_Top_Diagonal, over_Top_R_Point1);
            Line over_Right_Line1 = new Line(over_Top_R_Point1, over_Right_Seam_Point1);
            Line over_Seam_Header_Line = new Line(over_Left_Seam_Point1, over_Right_Seam_Radius_Point);
            Line over_Left_Line1 = new Line(over_Left_Seam_Point1, over_Bottom_L_Point2);
            Line over_Bottom_Line1 = new Line(over_Bottom_L_Point2, over_Bottom_R_Point1);
            Line over_Right_Line2 = new Line(over_Bottom_R_Point1, over_Right_Top_Point1);
            Line over_Chafer_Line = new Line(over_Bottom_L_Chamfer_Point, over_Bottom_R_Chamfer_Point);
            Line over_Bottom_Line2 = new Line(over_Bottom_L_Point3, over_Bottom_R_Point2);
            Line over_Right_Line3 = new Line(over_Bottom_R_Point2, over_Top_R_Point2);
            Line over_Left_Line2 = new Line(over_Bottom_L_Point3, over_Top_L_Point2);
            Line over_Header_Line2 = new Line(over_Top_L_Point2, over_Top_R_Point2);

            Arc radius = new Arc(over_Right_Seam_Point1, arc_Mid_Point, arc_End_Point, false);//가상 Radius
            Point3D[] intersect_Arc_Point = over_Left_Line1.IntersectWith(radius);//교차점
            Arc arc_Semi_Radius = new Arc(over_Right_Seam_Point1, arc_Mid_Point, intersect_Arc_Point[0], false);//Arc 적용


            outlinesList.AddRange(new Entity[]{
                //under_Bottom_Line1
                under_Bottom_Line1, under_Top_Line1,
                //under_Bottom_Line2
                under_Left_Line1, under_Bottom_Line2, under_Top_Line2,
                //under_Bottom_Line3
                under_Left_Line2, under_Bottom_Line3, under_Top_Line3,

                //over_Line1
                over_Bottom_Left_Line1, over_Diagonal_Line, over_Header_Line, over_Right_Line1, over_Seam_Header_Line, over_Left_Line1, over_Bottom_Line1, over_Right_Line2, over_Chafer_Line,
                over_Bottom_Line2, over_Right_Line3, over_Left_Line2, over_Header_Line2, 

                //arc
                arc_Semi_Radius,
            });

            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

            return outlinesList;
        }


        public List<Entity> GetCenterColumn_Drain_Detail(Point3D refPoint, double scaleValue)
        {
            List<Entity> EntityList = new List<Entity>();
            List<Entity> outlinesList = new List<Entity>();
            List<Entity> centerlinesList = new List<Entity>();

            Point3D referencePoint = CopyPoint(refPoint);

            double CenterlineLength = 3300;

            double outRadius_1 = 3000;
            double innerRadius_1 = 2850;
            double outRadius_2 = 2600;
            double innerRadius_2 = 2450;

            double angle = 120;
            double degrees90 = 90;
            double degrees5 = 5;
            double division_Angle = 360;
            //double drainHoles_Length = 150;

            Line CenterLineV = new Line(GetSumPoint(referencePoint, 0, CenterlineLength), GetSumPoint(referencePoint, 0, -CenterlineLength));
            Line CenterLineH = new Line(GetSumPoint(referencePoint, CenterlineLength, 0), GetSumPoint(referencePoint, -CenterlineLength, 0));

            //Point3D referencePoint = GetSumPoint(referencePoint, center_Point_Length_Width, center_Point_Length_Width);

            Circle outCircle_1 = new Circle(referencePoint, outRadius_1);
            Circle innerCircle_1 = new Circle(referencePoint, innerRadius_1);

            Circle outCircle_2 = new Circle(referencePoint, outRadius_2);
            Circle innerCircle_2 = new Circle(referencePoint, innerRadius_2);


            for (int i = 0; i < division_Angle; i++)
            {

                //outCircle
                Line out_drain_Line1 = new Line(GetSumPoint(referencePoint, innerRadius_1, 0), GetSumPoint(referencePoint, outRadius_1, 0));
                out_drain_Line1.Rotate(Utility.DegToRad((degrees90 + degrees5) + angle * i), Vector3D.AxisZ, referencePoint);
                Line out_drain_Line2 = new Line(GetSumPoint(referencePoint, innerRadius_1, 0), GetSumPoint(referencePoint, outRadius_1, 0));
                out_drain_Line2.Rotate(Utility.DegToRad((degrees90 - degrees5) + angle * i), Vector3D.AxisZ, referencePoint);


                //innerCircle
                Line inner_drain_Line1 = new Line(GetSumPoint(referencePoint, innerRadius_2, 0), GetSumPoint(referencePoint, outRadius_2, 0));
                inner_drain_Line1.Rotate(Utility.DegToRad((degrees90 + degrees5) + angle * i), Vector3D.AxisZ, referencePoint);
                Line inner_drain_Line2 = new Line(GetSumPoint(referencePoint, innerRadius_2, 0), GetSumPoint(referencePoint, outRadius_2, 0));
                inner_drain_Line2.Rotate(Utility.DegToRad((degrees90 - degrees5) + angle * i), Vector3D.AxisZ, referencePoint);



                //Line newLine = (Line)out_drain_Line1.Offset(50, Vector3D.AxisZ);

                outlinesList.AddRange(new Entity[]{
                    out_drain_Line1, out_drain_Line2,
                    inner_drain_Line1, inner_drain_Line2,
                });


            }



            outlinesList.AddRange(new Entity[]{
                    innerCircle_1, outCircle_1, innerCircle_2, outCircle_2,
            });

            centerlinesList.AddRange(new Entity[]{
                    CenterLineV, CenterLineH
            });

            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref centerlinesList, layerService.LayerCenterLine);

            EntityList.AddRange(outlinesList);
            EntityList.AddRange(centerlinesList);


            return EntityList;
        }

        public List<Entity> GetCenterColumn_BB_Edit(Point3D refPoint, double scaleValue)
        {
            List<Entity> outlinesList = new List<Entity>();
            Point3D referencePoint = CopyPoint(refPoint);
            double startPoint_plus = 50;
            double rectangle_Outer = 800;
            double rectangle_inner = 700;

            double gapAngle = 750;

            double circle_Length_1 = 120;
            double circle_Length_2 = 124;
            double circle_Length_3 = 130;
            double circle_Length_4 = 134;

            double liner_Length = 485;
            double liner_Offset_Length = 10;

            double angle = 90;
            double degrees360 = 360;

            double angleLength = 75;
            double angleThk = 6;
            double angleRound = 5;


            Point3D startPointPlus = GetSumPoint(referencePoint, startPoint_plus, startPoint_plus);
            Point3D centerPoint = GetSumPoint(referencePoint, rectangle_Outer / 2, rectangle_Outer / 2);

            Point3D filletPoint = GetSumPoint(centerPoint, -gapAngle / 2, gapAngle / 2);
            Point3D filletPoint1 = GetSumPoint(centerPoint, -gapAngle / 2, -gapAngle / 2);
            Point3D filletPoint2 = GetSumPoint(centerPoint, gapAngle / 2, -gapAngle / 2);
            Point3D filletPoint3 = GetSumPoint(centerPoint, gapAngle / 2, gapAngle / 2);

            List<Line> rectangleOuter = GetRectangle(referencePoint, rectangle_Outer, rectangle_Outer);
            List<Line> rectangleInner = GetRectangle(startPointPlus, rectangle_inner, rectangle_inner);
            Circle center_Circle_1 = new Circle(centerPoint, circle_Length_1);
            Circle center_Circle_2 = new Circle(centerPoint, circle_Length_2);
            Circle center_Circle_3 = new Circle(centerPoint, circle_Length_3);
            Circle center_Circle_4 = new Circle(centerPoint, circle_Length_4);

            /*Fillet참고*/
            outlinesList.AddRange(GetAngleList(filletPoint, angleLength, angleLength, angleThk, SHAPEDIRECTION.RIGHTTOP, angleRound));
            outlinesList.AddRange(GetAngleList(filletPoint1, angleLength, angleLength, angleThk, SHAPEDIRECTION.RIGHTBOTTOM, angleRound));
            outlinesList.AddRange(GetAngleList(filletPoint2, angleLength, angleLength, angleThk, SHAPEDIRECTION.LEFTBOTTOM, angleRound));
            outlinesList.AddRange(GetAngleList(filletPoint3, angleLength, angleLength, angleThk, SHAPEDIRECTION.LEFTTOP, angleRound));

            //필렛 예시문 private List<Entity> GetAngleList(Point3D refPoint, double width, double length, double thk, SHAPEDIRECTION shapeDirection = SHAPEDIRECTION.LEFTTOP, double radius = 0)



            for (int i = 0; i < degrees360; i++)
            {


                Line inner_Rotate_Line1 = new Line(GetSumPoint(centerPoint, circle_Length_4, liner_Offset_Length), GetSumPoint(centerPoint, liner_Length, liner_Offset_Length));
                Line inner_Rotate_Line2 = (Line)inner_Rotate_Line1.Offset(liner_Offset_Length * 2, Vector3D.AxisZ);
                inner_Rotate_Line1.Rotate(Utility.DegToRad((45) + angle * i), Vector3D.AxisZ, centerPoint);
                inner_Rotate_Line2.Rotate(Utility.DegToRad((45) + angle * i), Vector3D.AxisZ, centerPoint);



                outlinesList.AddRange(new Entity[]{
                   //inner_Rotate_Line001, inner_Rotate_Line002, inner_Rotate_Line_Top,
                   inner_Rotate_Line1, inner_Rotate_Line2,
                });
            };




            outlinesList.AddRange(new Entity[]{

                //바깥네모
                rectangleOuter[0], rectangleOuter[1], rectangleOuter[2], rectangleOuter[3],
                //안팎네모
                rectangleInner[0], rectangleInner[1], rectangleInner[2], rectangleInner[3],
                //중앙원
                center_Circle_1, center_Circle_2, center_Circle_3, center_Circle_4,
                //모서리앵글
                /*
                right_Angle_1[0], right_Angle_1[1], right_Angle_1[2], right_Angle_1[3],
                right_Angle_2[0], right_Angle_2[1], right_Angle_2[2], right_Angle_2[3],
                */

            });

            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

            return outlinesList;

        }

        /// <summary>  -----> 이대리님
        /// /////////////////////////////////////
        /// 


        #endregion

        private Point3D CopyPoint(Point3D selPoint)
        {
            return new Point3D(selPoint.X, selPoint.Y, selPoint.Z);
        }


        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }

        #region Function

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
                eachEntity.Rotate(Utility.DegToRad(angleToRatate), Vector3D.AxisZ, refPoint);
            }

            return angleList;
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
            outlineList.AddRange(doubleLineList);
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
                for (int j = 0; j < countLIst; j++)
                {
                    // 시작각도는 좌우 대칭이 되도록 중앙이 아닌 반절 각도로 시작
                    if (k == 0)
                    {
                        double radian = 0;

                        if (Rotate == 0) { }
                        else if (Rotate == 360) radian = Utility.DegToRad(eachAngel / 2);
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
                // Horizontal startPoint reSet 
                ChamferLine.StartPoint = GetSumPoint(HorizontalLine.StartPoint, ChamferLength, 0);
                // Trim
                if (isTrim) HorizontalLine.StartPoint = CopyPoint(ChamferLine.StartPoint);

                // Vertical EndPoint reSet
                if (cornerIndex == 0)
                {
                    ChamferLine.EndPoint = GetSumPoint(VerticalLine.EndPoint, 0, ChamferLength);

                    // Trim
                    if (isTrim) VerticalLine.EndPoint = CopyPoint(ChamferLine.EndPoint);
                }
                else
                {
                    ChamferLine.EndPoint = GetSumPoint(VerticalLine.StartPoint, 0, -ChamferLength);

                    // Trim
                    if (isTrim) VerticalLine.StartPoint = CopyPoint(ChamferLine.EndPoint);
                }
            }
            else
            { // index 1, 2
                // Horizontal startPoint reSet 
                ChamferLine.StartPoint = GetSumPoint(HorizontalLine.EndPoint, -ChamferLength, 0);
                // Trim
                if (isTrim) HorizontalLine.EndPoint = CopyPoint(ChamferLine.StartPoint);

                // Vertical EndPoint reSet
                if (cornerIndex == 1)
                {
                    ChamferLine.EndPoint = GetSumPoint(VerticalLine.EndPoint, 0, ChamferLength);

                    // Trim
                    if (isTrim) VerticalLine.EndPoint = CopyPoint(ChamferLine.EndPoint);
                }
                else
                {
                    ChamferLine.EndPoint = GetSumPoint(VerticalLine.StartPoint, 0, -ChamferLength);

                    // Trim
                    if (isTrim) VerticalLine.StartPoint = CopyPoint(ChamferLine.EndPoint);
                }
            }



            return ChamferLine;
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




        #endregion


        #region Enum
        public enum RECTANGLUNVIEW { NONE, LEFT, RIGHT, TOP, BOTTOM, }

        public enum VIEWPOSITION { NONE, LEFT, RIGHT, TOP, BOTTOM, TOP_BOTTOM, LEFT_RIGHT, }

        public enum SHAPEDIRECTION { NONE, LEFT, RIGHT, TOP, BOTTOM, LEFTTOP, LEFTBOTTOM, RIGHTTOP, RIGHTBOTTOM, }

        //public enum CENTERRING_TYPE { CRT_INT, CRT_EXT, DRT_INT, DRT_EXT, }

        #endregion
    }
}
