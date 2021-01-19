using ExcelAddIn.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelAddIn.Service
{
    public class PaneWindowService
    {

        private string GetActiveWindow()
        {
            return Globals.ThisAddIn.Application.ActiveWindow.Caption as string;
        }

        public string GetPaneTitleName(CUSTOMPANE_LIST selPane)
        {
            string titleName = "";

            switch (selPane)
            {
                case CUSTOMPANE_LIST.PROCESS:
                    titleName = "Design Process";
                    break;
                case CUSTOMPANE_LIST.INPUT:
                    titleName = "Input Window";
                    break;

                default:
                    break;
            }

            return titleName;
        }

        public object GetPaneObject(CUSTOMPANE_LIST selPane)
        {
            /*
            string activeWinCaption = GetActiveWindow();
            string selPaneType = GetPaneType(selPane);

            if (Globals.ThisAddIn.customConRomData != null)
            {
                foreach (Microsoft.Office.Tools.CustomTaskPane eachPane in Globals.ThisAddIn.CustomTaskPanes)
                {
                    if (eachPane.Control.Name == selPaneType)
                    {
                        Excel.Window eachWin = eachPane.Window as Excel.Window;
                        if (eachWin.Caption == activeWinCaption)
                        {
                            return eachPane.Control;
                        }
                    }
                }
            }

            return null;
            */
            return null;
        }

        public object GetPaneWPF(CUSTOMPANE_LIST selPane)
        {
            /*
            if (selPane == CUSTOMPANE_LIST.ROMDATA)
            {
                ROMDataPane newPane = GetPane(selPane) as ROMDataPane;
                if (newPane != null)
                    return newPane.WPFControlHost;
            }
            else if (selPane == CUSTOMPANE_LIST.ROMDATALIST)
            {
                ROMDataListPane newPane = GetPane(selPane) as ROMDataListPane;
                if (newPane != null)
                    return newPane.WPFControlHost;
            }
            else if (selPane == CUSTOMPANE_LIST.ROMDATAAFILL)
            {
                ROMDataAFillPane newPane = GetPane(selPane) as ROMDataAFillPane;
                if (newPane != null)
                    return newPane.WPFControlHost;
            }
            return null;
            */
            return null;
        }

        public bool CheckCreatePane(CUSTOMPANE_LIST selPane)
        {
            string titleName = GetPaneTitleName(selPane);
            string activeWinCaption = GetActiveWindow();
            bool isCreate = true;

            foreach (Microsoft.Office.Tools.CustomTaskPane eachPane in Globals.ThisAddIn.CustomTaskPanes)
            {
                Excel.Window eachWin = eachPane.Window as Excel.Window;
                if ((string)eachWin.Caption == activeWinCaption)
                {
                    if (eachPane.Title == titleName)
                    {
                        isCreate = false;
                        
                        eachPane.Visible = eachPane.Visible? false:true;

                        break;

                    }
                }


            }

            return isCreate;

        }

        public void VisiblePane(CUSTOMPANE_LIST selPane=CUSTOMPANE_LIST.NotSet, bool visibleValue = true, bool all = false)
        {
            string titleName = GetPaneTitleName(selPane);
            string activeWinCaption = GetActiveWindow();

            foreach (Microsoft.Office.Tools.CustomTaskPane eachPane in Globals.ThisAddIn.CustomTaskPanes)
            {
                Excel.Window eachWin = eachPane.Window as Excel.Window;
                if ((string)eachWin.Caption == activeWinCaption)
                {
                    if (all)
                    {
                        eachPane.Visible = visibleValue;
                    }
                    else
                    {
                        if (eachPane.Title == titleName)
                        {
                            eachPane.Visible = visibleValue;
                            break;

                        }
                    }
                }


            }

        }

    }
}
