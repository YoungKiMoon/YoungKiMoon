using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExcelDataLib.Utils;

using Excel = Microsoft.Office.Interop.Excel;



namespace ExcelDataLib.ExcelModels
{
    public class ExcelWorkSheetModel : Notifier
	{
        public ExcelWorkSheetModel()
        {
            ExcelApp = null;
            ExcelWork = null;
            ExcelSheet = null;
            ExcelWorkName = "";
            ExcelSheetName = "";
        }

		private Excel.Application _ExcelApp;
		public Excel.Application ExcelApp
		{
			get { return _ExcelApp; }
			set
			{
				_ExcelApp = value;
				OnPropertyChanged(nameof(ExcelApp));
			}
		}

		private Excel.Workbook _ExcelWork;
		public Excel.Workbook ExcelWork
		{
			get { return _ExcelWork; }
			set
			{
				_ExcelWork = value;
				OnPropertyChanged(nameof(ExcelWork));
			}
		}

		private Excel.Worksheet _ExcelSheet;
		public Excel.Worksheet ExcelSheet
		{
			get { return _ExcelSheet; }
			set
			{
				_ExcelSheet = value;
				OnPropertyChanged(nameof(ExcelSheet));
			}
		}


		private string _ExcelWorkName;
		public string ExcelWorkName
		{
			get { return _ExcelWorkName; }
			set
			{
				_ExcelWorkName = value;
				OnPropertyChanged(nameof(ExcelWorkName));
			}
		}

		private string _ExcelSheetName;
		public string ExcelSheetName
		{
			get { return _ExcelSheetName; }
			set
			{
				_ExcelSheetName = value;
				OnPropertyChanged(nameof(ExcelSheetName));
			}
		}
	}
}
