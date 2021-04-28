using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using WpfCadCon.Model;
using WpfCadCon.Utils;

namespace WpfCadCon.ViewModels
{
	public class TestTableViewModel : Notifier
	{
		public ObservableCollection<string> TableHeader
		{
			get { return _TableHeader; }
			set
			{
				_TableHeader = value;
				OnPropertyChanged(nameof(TableHeader));
			}
		}
		private ObservableCollection<string> _TableHeader;


		public ObservableCollection<TableModel> TableData
		{
			get { return _TableData; }
			set
			{
				_TableData = value;
				OnPropertyChanged(nameof(TableData));
			}
		}
		private ObservableCollection<TableModel> _TableData;
		public TestTableViewModel()
        {
			TableHeader = new ObservableCollection<string>();
			TableHeader.Add("열1");
			TableHeader.Add("열2");
			TableHeader.Add("열3");
			TableData = new ObservableCollection<TableModel>();
			CreateSampleData();
		}
		private void CreateSampleData()
        {
			for(int i = 0; i < 10; i++)
            {
				TableModel newTable = new TableModel();
				newTable.COL01 = i.ToString("00") + "COL01";
				newTable.COL02 = i.ToString("00") + "COL02";
				newTable.COL03 = i.ToString("00") + "COL03";
				TableData.Add(newTable);
			}
        }
    }
}
