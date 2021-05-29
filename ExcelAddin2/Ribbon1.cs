
using ExcelAddIn.Commons;
using ExcelAddIn.ExcelServices;
using ExcelAddIn.Properties;
using ExcelAddIn.Service;
using ExcelAddIn.Utils;
using ExcelAddIn.Windows;
using Microsoft.Office.Core;
using PaperSetting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using Office = Microsoft.Office.Core;

// TODO:  리본(XML) 항목을 설정하려면 다음 단계를 수행하십시오.

// 1. 다음 코드 블록을 ThisAddin, ThisWorkbook 또는 ThisDocument 클래스에 복사합니다.

//  protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
//  {
//      return new Ribbon1();
//  }

// 2. 단추 클릭 등의 사용자 작업을 처리하려면 이 클래스의 "리본 콜백" 영역에서 콜백
//    메서드를 만듭니다. 참고: 리본 디자이너에서 이 리본을 내보낸 경우 이벤트 처리기의 코드를
//    콜백 메서드로 이동하고 리본 확장성(RibbonX) 프로그래밍 모델에서 사용할 수 있도록
//    코드를 수정해야 합니다.

// 3. 리본 XML 파일의 컨트롤 태그에 특성을 할당하여 사용자 코드의 적절한 콜백 메서드를 식별합니다.  

// 자세한 내용은 Visual Studio Tools for Office 도움말에서 리본 XML 설명서를 참조하십시오.


namespace ExcelAddIn
{
    [ComVisible(true)]
    public class Ribbon1 : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;

        public Ribbon1()
        {
        }

        #region IRibbonExtensibility 멤버

        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText("ExcelAddIn.Ribbon1.xml");
        }

        #endregion

        #region 리본 콜백

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;

            
        }

        #endregion



        #region Label
        public string GetCustomLabel(IRibbonControl control)
        {
            if (control.Id == "cuCAD01")
            {
                return "TABAS" +" " + VersionData.GetVersion();
            }
            else if (control.Id == "cuGroup01")
            {
                return "TAnk Basic Design Automation System";
            }
            else if (control.Id == "cuGroupButton00")
            {
                return "ROOF TYPE";
            }
            else if (control.Id == "cuGroupButton01")
            {
                return "계산서 INFORM";
            }
            else if (control.Id == "cuGroupButton02")
            {
                return "타공정 INFORM";
            }
            else if (control.Id == "cuGroupButton03")
            {
                return "DESIGN PROCESS";
            }
            else if (control.Id == "cuGroupButton04")
            {
                return "DRAWING LIST";
            }
            else if (control.Id == "cuGroupButton05")
            {
                return "TANK FILE OPEN";
            }
            else if (control.Id == "cuGroupButton06")
            {
                return "INFO";
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region Image
        public Bitmap GetCustomImage(IRibbonControl control)
        {
            if (control.Id == "cuGroupButton00")
            {
                return Resources.CreatePlan;
            }
            else if (control.Id == "cuGroupButton01")
            {
                return Resources.list;
            }
            else if (control.Id == "cuGroupButton02")
            {
                return Resources.list;
            }
            else if (control.Id == "cuGroupButton03")
            {
                return Resources.process;
            }
            else if (control.Id == "cuGroupButton04")
            {
                return Resources.play;
            }
            else if (control.Id == "cuGroupButton05")
            {
                return Resources.iTank2;
            }
            else if (control.Id == "cuGroupButton06")
            {
                return Resources.list;
            }
            else
            {
                return Resources.right_arrowSmall;
            }
        }
        #endregion

        #region Action
        public void CallbackButton01(Office.IRibbonControl control)
        {
            if (control.Id == "cuGroupButton00")
            {
                TypePlanWindow newType = new TypePlanWindow();
                var ss= new IntPtr(CommonAddin.GetAddinApplicationHWND());
               
                newType.ShowDialog();
                ROOF_TYPE selButton = newType.selectButton;
                if (selButton != ROOF_TYPE.NotSet)
                {
                    ExcelService.ChangeRoofType(selButton);

                    if (Globals.ThisAddIn.customProcessPane != null)
                    {
                        int selIndex = 0;
                        Globals.ThisAddIn.customProcessPane.elementHost1WPF.ChangeProcess(selIndex);
                        Globals.ThisAddIn.customProcessPane.elementHost1WPF.ChangeSheet(selIndex);
                    }
                    else
                    {
                        ExcelService.ChangeSheet(EXCELSHEET_LIST.SHEET_GENERAL);
                        ExcelService.SetInformationWindowArea(false, false);
                    }


                }

            }
            else if (control.Id == "cuGroupButton01")
            {
                FileBaseService newFile = new FileBaseService();
                newFile.SelectFileOfCal();
            }
            else if (control.Id == "cuGroupButton02")
            {
                FileBaseService newFile = new FileBaseService();
                newFile.SelectFileOfOther();
                //Globals.ThisAddIn.ShowPaneRomData();
            }
            else if (control.Id == "cuGroupButton03")
            {
                Globals.ThisAddIn.ShowProcessPane();
                //Globals.ThisAddIn.ShowPaneRomDataList();
            }
            else if (control.Id == "cuGroupButton04")
            {
                //Service.PaneWindowService paneService = new Service.PaneWindowService();
                //paneService.VisiblePane(Commons.CUSTOMPANE_LIST.NotSet, false, true);
                //ExcelServices.ExcelService.ChangeSheet(Commons.EXCELSHEET_LIST.SHEET_MAIN);
                string workbookName=Globals.ThisAddIn.Application.ActiveWorkbook.Name;

                PaperSettingWindow cc = new PaperSettingWindow();
                cc.workbookName = workbookName;
                cc.activeCustomWorkbook = Globals.ThisAddIn.Application.ActiveWorkbook;
                cc.ShowDialog();

            }
            else if (control.Id == "cuGroupButton05")
            {
                //Globals.ThisAddIn.ShowInputPane();
                //Globals.ThisAddIn.ShowPaneRomWWW();
                //PaneWindowsService newService = new PaneWindowsService();
                //UserControl1 selCon= newService.GetWWWPane();
                //UserControl2 selWpf = newService.GetWWWPaneWPF();
                FileBaseService newFile = new FileBaseService();
                string selFile=newFile.CreateFile();
                if(selFile!="")
                    Process.Start(selFile);
            }
            else if (control.Id == "cuGroupButton06")
            {
                Globals.ThisAddIn.ShowInputPane();
                //Service.PaneWindowService paneService = new Service.PaneWindowService();
                //paneService.VisiblePane(Commons.CUSTOMPANE_LIST.NotSet, false, true);
                //ExcelServices.ExcelService.ChangeSheet(Commons.EXCELSHEET_LIST.SHEET_MAIN);
            }
        }

        #endregion



        #region 도우미

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
