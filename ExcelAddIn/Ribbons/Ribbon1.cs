using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ExcelAddIn
{
    public partial class Ribbon1
    {
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void button2_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.ShowProcessPane();
        }

        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("Start");
        }

        private void button9_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.ShowInputPane();
            
        }

        private void button13_Click(object sender, RibbonControlEventArgs e)
        {
            Service.PaneWindowService paneService = new Service.PaneWindowService();
            paneService.VisiblePane(Commons.CUSTOMPANE_LIST.NotSet, false, true);
            ExcelServices.ExcelService.ChangeSheet(Commons.EXCELSHEET_LIST.SHEET_MAIN);

        }
    }
}
