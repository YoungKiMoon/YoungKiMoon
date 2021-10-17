
using DrawSettingLib.Commons;
using DrawSettingLib.SettingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingServices
{
    public class PaperAreaService
    {

        public PaperAreaService()
        {
        }


        public List<PaperAreaModel> GetPaperAreaData(string selTankType, double selTankOD, string selAnnularString, string selTopAngleType)
        {

            //double selTankOD = 23000;
            //string selTopAngleType = "Detail i";
            bool selAnnular = false;
            if (selAnnularString.Contains("Yes"))
                selAnnular = true;


            List<PaperAreaModel> newAreaList = new List<PaperAreaModel>();

            newAreaList.AddRange(GetPaperAreaDataEtc());
            newAreaList.AddRange(GetPaperAreaBottom01(selTankOD, selAnnular));
            newAreaList.AddRange(GetPaperAreaRoofPlate(selTankType, selTankOD, selTopAngleType));


            return newAreaList;
        }

        #region Etc
        // Grid View Area
        public List<PaperAreaModel> GetPaperAreaDataEtc()
        {

            double viewIDNumber = 1;
            List<PaperAreaModel> newAreaList = new List<PaperAreaModel>();

            PaperAreaModel GAView = new PaperAreaModel();
            GAView.Name = PAPERMAIN_TYPE.GA1;
            GAView.Location.X = 334;
            GAView.Location.Y = 363;
            GAView.Size.X = 600;
            GAView.Size.Y = 400;
            GAView.IsFix = true;
            GAView.ReferencePoint.X = 100000;    //100M
            GAView.ReferencePoint.Y = 100000;    //100M
            newAreaList.Add(GAView);

            PaperAreaModel Orientation = new PaperAreaModel();
            Orientation.Name = PAPERMAIN_TYPE.ORIENTATION;
            Orientation.Location.X = 280;
            Orientation.Location.Y = 297;
            Orientation.Size.X = 500;
            Orientation.Size.Y = 570;
            Orientation.IsFix = true;
            Orientation.ReferencePoint.X = 400000;   //400M
            Orientation.ReferencePoint.Y = 100000;   //100M

            newAreaList.Add(Orientation);


            // Shell plate arrangement : 완료
            PaperAreaModel Detail17 = new PaperAreaModel();
            Detail17.DWGName = PAPERMAIN_TYPE.ShellPlateArrangement;
            Detail17.Page = 2;

            Detail17.Name = PAPERMAIN_TYPE.DETAIL;
            Detail17.SubName = PAPERSUB_TYPE.ShellPlateArrangement;
            Detail17.TitleName = "SHELL PLATE ARRANGEMENT";
            Detail17.TitleSubName = "OUTSIDE VIEW";
            Detail17.IsFix = true;
            Detail17.Row = 1;
            Detail17.RowSpan = 2;
            Detail17.Column = 1;
            Detail17.ColumnSpan = 4;
            Detail17.Priority = 500;
            Detail17.ScaleValue = 0;


            Detail17.ReferencePoint.X = 700000;  // 700M
            Detail17.ReferencePoint.Y = 200000;  // 100M
            Detail17.ModelCenterLocation.X = Detail17.ReferencePoint.X;
            Detail17.ModelCenterLocation.Y = Detail17.ReferencePoint.Y;

            Detail17.viewID = 23000 + viewIDNumber++;
            newAreaList.Add(Detail17);


            // One Course Shell Plate
            PaperAreaModel Detail02 = new PaperAreaModel();
            Detail02.DWGName = PAPERMAIN_TYPE.CourseShellPlate;
            Detail02.Page = 1;

            Detail02.Name = PAPERMAIN_TYPE.DETAIL;
            Detail02.SubName = PAPERSUB_TYPE.ONECOURSESHELLPLATE;
            Detail02.TitleName = "1ST COURSE SHELL PLATE";
            Detail02.TitleSubName = "OUTSIDE VIEW";


            Detail02.Size.X = 598 + 22 + 22;
            Detail02.Size.Y = 185 * 3 + 14;
            Detail02.ScaleValue = 0;// Auto Scale
            Detail02.ReferencePoint.X = 700000;  // 700M
            Detail02.ReferencePoint.Y = 400000;  // 400M

            Detail02.viewID = 12000 + viewIDNumber++;
            newAreaList.Add(Detail02);






            // 2021-08-23
            // Horizontal Joint
            PaperAreaModel Detail01 = new PaperAreaModel();
            Detail01.DWGName = PAPERMAIN_TYPE.ShellPlateArrangement;
            Detail01.Page = 1;

            Detail01.Name = PAPERMAIN_TYPE.DETAIL;
            Detail01.SubName = PAPERSUB_TYPE.HORIZONTALJOINT;
            Detail01.TitleName = "HORIZONTAL JOINT";
            Detail01.TitleSubName = "Test Sub";
            Detail01.IsFix = true;
            Detail01.Row = 1;
            Detail01.Column = 1;
            Detail01.RowSpan = 3;
            Detail01.ScaleValue = 0; // Auto Scale
            Detail01.otherWidth = 100;

            Detail01.ReferencePoint.X = 800000;  // 10000M
            Detail01.ReferencePoint.Y = 100000;  // 100M

            Detail01.viewID = 10000 + viewIDNumber++;
            newAreaList.Add(Detail01);






            // Com Ring
            PaperAreaModel Detail07 = new PaperAreaModel();
            Detail07.DWGName = PAPERMAIN_TYPE.ShellPlateArrangement;
            Detail07.Page = 1;

            Detail07.Name = PAPERMAIN_TYPE.DETAIL;
            Detail07.SubName = PAPERSUB_TYPE.ComRing;
            Detail07.TitleName = "COMP. RING.";
            Detail07.IsFix = false;
            Detail07.Priority = 500;
            Detail07.ScaleValue = 1;

            Detail07.otherWidth = 100;
            Detail07.otherHeight = 40;

            Detail07.ReferencePoint.X = 700000;
            Detail07.ReferencePoint.Y = 100000;
            Detail07.ModelCenterLocation.X = Detail07.ReferencePoint.X;
            Detail07.ModelCenterLocation.Y = Detail07.ReferencePoint.Y;

            Detail07.viewID = 13000 + viewIDNumber++;
            newAreaList.Add(Detail07);

            // Top Ring Cutting Plan
            PaperAreaModel Detail08 = new PaperAreaModel();
            Detail08.DWGName = PAPERMAIN_TYPE.ShellPlateArrangement;
            Detail08.Page = 1;

            Detail08.Name = PAPERMAIN_TYPE.DETAIL;
            Detail08.SubName = PAPERSUB_TYPE.TopRingCuttingPlan;
            Detail08.TitleName = "TOP RING CUTTING PLAN";
            Detail08.TitleSubName = "SCALE : 1/60";
            Detail08.IsFix = false;
            Detail08.Priority = 500;
            Detail08.ScaleValue = 60;

            Detail08.otherWidth = 100;
            Detail08.otherHeight = 40;

            Detail08.ReferencePoint.X = 700000;
            Detail08.ReferencePoint.Y = 120000;
            Detail08.ModelCenterLocation.X = Detail08.ReferencePoint.X;
            Detail08.ModelCenterLocation.Y = Detail08.ReferencePoint.Y;

            Detail08.viewID = 14000 + viewIDNumber++;
            newAreaList.Add(Detail08);


            // Com Ring Cutting Plan
            PaperAreaModel Detail09 = new PaperAreaModel();
            Detail09.DWGName = PAPERMAIN_TYPE.ShellPlateArrangement;
            Detail09.Page = 1;

            Detail09.Name = PAPERMAIN_TYPE.DETAIL;
            Detail09.SubName = PAPERSUB_TYPE.ComRingCuttingPlan;
            Detail09.TitleName = "COMP. RING CUTTING PLAN";
            Detail09.TitleSubName = "SCALE : 1/60";
            Detail09.IsFix = false;
            Detail09.Priority = 500;
            Detail09.ScaleValue = 60;

            Detail09.otherWidth = 100;
            Detail09.otherHeight = 40;

            Detail09.ReferencePoint.X = 700000;
            Detail09.ReferencePoint.Y = 140000;
            Detail09.ModelCenterLocation.X = Detail09.ReferencePoint.X;
            Detail09.ModelCenterLocation.Y = Detail09.ReferencePoint.Y;

            Detail09.viewID = 15000 + viewIDNumber++;
            newAreaList.Add(Detail09);







            // Anchor Chair
            PaperAreaModel Detail10 = new PaperAreaModel();
            Detail10.DWGName = PAPERMAIN_TYPE.ShellPlateArrangement;
            Detail10.Page = 1;

            Detail10.Name = PAPERMAIN_TYPE.DETAIL;
            Detail10.SubName = PAPERSUB_TYPE.AnchorChair;
            Detail10.TitleName = "ANCHOR CHAIR";
            Detail10.TitleSubName = "SCALE : 1/10";
            Detail10.IsFix = false;
            Detail10.Priority = 500;
            Detail10.ScaleValue = 10;

            Detail10.ReferencePoint.X = 820000;
            Detail10.ReferencePoint.Y = 100000;
            Detail10.ModelCenterLocation.X = Detail10.ReferencePoint.X;
            Detail10.ModelCenterLocation.Y = Detail10.ReferencePoint.Y;

            Detail10.viewID = 16000 + viewIDNumber++;
            newAreaList.Add(Detail10);


            // Anchor Chair : Detail 완료 :
            PaperAreaModel Detail11 = new PaperAreaModel();
            Detail11.DWGName = PAPERMAIN_TYPE.ShellPlateArrangement;
            Detail11.Page = 1;

            Detail11.Name = PAPERMAIN_TYPE.DETAIL;
            Detail11.SubName = PAPERSUB_TYPE.AnchorDetail;
            Detail11.TitleName = "ANCHOR B/2N DETAIL";
            Detail11.TitleSubName = "TYPE II";
            Detail11.IsFix = false;
            Detail11.Priority = 500;
            Detail11.ScaleValue = 6;

            Detail11.ReferencePoint.X = 830000;
            Detail11.ReferencePoint.Y = 100000;
            Detail11.ModelCenterLocation.X = Detail11.ReferencePoint.X;
            Detail11.ModelCenterLocation.Y = Detail11.ReferencePoint.Y;

            Detail11.viewID = 17000 + viewIDNumber++;
            newAreaList.Add(Detail11);



            // Name Plate Bracket : 완료
            PaperAreaModel Detail12 = new PaperAreaModel();
            Detail12.DWGName = PAPERMAIN_TYPE.ShellPlateArrangement;
            Detail12.Page = 1;

            Detail12.Name = PAPERMAIN_TYPE.DETAIL;
            Detail12.SubName = PAPERSUB_TYPE.NamePlateBracket;
            Detail12.TitleName = "NAME PLATE BRACKET";
            Detail12.TitleSubName = "SCALE : 1/3";
            Detail12.IsFix = false;
            Detail12.Priority = 500;
            Detail12.ScaleValue = 3;

            Detail12.ReferencePoint.X = 840000;
            Detail12.ReferencePoint.Y = 100000;
            Detail12.ModelCenterLocation.X = Detail12.ReferencePoint.X;
            Detail12.ModelCenterLocation.Y = Detail12.ReferencePoint.Y;

            Detail12.viewID = 18000 + viewIDNumber++;
            newAreaList.Add(Detail12);


            // Earth Lug : 완료
            PaperAreaModel Detail13 = new PaperAreaModel();
            Detail13.DWGName = PAPERMAIN_TYPE.ShellPlateArrangement;
            Detail13.Page = 1;

            Detail13.Name = PAPERMAIN_TYPE.DETAIL;
            Detail13.SubName = PAPERSUB_TYPE.EarthLug;
            Detail13.TitleName = "EARTH LUG";
            Detail13.TitleSubName = "SCALE : 1/3";
            Detail13.IsFix = false;
            Detail13.Priority = 500;
            Detail13.ScaleValue = 3;

            Detail13.ReferencePoint.X = 850000;
            Detail13.ReferencePoint.Y = 100000;
            Detail13.ModelCenterLocation.X = Detail13.ReferencePoint.X;
            Detail13.ModelCenterLocation.Y = Detail13.ReferencePoint.Y;

            Detail13.viewID = 19000 + viewIDNumber++;
            newAreaList.Add(Detail13);



            // Sett Check Piece : 완료
            PaperAreaModel Detail14 = new PaperAreaModel();
            Detail14.DWGName = PAPERMAIN_TYPE.ShellPlateArrangement;
            Detail14.Page = 1;

            Detail14.Name = PAPERMAIN_TYPE.DETAIL;
            Detail14.SubName = PAPERSUB_TYPE.SettlementCheckPiece;
            Detail14.TitleName = "SETTLEMENT CHECK PIECE";
            Detail14.TitleSubName = "SCALE : 1/3";
            Detail14.IsFix = false;
            Detail14.Priority = 500;
            Detail14.ScaleValue = 3;

            Detail14.ReferencePoint.X = 860000;
            Detail14.ReferencePoint.Y = 100000;
            Detail14.ModelCenterLocation.X = Detail14.ReferencePoint.X;
            Detail14.ModelCenterLocation.Y = Detail14.ReferencePoint.Y;

            Detail14.viewID = 20000 + viewIDNumber++;
            newAreaList.Add(Detail14);





            // Top Angle Joint : 완료
            PaperAreaModel Detail15 = new PaperAreaModel();
            Detail15.DWGName = PAPERMAIN_TYPE.ShellPlateArrangement;
            Detail15.Page = 1;

            Detail15.Name = PAPERMAIN_TYPE.DETAIL;
            Detail15.SubName = PAPERSUB_TYPE.TopAngleJoint;
            Detail15.TitleName = "TOP ANGLE JOINT DETAIL";
            Detail15.TitleSubName = "VIERW \"B\"";
            Detail15.IsFix = false;
            Detail15.Priority = 500;
            Detail15.ScaleValue = 1;

            Detail15.ReferencePoint.X = 870000;
            Detail15.ReferencePoint.Y = 100000;
            Detail15.ModelCenterLocation.X = Detail15.ReferencePoint.X;
            Detail15.ModelCenterLocation.Y = Detail15.ReferencePoint.Y;

            Detail15.viewID = 21000 + viewIDNumber++;
            newAreaList.Add(Detail15);



            // Wind Girder Joint : 완료
            PaperAreaModel Detail16 = new PaperAreaModel();
            Detail16.DWGName = PAPERMAIN_TYPE.ShellPlateArrangement;
            Detail16.Page = 1;

            Detail16.Name = PAPERMAIN_TYPE.DETAIL;
            Detail16.SubName = PAPERSUB_TYPE.WindGirderJoint;
            Detail16.TitleName = "WIND GIRDER-1 JOINT DETAIL";
            Detail16.TitleSubName = "VIERW \"C\"";
            Detail16.IsFix = false;
            Detail16.Priority = 500;
            Detail16.ScaleValue = 1;

            Detail16.ReferencePoint.X = 880000;
            Detail16.ReferencePoint.Y = 100000;
            Detail16.ModelCenterLocation.X = Detail16.ReferencePoint.X;
            Detail16.ModelCenterLocation.Y = Detail16.ReferencePoint.Y;

            Detail16.visible = false;

            Detail16.viewID = 22000 + viewIDNumber++;
            newAreaList.Add(Detail16);










            // Dim For Cutting : 완료
            PaperAreaModel Detail18 = new PaperAreaModel();
            Detail18.DWGName = PAPERMAIN_TYPE.ShellPlateArrangement;
            Detail18.Page = 2;

            Detail18.Name = PAPERMAIN_TYPE.DETAIL;
            Detail18.SubName = PAPERSUB_TYPE.DimensionsForCutting;
            Detail18.TitleName = "DIMENSIONS FOR CUTTING";
            Detail18.IsFix = false;
            Detail18.Priority = 500;
            Detail18.ScaleValue = 1;

            Detail18.ReferencePoint.X = 890000;
            Detail18.ReferencePoint.Y = 100000;
            Detail18.ModelCenterLocation.X = Detail18.ReferencePoint.X;
            Detail18.ModelCenterLocation.Y = Detail18.ReferencePoint.Y;

            Detail18.viewID = 24000 + viewIDNumber++;
            newAreaList.Add(Detail18);



            // Tolerance Limit : 완료
            PaperAreaModel Detail19 = new PaperAreaModel();
            Detail19.DWGName = PAPERMAIN_TYPE.ShellPlateArrangement;
            Detail19.Page = 2;

            Detail19.Name = PAPERMAIN_TYPE.DETAIL;
            Detail19.SubName = PAPERSUB_TYPE.ToleranceLimit;
            Detail19.TitleName = "TOLERANCE LIMIT";
            Detail19.TitleSubName = "SHELL PLATE EDGES SHALL BE STRAIGHT WITHIN A TOLERANCE OF ± 1mm.)";
            Detail19.IsFix = false;
            Detail19.Priority = 500;
            Detail19.ScaleValue = 1;

            Detail19.ReferencePoint.X = 900000;
            Detail19.ReferencePoint.Y = 100000;
            Detail19.ModelCenterLocation.X = Detail19.ReferencePoint.X;
            Detail19.ModelCenterLocation.Y = Detail19.ReferencePoint.Y;

            Detail19.viewID = 25000 + viewIDNumber++;
            newAreaList.Add(Detail19);


            // Shell Plate Chord Length : 완료
            PaperAreaModel Detail20 = new PaperAreaModel();
            Detail20.DWGName = PAPERMAIN_TYPE.ShellPlateArrangement;
            Detail20.Page = 2;

            Detail20.Name = PAPERMAIN_TYPE.DETAIL;
            Detail20.SubName = PAPERSUB_TYPE.ShellPlateChordLength;
            Detail20.TitleName = "SHELL PLATE CHORD LENGTH";
            Detail20.TitleSubName = "SECTION ";
            Detail20.IsFix = false;
            Detail20.Priority = 500;
            Detail20.ScaleValue = 1;

            Detail20.ReferencePoint.X = 910000;
            Detail20.ReferencePoint.Y = 100000;
            Detail20.ModelCenterLocation.X = Detail20.ReferencePoint.X;
            Detail20.ModelCenterLocation.Y = Detail20.ReferencePoint.Y;

            Detail20.viewID = 26000 + viewIDNumber++;
            newAreaList.Add(Detail20);




















            // Section DD : 완료
            PaperAreaModel Detail21 = new PaperAreaModel();
            Detail21.DWGName = PAPERMAIN_TYPE.ShellPlateArrangement;
            Detail21.Page = 2;

            Detail21.Name = PAPERMAIN_TYPE.DETAIL;
            Detail21.SubName = PAPERSUB_TYPE.SectionDD;
            Detail21.TitleName = "SECTION \"D\"-\"D\"";
            Detail21.IsFix = false;
            Detail21.Priority = 500;
            Detail21.ScaleValue = 1;

            Detail21.ReferencePoint.X = 900000;
            Detail21.ReferencePoint.Y = 110000;
            Detail21.ModelCenterLocation.X = Detail21.ReferencePoint.X;
            Detail21.ModelCenterLocation.Y = Detail21.ReferencePoint.Y;

            Detail21.viewID = 27000 + viewIDNumber++;
            newAreaList.Add(Detail21);


            // Vert Joint Detail : 완료 
            PaperAreaModel Detail22 = new PaperAreaModel();
            Detail22.DWGName = PAPERMAIN_TYPE.ShellPlateArrangement;
            Detail22.Page = 2;

            Detail22.Name = PAPERMAIN_TYPE.DETAIL;
            Detail22.SubName = PAPERSUB_TYPE.VertJointDetail;
            Detail22.TitleName = "VERT. JOINT DETIL";
            Detail22.IsFix = false;
            Detail22.Priority = 500;
            Detail22.ScaleValue = 1;

            Detail22.ReferencePoint.X = 910000;
            Detail22.ReferencePoint.Y = 110000;
            Detail22.ModelCenterLocation.X = Detail22.ReferencePoint.X;
            Detail22.ModelCenterLocation.Y = Detail22.ReferencePoint.Y;

            Detail22.viewID = 28000 + viewIDNumber++;
            newAreaList.Add(Detail22);




            return newAreaList;
        }

        #endregion


        #region Bottom

        private List<PaperAreaModel> GetPaperAreaBottom01(double selTankOD, bool selAnnular)
        {
            List<PaperAreaModel> newList = new List<PaperAreaModel>();



            if (selAnnular)
            {
                if (selTankOD <= 24800)
                {
                    //Annular Yes, OD <=24800
                    newList.AddRange(GetPaperAreaDataSample01());

                }
                else
                {
                    //Annular Yes, OD < 24800

                    newList.AddRange(GetPaperAreaDataSample02());

                }


            }
            else
            {
                if (selTankOD <= 24800)
                {
                    //Annular No, OD <=24800
                    newList.AddRange(GetPaperAreaDataSample03());

                }
                else
                {
                    //Annular No, OD < 24800
                    newList.AddRange(GetPaperAreaDataSample04());

                }
            }



            return newList;
        }




        public List<PaperAreaModel> GetPaperAreaDataSample01()
        {

            double viewIDNumber = 5000;

            List<PaperAreaModel> newList = new List<PaperAreaModel>();
            PaperAreaModel Detail11 = new PaperAreaModel();
            Detail11.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail11.Page = 1;

            Detail11.Name = PAPERMAIN_TYPE.DETAIL;
            Detail11.SubName = PAPERSUB_TYPE.BottomPlateArrangement;
            Detail11.TitleName = "BOTTOM PLATE ARRANGEMENT";
            Detail11.TitleSubName = "";
            Detail11.IsFix = true;
            Detail11.Row = 1;
            Detail11.Column = 1;
            Detail11.RowSpan = 3;
            Detail11.ColumnSpan = 3;
            Detail11.ScaleValue = 0; // Auto Scale
            Detail11.otherWidth = 420;
            Detail11.otherHeight = 290;

            Detail11.ReferencePoint.X = 400000;
            Detail11.ReferencePoint.Y = 400000;
            Detail11.ModelCenterLocation.X = Detail11.ReferencePoint.X;
            Detail11.ModelCenterLocation.Y = Detail11.ReferencePoint.Y;

            Detail11.viewID = 50000 + viewIDNumber++;
            newList.Add(Detail11);



            // Bottom Plate Joint Detail
            PaperAreaModel Detail12 = new PaperAreaModel();
            Detail12.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail12.Page = 1;

            Detail12.Name = PAPERMAIN_TYPE.DETAIL;
            Detail12.SubName = PAPERSUB_TYPE.BottomPlateJointAnnularDetail;
            Detail12.TitleName = "BOTTOM PLATE JOINT DETAIL";
            Detail12.TitleSubName = "DETAIL \"A\"";
            Detail12.IsFix = true;
            Detail12.Row = 4;
            Detail12.Column = 1;
            Detail12.RowSpan = 1;
            Detail12.ColumnSpan = 2;
            Detail12.Priority = 500;
            Detail12.ScaleValue = 1;

            Detail12.ReferencePoint.X = 920000;
            Detail12.ReferencePoint.Y = 100000;
            Detail12.ModelCenterLocation.X = Detail12.ReferencePoint.X;
            Detail12.ModelCenterLocation.Y = Detail12.ReferencePoint.Y;

            Detail12.viewID = 51000 + viewIDNumber++;
            newList.Add(Detail12);



            // Bottom Plate Welding Detail : C
            PaperAreaModel Detail13 = new PaperAreaModel();
            Detail13.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail13.Page = 1;

            Detail13.Name = PAPERMAIN_TYPE.DETAIL;
            Detail13.SubName = PAPERSUB_TYPE.BottomPlateWeldingDetailC;
            Detail13.TitleName = "BOTTOM PLATE WELDING DETAIL";
            Detail13.TitleSubName = "DETAIL \"C\"";
            Detail13.IsFix = false;
            Detail13.Priority = 500;
            Detail13.ScaleValue = 1;

            Detail13.ReferencePoint.X = 930000;
            Detail13.ReferencePoint.Y = 100000;
            Detail13.ModelCenterLocation.X = Detail13.ReferencePoint.X;
            Detail13.ModelCenterLocation.Y = Detail13.ReferencePoint.Y;

            Detail13.viewID = 52000 + viewIDNumber++;
            newList.Add(Detail13);



            // Bottom Plate Welding Detail : D
            PaperAreaModel Detail14 = new PaperAreaModel();
            Detail14.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail14.Page = 1;

            Detail14.Name = PAPERMAIN_TYPE.DETAIL;
            Detail14.SubName = PAPERSUB_TYPE.BottomPlateWeldingDetailD;
            Detail14.TitleName = "BOTTOM PLATE WELDING DETAIL";
            Detail14.TitleSubName = "DETAIL\"D\"";
            Detail14.IsFix = false;
            Detail14.Priority = 500;
            Detail14.ScaleValue = 1;

            Detail14.ReferencePoint.X = 940000;
            Detail14.ReferencePoint.Y = 100000;
            Detail14.ModelCenterLocation.X = Detail14.ReferencePoint.X;
            Detail14.ModelCenterLocation.Y = Detail14.ReferencePoint.Y;

            Detail14.viewID = 53000 + viewIDNumber++;
            newList.Add(Detail14);



            // Bottom Plate Welding Detail : BB
            PaperAreaModel Detail15 = new PaperAreaModel();
            Detail15.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail15.Page = 1;

            Detail15.Name = PAPERMAIN_TYPE.DETAIL;
            Detail15.SubName = PAPERSUB_TYPE.BottomPlateWeldingDetailBB;
            Detail15.TitleName = "BOTTOM PLATE WELDING DETAIL";
            Detail15.TitleSubName = "DETAIL\"B\"-\"B\"";
            Detail15.IsFix = false;
            Detail15.Priority = 500;
            Detail15.ScaleValue = 1;

            Detail15.ReferencePoint.X = 950000;
            Detail15.ReferencePoint.Y = 100000;
            Detail15.ModelCenterLocation.X = Detail15.ReferencePoint.X;
            Detail15.ModelCenterLocation.Y = Detail15.ReferencePoint.Y;

            Detail15.viewID = 54000 + viewIDNumber++;
            newList.Add(Detail15);



            // Backing Strip Welding Detail
            PaperAreaModel Detail16 = new PaperAreaModel();
            Detail16.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail16.Page = 1;

            Detail16.Name = PAPERMAIN_TYPE.DETAIL;
            Detail16.SubName = PAPERSUB_TYPE.BackingStripWeldingDetail;
            Detail16.TitleName = "BACKING STRIP WELDING DETAIL";
            Detail16.TitleSubName = "DETAIL\"E\"-\"E\"";
            Detail16.IsFix = false;
            Detail16.Priority = 500;
            Detail16.ScaleValue = 2;

            Detail16.ReferencePoint.X = 960000;
            Detail16.ReferencePoint.Y = 80000;
            Detail16.ModelCenterLocation.X = Detail16.ReferencePoint.X;
            Detail16.ModelCenterLocation.Y = Detail16.ReferencePoint.Y;

            Detail16.viewID = 55000 + viewIDNumber++;
            newList.Add(Detail16);



            // Bottom Plate & Shell Joint Detail
            PaperAreaModel Detail17 = new PaperAreaModel();
            Detail17.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail17.Page = 1;

            Detail17.Name = PAPERMAIN_TYPE.DETAIL;
            Detail17.SubName = PAPERSUB_TYPE.BottomPlateShellJointDetail;
            Detail17.TitleName = "BOTTOM PLATE & SHELL JOINT DETAIL";
            Detail17.TitleSubName = "DETAIL F";
            Detail17.IsFix = false;
            Detail17.Priority = 500;
            Detail17.ScaleValue = 1;

            Detail17.ReferencePoint.X = 970000;
            Detail17.ReferencePoint.Y = 100000;
            Detail17.ModelCenterLocation.X = Detail17.ReferencePoint.X;
            Detail17.ModelCenterLocation.Y = Detail17.ReferencePoint.Y;

            Detail17.viewID = 56000 + viewIDNumber++;
            newList.Add(Detail17);



            // Bottom Plate Cutting Plan
            PaperAreaModel Detail18 = new PaperAreaModel();
            Detail18.DWGName = PAPERMAIN_TYPE.BottomPlateCuttingPlan;
            Detail18.Page = 1;

            Detail18.Name = PAPERMAIN_TYPE.DETAIL;
            Detail18.SubName = PAPERSUB_TYPE.BottomPlateCuttingPlan;
            Detail18.TitleName = "BOTTOM PLATE CUTTING PLAN";
            Detail18.TitleSubName = "";
            Detail18.IsFix = false;
            Detail18.Priority = 500;
            Detail18.ScaleValue = 0; //Auto Scale
            Detail18.IsRepeat = true;
            Detail18.otherWidth = 135 - 15; // 135 -> dimension  position 
            Detail18.otherHeight = 50;


            Detail18.ReferencePoint.X = 980000;
            Detail18.ReferencePoint.Y = 100000;
            Detail18.ModelCenterLocation.X = Detail18.ReferencePoint.X;
            Detail18.ModelCenterLocation.Y = Detail18.ReferencePoint.Y;

            Detail18.viewID = 59000 + viewIDNumber++;
            newList.Add(Detail18);




            // Annular Plate Cutting Plan
            PaperAreaModel Detail19 = new PaperAreaModel();
            Detail19.DWGName = PAPERMAIN_TYPE.BottomPlateCuttingPlan;
            Detail19.Page = 1;

            Detail19.Name = PAPERMAIN_TYPE.DETAIL;
            Detail19.SubName = PAPERSUB_TYPE.AnnularPlateCuttingPlan;
            Detail19.TitleName = "ANNULAR PLATE CUTTING PLAN";
            Detail19.TitleSubName = "";
            Detail19.IsFix = false;
            Detail19.Priority = 900;
            Detail19.ScaleValue = 0;
            Detail19.IsRepeat = true;
            Detail19.otherWidth = 135 - 15; // 135 -> dimension  position 
            Detail19.otherHeight = 50;

            Detail19.ReferencePoint.X = 980000;
            Detail19.ReferencePoint.Y = 70000;
            Detail19.ModelCenterLocation.X = Detail19.ReferencePoint.X;
            Detail19.ModelCenterLocation.Y = Detail19.ReferencePoint.Y;

            Detail19.viewID = 58000 + viewIDNumber++;
            newList.Add(Detail19);



            // Baking Strip
            PaperAreaModel Detail20 = new PaperAreaModel();
            Detail20.DWGName = PAPERMAIN_TYPE.BottomPlateCuttingPlan;
            Detail20.Page = 1;

            Detail20.Name = PAPERMAIN_TYPE.DETAIL;
            Detail20.SubName = PAPERSUB_TYPE.BackingStrip;
            Detail20.TitleName = "BACKING STRIP";
            Detail20.TitleSubName = "";
            Detail20.IsFix = false;
            Detail20.Priority = 1001;
            Detail20.ScaleValue = 0;
            Detail20.otherWidth = 130;
            Detail20.otherHeight = 30;

            Detail20.ReferencePoint.X = 960000;
            Detail20.ReferencePoint.Y = 90000;
            Detail20.ModelCenterLocation.X = Detail20.ReferencePoint.X;
            Detail20.ModelCenterLocation.Y = Detail20.ReferencePoint.Y;

            Detail20.viewID = 60000 + viewIDNumber++;
            newList.Add(Detail20);


            return newList;
        }

        public List<PaperAreaModel> GetPaperAreaDataSample02()
        {
            List<PaperAreaModel> newList = new List<PaperAreaModel>();


            double viewIDNumber = 6000;

            // Annular YES, Big PLATE O.D Size 24800 이상

            // 2021-08-27
            // Bottom Plate Arrangement

            PaperAreaModel Detail21 = new PaperAreaModel();
            Detail21.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail21.Page = 1;

            Detail21.Name = PAPERMAIN_TYPE.DETAIL;
            Detail21.SubName = PAPERSUB_TYPE.BottomPlateArrangement;
            Detail21.TitleName = "BOTTOM PLATE ARRANGEMENT";
            Detail21.TitleSubName = "";
            Detail21.IsFix = true;
            Detail21.Row = 1;
            Detail21.Column = 1;
            Detail21.RowSpan = 4;
            Detail21.ColumnSpan = 4;
            Detail21.ScaleValue = 0; // Auto Scale
            Detail21.otherWidth = 570;
            Detail21.otherHeight = 440;

            Detail21.ReferencePoint.X = 400000;
            Detail21.ReferencePoint.Y = 400000;
            Detail21.ModelCenterLocation.X = Detail21.ReferencePoint.X;
            Detail21.ModelCenterLocation.Y = Detail21.ReferencePoint.Y;

            Detail21.viewID = 50000 + viewIDNumber++;
            newList.Add(Detail21);



            // Bottom Plate Joint Detail
            PaperAreaModel Detail22 = new PaperAreaModel();
            Detail22.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail22.Page = 2;

            Detail22.Name = PAPERMAIN_TYPE.DETAIL;
            Detail22.SubName = PAPERSUB_TYPE.BottomPlateJointAnnularDetail;
            Detail22.TitleName = "BOTTOM PLATE JOINT DETAIL";
            Detail22.TitleSubName = "DETAIL \"A\"";
            Detail22.IsFix = true;
            Detail22.Row = 1;
            Detail22.Column = 1;
            Detail22.RowSpan = 1;
            Detail22.ColumnSpan = 2;
            Detail22.Priority = 500;
            Detail22.ScaleValue = 1;

            Detail22.ReferencePoint.X = 920000;
            Detail22.ReferencePoint.Y = 100000;
            Detail22.ModelCenterLocation.X = Detail22.ReferencePoint.X;
            Detail22.ModelCenterLocation.Y = Detail22.ReferencePoint.Y;

            Detail22.viewID = 51000 + viewIDNumber++;
            newList.Add(Detail22);



            // Bottom Plate Welding Detail : C
            PaperAreaModel Detail23 = new PaperAreaModel();
            Detail23.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail23.Page = 2;

            Detail23.Name = PAPERMAIN_TYPE.DETAIL;
            Detail23.SubName = PAPERSUB_TYPE.BottomPlateWeldingDetailC;
            Detail23.TitleName = "BOTTOM PLATE WELDING DETAIL";
            Detail23.TitleSubName = "DETAIL \"C\"";
            Detail23.IsFix = false;
            Detail23.Priority = 500;
            Detail23.ScaleValue = 1;

            Detail23.ReferencePoint.X = 930000;
            Detail23.ReferencePoint.Y = 100000;
            Detail23.ModelCenterLocation.X = Detail23.ReferencePoint.X;
            Detail23.ModelCenterLocation.Y = Detail23.ReferencePoint.Y;

            Detail23.viewID = 52000 + viewIDNumber++;
            newList.Add(Detail23);



            // Bottom Plate Welding Detail : D
            PaperAreaModel Detail24 = new PaperAreaModel();
            Detail24.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail24.Page = 2;

            Detail24.Name = PAPERMAIN_TYPE.DETAIL;
            Detail24.SubName = PAPERSUB_TYPE.BottomPlateWeldingDetailD;
            Detail24.TitleName = "BOTTOM PLATE WELDING DETAIL";
            Detail24.TitleSubName = "DETAIL\"D\"";
            Detail24.IsFix = false;
            Detail24.Priority = 500;
            Detail24.ScaleValue = 1;

            Detail24.ReferencePoint.X = 940000;
            Detail24.ReferencePoint.Y = 100000;
            Detail24.ModelCenterLocation.X = Detail24.ReferencePoint.X;
            Detail24.ModelCenterLocation.Y = Detail24.ReferencePoint.Y;

            Detail24.viewID = 53000 + viewIDNumber++;
            newList.Add(Detail24);



            // Bottom Plate Welding Detail : BB
            PaperAreaModel Detail25 = new PaperAreaModel();
            Detail25.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail25.Page = 2;

            Detail25.Name = PAPERMAIN_TYPE.DETAIL;
            Detail25.SubName = PAPERSUB_TYPE.BottomPlateWeldingDetailBB;
            Detail25.TitleName = "BOTTOM PLATE WELDING DETAIL";
            Detail25.TitleSubName = "DETAIL\"B\"-\"B\"";
            Detail25.IsFix = false;
            Detail25.Priority = 500;
            Detail25.ScaleValue = 1;

            Detail25.ReferencePoint.X = 950000;
            Detail25.ReferencePoint.Y = 100000;
            Detail25.ModelCenterLocation.X = Detail25.ReferencePoint.X;
            Detail25.ModelCenterLocation.Y = Detail25.ReferencePoint.Y;

            Detail25.viewID = 54000 + viewIDNumber++;
            newList.Add(Detail25);



            // Backing Strip Welding Detail
            PaperAreaModel Detail26 = new PaperAreaModel();
            Detail26.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail26.Page = 2;

            Detail26.Name = PAPERMAIN_TYPE.DETAIL;
            Detail26.SubName = PAPERSUB_TYPE.BackingStripWeldingDetail;
            Detail26.TitleName = "BACKING STRIP WELDING DETAIL";
            Detail26.TitleSubName = "DETAIL\"E\"-\"E\"";
            Detail26.IsFix = false;
            Detail26.Priority = 500;
            Detail26.ScaleValue = 2;

            Detail26.ReferencePoint.X = 960000;
            Detail26.ReferencePoint.Y = 100000;
            Detail26.ModelCenterLocation.X = Detail26.ReferencePoint.X;
            Detail26.ModelCenterLocation.Y = Detail26.ReferencePoint.Y;

            Detail26.viewID = 55000 + viewIDNumber++;
            newList.Add(Detail26);



            // Bottom Plate & Shell Joint Detail
            PaperAreaModel Detail27 = new PaperAreaModel();
            Detail27.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail27.Page = 2;

            Detail27.Name = PAPERMAIN_TYPE.DETAIL;
            Detail27.SubName = PAPERSUB_TYPE.BottomPlateShellJointDetail;
            Detail27.TitleName = "BOTTOM PLATE & SHELL JOINT DETAIL";
            Detail27.TitleSubName = "DETAIL F";
            Detail27.IsFix = false;
            Detail27.Priority = 500;
            Detail27.ScaleValue = 1;

            Detail27.ReferencePoint.X = 970000;
            Detail27.ReferencePoint.Y = 100000;
            Detail27.ModelCenterLocation.X = Detail27.ReferencePoint.X;
            Detail27.ModelCenterLocation.Y = Detail27.ReferencePoint.Y;

            Detail27.viewID = 56000 + viewIDNumber++;
            newList.Add(Detail27);




            // Bottom Plate Cutting Plan
            PaperAreaModel Detail28 = new PaperAreaModel();
            Detail28.DWGName = PAPERMAIN_TYPE.BottomPlateCuttingPlan;
            Detail28.Page = 1;

            Detail28.Name = PAPERMAIN_TYPE.DETAIL;
            Detail28.SubName = PAPERSUB_TYPE.BottomPlateCuttingPlan;
            Detail28.TitleName = "BOTTOM PLATE CUTTING PLAN";
            Detail28.TitleSubName = "";
            Detail28.IsFix = false;
            Detail28.Priority = 500;
            Detail28.ScaleValue = 0; //Auto Scale
            Detail28.IsRepeat = true;
            Detail28.otherWidth = 135 - 15; // 135 -> dimension  position 
            Detail28.otherHeight = 50;

            Detail28.ReferencePoint.X = 980000;
            Detail28.ReferencePoint.Y = 100000;
            Detail28.ModelCenterLocation.X = Detail28.ReferencePoint.X;
            Detail28.ModelCenterLocation.Y = Detail28.ReferencePoint.Y;

            Detail28.viewID = 59000 + viewIDNumber++;
            newList.Add(Detail28);




            // Annular Plate Cutting Plan
            PaperAreaModel Detail29 = new PaperAreaModel();
            Detail29.DWGName = PAPERMAIN_TYPE.BottomPlateCuttingPlan;
            Detail29.Page = 1;

            Detail29.Name = PAPERMAIN_TYPE.DETAIL;
            Detail29.SubName = PAPERSUB_TYPE.AnnularPlateCuttingPlan;
            Detail29.TitleName = "ANNULAR PLATE CUTTING PLAN";
            Detail29.TitleSubName = "";
            Detail29.IsFix = false;
            Detail29.Priority = 900;
            Detail29.ScaleValue = 0; // Auto Scale
            Detail29.IsRepeat = true;
            Detail29.otherWidth = 135 - 15; // 135 -> dimension  position 
            Detail29.otherHeight = 50;

            Detail29.ReferencePoint.X = 980000;
            Detail29.ReferencePoint.Y = 70000;
            Detail29.ModelCenterLocation.X = Detail29.ReferencePoint.X;
            Detail29.ModelCenterLocation.Y = Detail29.ReferencePoint.Y;

            Detail29.viewID = 58000 + viewIDNumber++;
            newList.Add(Detail29);



            // Bakin Strip
            PaperAreaModel Detail30 = new PaperAreaModel();
            Detail30.DWGName = PAPERMAIN_TYPE.BottomPlateCuttingPlan;
            Detail30.Page = 1;

            Detail30.Name = PAPERMAIN_TYPE.DETAIL;
            Detail30.SubName = PAPERSUB_TYPE.BackingStrip;
            Detail30.TitleName = "BACKING STRIP";
            Detail30.TitleSubName = "";
            Detail30.IsFix = false;
            Detail30.Priority = 1001;
            Detail30.ScaleValue = 0;
            Detail30.otherWidth = 130;
            Detail30.otherHeight = 30;

            Detail30.ReferencePoint.X = 960000;
            Detail30.ReferencePoint.Y = 90000;
            Detail30.ModelCenterLocation.X = Detail30.ReferencePoint.X;
            Detail30.ModelCenterLocation.Y = Detail30.ReferencePoint.Y;

            Detail30.viewID = 60000 + viewIDNumber++;
            newList.Add(Detail30);



            return newList;
        }

        public List<PaperAreaModel> GetPaperAreaDataSample03()
        {
            List<PaperAreaModel> newList = new List<PaperAreaModel>();

            double viewIDNumber = 7000;


            // Annular NO, PLATE O.D Size 24800 이하


            // 2021-08-27
            // Bottom Plate Arrangement

            PaperAreaModel Detail31 = new PaperAreaModel();
            Detail31.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail31.Page = 1;

            Detail31.Name = PAPERMAIN_TYPE.DETAIL;
            Detail31.SubName = PAPERSUB_TYPE.BottomPlateArrangement;
            Detail31.TitleName = "BOTTOM PLATE ARRANGEMENT";
            Detail31.TitleSubName = "";
            Detail31.IsFix = true;
            Detail31.Row = 1;
            Detail31.Column = 1;
            Detail31.RowSpan = 3;
            Detail31.ColumnSpan = 3;
            Detail31.ScaleValue = 0; // Auto Scale
            Detail31.otherWidth = 420;
            Detail31.otherHeight = 290;


            Detail31.ReferencePoint.X = 400000;
            Detail31.ReferencePoint.Y = 400000;
            Detail31.ModelCenterLocation.X = Detail31.ReferencePoint.X;
            Detail31.ModelCenterLocation.Y = Detail31.ReferencePoint.Y;

            Detail31.viewID = 50000 + viewIDNumber++;
            newList.Add(Detail31);



            // Bottom Plate Joint Detail
            PaperAreaModel Detail32 = new PaperAreaModel();
            Detail32.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail32.Page = 1;

            Detail32.Name = PAPERMAIN_TYPE.DETAIL;
            Detail32.SubName = PAPERSUB_TYPE.BottomPlateJointDetail;
            Detail32.TitleName = "BOTTOM PLATE JOINT DETAIL";
            Detail32.TitleSubName = "DETAIL \"A\"";
            Detail32.IsFix = true;
            Detail32.Row = 4;
            Detail32.Column = 1;
            Detail32.RowSpan = 1;
            Detail32.ColumnSpan = 1;
            Detail32.Priority = 500;
            Detail32.ScaleValue = 1;

            Detail32.ReferencePoint.X = 920000;
            Detail32.ReferencePoint.Y = 100000;
            Detail32.ModelCenterLocation.X = Detail32.ReferencePoint.X;
            Detail32.ModelCenterLocation.Y = Detail32.ReferencePoint.Y;

            Detail32.viewID = 51000 + viewIDNumber++;
            newList.Add(Detail32);



            // Bottom Plate Welding Detail : C
            PaperAreaModel Detail33 = new PaperAreaModel();
            Detail33.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail33.Page = 1;

            Detail33.Name = PAPERMAIN_TYPE.DETAIL;
            Detail33.SubName = PAPERSUB_TYPE.BottomPlateWeldingDetailC;
            Detail33.TitleName = "BOTTOM PLATE WELDING DETAIL";
            Detail33.TitleSubName = "DETAIL \"C\"";
            Detail33.IsFix = false;
            Detail33.Priority = 500;
            Detail33.ScaleValue = 1;

            Detail33.ReferencePoint.X = 930000;
            Detail33.ReferencePoint.Y = 100000;
            Detail33.ModelCenterLocation.X = Detail33.ReferencePoint.X;
            Detail33.ModelCenterLocation.Y = Detail33.ReferencePoint.Y;

            Detail33.viewID = 52000 + viewIDNumber++;
            newList.Add(Detail33);



            // Bottom Plate Welding Detail : D
            PaperAreaModel Detail34 = new PaperAreaModel();
            Detail34.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail34.Page = 1;

            Detail34.Name = PAPERMAIN_TYPE.DETAIL;
            Detail34.SubName = PAPERSUB_TYPE.BottomPlateWeldingDetailD;
            Detail34.TitleName = "BOTTOM PLATE WELDING DETAIL";
            Detail34.TitleSubName = "DETAIL\"D\"";
            Detail34.IsFix = false;
            Detail34.Priority = 500;
            Detail34.ScaleValue = 1;

            Detail34.ReferencePoint.X = 940000;
            Detail34.ReferencePoint.Y = 100000;
            Detail34.ModelCenterLocation.X = Detail34.ReferencePoint.X;
            Detail34.ModelCenterLocation.Y = Detail34.ReferencePoint.Y;

            Detail34.viewID = 53000 + viewIDNumber++;
            newList.Add(Detail34);



            // Bottom Plate & Shell Joint Detail
            PaperAreaModel Detail35 = new PaperAreaModel();
            Detail35.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail35.Page = 1;

            Detail35.Name = PAPERMAIN_TYPE.DETAIL;
            Detail35.SubName = PAPERSUB_TYPE.BottomPlateShellJointDetail;
            Detail35.TitleName = "BOTTOM PLATE & SHELL JOINT DETAIL";
            Detail35.TitleSubName = "DETAIL F";
            Detail35.IsFix = false;
            Detail35.Priority = 500;
            Detail35.ScaleValue = 1;

            Detail35.ReferencePoint.X = 950000;
            Detail35.ReferencePoint.Y = 100000;
            Detail35.ModelCenterLocation.X = Detail35.ReferencePoint.X;
            Detail35.ModelCenterLocation.Y = Detail35.ReferencePoint.Y;

            Detail35.viewID = 56000 + viewIDNumber++;
            newList.Add(Detail35);




            // Bottom Plate Welding Detail
            PaperAreaModel Detail36 = new PaperAreaModel();
            Detail36.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail36.Page = 1;

            Detail36.Name = PAPERMAIN_TYPE.DETAIL;
            Detail36.SubName = PAPERSUB_TYPE.BottomPlateWeldingDetailBB;
            Detail36.TitleName = "BOTTOM PLATE WELDING DETAIL";
            Detail36.TitleSubName = "DETAIL\"B\"-\"B\"";
            Detail36.IsFix = false;
            Detail36.Priority = 500;
            Detail36.ScaleValue = 1;

            Detail36.ReferencePoint.X = 960000;
            Detail36.ReferencePoint.Y = 100000;
            Detail36.ModelCenterLocation.X = Detail36.ReferencePoint.X;
            Detail36.ModelCenterLocation.Y = Detail36.ReferencePoint.Y;

            Detail36.viewID = 54000 + viewIDNumber++;
            newList.Add(Detail36);




            // Bottom Plate Cutting Plan
            PaperAreaModel Detail37 = new PaperAreaModel();
            Detail37.DWGName = PAPERMAIN_TYPE.BottomPlateCuttingPlan;
            Detail37.Page = 1;

            Detail37.Name = PAPERMAIN_TYPE.DETAIL;
            Detail37.SubName = PAPERSUB_TYPE.BottomPlateCuttingPlan;
            Detail37.TitleName = "BOTTOM PLATE CUTTING PLAN";
            Detail37.TitleSubName = "";
            Detail37.IsFix = false;
            Detail37.Priority = 500;
            Detail37.ScaleValue = 0; //Auto Scale
            Detail37.IsRepeat = true;
            Detail37.otherWidth = 135 - 15; // 135 -> dimension  position 
            Detail37.otherHeight = 50;


            Detail37.ReferencePoint.X = 980000;
            Detail37.ReferencePoint.Y = 100000;
            Detail37.ModelCenterLocation.X = Detail37.ReferencePoint.X;
            Detail37.ModelCenterLocation.Y = Detail37.ReferencePoint.Y;

            Detail37.viewID = 59000 + viewIDNumber++;
            newList.Add(Detail37);




            return newList;
        }

        public List<PaperAreaModel> GetPaperAreaDataSample04()
        {
            List<PaperAreaModel> newList = new List<PaperAreaModel>();

            double viewIDNumber = 8000;

            // Annular NO, Big PLATE O.D Size 24800 이상

            // 2021-08-27
            // Bottom Plate Arrangement

            PaperAreaModel Detail38 = new PaperAreaModel();
            Detail38.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail38.Page = 1;

            Detail38.Name = PAPERMAIN_TYPE.DETAIL;
            Detail38.SubName = PAPERSUB_TYPE.BottomPlateArrangement;
            Detail38.TitleName = "BOTTOM PLATE ARRANGEMENT";
            Detail38.TitleSubName = "";
            Detail38.IsFix = true;
            Detail38.Row = 1;
            Detail38.Column = 1;
            Detail38.RowSpan = 4;
            Detail38.ColumnSpan = 4;
            Detail38.ScaleValue = 0; // Auto Scale
            Detail38.otherWidth = 570;
            Detail38.otherHeight = 440;

            Detail38.ReferencePoint.X = 400000;
            Detail38.ReferencePoint.Y = 400000;
            Detail38.ModelCenterLocation.X = Detail38.ReferencePoint.X;
            Detail38.ModelCenterLocation.Y = Detail38.ReferencePoint.Y;

            Detail38.viewID = 50000 + viewIDNumber++;
            newList.Add(Detail38);



            // Bottom Plate Joint Detail
            PaperAreaModel Detail39 = new PaperAreaModel();
            Detail39.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail39.Page = 2;

            Detail39.Name = PAPERMAIN_TYPE.DETAIL;
            Detail39.SubName = PAPERSUB_TYPE.BottomPlateJointDetail;
            Detail39.TitleName = "BOTTOM PLATE JOINT DETAIL";
            Detail39.TitleSubName = "DETAIL \"A\"";
            Detail39.IsFix = true;
            Detail39.Row = 1;
            Detail39.Column = 1;
            Detail39.RowSpan = 1;
            Detail39.ColumnSpan = 1;
            Detail39.Priority = 500;
            Detail39.ScaleValue = 1;

            Detail39.ReferencePoint.X = 920000;
            Detail39.ReferencePoint.Y = 100000;
            Detail39.ModelCenterLocation.X = Detail39.ReferencePoint.X;
            Detail39.ModelCenterLocation.Y = Detail39.ReferencePoint.Y;

            Detail39.viewID = 51000 + viewIDNumber++;
            newList.Add(Detail39);



            // Bottom Plate Welding Detail : C
            PaperAreaModel Detail40 = new PaperAreaModel();
            Detail40.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail40.Page = 2;

            Detail40.Name = PAPERMAIN_TYPE.DETAIL;
            Detail40.SubName = PAPERSUB_TYPE.BottomPlateWeldingDetailC;
            Detail40.TitleName = "BOTTOM PLATE WELDING DETAIL";
            Detail40.TitleSubName = "DETAIL \"C\"";
            Detail40.IsFix = false;
            Detail40.Priority = 500;
            Detail40.ScaleValue = 1;

            Detail40.ReferencePoint.X = 930000;
            Detail40.ReferencePoint.Y = 100000;
            Detail40.ModelCenterLocation.X = Detail40.ReferencePoint.X;
            Detail40.ModelCenterLocation.Y = Detail40.ReferencePoint.Y;

            Detail40.viewID = 52000 + viewIDNumber++;
            newList.Add(Detail40);



            // Bottom Plate Welding Detail : D
            PaperAreaModel Detail41 = new PaperAreaModel();
            Detail41.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail41.Page = 2;

            Detail41.Name = PAPERMAIN_TYPE.DETAIL;
            Detail41.SubName = PAPERSUB_TYPE.BottomPlateWeldingDetailD;
            Detail41.TitleName = "BOTTOM PLATE WELDING DETAIL";
            Detail41.TitleSubName = "DETAIL\"D\"";
            Detail41.IsFix = false;
            Detail41.Priority = 500;
            Detail41.ScaleValue = 1;

            Detail41.ReferencePoint.X = 940000;
            Detail41.ReferencePoint.Y = 100000;
            Detail41.ModelCenterLocation.X = Detail41.ReferencePoint.X;
            Detail41.ModelCenterLocation.Y = Detail41.ReferencePoint.Y;

            Detail41.viewID = 53000 + viewIDNumber++;
            newList.Add(Detail41);



            // Bottom Plate Welding Detail : BB
            PaperAreaModel Detail42 = new PaperAreaModel();
            Detail42.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail42.Page = 2;

            Detail42.Name = PAPERMAIN_TYPE.DETAIL;
            Detail42.SubName = PAPERSUB_TYPE.BottomPlateWeldingDetailBB;
            Detail42.TitleName = "BOTTOM PLATE WELDING DETAIL";
            Detail42.TitleSubName = "DETAIL\"B\"-\"B\"";
            Detail42.IsFix = false;
            Detail42.Priority = 500;
            Detail42.ScaleValue = 1;

            Detail42.ReferencePoint.X = 950000;
            Detail42.ReferencePoint.Y = 100000;
            Detail42.ModelCenterLocation.X = Detail42.ReferencePoint.X;
            Detail42.ModelCenterLocation.Y = Detail42.ReferencePoint.Y;

            Detail42.viewID = 54000 + viewIDNumber++;
            newList.Add(Detail42);



            // Bottom Plate & Shell Joint Detail
            PaperAreaModel Detail43 = new PaperAreaModel();
            Detail43.DWGName = PAPERMAIN_TYPE.BottomPlateArrangement;
            Detail43.Page = 2;

            Detail43.Name = PAPERMAIN_TYPE.DETAIL;
            Detail43.SubName = PAPERSUB_TYPE.BottomPlateShellJointDetail;
            Detail43.TitleName = "BOTTOM PLATE & SHELL JOINT DETAIL";
            Detail43.TitleSubName = "DETAIL F";
            Detail43.IsFix = false;
            Detail43.Priority = 500;
            Detail43.ScaleValue = 1;

            Detail43.ReferencePoint.X = 960000;
            Detail43.ReferencePoint.Y = 100000;
            Detail43.ModelCenterLocation.X = Detail43.ReferencePoint.X;
            Detail43.ModelCenterLocation.Y = Detail43.ReferencePoint.Y;

            Detail43.viewID = 56000 + viewIDNumber++;
            newList.Add(Detail43);




            // Bottom Plate Cutting Plan
            PaperAreaModel Detail44 = new PaperAreaModel();
            Detail44.DWGName = PAPERMAIN_TYPE.BottomPlateCuttingPlan;
            Detail44.Page = 1;

            Detail44.Name = PAPERMAIN_TYPE.DETAIL;
            Detail44.SubName = PAPERSUB_TYPE.BottomPlateCuttingPlan;
            Detail44.TitleName = "BOTTOM PLATE CUTTING PLAN";
            Detail44.TitleSubName = "";
            Detail44.IsFix = false;
            Detail44.Priority = 500;
            Detail44.ScaleValue = 0; //Auto Scale
            Detail44.IsRepeat = true;
            Detail44.otherWidth = 135 - 15; // 135 -> dimension  position 
            Detail44.otherHeight = 50;


            Detail44.ReferencePoint.X = 980000;
            Detail44.ReferencePoint.Y = 100000;
            Detail44.ModelCenterLocation.X = Detail44.ReferencePoint.X;
            Detail44.ModelCenterLocation.Y = Detail44.ReferencePoint.Y;

            Detail44.viewID = 59000 + viewIDNumber++;
            newList.Add(Detail44);



            return newList;
        }

        #endregion


        private List<PaperAreaModel> GetPaperAreaRoofPlate(string selTankType,double selRoofOD, string selTopAngleType)
        {
            List<PaperAreaModel> newList = new List<PaperAreaModel>();
            switch(selTankType.ToLower())
            {
                case "crt":
                    newList.AddRange(GetPaperAreaCRTRoofPlate(selRoofOD, selTopAngleType));
                    break;
                case "drt":
                    newList.AddRange(GetPaperAreaDRTRoofPlate(selRoofOD, selTopAngleType));
                    break;

            }       
            return newList;
        }


        #region CRT Roof
        // ROOF PLATE ARRANGEMENT, CUTTING PLAN
        private List<PaperAreaModel> GetPaperAreaCRTRoofPlate(double selRoofOD, string selTopAngleType)
        {
            List<PaperAreaModel> newList = new List<PaperAreaModel>();

            switch (selTopAngleType)
            {
                case "Detail i":
                    if (selRoofOD <= 24800)
                    {
                        //String I Type, OD <=24800
                        newList.AddRange(GetPaperAreaDataRoofPlate01());

                    }
                    else
                    {
                        //String I Type, OD > 24800

                        newList.AddRange(GetPaperAreaDataRoofPlate02());

                    }
                    break;

                case "Detail b":
                case "Detail d":
                case "Detail e":
                case "Detail k":
                    if (selRoofOD <= 24800)
                    {
                        //String Etc Type, OD <=24800
                        newList.AddRange(GetPaperAreaDataRoofPlate03());

                    }
                    else
                    {
                        //String Etc Type, OD > 24800
                        newList.AddRange(GetPaperAreaDataRoofPlate04());

                    }

                    break;
            }




            return newList;
        }


        // ROOF PLATE 
        public List<PaperAreaModel> GetPaperAreaDataRoofPlate01()  //String I Type, OD <=24800
        {

            double viewIDNumber = 9000;

            //Roof Plate Arrangement

            List<PaperAreaModel> newList = new List<PaperAreaModel>();
            PaperAreaModel Detail50 = new PaperAreaModel();
            Detail50.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail50.Page = 1;

            Detail50.Name = PAPERMAIN_TYPE.DETAIL;
            Detail50.SubName = PAPERSUB_TYPE.RoofPlateArrangement;
            Detail50.TitleName = "ROOF PLATE ARRANGEMENT";
            Detail50.TitleSubName = "";
            Detail50.IsFix = true;
            Detail50.Row = 1;
            Detail50.Column = 1;
            Detail50.RowSpan = 3;
            Detail50.ColumnSpan = 3;
            Detail50.ScaleValue = 0; // Auto Scale
            Detail50.otherWidth = 420;
            Detail50.otherHeight = 290;

            Detail50.ReferencePoint.X = 400000;
            Detail50.ReferencePoint.Y = 700000;
            Detail50.ModelCenterLocation.X = Detail50.ReferencePoint.X;
            Detail50.ModelCenterLocation.Y = Detail50.ReferencePoint.Y;

            Detail50.viewID = 60000 + viewIDNumber++;
            newList.Add(Detail50);



            // Roof Compression Ring JointDetail
            PaperAreaModel Detail51 = new PaperAreaModel();
            Detail51.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail51.Page = 1;

            Detail51.Name = PAPERMAIN_TYPE.DETAIL;
            Detail51.SubName = PAPERSUB_TYPE.RoofCompressionRingJointDetail;
            Detail51.TitleName = "ROOF COMPRESSION RING JOINT DETAIL";
            Detail51.TitleSubName = "DETAIL \"A\"";
            Detail51.IsFix = false;
            Detail51.Priority = 500;
            Detail51.ScaleValue = 1;

            Detail51.ReferencePoint.X = 920000;
            Detail51.ReferencePoint.Y = 110000;
            Detail51.ModelCenterLocation.X = Detail51.ReferencePoint.X;
            Detail51.ModelCenterLocation.Y = Detail51.ReferencePoint.Y;

            Detail51.viewID = 61000 + viewIDNumber++;
            newList.Add(Detail51);



            // Roof Plate Wellding Detail : C
            PaperAreaModel Detail52 = new PaperAreaModel();
            Detail52.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail52.Page = 1;

            Detail52.Name = PAPERMAIN_TYPE.DETAIL;
            Detail52.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailC;
            Detail52.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail52.TitleSubName = "DETAIL \"C\"";
            Detail52.IsFix = false;
            Detail52.Priority = 500;
            Detail52.ScaleValue = 1;

            Detail52.ReferencePoint.X = 930000;
            Detail52.ReferencePoint.Y = 110000;
            Detail52.ModelCenterLocation.X = Detail52.ReferencePoint.X;
            Detail52.ModelCenterLocation.Y = Detail52.ReferencePoint.Y;

            Detail52.viewID = 62000 + viewIDNumber++;
            newList.Add(Detail52);




            // Roof Plate Wellding Detail : D
            PaperAreaModel Detail53 = new PaperAreaModel();
            Detail53.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail53.Page = 1;

            Detail53.Name = PAPERMAIN_TYPE.DETAIL;
            Detail53.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailD;
            Detail53.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail53.TitleSubName = "DETAIL \"D\"";
            Detail53.IsFix = false;
            Detail53.Priority = 500;
            Detail53.ScaleValue = 1;

            Detail53.ReferencePoint.X = 940000;
            Detail53.ReferencePoint.Y = 110000;
            Detail53.ModelCenterLocation.X = Detail53.ReferencePoint.X;
            Detail53.ModelCenterLocation.Y = Detail53.ReferencePoint.Y;

            Detail53.viewID = 63000 + viewIDNumber++;
            newList.Add(Detail53);




            // Roof Compression Wellding Detail
            PaperAreaModel Detail54 = new PaperAreaModel();
            Detail54.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail54.Page = 1;

            Detail54.Name = PAPERMAIN_TYPE.DETAIL;
            Detail54.SubName = PAPERSUB_TYPE.RoofCompressionWeldingDetail;
            Detail54.TitleName = "ROOF COMPRESSION RING WELDING DETAIL";
            Detail54.TitleSubName = "SECTION \"E\"-\"E\"";
            Detail54.IsFix = false;
            Detail54.Priority = 500;
            Detail54.ScaleValue = 1;

            Detail54.ReferencePoint.X = 950000;
            Detail54.ReferencePoint.Y = 110000;
            Detail54.ModelCenterLocation.X = Detail54.ReferencePoint.X;
            Detail54.ModelCenterLocation.Y = Detail54.ReferencePoint.Y;

            Detail54.viewID = 64000 + viewIDNumber++;
            newList.Add(Detail54);



            // Roof Plate Wellding Detail : DD
            PaperAreaModel Detail55 = new PaperAreaModel();
            Detail55.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail55.Page = 1;

            Detail55.Name = PAPERMAIN_TYPE.DETAIL;
            Detail55.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailDD;
            Detail55.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail55.TitleSubName = "SECTION \"D\"-\"D\"";
            Detail55.IsFix = false;
            Detail55.Priority = 500;
            Detail55.ScaleValue = 1;

            Detail55.ReferencePoint.X = 955000;
            Detail55.ReferencePoint.Y = 110000;
            Detail55.ModelCenterLocation.X = Detail55.ReferencePoint.X;
            Detail55.ModelCenterLocation.Y = Detail55.ReferencePoint.Y;

            Detail55.viewID = 65000 + viewIDNumber++;
            newList.Add(Detail55);





            // Roof Plate Cutting Plan
            PaperAreaModel Detail56 = new PaperAreaModel();
            Detail56.DWGName = PAPERMAIN_TYPE.RoofPlateCuttingPlan;
            Detail56.Page = 1;

            Detail56.Name = PAPERMAIN_TYPE.DETAIL;
            Detail56.SubName = PAPERSUB_TYPE.RoofPlateCuttingPlan;
            Detail56.TitleName = "ROOF PLATE CUTTING PLAN";
            Detail56.TitleSubName = "";
            Detail56.IsFix = false;
            Detail56.Priority = 500;
            Detail56.ScaleValue = 0; //Auto Scale
            Detail56.IsRepeat = true;
            Detail56.otherWidth = 135 - 15; // 135 -> dimension position   
            Detail56.otherHeight = 50;


            Detail56.ReferencePoint.X = 1000000;
            Detail56.ReferencePoint.Y = 100000;
            Detail56.ModelCenterLocation.X = Detail56.ReferencePoint.X;
            Detail56.ModelCenterLocation.Y = Detail56.ReferencePoint.Y;

            Detail56.viewID = 66000 + viewIDNumber++;
            newList.Add(Detail56);


            // Commpression Ring Cutting Plan
            PaperAreaModel Detail57 = new PaperAreaModel();
            Detail57.DWGName = PAPERMAIN_TYPE.RoofPlateCuttingPlan;
            Detail57.Page = 1;

            Detail57.Name = PAPERMAIN_TYPE.DETAIL;
            Detail57.SubName = PAPERSUB_TYPE.RoofCompressionRingCuttingPlan;
            Detail57.TitleName = "COMMPRESSION RING CUTTING PLAN";
            Detail57.TitleSubName = "SALE 1/80";//차후 계산값 적용
            Detail57.IsFix = false;
            Detail57.Priority = 900;
            Detail57.ScaleValue = 0; //Auto Scale
            Detail57.IsRepeat = true;
            Detail57.otherWidth = 135 - 15; // 135 -> dimension position   
            Detail57.otherHeight = 50;


            Detail57.ReferencePoint.X = 1000000;
            Detail57.ReferencePoint.Y = 70000;
            Detail57.ModelCenterLocation.X = Detail57.ReferencePoint.X;
            Detail57.ModelCenterLocation.Y = Detail57.ReferencePoint.Y;

            Detail57.viewID = 67000 + viewIDNumber++;
            newList.Add(Detail57);




            return newList;
        }

        public List<PaperAreaModel> GetPaperAreaDataRoofPlate02()  //String I Type, OD > 24800
        {

            double viewIDNumber = 10000;

            //Roof Plate Arrangement

            List<PaperAreaModel> newList = new List<PaperAreaModel>();
            PaperAreaModel Detail60 = new PaperAreaModel();
            Detail60.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail60.Page = 1;

            Detail60.Name = PAPERMAIN_TYPE.DETAIL;
            Detail60.SubName = PAPERSUB_TYPE.RoofPlateArrangement;
            Detail60.TitleName = "ROOF PLATE ARRANGEMENT";
            Detail60.TitleSubName = "";
            Detail60.IsFix = true;
            Detail60.Row = 1;
            Detail60.Column = 1;
            Detail60.RowSpan = 4;
            Detail60.ColumnSpan = 4;
            Detail60.ScaleValue = 0; // Auto Scale
            Detail60.otherWidth = 570;
            Detail60.otherHeight = 440;

            Detail60.ReferencePoint.X = 400000;
            Detail60.ReferencePoint.Y = 700000;
            Detail60.ModelCenterLocation.X = Detail60.ReferencePoint.X;
            Detail60.ModelCenterLocation.Y = Detail60.ReferencePoint.Y;

            Detail60.viewID = 60000 + viewIDNumber++;
            newList.Add(Detail60);



            // Roof Compression Ring JointDetail
            PaperAreaModel Detail61 = new PaperAreaModel();
            Detail61.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail61.Page = 2;

            Detail61.Name = PAPERMAIN_TYPE.DETAIL;
            Detail61.SubName = PAPERSUB_TYPE.RoofCompressionRingJointDetail;
            Detail61.TitleName = "ROOF COMPRESSION RING JOINT DETAIL";
            Detail61.TitleSubName = "DETAIL \"A\"";
            Detail61.IsFix = false;
            Detail61.Priority = 500;
            Detail61.ScaleValue = 1;

            Detail61.ReferencePoint.X = 920000;
            Detail61.ReferencePoint.Y = 110000;
            Detail61.ModelCenterLocation.X = Detail61.ReferencePoint.X;
            Detail61.ModelCenterLocation.Y = Detail61.ReferencePoint.Y;

            Detail61.viewID = 61000 + viewIDNumber++;
            newList.Add(Detail61);



            // Roof Plate Wellding Detail : C
            PaperAreaModel Detail62 = new PaperAreaModel();
            Detail62.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail62.Page = 2;

            Detail62.Name = PAPERMAIN_TYPE.DETAIL;
            Detail62.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailC;
            Detail62.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail62.TitleSubName = "DETAIL \"C\"";
            Detail62.IsFix = false;
            Detail62.Priority = 500;
            Detail62.ScaleValue = 1;

            Detail62.ReferencePoint.X = 930000;
            Detail62.ReferencePoint.Y = 120000;
            Detail62.ModelCenterLocation.X = Detail62.ReferencePoint.X;
            Detail62.ModelCenterLocation.Y = Detail62.ReferencePoint.Y;

            Detail62.viewID = 62000 + viewIDNumber++;
            newList.Add(Detail62);


            // Roof Plate Wellding Detail : D
            PaperAreaModel Detail63 = new PaperAreaModel();
            Detail63.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail63.Page = 2;

            Detail63.Name = PAPERMAIN_TYPE.DETAIL;
            Detail63.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailD;
            Detail63.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail63.TitleSubName = "DETAIL \"D\"";
            Detail63.IsFix = false;
            Detail63.Priority = 500;
            Detail63.ScaleValue = 1;

            Detail63.ReferencePoint.X = 940000;
            Detail63.ReferencePoint.Y = 110000;
            Detail63.ModelCenterLocation.X = Detail63.ReferencePoint.X;
            Detail63.ModelCenterLocation.Y = Detail63.ReferencePoint.Y;

            Detail63.viewID = 63000 + viewIDNumber++;
            newList.Add(Detail63);



            // Roof Compression Wellding Detail
            PaperAreaModel Detail64 = new PaperAreaModel();
            Detail64.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail64.Page = 2;

            Detail64.Name = PAPERMAIN_TYPE.DETAIL;
            Detail64.SubName = PAPERSUB_TYPE.RoofCompressionWeldingDetail;
            Detail64.TitleName = "ROOF COMPRESSION RING WELDING DETAIL";
            Detail64.TitleSubName = "SECTION \"E\"-\"E\"";
            Detail64.IsFix = false;
            Detail64.Priority = 500;
            Detail64.ScaleValue = 1;

            Detail64.ReferencePoint.X = 950000;
            Detail64.ReferencePoint.Y = 110000;
            Detail64.ModelCenterLocation.X = Detail64.ReferencePoint.X;
            Detail64.ModelCenterLocation.Y = Detail64.ReferencePoint.Y;

            Detail64.viewID = 64000 + viewIDNumber++;
            newList.Add(Detail64);



            // Roof Plate Wellding Detail : DD
            PaperAreaModel Detail65 = new PaperAreaModel();
            Detail65.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail65.Page = 2;

            Detail65.Name = PAPERMAIN_TYPE.DETAIL;
            Detail65.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailDD;
            Detail65.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail65.TitleSubName = "SECTION \"D\"-\"D\"";
            Detail65.IsFix = false;
            Detail65.Priority = 500;
            Detail65.ScaleValue = 1;

            Detail65.ReferencePoint.X = 960000;
            Detail65.ReferencePoint.Y = 110000;
            Detail65.ModelCenterLocation.X = Detail65.ReferencePoint.X;
            Detail65.ModelCenterLocation.Y = Detail65.ReferencePoint.Y;

            Detail65.viewID = 65000 + viewIDNumber++;
            newList.Add(Detail65);





            // Roof Plate Cutting Plan
            PaperAreaModel Detail66 = new PaperAreaModel();
            Detail66.DWGName = PAPERMAIN_TYPE.RoofPlateCuttingPlan;
            Detail66.Page = 1;

            Detail66.Name = PAPERMAIN_TYPE.DETAIL;
            Detail66.SubName = PAPERSUB_TYPE.RoofPlateCuttingPlan;
            Detail66.TitleName = "ROOF PLATE CUTTING PLAN";
            Detail66.TitleSubName = "";
            Detail66.IsFix = false;
            Detail66.Priority = 500;
            Detail66.ScaleValue = 0; //Auto Scale
            Detail66.IsRepeat = true;
            Detail66.otherWidth = 135 - 15; // 135 -> dimension position 
            Detail66.otherHeight = 50;


            Detail66.ReferencePoint.X = 1000000;
            Detail66.ReferencePoint.Y = 100000;
            Detail66.ModelCenterLocation.X = Detail66.ReferencePoint.X;
            Detail66.ModelCenterLocation.Y = Detail66.ReferencePoint.Y;

            Detail66.viewID = 66000 + viewIDNumber++;
            newList.Add(Detail66);


            // Commpression Ring Cutting Plan
            PaperAreaModel Detail67 = new PaperAreaModel();
            Detail67.DWGName = PAPERMAIN_TYPE.RoofPlateCuttingPlan;
            Detail67.Page = 1;

            Detail67.Name = PAPERMAIN_TYPE.DETAIL;
            Detail67.SubName = PAPERSUB_TYPE.RoofCompressionRingCuttingPlan;
            Detail67.TitleName = "COMMPRESSION RING CUTTING PLAN";
            Detail67.TitleSubName = "SCALE 1/80";//차후 계산값 적용
            Detail67.IsFix = false;
            Detail67.Priority = 900;
            Detail67.ScaleValue = 0; //Auto Scale
            Detail67.IsRepeat = true;
            Detail67.otherWidth = 135 - 15; // 135 -> dimension position  
            Detail67.otherHeight = 50;


            Detail67.ReferencePoint.X = 1000000;
            Detail67.ReferencePoint.Y = 70000;
            Detail67.ModelCenterLocation.X = Detail67.ReferencePoint.X;
            Detail67.ModelCenterLocation.Y = Detail67.ReferencePoint.Y;

            Detail67.viewID = 67000 + viewIDNumber++;
            newList.Add(Detail67);




            return newList;

        }

        public List<PaperAreaModel> GetPaperAreaDataRoofPlate03()  //String Etc Type, OD <=24800
        {

            double viewIDNumber = 11000;
            //Roof Plate Arrangement

            List<PaperAreaModel> newList = new List<PaperAreaModel>();
            PaperAreaModel Detail70 = new PaperAreaModel();
            Detail70.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail70.Page = 1;

            Detail70.Name = PAPERMAIN_TYPE.DETAIL;
            Detail70.SubName = PAPERSUB_TYPE.RoofPlateArrangement;
            Detail70.TitleName = "ROOF PLATE ARRANGEMENT";
            Detail70.TitleSubName = "";
            Detail70.IsFix = true;
            Detail70.Row = 1;
            Detail70.Column = 1;
            Detail70.RowSpan = 3;
            Detail70.ColumnSpan = 3;
            Detail70.ScaleValue = 0; // Auto Scale
            Detail70.otherWidth = 420;
            Detail70.otherHeight = 290;

            Detail70.ReferencePoint.X = 400000;
            Detail70.ReferencePoint.Y = 700000;
            Detail70.ModelCenterLocation.X = Detail70.ReferencePoint.X;
            Detail70.ModelCenterLocation.Y = Detail70.ReferencePoint.Y;

            Detail70.viewID = 60000 + viewIDNumber++;
            newList.Add(Detail70);



            // Roof Compression Ring JointDetail(타입에 따라 이름 변경!!)
            PaperAreaModel Detail71 = new PaperAreaModel();
            Detail71.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail71.Page = 1;

            Detail71.Name = PAPERMAIN_TYPE.DETAIL;
            Detail71.SubName = PAPERSUB_TYPE.RoofCompressionRingJointDetail;
            Detail71.TitleName = "ROOF COMPRESSION RING JOINT DETAIL";
            Detail71.TitleSubName = "DETAIL \"A\"";
            Detail71.IsFix = false;
            Detail71.Priority = 500;
            Detail71.ScaleValue = 1;

            Detail71.ReferencePoint.X = 920000;
            Detail71.ReferencePoint.Y = 110000;
            Detail71.ModelCenterLocation.X = Detail71.ReferencePoint.X;
            Detail71.ModelCenterLocation.Y = Detail71.ReferencePoint.Y;

            Detail71.viewID = 61000 + viewIDNumber++;
            newList.Add(Detail71);



            // Roof Plate Wellding Detail : C
            PaperAreaModel Detail72 = new PaperAreaModel();
            Detail72.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail72.Page = 1;

            Detail72.Name = PAPERMAIN_TYPE.DETAIL;
            Detail72.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailC;
            Detail72.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail72.TitleSubName = "DETAIL \"C\"";
            Detail72.IsFix = false;
            Detail72.Priority = 500;
            Detail72.ScaleValue = 1;

            Detail72.ReferencePoint.X = 930000;
            Detail72.ReferencePoint.Y = 120000;
            Detail72.ModelCenterLocation.X = Detail72.ReferencePoint.X;
            Detail72.ModelCenterLocation.Y = Detail72.ReferencePoint.Y;

            Detail72.viewID = 62000 + viewIDNumber++;
            newList.Add(Detail72);



            // Roof Plate Wellding Detail : D
            PaperAreaModel Detail73 = new PaperAreaModel();
            Detail73.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail73.Page = 1;

            Detail73.Name = PAPERMAIN_TYPE.DETAIL;
            Detail73.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailD;
            Detail73.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail73.TitleSubName = "DETAIL \"D\"";
            Detail73.IsFix = false;
            Detail73.Priority = 500;
            Detail73.ScaleValue = 1;

            Detail73.ReferencePoint.X = 940000;
            Detail73.ReferencePoint.Y = 110000;
            Detail73.ModelCenterLocation.X = Detail73.ReferencePoint.X;
            Detail73.ModelCenterLocation.Y = Detail73.ReferencePoint.Y;

            Detail73.viewID = 63000 + viewIDNumber++;
            newList.Add(Detail73);



            // Roof Plate Wellding Detail : DD
            PaperAreaModel Detail74 = new PaperAreaModel();
            Detail74.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail74.Page = 1;

            Detail74.Name = PAPERMAIN_TYPE.DETAIL;
            Detail74.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailDD;
            Detail74.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail74.TitleSubName = "SECTION \"D\"-\"D\"";
            Detail74.IsFix = false;
            Detail74.Priority = 500;
            Detail74.ScaleValue = 1;

            Detail74.ReferencePoint.X = 950000;
            Detail74.ReferencePoint.Y = 110000;
            Detail74.ModelCenterLocation.X = Detail74.ReferencePoint.X;
            Detail74.ModelCenterLocation.Y = Detail74.ReferencePoint.Y;

            Detail74.viewID = 64000 + viewIDNumber++;
            newList.Add(Detail74);



            // Roof Plate Cutting Plan
            PaperAreaModel Detail75 = new PaperAreaModel();
            Detail75.DWGName = PAPERMAIN_TYPE.RoofPlateCuttingPlan;
            Detail75.Page = 1;

            Detail75.Name = PAPERMAIN_TYPE.DETAIL;
            Detail75.SubName = PAPERSUB_TYPE.RoofPlateCuttingPlan;
            Detail75.TitleName = "ROOF PLATE CUTTING PLAN";
            Detail75.TitleSubName = "";
            Detail75.IsFix = false;
            Detail75.Priority = 500;
            Detail75.ScaleValue = 0; //Auto Scale
            Detail75.IsRepeat = true;
            Detail75.otherWidth = 135 - 15; // 135 -> dimension  position 
            Detail75.otherHeight = 50;


            Detail75.ReferencePoint.X = 1000000;
            Detail75.ReferencePoint.Y = 100000;
            Detail75.ModelCenterLocation.X = Detail75.ReferencePoint.X;
            Detail75.ModelCenterLocation.Y = Detail75.ReferencePoint.Y;

            Detail75.viewID = 65000 + viewIDNumber++;
            newList.Add(Detail75);


            return newList;

        }

        public List<PaperAreaModel> GetPaperAreaDataRoofPlate04()  //String Etc Type, OD > 24800
        {

            double viewIDNumber = 12000;

            //Roof Plate Arrangement

            List<PaperAreaModel> newList = new List<PaperAreaModel>();
            PaperAreaModel Detail80 = new PaperAreaModel();
            Detail80.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail80.Page = 1;

            Detail80.Name = PAPERMAIN_TYPE.DETAIL;
            Detail80.SubName = PAPERSUB_TYPE.RoofPlateArrangement;
            Detail80.TitleName = "ROOF PLATE ARRANGEMENT";
            Detail80.TitleSubName = "";
            Detail80.IsFix = true;
            Detail80.Row = 1;
            Detail80.Column = 1;
            Detail80.RowSpan = 4;
            Detail80.ColumnSpan = 4;
            Detail80.ScaleValue = 0; // Auto Scale
            Detail80.otherWidth = 570;
            Detail80.otherHeight = 440;

            Detail80.ReferencePoint.X = 400000;
            Detail80.ReferencePoint.Y = 700000;
            Detail80.ModelCenterLocation.X = Detail80.ReferencePoint.X;
            Detail80.ModelCenterLocation.Y = Detail80.ReferencePoint.Y;

            Detail80.viewID = 60000 + viewIDNumber++;
            newList.Add(Detail80);



            // Roof Compression Ring JointDetail(타입에 따라 이름 변경!!)
            PaperAreaModel Detail81 = new PaperAreaModel();
            Detail81.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail81.Page = 2;

            Detail81.Name = PAPERMAIN_TYPE.DETAIL;
            Detail81.SubName = PAPERSUB_TYPE.RoofCompressionRingJointDetail;
            Detail81.TitleName = "ROOF COMPRESSION RING JOINT DETAIL";
            Detail81.TitleSubName = "DETAIL \"A\"";
            Detail81.IsFix = false;
            Detail81.Priority = 500;
            Detail81.ScaleValue = 1;

            Detail81.ReferencePoint.X = 920000;
            Detail81.ReferencePoint.Y = 110000;
            Detail81.ModelCenterLocation.X = Detail81.ReferencePoint.X;
            Detail81.ModelCenterLocation.Y = Detail81.ReferencePoint.Y;

            Detail81.viewID = 61000 + viewIDNumber++;
            newList.Add(Detail81);



            // Roof Plate Wellding Detail : C
            PaperAreaModel Detail82 = new PaperAreaModel();
            Detail82.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail82.Page = 2;

            Detail82.Name = PAPERMAIN_TYPE.DETAIL;
            Detail82.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailC;
            Detail82.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail82.TitleSubName = "DETAIL \"C\"";
            Detail82.IsFix = false;
            Detail82.Priority = 500;
            Detail82.ScaleValue = 1;

            Detail82.ReferencePoint.X = 930000;
            Detail82.ReferencePoint.Y = 120000;
            Detail82.ModelCenterLocation.X = Detail82.ReferencePoint.X;
            Detail82.ModelCenterLocation.Y = Detail82.ReferencePoint.Y;

            Detail82.viewID = 62000 + viewIDNumber++;
            newList.Add(Detail82);



            // Roof Plate Wellding Detail : D
            PaperAreaModel Detail83 = new PaperAreaModel();
            Detail83.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail83.Page = 2;

            Detail83.Name = PAPERMAIN_TYPE.DETAIL;
            Detail83.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailD;
            Detail83.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail83.TitleSubName = "DETAIL \"D\"";
            Detail83.IsFix = false;
            Detail83.Priority = 500;
            Detail83.ScaleValue = 1;

            Detail83.ReferencePoint.X = 940000;
            Detail83.ReferencePoint.Y = 110000;
            Detail83.ModelCenterLocation.X = Detail83.ReferencePoint.X;
            Detail83.ModelCenterLocation.Y = Detail83.ReferencePoint.Y;

            Detail83.viewID = 63000 + viewIDNumber++;
            newList.Add(Detail83);



            // Roof Plate Wellding Detail : DD
            PaperAreaModel Detail84 = new PaperAreaModel();
            Detail84.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail84.Page = 2;

            Detail84.Name = PAPERMAIN_TYPE.DETAIL;
            Detail84.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailDD;
            Detail84.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail84.TitleSubName = "SECTION \"D\"-\"D\"";
            Detail84.IsFix = false;
            Detail84.Priority = 500;
            Detail84.ScaleValue = 1;

            Detail84.ReferencePoint.X = 950000;
            Detail84.ReferencePoint.Y = 110000;
            Detail84.ModelCenterLocation.X = Detail84.ReferencePoint.X;
            Detail84.ModelCenterLocation.Y = Detail84.ReferencePoint.Y;

            Detail84.viewID = 64000 + viewIDNumber++;
            newList.Add(Detail84);





            // Roof Plate Cutting Plan
            PaperAreaModel Detail85 = new PaperAreaModel();
            Detail85.DWGName = PAPERMAIN_TYPE.RoofPlateCuttingPlan;
            Detail85.Page = 1;

            Detail85.Name = PAPERMAIN_TYPE.DETAIL;
            Detail85.SubName = PAPERSUB_TYPE.RoofPlateCuttingPlan;
            Detail85.TitleName = "ROOF PLATE CUTTING PLAN";
            Detail85.TitleSubName = "";
            Detail85.IsFix = false;
            Detail85.Priority = 500;
            Detail85.ScaleValue = 0; //Auto Scale
            Detail85.IsRepeat = true;
            Detail85.otherWidth = 135 - 15; // 135 -> dimension position 
            Detail85.otherHeight = 50;


            Detail85.ReferencePoint.X = 1000000;
            Detail85.ReferencePoint.Y = 100000;
            Detail85.ModelCenterLocation.X = Detail85.ReferencePoint.X;
            Detail85.ModelCenterLocation.Y = Detail85.ReferencePoint.Y;

            Detail85.viewID = 65000 + viewIDNumber++;
            newList.Add(Detail85);




            return newList;

        }



        #endregion



        #region DRT Roof

        // ROOF PLATE ARRANGEMENT, CUTTING PLAN
        private List<PaperAreaModel> GetPaperAreaDRTRoofPlate(double selRoofOD, string selTopAngleType)
        {
            List<PaperAreaModel> newList = new List<PaperAreaModel>();

            switch (selTopAngleType)
            {
                case "Detail i":
                    if (selRoofOD <= 24800)
                    {
                        //String I Type, OD <=24800
                        newList.AddRange(GetPaperAreaDataDRTRoofPlate01());

                    }
                    else
                    {
                        //String I Type, OD > 24800

                        newList.AddRange(GetPaperAreaDataDRTRoofPlate02());

                    }
                    break;

                case "Detail b":
                case "Detail d":
                case "Detail e":
                case "Detail k":
                    if (selRoofOD <= 24800)
                    {
                        //String Etc Type, OD <=24800
                        newList.AddRange(GetPaperAreaDataDRTRoofPlate03());

                    }
                    else
                    {
                        //String Etc Type, OD > 24800
                        newList.AddRange(GetPaperAreaDataDRTRoofPlate04());

                    }

                    break;
            }




            return newList;
        }


        // ROOF PLATE 
        public List<PaperAreaModel> GetPaperAreaDataDRTRoofPlate01()  //String I Type, OD <=24800
        {

            double viewIDNumber = 13000;

            //Roof Plate Arrangement

            List<PaperAreaModel> newList = new List<PaperAreaModel>();
            PaperAreaModel Detail100 = new PaperAreaModel();
            Detail100.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail100.Page = 1;

            Detail100.Name = PAPERMAIN_TYPE.DETAIL;
            Detail100.SubName = PAPERSUB_TYPE.RoofPlateArrangement;
            Detail100.TitleName = "ROOF PLATE ARRANGEMENT";
            Detail100.TitleSubName = "";
            Detail100.IsFix = true;
            Detail100.Row = 1;
            Detail100.Column = 1;
            Detail100.RowSpan = 3;
            Detail100.ColumnSpan = 3;
            Detail100.ScaleValue = 0; // Auto Scale
            Detail100.otherWidth = 420;
            Detail100.otherHeight = 290;

            Detail100.ReferencePoint.X = 400000;
            Detail100.ReferencePoint.Y = 800000;
            Detail100.ModelCenterLocation.X = Detail100.ReferencePoint.X;
            Detail100.ModelCenterLocation.Y = Detail100.ReferencePoint.Y;

            Detail100.viewID = 70000 + viewIDNumber++;
            newList.Add(Detail100);



            // Roof Compression Ring JointDetail
            PaperAreaModel Detail101 = new PaperAreaModel();
            Detail101.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail101.Page = 1;

            Detail101.Name = PAPERMAIN_TYPE.DETAIL;
            Detail101.SubName = PAPERSUB_TYPE.RoofCompressionRingJointDetail;
            Detail101.TitleName = "ROOF COMPRESSION RING JOINT DETAIL";
            Detail101.TitleSubName = "DETAIL \"A\"";
            Detail101.IsFix = false;
            Detail101.Priority = 500;
            Detail101.ScaleValue = 1;

            Detail101.ReferencePoint.X = 920000;
            Detail101.ReferencePoint.Y = 130000;
            Detail101.ModelCenterLocation.X = Detail101.ReferencePoint.X;
            Detail101.ModelCenterLocation.Y = Detail101.ReferencePoint.Y;

            Detail101.viewID = 71000 + viewIDNumber++;
            newList.Add(Detail101);



            // Roof Plate Wellding Detail : C
            PaperAreaModel Detail102 = new PaperAreaModel();
            Detail102.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail102.Page = 1;

            Detail102.Name = PAPERMAIN_TYPE.DETAIL;
            Detail102.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailC;
            Detail102.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail102.TitleSubName = "DETAIL \"C\"";
            Detail102.IsFix = false;
            Detail102.Priority = 500;
            Detail102.ScaleValue = 1;

            Detail102.ReferencePoint.X = 930000;
            Detail102.ReferencePoint.Y = 130000;
            Detail102.ModelCenterLocation.X = Detail102.ReferencePoint.X;
            Detail102.ModelCenterLocation.Y = Detail102.ReferencePoint.Y;

            Detail102.viewID = 72000 + viewIDNumber++;
            newList.Add(Detail102);



            // Roof Plate Wellding Detail : D
            PaperAreaModel Detail103 = new PaperAreaModel();
            Detail103.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail103.Page = 1;

            Detail103.Name = PAPERMAIN_TYPE.DETAIL;
            Detail103.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailD;
            Detail103.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail103.TitleSubName = "DETAIL \"D\"";
            Detail103.IsFix = false;
            Detail103.Priority = 500;
            Detail103.ScaleValue = 1;

            Detail103.ReferencePoint.X = 940000;
            Detail103.ReferencePoint.Y = 130000;
            Detail103.ModelCenterLocation.X = Detail103.ReferencePoint.X;
            Detail103.ModelCenterLocation.Y = Detail103.ReferencePoint.Y;

            Detail103.viewID = 73000 + viewIDNumber++;
            newList.Add(Detail103);



            // Roof Compression Wellding Detail
            PaperAreaModel Detail104 = new PaperAreaModel();
            Detail104.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail104.Page = 1;

            Detail104.Name = PAPERMAIN_TYPE.DETAIL;
            Detail104.SubName = PAPERSUB_TYPE.RoofCompressionWeldingDetail;
            Detail104.TitleName = "ROOF COMPRESSION RING WELDING DETAIL";
            Detail104.TitleSubName = "SECTION \"E\"-\"E\"";
            Detail104.IsFix = false;
            Detail104.Priority = 500;
            Detail104.ScaleValue = 1;

            Detail104.ReferencePoint.X = 950000;
            Detail104.ReferencePoint.Y = 130000;
            Detail104.ModelCenterLocation.X = Detail104.ReferencePoint.X;
            Detail104.ModelCenterLocation.Y = Detail104.ReferencePoint.Y;

            Detail104.viewID = 74000 + viewIDNumber++;
            newList.Add(Detail104);



            // Roof Plate Wellding Detail : DD
            PaperAreaModel Detail105 = new PaperAreaModel();
            Detail105.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail105.Page = 1;

            Detail105.Name = PAPERMAIN_TYPE.DETAIL;
            Detail105.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailDD;
            Detail105.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail105.TitleSubName = "SECTION \"D\"-\"D\"";
            Detail105.IsFix = false;
            Detail105.Priority = 500;
            Detail105.ScaleValue = 1;

            Detail105.ReferencePoint.X = 960000;
            Detail105.ReferencePoint.Y = 130000;
            Detail105.ModelCenterLocation.X = Detail105.ReferencePoint.X;
            Detail105.ModelCenterLocation.Y = Detail105.ReferencePoint.Y;

            Detail105.viewID = 75000 + viewIDNumber++;
            newList.Add(Detail105);





            // Roof Plate Cutting Plan
            PaperAreaModel Detail106 = new PaperAreaModel();
            Detail106.DWGName = PAPERMAIN_TYPE.RoofPlateCuttingPlan;
            Detail106.Page = 1;

            Detail106.Name = PAPERMAIN_TYPE.DETAIL;
            Detail106.SubName = PAPERSUB_TYPE.RoofPlateCuttingPlan;
            Detail106.TitleName = "ROOF PLATE CUTTING PLAN";
            Detail106.TitleSubName = "";
            Detail106.IsFix = false;
            Detail106.Priority = 500;
            Detail106.ScaleValue = 0; //Auto Scale
            Detail106.IsRepeat = true;
            Detail106.otherWidth = 135 - 15; // 135 -> dimension position
            Detail106.otherHeight = 50;


            Detail106.ReferencePoint.X = 1000000;
            Detail106.ReferencePoint.Y = 100000;
            Detail106.ModelCenterLocation.X = Detail106.ReferencePoint.X;
            Detail106.ModelCenterLocation.Y = Detail106.ReferencePoint.Y;

            Detail106.viewID = 76000 + viewIDNumber++;
            newList.Add(Detail106);


            // Commpression Ring Cutting Plan
            PaperAreaModel Detail107 = new PaperAreaModel();
            Detail107.DWGName = PAPERMAIN_TYPE.RoofPlateCuttingPlan;
            Detail107.Page = 1;

            Detail107.Name = PAPERMAIN_TYPE.DETAIL;
            Detail107.SubName = PAPERSUB_TYPE.RoofCompressionRingCuttingPlan;
            Detail107.TitleName = "COMMPRESSION RING CUTTING PLAN";
            Detail107.TitleSubName = "SALE 1/80"; //차후 계산값 적용
            Detail107.IsFix = false;
            Detail107.Priority = 900;
            Detail107.ScaleValue = 0; //Auto Scale
            Detail107.IsRepeat = true;
            Detail107.otherWidth = 135 - 15; // 135 -> dimension position
            Detail107.otherHeight = 50;


            Detail107.ReferencePoint.X = 1000000;
            Detail107.ReferencePoint.Y = 70000;
            Detail107.ModelCenterLocation.X = Detail107.ReferencePoint.X;
            Detail107.ModelCenterLocation.Y = Detail107.ReferencePoint.Y;

            Detail107.viewID = 78000 + viewIDNumber++;
            newList.Add(Detail107);




            return newList;
        }

        public List<PaperAreaModel> GetPaperAreaDataDRTRoofPlate02()  //String I Type, OD > 24800
        {

            double viewIDNumber = 14000;

            //Roof Plate Arrangement

            List<PaperAreaModel> newList = new List<PaperAreaModel>();
            PaperAreaModel Detail110 = new PaperAreaModel();
            Detail110.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail110.Page = 1;

            Detail110.Name = PAPERMAIN_TYPE.DETAIL;
            Detail110.SubName = PAPERSUB_TYPE.RoofPlateArrangement;
            Detail110.TitleName = "ROOF PLATE ARRANGEMENT";
            Detail110.TitleSubName = "";
            Detail110.IsFix = true;
            Detail110.Row = 1;
            Detail110.Column = 1;
            Detail110.RowSpan = 4;
            Detail110.ColumnSpan = 4;
            Detail110.ScaleValue = 0; // Auto Scale
            Detail110.otherWidth = 570;
            Detail110.otherHeight = 440;

            Detail110.ReferencePoint.X = 400000;
            Detail110.ReferencePoint.Y = 800000;
            Detail110.ModelCenterLocation.X = Detail110.ReferencePoint.X;
            Detail110.ModelCenterLocation.Y = Detail110.ReferencePoint.Y;

            Detail110.viewID = 80000 + viewIDNumber++;
            newList.Add(Detail110);



            // Roof Compression Ring JointDetail
            PaperAreaModel Detail111 = new PaperAreaModel();
            Detail111.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail111.Page = 2;

            Detail111.Name = PAPERMAIN_TYPE.DETAIL;
            Detail111.SubName = PAPERSUB_TYPE.RoofCompressionRingJointDetail;
            Detail111.TitleName = "ROOF COMPRESSION RING JOINT DETAIL";
            Detail111.TitleSubName = "DETAIL \"A\"";
            Detail111.IsFix = false;
            Detail111.Priority = 500;
            Detail111.ScaleValue = 1;

            Detail111.ReferencePoint.X = 920000;
            Detail111.ReferencePoint.Y = 130000;
            Detail111.ModelCenterLocation.X = Detail111.ReferencePoint.X;
            Detail111.ModelCenterLocation.Y = Detail111.ReferencePoint.Y;

            Detail111.viewID = 81000 + viewIDNumber++;
            newList.Add(Detail111);




            // Roof Plate Wellding Detail : C
            PaperAreaModel Detail112 = new PaperAreaModel();
            Detail112.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail112.Page = 2;

            Detail112.Name = PAPERMAIN_TYPE.DETAIL;
            Detail112.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailC;
            Detail112.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail112.TitleSubName = "DETAIL \"C\"";
            Detail112.IsFix = false;
            Detail112.Priority = 500;
            Detail112.ScaleValue = 1;

            Detail112.ReferencePoint.X = 930000;
            Detail112.ReferencePoint.Y = 130000;
            Detail112.ModelCenterLocation.X = Detail112.ReferencePoint.X;
            Detail112.ModelCenterLocation.Y = Detail112.ReferencePoint.Y;

            Detail112.viewID = 82000 + viewIDNumber++;
            newList.Add(Detail112);




            // Roof Plate Wellding Detail : D
            PaperAreaModel Detail113 = new PaperAreaModel();
            Detail113.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail113.Page = 2;

            Detail113.Name = PAPERMAIN_TYPE.DETAIL;
            Detail113.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailD;
            Detail113.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail113.TitleSubName = "DETAIL \"D\"";
            Detail113.IsFix = false;
            Detail113.Priority = 500;
            Detail113.ScaleValue = 1;

            Detail113.ReferencePoint.X = 940000;
            Detail113.ReferencePoint.Y = 130000;
            Detail113.ModelCenterLocation.X = Detail113.ReferencePoint.X;
            Detail113.ModelCenterLocation.Y = Detail113.ReferencePoint.Y;

            Detail113.viewID = 83000 + viewIDNumber++;
            newList.Add(Detail113);



            // Roof Compression Wellding Detail
            PaperAreaModel Detail114 = new PaperAreaModel();
            Detail114.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail114.Page = 2;

            Detail114.Name = PAPERMAIN_TYPE.DETAIL;
            Detail114.SubName = PAPERSUB_TYPE.RoofCompressionWeldingDetail;
            Detail114.TitleName = "ROOF COMPRESSION RING WELDING DETAIL";
            Detail114.TitleSubName = "SECTION \"E\"-\"E\"";
            Detail114.IsFix = false;
            Detail114.Priority = 500;
            Detail114.ScaleValue = 1;

            Detail114.ReferencePoint.X = 950000;
            Detail114.ReferencePoint.Y = 130000;
            Detail114.ModelCenterLocation.X = Detail114.ReferencePoint.X;
            Detail114.ModelCenterLocation.Y = Detail114.ReferencePoint.Y;

            Detail114.viewID = 84000 + viewIDNumber++;
            newList.Add(Detail114);



            // Roof Plate Wellding Detail : DD
            PaperAreaModel Detail115 = new PaperAreaModel();
            Detail115.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail115.Page = 2;

            Detail115.Name = PAPERMAIN_TYPE.DETAIL;
            Detail115.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailDD;
            Detail115.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail115.TitleSubName = "SECTION \"D\"-\"D\"";
            Detail115.IsFix = false;
            Detail115.Priority = 500;
            Detail115.ScaleValue = 1;

            Detail115.ReferencePoint.X = 960000;
            Detail115.ReferencePoint.Y = 130000;
            Detail115.ModelCenterLocation.X = Detail115.ReferencePoint.X;
            Detail115.ModelCenterLocation.Y = Detail115.ReferencePoint.Y;

            Detail115.viewID = 85000 + viewIDNumber++;
            newList.Add(Detail115);





            // Roof Plate Cutting Plan
            PaperAreaModel Detail116 = new PaperAreaModel();
            Detail116.DWGName = PAPERMAIN_TYPE.RoofPlateCuttingPlan;
            Detail116.Page = 1;

            Detail116.Name = PAPERMAIN_TYPE.DETAIL;
            Detail116.SubName = PAPERSUB_TYPE.RoofPlateCuttingPlan;
            Detail116.TitleName = "ROOF PLATE CUTTING PLAN";
            Detail116.TitleSubName = "";
            Detail116.IsFix = false;
            Detail116.Priority = 500;
            Detail116.ScaleValue = 0; //Auto Scale
            Detail116.IsRepeat = true;
            Detail116.otherWidth = 135 - 15; // 135 -> dimension position
            Detail116.otherHeight = 50;


            Detail116.ReferencePoint.X = 1000000;
            Detail116.ReferencePoint.Y = 100000;
            Detail116.ModelCenterLocation.X = Detail116.ReferencePoint.X;
            Detail116.ModelCenterLocation.Y = Detail116.ReferencePoint.Y;

            Detail116.viewID = 86000 + viewIDNumber++;
            newList.Add(Detail116);


            // Commpression Ring Cutting Plan
            PaperAreaModel Detail117 = new PaperAreaModel();
            Detail117.DWGName = PAPERMAIN_TYPE.RoofPlateCuttingPlan;
            Detail117.Page = 1;

            Detail117.Name = PAPERMAIN_TYPE.DETAIL;
            Detail117.SubName = PAPERSUB_TYPE.RoofCompressionRingCuttingPlan;
            Detail117.TitleName = "COMMPRESSION RING CUTTING PLAN";
            Detail117.TitleSubName = "SALE 1/80";//차후 계산값 적용
            Detail117.IsFix = false;
            Detail117.Priority = 900;
            Detail117.ScaleValue = 0; //Auto Scale
            Detail117.IsRepeat = true;
            Detail117.otherWidth = 135 - 15; // 135 -> dimension position
            Detail117.otherHeight = 50;


            Detail117.ReferencePoint.X = 1000000;
            Detail117.ReferencePoint.Y = 70000;
            Detail117.ModelCenterLocation.X = Detail117.ReferencePoint.X;
            Detail117.ModelCenterLocation.Y = Detail117.ReferencePoint.Y;

            Detail117.viewID = 87000 + viewIDNumber++;
            newList.Add(Detail117);




            return newList;

        }

        public List<PaperAreaModel> GetPaperAreaDataDRTRoofPlate03()  //String Etc Type, OD <=24800
        {

            double viewIDNumber = 15000;
            //Roof Plate Arrangement

            List<PaperAreaModel> newList = new List<PaperAreaModel>();
            PaperAreaModel Detail120 = new PaperAreaModel();
            Detail120.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail120.Page = 1;

            Detail120.Name = PAPERMAIN_TYPE.DETAIL;
            Detail120.SubName = PAPERSUB_TYPE.RoofPlateArrangement;
            Detail120.TitleName = "ROOF PLATE ARRANGEMENT";
            Detail120.TitleSubName = "";
            Detail120.IsFix = true;
            Detail120.Row = 1;
            Detail120.Column = 1;
            Detail120.RowSpan = 3;
            Detail120.ColumnSpan = 3;
            Detail120.ScaleValue = 0; // Auto Scale
            Detail120.otherWidth = 420;
            Detail120.otherHeight = 290;

            Detail120.ReferencePoint.X = 400000;
            Detail120.ReferencePoint.Y = 800000;
            Detail120.ModelCenterLocation.X = Detail120.ReferencePoint.X;
            Detail120.ModelCenterLocation.Y = Detail120.ReferencePoint.Y;

            Detail120.viewID = 90000 + viewIDNumber++;
            newList.Add(Detail120);



            // Roof Compression Ring JointDetail(타입에 따라 이름 변경!!)
            PaperAreaModel Detail121 = new PaperAreaModel();
            Detail121.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail121.Page = 1;

            Detail121.Name = PAPERMAIN_TYPE.DETAIL;
            Detail121.SubName = PAPERSUB_TYPE.RoofCompressionRingJointDetail;
            Detail121.TitleName = "ROOF COMPRESSION RING JOINT DETAIL";
            Detail121.TitleSubName = "DETAIL \"A\"";
            Detail121.IsFix = false;
            Detail121.Priority = 500;
            Detail121.ScaleValue = 1;

            Detail121.ReferencePoint.X = 920000;
            Detail121.ReferencePoint.Y = 130000;
            Detail121.ModelCenterLocation.X = Detail121.ReferencePoint.X;
            Detail121.ModelCenterLocation.Y = Detail121.ReferencePoint.Y;

            Detail121.viewID = 91000 + viewIDNumber++;
            newList.Add(Detail121);



            // Roof Plate Wellding Detail : C
            PaperAreaModel Detail122 = new PaperAreaModel();
            Detail122.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail122.Page = 1;

            Detail122.Name = PAPERMAIN_TYPE.DETAIL;
            Detail122.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailC;
            Detail122.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail122.TitleSubName = "DETAIL \"C\"";
            Detail122.IsFix = false;
            Detail122.Priority = 500;
            Detail122.ScaleValue = 1;

            Detail122.ReferencePoint.X = 930000;
            Detail122.ReferencePoint.Y = 130000;
            Detail122.ModelCenterLocation.X = Detail122.ReferencePoint.X;
            Detail122.ModelCenterLocation.Y = Detail122.ReferencePoint.Y;

            Detail122.viewID = 92000 + viewIDNumber++;
            newList.Add(Detail122);



            // Roof Plate Wellding Detail : D
            PaperAreaModel Detail123 = new PaperAreaModel();
            Detail123.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail123.Page = 1;

            Detail123.Name = PAPERMAIN_TYPE.DETAIL;
            Detail123.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailD;
            Detail123.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail123.TitleSubName = "DETAIL \"D\"";
            Detail123.IsFix = false;
            Detail123.Priority = 500;
            Detail123.ScaleValue = 1;

            Detail123.ReferencePoint.X = 940000;
            Detail123.ReferencePoint.Y = 130000;
            Detail123.ModelCenterLocation.X = Detail123.ReferencePoint.X;
            Detail123.ModelCenterLocation.Y = Detail123.ReferencePoint.Y;

            Detail123.viewID = 93000 + viewIDNumber++;
            newList.Add(Detail123);



            // Roof Plate Wellding Detail : DD
            PaperAreaModel Detail124 = new PaperAreaModel();
            Detail124.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail124.Page = 1;

            Detail124.Name = PAPERMAIN_TYPE.DETAIL;
            Detail124.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailDD;
            Detail124.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail124.TitleSubName = "SECTION \"D\"-\"D\"";
            Detail124.IsFix = false;
            Detail124.Priority = 500;
            Detail124.ScaleValue = 1;

            Detail124.ReferencePoint.X = 950000;
            Detail124.ReferencePoint.Y = 130000;
            Detail124.ModelCenterLocation.X = Detail124.ReferencePoint.X;
            Detail124.ModelCenterLocation.Y = Detail124.ReferencePoint.Y;

            Detail124.viewID = 94000 + viewIDNumber++;
            newList.Add(Detail124);





            // Roof Plate Cutting Plan
            PaperAreaModel Detail125 = new PaperAreaModel();
            Detail125.DWGName = PAPERMAIN_TYPE.RoofPlateCuttingPlan;
            Detail125.Page = 1;

            Detail125.Name = PAPERMAIN_TYPE.DETAIL;
            Detail125.SubName = PAPERSUB_TYPE.RoofPlateCuttingPlan;
            Detail125.TitleName = "ROOF PLATE CUTTING PLAN";
            Detail125.TitleSubName = "";
            Detail125.IsFix = false;
            Detail125.Priority = 500;
            Detail125.ScaleValue = 0; //Auto Scale
            Detail125.IsRepeat = true;
            Detail125.otherWidth = 135 - 15; // 135 -> dimension position
            Detail125.otherHeight = 50;


            Detail125.ReferencePoint.X = 1000000;
            Detail125.ReferencePoint.Y = 100000;
            Detail125.ModelCenterLocation.X = Detail125.ReferencePoint.X;
            Detail125.ModelCenterLocation.Y = Detail125.ReferencePoint.Y;

            Detail125.viewID = 95000 + viewIDNumber++;
            newList.Add(Detail125);


            return newList;

        }

        public List<PaperAreaModel> GetPaperAreaDataDRTRoofPlate04()  //String Etc Type, OD > 24800
        {

            double viewIDNumber = 16000;

            //Roof Plate Arrangement

            List<PaperAreaModel> newList = new List<PaperAreaModel>();
            PaperAreaModel Detail130 = new PaperAreaModel();
            Detail130.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail130.Page = 1;

            Detail130.Name = PAPERMAIN_TYPE.DETAIL;
            Detail130.SubName = PAPERSUB_TYPE.RoofPlateArrangement;
            Detail130.TitleName = "ROOF PLATE ARRANGEMENT";
            Detail130.TitleSubName = "";
            Detail130.IsFix = true;
            Detail130.Row = 1;
            Detail130.Column = 1;
            Detail130.RowSpan = 4;
            Detail130.ColumnSpan = 4;
            Detail130.ScaleValue = 0; // Auto Scale
            Detail130.otherWidth = 570;
            Detail130.otherHeight = 440;

            Detail130.ReferencePoint.X = 400000;
            Detail130.ReferencePoint.Y = 800000;
            Detail130.ModelCenterLocation.X = Detail130.ReferencePoint.X;
            Detail130.ModelCenterLocation.Y = Detail130.ReferencePoint.Y;

            Detail130.viewID = 100000 + viewIDNumber++;
            newList.Add(Detail130);



            // Roof Compression Ring JointDetail(타입에 따라 이름 변경!!)
            PaperAreaModel Detail131 = new PaperAreaModel();
            Detail131.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail131.Page = 2;

            Detail131.Name = PAPERMAIN_TYPE.DETAIL;
            Detail131.SubName = PAPERSUB_TYPE.RoofCompressionRingJointDetail;
            Detail131.TitleName = "ROOF COMPRESSION RING JOINT DETAIL";
            Detail131.TitleSubName = "DETAIL \"A\"";
            Detail131.IsFix = false;
            Detail131.Priority = 500;
            Detail131.ScaleValue = 1;

            Detail131.ReferencePoint.X = 920000;
            Detail131.ReferencePoint.Y = 130000;
            Detail131.ModelCenterLocation.X = Detail131.ReferencePoint.X;
            Detail131.ModelCenterLocation.Y = Detail131.ReferencePoint.Y;

            Detail131.viewID = 101000 + viewIDNumber++;
            newList.Add(Detail131);




            // Roof Plate Wellding Detail : C
            PaperAreaModel Detail132 = new PaperAreaModel();
            Detail132.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail132.Page = 2;

            Detail132.Name = PAPERMAIN_TYPE.DETAIL;
            Detail132.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailC;
            Detail132.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail132.TitleSubName = "DETAIL \"C\"";
            Detail132.IsFix = false;
            Detail132.Priority = 500;
            Detail132.ScaleValue = 1;

            Detail132.ReferencePoint.X = 930000;
            Detail132.ReferencePoint.Y = 130000;
            Detail132.ModelCenterLocation.X = Detail132.ReferencePoint.X;
            Detail132.ModelCenterLocation.Y = Detail132.ReferencePoint.Y;

            Detail132.viewID = 102000 + viewIDNumber++;
            newList.Add(Detail132);



            // Roof Plate Wellding Detail : D
            PaperAreaModel Detail133 = new PaperAreaModel();
            Detail133.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail133.Page = 2;

            Detail133.Name = PAPERMAIN_TYPE.DETAIL;
            Detail133.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailD;
            Detail133.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail133.TitleSubName = "DETAIL \"D\"";
            Detail133.IsFix = false;
            Detail133.Priority = 500;
            Detail133.ScaleValue = 1;

            Detail133.ReferencePoint.X = 940000;
            Detail133.ReferencePoint.Y = 130000;
            Detail133.ModelCenterLocation.X = Detail133.ReferencePoint.X;
            Detail133.ModelCenterLocation.Y = Detail133.ReferencePoint.Y;

            Detail133.viewID = 103000 + viewIDNumber++;
            newList.Add(Detail133);



            // Roof Plate Wellding Detail : DD
            PaperAreaModel Detail134 = new PaperAreaModel();
            Detail134.DWGName = PAPERMAIN_TYPE.RoofPlateArrangement;
            Detail134.Page = 2;

            Detail134.Name = PAPERMAIN_TYPE.DETAIL;
            Detail134.SubName = PAPERSUB_TYPE.RoofPlateWeldingDetailDD;
            Detail134.TitleName = "ROOF PLATE WELDING DETAIL";
            Detail134.TitleSubName = "SECTION \"D\"-\"D\"";
            Detail134.IsFix = false;
            Detail134.Priority = 500;
            Detail134.ScaleValue = 1;

            Detail134.ReferencePoint.X = 950000;
            Detail134.ReferencePoint.Y = 130000;
            Detail134.ModelCenterLocation.X = Detail134.ReferencePoint.X;
            Detail134.ModelCenterLocation.Y = Detail134.ReferencePoint.Y;

            Detail134.viewID = 104000 + viewIDNumber++;
            newList.Add(Detail134);





            // Roof Plate Cutting Plan
            PaperAreaModel Detail135 = new PaperAreaModel();
            Detail135.DWGName = PAPERMAIN_TYPE.RoofPlateCuttingPlan;
            Detail135.Page = 1;

            Detail135.Name = PAPERMAIN_TYPE.DETAIL;
            Detail135.SubName = PAPERSUB_TYPE.RoofPlateCuttingPlan;
            Detail135.TitleName = "ROOF PLATE CUTTING PLAN";
            Detail135.TitleSubName = "";
            Detail135.IsFix = false;
            Detail135.Priority = 500;
            Detail135.ScaleValue = 0; //Auto Scale
            Detail135.IsRepeat = true;
            Detail135.otherWidth = 135 - 15; // 135 -> dimension position
            Detail135.otherHeight = 50;


            Detail135.ReferencePoint.X = 1000000;
            Detail135.ReferencePoint.Y = 100000;
            Detail135.ModelCenterLocation.X = Detail135.ReferencePoint.X;
            Detail135.ModelCenterLocation.Y = Detail135.ReferencePoint.Y;

            Detail135.viewID = 105000 + viewIDNumber++;
            newList.Add(Detail135);




            return newList;

        }



        #endregion





        public PaperAreaModel GetPaperAreaModel(PAPERMAIN_TYPE mainName, PAPERSUB_TYPE subName, List<PaperAreaModel> selList)
        {
            PaperAreaModel returnModel = new PaperAreaModel();


            // Paper
            foreach (PaperAreaModel eachArea in selList)
            {
                if (eachArea.Name == mainName)
                    if (eachArea.SubName == subName)
                    {
                        returnModel = eachArea;
                        break;
                    }
            }

            return returnModel;
        }

        public double GetPaperScaleValue(PAPERMAIN_TYPE mainName, PAPERSUB_TYPE subName, List<PaperAreaModel> selList)
        {
            double returnScale = 1;


            // Paper
            foreach (PaperAreaModel eachArea in selList)
            {
                if (eachArea.Name == mainName)
                    if (eachArea.SubName == subName)
                    {
                        returnScale = eachArea.ScaleValue;
                        break;
                    }
            }

            return returnScale;
        }





        // Type Name
        public PAPERMAIN_TYPE GetPaperMainType(string selName)
        {
            PAPERMAIN_TYPE returnValue = PAPERMAIN_TYPE.NotSet;
            switch (selName)
            {
                case "ga":
                    returnValue = PAPERMAIN_TYPE.GA1;
                    break;
                case "orientation":
                    returnValue = PAPERMAIN_TYPE.ORIENTATION;
                    break;
                case "detail":
                    returnValue = PAPERMAIN_TYPE.DETAIL;
                    break;
            }
            return returnValue;
        }

        public PAPERSUB_TYPE GetPaperSubType(string selName)
        {
            PAPERSUB_TYPE returnValue = PAPERSUB_TYPE.NotSet;
            switch (selName)
            {
                case "horizontaljoint":
                    returnValue = PAPERSUB_TYPE.HORIZONTALJOINT;
                    break;


                case "comring":
                    returnValue = PAPERSUB_TYPE.ComRing;
                    break;
                case "topringcuttingplan":
                    returnValue = PAPERSUB_TYPE.TopRingCuttingPlan;
                    break;
                case "comringcuttingplan":
                    returnValue = PAPERSUB_TYPE.ComRingCuttingPlan;
                    break;


                case "anchorchair":
                    returnValue = PAPERSUB_TYPE.AnchorChair;
                    break;
                case "anchordetail":
                    returnValue = PAPERSUB_TYPE.AnchorDetail;
                    break;


                case "topanglejoint":
                    returnValue = PAPERSUB_TYPE.TopAngleJoint;
                    break;
                case "windgirderjoint":
                    returnValue = PAPERSUB_TYPE.WindGirderJoint;
                    break;


                case "sectiondd":
                    returnValue = PAPERSUB_TYPE.SectionDD;
                    break;


                case "vertjointdetail":
                    returnValue = PAPERSUB_TYPE.VertJointDetail;
                    break;



                case "dimensionsforcutting":
                    returnValue = PAPERSUB_TYPE.DimensionsForCutting;
                    break;
                case "tolerancelimit":
                    returnValue = PAPERSUB_TYPE.ToleranceLimit;
                    break;
                case "shellplatechordlength":
                    returnValue = PAPERSUB_TYPE.ShellPlateChordLength;
                    break;


                case "nameplatebracket":
                    returnValue = PAPERSUB_TYPE.NamePlateBracket;
                    break;
                case "earthlug":
                    returnValue = PAPERSUB_TYPE.EarthLug;
                    break;
                case "settlementcheckpiece":
                    returnValue = PAPERSUB_TYPE.SettlementCheckPiece;
                    break;



                case "shellplatearrangement":
                    returnValue = PAPERSUB_TYPE.ShellPlateArrangement;
                    break;



                case "onecourseshellplate":
                    returnValue = PAPERSUB_TYPE.ONECOURSESHELLPLATE;
                    break;


                // Bottom
                case "bottomplatearrangement":
                    returnValue = PAPERSUB_TYPE.BottomPlateArrangement;
                    break;


                case "bottomplatejointdetail":
                    returnValue = PAPERSUB_TYPE.BottomPlateJointDetail;
                    break;
                case "bottomplatejointannulardetail":
                    returnValue = PAPERSUB_TYPE.BottomPlateJointAnnularDetail;
                    break;

                case "bottomplateweldingdetailc":
                    returnValue = PAPERSUB_TYPE.BottomPlateWeldingDetailC;
                    break;
                case "bottomplateweldingdetaild":
                    returnValue = PAPERSUB_TYPE.BottomPlateWeldingDetailD;
                    break;
                case "bottomplateweldingdetailbb":
                    returnValue = PAPERSUB_TYPE.BottomPlateWeldingDetailBB;
                    break;

                case "backingstripweldingdetail":
                    returnValue = PAPERSUB_TYPE.BackingStripWeldingDetail;
                    break;
                case "bottomplateshelljointdetail":
                    returnValue = PAPERSUB_TYPE.BottomPlateShellJointDetail;
                    break;

                case "bottomplatecuttingplan":
                    returnValue = PAPERSUB_TYPE.BottomPlateCuttingPlan;
                    break;
                case "annularplatecuttingplan":
                    returnValue = PAPERSUB_TYPE.AnnularPlateCuttingPlan;
                    break;
                case "backingstrip":
                    returnValue = PAPERSUB_TYPE.BackingStrip;
                    break;


                // Roof
                case "roofplatearrangement":
                    returnValue = PAPERSUB_TYPE.RoofPlateArrangement;
                    break;

                case "roofcompressionringjointdetail":
                    returnValue = PAPERSUB_TYPE.RoofCompressionRingJointDetail;
                    break;
                case "roofplateweldingdetailc":
                    returnValue = PAPERSUB_TYPE.RoofPlateWeldingDetailC;
                    break;
                case "roofplateweldingdetaild":
                    returnValue = PAPERSUB_TYPE.RoofPlateWeldingDetailD;
                    break;
                case "roofplateweldingdetaildd":
                    returnValue = PAPERSUB_TYPE.RoofPlateWeldingDetailDD;
                    break;
                case "roofcompressionweldingdetail":
                    returnValue = PAPERSUB_TYPE.RoofCompressionWeldingDetail;
                    break;

                case "roofplatecuttingplan":
                    returnValue = PAPERSUB_TYPE.RoofPlateCuttingPlan;
                    break;
                case "roofcompressionringcuttingplan":
                    returnValue = PAPERSUB_TYPE.RoofCompressionRingCuttingPlan;
                    break;




            }
            return returnValue;
        }
    }
}