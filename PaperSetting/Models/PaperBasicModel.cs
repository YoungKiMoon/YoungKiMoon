using PaperSetting.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaperSetting.Models
{
    public class PaperBasicModel : Notifier, ICloneable
    {
        public PaperBasicModel()
        {
            No = "";
            Title = "";
            DwgNo = "";
            StampName = "";
        }

        public object Clone()
        {
            PaperBasicModel newModel = new PaperBasicModel();
            newModel.No = No;
            newModel.Title = Title;
            newModel.DwgNo = DwgNo;
            newModel.StampName = StampName;
            return newModel;
        }

        private string _No;
        public string No
        {
            get { return _No; }
            set
            {
                _No = value;
                OnPropertyChanged(nameof(No));
            }
        }

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string _DwgNo;
        public string DwgNo
        {
            get { return _DwgNo; }
            set
            {
                _DwgNo = value;
                OnPropertyChanged(nameof(DwgNo));
            }
        }

        private string _StampName;
        public string StampName
        {
            get { return _StampName; }
            set
            {
                _StampName = value;
                OnPropertyChanged(nameof(StampName));
            }
        }

    }
}
