
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









        #region Roof Structure Type
        // Roof Structure 
        private List<PaperAreaModel> GetPaperAreaStructure(string selTankType, string selStructureType, double selLayerCount, double selRoofOD)
        {
            List<PaperAreaModel> newList = new List<PaperAreaModel>();
            switch (selTankType.ToLower())
            {
                case "crt":
                    newList.AddRange(GetPaperAreaCRTStructure(selStructureType, selLayerCount));
                    break;
                case "drt":
                    newList.AddRange(GetPaperAreaDRTStructure(selStructureType, selLayerCount));
                    break;

            }
            return newList;
        }

        #endregion

        #region CRTStructure

        private List<PaperAreaModel> GetPaperAreaCRTStructure(string selStructureType, double selLayerCount)
        {
            List<PaperAreaModel> newList = new List<PaperAreaModel>();
            switch (selStructureType)
            {
                case "CenterRingInt":
                    newList.AddRange(GetPaperAreaCRTCenterRingInternal());
                    break;

                case "CenterRingExt":
                    newList.AddRange(GetPaperAreaCRTCenterRingExternal());
                    break;
                case "Column":
                    if (selLayerCount == 1)
                    {
                        //LayerCount 1 = Assembly, Orientation 1장, Girder 없음
                        newList.AddRange(GatPaperAreaCRTStructureColumn01());
                    }
                    else if (selLayerCount == 2)
                    {
                        //LayerCount 2 = Assembly, Orientation 1장, Girder, Rafter 1장
                        newList.AddRange(GatPaperAreaCRTStructureColumn02());
                    }
                    else if (selLayerCount == 3 || selLayerCount == 4)
                    {
                        //LayerCount 3이상 = Assembly, Orientation 각 1장씩, Girder, Rafter 따로구성
                        newList.AddRange(GatPaperAreaCRTStructureColumn03());
                    }
                    else if (selLayerCount == 5)
                    {
                        //LayerCount 5 = Assembly, Orientation 각 1장씩, Girder, Rafter 2장씩 구성
                        newList.AddRange(GatPaperAreaCRTStructureColumn04());
                    }
                    else if (selLayerCount >= 6)
                    {
                        //LayerCount 5 = Assembly, Orientation 각 1장씩, Girder, Rafter 2장씩 구성
                        newList.AddRange(GatPaperAreaCRTStructureColumn05());

                    }



                    break;
            }
            return newList;
        }

        public List<PaperAreaModel> GetPaperAreaCRTCenterRingInternal()
        {
            double viewIDNumber = 17000;

            List<PaperAreaModel> newList = new List<PaperAreaModel>();

            PaperAreaModel Detail140 = new PaperAreaModel();
            Detail140.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail140.Page = 1;

            Detail140.Name = PAPERMAIN_TYPE.DETAIL;
            Detail140.SubName = PAPERSUB_TYPE.RoofStructureAssembly;
            Detail140.TitleName = "ROOF STRUCTURE ASSEMBLY";
            Detail140.TitleSubName = "";
            Detail140.IsFix = true;
            Detail140.Row = 1;
            Detail140.Column = 1;
            Detail140.RowSpan = 4;
            Detail140.ColumnSpan = 6;
            Detail140.ScaleValue = 0; // Auto Scale
            Detail140.otherWidth = 260;
            Detail140.otherHeight = 350;

            Detail140.ReferencePoint.X = 100000;
            Detail140.ReferencePoint.Y = -100000;
            Detail140.ModelCenterLocation.X = Detail140.ReferencePoint.X;
            Detail140.ModelCenterLocation.Y = Detail140.ReferencePoint.Y;

            Detail140.viewID = 106000 + viewIDNumber++;
            newList.Add(Detail140);

            PaperAreaModel Detail141 = new PaperAreaModel();
            Detail141.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail141.Page = 1;

            Detail141.Name = PAPERMAIN_TYPE.DETAIL;
            Detail141.SubName = PAPERSUB_TYPE.RoofStructureOrientation;
            Detail141.TitleName = "ROOF STRUCTURE ORIENTATION";
            Detail141.TitleSubName = "";
            Detail141.IsFix = true;
            Detail141.Row = 1;
            Detail141.Column = 7;
            Detail141.RowSpan = 4;
            Detail141.ColumnSpan = 6;
            Detail141.ScaleValue = 0; // Auto Scale
            Detail141.otherWidth = 260;
            Detail141.otherHeight = 350;

            Detail141.ReferencePoint.X = 200000;
            Detail141.ReferencePoint.Y = -100000;
            Detail141.ModelCenterLocation.X = Detail141.ReferencePoint.X;
            Detail141.ModelCenterLocation.Y = Detail141.ReferencePoint.Y;

            Detail141.viewID = 107000 + viewIDNumber++;
            newList.Add(Detail141);


            PaperAreaModel Detail142 = new PaperAreaModel();
            Detail142.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail142.Page = 2;

            Detail142.Name = PAPERMAIN_TYPE.DETAIL;
            Detail142.SubName = PAPERSUB_TYPE.RafterSideClipDetail;
            Detail142.TitleName = "RAFTER SIDE CLIP DETAIL";
            Detail142.TitleSubName = "(SCALE : " + Detail142.ScaleValue + ")";
            Detail142.IsFix = true;
            Detail142.Row = 1;
            Detail142.Column = 1;
            Detail142.RowSpan = 2;
            Detail142.ColumnSpan = 6;
            Detail142.ScaleValue = 0; // Auto Scale
            Detail142.TitleSubName = "(SCALE : " + Detail142.ScaleValue + ")";// Scale 값 받기 위해 한번 더 기록

            Detail142.otherWidth = 75;
            Detail142.otherHeight = 90;

            Detail142.ReferencePoint.X = 10000;
            Detail142.ReferencePoint.Y = -200000;
            Detail142.ModelCenterLocation.X = Detail142.ReferencePoint.X;
            Detail142.ModelCenterLocation.Y = Detail142.ReferencePoint.Y;

            Detail142.viewID = 108000 + viewIDNumber++;
            newList.Add(Detail142);

            PaperAreaModel Detail143 = new PaperAreaModel();
            Detail143.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail143.Page = 2;

            Detail143.Name = PAPERMAIN_TYPE.DETAIL;
            Detail143.SubName = PAPERSUB_TYPE.RafterDetail;
            Detail143.TitleName = "RAFTER DETAIL";
            Detail143.TitleSubName = "(SCALE : " + Detail143.ScaleValue + ")";
            Detail143.IsFix = true;
            Detail143.Row = 1;
            Detail143.Column = 1;
            Detail143.RowSpan = 1;
            Detail143.ColumnSpan = 6;
            Detail143.ScaleValue = 0; // Auto Scale
            Detail143.TitleSubName = "(SCALE : " + Detail143.ScaleValue + ")";// Scale 값 받기 위해 한번 더 기록

            Detail143.otherWidth = 270;
            Detail143.otherHeight = 70;

            Detail143.ReferencePoint.X = 20000;
            Detail143.ReferencePoint.Y = -200000;
            Detail143.ModelCenterLocation.X = Detail143.ReferencePoint.X;
            Detail143.ModelCenterLocation.Y = Detail143.ReferencePoint.Y;

            Detail143.viewID = 109000 + viewIDNumber++;
            newList.Add(Detail143);

            PaperAreaModel Detail144 = new PaperAreaModel();
            Detail144.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail144.Page = 2;

            Detail144.Name = PAPERMAIN_TYPE.DETAIL;
            Detail144.SubName = PAPERSUB_TYPE.CenterRingDetail;
            Detail144.TitleName = "CENTER RING DETAIL";
            Detail144.TitleSubName = "(SCALE : " + Detail144.ScaleValue + ")";
            Detail144.IsFix = true;
            Detail144.Row = 1;
            Detail144.Column = 1;
            Detail144.RowSpan = 2;
            Detail144.ColumnSpan = 4;
            Detail144.ScaleValue = 0; // Auto Scale
            Detail144.TitleSubName = "(SCALE : " + Detail144.ScaleValue + ")";// Scale 값 받기 위해 한번 더 기록

            Detail144.otherWidth = 150;
            Detail144.otherHeight = 30;

            Detail144.ReferencePoint.X = 30000;
            Detail144.ReferencePoint.Y = -200000;
            Detail144.ModelCenterLocation.X = Detail144.ReferencePoint.X;
            Detail144.ModelCenterLocation.Y = Detail144.ReferencePoint.Y;

            Detail144.viewID = 110000 + viewIDNumber++;
            newList.Add(Detail144);

            PaperAreaModel Detail145 = new PaperAreaModel();
            Detail145.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail145.Page = 2;

            Detail145.Name = PAPERMAIN_TYPE.DETAIL;
            Detail145.SubName = PAPERSUB_TYPE.RafterCenterClipDetail;
            Detail145.TitleName = "RAFTER CENTER CLIP DETAIL";
            Detail145.TitleSubName = "(SCALE : " + Detail145.ScaleValue + ")";
            Detail145.IsFix = true;
            Detail145.Row = 1;
            Detail145.Column = 1;
            Detail145.RowSpan = 2;
            Detail145.ColumnSpan = 4;
            Detail145.ScaleValue = 0; // Auto Scale
            Detail145.TitleSubName = "(SCALE : " + Detail145.ScaleValue + ")";// Scale 값 받기 위해 한번 더 기록

            Detail145.otherWidth = 90;
            Detail145.otherHeight = 55;

            Detail145.ReferencePoint.X = 40000;
            Detail145.ReferencePoint.Y = -200000;
            Detail145.ModelCenterLocation.X = Detail145.ReferencePoint.X;
            Detail145.ModelCenterLocation.Y = Detail145.ReferencePoint.Y;

            Detail145.viewID = 111000 + viewIDNumber++;
            newList.Add(Detail145);

            PaperAreaModel Detail146 = new PaperAreaModel();
            Detail146.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail146.Page = 2;

            Detail146.Name = PAPERMAIN_TYPE.DETAIL;
            Detail146.SubName = PAPERSUB_TYPE.PurlinDetail;
            Detail146.TitleName = "PULRIN DETAIL";
            Detail146.TitleSubName = "(SCALE : " + Detail146.ScaleValue + ")";
            Detail146.IsFix = false;
            Detail146.Row = 1;
            Detail146.Column = 1;
            Detail146.RowSpan = 1;
            Detail146.ColumnSpan = 3;
            Detail146.ScaleValue = 0; // Auto Scale
            Detail146.TitleSubName = "(SCALE : " + Detail146.ScaleValue + ")";// Scale 값 받기 위해 한번 더 기록

            Detail146.otherWidth = 125;
            Detail146.otherHeight = 40;

            Detail146.ReferencePoint.X = 50000;
            Detail146.ReferencePoint.Y = -200000;
            Detail146.ModelCenterLocation.X = Detail146.ReferencePoint.X;
            Detail146.ModelCenterLocation.Y = Detail146.ReferencePoint.Y;

            Detail146.viewID = 112000 + viewIDNumber++;
            newList.Add(Detail146);

            PaperAreaModel Detail147 = new PaperAreaModel();
            Detail147.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail147.Page = 2;

            Detail147.Name = PAPERMAIN_TYPE.DETAIL;
            Detail147.SubName = PAPERSUB_TYPE.SectionAA;
            Detail147.TitleName = "SECTION \"A\"-\"A\"";
            Detail147.TitleSubName = "";
            Detail147.IsFix = false;
            Detail147.Row = 1;
            Detail147.Column = 1;
            Detail147.RowSpan = 1;
            Detail147.ColumnSpan = 3;
            Detail147.ScaleValue = 1;


            Detail147.ReferencePoint.X = 60000;
            Detail147.ReferencePoint.Y = -200000;
            Detail147.ModelCenterLocation.X = Detail147.ReferencePoint.X;
            Detail147.ModelCenterLocation.Y = Detail147.ReferencePoint.Y;

            Detail147.viewID = 113000 + viewIDNumber++;
            newList.Add(Detail147);

            PaperAreaModel Detail148 = new PaperAreaModel();
            Detail148.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail148.Page = 2;

            Detail148.Name = PAPERMAIN_TYPE.DETAIL;
            Detail148.SubName = PAPERSUB_TYPE.RibPlateDetail;
            Detail148.TitleName = "RIB PLATE DETAIL";
            Detail148.TitleSubName = "";
            Detail148.IsFix = false;
            Detail148.Row = 1;
            Detail148.Column = 1;
            Detail148.RowSpan = 1;
            Detail148.ColumnSpan = 3;
            Detail148.ScaleValue = 1;


            Detail148.ReferencePoint.X = 70000;
            Detail148.ReferencePoint.Y = -200000;
            Detail148.ModelCenterLocation.X = Detail148.ReferencePoint.X;
            Detail148.ModelCenterLocation.Y = Detail148.ReferencePoint.Y;

            Detail148.viewID = 114000 + viewIDNumber++;
            newList.Add(Detail148);





            return newList;

        }
        public List<PaperAreaModel> GetPaperAreaCRTCenterRingExternal()
        {
            double viewIDNumber = 18000;

            List<PaperAreaModel> newList = new List<PaperAreaModel>();

            PaperAreaModel Detail150 = new PaperAreaModel();
            Detail150.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail150.Page = 1;

            Detail150.Name = PAPERMAIN_TYPE.DETAIL;
            Detail150.SubName = PAPERSUB_TYPE.RoofStructureAssembly;
            Detail150.TitleName = "ROOF STRUCTURE ASSEMBLY";
            Detail150.TitleSubName = "";
            Detail150.IsFix = true;
            Detail150.Row = 1;
            Detail150.Column = 1;
            Detail150.RowSpan = 4;
            Detail150.ColumnSpan = 2;
            Detail150.ScaleValue = 0; // Auto Scale
            Detail150.otherWidth = 260;
            Detail150.otherHeight = 350;

            Detail150.ReferencePoint.X = 100000;
            Detail150.ReferencePoint.Y = -100000;
            Detail150.ModelCenterLocation.X = Detail150.ReferencePoint.X;
            Detail150.ModelCenterLocation.Y = Detail150.ReferencePoint.Y;

            Detail150.viewID = 115000 + viewIDNumber++;
            newList.Add(Detail150);

            PaperAreaModel Detail151 = new PaperAreaModel();
            Detail151.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail151.Page = 1;

            Detail151.Name = PAPERMAIN_TYPE.DETAIL;
            Detail151.SubName = PAPERSUB_TYPE.RoofStructureOrientation;
            Detail151.TitleName = "ROOF STRUCTURE ORIENTATION";
            Detail151.TitleSubName = "";
            Detail151.IsFix = true;
            Detail151.Row = 1;
            Detail151.Column = 3;
            Detail151.RowSpan = 4;
            Detail151.ColumnSpan = 2;
            Detail151.ScaleValue = 0; // Auto Scale
            Detail151.otherWidth = 260;
            Detail151.otherHeight = 350;

            Detail151.ReferencePoint.X = 200000;
            Detail151.ReferencePoint.Y = -100000;
            Detail151.ModelCenterLocation.X = Detail151.ReferencePoint.X;
            Detail151.ModelCenterLocation.Y = Detail151.ReferencePoint.Y;

            Detail151.viewID = 116000 + viewIDNumber++;
            newList.Add(Detail151);

            PaperAreaModel Detail152 = new PaperAreaModel();
            Detail152.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail152.Page = 2;

            Detail152.Name = PAPERMAIN_TYPE.DETAIL;
            Detail152.SubName = PAPERSUB_TYPE.CenterRingRafterDetail;
            Detail152.TitleName = "CENTER RING & RAFTER DETAIL";
            Detail152.TitleSubName = "(SCALE : " + Detail152.ScaleValue + ")";
            Detail152.IsFix = true;
            Detail152.Row = 1;
            Detail152.Column = 1;
            Detail152.RowSpan = 2;
            Detail152.ColumnSpan = 4;
            Detail152.ScaleValue = 0; // Auto Scale
            Detail152.TitleSubName = "(SCALE : " + Detail152.ScaleValue + ")";// Scale 값 받기 위해 한번 더 기록

            Detail152.otherWidth = 350; //수정 요함
            Detail152.otherHeight = 250; // 수정 요함

            Detail152.ReferencePoint.X = 10000;
            Detail152.ReferencePoint.Y = -200000;
            Detail152.ModelCenterLocation.X = Detail152.ReferencePoint.X;
            Detail152.ModelCenterLocation.Y = Detail152.ReferencePoint.Y;

            Detail152.viewID = 117000 + viewIDNumber++;
            newList.Add(Detail152);

            PaperAreaModel Detail153 = new PaperAreaModel();
            Detail153.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail153.Page = 2;

            Detail153.Name = PAPERMAIN_TYPE.DETAIL;
            Detail153.SubName = PAPERSUB_TYPE.CenterRingDetail;
            Detail153.TitleName = "CENTER RING DETAIL";
            Detail153.TitleSubName = "(SCALE : " + Detail153.ScaleValue + ")";
            Detail153.IsFix = true;
            Detail153.Row = 3;
            Detail153.Column = 1;
            Detail153.RowSpan = 2;
            Detail153.ColumnSpan = 2;
            Detail153.ScaleValue = 0; // Auto Scale
            Detail153.TitleSubName = "(SCALE : " + Detail153.ScaleValue + ")";// Scale 값 받기 위해 한번 더 기록

            Detail153.otherWidth = 230; //수정 요함
            Detail153.otherHeight = 230; //수정 요함

            Detail153.ReferencePoint.X = 20000;
            Detail153.ReferencePoint.Y = -200000;
            Detail153.ModelCenterLocation.X = Detail153.ReferencePoint.X;
            Detail153.ModelCenterLocation.Y = Detail153.ReferencePoint.Y;

            Detail153.viewID = 118000 + viewIDNumber++;
            newList.Add(Detail153);

            PaperAreaModel Detail154 = new PaperAreaModel();
            Detail154.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail154.Page = 2;

            Detail154.Name = PAPERMAIN_TYPE.DETAIL;
            Detail154.SubName = PAPERSUB_TYPE.DetailB;
            Detail154.TitleName = "DETAIL \"B\"";
            Detail154.TitleSubName = "";
            Detail154.IsFix = true;
            Detail154.Row = 3;
            Detail154.Column = 3;
            Detail154.ScaleValue = 1;

            Detail154.otherWidth = 100;
            Detail154.otherHeight = 100;

            Detail154.ReferencePoint.X = 30000;
            Detail154.ReferencePoint.Y = -200000;
            Detail154.ModelCenterLocation.X = Detail154.ReferencePoint.X;
            Detail154.ModelCenterLocation.Y = Detail154.ReferencePoint.Y;

            Detail154.viewID = 119000 + viewIDNumber++;
            newList.Add(Detail154);

            PaperAreaModel Detail155 = new PaperAreaModel();
            Detail155.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail155.Page = 2;

            Detail155.Name = PAPERMAIN_TYPE.DETAIL;
            Detail155.SubName = PAPERSUB_TYPE.SectionCC;
            Detail155.TitleName = "SECTION \"C\"-\"C\"";
            Detail155.TitleSubName = "";
            Detail155.IsFix = true;
            Detail155.Row = 3;
            Detail155.Column = 4;
            Detail155.ScaleValue = 1;

            Detail155.otherWidth = 100;
            Detail155.otherHeight = 100;

            Detail155.ReferencePoint.X = 40000;
            Detail155.ReferencePoint.Y = -200000;
            Detail155.ModelCenterLocation.X = Detail155.ReferencePoint.X;
            Detail155.ModelCenterLocation.Y = Detail155.ReferencePoint.Y;

            Detail155.viewID = 120000 + viewIDNumber++;
            newList.Add(Detail155);

            PaperAreaModel Detail156 = new PaperAreaModel();
            Detail156.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail156.Page = 2;

            Detail156.Name = PAPERMAIN_TYPE.DETAIL;
            Detail156.SubName = PAPERSUB_TYPE.RafterAndReinfPadCrossDetail;
            Detail156.TitleName = "RAFTER AND REINF. PAD CROSS DETAil";
            Detail156.TitleSubName = "";
            Detail156.IsFix = false;
            Detail156.ScaleValue = 1;

            Detail156.otherWidth = 125;
            Detail156.otherHeight = 60;

            Detail156.ReferencePoint.X = 50000;
            Detail156.ReferencePoint.Y = -200000;
            Detail156.ModelCenterLocation.X = Detail156.ReferencePoint.X;
            Detail156.ModelCenterLocation.Y = Detail156.ReferencePoint.Y;

            Detail156.viewID = 121000 + viewIDNumber++;
            newList.Add(Detail156);

            PaperAreaModel Detail157 = new PaperAreaModel();
            Detail157.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail157.Page = 2;

            Detail157.Name = PAPERMAIN_TYPE.DETAIL;
            Detail157.SubName = PAPERSUB_TYPE.ViewCC;
            Detail157.TitleName = "VIEW \"C\"-\"C\"";
            Detail157.TitleSubName = "";
            Detail157.IsFix = false;
            Detail157.ScaleValue = 1;


            Detail157.ReferencePoint.X = 60000;
            Detail157.ReferencePoint.Y = -200000;
            Detail157.ModelCenterLocation.X = Detail157.ReferencePoint.X;
            Detail157.ModelCenterLocation.Y = Detail157.ReferencePoint.Y;

            Detail157.viewID = 122000 + viewIDNumber++;
            newList.Add(Detail157);



            return newList;
        }
        public List<PaperAreaModel> GatPaperAreaCRTStructureColumn01() // LayerCont 1
        {
            double viewIDNumber = 19000;

            List<PaperAreaModel> newList = new List<PaperAreaModel>();

            PaperAreaModel Detail160 = new PaperAreaModel();
            Detail160.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail160.Page = 1;

            Detail160.Name = PAPERMAIN_TYPE.DETAIL;
            Detail160.SubName = PAPERSUB_TYPE.RoofStructureAssembly;
            Detail160.TitleName = "ROOF STRUCTURE ASSEMBLY";
            Detail160.TitleSubName = "";
            Detail160.IsFix = true;
            Detail160.Row = 1;
            Detail160.Column = 1;
            Detail160.RowSpan = 4;
            Detail160.ColumnSpan = 2;
            Detail160.ScaleValue = 0; // Auto Scale
            Detail160.otherWidth = 260;
            Detail160.otherHeight = 350;

            Detail160.ReferencePoint.X = 100000;
            Detail160.ReferencePoint.Y = -100000;
            Detail160.ModelCenterLocation.X = Detail160.ReferencePoint.X;
            Detail160.ModelCenterLocation.Y = Detail160.ReferencePoint.Y;

            Detail160.viewID = 123000 + viewIDNumber++;
            newList.Add(Detail160);

            PaperAreaModel Detail161 = new PaperAreaModel();
            Detail161.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail161.Page = 1;

            Detail161.Name = PAPERMAIN_TYPE.DETAIL;
            Detail161.SubName = PAPERSUB_TYPE.RoofStructureOrientation;
            Detail161.TitleName = "ROOF STRUCTURE ORIENTATION";
            Detail161.TitleSubName = "";
            Detail161.IsFix = true;
            Detail161.Row = 1;
            Detail161.Column = 1;
            Detail161.ScaleValue = 0; // Auto Scale
            Detail161.otherWidth = 120; // 수정요함
            Detail161.otherHeight = 120; // 수정요함

            Detail161.ReferencePoint.X = 200000;
            Detail161.ReferencePoint.Y = -100000;
            Detail161.ModelCenterLocation.X = Detail161.ReferencePoint.X;
            Detail161.ModelCenterLocation.Y = Detail161.ReferencePoint.Y;

            Detail161.viewID = 124000 + viewIDNumber++;
            newList.Add(Detail161);

            PaperAreaModel Detail162 = new PaperAreaModel();
            Detail162.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail162.Page = 2;

            Detail162.Name = PAPERMAIN_TYPE.DETAIL;
            Detail162.SubName = PAPERSUB_TYPE.SectionAA;
            Detail162.TitleName = "SECTION\"A\"-\"A\"";
            Detail162.TitleSubName = "(1st COLUMN TOP PLATE)";
            Detail162.IsFix = true;
            Detail162.Row = 1;
            Detail162.Column = 1;
            Detail162.ScaleValue = 0; // Auto Scale
            Detail162.otherWidth = 120; //수정 요함
            Detail162.otherHeight = 300; // 수정 요함

            Detail162.ReferencePoint.X = 10000;
            Detail162.ReferencePoint.Y = -200000;
            Detail162.ModelCenterLocation.X = Detail162.ReferencePoint.X;
            Detail162.ModelCenterLocation.Y = Detail162.ReferencePoint.Y;

            Detail162.viewID = 125000 + viewIDNumber++;
            newList.Add(Detail162);

            PaperAreaModel Detail163 = new PaperAreaModel();
            Detail163.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail163.Page = 2;

            Detail163.Name = PAPERMAIN_TYPE.DETAIL;
            Detail163.SubName = PAPERSUB_TYPE.CenterColumnDetail;
            Detail163.TitleName = "CENTER COLUMN DETAIL";
            Detail163.TitleSubName = "";
            Detail163.IsFix = true;
            Detail163.Row = 2;
            Detail163.Column = 1;
            Detail163.RowSpan = 3;
            Detail163.ColumnSpan = 1;
            Detail163.ScaleValue = 0; // Auto Scale

            Detail163.otherWidth = 120; //수정 요함
            Detail163.otherHeight = 260; //수정 요함

            Detail163.ReferencePoint.X = 20000;
            Detail163.ReferencePoint.Y = -200000;
            Detail163.ModelCenterLocation.X = Detail163.ReferencePoint.X;
            Detail163.ModelCenterLocation.Y = Detail163.ReferencePoint.Y;

            Detail163.viewID = 126000 + viewIDNumber++;
            newList.Add(Detail163);

            PaperAreaModel Detail164 = new PaperAreaModel();
            Detail164.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail164.Page = 2;

            Detail164.Name = PAPERMAIN_TYPE.DETAIL;
            Detail164.SubName = PAPERSUB_TYPE.Rafter;
            Detail164.TitleName = "RAFTER";
            Detail164.TitleSubName = "(ㄷ200x80x7.35x11)"; //Rafter 정보 수정요함
            Detail164.IsFix = true;
            Detail164.Row = 1;
            Detail164.Column = 2;
            Detail164.RowSpan = 1;
            Detail164.ColumnSpan = 2;
            Detail164.ScaleValue = 0; // Auto Scale

            Detail164.otherWidth = 200;
            Detail164.otherHeight = 100;

            Detail164.ReferencePoint.X = 30000;
            Detail164.ReferencePoint.Y = -200000;
            Detail164.ModelCenterLocation.X = Detail164.ReferencePoint.X;
            Detail164.ModelCenterLocation.Y = Detail164.ReferencePoint.Y;

            Detail164.viewID = 127000 + viewIDNumber++;
            newList.Add(Detail164);

            PaperAreaModel Detail165 = new PaperAreaModel();
            Detail165.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail165.Page = 2;

            Detail165.Name = PAPERMAIN_TYPE.DETAIL;
            Detail165.SubName = PAPERSUB_TYPE.DetailF;
            Detail165.TitleName = "DETAIL \"F\"";
            Detail165.TitleSubName = "";
            Detail165.IsFix = true;
            Detail165.Row = 1;
            Detail165.Column = 4;
            Detail165.ScaleValue = 0; // Auto Scale

            Detail165.otherWidth = 100;
            Detail165.otherHeight = 100;

            Detail165.ReferencePoint.X = 40000;
            Detail165.ReferencePoint.Y = -200000;
            Detail165.ModelCenterLocation.X = Detail165.ReferencePoint.X;
            Detail165.ModelCenterLocation.Y = Detail165.ReferencePoint.Y;

            Detail165.viewID = 128000 + viewIDNumber++;
            newList.Add(Detail165);

            PaperAreaModel Detail166 = new PaperAreaModel();
            Detail166.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail166.Page = 2;

            Detail166.Name = PAPERMAIN_TYPE.DETAIL;
            Detail166.SubName = PAPERSUB_TYPE.SectionBB;
            Detail166.TitleName = "SECTION \"B\"-\"B\"";
            Detail166.TitleSubName = "";
            Detail166.IsFix = true;
            Detail166.Row = 2;
            Detail166.Column = 2;
            Detail166.ScaleValue = 0; //Auto Scale

            Detail166.otherWidth = 120;
            Detail166.otherHeight = 120;

            Detail166.ReferencePoint.X = 50000;
            Detail166.ReferencePoint.Y = -200000;
            Detail166.ModelCenterLocation.X = Detail166.ReferencePoint.X;
            Detail166.ModelCenterLocation.Y = Detail166.ReferencePoint.Y;

            Detail166.viewID = 129000 + viewIDNumber++;
            newList.Add(Detail166);

            PaperAreaModel Detail167 = new PaperAreaModel();
            Detail167.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail167.Page = 2;

            Detail167.Name = PAPERMAIN_TYPE.DETAIL;
            Detail167.SubName = PAPERSUB_TYPE.SectionEE;
            Detail167.TitleName = "SECTION \"E\"-\"E\"";
            Detail167.TitleSubName = "";
            Detail167.IsFix = true;
            Detail167.Row = 2;
            Detail167.Column = 3;
            Detail167.ScaleValue = 1;


            Detail167.ReferencePoint.X = 60000;
            Detail167.ReferencePoint.Y = -200000;
            Detail167.ModelCenterLocation.X = Detail167.ReferencePoint.X;
            Detail167.ModelCenterLocation.Y = Detail167.ReferencePoint.Y;

            Detail167.viewID = 130000 + viewIDNumber++;
            newList.Add(Detail167);


            PaperAreaModel Detail168 = new PaperAreaModel();
            Detail168.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail168.Page = 2;

            Detail168.Name = PAPERMAIN_TYPE.DETAIL;
            Detail168.SubName = PAPERSUB_TYPE.DrainDetail;
            Detail168.TitleName = "DRAIN DETAIL";
            Detail168.TitleSubName = "(SECTION \"G\")";
            Detail168.IsFix = true;
            Detail168.Row = 2;
            Detail168.Column = 4;
            Detail168.ScaleValue = 1;


            Detail168.ReferencePoint.X = 70000;
            Detail168.ReferencePoint.Y = -200000;
            Detail168.ModelCenterLocation.X = Detail168.ReferencePoint.X;
            Detail168.ModelCenterLocation.Y = Detail168.ReferencePoint.Y;

            Detail168.viewID = 131000 + viewIDNumber++;
            newList.Add(Detail168);

            PaperAreaModel Detail169 = new PaperAreaModel();
            Detail169.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail169.Page = 2;

            Detail169.Name = PAPERMAIN_TYPE.DETAIL;
            Detail169.SubName = PAPERSUB_TYPE.DetailC;
            Detail169.TitleName = "DETAIL \"C\"";
            Detail169.TitleSubName = "";
            Detail169.IsFix = true;
            Detail169.Row = 3;
            Detail169.Column = 2;
            Detail169.ScaleValue = 1;


            Detail169.ReferencePoint.X = 80000;
            Detail169.ReferencePoint.Y = -200000;
            Detail169.ModelCenterLocation.X = Detail169.ReferencePoint.X;
            Detail169.ModelCenterLocation.Y = Detail169.ReferencePoint.Y;

            Detail169.viewID = 132000 + viewIDNumber++;
            newList.Add(Detail169);

            PaperAreaModel Detail170 = new PaperAreaModel();
            Detail170.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail170.Page = 2;

            Detail170.Name = PAPERMAIN_TYPE.DETAIL;
            Detail170.SubName = PAPERSUB_TYPE.DetailD;
            Detail170.TitleName = "DETAIL \"D\"";
            Detail170.TitleSubName = "";
            Detail170.IsFix = true;
            Detail170.Row = 4;
            Detail170.Column = 2;
            Detail170.ScaleValue = 1;


            Detail170.ReferencePoint.X = 90000;
            Detail170.ReferencePoint.Y = -200000;
            Detail170.ModelCenterLocation.X = Detail170.ReferencePoint.X;
            Detail170.ModelCenterLocation.Y = Detail170.ReferencePoint.Y;

            Detail170.viewID = 133000 + viewIDNumber++;
            newList.Add(Detail170);

            PaperAreaModel Detail171 = new PaperAreaModel();
            Detail171.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail171.Page = 2;

            Detail171.Name = PAPERMAIN_TYPE.DETAIL;
            Detail171.SubName = PAPERSUB_TYPE.RafterSideClipDetail;
            Detail171.TitleName = "RAFTER SIDE CLIP DETAIL";
            Detail171.TitleSubName = "(SCALE : " + Detail171.ScaleValue + ")";
            Detail171.IsFix = true;
            Detail171.Row = 4;
            Detail171.Column = 2;
            Detail171.ScaleValue = 0;//Auto Scali
            Detail171.otherWidth = 75;
            Detail171.otherHeight = 90;
            Detail171.TitleSubName = "(SCALE : " + Detail171.ScaleValue + ")";// Scale 값 받기 위해 한번 더 기록



            Detail171.ReferencePoint.X = 100000;
            Detail171.ReferencePoint.Y = -200000;
            Detail171.ModelCenterLocation.X = Detail171.ReferencePoint.X;
            Detail171.ModelCenterLocation.Y = Detail171.ReferencePoint.Y;

            Detail171.viewID = 134000 + viewIDNumber++;
            newList.Add(Detail171);






            return newList;

        }
        public List<PaperAreaModel> GatPaperAreaCRTStructureColumn02() //LayerCount 2 
        {
            double viewIDNumber = 20000;

            List<PaperAreaModel> newList = new List<PaperAreaModel>();

            PaperAreaModel Detail180 = new PaperAreaModel();
            Detail180.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail180.Page = 1;

            Detail180.Name = PAPERMAIN_TYPE.DETAIL;
            Detail180.SubName = PAPERSUB_TYPE.RoofStructureAssembly;
            Detail180.TitleName = "ROOF STRUCTURE ASSEMBLY";
            Detail180.TitleSubName = "";
            Detail180.IsFix = true;
            Detail180.Row = 1;
            Detail180.Column = 1;
            Detail180.RowSpan = 4;
            Detail180.ColumnSpan = 2;
            Detail180.ScaleValue = 0; // Auto Scale
            Detail180.otherWidth = 260;
            Detail180.otherHeight = 350;

            Detail180.ReferencePoint.X = 100000;
            Detail180.ReferencePoint.Y = -100000;
            Detail180.ModelCenterLocation.X = Detail180.ReferencePoint.X;
            Detail180.ModelCenterLocation.Y = Detail180.ReferencePoint.Y;

            Detail180.viewID = 135000 + viewIDNumber++;
            newList.Add(Detail180);

            PaperAreaModel Detail181 = new PaperAreaModel();
            Detail181.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail181.Page = 1;

            Detail181.Name = PAPERMAIN_TYPE.DETAIL;
            Detail181.SubName = PAPERSUB_TYPE.RoofStructureOrientation;
            Detail181.TitleName = "ROOF STRUCTURE ORIENTATION";
            Detail181.TitleSubName = "";
            Detail181.IsFix = true;
            Detail181.Row = 1;
            Detail181.Column = 1;
            Detail181.ScaleValue = 0; // Auto Scale
            Detail181.otherWidth = 260; // 수정요함
            Detail181.otherHeight = 350; // 수정요함

            Detail181.ReferencePoint.X = 200000;
            Detail181.ReferencePoint.Y = -100000;
            Detail181.ModelCenterLocation.X = Detail181.ReferencePoint.X;
            Detail181.ModelCenterLocation.Y = Detail181.ReferencePoint.Y;

            Detail181.viewID = 136000 + viewIDNumber++;
            newList.Add(Detail181);

            PaperAreaModel Detail182 = new PaperAreaModel();
            Detail182.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail182.Page = 2;

            Detail182.Name = PAPERMAIN_TYPE.DETAIL;
            Detail182.SubName = PAPERSUB_TYPE.G1Girder;
            Detail182.TitleName = "G1 GIRDER";
            Detail182.TitleSubName = "(H300x300x10x15)"; //Girder 정보 불러와야 함
            Detail182.IsFix = true;
            Detail182.Row = 1;
            Detail182.Column = 1;
            Detail182.RowSpan = 1;
            Detail182.ColumnSpan = 3;
            Detail182.ScaleValue = 0;//Auto Scali
            Detail182.otherWidth = 400; //수정요함
            Detail182.otherHeight = 120; // 수정요함



            Detail182.ReferencePoint.X = 10000;
            Detail182.ReferencePoint.Y = -200000;
            Detail182.ModelCenterLocation.X = Detail182.ReferencePoint.X;
            Detail182.ModelCenterLocation.Y = Detail182.ReferencePoint.Y;

            Detail182.viewID = 137000 + viewIDNumber++;
            newList.Add(Detail182);


            PaperAreaModel Detail183 = new PaperAreaModel();
            Detail183.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail183.Page = 2;

            Detail183.Name = PAPERMAIN_TYPE.DETAIL;
            Detail183.SubName = PAPERSUB_TYPE.DetailA;
            Detail183.TitleName = "DETAIL \"A\"";
            Detail183.TitleSubName = "(TYP.)";
            Detail183.IsFix = true;
            Detail183.Row = 1;
            Detail183.Column = 4;
            Detail183.ScaleValue = 0;//Auto Scali
            Detail183.otherWidth = 120; //수정요함
            Detail183.otherHeight = 120; // 수정요함



            Detail183.ReferencePoint.X = 20000;
            Detail183.ReferencePoint.Y = -200000;
            Detail183.ModelCenterLocation.X = Detail183.ReferencePoint.X;
            Detail183.ModelCenterLocation.Y = Detail183.ReferencePoint.Y;

            Detail183.viewID = 138000 + viewIDNumber++;
            newList.Add(Detail183);

            PaperAreaModel Detail184 = new PaperAreaModel();
            Detail184.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail184.Page = 2;

            Detail184.Name = PAPERMAIN_TYPE.DETAIL;
            Detail184.SubName = PAPERSUB_TYPE.InnerRafter;
            Detail184.TitleName = "INNER RAFTER " + "R1~R" + "3"; // 이름 수정 필요함
            Detail184.TitleSubName = "(SEE TABLE2)"; //Girder 정보 불러와야 함
            Detail184.IsFix = true;
            Detail184.Row = 2;
            Detail184.Column = 1;
            Detail184.RowSpan = 1;
            Detail184.ColumnSpan = 2;
            Detail184.ScaleValue = 0;//Auto Scali
            Detail184.otherWidth = 250; //수정요함
            Detail184.otherHeight = 120; // 수정요함



            Detail184.ReferencePoint.X = 30000;
            Detail184.ReferencePoint.Y = -200000;
            Detail184.ModelCenterLocation.X = Detail184.ReferencePoint.X;
            Detail184.ModelCenterLocation.Y = Detail184.ReferencePoint.Y;

            Detail184.viewID = 139000 + viewIDNumber++;
            newList.Add(Detail184);

            PaperAreaModel Detail185 = new PaperAreaModel();
            Detail185.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail185.Page = 2;

            Detail185.Name = PAPERMAIN_TYPE.DETAIL;
            Detail185.SubName = PAPERSUB_TYPE.InnerRafter;
            Detail185.TitleName = "OUTER RAFTER " + "R" + "4" + "~R" + "7"; // 이름 수정 필요함
            Detail185.TitleSubName = "(SEE TABLE2)"; //Girder 정보 불러와야 함
            Detail185.IsFix = true;
            Detail185.Row = 2;
            Detail185.Column = 1;
            Detail185.RowSpan = 1;
            Detail185.ColumnSpan = 2;
            Detail185.ScaleValue = 0;//Auto Scali
            Detail185.otherWidth = 250; //수정요함
            Detail185.otherHeight = 120; // 수정요함



            Detail185.ReferencePoint.X = 40000;
            Detail185.ReferencePoint.Y = -200000;
            Detail185.ModelCenterLocation.X = Detail185.ReferencePoint.X;
            Detail185.ModelCenterLocation.Y = Detail185.ReferencePoint.Y;

            Detail185.viewID = 140000 + viewIDNumber++;
            newList.Add(Detail185);


            PaperAreaModel Detail186 = new PaperAreaModel();
            Detail186.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail186.Page = 2;

            Detail186.Name = PAPERMAIN_TYPE.DETAIL;
            Detail186.SubName = PAPERSUB_TYPE.BracketDetail;
            Detail186.TitleName = "BRACkET DETAIL";
            Detail186.TitleSubName = "(SEE TABLE1)";
            Detail186.IsFix = true;
            Detail186.Row = 2;
            Detail186.Column = 3;
            Detail186.ScaleValue = 0;//Auto Scali
            Detail186.otherWidth = 120; //수정요함
            Detail186.otherHeight = 120; // 수정요함



            Detail186.ReferencePoint.X = 50000;
            Detail186.ReferencePoint.Y = -200000;
            Detail186.ModelCenterLocation.X = Detail186.ReferencePoint.X;
            Detail186.ModelCenterLocation.Y = Detail186.ReferencePoint.Y;

            Detail186.viewID = 141000 + viewIDNumber++;
            newList.Add(Detail186);

            PaperAreaModel Detail187 = new PaperAreaModel();
            Detail187.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail187.Page = 2;

            Detail187.Name = PAPERMAIN_TYPE.DETAIL;
            Detail187.SubName = PAPERSUB_TYPE.Table1; ;
            Detail187.TitleName = "TABLE1";
            Detail187.TitleSubName = "";
            Detail187.IsFix = true;
            Detail187.Row = 4;
            Detail187.Column = 1;
            Detail187.ScaleValue = 0;//Auto Scali
            Detail187.otherWidth = 120; //수정요함
            Detail187.otherHeight = 120; // 수정요함



            Detail187.ReferencePoint.X = 60000;
            Detail187.ReferencePoint.Y = -200000;
            Detail187.ModelCenterLocation.X = Detail187.ReferencePoint.X;
            Detail187.ModelCenterLocation.Y = Detail187.ReferencePoint.Y;

            Detail187.viewID = 142000 + viewIDNumber++;
            newList.Add(Detail187);

            PaperAreaModel Detail188 = new PaperAreaModel();
            Detail188.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail188.Page = 2;

            Detail188.Name = PAPERMAIN_TYPE.DETAIL;
            Detail188.SubName = PAPERSUB_TYPE.Table2; ;
            Detail188.TitleName = "TABLE2";
            Detail188.TitleSubName = "";
            Detail188.IsFix = true;
            Detail188.Row = 4;
            Detail188.Column = 1;
            Detail188.ScaleValue = 0;//Auto Scali
            Detail188.otherWidth = 120; //수정요함
            Detail188.otherHeight = 120; // 수정요함



            Detail188.ReferencePoint.X = 70000;
            Detail188.ReferencePoint.Y = -200000;
            Detail188.ModelCenterLocation.X = Detail188.ReferencePoint.X;
            Detail188.ModelCenterLocation.Y = Detail188.ReferencePoint.Y;

            Detail188.viewID = 143000 + viewIDNumber++;
            newList.Add(Detail188);


            PaperAreaModel Detail189 = new PaperAreaModel();
            Detail189.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail189.Page = 2;

            Detail189.Name = PAPERMAIN_TYPE.DETAIL;
            Detail189.SubName = PAPERSUB_TYPE.RafterSideClipDetail;
            Detail189.TitleName = "RAFTER SIDE CLIP DETAIL";
            Detail189.TitleSubName = "(SCALE : " + Detail189.ScaleValue + ")";
            Detail189.IsFix = true;
            Detail189.Row = 3;
            Detail189.Column = 3;
            Detail189.RowSpan = 2;
            Detail189.ColumnSpan = 2;
            Detail189.ScaleValue = 0;//Auto Scali
            Detail189.otherWidth = 75; //수정요함
            Detail189.otherHeight = 90; // 수정요함
            Detail189.TitleSubName = "(SCALE : " + Detail189.ScaleValue + ")";// Scale 값 받기 위해 한번 더 기록



            Detail189.ReferencePoint.X = 80000;
            Detail189.ReferencePoint.Y = -200000;
            Detail189.ModelCenterLocation.X = Detail189.ReferencePoint.X;
            Detail189.ModelCenterLocation.Y = Detail189.ReferencePoint.Y;

            Detail189.viewID = 144000 + viewIDNumber++;
            newList.Add(Detail189);




            ///////////////Column Set/////////////////////

            PaperAreaModel Detail190 = new PaperAreaModel();
            Detail190.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail190.Page = 3;

            Detail190.Name = PAPERMAIN_TYPE.DETAIL;
            Detail190.SubName = PAPERSUB_TYPE.SectionAA;
            Detail190.TitleName = "SECTION\"A\"-\"A\"";
            Detail190.TitleSubName = "(1st COLUMN TOP PLATE)";
            Detail190.IsFix = true;
            Detail190.Row = 1;
            Detail190.Column = 1;
            Detail190.ScaleValue = 0; // Auto Scale
            Detail190.otherWidth = 120; //수정 요함
            Detail190.otherHeight = 300; // 수정 요함

            Detail190.ReferencePoint.X = 90000;
            Detail190.ReferencePoint.Y = -200000;
            Detail190.ModelCenterLocation.X = Detail190.ReferencePoint.X;
            Detail190.ModelCenterLocation.Y = Detail190.ReferencePoint.Y;

            Detail190.viewID = 145000 + viewIDNumber++;
            newList.Add(Detail190);

            PaperAreaModel Detail191 = new PaperAreaModel();
            Detail191.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail191.Page = 3;

            Detail191.Name = PAPERMAIN_TYPE.DETAIL;
            Detail191.SubName = PAPERSUB_TYPE.CenterColumnDetail;
            Detail191.TitleName = "CENTER COLUMN DETAIL";
            Detail191.TitleSubName = "";
            Detail191.IsFix = true;
            Detail191.Row = 2;
            Detail191.Column = 1;
            Detail191.RowSpan = 3;
            Detail191.ColumnSpan = 1;
            Detail191.ScaleValue = 0; // Auto Scale

            Detail191.otherWidth = 120; //수정 요함
            Detail191.otherHeight = 260; //수정 요함

            Detail191.ReferencePoint.X = 100000;
            Detail191.ReferencePoint.Y = -200000;
            Detail191.ModelCenterLocation.X = Detail191.ReferencePoint.X;
            Detail191.ModelCenterLocation.Y = Detail191.ReferencePoint.Y;

            Detail191.viewID = 146000 + viewIDNumber++;
            newList.Add(Detail191);

            PaperAreaModel Detail192 = new PaperAreaModel();
            Detail192.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail192.Page = 3;

            Detail192.Name = PAPERMAIN_TYPE.DETAIL;
            Detail192.SubName = PAPERSUB_TYPE.DetailF;
            Detail192.TitleName = "DETAIL\"F\"";
            Detail192.TitleSubName = "";
            Detail192.IsFix = true;
            Detail192.Row = 1;
            Detail192.Column = 2;
            Detail192.ScaleValue = 0; // Auto Scale

            Detail192.otherWidth = 100;
            Detail192.otherHeight = 100;

            Detail192.ReferencePoint.X = 110000;
            Detail192.ReferencePoint.Y = -200000;
            Detail192.ModelCenterLocation.X = Detail192.ReferencePoint.X;
            Detail192.ModelCenterLocation.Y = Detail192.ReferencePoint.Y;

            Detail192.viewID = 147000 + viewIDNumber++;
            newList.Add(Detail192);

            PaperAreaModel Detail193 = new PaperAreaModel();
            Detail193.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail193.Page = 3;

            Detail193.Name = PAPERMAIN_TYPE.DETAIL;
            Detail193.SubName = PAPERSUB_TYPE.DrainDetail;
            Detail193.TitleName = "DRAIN DETAIL";
            Detail193.TitleSubName = "(SECTION \"G\")";
            Detail193.IsFix = true;
            Detail193.Row = 1;
            Detail193.Column = 3;
            Detail193.ScaleValue = 0; // Auto Scale

            Detail193.otherWidth = 100;
            Detail193.otherHeight = 100;

            Detail193.ReferencePoint.X = 120000;
            Detail193.ReferencePoint.Y = -200000;
            Detail193.ModelCenterLocation.X = Detail193.ReferencePoint.X;
            Detail193.ModelCenterLocation.Y = Detail193.ReferencePoint.Y;

            Detail193.viewID = 148000 + viewIDNumber++;
            newList.Add(Detail193);

            PaperAreaModel Detail194 = new PaperAreaModel();
            Detail194.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail194.Page = 3;

            Detail194.Name = PAPERMAIN_TYPE.DETAIL;
            Detail194.SubName = PAPERSUB_TYPE.SectionBB;
            Detail194.TitleName = "SECTION \"B\"-\"B\"";
            Detail194.TitleSubName = "";
            Detail194.IsFix = true;
            Detail194.Row = 2;
            Detail194.Column = 2;
            Detail194.ScaleValue = 0; //Auto Scale

            Detail194.otherWidth = 120;
            Detail194.otherHeight = 120;

            Detail194.ReferencePoint.X = 130000;
            Detail194.ReferencePoint.Y = -200000;
            Detail194.ModelCenterLocation.X = Detail194.ReferencePoint.X;
            Detail194.ModelCenterLocation.Y = Detail194.ReferencePoint.Y;

            Detail194.viewID = 149000 + viewIDNumber++;
            newList.Add(Detail194);

            PaperAreaModel Detail195 = new PaperAreaModel();
            Detail195.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail195.Page = 3;

            Detail195.Name = PAPERMAIN_TYPE.DETAIL;
            Detail195.SubName = PAPERSUB_TYPE.DetailC;
            Detail195.TitleName = "DETAIL \"C\"";
            Detail195.TitleSubName = "";
            Detail195.IsFix = true;
            Detail195.Row = 2;
            Detail195.Column = 3;
            Detail195.ScaleValue = 0; //Auto Scale

            Detail195.otherWidth = 120;
            Detail195.otherHeight = 120;

            Detail195.ReferencePoint.X = 140000;
            Detail195.ReferencePoint.Y = -200000;
            Detail195.ModelCenterLocation.X = Detail195.ReferencePoint.X;
            Detail195.ModelCenterLocation.Y = Detail195.ReferencePoint.Y;

            Detail195.viewID = 150000 + viewIDNumber++;
            newList.Add(Detail195);



            PaperAreaModel Detail196 = new PaperAreaModel();
            Detail196.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail196.Page = 3;

            Detail196.Name = PAPERMAIN_TYPE.DETAIL;
            Detail196.SubName = PAPERSUB_TYPE.SectionEE;
            Detail196.TitleName = "SECTION \"E\"-\"E\"";
            Detail196.TitleSubName = "";
            Detail196.IsFix = true;
            Detail196.Row = 3;
            Detail196.Column = 2;
            Detail196.ScaleValue = 0; //Auto Scale

            Detail196.otherWidth = 120;
            Detail196.otherHeight = 120;

            Detail196.ReferencePoint.X = 150000;
            Detail196.ReferencePoint.Y = -200000;
            Detail196.ModelCenterLocation.X = Detail196.ReferencePoint.X;
            Detail196.ModelCenterLocation.Y = Detail196.ReferencePoint.Y;

            Detail196.viewID = 151000 + viewIDNumber++;
            newList.Add(Detail196);


            PaperAreaModel Detail197 = new PaperAreaModel();
            Detail197.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail197.Page = 3;

            Detail197.Name = PAPERMAIN_TYPE.DETAIL;
            Detail197.SubName = PAPERSUB_TYPE.DetailD;
            Detail197.TitleName = "DETAIL \"D\"";
            Detail197.TitleSubName = "";
            Detail197.IsFix = true;
            Detail197.Row = 3;
            Detail197.Column = 3;
            Detail197.ScaleValue = 1;


            Detail197.ReferencePoint.X = 160000;
            Detail197.ReferencePoint.Y = -200000;
            Detail197.ModelCenterLocation.X = Detail197.ReferencePoint.X;
            Detail197.ModelCenterLocation.Y = Detail197.ReferencePoint.Y;

            Detail197.viewID = 152000 + viewIDNumber++;
            newList.Add(Detail197);


            //////////////////Column Set///////////////////////

            PaperAreaModel Detail198 = new PaperAreaModel();
            Detail198.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail198.Page = 4;

            Detail198.Name = PAPERMAIN_TYPE.DETAIL;
            Detail198.SubName = PAPERSUB_TYPE.SectionAA;
            Detail198.TitleName = "SECTION\"A\"-\"A\"";
            Detail198.TitleSubName = "(2nd COLUMN TOP PLATE)";
            Detail198.IsFix = true;
            Detail198.Row = 1;
            Detail198.Column = 1;
            Detail198.ScaleValue = 0; // Auto Scale
            Detail198.otherWidth = 120; //수정 요함
            Detail198.otherHeight = 300; // 수정 요함

            Detail198.ReferencePoint.X = 170000;
            Detail198.ReferencePoint.Y = -200000;
            Detail198.ModelCenterLocation.X = Detail198.ReferencePoint.X;
            Detail198.ModelCenterLocation.Y = Detail198.ReferencePoint.Y;

            Detail198.viewID = 153000 + viewIDNumber++;
            newList.Add(Detail198);

            PaperAreaModel Detail199 = new PaperAreaModel();
            Detail199.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail199.Page = 4;

            Detail199.Name = PAPERMAIN_TYPE.DETAIL;
            Detail199.SubName = PAPERSUB_TYPE.CenterColumnDetail;
            Detail199.TitleName = "(C2)SIDE COLUMN DETAIL"; //C1~C6 적용 가능??
            Detail199.TitleSubName = "";
            Detail199.IsFix = true;
            Detail199.Row = 2;
            Detail199.Column = 1;
            Detail199.RowSpan = 3;
            Detail199.ColumnSpan = 1;
            Detail199.ScaleValue = 0; // Auto Scale

            Detail199.otherWidth = 120; //수정 요함
            Detail199.otherHeight = 260; //수정 요함

            Detail199.ReferencePoint.X = 180000;
            Detail199.ReferencePoint.Y = -200000;
            Detail199.ModelCenterLocation.X = Detail199.ReferencePoint.X;
            Detail199.ModelCenterLocation.Y = Detail199.ReferencePoint.Y;

            Detail199.viewID = 154000 + viewIDNumber++;
            newList.Add(Detail199);

            PaperAreaModel Detail200 = new PaperAreaModel();
            Detail200.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail200.Page = 4;

            Detail200.Name = PAPERMAIN_TYPE.DETAIL;
            Detail200.SubName = PAPERSUB_TYPE.GirderBracketDetail;
            Detail200.TitleName = "GIRDER BLACKET DETAIL";
            Detail200.TitleSubName = "";
            Detail200.IsFix = true;
            Detail200.Row = 1;
            Detail200.Column = 2;
            Detail200.ScaleValue = 0; // Auto Scale

            Detail200.otherWidth = 120;
            Detail200.otherHeight = 120;

            Detail200.ReferencePoint.X = 190000;
            Detail200.ReferencePoint.Y = -200000;
            Detail200.ModelCenterLocation.X = Detail200.ReferencePoint.X;
            Detail200.ModelCenterLocation.Y = Detail200.ReferencePoint.Y;

            Detail200.viewID = 155000 + viewIDNumber++;
            newList.Add(Detail200);

            PaperAreaModel Detail201 = new PaperAreaModel();
            Detail201.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail201.Page = 4;

            Detail201.Name = PAPERMAIN_TYPE.DETAIL;
            Detail201.SubName = PAPERSUB_TYPE.DrainDetail;
            Detail201.TitleName = "DRAIN DETAIL";
            Detail201.TitleSubName = "(SECTION \"G\")";
            Detail201.IsFix = true;
            Detail201.Row = 1;
            Detail201.Column = 3;
            Detail201.ScaleValue = 0; // Auto Scale

            Detail201.otherWidth = 100;
            Detail201.otherHeight = 100;

            Detail201.ReferencePoint.X = 200000;
            Detail201.ReferencePoint.Y = -200000;
            Detail201.ModelCenterLocation.X = Detail201.ReferencePoint.X;
            Detail201.ModelCenterLocation.Y = Detail201.ReferencePoint.Y;

            Detail201.viewID = 156000 + viewIDNumber++;
            newList.Add(Detail201);

            PaperAreaModel Detail202 = new PaperAreaModel();
            Detail202.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail202.Page = 4;

            Detail202.Name = PAPERMAIN_TYPE.DETAIL;
            Detail202.SubName = PAPERSUB_TYPE.SectionBB;
            Detail202.TitleName = "SECTION \"B\"-\"B\"";
            Detail202.TitleSubName = "";
            Detail202.IsFix = true;
            Detail202.Row = 2;
            Detail202.Column = 2;
            Detail202.ScaleValue = 0; //Auto Scale

            Detail202.otherWidth = 120;
            Detail202.otherHeight = 120;

            Detail202.ReferencePoint.X = 210000;
            Detail202.ReferencePoint.Y = -200000;
            Detail202.ModelCenterLocation.X = Detail202.ReferencePoint.X;
            Detail202.ModelCenterLocation.Y = Detail202.ReferencePoint.Y;

            Detail202.viewID = 157000 + viewIDNumber++;
            newList.Add(Detail202);

            PaperAreaModel Detail203 = new PaperAreaModel();
            Detail203.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail203.Page = 4;

            Detail203.Name = PAPERMAIN_TYPE.DETAIL;
            Detail203.SubName = PAPERSUB_TYPE.DetailC;
            Detail203.TitleName = "DETAIL \"C\"";
            Detail203.TitleSubName = "";
            Detail203.IsFix = true;
            Detail203.Row = 2;
            Detail203.Column = 3;
            Detail203.ScaleValue = 0; //Auto Scale

            Detail203.otherWidth = 120;
            Detail203.otherHeight = 120;

            Detail203.ReferencePoint.X = 220000;
            Detail203.ReferencePoint.Y = -200000;
            Detail203.ModelCenterLocation.X = Detail203.ReferencePoint.X;
            Detail203.ModelCenterLocation.Y = Detail203.ReferencePoint.Y;

            Detail203.viewID = 158000 + viewIDNumber++;
            newList.Add(Detail203);



            PaperAreaModel Detail204 = new PaperAreaModel();
            Detail204.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail204.Page = 4;

            Detail204.Name = PAPERMAIN_TYPE.DETAIL;
            Detail204.SubName = PAPERSUB_TYPE.SectionEE;
            Detail204.TitleName = "SECTION \"E\"-\"E\"";
            Detail204.TitleSubName = "";
            Detail204.IsFix = true;
            Detail204.Row = 3;
            Detail204.Column = 2;
            Detail204.ScaleValue = 0; //Auto Scale

            Detail204.otherWidth = 120;
            Detail204.otherHeight = 120;

            Detail204.ReferencePoint.X = 230000;
            Detail204.ReferencePoint.Y = -200000;
            Detail204.ModelCenterLocation.X = Detail204.ReferencePoint.X;
            Detail204.ModelCenterLocation.Y = Detail204.ReferencePoint.Y;

            Detail204.viewID = 159000 + viewIDNumber++;
            newList.Add(Detail204);


            PaperAreaModel Detail205 = new PaperAreaModel();
            Detail205.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail205.Page = 4;

            Detail205.Name = PAPERMAIN_TYPE.DETAIL;
            Detail205.SubName = PAPERSUB_TYPE.DetailD;
            Detail205.TitleName = "DETAIL \"D\"";
            Detail205.TitleSubName = "";
            Detail205.IsFix = true;
            Detail205.Row = 3;
            Detail205.Column = 3;
            Detail205.ScaleValue = 1;


            Detail205.ReferencePoint.X = 240000;
            Detail205.ReferencePoint.Y = -200000;
            Detail205.ModelCenterLocation.X = Detail205.ReferencePoint.X;
            Detail205.ModelCenterLocation.Y = Detail205.ReferencePoint.Y;

            Detail205.viewID = 160000 + viewIDNumber++;
            newList.Add(Detail205);






            return newList;

        }
        public List<PaperAreaModel> GatPaperAreaCRTStructureColumn03() //LayerCount 3~4 
        {
            double viewIDNumber = 21000;

            List<PaperAreaModel> newList = new List<PaperAreaModel>();

            PaperAreaModel Detail206 = new PaperAreaModel();
            Detail206.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail206.Page = 1;

            Detail206.Name = PAPERMAIN_TYPE.DETAIL;
            Detail206.SubName = PAPERSUB_TYPE.RoofStructureAssembly;
            Detail206.TitleName = "ROOF STRUCTURE ASSEMBLY";
            Detail206.TitleSubName = "";
            Detail206.IsFix = true;
            Detail206.Row = 1;
            Detail206.Column = 1;
            Detail206.ScaleValue = 0; // Auto Scale
            Detail206.otherWidth = 570;
            Detail206.otherHeight = 440;

            Detail206.ReferencePoint.X = 100000;
            Detail206.ReferencePoint.Y = -100000;
            Detail206.ModelCenterLocation.X = Detail206.ReferencePoint.X;
            Detail206.ModelCenterLocation.Y = Detail206.ReferencePoint.Y;

            Detail206.viewID = 161000 + viewIDNumber++;
            newList.Add(Detail206);

            PaperAreaModel Detail207 = new PaperAreaModel();
            Detail207.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail207.Page = 2;

            Detail207.Name = PAPERMAIN_TYPE.DETAIL;
            Detail207.SubName = PAPERSUB_TYPE.RoofStructureOrientation;
            Detail207.TitleName = "ROOF STRUCTURE ORIENTATION";
            Detail207.TitleSubName = "";
            Detail207.IsFix = true;
            Detail207.Row = 1;
            Detail207.Column = 1;
            Detail207.ScaleValue = 0; // Auto Scale
            Detail207.otherWidth = 570; // 수정요함
            Detail207.otherHeight = 440; // 수정요함

            Detail207.ReferencePoint.X = 200000;
            Detail207.ReferencePoint.Y = -100000;
            Detail207.ModelCenterLocation.X = Detail207.ReferencePoint.X;
            Detail207.ModelCenterLocation.Y = Detail207.ReferencePoint.Y;

            Detail207.viewID = 162000 + viewIDNumber++;
            newList.Add(Detail207);

            PaperAreaModel Detail208 = new PaperAreaModel();
            Detail208.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail208.Page = 3;

            Detail208.Name = PAPERMAIN_TYPE.DETAIL;
            Detail208.SubName = PAPERSUB_TYPE.Girder;
            Detail208.TitleName = "GIRDER"; // Girder 번호 적용 필요
            Detail208.TitleSubName = "(H300x300x10x15)"; //Girder 정보 불러와야 함
            Detail208.IsFix = false;
            Detail208.RowSpan = 1;
            Detail208.ColumnSpan = 3;
            Detail208.ScaleValue = 0;//Auto Scali
            Detail208.IsRepeat = true;

            Detail208.otherWidth = 400; //수정요함
            Detail208.otherHeight = 120; // 수정요함



            Detail208.ReferencePoint.X = 10000;
            Detail208.ReferencePoint.Y = -200000;
            Detail208.ModelCenterLocation.X = Detail208.ReferencePoint.X;
            Detail208.ModelCenterLocation.Y = Detail208.ReferencePoint.Y;

            Detail208.viewID = 163000 + viewIDNumber++;
            newList.Add(Detail208);

            PaperAreaModel Detail209 = new PaperAreaModel();
            Detail209.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail209.Page = 3;

            Detail209.Name = PAPERMAIN_TYPE.DETAIL;
            Detail209.SubName = PAPERSUB_TYPE.BracketDetail; ;
            Detail209.TitleName = "BRACKET DETAIL";
            Detail209.TitleSubName = "(SEE TABLE1)";
            Detail209.IsFix = true;
            Detail209.Row = 1;
            Detail209.Column = 4;
            Detail209.ScaleValue = 0;//Auto Scali
            Detail209.otherWidth = 120; //수정요함
            Detail209.otherHeight = 120; // 수정요함

            Detail209.ReferencePoint.X = 20000;
            Detail209.ReferencePoint.Y = -200000;
            Detail209.ModelCenterLocation.X = Detail209.ReferencePoint.X;
            Detail209.ModelCenterLocation.Y = Detail209.ReferencePoint.Y;

            Detail209.viewID = 164000 + viewIDNumber++;
            newList.Add(Detail209);


            PaperAreaModel Detail210 = new PaperAreaModel();
            Detail210.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail210.Page = 3;

            Detail210.Name = PAPERMAIN_TYPE.DETAIL;
            Detail210.SubName = PAPERSUB_TYPE.Table1; ;
            Detail210.TitleName = "TABLE1";
            Detail210.TitleSubName = "";
            Detail210.IsFix = true;
            Detail210.Row = 2;
            Detail210.Column = 4;
            Detail210.ScaleValue = 1;

            Detail210.ReferencePoint.X = 30000;
            Detail210.ReferencePoint.Y = -200000;
            Detail210.ModelCenterLocation.X = Detail210.ReferencePoint.X;
            Detail210.ModelCenterLocation.Y = Detail210.ReferencePoint.Y;

            Detail210.viewID = 165000 + viewIDNumber++;
            newList.Add(Detail210);


            PaperAreaModel Detail211 = new PaperAreaModel();
            Detail211.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail211.Page = 3;

            Detail211.Name = PAPERMAIN_TYPE.DETAIL;
            Detail211.SubName = PAPERSUB_TYPE.DetailA;
            Detail211.TitleName = "DETAIL \"A\"";
            Detail211.TitleSubName = "(TYP.)";
            Detail211.IsFix = false;
            Detail211.ScaleValue = 0; //Auto Scale

            Detail211.ReferencePoint.X = 40000;
            Detail211.ReferencePoint.Y = -200000;
            Detail211.ModelCenterLocation.X = Detail211.ReferencePoint.X;
            Detail211.ModelCenterLocation.Y = Detail211.ReferencePoint.Y;

            Detail211.viewID = 166000 + viewIDNumber++;
            newList.Add(Detail211);


            PaperAreaModel Detail212 = new PaperAreaModel();
            Detail212.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail212.Page = 3;

            Detail212.Name = PAPERMAIN_TYPE.DETAIL;
            Detail212.SubName = PAPERSUB_TYPE.Rafter;
            Detail212.TitleName = "RAFTER"; //Rafter 번호 표기 요함
            Detail212.TitleSubName = "(ㄷ200x80x7.35x11)"; //Rafter 정보 수정요함
            Detail212.IsFix = false;
            Detail212.RowSpan = 1;
            Detail212.ColumnSpan = 2;
            Detail212.ScaleValue = 0; // Auto Scale
            Detail212.IsRepeat = true;

            Detail212.otherWidth = 200;
            Detail212.otherHeight = 100;

            Detail212.ReferencePoint.X = 50000;
            Detail212.ReferencePoint.Y = -200000;
            Detail212.ModelCenterLocation.X = Detail212.ReferencePoint.X;
            Detail212.ModelCenterLocation.Y = Detail212.ReferencePoint.Y;

            Detail212.viewID = 167000 + viewIDNumber++;
            newList.Add(Detail212);


            PaperAreaModel Detail213 = new PaperAreaModel();
            Detail213.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail213.Page = 2;

            Detail213.Name = PAPERMAIN_TYPE.DETAIL;
            Detail213.SubName = PAPERSUB_TYPE.RafterSideClipDetail;
            Detail213.TitleName = "RAFTER SIDE CLIP DETAIL";
            Detail213.TitleSubName = "(SCALE : " + Detail213.ScaleValue + ")";
            Detail213.IsFix = true;
            Detail213.Row = 1;
            Detail213.Column = 3;
            Detail213.RowSpan = 2;
            Detail213.ColumnSpan = 2;
            Detail213.ScaleValue = 0;//Auto Scali
            Detail213.otherWidth = 75; //수정요함
            Detail213.otherHeight = 90; // 수정요함
            Detail213.TitleSubName = "(SCALE : " + Detail213.ScaleValue + ")";// Scale 값 받기 위해 한번 더 기록



            Detail213.ReferencePoint.X = 60000;
            Detail213.ReferencePoint.Y = -200000;
            Detail213.ModelCenterLocation.X = Detail213.ReferencePoint.X;
            Detail213.ModelCenterLocation.Y = Detail213.ReferencePoint.Y;

            Detail213.viewID = 168000 + viewIDNumber++;
            newList.Add(Detail213);


            PaperAreaModel Detail214 = new PaperAreaModel();
            Detail214.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail214.Page = 3;

            Detail214.Name = PAPERMAIN_TYPE.DETAIL;
            Detail214.SubName = PAPERSUB_TYPE.Table2; ;
            Detail214.TitleName = "TABLE2";
            Detail214.TitleSubName = "";
            Detail214.IsFix = false;
            Detail214.ScaleValue = 1;

            Detail214.ReferencePoint.X = 70000;
            Detail214.ReferencePoint.Y = -200000;
            Detail214.ModelCenterLocation.X = Detail214.ReferencePoint.X;
            Detail214.ModelCenterLocation.Y = Detail214.ReferencePoint.Y;

            Detail214.viewID = 169000 + viewIDNumber++;
            newList.Add(Detail214);


            ///////////////Center Column Set/////////////////////

            PaperAreaModel Detail220 = new PaperAreaModel();
            Detail220.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail220.Page = 4;

            Detail220.Name = PAPERMAIN_TYPE.DETAIL;
            Detail220.SubName = PAPERSUB_TYPE.SectionAA;
            Detail220.TitleName = "SECTION\"A\"-\"A\"";
            Detail220.TitleSubName = "(1st COLUMN TOP PLATE)";
            Detail220.IsFix = true;
            Detail220.Row = 1;
            Detail220.Column = 1;
            Detail220.ScaleValue = 0; // Auto Scale
            Detail220.otherWidth = 120; //수정 요함
            Detail220.otherHeight = 300; // 수정 요함

            Detail220.ReferencePoint.X = 80000;
            Detail220.ReferencePoint.Y = -200000;
            Detail220.ModelCenterLocation.X = Detail220.ReferencePoint.X;
            Detail220.ModelCenterLocation.Y = Detail220.ReferencePoint.Y;

            Detail220.viewID = 170000 + viewIDNumber++;
            newList.Add(Detail220);

            PaperAreaModel Detail221 = new PaperAreaModel();
            Detail221.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail221.Page = 4;

            Detail221.Name = PAPERMAIN_TYPE.DETAIL;
            Detail221.SubName = PAPERSUB_TYPE.CenterColumnDetail;
            Detail221.TitleName = "CENTER COLUMN DETAIL";
            Detail221.TitleSubName = "";
            Detail221.IsFix = true;
            Detail221.Row = 2;
            Detail221.Column = 1;
            Detail221.RowSpan = 3;
            Detail221.ColumnSpan = 1;
            Detail221.ScaleValue = 0; // Auto Scale

            Detail221.otherWidth = 120; //수정 요함
            Detail221.otherHeight = 260; //수정 요함

            Detail221.ReferencePoint.X = 100000;
            Detail221.ReferencePoint.Y = -200000;
            Detail221.ModelCenterLocation.X = Detail221.ReferencePoint.X;
            Detail221.ModelCenterLocation.Y = Detail221.ReferencePoint.Y;

            Detail221.viewID = 171000 + viewIDNumber++;
            newList.Add(Detail221);

            PaperAreaModel Detail222 = new PaperAreaModel();
            Detail222.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail222.Page = 4;

            Detail222.Name = PAPERMAIN_TYPE.DETAIL;
            Detail222.SubName = PAPERSUB_TYPE.DetailF;
            Detail222.TitleName = "DETAIL\"F\"";
            Detail222.TitleSubName = "";
            Detail222.IsFix = true;
            Detail222.Row = 1;
            Detail222.Column = 2;
            Detail222.ScaleValue = 0; // Auto Scale

            Detail222.otherWidth = 100;
            Detail222.otherHeight = 100;

            Detail222.ReferencePoint.X = 110000;
            Detail222.ReferencePoint.Y = -200000;
            Detail222.ModelCenterLocation.X = Detail222.ReferencePoint.X;
            Detail222.ModelCenterLocation.Y = Detail222.ReferencePoint.Y;

            Detail222.viewID = 172000 + viewIDNumber++;
            newList.Add(Detail222);

            PaperAreaModel Detail223 = new PaperAreaModel();
            Detail223.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail223.Page = 4;

            Detail223.Name = PAPERMAIN_TYPE.DETAIL;
            Detail223.SubName = PAPERSUB_TYPE.DrainDetail;
            Detail223.TitleName = "DRAIN DETAIL";
            Detail223.TitleSubName = "(SECTION \"G\")";
            Detail223.IsFix = true;
            Detail223.Row = 1;
            Detail223.Column = 3;
            Detail223.ScaleValue = 0; // Auto Scale

            Detail223.otherWidth = 100;
            Detail223.otherHeight = 100;

            Detail223.ReferencePoint.X = 120000;
            Detail223.ReferencePoint.Y = -200000;
            Detail223.ModelCenterLocation.X = Detail223.ReferencePoint.X;
            Detail223.ModelCenterLocation.Y = Detail223.ReferencePoint.Y;

            Detail223.viewID = 173000 + viewIDNumber++;
            newList.Add(Detail223);

            PaperAreaModel Detail224 = new PaperAreaModel();
            Detail224.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail224.Page = 4;

            Detail224.Name = PAPERMAIN_TYPE.DETAIL;
            Detail224.SubName = PAPERSUB_TYPE.SectionBB;
            Detail224.TitleName = "SECTION \"B\"-\"B\"";
            Detail224.TitleSubName = "";
            Detail224.IsFix = true;
            Detail224.Row = 2;
            Detail224.Column = 2;
            Detail224.ScaleValue = 0; //Auto Scale

            Detail224.otherWidth = 120;
            Detail224.otherHeight = 120;

            Detail224.ReferencePoint.X = 130000;
            Detail224.ReferencePoint.Y = -200000;
            Detail224.ModelCenterLocation.X = Detail224.ReferencePoint.X;
            Detail224.ModelCenterLocation.Y = Detail224.ReferencePoint.Y;

            Detail224.viewID = 174000 + viewIDNumber++;
            newList.Add(Detail224);

            PaperAreaModel Detail225 = new PaperAreaModel();
            Detail225.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail225.Page = 4;

            Detail225.Name = PAPERMAIN_TYPE.DETAIL;
            Detail225.SubName = PAPERSUB_TYPE.DetailC;
            Detail225.TitleName = "DETAIL \"C\"";
            Detail225.TitleSubName = "";
            Detail225.IsFix = true;
            Detail225.Row = 2;
            Detail225.Column = 3;
            Detail225.ScaleValue = 0; //Auto Scale

            Detail225.otherWidth = 120;
            Detail225.otherHeight = 120;

            Detail225.ReferencePoint.X = 140000;
            Detail225.ReferencePoint.Y = -200000;
            Detail225.ModelCenterLocation.X = Detail225.ReferencePoint.X;
            Detail225.ModelCenterLocation.Y = Detail225.ReferencePoint.Y;

            Detail225.viewID = 175000 + viewIDNumber++;
            newList.Add(Detail225);



            PaperAreaModel Detail226 = new PaperAreaModel();
            Detail226.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail226.Page = 4;

            Detail226.Name = PAPERMAIN_TYPE.DETAIL;
            Detail226.SubName = PAPERSUB_TYPE.SectionEE;
            Detail226.TitleName = "SECTION \"E\"-\"E\"";
            Detail226.TitleSubName = "";
            Detail226.IsFix = true;
            Detail226.Row = 3;
            Detail226.Column = 2;
            Detail226.ScaleValue = 0; //Auto Scale

            Detail226.otherWidth = 120;
            Detail226.otherHeight = 120;

            Detail226.ReferencePoint.X = 150000;
            Detail226.ReferencePoint.Y = -200000;
            Detail226.ModelCenterLocation.X = Detail226.ReferencePoint.X;
            Detail226.ModelCenterLocation.Y = Detail226.ReferencePoint.Y;

            Detail226.viewID = 176000 + viewIDNumber++;
            newList.Add(Detail226);


            PaperAreaModel Detail227 = new PaperAreaModel();
            Detail227.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail227.Page = 4;

            Detail227.Name = PAPERMAIN_TYPE.DETAIL;
            Detail227.SubName = PAPERSUB_TYPE.DetailD;
            Detail227.TitleName = "DETAIL \"D\"";
            Detail227.TitleSubName = "";
            Detail227.IsFix = true;
            Detail227.Row = 3;
            Detail227.Column = 3;
            Detail227.ScaleValue = 1;


            Detail227.ReferencePoint.X = 160000;
            Detail227.ReferencePoint.Y = -200000;
            Detail227.ModelCenterLocation.X = Detail227.ReferencePoint.X;
            Detail227.ModelCenterLocation.Y = Detail227.ReferencePoint.Y;

            Detail227.viewID = 177000 + viewIDNumber++;
            newList.Add(Detail227);


            //////////////////Side Column Set///////////////////////

            PaperAreaModel Detail230 = new PaperAreaModel();
            Detail230.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail230.Page = 5;

            Detail230.Name = PAPERMAIN_TYPE.DETAIL;
            Detail230.SubName = PAPERSUB_TYPE.SectionAA;
            Detail230.TitleName = "SECTION\"A\"-\"A\"";
            Detail230.TitleSubName = "(2nd COLUMN TOP PLATE)";
            Detail230.IsFix = true;
            Detail230.Row = 1;
            Detail230.Column = 1;
            Detail230.ScaleValue = 0; // Auto Scale
            Detail230.otherWidth = 120; //수정 요함
            Detail230.otherHeight = 300; // 수정 요함

            Detail230.ReferencePoint.X = 170000;
            Detail230.ReferencePoint.Y = -200000;
            Detail230.ModelCenterLocation.X = Detail230.ReferencePoint.X;
            Detail230.ModelCenterLocation.Y = Detail230.ReferencePoint.Y;

            Detail230.viewID = 178000 + viewIDNumber++;
            newList.Add(Detail230);

            PaperAreaModel Detail231 = new PaperAreaModel();
            Detail231.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail231.Page = 5;

            Detail231.Name = PAPERMAIN_TYPE.DETAIL;
            Detail231.SubName = PAPERSUB_TYPE.CenterColumnDetail;
            Detail231.TitleName = "(C2)SIDE COLUMN DETAIL"; //C1~C6 적용 가능??
            Detail231.TitleSubName = "";
            Detail231.IsFix = true;
            Detail231.Row = 2;
            Detail231.Column = 1;
            Detail231.RowSpan = 3;
            Detail231.ColumnSpan = 1;
            Detail231.ScaleValue = 0; // Auto Scale

            Detail231.otherWidth = 120; //수정 요함
            Detail231.otherHeight = 260; //수정 요함

            Detail231.ReferencePoint.X = 180000;
            Detail231.ReferencePoint.Y = -200000;
            Detail231.ModelCenterLocation.X = Detail231.ReferencePoint.X;
            Detail231.ModelCenterLocation.Y = Detail231.ReferencePoint.Y;

            Detail231.viewID = 179000 + viewIDNumber++;
            newList.Add(Detail231);

            PaperAreaModel Detail232 = new PaperAreaModel();
            Detail232.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail232.Page = 5;

            Detail232.Name = PAPERMAIN_TYPE.DETAIL;
            Detail232.SubName = PAPERSUB_TYPE.GirderBracketDetail;
            Detail232.TitleName = "GIRDER BLACKET DETAIL";
            Detail232.TitleSubName = "";
            Detail232.IsFix = true;
            Detail232.Row = 1;
            Detail232.Column = 2;
            Detail232.ScaleValue = 0; // Auto Scale

            Detail232.otherWidth = 120;
            Detail232.otherHeight = 120;

            Detail232.ReferencePoint.X = 190000;
            Detail232.ReferencePoint.Y = -200000;
            Detail232.ModelCenterLocation.X = Detail232.ReferencePoint.X;
            Detail232.ModelCenterLocation.Y = Detail232.ReferencePoint.Y;

            Detail232.viewID = 180000 + viewIDNumber++;
            newList.Add(Detail232);

            PaperAreaModel Detail234 = new PaperAreaModel();
            Detail234.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail234.Page = 5;

            Detail234.Name = PAPERMAIN_TYPE.DETAIL;
            Detail234.SubName = PAPERSUB_TYPE.DrainDetail;
            Detail234.TitleName = "DRAIN DETAIL";
            Detail234.TitleSubName = "(SECTION \"G\")";
            Detail234.IsFix = true;
            Detail234.Row = 1;
            Detail234.Column = 3;
            Detail234.ScaleValue = 0; // Auto Scale

            Detail234.otherWidth = 100;
            Detail234.otherHeight = 100;

            Detail234.ReferencePoint.X = 200000;
            Detail234.ReferencePoint.Y = -200000;
            Detail234.ModelCenterLocation.X = Detail234.ReferencePoint.X;
            Detail234.ModelCenterLocation.Y = Detail234.ReferencePoint.Y;

            Detail234.viewID = 181000 + viewIDNumber++;
            newList.Add(Detail234);

            PaperAreaModel Detail235 = new PaperAreaModel();
            Detail235.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail235.Page = 5;

            Detail235.Name = PAPERMAIN_TYPE.DETAIL;
            Detail235.SubName = PAPERSUB_TYPE.SectionBB;
            Detail235.TitleName = "SECTION \"B\"-\"B\"";
            Detail235.TitleSubName = "";
            Detail235.IsFix = true;
            Detail235.Row = 2;
            Detail235.Column = 2;
            Detail235.ScaleValue = 0; //Auto Scale

            Detail235.otherWidth = 120;
            Detail235.otherHeight = 120;

            Detail235.ReferencePoint.X = 210000;
            Detail235.ReferencePoint.Y = -200000;
            Detail235.ModelCenterLocation.X = Detail235.ReferencePoint.X;
            Detail235.ModelCenterLocation.Y = Detail235.ReferencePoint.Y;

            Detail235.viewID = 182000 + viewIDNumber++;
            newList.Add(Detail235);

            PaperAreaModel Detail236 = new PaperAreaModel();
            Detail236.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail236.Page = 5;

            Detail236.Name = PAPERMAIN_TYPE.DETAIL;
            Detail236.SubName = PAPERSUB_TYPE.DetailC;
            Detail236.TitleName = "DETAIL \"C\"";
            Detail236.TitleSubName = "";
            Detail236.IsFix = true;
            Detail236.Row = 2;
            Detail236.Column = 3;
            Detail236.ScaleValue = 0; //Auto Scale

            Detail236.otherWidth = 120;
            Detail236.otherHeight = 120;

            Detail236.ReferencePoint.X = 220000;
            Detail236.ReferencePoint.Y = -200000;
            Detail236.ModelCenterLocation.X = Detail236.ReferencePoint.X;
            Detail236.ModelCenterLocation.Y = Detail236.ReferencePoint.Y;

            Detail236.viewID = 183000 + viewIDNumber++;
            newList.Add(Detail236);



            PaperAreaModel Detail237 = new PaperAreaModel();
            Detail237.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail237.Page = 5;

            Detail237.Name = PAPERMAIN_TYPE.DETAIL;
            Detail237.SubName = PAPERSUB_TYPE.SectionEE;
            Detail237.TitleName = "SECTION \"E\"-\"E\"";
            Detail237.TitleSubName = "";
            Detail237.IsFix = true;
            Detail237.Row = 3;
            Detail237.Column = 2;
            Detail237.ScaleValue = 0; //Auto Scale

            Detail237.otherWidth = 120;
            Detail237.otherHeight = 120;

            Detail237.ReferencePoint.X = 230000;
            Detail237.ReferencePoint.Y = -200000;
            Detail237.ModelCenterLocation.X = Detail237.ReferencePoint.X;
            Detail237.ModelCenterLocation.Y = Detail237.ReferencePoint.Y;

            Detail237.viewID = 184000 + viewIDNumber++;
            newList.Add(Detail237);


            PaperAreaModel Detail238 = new PaperAreaModel();
            Detail238.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail238.Page = 5;

            Detail238.Name = PAPERMAIN_TYPE.DETAIL;
            Detail238.SubName = PAPERSUB_TYPE.DetailD;
            Detail238.TitleName = "DETAIL \"D\"";
            Detail238.TitleSubName = "";
            Detail238.IsFix = true;
            Detail238.Row = 3;
            Detail238.Column = 3;
            Detail238.ScaleValue = 1;


            Detail238.ReferencePoint.X = 240000;
            Detail238.ReferencePoint.Y = -200000;
            Detail238.ModelCenterLocation.X = Detail238.ReferencePoint.X;
            Detail238.ModelCenterLocation.Y = Detail238.ReferencePoint.Y;

            Detail238.viewID = 185000 + viewIDNumber++;
            newList.Add(Detail238);

            //////////////////Side Column Set///////////////////////

            PaperAreaModel Detail240 = new PaperAreaModel();
            Detail240.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail240.Page = 6;

            Detail240.Name = PAPERMAIN_TYPE.DETAIL;
            Detail240.SubName = PAPERSUB_TYPE.SectionAA;
            Detail240.TitleName = "SECTION\"A\"-\"A\"";
            Detail240.TitleSubName = "(2nd COLUMN TOP PLATE)";
            Detail240.IsFix = true;
            Detail240.Row = 1;
            Detail240.Column = 1;
            Detail240.ScaleValue = 0; // Auto Scale
            Detail240.otherWidth = 120; //수정 요함
            Detail240.otherHeight = 300; // 수정 요함

            Detail240.ReferencePoint.X = 250000;
            Detail240.ReferencePoint.Y = -200000;
            Detail240.ModelCenterLocation.X = Detail240.ReferencePoint.X;
            Detail240.ModelCenterLocation.Y = Detail240.ReferencePoint.Y;

            Detail240.viewID = 186000 + viewIDNumber++;
            newList.Add(Detail240);

            PaperAreaModel Detail241 = new PaperAreaModel();
            Detail241.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail241.Page = 6;

            Detail241.Name = PAPERMAIN_TYPE.DETAIL;
            Detail241.SubName = PAPERSUB_TYPE.CenterColumnDetail;
            Detail241.TitleName = "(C2)SIDE COLUMN DETAIL"; //C1~C6 적용 가능??
            Detail241.TitleSubName = "";
            Detail241.IsFix = true;
            Detail241.Row = 2;
            Detail241.Column = 1;
            Detail241.RowSpan = 3;
            Detail241.ColumnSpan = 1;
            Detail241.ScaleValue = 0; // Auto Scale

            Detail241.otherWidth = 120; //수정 요함
            Detail241.otherHeight = 260; //수정 요함

            Detail241.ReferencePoint.X = 260000;
            Detail241.ReferencePoint.Y = -200000;
            Detail241.ModelCenterLocation.X = Detail241.ReferencePoint.X;
            Detail241.ModelCenterLocation.Y = Detail241.ReferencePoint.Y;

            Detail241.viewID = 187000 + viewIDNumber++;
            newList.Add(Detail241);

            PaperAreaModel Detail242 = new PaperAreaModel();
            Detail242.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail242.Page = 6;

            Detail242.Name = PAPERMAIN_TYPE.DETAIL;
            Detail242.SubName = PAPERSUB_TYPE.GirderBracketDetail;
            Detail242.TitleName = "GIRDER BLACKET DETAIL";
            Detail242.TitleSubName = "";
            Detail242.IsFix = true;
            Detail242.Row = 1;
            Detail242.Column = 2;
            Detail242.ScaleValue = 0; // Auto Scale

            Detail242.otherWidth = 120;
            Detail242.otherHeight = 120;

            Detail242.ReferencePoint.X = 270000;
            Detail242.ReferencePoint.Y = -200000;
            Detail242.ModelCenterLocation.X = Detail242.ReferencePoint.X;
            Detail242.ModelCenterLocation.Y = Detail242.ReferencePoint.Y;

            Detail242.viewID = 188000 + viewIDNumber++;
            newList.Add(Detail242);

            PaperAreaModel Detail244 = new PaperAreaModel();
            Detail244.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail244.Page = 6;

            Detail244.Name = PAPERMAIN_TYPE.DETAIL;
            Detail244.SubName = PAPERSUB_TYPE.DrainDetail;
            Detail244.TitleName = "DRAIN DETAIL";
            Detail244.TitleSubName = "(SECTION \"G\")";
            Detail244.IsFix = true;
            Detail244.Row = 1;
            Detail244.Column = 3;
            Detail244.ScaleValue = 0; // Auto Scale

            Detail244.otherWidth = 100;
            Detail244.otherHeight = 100;

            Detail244.ReferencePoint.X = 280000;
            Detail244.ReferencePoint.Y = -200000;
            Detail244.ModelCenterLocation.X = Detail244.ReferencePoint.X;
            Detail244.ModelCenterLocation.Y = Detail244.ReferencePoint.Y;

            Detail244.viewID = 189000 + viewIDNumber++;
            newList.Add(Detail244);

            PaperAreaModel Detail245 = new PaperAreaModel();
            Detail245.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail245.Page = 6;

            Detail245.Name = PAPERMAIN_TYPE.DETAIL;
            Detail245.SubName = PAPERSUB_TYPE.SectionBB;
            Detail245.TitleName = "SECTION \"B\"-\"B\"";
            Detail245.TitleSubName = "";
            Detail245.IsFix = true;
            Detail245.Row = 2;
            Detail245.Column = 2;
            Detail245.ScaleValue = 0; //Auto Scale

            Detail245.otherWidth = 120;
            Detail245.otherHeight = 120;

            Detail245.ReferencePoint.X = 290000;
            Detail245.ReferencePoint.Y = -200000;
            Detail245.ModelCenterLocation.X = Detail245.ReferencePoint.X;
            Detail245.ModelCenterLocation.Y = Detail245.ReferencePoint.Y;

            Detail245.viewID = 190000 + viewIDNumber++;
            newList.Add(Detail245);

            PaperAreaModel Detail246 = new PaperAreaModel();
            Detail246.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail246.Page = 6;

            Detail246.Name = PAPERMAIN_TYPE.DETAIL;
            Detail246.SubName = PAPERSUB_TYPE.DetailC;
            Detail246.TitleName = "DETAIL \"C\"";
            Detail246.TitleSubName = "";
            Detail246.IsFix = true;
            Detail246.Row = 2;
            Detail246.Column = 3;
            Detail246.ScaleValue = 0; //Auto Scale

            Detail246.otherWidth = 120;
            Detail246.otherHeight = 120;

            Detail246.ReferencePoint.X = 300000;
            Detail246.ReferencePoint.Y = -200000;
            Detail246.ModelCenterLocation.X = Detail246.ReferencePoint.X;
            Detail246.ModelCenterLocation.Y = Detail246.ReferencePoint.Y;

            Detail246.viewID = 200000 + viewIDNumber++;
            newList.Add(Detail246);



            PaperAreaModel Detail247 = new PaperAreaModel();
            Detail247.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail247.Page = 6;

            Detail247.Name = PAPERMAIN_TYPE.DETAIL;
            Detail247.SubName = PAPERSUB_TYPE.SectionEE;
            Detail247.TitleName = "SECTION \"E\"-\"E\"";
            Detail247.TitleSubName = "";
            Detail247.IsFix = true;
            Detail247.Row = 3;
            Detail247.Column = 2;
            Detail247.ScaleValue = 0; //Auto Scale

            Detail247.otherWidth = 120;
            Detail247.otherHeight = 120;

            Detail247.ReferencePoint.X = 310000;
            Detail247.ReferencePoint.Y = -200000;
            Detail247.ModelCenterLocation.X = Detail247.ReferencePoint.X;
            Detail247.ModelCenterLocation.Y = Detail247.ReferencePoint.Y;

            Detail247.viewID = 201000 + viewIDNumber++;
            newList.Add(Detail247);


            PaperAreaModel Detail248 = new PaperAreaModel();
            Detail248.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail248.Page = 6;

            Detail248.Name = PAPERMAIN_TYPE.DETAIL;
            Detail248.SubName = PAPERSUB_TYPE.DetailD;
            Detail248.TitleName = "DETAIL \"D\"";
            Detail248.TitleSubName = "";
            Detail248.IsFix = true;
            Detail248.Row = 3;
            Detail248.Column = 3;
            Detail248.ScaleValue = 1;


            Detail248.ReferencePoint.X = 320000;
            Detail248.ReferencePoint.Y = -200000;
            Detail248.ModelCenterLocation.X = Detail248.ReferencePoint.X;
            Detail248.ModelCenterLocation.Y = Detail248.ReferencePoint.Y;

            Detail248.viewID = 202000 + viewIDNumber++;
            newList.Add(Detail248);



            return newList;

        }
        public List<PaperAreaModel> GatPaperAreaCRTStructureColumn04() //LayerCount 5 
        {
            double viewIDNumber = 22000;

            List<PaperAreaModel> newList = new List<PaperAreaModel>();

            PaperAreaModel Detail250 = new PaperAreaModel();
            Detail250.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail250.Page = 1;

            Detail250.Name = PAPERMAIN_TYPE.DETAIL;
            Detail250.SubName = PAPERSUB_TYPE.RoofStructureAssembly;
            Detail250.TitleName = "ROOF STRUCTURE ASSEMBLY";
            Detail250.TitleSubName = "";
            Detail250.IsFix = true;
            Detail250.Row = 1;
            Detail250.Column = 1;
            Detail250.ScaleValue = 0; // Auto Scale
            Detail250.otherWidth = 570;
            Detail250.otherHeight = 440;

            Detail250.ReferencePoint.X = 100000;
            Detail250.ReferencePoint.Y = -100000;
            Detail250.ModelCenterLocation.X = Detail250.ReferencePoint.X;
            Detail250.ModelCenterLocation.Y = Detail250.ReferencePoint.Y;

            Detail250.viewID = 203000 + viewIDNumber++;
            newList.Add(Detail250);

            PaperAreaModel Detail251 = new PaperAreaModel();
            Detail251.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail251.Page = 2;

            Detail251.Name = PAPERMAIN_TYPE.DETAIL;
            Detail251.SubName = PAPERSUB_TYPE.RoofStructureOrientation;
            Detail251.TitleName = "ROOF STRUCTURE ORIENTATION";
            Detail251.TitleSubName = "";
            Detail251.IsFix = true;
            Detail251.Row = 1;
            Detail251.Column = 1;
            Detail251.ScaleValue = 0; // Auto Scale
            Detail251.otherWidth = 570; // 수정요함
            Detail251.otherHeight = 440; // 수정요함

            Detail251.ReferencePoint.X = 200000;
            Detail251.ReferencePoint.Y = -100000;
            Detail251.ModelCenterLocation.X = Detail251.ReferencePoint.X;
            Detail251.ModelCenterLocation.Y = Detail251.ReferencePoint.Y;

            Detail251.viewID = 204000 + viewIDNumber++;
            newList.Add(Detail251);

            PaperAreaModel Detail252 = new PaperAreaModel();
            Detail252.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail252.Page = 3;

            Detail252.Name = PAPERMAIN_TYPE.DETAIL;
            Detail252.SubName = PAPERSUB_TYPE.Girder;
            Detail252.TitleName = "GIRDER"; // Girder 번호 적용 필요
            Detail252.TitleSubName = "(H300x300x10x15)"; //Girder 정보 불러와야 함
            Detail252.IsFix = false;
            Detail252.RowSpan = 1;
            Detail252.ColumnSpan = 3;
            Detail252.ScaleValue = 0;//Auto Scali
            Detail252.IsRepeat = true;

            Detail252.otherWidth = 400; //수정요함
            Detail252.otherHeight = 120; // 수정요함

            Detail252.ReferencePoint.X = 10000;
            Detail252.ReferencePoint.Y = -100000;
            Detail252.ModelCenterLocation.X = Detail252.ReferencePoint.X;
            Detail252.ModelCenterLocation.Y = Detail252.ReferencePoint.Y;

            Detail252.viewID = 205000 + viewIDNumber++;
            newList.Add(Detail252);




            PaperAreaModel Detail253 = new PaperAreaModel();
            Detail253.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail253.Page = 3;

            Detail253.Name = PAPERMAIN_TYPE.DETAIL;
            Detail253.SubName = PAPERSUB_TYPE.EmptyArea; //빈칸
            Detail253.TitleName = "";
            Detail253.TitleSubName = "";
            Detail253.IsFix = true;
            Detail253.Row = 4;
            Detail253.Column = 1;
            Detail253.RowSpan = 4;
            Detail253.ColumnSpan = 4;

            Detail253.ReferencePoint.X = 20000;
            Detail253.ReferencePoint.Y = -100000;
            Detail253.ModelCenterLocation.X = Detail253.ReferencePoint.X;
            Detail253.ModelCenterLocation.Y = Detail253.ReferencePoint.Y;

            Detail253.viewID = 206000 + viewIDNumber++;
            newList.Add(Detail253);




            PaperAreaModel Detail254 = new PaperAreaModel();
            Detail254.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail254.Page = 3;

            Detail254.Name = PAPERMAIN_TYPE.DETAIL;
            Detail254.SubName = PAPERSUB_TYPE.BracketDetail; ;
            Detail254.TitleName = "BRACKET DETAIL";
            Detail254.TitleSubName = "(SEE TABLE1)";
            Detail254.IsFix = true;
            Detail254.Row = 1;
            Detail254.Column = 4;
            Detail254.ScaleValue = 0;//Auto Scali
            Detail254.otherWidth = 120; //수정요함
            Detail254.otherHeight = 120; // 수정요함

            Detail254.ReferencePoint.X = 30000;
            Detail254.ReferencePoint.Y = -200000;
            Detail254.ModelCenterLocation.X = Detail254.ReferencePoint.X;
            Detail254.ModelCenterLocation.Y = Detail254.ReferencePoint.Y;

            Detail254.viewID = 207000 + viewIDNumber++;
            newList.Add(Detail254);


            PaperAreaModel Detail255 = new PaperAreaModel();
            Detail255.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail255.Page = 3;

            Detail255.Name = PAPERMAIN_TYPE.DETAIL;
            Detail255.SubName = PAPERSUB_TYPE.Table1; ;
            Detail255.TitleName = "TABLE1";
            Detail255.TitleSubName = "";
            Detail255.IsFix = true;
            Detail255.Row = 2;
            Detail255.Column = 4;
            Detail255.ScaleValue = 1;

            Detail255.ReferencePoint.X = 40000;
            Detail255.ReferencePoint.Y = -200000;
            Detail255.ModelCenterLocation.X = Detail255.ReferencePoint.X;
            Detail255.ModelCenterLocation.Y = Detail255.ReferencePoint.Y;

            Detail255.viewID = 208000 + viewIDNumber++;
            newList.Add(Detail255);


            PaperAreaModel Detail256 = new PaperAreaModel();
            Detail256.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail256.Page = 3;

            Detail256.Name = PAPERMAIN_TYPE.DETAIL;
            Detail256.SubName = PAPERSUB_TYPE.DetailA;
            Detail256.TitleName = "DETAIL \"A\"";
            Detail256.TitleSubName = "(TYP.)";
            Detail256.IsFix = false;
            Detail256.ScaleValue = 0; //Auto Scale

            Detail256.ReferencePoint.X = 50000;
            Detail256.ReferencePoint.Y = -200000;
            Detail256.ModelCenterLocation.X = Detail256.ReferencePoint.X;
            Detail256.ModelCenterLocation.Y = Detail256.ReferencePoint.Y;

            Detail256.viewID = 209000 + viewIDNumber++;
            newList.Add(Detail256);



            PaperAreaModel Detail257 = new PaperAreaModel();
            Detail257.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail257.Page = 4;

            Detail257.Name = PAPERMAIN_TYPE.DETAIL;
            Detail257.SubName = PAPERSUB_TYPE.Girder;
            Detail257.TitleName = "GIRDER"; // Girder 번호 적용 필요
            Detail257.TitleSubName = "(H300x300x10x15)"; //Girder 정보 불러와야 함
            Detail257.IsFix = false;
            Detail257.RowSpan = 1;
            Detail257.ColumnSpan = 3;
            Detail257.ScaleValue = 0;//Auto Scali
            Detail257.IsRepeat = true;

            Detail257.otherWidth = 400; //수정요함
            Detail257.otherHeight = 120; // 수정요함

            Detail257.ReferencePoint.X = 60000;
            Detail257.ReferencePoint.Y = -100000;
            Detail257.ModelCenterLocation.X = Detail257.ReferencePoint.X;
            Detail257.ModelCenterLocation.Y = Detail257.ReferencePoint.Y;

            Detail257.viewID = 210000 + viewIDNumber++;
            newList.Add(Detail257);


            PaperAreaModel Detail258 = new PaperAreaModel();
            Detail258.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail258.Page = 4;

            Detail258.Name = PAPERMAIN_TYPE.DETAIL;
            Detail258.SubName = PAPERSUB_TYPE.BracketDetail; ;
            Detail258.TitleName = "BRACKET DETAIL";
            Detail258.TitleSubName = "(SEE TABLE1)";
            Detail258.IsFix = true;
            Detail258.Row = 1;
            Detail258.Column = 4;
            Detail258.ScaleValue = 0;//Auto Scali
            Detail258.otherWidth = 120; //수정요함
            Detail258.otherHeight = 120; // 수정요함

            Detail258.ReferencePoint.X = 70000;
            Detail258.ReferencePoint.Y = -200000;
            Detail258.ModelCenterLocation.X = Detail258.ReferencePoint.X;
            Detail258.ModelCenterLocation.Y = Detail258.ReferencePoint.Y;

            Detail258.viewID = 211000 + viewIDNumber++;
            newList.Add(Detail258);


            PaperAreaModel Detail259 = new PaperAreaModel();
            Detail259.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail259.Page = 4;

            Detail259.Name = PAPERMAIN_TYPE.DETAIL;
            Detail259.SubName = PAPERSUB_TYPE.Table2; ;
            Detail259.TitleName = "TABLE2";
            Detail259.TitleSubName = "";
            Detail259.IsFix = true;
            Detail259.Row = 2;
            Detail259.Column = 4;
            Detail259.ScaleValue = 1;

            Detail259.ReferencePoint.X = 80000;
            Detail259.ReferencePoint.Y = -200000;
            Detail259.ModelCenterLocation.X = Detail259.ReferencePoint.X;
            Detail259.ModelCenterLocation.Y = Detail259.ReferencePoint.Y;

            Detail259.viewID = 212000 + viewIDNumber++;
            newList.Add(Detail259);


            PaperAreaModel Detail260 = new PaperAreaModel();
            Detail260.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail260.Page = 4;

            Detail260.Name = PAPERMAIN_TYPE.DETAIL;
            Detail260.SubName = PAPERSUB_TYPE.DetailA;
            Detail260.TitleName = "DETAIL \"A\"";
            Detail260.TitleSubName = "(TYP.)";
            Detail260.IsFix = false;
            Detail260.ScaleValue = 0; //Auto Scale

            Detail260.ReferencePoint.X = 90000;
            Detail260.ReferencePoint.Y = -200000;
            Detail260.ModelCenterLocation.X = Detail260.ReferencePoint.X;
            Detail260.ModelCenterLocation.Y = Detail260.ReferencePoint.Y;

            Detail260.viewID = 213000 + viewIDNumber++;
            newList.Add(Detail260);


            PaperAreaModel Detail261 = new PaperAreaModel();
            Detail261.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail261.Page = 5;

            Detail261.Name = PAPERMAIN_TYPE.DETAIL;
            Detail261.SubName = PAPERSUB_TYPE.Rafter;
            Detail261.TitleName = "RAFTER"; //Rafter 번호 표기 요함
            Detail261.TitleSubName = "(ㄷ200x80x7.35x11)"; //Rafter 정보 수정요함
            Detail261.IsFix = false;
            Detail261.RowSpan = 1;
            Detail261.ColumnSpan = 2;
            Detail261.ScaleValue = 0; // Auto Scale
            Detail261.IsRepeat = true;

            Detail261.otherWidth = 200;
            Detail261.otherHeight = 100;

            Detail261.ReferencePoint.X = 100000;
            Detail261.ReferencePoint.Y = -200000;
            Detail261.ModelCenterLocation.X = Detail261.ReferencePoint.X;
            Detail261.ModelCenterLocation.Y = Detail261.ReferencePoint.Y;

            Detail261.viewID = 214000 + viewIDNumber++;
            newList.Add(Detail261);

            PaperAreaModel Detail262 = new PaperAreaModel();
            Detail262.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail262.Page = 5;

            Detail262.Name = PAPERMAIN_TYPE.DETAIL;
            Detail262.SubName = PAPERSUB_TYPE.Table3; ;
            Detail262.TitleName = "TABLE3";
            Detail262.TitleSubName = "";
            Detail262.IsFix = false;
            Detail262.RowSpan = 1;
            Detail262.ColumnSpan = 2;
            Detail262.ScaleValue = 1;

            Detail262.ReferencePoint.X = 110000;
            Detail262.ReferencePoint.Y = -200000;
            Detail262.ModelCenterLocation.X = Detail262.ReferencePoint.X;
            Detail262.ModelCenterLocation.Y = Detail262.ReferencePoint.Y;

            Detail262.viewID = 215000 + viewIDNumber++;
            newList.Add(Detail262);


            PaperAreaModel Detail263 = new PaperAreaModel();
            Detail263.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail263.Page = 5;

            Detail263.Name = PAPERMAIN_TYPE.DETAIL;
            Detail263.SubName = PAPERSUB_TYPE.EmptyArea; //빈칸
            Detail263.TitleName = "";
            Detail263.TitleSubName = "";
            Detail263.IsFix = true;
            Detail263.Row = 3;
            Detail263.Column = 3;
            Detail263.RowSpan = 1;
            Detail263.ColumnSpan = 2;

            Detail263.ReferencePoint.X = 20000;
            Detail263.ReferencePoint.Y = -100000;
            Detail263.ModelCenterLocation.X = Detail263.ReferencePoint.X;
            Detail263.ModelCenterLocation.Y = Detail263.ReferencePoint.Y;

            Detail263.viewID = 206100 + viewIDNumber++;
            newList.Add(Detail263);

            PaperAreaModel Detail264 = new PaperAreaModel();
            Detail264.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail264.Page = 5;

            Detail264.Name = PAPERMAIN_TYPE.DETAIL;
            Detail264.SubName = PAPERSUB_TYPE.EmptyArea; //빈칸
            Detail264.TitleName = "";
            Detail264.TitleSubName = "";
            Detail264.IsFix = true;
            Detail264.Row = 4;
            Detail264.Column = 1;
            Detail264.RowSpan = 4;
            Detail264.ColumnSpan = 4;

            Detail264.ReferencePoint.X = 20000;
            Detail264.ReferencePoint.Y = -100000;
            Detail264.ModelCenterLocation.X = Detail264.ReferencePoint.X;
            Detail264.ModelCenterLocation.Y = Detail264.ReferencePoint.Y;

            Detail264.viewID = 206200 + viewIDNumber++;
            newList.Add(Detail264);



            PaperAreaModel Detail265 = new PaperAreaModel();
            Detail265.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail265.Page = 6;

            Detail265.Name = PAPERMAIN_TYPE.DETAIL;
            Detail265.SubName = PAPERSUB_TYPE.Rafter;
            Detail265.TitleName = "RAFTER"; //Rafter 번호 표기 요함
            Detail265.TitleSubName = "(ㄷ200x80x7.35x11)"; //Rafter 정보 수정요함
            Detail265.IsFix = false;
            Detail265.RowSpan = 1;
            Detail265.ColumnSpan = 2;
            Detail265.ScaleValue = 0; // Auto Scale
            Detail265.IsRepeat = true;

            Detail265.otherWidth = 200;
            Detail265.otherHeight = 100;

            Detail265.ReferencePoint.X = 120000;
            Detail265.ReferencePoint.Y = -200000;
            Detail265.ModelCenterLocation.X = Detail265.ReferencePoint.X;
            Detail265.ModelCenterLocation.Y = Detail265.ReferencePoint.Y;

            Detail265.viewID = 216000 + viewIDNumber++;
            newList.Add(Detail265);


            PaperAreaModel Detail266 = new PaperAreaModel();
            Detail266.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail266.Page = 6;

            Detail266.Name = PAPERMAIN_TYPE.DETAIL;
            Detail266.SubName = PAPERSUB_TYPE.RafterSideClipDetail;
            Detail266.TitleName = "RAFTER SIDE CLIP DETAIL";
            Detail266.TitleSubName = "(SCALE : " + Detail266.ScaleValue + ")";
            Detail266.IsFix = false;
            Detail266.RowSpan = 2;
            Detail266.ColumnSpan = 2;
            Detail266.ScaleValue = 0;//Auto Scali
            Detail266.otherWidth = 75; //수정요함
            Detail266.otherHeight = 90; // 수정요함
            Detail266.TitleSubName = "(SCALE : " + Detail266.ScaleValue + ")";// Scale 값 받기 위해 한번 더 기록

            Detail266.ReferencePoint.X = 130000;
            Detail266.ReferencePoint.Y = -200000;
            Detail266.ModelCenterLocation.X = Detail266.ReferencePoint.X;
            Detail266.ModelCenterLocation.Y = Detail266.ReferencePoint.Y;

            Detail266.viewID = 217000 + viewIDNumber++;
            newList.Add(Detail266);


            PaperAreaModel Detail267 = new PaperAreaModel();
            Detail267.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail267.Page = 6;

            Detail267.Name = PAPERMAIN_TYPE.DETAIL;
            Detail267.SubName = PAPERSUB_TYPE.Table4; ;
            Detail267.TitleName = "TABLE4";
            Detail267.TitleSubName = "";
            Detail267.IsFix = false;
            Detail267.RowSpan = 1;
            Detail267.ColumnSpan = 2;
            Detail267.ScaleValue = 1;

            Detail267.ReferencePoint.X = 140000;
            Detail267.ReferencePoint.Y = -200000;
            Detail267.ModelCenterLocation.X = Detail267.ReferencePoint.X;
            Detail267.ModelCenterLocation.Y = Detail267.ReferencePoint.Y;

            Detail267.viewID = 218000 + viewIDNumber++;
            newList.Add(Detail267);


            ///////////////Center Column Set/////////////////////

            PaperAreaModel Detail270 = new PaperAreaModel();
            Detail270.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail270.Page = 7;

            Detail270.Name = PAPERMAIN_TYPE.DETAIL;
            Detail270.SubName = PAPERSUB_TYPE.SectionAA;
            Detail270.TitleName = "SECTION\"A\"-\"A\"";
            Detail270.TitleSubName = "(1st COLUMN TOP PLATE)";
            Detail270.IsFix = true;
            Detail270.Row = 1;
            Detail270.Column = 1;
            Detail270.ScaleValue = 0; // Auto Scale
            Detail270.otherWidth = 120; //수정 요함
            Detail270.otherHeight = 300; // 수정 요함

            Detail270.ReferencePoint.X = 150000;
            Detail270.ReferencePoint.Y = -200000;
            Detail270.ModelCenterLocation.X = Detail270.ReferencePoint.X;
            Detail270.ModelCenterLocation.Y = Detail270.ReferencePoint.Y;

            Detail270.viewID = 219000 + viewIDNumber++;
            newList.Add(Detail270);

            PaperAreaModel Detail271 = new PaperAreaModel();
            Detail271.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail271.Page = 7;

            Detail271.Name = PAPERMAIN_TYPE.DETAIL;
            Detail271.SubName = PAPERSUB_TYPE.CenterColumnDetail;
            Detail271.TitleName = "CENTER COLUMN DETAIL";
            Detail271.TitleSubName = "";
            Detail271.IsFix = true;
            Detail271.Row = 2;
            Detail271.Column = 1;
            Detail271.RowSpan = 3;
            Detail271.ColumnSpan = 1;
            Detail271.ScaleValue = 0; // Auto Scale

            Detail271.otherWidth = 120; //수정 요함
            Detail271.otherHeight = 260; //수정 요함

            Detail271.ReferencePoint.X = 160000;
            Detail271.ReferencePoint.Y = -200000;
            Detail271.ModelCenterLocation.X = Detail271.ReferencePoint.X;
            Detail271.ModelCenterLocation.Y = Detail271.ReferencePoint.Y;

            Detail271.viewID = 220000 + viewIDNumber++;
            newList.Add(Detail271);

            PaperAreaModel Detail272 = new PaperAreaModel();
            Detail272.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail272.Page = 7;

            Detail272.Name = PAPERMAIN_TYPE.DETAIL;
            Detail272.SubName = PAPERSUB_TYPE.DetailF;
            Detail272.TitleName = "DETAIL\"F\"";
            Detail272.TitleSubName = "";
            Detail272.IsFix = true;
            Detail272.Row = 1;
            Detail272.Column = 2;
            Detail272.ScaleValue = 0; // Auto Scale

            Detail272.otherWidth = 100;
            Detail272.otherHeight = 100;

            Detail272.ReferencePoint.X = 170000;
            Detail272.ReferencePoint.Y = -200000;
            Detail272.ModelCenterLocation.X = Detail272.ReferencePoint.X;
            Detail272.ModelCenterLocation.Y = Detail272.ReferencePoint.Y;

            Detail272.viewID = 221000 + viewIDNumber++;
            newList.Add(Detail272);

            PaperAreaModel Detail273 = new PaperAreaModel();
            Detail273.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail273.Page = 7;

            Detail273.Name = PAPERMAIN_TYPE.DETAIL;
            Detail273.SubName = PAPERSUB_TYPE.DrainDetail;
            Detail273.TitleName = "DRAIN DETAIL";
            Detail273.TitleSubName = "(SECTION \"G\")";
            Detail273.IsFix = true;
            Detail273.Row = 1;
            Detail273.Column = 3;
            Detail273.ScaleValue = 0; // Auto Scale

            Detail273.otherWidth = 100;
            Detail273.otherHeight = 100;

            Detail273.ReferencePoint.X = 180000;
            Detail273.ReferencePoint.Y = -200000;
            Detail273.ModelCenterLocation.X = Detail273.ReferencePoint.X;
            Detail273.ModelCenterLocation.Y = Detail273.ReferencePoint.Y;

            Detail273.viewID = 222000 + viewIDNumber++;
            newList.Add(Detail273);

            PaperAreaModel Detail274 = new PaperAreaModel();
            Detail274.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail274.Page = 7;

            Detail274.Name = PAPERMAIN_TYPE.DETAIL;
            Detail274.SubName = PAPERSUB_TYPE.SectionBB;
            Detail274.TitleName = "SECTION \"B\"-\"B\"";
            Detail274.TitleSubName = "";
            Detail274.IsFix = true;
            Detail274.Row = 2;
            Detail274.Column = 2;
            Detail274.ScaleValue = 0; //Auto Scale

            Detail274.otherWidth = 120;
            Detail274.otherHeight = 120;

            Detail274.ReferencePoint.X = 190000;
            Detail274.ReferencePoint.Y = -200000;
            Detail274.ModelCenterLocation.X = Detail274.ReferencePoint.X;
            Detail274.ModelCenterLocation.Y = Detail274.ReferencePoint.Y;

            Detail274.viewID = 223000 + viewIDNumber++;
            newList.Add(Detail274);

            PaperAreaModel Detail275 = new PaperAreaModel();
            Detail275.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail275.Page = 7;

            Detail275.Name = PAPERMAIN_TYPE.DETAIL;
            Detail275.SubName = PAPERSUB_TYPE.DetailC;
            Detail275.TitleName = "DETAIL \"C\"";
            Detail275.TitleSubName = "";
            Detail275.IsFix = true;
            Detail275.Row = 2;
            Detail275.Column = 3;
            Detail275.ScaleValue = 0; //Auto Scale

            Detail275.otherWidth = 120;
            Detail275.otherHeight = 120;

            Detail275.ReferencePoint.X = 200000;
            Detail275.ReferencePoint.Y = -200000;
            Detail275.ModelCenterLocation.X = Detail275.ReferencePoint.X;
            Detail275.ModelCenterLocation.Y = Detail275.ReferencePoint.Y;

            Detail275.viewID = 224000 + viewIDNumber++;
            newList.Add(Detail275);



            PaperAreaModel Detail276 = new PaperAreaModel();
            Detail276.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail276.Page = 7;

            Detail276.Name = PAPERMAIN_TYPE.DETAIL;
            Detail276.SubName = PAPERSUB_TYPE.SectionEE;
            Detail276.TitleName = "SECTION \"E\"-\"E\"";
            Detail276.TitleSubName = "";
            Detail276.IsFix = true;
            Detail276.Row = 3;
            Detail276.Column = 2;
            Detail276.ScaleValue = 0; //Auto Scale

            Detail276.otherWidth = 120;
            Detail276.otherHeight = 120;

            Detail276.ReferencePoint.X = 210000;
            Detail276.ReferencePoint.Y = -200000;
            Detail276.ModelCenterLocation.X = Detail276.ReferencePoint.X;
            Detail276.ModelCenterLocation.Y = Detail276.ReferencePoint.Y;

            Detail276.viewID = 225000 + viewIDNumber++;
            newList.Add(Detail276);


            PaperAreaModel Detail277 = new PaperAreaModel();
            Detail277.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail277.Page = 7;

            Detail277.Name = PAPERMAIN_TYPE.DETAIL;
            Detail277.SubName = PAPERSUB_TYPE.DetailD;
            Detail277.TitleName = "DETAIL \"D\"";
            Detail277.TitleSubName = "";
            Detail277.IsFix = true;
            Detail277.Row = 3;
            Detail277.Column = 3;
            Detail277.ScaleValue = 1;


            Detail277.ReferencePoint.X = 220000;
            Detail277.ReferencePoint.Y = -200000;
            Detail277.ModelCenterLocation.X = Detail277.ReferencePoint.X;
            Detail277.ModelCenterLocation.Y = Detail277.ReferencePoint.Y;

            Detail277.viewID = 226000 + viewIDNumber++;
            newList.Add(Detail277);


            //////////////////Side Column Set///////////////////////

            PaperAreaModel Detail280 = new PaperAreaModel();
            Detail280.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail280.Page = 8;

            Detail280.Name = PAPERMAIN_TYPE.DETAIL;
            Detail280.SubName = PAPERSUB_TYPE.SectionAA;
            Detail280.TitleName = "SECTION\"A\"-\"A\"";
            Detail280.TitleSubName = "(2nd COLUMN TOP PLATE)";
            Detail280.IsFix = true;
            Detail280.Row = 1;
            Detail280.Column = 1;
            Detail280.ScaleValue = 0; // Auto Scale
            Detail280.otherWidth = 120; //수정 요함
            Detail280.otherHeight = 300; // 수정 요함

            Detail280.ReferencePoint.X = 230000;
            Detail280.ReferencePoint.Y = -200000;
            Detail280.ModelCenterLocation.X = Detail280.ReferencePoint.X;
            Detail280.ModelCenterLocation.Y = Detail280.ReferencePoint.Y;

            Detail280.viewID = 227000 + viewIDNumber++;
            newList.Add(Detail280);

            PaperAreaModel Detail281 = new PaperAreaModel();
            Detail281.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail281.Page = 8;

            Detail281.Name = PAPERMAIN_TYPE.DETAIL;
            Detail281.SubName = PAPERSUB_TYPE.CenterColumnDetail;
            Detail281.TitleName = "(C2)SIDE COLUMN DETAIL"; //C1~C6 적용 가능??
            Detail281.TitleSubName = "";
            Detail281.IsFix = true;
            Detail281.Row = 2;
            Detail281.Column = 1;
            Detail281.RowSpan = 3;
            Detail281.ColumnSpan = 1;
            Detail281.ScaleValue = 0; // Auto Scale

            Detail281.otherWidth = 120; //수정 요함
            Detail281.otherHeight = 260; //수정 요함

            Detail281.ReferencePoint.X = 240000;
            Detail281.ReferencePoint.Y = -200000;
            Detail281.ModelCenterLocation.X = Detail281.ReferencePoint.X;
            Detail281.ModelCenterLocation.Y = Detail281.ReferencePoint.Y;

            Detail281.viewID = 228000 + viewIDNumber++;
            newList.Add(Detail281);

            PaperAreaModel Detail282 = new PaperAreaModel();
            Detail282.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail282.Page = 8;

            Detail282.Name = PAPERMAIN_TYPE.DETAIL;
            Detail282.SubName = PAPERSUB_TYPE.GirderBracketDetail;
            Detail282.TitleName = "GIRDER BLACKET DETAIL";
            Detail282.TitleSubName = "";
            Detail282.IsFix = true;
            Detail282.Row = 1;
            Detail282.Column = 2;
            Detail282.ScaleValue = 0; // Auto Scale

            Detail282.otherWidth = 120;
            Detail282.otherHeight = 120;

            Detail282.ReferencePoint.X = 250000;
            Detail282.ReferencePoint.Y = -200000;
            Detail282.ModelCenterLocation.X = Detail282.ReferencePoint.X;
            Detail282.ModelCenterLocation.Y = Detail282.ReferencePoint.Y;

            Detail282.viewID = 229000 + viewIDNumber++;
            newList.Add(Detail282);

            PaperAreaModel Detail284 = new PaperAreaModel();
            Detail284.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail284.Page = 8;

            Detail284.Name = PAPERMAIN_TYPE.DETAIL;
            Detail284.SubName = PAPERSUB_TYPE.DrainDetail;
            Detail284.TitleName = "DRAIN DETAIL";
            Detail284.TitleSubName = "(SECTION \"G\")";
            Detail284.IsFix = true;
            Detail284.Row = 1;
            Detail284.Column = 3;
            Detail284.ScaleValue = 0; // Auto Scale

            Detail284.otherWidth = 100;
            Detail284.otherHeight = 100;

            Detail284.ReferencePoint.X = 260000;
            Detail284.ReferencePoint.Y = -200000;
            Detail284.ModelCenterLocation.X = Detail284.ReferencePoint.X;
            Detail284.ModelCenterLocation.Y = Detail284.ReferencePoint.Y;

            Detail284.viewID = 230000 + viewIDNumber++;
            newList.Add(Detail284);

            PaperAreaModel Detail285 = new PaperAreaModel();
            Detail285.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail285.Page = 8;

            Detail285.Name = PAPERMAIN_TYPE.DETAIL;
            Detail285.SubName = PAPERSUB_TYPE.SectionBB;
            Detail285.TitleName = "SECTION \"B\"-\"B\"";
            Detail285.TitleSubName = "";
            Detail285.IsFix = true;
            Detail285.Row = 2;
            Detail285.Column = 2;
            Detail285.ScaleValue = 0; //Auto Scale

            Detail285.otherWidth = 120;
            Detail285.otherHeight = 120;

            Detail285.ReferencePoint.X = 270000;
            Detail285.ReferencePoint.Y = -200000;
            Detail285.ModelCenterLocation.X = Detail285.ReferencePoint.X;
            Detail285.ModelCenterLocation.Y = Detail285.ReferencePoint.Y;

            Detail285.viewID = 231000 + viewIDNumber++;
            newList.Add(Detail285);

            PaperAreaModel Detail286 = new PaperAreaModel();
            Detail286.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail286.Page = 8;

            Detail286.Name = PAPERMAIN_TYPE.DETAIL;
            Detail286.SubName = PAPERSUB_TYPE.DetailC;
            Detail286.TitleName = "DETAIL \"C\"";
            Detail286.TitleSubName = "";
            Detail286.IsFix = true;
            Detail286.Row = 2;
            Detail286.Column = 3;
            Detail286.ScaleValue = 0; //Auto Scale

            Detail286.otherWidth = 120;
            Detail286.otherHeight = 120;

            Detail286.ReferencePoint.X = 280000;
            Detail286.ReferencePoint.Y = -200000;
            Detail286.ModelCenterLocation.X = Detail286.ReferencePoint.X;
            Detail286.ModelCenterLocation.Y = Detail286.ReferencePoint.Y;

            Detail286.viewID = 232000 + viewIDNumber++;
            newList.Add(Detail286);



            PaperAreaModel Detail287 = new PaperAreaModel();
            Detail287.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail287.Page = 8;

            Detail287.Name = PAPERMAIN_TYPE.DETAIL;
            Detail287.SubName = PAPERSUB_TYPE.SectionEE;
            Detail287.TitleName = "SECTION \"E\"-\"E\"";
            Detail287.TitleSubName = "";
            Detail287.IsFix = true;
            Detail287.Row = 3;
            Detail287.Column = 2;
            Detail287.ScaleValue = 0; //Auto Scale

            Detail287.otherWidth = 120;
            Detail287.otherHeight = 120;

            Detail287.ReferencePoint.X = 290000;
            Detail287.ReferencePoint.Y = -200000;
            Detail287.ModelCenterLocation.X = Detail287.ReferencePoint.X;
            Detail287.ModelCenterLocation.Y = Detail287.ReferencePoint.Y;

            Detail287.viewID = 233000 + viewIDNumber++;
            newList.Add(Detail287);


            PaperAreaModel Detail288 = new PaperAreaModel();
            Detail288.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail288.Page = 8;

            Detail288.Name = PAPERMAIN_TYPE.DETAIL;
            Detail288.SubName = PAPERSUB_TYPE.DetailD;
            Detail288.TitleName = "DETAIL \"D\"";
            Detail288.TitleSubName = "";
            Detail288.IsFix = true;
            Detail288.Row = 3;
            Detail288.Column = 3;
            Detail288.ScaleValue = 1;


            Detail288.ReferencePoint.X = 300000;
            Detail288.ReferencePoint.Y = -200000;
            Detail288.ModelCenterLocation.X = Detail288.ReferencePoint.X;
            Detail288.ModelCenterLocation.Y = Detail288.ReferencePoint.Y;

            Detail288.viewID = 234000 + viewIDNumber++;
            newList.Add(Detail288);

            //////////////////Side Column Set///////////////////////

            PaperAreaModel Detail290 = new PaperAreaModel();
            Detail290.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail290.Page = 9;

            Detail290.Name = PAPERMAIN_TYPE.DETAIL;
            Detail290.SubName = PAPERSUB_TYPE.SectionAA;
            Detail290.TitleName = "SECTION\"A\"-\"A\"";
            Detail290.TitleSubName = "(2nd COLUMN TOP PLATE)";
            Detail290.IsFix = true;
            Detail290.Row = 1;
            Detail290.Column = 1;
            Detail290.ScaleValue = 0; // Auto Scale
            Detail290.otherWidth = 120; //수정 요함
            Detail290.otherHeight = 300; // 수정 요함

            Detail290.ReferencePoint.X = 310000;
            Detail290.ReferencePoint.Y = -200000;
            Detail290.ModelCenterLocation.X = Detail290.ReferencePoint.X;
            Detail290.ModelCenterLocation.Y = Detail290.ReferencePoint.Y;

            Detail290.viewID = 235000 + viewIDNumber++;
            newList.Add(Detail290);

            PaperAreaModel Detail291 = new PaperAreaModel();
            Detail291.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail291.Page = 9;

            Detail291.Name = PAPERMAIN_TYPE.DETAIL;
            Detail291.SubName = PAPERSUB_TYPE.CenterColumnDetail;
            Detail291.TitleName = "(C2)SIDE COLUMN DETAIL"; //C1~C6 적용 가능??
            Detail291.TitleSubName = "";
            Detail291.IsFix = true;
            Detail291.Row = 2;
            Detail291.Column = 1;
            Detail291.RowSpan = 3;
            Detail291.ColumnSpan = 1;
            Detail291.ScaleValue = 0; // Auto Scale

            Detail291.otherWidth = 120; //수정 요함
            Detail291.otherHeight = 260; //수정 요함

            Detail291.ReferencePoint.X = 320000;
            Detail291.ReferencePoint.Y = -200000;
            Detail291.ModelCenterLocation.X = Detail291.ReferencePoint.X;
            Detail291.ModelCenterLocation.Y = Detail291.ReferencePoint.Y;

            Detail291.viewID = 236000 + viewIDNumber++;
            newList.Add(Detail291);

            PaperAreaModel Detail292 = new PaperAreaModel();
            Detail292.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail292.Page = 9;

            Detail292.Name = PAPERMAIN_TYPE.DETAIL;
            Detail292.SubName = PAPERSUB_TYPE.GirderBracketDetail;
            Detail292.TitleName = "GIRDER BLACKET DETAIL";
            Detail292.TitleSubName = "";
            Detail292.IsFix = true;
            Detail292.Row = 1;
            Detail292.Column = 2;
            Detail292.ScaleValue = 0; // Auto Scale

            Detail292.otherWidth = 120;
            Detail292.otherHeight = 120;

            Detail292.ReferencePoint.X = 330000;
            Detail292.ReferencePoint.Y = -200000;
            Detail292.ModelCenterLocation.X = Detail292.ReferencePoint.X;
            Detail292.ModelCenterLocation.Y = Detail292.ReferencePoint.Y;

            Detail292.viewID = 237000 + viewIDNumber++;
            newList.Add(Detail292);

            PaperAreaModel Detail294 = new PaperAreaModel();
            Detail294.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail294.Page = 9;

            Detail294.Name = PAPERMAIN_TYPE.DETAIL;
            Detail294.SubName = PAPERSUB_TYPE.DrainDetail;
            Detail294.TitleName = "DRAIN DETAIL";
            Detail294.TitleSubName = "(SECTION \"G\")";
            Detail294.IsFix = true;
            Detail294.Row = 1;
            Detail294.Column = 3;
            Detail294.ScaleValue = 0; // Auto Scale

            Detail294.otherWidth = 100;
            Detail294.otherHeight = 100;

            Detail294.ReferencePoint.X = 340000;
            Detail294.ReferencePoint.Y = -200000;
            Detail294.ModelCenterLocation.X = Detail294.ReferencePoint.X;
            Detail294.ModelCenterLocation.Y = Detail294.ReferencePoint.Y;

            Detail294.viewID = 238000 + viewIDNumber++;
            newList.Add(Detail294);

            PaperAreaModel Detail295 = new PaperAreaModel();
            Detail295.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail295.Page = 9;

            Detail295.Name = PAPERMAIN_TYPE.DETAIL;
            Detail295.SubName = PAPERSUB_TYPE.SectionBB;
            Detail295.TitleName = "SECTION \"B\"-\"B\"";
            Detail295.TitleSubName = "";
            Detail295.IsFix = true;
            Detail295.Row = 2;
            Detail295.Column = 2;
            Detail295.ScaleValue = 0; //Auto Scale

            Detail295.otherWidth = 120;
            Detail295.otherHeight = 120;

            Detail295.ReferencePoint.X = 350000;
            Detail295.ReferencePoint.Y = -200000;
            Detail295.ModelCenterLocation.X = Detail295.ReferencePoint.X;
            Detail295.ModelCenterLocation.Y = Detail295.ReferencePoint.Y;

            Detail295.viewID = 239000 + viewIDNumber++;
            newList.Add(Detail295);

            PaperAreaModel Detail296 = new PaperAreaModel();
            Detail296.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail296.Page = 9;

            Detail296.Name = PAPERMAIN_TYPE.DETAIL;
            Detail296.SubName = PAPERSUB_TYPE.DetailC;
            Detail296.TitleName = "DETAIL \"C\"";
            Detail296.TitleSubName = "";
            Detail296.IsFix = true;
            Detail296.Row = 2;
            Detail296.Column = 3;
            Detail296.ScaleValue = 0; //Auto Scale

            Detail296.otherWidth = 120;
            Detail296.otherHeight = 120;

            Detail296.ReferencePoint.X = 360000;
            Detail296.ReferencePoint.Y = -200000;
            Detail296.ModelCenterLocation.X = Detail296.ReferencePoint.X;
            Detail296.ModelCenterLocation.Y = Detail296.ReferencePoint.Y;

            Detail296.viewID = 240000 + viewIDNumber++;
            newList.Add(Detail296);



            PaperAreaModel Detail297 = new PaperAreaModel();
            Detail297.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail297.Page = 9;

            Detail297.Name = PAPERMAIN_TYPE.DETAIL;
            Detail297.SubName = PAPERSUB_TYPE.SectionEE;
            Detail297.TitleName = "SECTION \"E\"-\"E\"";
            Detail297.TitleSubName = "";
            Detail297.IsFix = true;
            Detail297.Row = 3;
            Detail297.Column = 2;
            Detail297.ScaleValue = 0; //Auto Scale

            Detail297.otherWidth = 120;
            Detail297.otherHeight = 120;

            Detail297.ReferencePoint.X = 370000;
            Detail297.ReferencePoint.Y = -200000;
            Detail297.ModelCenterLocation.X = Detail297.ReferencePoint.X;
            Detail297.ModelCenterLocation.Y = Detail297.ReferencePoint.Y;

            Detail297.viewID = 241000 + viewIDNumber++;
            newList.Add(Detail297);


            PaperAreaModel Detail298 = new PaperAreaModel();
            Detail298.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail298.Page = 9;

            Detail298.Name = PAPERMAIN_TYPE.DETAIL;
            Detail298.SubName = PAPERSUB_TYPE.DetailD;
            Detail298.TitleName = "DETAIL \"D\"";
            Detail298.TitleSubName = "";
            Detail298.IsFix = true;
            Detail298.Row = 3;
            Detail298.Column = 3;
            Detail298.ScaleValue = 1;


            Detail298.ReferencePoint.X = 380000;
            Detail298.ReferencePoint.Y = -200000;
            Detail298.ModelCenterLocation.X = Detail298.ReferencePoint.X;
            Detail298.ModelCenterLocation.Y = Detail298.ReferencePoint.Y;

            Detail298.viewID = 242000 + viewIDNumber++;
            newList.Add(Detail298);



            //////////////////Side Column Set///////////////////////

            PaperAreaModel Detail300 = new PaperAreaModel();
            Detail300.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail300.Page = 10;

            Detail300.Name = PAPERMAIN_TYPE.DETAIL;
            Detail300.SubName = PAPERSUB_TYPE.SectionAA;
            Detail300.TitleName = "SECTION\"A\"-\"A\"";
            Detail300.TitleSubName = "(2nd COLUMN TOP PLATE)";
            Detail300.IsFix = true;
            Detail300.Row = 1;
            Detail300.Column = 1;
            Detail300.ScaleValue = 0; // Auto Scale
            Detail300.otherWidth = 120; //수정 요함
            Detail300.otherHeight = 300; // 수정 요함

            Detail300.ReferencePoint.X = 390000;
            Detail300.ReferencePoint.Y = -200000;
            Detail300.ModelCenterLocation.X = Detail300.ReferencePoint.X;
            Detail300.ModelCenterLocation.Y = Detail300.ReferencePoint.Y;

            Detail300.viewID = 243000 + viewIDNumber++;
            newList.Add(Detail300);

            PaperAreaModel Detail301 = new PaperAreaModel();
            Detail301.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail301.Page = 10;

            Detail301.Name = PAPERMAIN_TYPE.DETAIL;
            Detail301.SubName = PAPERSUB_TYPE.CenterColumnDetail;
            Detail301.TitleName = "(C2)SIDE COLUMN DETAIL"; //C1~C6 적용 가능??
            Detail301.TitleSubName = "";
            Detail301.IsFix = true;
            Detail301.Row = 2;
            Detail301.Column = 1;
            Detail301.RowSpan = 3;
            Detail301.ColumnSpan = 1;
            Detail301.ScaleValue = 0; // Auto Scale

            Detail301.otherWidth = 120; //수정 요함
            Detail301.otherHeight = 260; //수정 요함

            Detail301.ReferencePoint.X = 400000;
            Detail301.ReferencePoint.Y = -200000;
            Detail301.ModelCenterLocation.X = Detail301.ReferencePoint.X;
            Detail301.ModelCenterLocation.Y = Detail301.ReferencePoint.Y;

            Detail301.viewID = 244000 + viewIDNumber++;
            newList.Add(Detail301);

            PaperAreaModel Detail302 = new PaperAreaModel();
            Detail302.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail302.Page = 10;

            Detail302.Name = PAPERMAIN_TYPE.DETAIL;
            Detail302.SubName = PAPERSUB_TYPE.GirderBracketDetail;
            Detail302.TitleName = "GIRDER BLACKET DETAIL";
            Detail302.TitleSubName = "";
            Detail302.IsFix = true;
            Detail302.Row = 1;
            Detail302.Column = 2;
            Detail302.ScaleValue = 0; // Auto Scale

            Detail302.otherWidth = 120;
            Detail302.otherHeight = 120;

            Detail302.ReferencePoint.X = 410000;
            Detail302.ReferencePoint.Y = -200000;
            Detail302.ModelCenterLocation.X = Detail302.ReferencePoint.X;
            Detail302.ModelCenterLocation.Y = Detail302.ReferencePoint.Y;

            Detail302.viewID = 245000 + viewIDNumber++;
            newList.Add(Detail302);

            PaperAreaModel Detail304 = new PaperAreaModel();
            Detail304.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail304.Page = 10;

            Detail304.Name = PAPERMAIN_TYPE.DETAIL;
            Detail304.SubName = PAPERSUB_TYPE.DrainDetail;
            Detail304.TitleName = "DRAIN DETAIL";
            Detail304.TitleSubName = "(SECTION \"G\")";
            Detail304.IsFix = true;
            Detail304.Row = 1;
            Detail304.Column = 3;
            Detail304.ScaleValue = 0; // Auto Scale

            Detail304.otherWidth = 100;
            Detail304.otherHeight = 100;

            Detail304.ReferencePoint.X = 420000;
            Detail304.ReferencePoint.Y = -200000;
            Detail304.ModelCenterLocation.X = Detail304.ReferencePoint.X;
            Detail304.ModelCenterLocation.Y = Detail304.ReferencePoint.Y;

            Detail304.viewID = 246000 + viewIDNumber++;
            newList.Add(Detail304);

            PaperAreaModel Detail305 = new PaperAreaModel();
            Detail305.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail305.Page = 10;

            Detail305.Name = PAPERMAIN_TYPE.DETAIL;
            Detail305.SubName = PAPERSUB_TYPE.SectionBB;
            Detail305.TitleName = "SECTION \"B\"-\"B\"";
            Detail305.TitleSubName = "";
            Detail305.IsFix = true;
            Detail305.Row = 2;
            Detail305.Column = 2;
            Detail305.ScaleValue = 0; //Auto Scale

            Detail305.otherWidth = 120;
            Detail305.otherHeight = 120;

            Detail305.ReferencePoint.X = 430000;
            Detail305.ReferencePoint.Y = -200000;
            Detail305.ModelCenterLocation.X = Detail305.ReferencePoint.X;
            Detail305.ModelCenterLocation.Y = Detail305.ReferencePoint.Y;

            Detail305.viewID = 247000 + viewIDNumber++;
            newList.Add(Detail305);

            PaperAreaModel Detail306 = new PaperAreaModel();
            Detail306.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail306.Page = 10;

            Detail306.Name = PAPERMAIN_TYPE.DETAIL;
            Detail306.SubName = PAPERSUB_TYPE.DetailC;
            Detail306.TitleName = "DETAIL \"C\"";
            Detail306.TitleSubName = "";
            Detail306.IsFix = true;
            Detail306.Row = 2;
            Detail306.Column = 3;
            Detail306.ScaleValue = 0; //Auto Scale

            Detail306.otherWidth = 120;
            Detail306.otherHeight = 120;

            Detail306.ReferencePoint.X = 440000;
            Detail306.ReferencePoint.Y = -200000;
            Detail306.ModelCenterLocation.X = Detail306.ReferencePoint.X;
            Detail306.ModelCenterLocation.Y = Detail306.ReferencePoint.Y;

            Detail306.viewID = 247000 + viewIDNumber++;
            newList.Add(Detail306);



            PaperAreaModel Detail307 = new PaperAreaModel();
            Detail307.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail307.Page = 10;

            Detail307.Name = PAPERMAIN_TYPE.DETAIL;
            Detail307.SubName = PAPERSUB_TYPE.SectionEE;
            Detail307.TitleName = "SECTION \"E\"-\"E\"";
            Detail307.TitleSubName = "";
            Detail307.IsFix = true;
            Detail307.Row = 3;
            Detail307.Column = 2;
            Detail307.ScaleValue = 0; //Auto Scale

            Detail307.otherWidth = 120;
            Detail307.otherHeight = 120;

            Detail307.ReferencePoint.X = 450000;
            Detail307.ReferencePoint.Y = -200000;
            Detail307.ModelCenterLocation.X = Detail307.ReferencePoint.X;
            Detail307.ModelCenterLocation.Y = Detail307.ReferencePoint.Y;

            Detail307.viewID = 248000 + viewIDNumber++;
            newList.Add(Detail307);


            PaperAreaModel Detail308 = new PaperAreaModel();
            Detail308.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail308.Page = 10;

            Detail308.Name = PAPERMAIN_TYPE.DETAIL;
            Detail308.SubName = PAPERSUB_TYPE.DetailD;
            Detail308.TitleName = "DETAIL \"D\"";
            Detail308.TitleSubName = "";
            Detail308.IsFix = true;
            Detail308.Row = 3;
            Detail308.Column = 3;
            Detail308.ScaleValue = 1;


            Detail308.ReferencePoint.X = 460000;
            Detail308.ReferencePoint.Y = -200000;
            Detail308.ModelCenterLocation.X = Detail308.ReferencePoint.X;
            Detail308.ModelCenterLocation.Y = Detail308.ReferencePoint.Y;

            Detail308.viewID = 249000 + viewIDNumber++;
            newList.Add(Detail308);


            return newList;

        }
        public List<PaperAreaModel> GatPaperAreaCRTStructureColumn05() //LayerCount 6 ~ 
        {
            double viewIDNumber = 22000;

            List<PaperAreaModel> newList = new List<PaperAreaModel>();

            PaperAreaModel Detail310 = new PaperAreaModel();
            Detail310.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail310.Page = 1;

            Detail310.Name = PAPERMAIN_TYPE.DETAIL;
            Detail310.SubName = PAPERSUB_TYPE.RoofStructureAssembly;
            Detail310.TitleName = "ROOF STRUCTURE ASSEMBLY";
            Detail310.TitleSubName = "";
            Detail310.IsFix = true;
            Detail310.Row = 1;
            Detail310.Column = 1;
            Detail310.ScaleValue = 0; // Auto Scale
            Detail310.otherWidth = 570;
            Detail310.otherHeight = 440;

            Detail310.ReferencePoint.X = 100000;
            Detail310.ReferencePoint.Y = -100000;
            Detail310.ModelCenterLocation.X = Detail310.ReferencePoint.X;
            Detail310.ModelCenterLocation.Y = Detail310.ReferencePoint.Y;

            Detail310.viewID = 250000 + viewIDNumber++;
            newList.Add(Detail310);

            PaperAreaModel Detail311 = new PaperAreaModel();
            Detail311.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail311.Page = 2;

            Detail311.Name = PAPERMAIN_TYPE.DETAIL;
            Detail311.SubName = PAPERSUB_TYPE.RoofStructureOrientation;
            Detail311.TitleName = "ROOF STRUCTURE ORIENTATION";
            Detail311.TitleSubName = "";
            Detail311.IsFix = true;
            Detail311.Row = 1;
            Detail311.Column = 1;
            Detail311.ScaleValue = 0; // Auto Scale
            Detail311.otherWidth = 570; // 수정요함
            Detail311.otherHeight = 440; // 수정요함

            Detail311.ReferencePoint.X = 200000;
            Detail311.ReferencePoint.Y = -100000;
            Detail311.ModelCenterLocation.X = Detail311.ReferencePoint.X;
            Detail311.ModelCenterLocation.Y = Detail311.ReferencePoint.Y;

            Detail311.viewID = 251000 + viewIDNumber++;
            newList.Add(Detail311);

            PaperAreaModel Detail312 = new PaperAreaModel();
            Detail312.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail312.Page = 3;

            Detail312.Name = PAPERMAIN_TYPE.DETAIL;
            Detail312.SubName = PAPERSUB_TYPE.Girder;
            Detail312.TitleName = "GIRDER"; // Girder 번호 적용 필요
            Detail312.TitleSubName = "(H300x300x10x15)"; //Girder 정보 불러와야 함
            Detail312.IsFix = false;
            Detail312.RowSpan = 1;
            Detail312.ColumnSpan = 3;
            Detail312.ScaleValue = 0;//Auto Scali
            Detail312.IsRepeat = true;

            Detail312.otherWidth = 400; //수정요함
            Detail312.otherHeight = 120; // 수정요함

            Detail312.ReferencePoint.X = 10000;
            Detail312.ReferencePoint.Y = -100000;
            Detail312.ModelCenterLocation.X = Detail312.ReferencePoint.X;
            Detail312.ModelCenterLocation.Y = Detail312.ReferencePoint.Y;

            Detail312.viewID = 252000 + viewIDNumber++;
            newList.Add(Detail312);



            PaperAreaModel Detail314 = new PaperAreaModel();
            Detail314.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail314.Page = 3;

            Detail314.Name = PAPERMAIN_TYPE.DETAIL;
            Detail314.SubName = PAPERSUB_TYPE.BracketDetail; ;
            Detail314.TitleName = "BRACKET DETAIL";
            Detail314.TitleSubName = "(SEE TABLE1)";
            Detail314.IsFix = true;
            Detail314.Row = 1;
            Detail314.Column = 4;
            Detail314.ScaleValue = 0;//Auto Scali
            Detail314.otherWidth = 120; //수정요함
            Detail314.otherHeight = 120; // 수정요함

            Detail314.ReferencePoint.X = 30000;
            Detail314.ReferencePoint.Y = -200000;
            Detail314.ModelCenterLocation.X = Detail314.ReferencePoint.X;
            Detail314.ModelCenterLocation.Y = Detail314.ReferencePoint.Y;

            Detail314.viewID = 253000 + viewIDNumber++;
            newList.Add(Detail314);


            PaperAreaModel Detail315 = new PaperAreaModel();
            Detail315.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail315.Page = 3;

            Detail315.Name = PAPERMAIN_TYPE.DETAIL;
            Detail315.SubName = PAPERSUB_TYPE.Table1; ;
            Detail315.TitleName = "TABLE1";
            Detail315.TitleSubName = "";
            Detail315.IsFix = true;
            Detail315.Row = 2;
            Detail315.Column = 4;
            Detail315.ScaleValue = 1;

            Detail315.ReferencePoint.X = 40000;
            Detail315.ReferencePoint.Y = -200000;
            Detail315.ModelCenterLocation.X = Detail315.ReferencePoint.X;
            Detail315.ModelCenterLocation.Y = Detail315.ReferencePoint.Y;

            Detail315.viewID = 254000 + viewIDNumber++;
            newList.Add(Detail315);


            PaperAreaModel Detail316 = new PaperAreaModel();
            Detail316.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail316.Page = 3;

            Detail316.Name = PAPERMAIN_TYPE.DETAIL;
            Detail316.SubName = PAPERSUB_TYPE.DetailA;
            Detail316.TitleName = "DETAIL \"A\"";
            Detail316.TitleSubName = "(TYP.)";
            Detail316.IsFix = false;
            Detail316.ScaleValue = 0; //Auto Scale

            Detail316.ReferencePoint.X = 50000;
            Detail316.ReferencePoint.Y = -200000;
            Detail316.ModelCenterLocation.X = Detail316.ReferencePoint.X;
            Detail316.ModelCenterLocation.Y = Detail316.ReferencePoint.Y;

            Detail316.viewID = 255000 + viewIDNumber++;
            newList.Add(Detail316);



            PaperAreaModel Detail317 = new PaperAreaModel();
            Detail317.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail317.Page = 4;

            Detail317.Name = PAPERMAIN_TYPE.DETAIL;
            Detail317.SubName = PAPERSUB_TYPE.Girder;
            Detail317.TitleName = "GIRDER"; // Girder 번호 적용 필요
            Detail317.TitleSubName = "(H300x300x10x15)"; //Girder 정보 불러와야 함
            Detail317.IsFix = false;
            Detail317.RowSpan = 1;
            Detail317.ColumnSpan = 3;
            Detail317.ScaleValue = 0;//Auto Scali
            Detail317.IsRepeat = true;

            Detail317.otherWidth = 400; //수정요함
            Detail317.otherHeight = 120; // 수정요함

            Detail317.ReferencePoint.X = 60000;
            Detail317.ReferencePoint.Y = -100000;
            Detail317.ModelCenterLocation.X = Detail317.ReferencePoint.X;
            Detail317.ModelCenterLocation.Y = Detail317.ReferencePoint.Y;

            Detail317.viewID = 256000 + viewIDNumber++;
            newList.Add(Detail317);


            PaperAreaModel Detail318 = new PaperAreaModel();
            Detail318.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail318.Page = 4;

            Detail318.Name = PAPERMAIN_TYPE.DETAIL;
            Detail318.SubName = PAPERSUB_TYPE.BracketDetail; ;
            Detail318.TitleName = "BRACKET DETAIL";
            Detail318.TitleSubName = "(SEE TABLE1)";
            Detail318.IsFix = true;
            Detail318.Row = 1;
            Detail318.Column = 4;
            Detail318.ScaleValue = 0;//Auto Scali
            Detail318.otherWidth = 120; //수정요함
            Detail318.otherHeight = 120; // 수정요함

            Detail318.ReferencePoint.X = 70000;
            Detail318.ReferencePoint.Y = -200000;
            Detail318.ModelCenterLocation.X = Detail318.ReferencePoint.X;
            Detail318.ModelCenterLocation.Y = Detail318.ReferencePoint.Y;

            Detail318.viewID = 257000 + viewIDNumber++;
            newList.Add(Detail318);


            PaperAreaModel Detail319 = new PaperAreaModel();
            Detail319.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail319.Page = 4;

            Detail319.Name = PAPERMAIN_TYPE.DETAIL;
            Detail319.SubName = PAPERSUB_TYPE.Table2; ;
            Detail319.TitleName = "TABLE2";
            Detail319.TitleSubName = "";
            Detail319.IsFix = true;
            Detail319.Row = 2;
            Detail319.Column = 4;
            Detail319.ScaleValue = 1;

            Detail319.ReferencePoint.X = 80000;
            Detail319.ReferencePoint.Y = -200000;
            Detail319.ModelCenterLocation.X = Detail319.ReferencePoint.X;
            Detail319.ModelCenterLocation.Y = Detail319.ReferencePoint.Y;

            Detail319.viewID = 258000 + viewIDNumber++;
            newList.Add(Detail319);


            PaperAreaModel Detail320 = new PaperAreaModel();
            Detail320.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail320.Page = 4;

            Detail320.Name = PAPERMAIN_TYPE.DETAIL;
            Detail320.SubName = PAPERSUB_TYPE.DetailA;
            Detail320.TitleName = "DETAIL \"A\"";
            Detail320.TitleSubName = "(TYP.)";
            Detail320.IsFix = false;
            Detail320.ScaleValue = 0; //Auto Scale

            Detail320.ReferencePoint.X = 90000;
            Detail320.ReferencePoint.Y = -200000;
            Detail320.ModelCenterLocation.X = Detail320.ReferencePoint.X;
            Detail320.ModelCenterLocation.Y = Detail320.ReferencePoint.Y;

            Detail320.viewID = 259000 + viewIDNumber++;
            newList.Add(Detail320);


            PaperAreaModel Detail321 = new PaperAreaModel();
            Detail321.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail321.Page = 5;

            Detail321.Name = PAPERMAIN_TYPE.DETAIL;
            Detail321.SubName = PAPERSUB_TYPE.Rafter;
            Detail321.TitleName = "RAFTER"; //Rafter 번호 표기 요함
            Detail321.TitleSubName = "(ㄷ200x80x7.35x11)"; //Rafter 정보 수정요함
            Detail321.IsFix = false;
            Detail321.RowSpan = 1;
            Detail321.ColumnSpan = 2;
            Detail321.ScaleValue = 0; // Auto Scale
            Detail321.IsRepeat = true;

            Detail321.otherWidth = 200;
            Detail321.otherHeight = 100;

            Detail321.ReferencePoint.X = 100000;
            Detail321.ReferencePoint.Y = -200000;
            Detail321.ModelCenterLocation.X = Detail321.ReferencePoint.X;
            Detail321.ModelCenterLocation.Y = Detail321.ReferencePoint.Y;

            Detail321.viewID = 260000 + viewIDNumber++;
            newList.Add(Detail321);

            PaperAreaModel Detail322 = new PaperAreaModel();
            Detail322.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail322.Page = 5;

            Detail322.Name = PAPERMAIN_TYPE.DETAIL;
            Detail322.SubName = PAPERSUB_TYPE.Table3; //5Page 마지막에 배치
            Detail322.TitleName = "TABLE3";
            Detail322.TitleSubName = "";
            Detail322.IsFix = false;
            Detail322.RowSpan = 1;
            Detail322.ColumnSpan = 2;
            Detail322.ScaleValue = 1;

            Detail322.ReferencePoint.X = 110000;
            Detail322.ReferencePoint.Y = -200000;
            Detail322.ModelCenterLocation.X = Detail322.ReferencePoint.X;
            Detail322.ModelCenterLocation.Y = Detail322.ReferencePoint.Y;

            Detail322.viewID = 261000 + viewIDNumber++;
            newList.Add(Detail322);


            PaperAreaModel Detail325 = new PaperAreaModel();
            Detail325.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail325.Page = 6;

            Detail325.Name = PAPERMAIN_TYPE.DETAIL;
            Detail325.SubName = PAPERSUB_TYPE.Rafter;
            Detail325.TitleName = "RAFTER"; //Rafter 번호 표기 요함
            Detail325.TitleSubName = "(ㄷ200x80x7.35x11)"; //Rafter 정보 수정요함
            Detail325.IsFix = false;
            Detail325.RowSpan = 1;
            Detail325.ColumnSpan = 2;
            Detail325.ScaleValue = 0; // Auto Scale
            Detail325.IsRepeat = true;

            Detail325.otherWidth = 200;
            Detail325.otherHeight = 100;

            Detail325.ReferencePoint.X = 120000;
            Detail325.ReferencePoint.Y = -200000;
            Detail325.ModelCenterLocation.X = Detail325.ReferencePoint.X;
            Detail325.ModelCenterLocation.Y = Detail325.ReferencePoint.Y;

            Detail325.viewID = 262000 + viewIDNumber++;
            newList.Add(Detail325);


            PaperAreaModel Detail326 = new PaperAreaModel();
            Detail326.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail326.Page = 6;

            Detail326.Name = PAPERMAIN_TYPE.DETAIL;
            Detail326.SubName = PAPERSUB_TYPE.RafterSideClipDetail;
            Detail326.TitleName = "RAFTER SIDE CLIP DETAIL";
            Detail326.TitleSubName = "(SCALE : " + Detail326.ScaleValue + ")";
            Detail326.IsFix = false;
            Detail326.RowSpan = 2;
            Detail326.ColumnSpan = 2;
            Detail326.ScaleValue = 0;//Auto Scali
            Detail326.otherWidth = 75; //수정요함
            Detail326.otherHeight = 90; // 수정요함
            Detail326.TitleSubName = "(SCALE : " + Detail326.ScaleValue + ")";// Scale 값 받기 위해 한번 더 기록

            Detail326.ReferencePoint.X = 130000;
            Detail326.ReferencePoint.Y = -200000;
            Detail326.ModelCenterLocation.X = Detail326.ReferencePoint.X;
            Detail326.ModelCenterLocation.Y = Detail326.ReferencePoint.Y;

            Detail326.viewID = 263000 + viewIDNumber++;
            newList.Add(Detail326);


            PaperAreaModel Detail327 = new PaperAreaModel();
            Detail327.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail327.Page = 6;

            Detail327.Name = PAPERMAIN_TYPE.DETAIL;
            Detail327.SubName = PAPERSUB_TYPE.Table4; ;
            Detail327.TitleName = "TABLE4";
            Detail327.TitleSubName = "";
            Detail327.IsFix = false;
            Detail327.RowSpan = 1;
            Detail327.ColumnSpan = 2;
            Detail327.ScaleValue = 1;

            Detail327.ReferencePoint.X = 140000;
            Detail327.ReferencePoint.Y = -200000;
            Detail327.ModelCenterLocation.X = Detail327.ReferencePoint.X;
            Detail327.ModelCenterLocation.Y = Detail327.ReferencePoint.Y;

            Detail327.viewID = 264000 + viewIDNumber++;
            newList.Add(Detail327);


            ///////////////Center Column Set/////////////////////

            PaperAreaModel Detail330 = new PaperAreaModel();
            Detail330.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail330.Page = 7;

            Detail330.Name = PAPERMAIN_TYPE.DETAIL;
            Detail330.SubName = PAPERSUB_TYPE.SectionAA;
            Detail330.TitleName = "SECTION\"A\"-\"A\"";
            Detail330.TitleSubName = "(1st COLUMN TOP PLATE)";
            Detail330.IsFix = true;
            Detail330.Row = 1;
            Detail330.Column = 1;
            Detail330.ScaleValue = 0; // Auto Scale
            Detail330.otherWidth = 120; //수정 요함
            Detail330.otherHeight = 300; // 수정 요함

            Detail330.ReferencePoint.X = 150000;
            Detail330.ReferencePoint.Y = -200000;
            Detail330.ModelCenterLocation.X = Detail330.ReferencePoint.X;
            Detail330.ModelCenterLocation.Y = Detail330.ReferencePoint.Y;

            Detail330.viewID = 265000 + viewIDNumber++;
            newList.Add(Detail330);

            PaperAreaModel Detail331 = new PaperAreaModel();
            Detail331.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail331.Page = 7;

            Detail331.Name = PAPERMAIN_TYPE.DETAIL;
            Detail331.SubName = PAPERSUB_TYPE.CenterColumnDetail;
            Detail331.TitleName = "CENTER COLUMN DETAIL";
            Detail331.TitleSubName = "";
            Detail331.IsFix = true;
            Detail331.Row = 2;
            Detail331.Column = 1;
            Detail331.RowSpan = 3;
            Detail331.ColumnSpan = 1;
            Detail331.ScaleValue = 0; // Auto Scale

            Detail331.otherWidth = 120; //수정 요함
            Detail331.otherHeight = 260; //수정 요함

            Detail331.ReferencePoint.X = 160000;
            Detail331.ReferencePoint.Y = -200000;
            Detail331.ModelCenterLocation.X = Detail331.ReferencePoint.X;
            Detail331.ModelCenterLocation.Y = Detail331.ReferencePoint.Y;

            Detail331.viewID = 266000 + viewIDNumber++;
            newList.Add(Detail331);

            PaperAreaModel Detail332 = new PaperAreaModel();
            Detail332.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail332.Page = 7;

            Detail332.Name = PAPERMAIN_TYPE.DETAIL;
            Detail332.SubName = PAPERSUB_TYPE.DetailF;
            Detail332.TitleName = "DETAIL\"F\"";
            Detail332.TitleSubName = "";
            Detail332.IsFix = true;
            Detail332.Row = 1;
            Detail332.Column = 2;
            Detail332.ScaleValue = 0; // Auto Scale

            Detail332.otherWidth = 100;
            Detail332.otherHeight = 100;

            Detail332.ReferencePoint.X = 170000;
            Detail332.ReferencePoint.Y = -200000;
            Detail332.ModelCenterLocation.X = Detail332.ReferencePoint.X;
            Detail332.ModelCenterLocation.Y = Detail332.ReferencePoint.Y;

            Detail332.viewID = 267000 + viewIDNumber++;
            newList.Add(Detail332);

            PaperAreaModel Detail333 = new PaperAreaModel();
            Detail333.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail333.Page = 7;

            Detail333.Name = PAPERMAIN_TYPE.DETAIL;
            Detail333.SubName = PAPERSUB_TYPE.DrainDetail;
            Detail333.TitleName = "DRAIN DETAIL";
            Detail333.TitleSubName = "(SECTION \"G\")";
            Detail333.IsFix = true;
            Detail333.Row = 1;
            Detail333.Column = 3;
            Detail333.ScaleValue = 0; // Auto Scale

            Detail333.otherWidth = 100;
            Detail333.otherHeight = 100;

            Detail333.ReferencePoint.X = 180000;
            Detail333.ReferencePoint.Y = -200000;
            Detail333.ModelCenterLocation.X = Detail333.ReferencePoint.X;
            Detail333.ModelCenterLocation.Y = Detail333.ReferencePoint.Y;

            Detail333.viewID = 268000 + viewIDNumber++;
            newList.Add(Detail333);

            PaperAreaModel Detail334 = new PaperAreaModel();
            Detail334.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail334.Page = 7;

            Detail334.Name = PAPERMAIN_TYPE.DETAIL;
            Detail334.SubName = PAPERSUB_TYPE.SectionBB;
            Detail334.TitleName = "SECTION \"B\"-\"B\"";
            Detail334.TitleSubName = "";
            Detail334.IsFix = true;
            Detail334.Row = 2;
            Detail334.Column = 2;
            Detail334.ScaleValue = 0; //Auto Scale

            Detail334.otherWidth = 120;
            Detail334.otherHeight = 120;

            Detail334.ReferencePoint.X = 190000;
            Detail334.ReferencePoint.Y = -200000;
            Detail334.ModelCenterLocation.X = Detail334.ReferencePoint.X;
            Detail334.ModelCenterLocation.Y = Detail334.ReferencePoint.Y;

            Detail334.viewID = 269000 + viewIDNumber++;
            newList.Add(Detail334);

            PaperAreaModel Detail335 = new PaperAreaModel();
            Detail335.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail335.Page = 7;

            Detail335.Name = PAPERMAIN_TYPE.DETAIL;
            Detail335.SubName = PAPERSUB_TYPE.DetailC;
            Detail335.TitleName = "DETAIL \"C\"";
            Detail335.TitleSubName = "";
            Detail335.IsFix = true;
            Detail335.Row = 2;
            Detail335.Column = 3;
            Detail335.ScaleValue = 0; //Auto Scale

            Detail335.otherWidth = 120;
            Detail335.otherHeight = 120;

            Detail335.ReferencePoint.X = 200000;
            Detail335.ReferencePoint.Y = -200000;
            Detail335.ModelCenterLocation.X = Detail335.ReferencePoint.X;
            Detail335.ModelCenterLocation.Y = Detail335.ReferencePoint.Y;

            Detail335.viewID = 270000 + viewIDNumber++;
            newList.Add(Detail335);



            PaperAreaModel Detail336 = new PaperAreaModel();
            Detail336.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail336.Page = 7;

            Detail336.Name = PAPERMAIN_TYPE.DETAIL;
            Detail336.SubName = PAPERSUB_TYPE.SectionEE;
            Detail336.TitleName = "SECTION \"E\"-\"E\"";
            Detail336.TitleSubName = "";
            Detail336.IsFix = true;
            Detail336.Row = 3;
            Detail336.Column = 2;
            Detail336.ScaleValue = 0; //Auto Scale

            Detail336.otherWidth = 120;
            Detail336.otherHeight = 120;

            Detail336.ReferencePoint.X = 210000;
            Detail336.ReferencePoint.Y = -200000;
            Detail336.ModelCenterLocation.X = Detail336.ReferencePoint.X;
            Detail336.ModelCenterLocation.Y = Detail336.ReferencePoint.Y;

            Detail336.viewID = 271000 + viewIDNumber++;
            newList.Add(Detail336);


            PaperAreaModel Detail337 = new PaperAreaModel();
            Detail337.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail337.Page = 7;

            Detail337.Name = PAPERMAIN_TYPE.DETAIL;
            Detail337.SubName = PAPERSUB_TYPE.DetailD;
            Detail337.TitleName = "DETAIL \"D\"";
            Detail337.TitleSubName = "";
            Detail337.IsFix = true;
            Detail337.Row = 3;
            Detail337.Column = 3;
            Detail337.ScaleValue = 1;


            Detail337.ReferencePoint.X = 220000;
            Detail337.ReferencePoint.Y = -200000;
            Detail337.ModelCenterLocation.X = Detail337.ReferencePoint.X;
            Detail337.ModelCenterLocation.Y = Detail337.ReferencePoint.Y;

            Detail337.viewID = 272000 + viewIDNumber++;
            newList.Add(Detail337);


            //////////////////Side Column Set///////////////////////

            PaperAreaModel Detail340 = new PaperAreaModel();
            Detail340.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail340.Page = 8;

            Detail340.Name = PAPERMAIN_TYPE.DETAIL;
            Detail340.SubName = PAPERSUB_TYPE.SectionAA;
            Detail340.TitleName = "SECTION\"A\"-\"A\"";
            Detail340.TitleSubName = "(2nd COLUMN TOP PLATE)";
            Detail340.IsFix = true;
            Detail340.Row = 1;
            Detail340.Column = 1;
            Detail340.ScaleValue = 0; // Auto Scale
            Detail340.otherWidth = 120; //수정 요함
            Detail340.otherHeight = 300; // 수정 요함

            Detail340.ReferencePoint.X = 230000;
            Detail340.ReferencePoint.Y = -200000;
            Detail340.ModelCenterLocation.X = Detail340.ReferencePoint.X;
            Detail340.ModelCenterLocation.Y = Detail340.ReferencePoint.Y;

            Detail340.viewID = 273000 + viewIDNumber++;
            newList.Add(Detail340);

            PaperAreaModel Detail341 = new PaperAreaModel();
            Detail341.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail341.Page = 8;

            Detail341.Name = PAPERMAIN_TYPE.DETAIL;
            Detail341.SubName = PAPERSUB_TYPE.CenterColumnDetail;
            Detail341.TitleName = "(C2)SIDE COLUMN DETAIL"; //C1~C6 적용 가능??
            Detail341.TitleSubName = "";
            Detail341.IsFix = true;
            Detail341.Row = 2;
            Detail341.Column = 1;
            Detail341.RowSpan = 3;
            Detail341.ColumnSpan = 1;
            Detail341.ScaleValue = 0; // Auto Scale

            Detail341.otherWidth = 120; //수정 요함
            Detail341.otherHeight = 260; //수정 요함

            Detail341.ReferencePoint.X = 240000;
            Detail341.ReferencePoint.Y = -200000;
            Detail341.ModelCenterLocation.X = Detail341.ReferencePoint.X;
            Detail341.ModelCenterLocation.Y = Detail341.ReferencePoint.Y;

            Detail341.viewID = 274000 + viewIDNumber++;
            newList.Add(Detail341);

            PaperAreaModel Detail342 = new PaperAreaModel();
            Detail342.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail342.Page = 8;

            Detail342.Name = PAPERMAIN_TYPE.DETAIL;
            Detail342.SubName = PAPERSUB_TYPE.GirderBracketDetail;
            Detail342.TitleName = "GIRDER BLACKET DETAIL";
            Detail342.TitleSubName = "";
            Detail342.IsFix = true;
            Detail342.Row = 1;
            Detail342.Column = 2;
            Detail342.ScaleValue = 0; // Auto Scale

            Detail342.otherWidth = 120;
            Detail342.otherHeight = 120;

            Detail342.ReferencePoint.X = 250000;
            Detail342.ReferencePoint.Y = -200000;
            Detail342.ModelCenterLocation.X = Detail342.ReferencePoint.X;
            Detail342.ModelCenterLocation.Y = Detail342.ReferencePoint.Y;

            Detail342.viewID = 275000 + viewIDNumber++;
            newList.Add(Detail342);

            PaperAreaModel Detail344 = new PaperAreaModel();
            Detail344.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail344.Page = 8;

            Detail344.Name = PAPERMAIN_TYPE.DETAIL;
            Detail344.SubName = PAPERSUB_TYPE.DrainDetail;
            Detail344.TitleName = "DRAIN DETAIL";
            Detail344.TitleSubName = "(SECTION \"G\")";
            Detail344.IsFix = true;
            Detail344.Row = 1;
            Detail344.Column = 3;
            Detail344.ScaleValue = 0; // Auto Scale

            Detail344.otherWidth = 100;
            Detail344.otherHeight = 100;

            Detail344.ReferencePoint.X = 260000;
            Detail344.ReferencePoint.Y = -200000;
            Detail344.ModelCenterLocation.X = Detail344.ReferencePoint.X;
            Detail344.ModelCenterLocation.Y = Detail344.ReferencePoint.Y;

            Detail344.viewID = 276000 + viewIDNumber++;
            newList.Add(Detail344);

            PaperAreaModel Detail345 = new PaperAreaModel();
            Detail345.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail345.Page = 8;

            Detail345.Name = PAPERMAIN_TYPE.DETAIL;
            Detail345.SubName = PAPERSUB_TYPE.SectionBB;
            Detail345.TitleName = "SECTION \"B\"-\"B\"";
            Detail345.TitleSubName = "";
            Detail345.IsFix = true;
            Detail345.Row = 2;
            Detail345.Column = 2;
            Detail345.ScaleValue = 0; //Auto Scale

            Detail345.otherWidth = 120;
            Detail345.otherHeight = 120;

            Detail345.ReferencePoint.X = 270000;
            Detail345.ReferencePoint.Y = -200000;
            Detail345.ModelCenterLocation.X = Detail345.ReferencePoint.X;
            Detail345.ModelCenterLocation.Y = Detail345.ReferencePoint.Y;

            Detail345.viewID = 277000 + viewIDNumber++;
            newList.Add(Detail345);

            PaperAreaModel Detail346 = new PaperAreaModel();
            Detail346.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail346.Page = 8;

            Detail346.Name = PAPERMAIN_TYPE.DETAIL;
            Detail346.SubName = PAPERSUB_TYPE.DetailC;
            Detail346.TitleName = "DETAIL \"C\"";
            Detail346.TitleSubName = "";
            Detail346.IsFix = true;
            Detail346.Row = 2;
            Detail346.Column = 3;
            Detail346.ScaleValue = 0; //Auto Scale

            Detail346.otherWidth = 120;
            Detail346.otherHeight = 120;

            Detail346.ReferencePoint.X = 280000;
            Detail346.ReferencePoint.Y = -200000;
            Detail346.ModelCenterLocation.X = Detail346.ReferencePoint.X;
            Detail346.ModelCenterLocation.Y = Detail346.ReferencePoint.Y;

            Detail346.viewID = 278000 + viewIDNumber++;
            newList.Add(Detail346);



            PaperAreaModel Detail347 = new PaperAreaModel();
            Detail347.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail347.Page = 8;

            Detail347.Name = PAPERMAIN_TYPE.DETAIL;
            Detail347.SubName = PAPERSUB_TYPE.SectionEE;
            Detail347.TitleName = "SECTION \"E\"-\"E\"";
            Detail347.TitleSubName = "";
            Detail347.IsFix = true;
            Detail347.Row = 3;
            Detail347.Column = 2;
            Detail347.ScaleValue = 0; //Auto Scale

            Detail347.otherWidth = 120;
            Detail347.otherHeight = 120;

            Detail347.ReferencePoint.X = 290000;
            Detail347.ReferencePoint.Y = -200000;
            Detail347.ModelCenterLocation.X = Detail347.ReferencePoint.X;
            Detail347.ModelCenterLocation.Y = Detail347.ReferencePoint.Y;

            Detail347.viewID = 279000 + viewIDNumber++;
            newList.Add(Detail347);


            PaperAreaModel Detail348 = new PaperAreaModel();
            Detail348.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail348.Page = 8;

            Detail348.Name = PAPERMAIN_TYPE.DETAIL;
            Detail348.SubName = PAPERSUB_TYPE.DetailD;
            Detail348.TitleName = "DETAIL \"D\"";
            Detail348.TitleSubName = "";
            Detail348.IsFix = true;
            Detail348.Row = 3;
            Detail348.Column = 3;
            Detail348.ScaleValue = 1;


            Detail348.ReferencePoint.X = 300000;
            Detail348.ReferencePoint.Y = -200000;
            Detail348.ModelCenterLocation.X = Detail348.ReferencePoint.X;
            Detail348.ModelCenterLocation.Y = Detail348.ReferencePoint.Y;

            Detail348.viewID = 280000 + viewIDNumber++;
            newList.Add(Detail348);

            //////////////////Side Column Set///////////////////////

            PaperAreaModel Detail350 = new PaperAreaModel();
            Detail350.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail350.Page = 9;

            Detail350.Name = PAPERMAIN_TYPE.DETAIL;
            Detail350.SubName = PAPERSUB_TYPE.SectionAA;
            Detail350.TitleName = "SECTION\"A\"-\"A\"";
            Detail350.TitleSubName = "(2nd COLUMN TOP PLATE)";
            Detail350.IsFix = true;
            Detail350.Row = 1;
            Detail350.Column = 1;
            Detail350.ScaleValue = 0; // Auto Scale
            Detail350.otherWidth = 120; //수정 요함
            Detail350.otherHeight = 300; // 수정 요함

            Detail350.ReferencePoint.X = 310000;
            Detail350.ReferencePoint.Y = -200000;
            Detail350.ModelCenterLocation.X = Detail350.ReferencePoint.X;
            Detail350.ModelCenterLocation.Y = Detail350.ReferencePoint.Y;

            Detail350.viewID = 281000 + viewIDNumber++;
            newList.Add(Detail350);

            PaperAreaModel Detail351 = new PaperAreaModel();
            Detail351.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail351.Page = 9;

            Detail351.Name = PAPERMAIN_TYPE.DETAIL;
            Detail351.SubName = PAPERSUB_TYPE.CenterColumnDetail;
            Detail351.TitleName = "(C2)SIDE COLUMN DETAIL"; //C1~C6 적용 가능??
            Detail351.TitleSubName = "";
            Detail351.IsFix = true;
            Detail351.Row = 2;
            Detail351.Column = 1;
            Detail351.RowSpan = 3;
            Detail351.ColumnSpan = 1;
            Detail351.ScaleValue = 0; // Auto Scale

            Detail351.otherWidth = 120; //수정 요함
            Detail351.otherHeight = 260; //수정 요함

            Detail351.ReferencePoint.X = 320000;
            Detail351.ReferencePoint.Y = -200000;
            Detail351.ModelCenterLocation.X = Detail351.ReferencePoint.X;
            Detail351.ModelCenterLocation.Y = Detail351.ReferencePoint.Y;

            Detail351.viewID = 282000 + viewIDNumber++;
            newList.Add(Detail351);

            PaperAreaModel Detail352 = new PaperAreaModel();
            Detail352.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail352.Page = 9;

            Detail352.Name = PAPERMAIN_TYPE.DETAIL;
            Detail352.SubName = PAPERSUB_TYPE.GirderBracketDetail;
            Detail352.TitleName = "GIRDER BLACKET DETAIL";
            Detail352.TitleSubName = "";
            Detail352.IsFix = true;
            Detail352.Row = 1;
            Detail352.Column = 2;
            Detail352.ScaleValue = 0; // Auto Scale

            Detail352.otherWidth = 120;
            Detail352.otherHeight = 120;

            Detail352.ReferencePoint.X = 330000;
            Detail352.ReferencePoint.Y = -200000;
            Detail352.ModelCenterLocation.X = Detail352.ReferencePoint.X;
            Detail352.ModelCenterLocation.Y = Detail352.ReferencePoint.Y;

            Detail352.viewID = 283000 + viewIDNumber++;
            newList.Add(Detail352);

            PaperAreaModel Detail354 = new PaperAreaModel();
            Detail354.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail354.Page = 9;

            Detail354.Name = PAPERMAIN_TYPE.DETAIL;
            Detail354.SubName = PAPERSUB_TYPE.DrainDetail;
            Detail354.TitleName = "DRAIN DETAIL";
            Detail354.TitleSubName = "(SECTION \"G\")";
            Detail354.IsFix = true;
            Detail354.Row = 1;
            Detail354.Column = 3;
            Detail354.ScaleValue = 0; // Auto Scale

            Detail354.otherWidth = 100;
            Detail354.otherHeight = 100;

            Detail354.ReferencePoint.X = 340000;
            Detail354.ReferencePoint.Y = -200000;
            Detail354.ModelCenterLocation.X = Detail354.ReferencePoint.X;
            Detail354.ModelCenterLocation.Y = Detail354.ReferencePoint.Y;

            Detail354.viewID = 284000 + viewIDNumber++;
            newList.Add(Detail354);

            PaperAreaModel Detail355 = new PaperAreaModel();
            Detail355.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail355.Page = 9;

            Detail355.Name = PAPERMAIN_TYPE.DETAIL;
            Detail355.SubName = PAPERSUB_TYPE.SectionBB;
            Detail355.TitleName = "SECTION \"B\"-\"B\"";
            Detail355.TitleSubName = "";
            Detail355.IsFix = true;
            Detail355.Row = 2;
            Detail355.Column = 2;
            Detail355.ScaleValue = 0; //Auto Scale

            Detail355.otherWidth = 120;
            Detail355.otherHeight = 120;

            Detail355.ReferencePoint.X = 350000;
            Detail355.ReferencePoint.Y = -200000;
            Detail355.ModelCenterLocation.X = Detail355.ReferencePoint.X;
            Detail355.ModelCenterLocation.Y = Detail355.ReferencePoint.Y;

            Detail355.viewID = 285000 + viewIDNumber++;
            newList.Add(Detail355);

            PaperAreaModel Detail356 = new PaperAreaModel();
            Detail356.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail356.Page = 9;

            Detail356.Name = PAPERMAIN_TYPE.DETAIL;
            Detail356.SubName = PAPERSUB_TYPE.DetailC;
            Detail356.TitleName = "DETAIL \"C\"";
            Detail356.TitleSubName = "";
            Detail356.IsFix = true;
            Detail356.Row = 2;
            Detail356.Column = 3;
            Detail356.ScaleValue = 0; //Auto Scale

            Detail356.otherWidth = 120;
            Detail356.otherHeight = 120;

            Detail356.ReferencePoint.X = 360000;
            Detail356.ReferencePoint.Y = -200000;
            Detail356.ModelCenterLocation.X = Detail356.ReferencePoint.X;
            Detail356.ModelCenterLocation.Y = Detail356.ReferencePoint.Y;

            Detail356.viewID = 286000 + viewIDNumber++;
            newList.Add(Detail356);



            PaperAreaModel Detail357 = new PaperAreaModel();
            Detail357.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail357.Page = 9;

            Detail357.Name = PAPERMAIN_TYPE.DETAIL;
            Detail357.SubName = PAPERSUB_TYPE.SectionEE;
            Detail357.TitleName = "SECTION \"E\"-\"E\"";
            Detail357.TitleSubName = "";
            Detail357.IsFix = true;
            Detail357.Row = 3;
            Detail357.Column = 2;
            Detail357.ScaleValue = 0; //Auto Scale

            Detail357.otherWidth = 120;
            Detail357.otherHeight = 120;

            Detail357.ReferencePoint.X = 370000;
            Detail357.ReferencePoint.Y = -200000;
            Detail357.ModelCenterLocation.X = Detail357.ReferencePoint.X;
            Detail357.ModelCenterLocation.Y = Detail357.ReferencePoint.Y;

            Detail357.viewID = 287000 + viewIDNumber++;
            newList.Add(Detail357);


            PaperAreaModel Detail358 = new PaperAreaModel();
            Detail358.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail358.Page = 9;

            Detail358.Name = PAPERMAIN_TYPE.DETAIL;
            Detail358.SubName = PAPERSUB_TYPE.DetailD;
            Detail358.TitleName = "DETAIL \"D\"";
            Detail358.TitleSubName = "";
            Detail358.IsFix = true;
            Detail358.Row = 3;
            Detail358.Column = 3;
            Detail358.ScaleValue = 1;


            Detail358.ReferencePoint.X = 380000;
            Detail358.ReferencePoint.Y = -200000;
            Detail358.ModelCenterLocation.X = Detail358.ReferencePoint.X;
            Detail358.ModelCenterLocation.Y = Detail358.ReferencePoint.Y;

            Detail358.viewID = 288000 + viewIDNumber++;
            newList.Add(Detail358);



            //////////////////Side Column Set///////////////////////

            PaperAreaModel Detail360 = new PaperAreaModel();
            Detail360.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail360.Page = 10;

            Detail360.Name = PAPERMAIN_TYPE.DETAIL;
            Detail360.SubName = PAPERSUB_TYPE.SectionAA;
            Detail360.TitleName = "SECTION\"A\"-\"A\"";
            Detail360.TitleSubName = "(2nd COLUMN TOP PLATE)";
            Detail360.IsFix = true;
            Detail360.Row = 1;
            Detail360.Column = 1;
            Detail360.ScaleValue = 0; // Auto Scale
            Detail360.otherWidth = 120; //수정 요함
            Detail360.otherHeight = 300; // 수정 요함

            Detail360.ReferencePoint.X = 390000;
            Detail360.ReferencePoint.Y = -200000;
            Detail360.ModelCenterLocation.X = Detail360.ReferencePoint.X;
            Detail360.ModelCenterLocation.Y = Detail360.ReferencePoint.Y;

            Detail360.viewID = 289000 + viewIDNumber++;
            newList.Add(Detail360);

            PaperAreaModel Detail361 = new PaperAreaModel();
            Detail361.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail361.Page = 10;

            Detail361.Name = PAPERMAIN_TYPE.DETAIL;
            Detail361.SubName = PAPERSUB_TYPE.CenterColumnDetail;
            Detail361.TitleName = "(C2)SIDE COLUMN DETAIL"; //C1~C6 적용 가능??
            Detail361.TitleSubName = "";
            Detail361.IsFix = true;
            Detail361.Row = 2;
            Detail361.Column = 1;
            Detail361.RowSpan = 3;
            Detail361.ColumnSpan = 1;
            Detail361.ScaleValue = 0; // Auto Scale

            Detail361.otherWidth = 120; //수정 요함
            Detail361.otherHeight = 260; //수정 요함

            Detail361.ReferencePoint.X = 400000;
            Detail361.ReferencePoint.Y = -200000;
            Detail361.ModelCenterLocation.X = Detail361.ReferencePoint.X;
            Detail361.ModelCenterLocation.Y = Detail361.ReferencePoint.Y;

            Detail361.viewID = 290000 + viewIDNumber++;
            newList.Add(Detail361);

            PaperAreaModel Detail362 = new PaperAreaModel();
            Detail362.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail362.Page = 10;

            Detail362.Name = PAPERMAIN_TYPE.DETAIL;
            Detail362.SubName = PAPERSUB_TYPE.GirderBracketDetail;
            Detail362.TitleName = "GIRDER BLACKET DETAIL";
            Detail362.TitleSubName = "";
            Detail362.IsFix = true;
            Detail362.Row = 1;
            Detail362.Column = 2;
            Detail362.ScaleValue = 0; // Auto Scale

            Detail362.otherWidth = 120;
            Detail362.otherHeight = 120;

            Detail362.ReferencePoint.X = 410000;
            Detail362.ReferencePoint.Y = -200000;
            Detail362.ModelCenterLocation.X = Detail362.ReferencePoint.X;
            Detail362.ModelCenterLocation.Y = Detail362.ReferencePoint.Y;

            Detail362.viewID = 291000 + viewIDNumber++;
            newList.Add(Detail362);

            PaperAreaModel Detail364 = new PaperAreaModel();
            Detail364.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail364.Page = 10;

            Detail364.Name = PAPERMAIN_TYPE.DETAIL;
            Detail364.SubName = PAPERSUB_TYPE.DrainDetail;
            Detail364.TitleName = "DRAIN DETAIL";
            Detail364.TitleSubName = "(SECTION \"G\")";
            Detail364.IsFix = true;
            Detail364.Row = 1;
            Detail364.Column = 3;
            Detail364.ScaleValue = 0; // Auto Scale

            Detail364.otherWidth = 100;
            Detail364.otherHeight = 100;

            Detail364.ReferencePoint.X = 420000;
            Detail364.ReferencePoint.Y = -200000;
            Detail364.ModelCenterLocation.X = Detail364.ReferencePoint.X;
            Detail364.ModelCenterLocation.Y = Detail364.ReferencePoint.Y;

            Detail364.viewID = 292000 + viewIDNumber++;
            newList.Add(Detail364);

            PaperAreaModel Detail365 = new PaperAreaModel();
            Detail365.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail365.Page = 10;

            Detail365.Name = PAPERMAIN_TYPE.DETAIL;
            Detail365.SubName = PAPERSUB_TYPE.SectionBB;
            Detail365.TitleName = "SECTION \"B\"-\"B\"";
            Detail365.TitleSubName = "";
            Detail365.IsFix = true;
            Detail365.Row = 2;
            Detail365.Column = 2;
            Detail365.ScaleValue = 0; //Auto Scale

            Detail365.otherWidth = 120;
            Detail365.otherHeight = 120;

            Detail365.ReferencePoint.X = 430000;
            Detail365.ReferencePoint.Y = -200000;
            Detail365.ModelCenterLocation.X = Detail365.ReferencePoint.X;
            Detail365.ModelCenterLocation.Y = Detail365.ReferencePoint.Y;

            Detail365.viewID = 293000 + viewIDNumber++;
            newList.Add(Detail365);

            PaperAreaModel Detail366 = new PaperAreaModel();
            Detail366.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail366.Page = 10;

            Detail366.Name = PAPERMAIN_TYPE.DETAIL;
            Detail366.SubName = PAPERSUB_TYPE.DetailC;
            Detail366.TitleName = "DETAIL \"C\"";
            Detail366.TitleSubName = "";
            Detail366.IsFix = true;
            Detail366.Row = 2;
            Detail366.Column = 3;
            Detail366.ScaleValue = 0; //Auto Scale

            Detail366.otherWidth = 120;
            Detail366.otherHeight = 120;

            Detail366.ReferencePoint.X = 440000;
            Detail366.ReferencePoint.Y = -200000;
            Detail366.ModelCenterLocation.X = Detail366.ReferencePoint.X;
            Detail366.ModelCenterLocation.Y = Detail366.ReferencePoint.Y;

            Detail366.viewID = 294000 + viewIDNumber++;
            newList.Add(Detail366);



            PaperAreaModel Detail367 = new PaperAreaModel();
            Detail367.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail367.Page = 10;

            Detail367.Name = PAPERMAIN_TYPE.DETAIL;
            Detail367.SubName = PAPERSUB_TYPE.SectionEE;
            Detail367.TitleName = "SECTION \"E\"-\"E\"";
            Detail367.TitleSubName = "";
            Detail367.IsFix = true;
            Detail367.Row = 3;
            Detail367.Column = 2;
            Detail367.ScaleValue = 0; //Auto Scale

            Detail367.otherWidth = 120;
            Detail367.otherHeight = 120;

            Detail367.ReferencePoint.X = 450000;
            Detail367.ReferencePoint.Y = -200000;
            Detail367.ModelCenterLocation.X = Detail367.ReferencePoint.X;
            Detail367.ModelCenterLocation.Y = Detail367.ReferencePoint.Y;

            Detail367.viewID = 295000 + viewIDNumber++;
            newList.Add(Detail367);


            PaperAreaModel Detail368 = new PaperAreaModel();
            Detail368.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail368.Page = 10;

            Detail368.Name = PAPERMAIN_TYPE.DETAIL;
            Detail368.SubName = PAPERSUB_TYPE.DetailD;
            Detail368.TitleName = "DETAIL \"D\"";
            Detail368.TitleSubName = "";
            Detail368.IsFix = true;
            Detail368.Row = 3;
            Detail368.Column = 3;
            Detail368.ScaleValue = 1;


            Detail368.ReferencePoint.X = 460000;
            Detail368.ReferencePoint.Y = -200000;
            Detail368.ModelCenterLocation.X = Detail368.ReferencePoint.X;
            Detail368.ModelCenterLocation.Y = Detail368.ReferencePoint.Y;

            Detail368.viewID = 296000 + viewIDNumber++;
            newList.Add(Detail368);


            //////////////////Side Column Set///////////////////////

            PaperAreaModel Detail370 = new PaperAreaModel();
            Detail370.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail370.Page = 11;

            Detail370.Name = PAPERMAIN_TYPE.DETAIL;
            Detail370.SubName = PAPERSUB_TYPE.SectionAA;
            Detail370.TitleName = "SECTION\"A\"-\"A\"";
            Detail370.TitleSubName = "(2nd COLUMN TOP PLATE)";
            Detail370.IsFix = true;
            Detail370.Row = 1;
            Detail370.Column = 1;
            Detail370.ScaleValue = 0; // Auto Scale
            Detail370.otherWidth = 120; //수정 요함
            Detail370.otherHeight = 300; // 수정 요함

            Detail370.ReferencePoint.X = 80000 + 390000;
            Detail370.ReferencePoint.Y = -200000;
            Detail370.ModelCenterLocation.X = Detail370.ReferencePoint.X;
            Detail370.ModelCenterLocation.Y = Detail370.ReferencePoint.Y;

            Detail370.viewID = 297000 + viewIDNumber++;
            newList.Add(Detail370);

            PaperAreaModel Detail371 = new PaperAreaModel();
            Detail371.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail371.Page = 11;

            Detail371.Name = PAPERMAIN_TYPE.DETAIL;
            Detail371.SubName = PAPERSUB_TYPE.CenterColumnDetail;
            Detail371.TitleName = "(C2)SIDE COLUMN DETAIL"; //C1~C6 적용 가능??
            Detail371.TitleSubName = "";
            Detail371.IsFix = true;
            Detail371.Row = 2;
            Detail371.Column = 1;
            Detail371.RowSpan = 3;
            Detail371.ColumnSpan = 1;
            Detail371.ScaleValue = 0; // Auto Scale

            Detail371.otherWidth = 120; //수정 요함
            Detail371.otherHeight = 260; //수정 요함

            Detail371.ReferencePoint.X = 80000 + 400000;
            Detail371.ReferencePoint.Y = -200000;
            Detail371.ModelCenterLocation.X = Detail371.ReferencePoint.X;
            Detail371.ModelCenterLocation.Y = Detail371.ReferencePoint.Y;

            Detail371.viewID = 298000 + viewIDNumber++;
            newList.Add(Detail371);

            PaperAreaModel Detail372 = new PaperAreaModel();
            Detail372.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail372.Page = 11;

            Detail372.Name = PAPERMAIN_TYPE.DETAIL;
            Detail372.SubName = PAPERSUB_TYPE.GirderBracketDetail;
            Detail372.TitleName = "GIRDER BLACKET DETAIL";
            Detail372.TitleSubName = "";
            Detail372.IsFix = true;
            Detail372.Row = 1;
            Detail372.Column = 2;
            Detail372.ScaleValue = 0; // Auto Scale

            Detail372.otherWidth = 120;
            Detail372.otherHeight = 120;

            Detail372.ReferencePoint.X = 80000 + 410000;
            Detail372.ReferencePoint.Y = -200000;
            Detail372.ModelCenterLocation.X = Detail372.ReferencePoint.X;
            Detail372.ModelCenterLocation.Y = Detail372.ReferencePoint.Y;

            Detail372.viewID = 299000 + viewIDNumber++;
            newList.Add(Detail372);

            PaperAreaModel Detail374 = new PaperAreaModel();
            Detail374.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail374.Page = 11;

            Detail374.Name = PAPERMAIN_TYPE.DETAIL;
            Detail374.SubName = PAPERSUB_TYPE.DrainDetail;
            Detail374.TitleName = "DRAIN DETAIL";
            Detail374.TitleSubName = "(SECTION \"G\")";
            Detail374.IsFix = true;
            Detail374.Row = 1;
            Detail374.Column = 3;
            Detail374.ScaleValue = 0; // Auto Scale

            Detail374.otherWidth = 100;
            Detail374.otherHeight = 100;

            Detail374.ReferencePoint.X = 80000 + 420000;
            Detail374.ReferencePoint.Y = -200000;
            Detail374.ModelCenterLocation.X = Detail374.ReferencePoint.X;
            Detail374.ModelCenterLocation.Y = Detail374.ReferencePoint.Y;

            Detail374.viewID = 300000 + viewIDNumber++;
            newList.Add(Detail374);

            PaperAreaModel Detail375 = new PaperAreaModel();
            Detail375.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail375.Page = 11;

            Detail375.Name = PAPERMAIN_TYPE.DETAIL;
            Detail375.SubName = PAPERSUB_TYPE.SectionBB;
            Detail375.TitleName = "SECTION \"B\"-\"B\"";
            Detail375.TitleSubName = "";
            Detail375.IsFix = true;
            Detail375.Row = 2;
            Detail375.Column = 2;
            Detail375.ScaleValue = 0; //Auto Scale

            Detail375.otherWidth = 120;
            Detail375.otherHeight = 120;

            Detail375.ReferencePoint.X = 80000 + 430000;
            Detail375.ReferencePoint.Y = -200000;
            Detail375.ModelCenterLocation.X = Detail375.ReferencePoint.X;
            Detail375.ModelCenterLocation.Y = Detail375.ReferencePoint.Y;

            Detail375.viewID = 301000 + viewIDNumber++;
            newList.Add(Detail375);

            PaperAreaModel Detail376 = new PaperAreaModel();
            Detail376.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail376.Page = 11;

            Detail376.Name = PAPERMAIN_TYPE.DETAIL;
            Detail376.SubName = PAPERSUB_TYPE.DetailC;
            Detail376.TitleName = "DETAIL \"C\"";
            Detail376.TitleSubName = "";
            Detail376.IsFix = true;
            Detail376.Row = 2;
            Detail376.Column = 3;
            Detail376.ScaleValue = 0; //Auto Scale

            Detail376.otherWidth = 120;
            Detail376.otherHeight = 120;

            Detail376.ReferencePoint.X = 80000 + 440000;
            Detail376.ReferencePoint.Y = -200000;
            Detail376.ModelCenterLocation.X = Detail376.ReferencePoint.X;
            Detail376.ModelCenterLocation.Y = Detail376.ReferencePoint.Y;

            Detail376.viewID = 302000 + viewIDNumber++;
            newList.Add(Detail376);



            PaperAreaModel Detail377 = new PaperAreaModel();
            Detail377.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail377.Page = 11;

            Detail377.Name = PAPERMAIN_TYPE.DETAIL;
            Detail377.SubName = PAPERSUB_TYPE.SectionEE;
            Detail377.TitleName = "SECTION \"E\"-\"E\"";
            Detail377.TitleSubName = "";
            Detail377.IsFix = true;
            Detail377.Row = 3;
            Detail377.Column = 2;
            Detail377.ScaleValue = 0; //Auto Scale

            Detail377.otherWidth = 120;
            Detail377.otherHeight = 120;

            Detail377.ReferencePoint.X = 80000 + 450000;
            Detail377.ReferencePoint.Y = -200000;
            Detail377.ModelCenterLocation.X = Detail377.ReferencePoint.X;
            Detail377.ModelCenterLocation.Y = Detail377.ReferencePoint.Y;

            Detail377.viewID = 303000 + viewIDNumber++;
            newList.Add(Detail377);


            PaperAreaModel Detail378 = new PaperAreaModel();
            Detail378.DWGName = PAPERMAIN_TYPE.DetailOfRoofStructure;
            Detail378.Page = 11;

            Detail378.Name = PAPERMAIN_TYPE.DETAIL;
            Detail378.SubName = PAPERSUB_TYPE.DetailD;
            Detail378.TitleName = "DETAIL \"D\"";
            Detail378.TitleSubName = "";
            Detail378.IsFix = true;
            Detail378.Row = 3;
            Detail378.Column = 3;
            Detail378.ScaleValue = 1;


            Detail378.ReferencePoint.X = 80000 + 460000;
            Detail378.ReferencePoint.Y = -200000;
            Detail378.ModelCenterLocation.X = Detail378.ReferencePoint.X;
            Detail378.ModelCenterLocation.Y = Detail378.ReferencePoint.Y;

            Detail378.viewID = 304000 + viewIDNumber++;
            newList.Add(Detail378);









            return newList;

        }












        #endregion



        #region DRTStructure

        //DRTRoof Type
        private List<PaperAreaModel> GetPaperAreaDRTStructure(string selStructureType, double selRoofOD)
        {
            List<PaperAreaModel> newList = new List<PaperAreaModel>();
            switch (selStructureType)
            {
                case "CenterRingInt":
                    if (selRoofOD <= 24800) //임시값 - 최과장님께 받아야 함.
                    {
                        newList.AddRange(GetPaperAreaDRTCenterRingInternal01());
                    }
                    else
                    {
                        newList.AddRange(GetPaperAreaDRTCenterRingInternal02());
                    }
                    break;

                case "CenterRingExt":
                    if (selRoofOD <= 24800) //임시값 -  최과장님께 받아야 함.
                    {
                        newList.AddRange(GetPaperAreaDRTCenterRingExternal01());
                    }
                    else
                    {
                        newList.AddRange(GetPaperAreaDRTCenterRingExternal02());
                    }
                    break;
            }
            return newList;
        }

        private List<PaperAreaModel> GetPaperAreaDRTCenterRingInternal01() // Assembly, Orientation 1장
        {
            double viewIDNumber = 21000;

            List<PaperAreaModel> newList = new List<PaperAreaModel>();



            return newList;
        }
        private List<PaperAreaModel> GetPaperAreaDRTCenterRingInternal02() //Assembly, Orientation 각 1장씩
        {
            double viewIDNumber = 22000;

            List<PaperAreaModel> newList = new List<PaperAreaModel>();



            return newList;
        }
        private List<PaperAreaModel> GetPaperAreaDRTCenterRingExternal01() //Assembly, Orientation 1장
        {
            double viewIDNumber = 23000;

            List<PaperAreaModel> newList = new List<PaperAreaModel>();



            return newList;
        }
        private List<PaperAreaModel> GetPaperAreaDRTCenterRingExternal02() //Assembly, Orientation 각 1장씩
        {
            double viewIDNumber = 24000;

            List<PaperAreaModel> newList = new List<PaperAreaModel>();



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