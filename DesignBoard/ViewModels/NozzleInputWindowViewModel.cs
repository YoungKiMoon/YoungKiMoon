using DesignBoard.Models;
using DesignBoard.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DesignBoard.ViewModels
{
    public class NozzleInputWindowViewModel : Notifier
    {
        public ObservableCollection<NozzleModel> NozzleList
        {
            get { return _NozzleList; }
            set
            {
                _NozzleList = value;
                OnPropertyChanged(nameof(NozzleList));
            }
        }
        private ObservableCollection<NozzleModel> _NozzleList;

        public NozzleInputWindowViewModel()
        {
            NozzleList = new ObservableCollection<NozzleModel>();
        }


        public void ReCheck()
        {
            foreach(NozzleModel eachModel in NozzleList)
            {
                eachModel.Error = false;
            }
        }

        public void CreateData()
        {
            NozzleList.Clear();
            NozzleList.Add(new NozzleModel() { Error = false, Position = "SHELL", Mark = "M1", Size = "24", Rating = "", Type = "", Facing = "", R = "16380", H = "600", Service = "MANHOLE", Remarks = "W/COVER & DAVIT" });
            NozzleList.Add(new NozzleModel() { Error = false, Position = "SHELL", Mark = "T", Size = "2", Rating = "#150", Type = "WN", Facing = "RF", R = "16380", H = "1400", Service = "THERMOWELL0", Remarks = "W/COVER" });
            NozzleList.Add(new NozzleModel() { Error = false, Position = "SHELL", Mark = "L2A", Size = "3", Rating = "#150", Type = "WN", Facing = "RF", R = "16430", H = "14500", Service = "LEVEL", Remarks = "W/COVER" });
            NozzleList.Add(new NozzleModel() { Error = false, Position = "SHELL", Mark = "L2B", Size = "3", Rating = "#150", Type = "WN", Facing = "RF", R = "16380", H = "800", Service = "LEVEL", Remarks = "W/COVER" });
            NozzleList.Add(new NozzleModel() { Error = false, Position = "SHELL", Mark = "N11A", Size = "2", Rating = "#150", Type = "WN", Facing = "RF", R = "16380", H = "1800", Service = "RECYCLE", Remarks = "" });
            NozzleList.Add(new NozzleModel() { Error = true, Position = "SHELL", Mark = "N11B", Size = "2", Rating = "#150", Type = "WN", Facing = "RF", R = "6380", H = "6750", Service = "RETURN", Remarks = "" });
            NozzleList.Add(new NozzleModel() { Error = false, Position = "SHELL", Mark = "N11C", Size = "2", Rating = "#150", Type = "WN", Facing = "RF", R = "16380", H = "12200", Service = "RETURN", Remarks = "" });
            NozzleList.Add(new NozzleModel() { Error = false, Position = "SHELL", Mark = "N3A", Size = "6", Rating = "#150", Type = "WN", Facing = "RF", R = "16380", H = "235", Service = "DRAIN", Remarks = "W/ SUMP" });

            NozzleList.Add(new NozzleModel() { Error = false, Position = "ROOF", Mark = "P1", Size = "2", Rating = "#150", Type = "", Facing = "", R = "15200", H = "19520", Service = "PT CONN", Remarks = "" });
            NozzleList.Add(new NozzleModel() { Error = false, Position = "ROOF", Mark = "T1", Size = "3", Rating = "#150", Type = "WN", Facing = "RF", R = "13700", H = "19620", Service = "TEST SER", Remarks = "W/ INT.PIPE" });
            NozzleList.Add(new NozzleModel() { Error = false, Position = "ROOF", Mark = "L1", Size = "4", Rating = "#150", Type = "WN", Facing = "RF", R = "14700", H = "19550", Service = "LS CONN", Remarks = "" });
            NozzleList.Add(new NozzleModel() { Error = true, Position = "ROOF", Mark = "L2", Size = "8", Rating = "#150", Type = "WN", Facing = "RF", R = "14700", H = "9550", Service = "TEST SER", Remarks = "WN/ INTERNAL PIPE" });
            NozzleList.Add(new NozzleModel() { Error = false, Position = "ROOF", Mark = "GH1", Size = "8", Rating = "#150", Type = "WN", Facing = "FF", R = "14700", H = "19550", Service = "GAUGE HATCH", Remarks = "W/ GAUGE HATCH" });
            NozzleList.Add(new NozzleModel() { Error = false, Position = "ROOF", Mark = "v1", Size = "12", Rating = "#150", Type = "WN", Facing = "RF", R = "0", H = "20520", Service = "VENT", Remarks = "W/ GOOSE NECK & BIRD SCREEN" });

        }


    }
}
