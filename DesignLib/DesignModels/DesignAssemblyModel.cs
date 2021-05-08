using DesignLib.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DesignLib.DesignModels
{
    public class DesignAssemblyModel : Notifier
    {
		public DesignAssemblyModel()
		{
			DisplayName = "";
			AssemblyName = "";

			Data = new ObservableCollection<object>();

			AssemblyList = new ObservableCollection<object>();
			DimensionLineList = new ObservableCollection<object>();
			LeaderLineList = new ObservableCollection<object>();
			EtcLineList = new ObservableCollection<object>();
		}

		public string DisplayName
		{
			get { return _DisplayName; }
			set
			{
				_DisplayName = value;
				OnPropertyChanged(nameof(DisplayName));
			}
		}
		private string _DisplayName;

		public string AssemblyName
		{
			get { return _AssemblyName; }
			set
			{
				_AssemblyName = value;
				OnPropertyChanged(nameof(AssemblyName));
			}
		}
		private string _AssemblyName;

		public ObservableCollection<object> Data
		{
			get { return _Data; }
			set
			{
				_Data = value;
				OnPropertyChanged(nameof(Data));
			}
		}
		private ObservableCollection<object> _Data;

		public void SetDataList<T>(ObservableCollection<T> selObj)
        {
			Data.Clear();
			foreach (T eachItem in selObj)
				Data.Add(eachItem);
        }

		public ObservableCollection<object> AssemblyList
		{
			get { return _AssemblyList; }
			set
			{
				_AssemblyList = value;
				OnPropertyChanged(nameof(AssemblyList));
			}
		}
		private ObservableCollection<object> _AssemblyList;

		public ObservableCollection<object> DimensionLineList
		{
			get { return _DimensionLineList; }
			set
			{
				_DimensionLineList = value;
				OnPropertyChanged(nameof(DimensionLineList));
			}
		}
		private ObservableCollection<object> _DimensionLineList;

		public ObservableCollection<object> LeaderLineList
		{
			get { return _LeaderLineList; }
			set
			{
				_LeaderLineList = value;
				OnPropertyChanged(nameof(LeaderLineList));
			}
		}
		private ObservableCollection<object> _LeaderLineList;

		public ObservableCollection<object> EtcLineList
		{
			get { return _EtcLineList; }
			set
			{
				_EtcLineList = value;
				OnPropertyChanged(nameof(EtcLineList));
			}
		}
		private ObservableCollection<object> _EtcLineList;

		public void addDimensionLine<T>(ObservableCollection<T> selObj)
		{
			foreach (T eachItem in selObj)
				DimensionLineList.Add(eachItem);
		}
		public void addLeaderLine<T>(ObservableCollection<T> selObj)
		{
			foreach (T eachItem in selObj)
				LeaderLineList.Add(eachItem);
		}
		public void addEtcLine<T>(ObservableCollection<T> selObj)
		{
			foreach (T eachItem in selObj)
				EtcLineList.Add(eachItem);
		}
	}
}
