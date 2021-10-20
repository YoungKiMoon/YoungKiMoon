using AssemblyLib.AssemblyModels;
using DrawSettingLib.Commons;
using DrawSettingLib.SettingModels;
using DrawSettingLib.SettingServices;
using DrawWork.Commons;
using DrawWork.DrawDetailServices;
using DrawWork.DrawGridServices;
using DrawWork.DrawModels;
using DrawWork.DrawSacleServices;
using PaperSetting.Commons;
using PaperSetting.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaperSetting.Services
{
    public class PaperSettingService
    {
        public PaperSettingService()
        {

        }

        public ObservableCollection<PaperDwgModel> CreateDrawingCRTList(AssemblyModel assemData,object selModel)
        {
            ObservableCollection<PaperDwgModel> newList = new ObservableCollection<PaperDwgModel>();

            DrawDetailRoofBottomService roofBottomService = new DrawDetailRoofBottomService(assemData,selModel);

            #region ETC
            List<string> etcName = new List<string>();
            etcName.Add("ORIENTATION");
            etcName.Add("SHELL PLATE ARRANGE");
            etcName.Add("1st COURSE SHELL PLATE");
            etcName.Add("BOTTOM PLATE ARRANGE");

            etcName.Add("ROOF PLATE ARRANGE");
            etcName.Add("ROOF STRUCTURE");
            etcName.Add("SHELL MANHOLE");
            etcName.Add("ROOF MANHOLE");
            etcName.Add("NOZZLE&NOZZLE ACCESSORY");

            etcName.Add("LOADING DATA");
            etcName.Add("SHELL PLATE DEVELOPMENT");
            etcName.Add("CLEANOUT DOOR");
            etcName.Add("WATER SPRAY SYSTEM");
            etcName.Add("AIRFOAM CHAMBER CONN.");

            etcName.Add("AIRFOAM CHAMBER PLATFORM");
            etcName.Add("STAIRWAY");
            etcName.Add("LADDER");
            etcName.Add("ROOF HANDRAIL");
            etcName.Add("INSULATION SUPPORT");

            etcName.Add("EXE. PIPE SUPPORT CLIP");
            etcName.Add("NAME PLATE");
            #endregion


            int etcIndex = 0;
            foreach(DrawingListCRTInputModel eachDrawing in assemData.DrawingListCRTInput)
            {
                etcIndex++;

                string newDescription = (string)eachDrawing.Description.Clone();
                //string newDescription = "GENERAL ASSEMBLY(" + etcIndex + "/2)";

                if (newDescription == "GENERAL ASSEMBLY(1-2)")
                {
                    PaperDwgModel newPaper01 = new PaperDwgModel();
                    newPaper01.Name = PAPERMAIN_TYPE.GA1;

                    // Sheet
                    newPaper01.SheetSize = GetSizeModel(PAPERFORMAT_TYPE.A1_ISO);

                    // Basic
                    newPaper01.Basic = new PaperBasicModel(true, "01", newDescription, "VP-210220-MF-001", "AS BUILT");

                    // Revision : 빈곳
                    PaperRevisionModel newRevision = new PaperRevisionModel();
                    newRevision.RevString = "A";
                    newRevision.Description = "FOR APPROVAL";
                    newRevision.DateString = "21/05/13";
                    newRevision.DRWN = "KDM1";
                    newRevision.CHKD = "KDM2";
                    newRevision.REVD = "KDM3";
                    newRevision.APVD = "KDM4";
                    newPaper01.Revisions.Add(newRevision);

                    // Table
                    // Table : Shell : Nozzle
                    // Table : Sheel : Roof
                    // Table : Tank Specification Data
                    PaperTableModel newTable01 = new PaperTableModel();
                    newTable01.No = "1";
                    newTable01.Name = "TANK SPECIFICATION DATA";
                    newTable01.TableSelection = "TANK_SPECIFICATION_DATA";
                    newTable01.Dock.DockPosition = DOCKPOSITION_TYPE.RIGHT;
                    newTable01.Dock.VerticalAlignment = VERTICALALIGNMENT_TYPE.TOP;
                    newTable01.Dock.DockPriority = 1;


                    PaperTableModel newTable02 = new PaperTableModel();
                    newTable02.No = "2";
                    newTable02.Name = "CONNECTION SCHEDULE";
                    newTable02.TableSelection = "SHELL_NOZZLES";
                    newTable02.Dock.DockPosition = DOCKPOSITION_TYPE.BOTTOM;
                    newTable02.Dock.HorizontalAlignment = HORIZONTALALIGNMENT_TYPE.LEFT;
                    newTable02.Dock.DockPriority = 2;

                    PaperTableModel newTable03 = new PaperTableModel();
                    newTable03.No = "3";
                    newTable03.Name = "CONNECTION SCHEDULE";
                    newTable03.TableSelection = "ROOF_NOZZLES";
                    newTable03.Dock.DockPosition = DOCKPOSITION_TYPE.BOTTOM;
                    newTable03.Dock.HorizontalAlignment = HORIZONTALALIGNMENT_TYPE.LEFT;
                    newTable03.Dock.DockPriority = 3;

                    PaperTableModel newTable04 = new PaperTableModel();
                    newTable04.No = "4";
                    newTable04.Name = "NOZZLE PROJECTION";
                    newTable04.TableSelection = "NOZZLE_PROJECTION";
                    newTable04.Dock.DockPosition = DOCKPOSITION_TYPE.BOTTOM;
                    newTable04.Dock.HorizontalAlignment = HORIZONTALALIGNMENT_TYPE.RIGHT;
                    newTable04.Dock.DockPriority = 4;

                    newPaper01.Tables.Add(newTable01);
                    newPaper01.Tables.Add(newTable02);
                    newPaper01.Tables.Add(newTable03);
                    newPaper01.Tables.Add(newTable04);



                    if (assemData.BottomInput[0].DripRing.ToLower() == "yes")
                    {
                        PaperTableModel newTable05 = new PaperTableModel();
                        newTable05.No = "445";
                        newTable05.Name = "DRIP RING";
                        newTable05.TableSelection = "DRIP RING";
                        newTable05.Dock.DockPosition = DOCKPOSITION_TYPE.FLOATING;
                        newTable05.Dock.HorizontalAlignment = HORIZONTALALIGNMENT_TYPE.RIGHT;
                        newTable05.Dock.DockPriority = 5;
                        newPaper01.Tables.Add(newTable05);
                    }

                    // Note : 없음

                    if (true)
                    {

                    
                        // ViewPort
                        PaperViewportModel newView01 = new PaperViewportModel();
                        newView01.No = "00";
                        newView01.Name = "GA";
                        newView01.AssemblySelection = "GeneralAssembly";

                        ViewPortModel cuView = new ViewPortModel();
                        cuView.ScaleStr = "90";
                        cuView.TargetX = "27000";
                        cuView.TargetY = "24000";
                        //cuView.TargetX = "17000";
                        //cuView.TargetY = "14000";
                        cuView.LocationX = "334";
                        cuView.LocationY = "363";
                        cuView.SizeX = "600";
                        cuView.SizeY = "400";

                        newView01.ViewPort = cuView;
                        newPaper01.ViewPorts.Add(newView01);
                    }

                    newList.Add(newPaper01);
                }
                else if (newDescription == "GENERAL ASSEMBLY(2-2)")
                {
                    PaperDwgModel newPaper = new PaperDwgModel();
                    newPaper.Name = PAPERMAIN_TYPE.GA2;

                    // Sheet
                    newPaper.SheetSize = GetSizeModel(PAPERFORMAT_TYPE.A1_ISO);

                    // Basic
                    newPaper.Basic = new PaperBasicModel(true, etcIndex.ToString("02"), newDescription, "VP-210424-MF-" + etcIndex.ToString("000"), "AS BUILT");

                    // Note
                    PaperNoteModel newNote01 = new PaperNoteModel();
                    newNote01.No = "1";
                    newNote01.Name = "GENERAL NOTES";
                    newNote01.Note = "GENERAL NOTES";
                    newNote01.Dock.DockPosition = DOCKPOSITION_TYPE.FLOATING;
                    newNote01.Dock.HorizontalAlignment = HORIZONTALALIGNMENT_TYPE.LEFT;
                    newNote01.Dock.DockPriority = 1;
                    newNote01.Location.X = 7+50;
                    newNote01.Location.Y = 594-7-50;

                    newPaper.Notes.Add(newNote01);

                    newList.Add(newPaper);
                }
                else if(newDescription=="NOZZLE ORIENTATION")
                {
                    PaperDwgModel newPaper01 = new PaperDwgModel();
                    newPaper01.Name = PAPERMAIN_TYPE.ORIENTATION;

                    // Sheet
                    newPaper01.SheetSize = GetSizeModel(PAPERFORMAT_TYPE.A1_ISO);

                    // Basic
                    newPaper01.Basic = new PaperBasicModel(true, "03", newDescription, "VP-210220-MF-001", "AS BUILT");

                    // Revision : 빈곳
                    PaperRevisionModel newRevision = new PaperRevisionModel();
                    newRevision.RevString = "A";
                    newRevision.Description = "FOR APPROVAL";
                    newRevision.DateString = "21/05/13";
                    newRevision.DRWN = "KDM1";
                    newRevision.CHKD = "KDM2";
                    newRevision.REVD = "KDM3";
                    newRevision.APVD = "KDM4";
                    newPaper01.Revisions.Add(newRevision);

                    // Table
                    // Table : Shell : Nozzle
                    // Table : Sheel : Roof
                    // Table : Tank Specification Data
                    
                    PaperTableModel newTable02 = new PaperTableModel();
                    newTable02.No = "1";
                    newTable02.Name = "CONNECTION SCHEDULE";
                    newTable02.TableSelection = "SHELL_NOZZLES";
                    newTable02.Dock.DockPosition = DOCKPOSITION_TYPE.RIGHT;
                    newTable02.Dock.VerticalAlignment = VERTICALALIGNMENT_TYPE.TOP;
                    newTable02.Dock.DockPriority = 1;

                    PaperTableModel newTable03 = new PaperTableModel();
                    newTable03.No = "2";
                    newTable03.Name = "CONNECTION SCHEDULE";
                    newTable03.TableSelection = "ROOF_NOZZLES";
                    newTable03.Dock.DockPosition = DOCKPOSITION_TYPE.RIGHT;
                    newTable03.Dock.VerticalAlignment = VERTICALALIGNMENT_TYPE.TOP;
                    newTable03.Dock.DockPriority = 1;

                    PaperTableModel newTable04 = new PaperTableModel();
                    newTable04.No = "3";
                    newTable04.Name = "NOZZLE PROJECTION";
                    newTable04.TableSelection = "NOZZLE_PROJECTION";
                    newTable04.Dock.DockPosition = DOCKPOSITION_TYPE.BOTTOM;
                    newTable04.Dock.HorizontalAlignment = HORIZONTALALIGNMENT_TYPE.RIGHT;
                    newTable04.Dock.DockPriority = 3;

                    PaperTableModel newTable05 = new PaperTableModel();
                    newTable05.No = "4";
                    newTable05.Name = "DIRECTION MARK";
                    newTable05.TableSelection = "DIRECTION_MARK";
                    newTable05.Dock.DockPosition = DOCKPOSITION_TYPE.FLOATING;
                    newTable05.Dock.HorizontalAlignment = HORIZONTALALIGNMENT_TYPE.RIGHT;
                    newTable05.Dock.DockPriority = 4;

                    PaperTableModel newTable06 = new PaperTableModel();
                    newTable06.No = "995";
                    newTable06.Name = "ETC POSITION MARK";
                    newTable06.TableSelection = "ETC_POSITION_MARK";
                    newTable06.Dock.DockPosition = DOCKPOSITION_TYPE.RIGHT;
                    newTable06.Dock.VerticalAlignment = VERTICALALIGNMENT_TYPE.TOP;
                    newTable06.Dock.DockPriority = 4;

                    newPaper01.Tables.Add(newTable02);
                    newPaper01.Tables.Add(newTable03);
                    newPaper01.Tables.Add(newTable04);
                    newPaper01.Tables.Add(newTable05);
                    newPaper01.Tables.Add(newTable06);

                    newList.Add(newPaper01);



                    if (true)
                    {


                        // ViewPort
                        PaperViewportModel newView01 = new PaperViewportModel();
                        newView01.No = "00";
                        newView01.Name = "NozzleOrientation";
                        newView01.AssemblySelection = "NozzleOrientation";

                        ViewPortModel cuView = new ViewPortModel();
                        cuView.ScaleStr = "90";
                        cuView.TargetX = "27000";
                        cuView.TargetY = "24000";
                        //cuView.TargetX = "17000";
                        //cuView.TargetY = "14000";
                        cuView.LocationX = "280";
                        cuView.LocationY = "297";
                        cuView.SizeX = "500";
                        cuView.SizeY = "570";

                        newView01.ViewPort = cuView;
                        newPaper01.ViewPorts.Add(newView01);
                    }
                }
                else if(newDescription=="1")
                {
                    PaperDwgModel newPaper = new PaperDwgModel();
                    newPaper.SheetSize = GetSizeModel(PAPERFORMAT_TYPE.A1_ISO);

                    // Basic
                    newPaper.Basic = new PaperBasicModel(true, etcIndex.ToString("00"), newDescription, "VP-210424-MF-" + etcIndex.ToString("000"), "AS BUILT");
                    newList.Add(newPaper);
                }
                else if (newDescription == "SHELL PLATE ARRANGEMENT")
                {
                    PaperDwgModel newPaper1 = new PaperDwgModel();
                    newPaper1.Name = PAPERMAIN_TYPE.ShellPlateArrangement;
                    newPaper1.Page = 1;

                    newPaper1.SheetSize = GetSizeModel(PAPERFORMAT_TYPE.A1_ISO);
                    newPaper1.RowDef = 3;
                    newPaper1.ColumnDef = 4;
                    // Basic
                    newPaper1.Basic = new PaperBasicModel(true, etcIndex.ToString("00"), newDescription, "VP-210424-MF-" + etcIndex.ToString("000"), "AS BUILT");
                    newList.Add(newPaper1);



                    PaperTableModel newTable01 = new PaperTableModel();
                    newTable01.No = "1";
                    newTable01.Name = "Shell Plate BM";
                    newTable01.TableSelection = "Shell_PLATE_BM";
                    newTable01.Dock.DockPosition = DOCKPOSITION_TYPE.RIGHT;
                    newTable01.Dock.VerticalAlignment = VERTICALALIGNMENT_TYPE.TOP;
                    newTable01.Dock.DockPriority = 1;


                    newPaper1.Tables.Add(newTable01);



                    PaperDwgModel newPaper2 = new PaperDwgModel();
                    newPaper2.Name = PAPERMAIN_TYPE.ShellPlateArrangement;
                    newPaper2.Page = 2;

                    newPaper2.SheetSize = GetSizeModel(PAPERFORMAT_TYPE.A1_ISO);
                    newPaper2.RowDef = 4;
                    newPaper2.ColumnDef = 4;
                    // Basic
                    newPaper2.Basic = new PaperBasicModel(true, etcIndex.ToString("00"), newDescription, "VP-210424-MF-" + etcIndex.ToString("000"), "AS BUILT");
                    newList.Add(newPaper2);



                }
                else if (newDescription == "1st COURSE SHELL PLATE")
                {
                    // Course 수 만큼
                    double coursePageCount = 0;
                    foreach(PaperAreaModel eachArea in SingletonData.PaperArea.AreaList)
                    {
                        if (eachArea.DWGName == PAPERMAIN_TYPE.CourseShellPlate)
                        {
                            coursePageCount++;

                            PaperDwgModel newPaper3 = new PaperDwgModel();
                            newPaper3.Name = PAPERMAIN_TYPE.CourseShellPlate;
                            newPaper3.Page = coursePageCount;

                            newPaper3.SheetSize = GetSizeModel(PAPERFORMAT_TYPE.A1_ISO);
                            newPaper3.RowDef = 1;
                            newPaper3.ColumnDef = 1;
                            // Basic
                            newPaper3.Basic = new PaperBasicModel(true, etcIndex.ToString("00"), newDescription, "VP-210424-MF-" + etcIndex.ToString("000"), "AS BUILT");
                            newList.Add(newPaper3);
                        }
                    }

                }
                else if (newDescription == "BOTTOM PLATE ARRANGEMENT")
                {


                    double selTankOD = roofBottomService.GetBottomRoofOD();
                    //bool selAnnular = assemData.BottomInput[0].AnnularPlate.Contains("yes");

                    if (selTankOD <= 24800)
                    {
                        //Annular Yes, OD <=24800
                        PaperDwgModel newPaper1 = new PaperDwgModel();
                        newPaper1.Name = PAPERMAIN_TYPE.BottomPlateArrangement;
                        newPaper1.Page = 1;

                        newPaper1.SheetSize = GetSizeModel(PAPERFORMAT_TYPE.A1_ISO);
                        newPaper1.RowDef = 4;
                        newPaper1.ColumnDef = 4;
                        // Basic
                        newPaper1.Basic = new PaperBasicModel(true, etcIndex.ToString("00"), newDescription, "VP-210424-MF-" + etcIndex.ToString("000"), "AS BUILT");
                        newList.Add(newPaper1);

                    }
                    else
                    {
                        //Annular Yes, OD < 24800

                        PaperDwgModel newPaper1 = new PaperDwgModel();
                        newPaper1.Name = PAPERMAIN_TYPE.BottomPlateArrangement;
                        newPaper1.Page = 1;

                        newPaper1.SheetSize = GetSizeModel(PAPERFORMAT_TYPE.A1_ISO);
                        newPaper1.RowDef = 4;
                        newPaper1.ColumnDef = 4;
                        // Basic
                        newPaper1.Basic = new PaperBasicModel(true, etcIndex.ToString("00"), newDescription, "VP-210424-MF-" + etcIndex.ToString("000"), "AS BUILT");
                        newList.Add(newPaper1);

                        PaperDwgModel newPaper2 = new PaperDwgModel();
                        newPaper2.Name = PAPERMAIN_TYPE.BottomPlateArrangement;
                        newPaper2.Page = 2;

                        newPaper2.SheetSize = GetSizeModel(PAPERFORMAT_TYPE.A1_ISO);
                        newPaper2.RowDef = 4;
                        newPaper2.ColumnDef = 4;
                        // Basic
                        newPaper2.Basic = new PaperBasicModel(true, etcIndex.ToString("00"), newDescription, "VP-210424-MF-" + etcIndex.ToString("000"), "AS BUILT");
                        newList.Add(newPaper2);

                    }


                }

                else if (newDescription == "BOTTOM PLATE CUTTING PLAN")
                {

                    if (SingletonData.BottomPlateList.Count > 0)
                    {
                        
                        double refRow = 4;
                        double refColumn = 3;
                        double onePageCount = refRow * refColumn;
                        double cuttingPlanItemCount = SingletonData.BottomPlateList.Count + SingletonData.BottomAnnularPlateList.Count;
                        double cuttingPageCount = Math.Ceiling(cuttingPlanItemCount / onePageCount);
                        for (int i = 0; i < cuttingPageCount; i++)
                        {

                            PaperDwgModel newPaper1 = new PaperDwgModel();
                            newPaper1.Name = PAPERMAIN_TYPE.BottomPlateCuttingPlan;
                            newPaper1.Page = i + 1;

                            newPaper1.SheetSize = GetSizeModel(PAPERFORMAT_TYPE.A1_ISO);
                            newPaper1.RowDef = refRow;
                            newPaper1.ColumnDef = refColumn;
                            // Basic
                            newPaper1.Basic = new PaperBasicModel(true, etcIndex.ToString("00"), newDescription, "VP-210424-MF-" + etcIndex.ToString("000"), "AS BUILT");
                            newList.Add(newPaper1);

                            if (i == 0)
                            {
                                PaperTableModel newTable01 = new PaperTableModel();
                                newTable01.No = "1";
                                newTable01.Name = "Bottom Plate BM";
                                newTable01.TableSelection = "Bottom_PLATE_BM";
                                newTable01.Dock.DockPosition = DOCKPOSITION_TYPE.RIGHT;
                                newTable01.Dock.VerticalAlignment = VERTICALALIGNMENT_TYPE.TOP;
                                newTable01.Dock.DockPriority = 1;


                                newPaper1.Tables.Add(newTable01);
                            }
                        }
                    }

                        


                }

                // Roof
                else if (newDescription == "ROOF PLATE ARRANGEMENT")
                {


                    double selTankOD = roofBottomService.GetBottomRoofOD();
                    //bool selAnnular = assemData.BottomInput[0].AnnularPlate.Contains("yes");

                    if (selTankOD <= 24800)
                    {
                        //Annular Yes, OD <=24800
                        PaperDwgModel newPaper1 = new PaperDwgModel();
                        newPaper1.Name = PAPERMAIN_TYPE.RoofPlateArrangement;
                        newPaper1.Page = 1;

                        newPaper1.SheetSize = GetSizeModel(PAPERFORMAT_TYPE.A1_ISO);
                        newPaper1.RowDef = 4;
                        newPaper1.ColumnDef = 4;
                        // Basic
                        newPaper1.Basic = new PaperBasicModel(true, etcIndex.ToString("00"), newDescription, "VP-210424-MF-" + etcIndex.ToString("000"), "AS BUILT");
                        newList.Add(newPaper1);

                    }
                    else
                    {
                        //Annular Yes, OD < 24800

                        PaperDwgModel newPaper1 = new PaperDwgModel();
                        newPaper1.Name = PAPERMAIN_TYPE.RoofPlateArrangement;
                        newPaper1.Page = 1;

                        newPaper1.SheetSize = GetSizeModel(PAPERFORMAT_TYPE.A1_ISO);
                        newPaper1.RowDef = 4;
                        newPaper1.ColumnDef = 4;
                        // Basic
                        newPaper1.Basic = new PaperBasicModel(true, etcIndex.ToString("00"), newDescription, "VP-210424-MF-" + etcIndex.ToString("000"), "AS BUILT");
                        newList.Add(newPaper1);

                        PaperDwgModel newPaper2 = new PaperDwgModel();
                        newPaper2.Name = PAPERMAIN_TYPE.RoofPlateArrangement;
                        newPaper2.Page = 2;

                        newPaper2.SheetSize = GetSizeModel(PAPERFORMAT_TYPE.A1_ISO);
                        newPaper2.RowDef = 4;
                        newPaper2.ColumnDef = 4;
                        // Basic
                        newPaper2.Basic = new PaperBasicModel(true, etcIndex.ToString("00"), newDescription, "VP-210424-MF-" + etcIndex.ToString("000"), "AS BUILT");
                        newList.Add(newPaper2);

                    }


                }

                else if (newDescription == "ROOF PLATE CUTTING PLAN")
                {

                    if (SingletonData.RoofPlateList.Count > 0)
                    {
                        double refRow = 4;
                        double refColumn = 3;
                        double onePageCount = refRow * refColumn;
                        double cuttingPlanItemCount = SingletonData.RoofPlateList.Count + SingletonData.RoofComRingPlateList.Count;
                        double cuttingPageCount = Math.Ceiling(cuttingPlanItemCount / onePageCount);
                        for (int i = 0; i < cuttingPageCount; i++)
                        {

                            PaperDwgModel newPaper1 = new PaperDwgModel();
                            newPaper1.Name = PAPERMAIN_TYPE.RoofPlateCuttingPlan;
                            newPaper1.Page = i + 1;

                            newPaper1.SheetSize = GetSizeModel(PAPERFORMAT_TYPE.A1_ISO);
                            newPaper1.RowDef = refRow;
                            newPaper1.ColumnDef = refColumn;
                            // Basic
                            newPaper1.Basic = new PaperBasicModel(true, etcIndex.ToString("00"), newDescription, "VP-210424-MF-" + etcIndex.ToString("000"), "AS BUILT");
                            newList.Add(newPaper1);


                            if (i == 0)
                            {
                                PaperTableModel newTable01 = new PaperTableModel();
                                newTable01.No = "1";
                                newTable01.Name = "Roof Plate BM";
                                newTable01.TableSelection = "Roof_PLATE_BM";
                                newTable01.Dock.DockPosition = DOCKPOSITION_TYPE.RIGHT;
                                newTable01.Dock.VerticalAlignment = VERTICALALIGNMENT_TYPE.TOP;
                                newTable01.Dock.DockPriority = 1;


                                newPaper1.Tables.Add(newTable01);
                            }
                        }
                    }


                }

                else if (newDescription=="DETAIL OF ROOF STRUCTURE")
                {
                    List<PaperModel> eachPaperList = new List<PaperModel>();
                    // 현재 CRT만 고려됨
                    if (SingletonData.TankType == TANK_TYPE.CRT)
                    {
                        // Column 타입만 고려됨
                        if (assemData.StructureCRTInput[0].SupportingType.ToLower().Contains("column"))
                        {
                            double columnCount = assemData.StructureCRTColumnInput.Count;
                            PaperService paperService = new PaperService();
                            eachPaperList= paperService.GetPaperModel_StructureColumnType(columnCount);
                        }
                    }

                    foreach(PaperModel eachModel in eachPaperList)
                    {
                        PaperDwgModel newPaper = new PaperDwgModel();
                        newPaper.Name = eachModel.DWGName;
                        newPaper.Page = eachModel.Page;

                        newPaper.SheetSize = GetSizeModel(PAPERFORMAT_TYPE.A1_ISO);
                        newPaper.RowDef = eachModel.RowDef;
                        newPaper.ColumnDef = eachModel.ColumnDef;
                        // Basic
                        newPaper.Basic = new PaperBasicModel(true, etcIndex.ToString("00"), newDescription, "VP-210424-MF-" + etcIndex.ToString("000"), "AS BUILT");
                        newList.Add(newPaper);
                    }
                }


            }



            return newList;
        }



        // Grid 
        public ObservableCollection<DrawPaperGridModel> CreatePaperGridList(ObservableCollection<PaperDwgModel> selList)
        {
            ObservableCollection<DrawPaperGridModel> newList = new ObservableCollection<DrawPaperGridModel>();
            
            foreach(PaperDwgModel eachDwg in selList)
            {
                DrawPaperGridModel newGrid = new DrawPaperGridModel();

                newGrid.Name = eachDwg.Name;
                newGrid.Page = eachDwg.Page;
                // Adjust
                double widthAdj= -(7 * 2) - 2 - 185;  // 7 Frame, 2 left Gap, 185 Right Area
                double heightAdj = -(7 * 2) - 10;     // 7 Frame, 10 Bottom Gap

                newGrid.Size.Width = eachDwg.SheetSize.Width + widthAdj;
                newGrid.Size.Height = eachDwg.SheetSize.Height + heightAdj;

                newGrid.Location.X = 7 + 2;
                newGrid.Location.Y = 7 + 10;

                newGrid.ColumnDef = eachDwg.ColumnDef;
                newGrid.RowDef = eachDwg.RowDef;

                newList.Add(newGrid);
            }

            return newList;
        }


        public ObservableCollection<PaperDwgModel> CreateDrawingCRTList()
        {
            ObservableCollection<PaperDwgModel> newList = new ObservableCollection<PaperDwgModel>();

            #region GA
            PaperDwgModel newPaper01 = new PaperDwgModel();

            // Sheet
            newPaper01.SheetSize = GetSizeModel(PAPERFORMAT_TYPE.A2_ISO);

            // Basic
            newPaper01.Basic = new PaperBasicModel(true, "01", "GENERAL ASSEMBLY(1/2)", "VP-210220-MF-001", "AS BUILT");

            // Revision : 빈곳
            PaperRevisionModel newRevision = new PaperRevisionModel();
            newRevision.RevString = "A";
            newRevision.Description = "FOR APPROVAL";
            newRevision.DateString = "21/05/13";
            newRevision.DRWN = "KDM1";
            newRevision.CHKD = "KDM2";
            newRevision.REVD = "KDM3";
            newRevision.APVD = "KDM4";
            newPaper01.Revisions.Add(newRevision);

            // Table
            // Table : Shell : Nozzle
            // Table : Sheel : Roof
            // Table : Tank Specification Data
            PaperTableModel newTable01 = new PaperTableModel();
            newTable01.No = "1";
            newTable01.Name = "TANK SPECIFICATION DATA";
            newTable01.TableSelection = "TANK_SPECIFICATION_DATA";
            newTable01.Dock.DockPosition = DOCKPOSITION_TYPE.RIGHT;
            newTable01.Dock.VerticalAlignment = VERTICALALIGNMENT_TYPE.TOP;
            newTable01.Dock.DockPriority = 1;

            PaperTableModel newTable02 = new PaperTableModel();
            newTable02.No = "2";
            newTable02.Name = "CONNECTION SCHEDULE";
            newTable02.TableSelection = "SHELL_NOZZLES";
            newTable02.Dock.DockPosition = DOCKPOSITION_TYPE.BOTTOM;
            newTable02.Dock.HorizontalAlignment = HORIZONTALALIGNMENT_TYPE.LEFT;
            newTable02.Dock.DockPriority = 1;

            PaperTableModel newTable03 = new PaperTableModel();
            newTable03.No = "3";
            newTable03.Name = "CONNECTION SCHEDULE";
            newTable03.TableSelection = "ROOF_NOZZLES";
            newTable03.Dock.DockPosition = DOCKPOSITION_TYPE.RIGHT;
            newTable03.Dock.HorizontalAlignment = HORIZONTALALIGNMENT_TYPE.LEFT;
            newTable03.Dock.DockPriority = 1;

            newPaper01.Tables.Add(newTable01);
            newPaper01.Tables.Add(newTable02);
            newPaper01.Tables.Add(newTable03);


            // Note : 없음


            // ViewPort
            PaperViewportModel newView01 = new PaperViewportModel();
            newView01.No = "00";
            newView01.Name = "GA";
            newView01.AssemblySelection = "GeneralAssembly";

            ViewPortModel cuView = new ViewPortModel();
            cuView.ScaleStr = "90";
            cuView.TargetX = "17000";
            cuView.TargetY = "14000";
            cuView.LocationX = "330";
            cuView.LocationY = "360";
            cuView.SizeX = "600";
            cuView.SizeY = "400";

            newView01.ViewPort = cuView;
            newPaper01.ViewPorts.Add(newView01);

            newList.Add(newPaper01);



            PaperDwgModel newPaper02 = new PaperDwgModel();

            // Sheet
            newPaper02.SheetSize = GetSizeModel(PAPERFORMAT_TYPE.A2_ISO);

            // Basic
            newPaper02.Basic = new PaperBasicModel(true, "02", "GENERAL ASSEMBLY(2/2)","VP-210220-MF-002", "AS BUILT");

            // Note
            PaperNoteModel newNote01 = new PaperNoteModel();
            newNote01.No = "1";
            newNote01.Name = "GENERAL NOTES";
            newNote01.Note = "GENERAL NOTES";
            newNote01.Dock.DockPosition = DOCKPOSITION_TYPE.FLOATING;
            newNote01.Dock.HorizontalAlignment = HORIZONTALALIGNMENT_TYPE.LEFT;
            newNote01.Dock.DockPriority = 1;
            newNote01.Location.X = 7 + 50;
            newNote01.Location.Y = 420 - 7 - 50;

            newPaper02.Notes.Add(newNote01);

            newList.Add(newPaper02);




            #endregion

            #region ETC
            List<string> etcName = new List<string>();
            //etcName.Add("ORIENTATION");
            etcName.Add("SHELL PLATE ARRANGE");
            etcName.Add("1st COURSE SHELL PLATE");
            etcName.Add("BOTTOM PLATE ARRANGE");

            etcName.Add("ROOF PLATE ARRANGE");
            etcName.Add("ROOF STRUCTURE");
            etcName.Add("SHELL MANHOLE");
            etcName.Add("ROOF MANHOLE");
            etcName.Add("NOZZLE&NOZZLE ACCESSORY");

            etcName.Add("LOADING DATA");
            etcName.Add("SHELL PLATE DEVELOPMENT");
            etcName.Add("CLEANOUT DOOR");
            etcName.Add("WATER SPRAY SYSTEM");
            etcName.Add("AIRFOAM CHAMBER CONN.");

            etcName.Add("AIRFOAM CHAMBER PLATFORM");
            etcName.Add("STAIRWAY");
            etcName.Add("LADDER");
            etcName.Add("ROOF HANDRAIL");
            etcName.Add("INSULATION SUPPORT");

            etcName.Add("EXE. PIPE SUPPORT CLIP");
            etcName.Add("NAME PLATE");

            int etcIndex = 0;
            //foreach (string eachName in etcName)
            //{
            //    etcIndex++;
            //    PaperDwgModel newPaper = new PaperDwgModel();
            //    newPaper.SheetSize = GetSizeModel(PAPERFORMAT_TYPE.A2_ISO);

            //    // Basic
            //    newPaper.Basic = new PaperBasicModel(true, etcIndex.ToString("00"), eachName, "VP-210424-MF-" + etcIndex.ToString("000"), "AS BUILT");
            //    newList.Add(newPaper);
            //}

            #endregion

            return newList;
        }


        public void SetCuttingModel()
        {
            DrawScaleService scaleService = new DrawScaleService();

            // 이거 매우 중요함 : 페이지에 종속 되어야 하나, 여기서는 강제 적용
            double refRow = 4;
            double refColumn = 3;
            double pageMaxCount = refRow * refColumn;

            double itemCount = 0;
            double pageCount = 0;

            // Roof
            itemCount = 0;
            pageCount = 0;
            PaperAreaModel roofCuttingPlanModel = SingletonData.PaperArea.GetAreaModel(PAPERMAIN_TYPE.DETAIL, PAPERSUB_TYPE.RoofPlateCuttingPlan);
            // 삭제 매우 중요함
            SingletonData.PaperArea.AreaList.Remove(roofCuttingPlanModel);
            List<PaperAreaModel> roofAreaModel = new List<PaperAreaModel>();
            foreach(DrawOnePlateModel eachCutting in SingletonData.RoofPlateList)
            {
                itemCount++;
                pageCount = Math.Ceiling(itemCount / pageMaxCount);

                PaperAreaModel newModel = roofCuttingPlanModel.CustomClone();

                // Page Number
                newModel.Page = pageCount;
                // Visible
                newModel.visible = true;
                // Scale
                newModel.ScaleValue = scaleService.GetScaleCalValue(newModel.otherWidth, newModel.otherHeight, eachCutting.PlateLength, eachCutting.PlateWidth);
                // Model CenterPoint
                newModel.ModelCenterLocation.X = eachCutting.CenterPoint.X +10* newModel.ScaleValue;
                newModel.ModelCenterLocation.Y = eachCutting.CenterPoint.Y;

                // Sub Title
                //newModel.TitleName="R" + "-" + newModel.TitleName;

                // View ID
                newModel.viewID += itemCount;
                roofAreaModel.Add(newModel);
            }
            SingletonData.PaperArea.AreaList.AddRange(roofAreaModel);



            // Roof : Compression Ring
            PaperAreaModel roofComRingCuttingPlanModel = SingletonData.PaperArea.GetAreaModel(PAPERMAIN_TYPE.DETAIL, PAPERSUB_TYPE.RoofCompressionRingCuttingPlan);
            // 삭제 매우 중요함
            SingletonData.PaperArea.AreaList.Remove(roofComRingCuttingPlanModel);
            List<PaperAreaModel> roofComRingAreaModel = new List<PaperAreaModel>();
            foreach (DrawOnePlateModel eachCutting in SingletonData.RoofComRingPlateList)
            {
                itemCount++;
                pageCount = Math.Ceiling(itemCount / pageMaxCount);

                PaperAreaModel newModel = roofComRingCuttingPlanModel.CustomClone();

                // Page Number
                newModel.Page = pageCount;
                // Visible
                newModel.visible = true;
                // Scale
                newModel.ScaleValue = scaleService.GetScaleCalValue(newModel.otherWidth, newModel.otherHeight, eachCutting.PlateLength, eachCutting.PlateWidth);
                // Model CenterPoint
                newModel.ModelCenterLocation.X = eachCutting.CenterPoint.X + 10 * newModel.ScaleValue;
                newModel.ModelCenterLocation.Y = eachCutting.CenterPoint.Y;

                // Sub Title
                //newModel.TitleName=eachCutting.displayName + "-" + newModel.TitleName;

                newModel.TitleSubName = "(SCALE 1/" + newModel.ScaleValue + ")";

                // View ID
                newModel.viewID += itemCount;
                roofComRingAreaModel.Add(newModel);
            }
            SingletonData.PaperArea.AreaList.AddRange(roofComRingAreaModel);




            // Bottom
            itemCount = 0;
            pageCount = 0;
            PaperAreaModel bottomCuttingPlanModel = SingletonData.PaperArea.GetAreaModel(PAPERMAIN_TYPE.DETAIL, PAPERSUB_TYPE.BottomPlateCuttingPlan);
            // 삭제 매우 중요함
            SingletonData.PaperArea.AreaList.Remove(bottomCuttingPlanModel);
            List<PaperAreaModel> bottomAreaModel = new List<PaperAreaModel>();
            foreach (DrawOnePlateModel eachCutting in SingletonData.BottomPlateList)
            {
                itemCount++;
                pageCount = Math.Ceiling(itemCount / pageMaxCount);

                PaperAreaModel newModel = bottomCuttingPlanModel.CustomClone();

                // Page Number
                newModel.Page = pageCount;
                // Visible
                newModel.visible = true;
                // Scale
                newModel.ScaleValue = scaleService.GetScaleCalValue(newModel.otherWidth, newModel.otherHeight, eachCutting.PlateLength, eachCutting.PlateWidth);
                // Model CenterPoint
                newModel.ModelCenterLocation.X = eachCutting.CenterPoint.X+ 10 * newModel.ScaleValue;
                newModel.ModelCenterLocation.Y = eachCutting.CenterPoint.Y;

                // Sub Title
                //newModel.TitleName = "B"+ "-" + newModel.TitleName;

                // View ID
                newModel.viewID += itemCount;
                bottomAreaModel.Add(newModel);
            }
            SingletonData.PaperArea.AreaList.AddRange(bottomAreaModel);



            // Bottom : Annular
            PaperAreaModel bottomAnnularCuttingPlanModel = SingletonData.PaperArea.GetAreaModel(PAPERMAIN_TYPE.DETAIL, PAPERSUB_TYPE.AnnularPlateCuttingPlan);
            // 삭제 매우 중요함
            SingletonData.PaperArea.AreaList.Remove(bottomAnnularCuttingPlanModel);
            List<PaperAreaModel> bottomAnnularAreaModel = new List<PaperAreaModel>();
            foreach (DrawOnePlateModel eachCutting in SingletonData.BottomAnnularPlateList)
            {
                itemCount++;
                pageCount = Math.Ceiling(itemCount / pageMaxCount);

                PaperAreaModel newModel = bottomAnnularCuttingPlanModel.CustomClone();

                // Page Number
                newModel.Page = pageCount;
                // Visible
                newModel.visible = true;
                // Scale
                newModel.ScaleValue = scaleService.GetScaleCalValue(newModel.otherWidth, newModel.otherHeight, eachCutting.PlateLength, eachCutting.PlateWidth);
                // Model CenterPoint
                newModel.ModelCenterLocation.X = eachCutting.CenterPoint.X + 10 * newModel.ScaleValue;
                newModel.ModelCenterLocation.Y = eachCutting.CenterPoint.Y;

                // Sub Title
                //newModel.TitleName=eachCutting.displayName + "-" + newModel.TitleName;
                newModel.TitleSubName="(SCALE 1/" + newModel.ScaleValue +")";
                // View ID
                newModel.viewID += itemCount;
                bottomAnnularAreaModel.Add(newModel);
            }
            SingletonData.PaperArea.AreaList.AddRange(bottomAnnularAreaModel);



            // Backing Strip
            itemCount++;
            pageCount = Math.Ceiling(itemCount / pageMaxCount);
            PaperAreaModel backingStripCuttingPlanModel = SingletonData.PaperArea.GetAreaModel(PAPERMAIN_TYPE.DETAIL, PAPERSUB_TYPE.BackingStrip);
            backingStripCuttingPlanModel.Page = pageCount;


            // Priority : 매우 중요
            SingletonData.PaperArea.AreaList = SingletonData.PaperArea.AreaList.OrderByDescending(x => x.IsFix).ThenBy(x=>x.Priority).ToList();
            //SingletonData.PaperArea.AreaList.Sort((x, y) => x.Priority.CompareTo(y.Priority));
        }


        private PaperSizeModel GetSizeModel(PAPERFORMAT_TYPE selPaper)
        {
            PaperSizeModel newSize;
            switch (selPaper)
            {
                case PAPERFORMAT_TYPE.A1_ISO:
                    newSize = new PaperSizeModel("", selPaper, 841, 594);
                    break;
                case PAPERFORMAT_TYPE.A2_ISO:
                    newSize = new PaperSizeModel("", selPaper, 594, 420);
                    break;
                case PAPERFORMAT_TYPE.A3_ISO:
                    newSize = new PaperSizeModel("", selPaper, 420, 297);
                    break;
                default:
                    goto case PAPERFORMAT_TYPE.A1_ISO;
            }
            return newSize;
        }

    }
}
