using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawShapeLib.DrawServices;
using DrawWork.Commons;
using DrawWork.DrawModels;
using DrawWork.DrawServices;
using DrawWork.DrawStyleServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawDetailServices
{
    public class DrawDetailStructureShareService
    {

        private StyleFunctionService styleService;
        private LayerStyleService layerService;


        private DrawShapeServices shapeService;


        // Jang
        private DrawCommonService drawComService;


        public DrawDetailStructureShareService()
        {
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();

            shapeService = new DrawShapeServices();

            drawComService = new DrawCommonService();
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



        public List<Entity> DrawDetailRafter(Point3D refPoint, double scaleValue, double rafterLength, double rotateValue )
        {

            List<Entity> outlinesList = new List<Entity>();


            // 좌측 하단
            Point3D workingPoint = GetSumPoint(refPoint, 0, 0);

            //ABC
            double A = rafterLength; //rafter Lenth
            double A1 = 70;  //woking Point to rafter : NOT USED
            double B = 180; //rafter Width
            double B1 = 30;  //centering Point to rafter : NOT USED
            double C = 60;  //rafter to hole : NOT USED
            double C1 = 70;  //hole to hole length gap
            double D = 80;  //hole to hole width gap
            double D1 = 40;  //workingPoint부터 rafter까지의 x축 거리
            double E = 0;   //SHELL ID/4 : NOT USED

            double slotholeHT = 23.0;
            double slotholeWD = 45.0;


            //FIXED??
            double clipAngle = rotateValue;


            //AEMAE
            double t2 = 6;   //Thk of doubleLine
            

            //rafter And Holes
            List<Line> rafter = GetDoubleLineRectangle(GetSumPoint(workingPoint, 0, 0), B, A, t2, RECTANGLUNVIEW.NONE);

            //Left
            List<Entity> leftSlotHoles = GetSlotHoles(GetSumPoint(rafter[3].MidPoint, D1 + C1 / 2, 0), slotholeHT / 2, D, slotholeWD / 2, C1);
            List<Entity> leftHoles = GetHoles(GetSumPoint(rafter[3].MidPoint, D1 + C1 / 2, 0), slotholeHT / 2, C1, D);

            //Right
            List<Entity> rightSlotHoles = GetSlotHoles(GetSumPoint(rafter[1].MidPoint, -(D1 + C1 / 2), 0), slotholeHT / 2, D, slotholeWD / 2, C1);
            List<Entity> rightHoles = GetHoles(GetSumPoint(rafter[1].MidPoint, -(D1 + C1 / 2), 0), slotholeHT / 2, C1, D);


            List<Entity> rafterNslot = new List<Entity>();
            rafterNslot.AddRange(rafter);
            rafterNslot.AddRange(leftSlotHoles);
            rafterNslot.AddRange(leftHoles);
            rafterNslot.AddRange(rightSlotHoles);
            rafterNslot.AddRange(rightHoles);
            foreach (Entity eachEntity in rafterNslot)
            {
                //rater의 회전기준점 : 라프타를 구성하는 사각형의 좌측 상단
                eachEntity.Rotate(clipAngle, Vector3D.AxisZ, GetSumPoint(rafter[2].StartPoint, 0, 0));
            }
            outlinesList.AddRange(rafterNslot);

            outlinesList.AddRange(new Entity[] {


            });


            //styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);
            return outlinesList;

        }



        public List<Entity> DrawDetailRafterSideClipDetail(Point3D refPoint, double drawType = 1)
        {

            List<Entity> outlinesList = new List<Entity>();

            Point3D workingPoint = GetSumPoint(refPoint, 0, 0);

            //ABC
            double A = 200; //rafterWidth
            double A1 = 7;   //roof to clip Distance 
            double B = 48;  //clip To rafter hole center : NOT USED
            double B1 = 30;  //working Point to pad Start Point
            double C = 200; //clipRightWidth
            double C1 = 25;  //pad Start Point to clip start Point
            double D = 55;  //rafter to hole : NOT USED
            double D1 = 70;  //workingPoint부터 rafter까지의 x축 거리
            double E = 90;  //hole to hole width gap
            double E1 = 55;  //slothole to clip right line
            double F = 350; //Pad Width\
            double F1 = 40;  //rafter의 끝에서 첫번째 홀 중심까지의 거리
            double G = 300; //clip left width
            double G1 = 70;  //hole to hole length gap
            double H1 = 50;  //clipBottomLength
            double slotholeHT = 23.0;
            double slotholeWD = 45.0;

            double t = 6;   //현재 있는 값은 t2, t3등도 전부 동일함


            //FIXED??
            double roofSlope = Utility.DegToRad(10); //Angle의 경우 나중에 rad값으로 받기로 함!!
            double value_byD1 = D1 * Math.Tan(roofSlope); //for slothole info : workingPoint부터 rafter까지의 y축 거리

            //AEMAE
            double roofLenth = 600;                  //for loof info : 임의값
            double roofwidth = 7;                    //for loof info : 임의값
            double rafterLength = 236.7;                //for rafter info : 임의값
            double columnWidth = 420;                  //for column info : 임의값

            //for moving roof start point
            double move_x = -6;
            double move_y = 0;

            Point3D topViewWorkingPoint = GetSumPoint(refPoint, 0, 0);
            Point3D frontViewWorkingPoint = GetSumPoint(refPoint, 0, 0);

            Point3D roofStartPoint = GetSumPoint(refPoint, move_x, move_y);
            //roof Line
            List<Line> roof = GetRectangle(GetSumPoint(roofStartPoint, 0, 0), roofwidth, roofLenth, RECTANGLUNVIEW.NONE);
            foreach (Entity eachEntity in roof)
            {
                eachEntity.Rotate(roofSlope, Vector3D.AxisZ, GetSumPoint(roofStartPoint, 0, 0));
            }

            //angle Line
            List<Entity> angle = GetAngleList(GetSumPoint(workingPoint, -t, 0), 75, 75, 7, SHAPEDIRECTION.LEFTTOP, 4);// 앵글 수치는 임의값
            //column Line
            List<Line> column = GetRectangle(GetSumPoint(workingPoint, -t, -10 - columnWidth), columnWidth, t, RECTANGLUNVIEW.NONE);//y축으로 내려오는 10은 앵글이 있는 경우 고정값
            List<Line> pad = GetRectangle(GetSumPoint(workingPoint, 0, -B1 - F), F, t, RECTANGLUNVIEW.NONE);
            //rafter And Holes
            List<Line> rafter = GetDoubleLineRectangle(GetSumPoint(roofStartPoint, D1, -A + value_byD1), A, rafterLength, 10, RECTANGLUNVIEW.NONE); //thk는 임의값
            List<Entity> slotHoles = GetSlotHoles(GetSumPoint(rafter[3].MidPoint, F1 + G1 / 2, 0), slotholeHT / 2, E, slotholeWD / 2, G1);
            List<Entity> holes = GetHoles(GetSumPoint(rafter[3].MidPoint, F1 + G1 / 2, 0), slotholeHT / 2, G1, E);

            List<Entity> slots = new List<Entity>();
            slots.AddRange(slotHoles);
            slots.AddRange(holes);

            foreach (Entity eachEntity in slots)
            {
                //rater의 회전기준점 : 라프타를 구성하는 사각형의 좌측 상단
                eachEntity.Rotate(roofSlope, Vector3D.AxisZ, GetSumPoint(rafter[2].StartPoint, 0, 0));
            }

            foreach (Entity eachEntity in rafter)
            {
                //rater의 회전기준점 : 라프타를 구성하는 사각형의 좌측 상단
                eachEntity.Rotate(roofSlope, Vector3D.AxisZ, GetSumPoint(rafter[2].StartPoint, 0, 0));
            }

            //Using intersect for ClipDistance//
            Circle forClipDistance = new Circle(rafter[2].StartPoint, A1);
            Point3D[] clipSlideStartPoint = rafter[3].IntersectWith(forClipDistance);
            Circle underHole = (Circle)holes[0];
            Line clipRightGuideLine = new Line(GetSumPoint(underHole.Center, E1, 0), GetSumPoint(underHole.Center, E1, 500));

            //clip Line
            Line clipTopLinkLine = new Line(GetSumPoint(workingPoint, t, -B1 - C1), GetSumPoint(clipSlideStartPoint[0], 0, 0));
            //Using intersect//
            Line clipTopslideLineGuide = new Line(GetSumPoint(clipTopLinkLine.EndPoint, 0, 0), GetSumPoint(clipTopLinkLine.EndPoint, 500, 0));
            clipTopslideLineGuide.Rotate(roofSlope, Vector3D.AxisZ, clipTopLinkLine.EndPoint);
            Point3D[] clipSlideEndPoint = clipTopslideLineGuide.IntersectWith(clipRightGuideLine);

            Line clipTopslideLine = new Line(GetSumPoint(clipTopLinkLine.EndPoint, 0, 0), GetSumPoint(clipSlideEndPoint[0], 0, 0));
            Line clipRightLine = new Line(GetSumPoint(clipTopslideLine.EndPoint, 0, 0), GetSumPoint(clipTopslideLine.EndPoint, 0, -C));
            Line clipBottomLine = new Line(GetSumPoint(clipTopLinkLine.StartPoint, 0, -G), GetSumPoint(clipTopLinkLine.StartPoint, H1, -G));
            Line clipBottomslideLine = new Line(GetSumPoint(clipBottomLine.EndPoint, 0, 0), GetSumPoint(clipRightLine.EndPoint, 0, 0));

            //////////////////////CALCULATE DISTANCE for OTHER VIEW////////////////////////
            //////////////////////The order of the holes represents ///////////////////////
            /////////////////the order from the pad based on the TOP VIEW./////////////////
            Circle firstHole = (Circle)holes[2];
            Circle secondHole = (Circle)holes[0];
            Circle thirdHole = (Circle)holes[3];
            Circle fourthHole = (Circle)holes[1];


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
                //outlinesList.AddRange(roof);
                //outlinesList.AddRange(angle);
                //outlinesList.AddRange(column);
                outlinesList.AddRange(pad);
                outlinesList.AddRange(slots);

                
                outlinesList.AddRange(new Entity[] {

                clipTopLinkLine, clipTopslideLine, clipRightLine,
                clipBottomLine, clipBottomslideLine,


            });
            }
            else if (drawType == 2)
            {
                //Top View
                outlinesList.AddRange(padTop);
                outlinesList.AddRange(clipTop);

                outlinesList.AddRange(new Entity[] {

                columnOuterLine, columnInnerLine, HoleCenterLine1, HoleCenterLine2, HoleCenterLine3, HoleCenterLine4,

            });
            }
            else if (drawType == 3)
            {
                //Front View
                outlinesList.AddRange(padFront);
                outlinesList.AddRange(clipFront);

                outlinesList.AddRange(new Entity[] {

                clipFrontStartLine, clipRightEndLine, clipFrontTopLeftLine, clipFrontTopRightLine
            });
            }


            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);
            return outlinesList;

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


        // Centering : Top View
        public DrawEntityModel testDrawCenterRing_TopView(Point3D refPoint, double scaleValue)
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

        #endregion



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


        private List<Entity> GetSlotHoles(Point3D refPoint, double holeRadius, double widthGap, double slotHoleLength, double lengthGap)
        {
            //refPoint는 홀과 홀 사이의 중앙인 점을 입력해주시면됩니다
            //lengthGap=0인경우는 2개, lengthGap에 값이 입력된 경우는 홀이 4개로 그려집니다 :)

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




        #endregion


        #region Enum
        public enum RECTANGLUNVIEW { NONE, LEFT, RIGHT, TOP, BOTTOM, }

        public enum VIEWPOSITION { NONE, LEFT, RIGHT, TOP, BOTTOM, TOP_BOTTOM, LEFT_RIGHT, }

        public enum SHAPEDIRECTION { NONE, LEFT, RIGHT, TOP, BOTTOM, LEFTTOP, LEFTBOTTOM, RIGHTTOP, RIGHTBOTTOM, }

        //public enum CENTERRING_TYPE { CRT_INT, CRT_EXT, DRT_INT, DRT_EXT, }

        #endregion
    }
}
