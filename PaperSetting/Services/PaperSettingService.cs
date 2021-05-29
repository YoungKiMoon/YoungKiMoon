using AssemblyLib.AssemblyModels;
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

        public ObservableCollection<PaperDwgModel> CreateDrawingCRTList(AssemblyModel assemData)
        {
            ObservableCollection<PaperDwgModel> newList = new ObservableCollection<PaperDwgModel>();

            #region GA



            #endregion

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
                    newTable02.Dock.DockPriority = 1;

                    PaperTableModel newTable03 = new PaperTableModel();
                    newTable03.No = "3";
                    newTable03.Name = "CONNECTION SCHEDULE";
                    newTable03.TableSelection = "ROOF_NOZZLES";
                    newTable03.Dock.DockPosition = DOCKPOSITION_TYPE.BOTTOM;
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
                    cuView.TargetX = "27000";
                    cuView.TargetY = "24000";
                    //cuView.TargetX = "17000";
                    //cuView.TargetY = "14000";
                    cuView.LocationX = "330";
                    cuView.LocationY = "360";
                    cuView.SizeX = "600";
                    cuView.SizeY = "400";

                    newView01.ViewPort = cuView;
                    newPaper01.ViewPorts.Add(newView01);

                    newList.Add(newPaper01);
                }
                else if (newDescription == "GENERAL ASSEMBLY(2-2)")
                {
                    PaperDwgModel newPaper = new PaperDwgModel();

                    // Sheet
                    newPaper.SheetSize = GetSizeModel(PAPERFORMAT_TYPE.A1_ISO);

                    // Basic
                    newPaper.Basic = new PaperBasicModel(true, etcIndex.ToString("00"), newDescription, "VP-210424-MF-" + etcIndex.ToString("000"), "AS BUILT");

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
                else if(newDescription=="1")
                {
                    PaperDwgModel newPaper = new PaperDwgModel();
                    newPaper.SheetSize = GetSizeModel(PAPERFORMAT_TYPE.A1_ISO);

                    // Basic
                    newPaper.Basic = new PaperBasicModel(true, etcIndex.ToString("00"), newDescription, "VP-210424-MF-" + etcIndex.ToString("000"), "AS BUILT");
                    newList.Add(newPaper);
                }

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


        private SizeModel GetSizeModel(PAPERFORMAT_TYPE selPaper)
        {
            SizeModel newSize;
            switch (selPaper)
            {
                case PAPERFORMAT_TYPE.A1_ISO:
                    newSize = new SizeModel("", selPaper, 841, 594);
                    break;
                case PAPERFORMAT_TYPE.A2_ISO:
                    newSize = new SizeModel("", selPaper, 594, 420);
                    break;
                case PAPERFORMAT_TYPE.A3_ISO:
                    newSize = new SizeModel("", selPaper, 420, 297);
                    break;
                default:
                    goto case PAPERFORMAT_TYPE.A1_ISO;
            }
            return newSize;
        }

    }
}
