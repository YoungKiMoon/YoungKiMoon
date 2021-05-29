using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using ExcelAddIn.Panes;
using ExcelAddIn.Service;
using ExcelAddIn.Commons;
using ExcelAddIn.ExcelModels;

namespace ExcelAddIn
{
    public partial class ThisAddIn
    {

        private PaneWindowService paneService = new PaneWindowService();

        // TaskPane
        private Microsoft.Office.Tools.CustomTaskPane customProcessTaskPane;
        public ProcessPane customProcessPane;
        private Microsoft.Office.Tools.CustomTaskPane customInputTaskPane;
        public InputPane customInputPane;

        // Excel : WorkSheet
        public ExcelWorkSheetModel ActionActiveSheet;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            string processName = paneService.GetPaneTitleName(CUSTOMPANE_LIST.PROCESS);

            customProcessPane = new ProcessPane();
            customProcessTaskPane = this.CustomTaskPanes.Add(customProcessPane, processName);

            customProcessTaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionTop;
            customProcessTaskPane.Height = 103;

            customProcessTaskPane.Visible = true;

            ActionActiveSheet = null;
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }


        #region VSTO에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion

        #region Ribbon
        protected override Office.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return new Ribbon1();
        }
        #endregion


        public void ShowProcessPane()
        {

            if (paneService.CheckCreatePane(CUSTOMPANE_LIST.PROCESS))
            {
                string processName = paneService.GetPaneTitleName(CUSTOMPANE_LIST.PROCESS);

                customProcessPane = new ProcessPane();
                customProcessTaskPane = this.CustomTaskPanes.Add(customProcessPane, processName);

                customProcessTaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionTop;
                customProcessTaskPane.Height = 103;

                customProcessTaskPane.Visible = true;
            }



        }

        public void ShowInputPane()
        {
            if (paneService.CheckCreatePane(CUSTOMPANE_LIST.INPUT))
            {

                string processName = paneService.GetPaneTitleName(CUSTOMPANE_LIST.INPUT);
                customInputPane = new InputPane();
                customInputTaskPane = this.CustomTaskPanes.Add(customInputPane, processName);

                customInputTaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionRight;
                customInputTaskPane.Width = 320;

                customInputTaskPane.Visible = true;
            }

        }

    }
}
