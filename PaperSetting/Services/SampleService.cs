using PaperSetting.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PaperSetting.Services
{
    public static class SampleService
    {
        public static ObservableCollection<PaperDwgModel> CreatePaperSample()
        {
            ObservableCollection<PaperDwgModel> newModel = new ObservableCollection<PaperDwgModel>();
            List<string> dwgList = GetDWGList();

            int paperNo = 0;
            foreach(string eachDwg in dwgList)
            {
                paperNo++;
                PaperDwgModel newPaper = new PaperDwgModel();

                // Basic
                newPaper.Basic.No = paperNo.ToString();
                newPaper.Basic.Title = eachDwg;
                newPaper.Basic.DwgNo = "VP-210220-MF-" + paperNo.ToString("000");
                newPaper.Basic.StampName = "AS BUILT";

                int subCount = 4;
                for(int i = 1; i <= subCount; i++)
                {
                    // Revision
                    PaperRevisionModel newRevision = new PaperRevisionModel();
                    newRevision.RevString = i.ToString();
                    newRevision.DateString = "JAN." + i.ToString("00") + ".2021";
                    newRevision.Description = "FOR INFORMATION";
                    newRevision.PreparedName = "-";
                    newRevision.CheckedName = "-";
                    newRevision.ApprovedName = "-";
                    newPaper.Revisions.Add(newRevision);

                    // Table
                    PaperTableModel newTable = new PaperTableModel();
                    newTable.No = i.ToString();
                    newTable.Name = "Table" + i.ToString("00");
                    newTable.TableSelection = "CustomTable" + i.ToString("00");
                    newPaper.Tables.Add(newTable);

                    // Viewport
                    PaperViewportModel newViewport = new PaperViewportModel();
                    newViewport.No = i.ToString();
                    newViewport.Name = "ViewPort" + i.ToString("00");
                    newViewport.AssemblySelection = "CustomAssembly" + i.ToString("00");
                    newPaper.ViewPorts.Add(newViewport);

                    // Note
                    PaperNoteModel newNote = new PaperNoteModel();
                    newNote.No = i.ToString();
                    newNote.Name = "Note" + i.ToString("00");
                    newNote.Note = "CustomNote" + i.ToString("00");
                    newPaper.Notes.Add(newNote);
                }

                newModel.Add(newPaper);
            }
            return newModel;
        }

        public static List<string> GetDWGList()
        {

            List<string> newList = new List<string>();
            newList.Add("GENERAL ASSEMBLY");
            newList.Add("ORIENTATION");
            newList.Add("SHELL PLATE ARRANGE");
            newList.Add("1st COURSE SHELL PLATE");
            newList.Add("BOTTOM PLATE ARRANGE");

            newList.Add("ROOF PLATE ARRANGE");
            newList.Add("ROOF STRUCTURE");
            newList.Add("SHELL MANHOLE");
            newList.Add("ROOF MANHOLE");
            newList.Add("NOZZLE&NOZZLE ACCESSORY");

            newList.Add("LOADING DATA");
            newList.Add("SHELL PLATE DEVELOPMENT");
            newList.Add("CLEANOUT DOOR");
            newList.Add("WATER SPRAY SYSTEM");
            newList.Add("AIRFOAM CHAMBER CONN.");

            newList.Add("AIRFOAM CHAMBER PLATFORM");
            newList.Add("STAIRWAY");
            newList.Add("LADDER");
            newList.Add("ROOF HANDRAIL");
            newList.Add("INSULATION SUPPORT");

            newList.Add("EXE. PIPE SUPPORT CLIP");
            newList.Add("NAME PLATE");

            return newList;
        }
    }
}
