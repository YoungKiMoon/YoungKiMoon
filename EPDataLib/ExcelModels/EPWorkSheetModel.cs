using EPDataLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPDataLib.ExcelModels
{
    public class EPWorkSheetModel : Notifier
    {

		public EPWorkSheetModel()
		{
			ExcelWorkName = "";
			ExcelSheetName = "";

			RowCount = 1;
			ColumnCount = 1;
			DataArray = null;

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



		public int ColumnCount
		{
			get { return _ColumnCount; }
			set
			{
				_ColumnCount = value;
				OnPropertyChanged(nameof(ColumnCount));
			}
		}
		private int _ColumnCount;
		public int RowCount
		{
			get { return _RowCount; }
			set
			{
				_RowCount = value;
				OnPropertyChanged(nameof(RowCount));
			}
		}
		private int _RowCount;

		public dynamic DataArray
		{
			get { return _DataArray; }
			set
			{
				_DataArray = value;
				OnPropertyChanged(nameof(DataArray));
			}
		}
		private dynamic _DataArray;

	}
}
