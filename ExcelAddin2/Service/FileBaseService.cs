using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelAddIn.Service
{
    public class FileBaseService
    {

        #region File Select

        private string[] SelectFileBase(string selTitle, string selFilter, bool multiValue = false)
        {
            string[] newFile = null;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = selTitle;
            ofd.Multiselect = multiValue;
            ofd.Filter = selFilter;
            //ofd.FilterIndex = 1;

            if (ofd.ShowDialog() == true)
                if (ofd.FileNames.Length > 0)
                    newFile = ofd.FileNames;

            return newFile;
        }
        public string[] SelectFileOfCal()
        {
            return SelectFileBase("계산서 INFORM", "WORD File|*.DOC;*.DOCX|All Files|*.*;");
        }
        public string[] SelectFileOfOther()
        {
            return SelectFileBase("타공정 INFORM", "CSV File|*.CSV|All Files|*.*;");
        }
        #endregion

    }
}
