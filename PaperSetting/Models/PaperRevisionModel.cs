using PaperSetting.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaperSetting.Models
{
    public class PaperRevisionModel : Notifier, IBaseModel, ICloneable
    {
        public PaperRevisionModel()
        {
            No = "";
            RevString = "";
            DateString = "";
            Description = "";
            PreparedName = "";
            CheckedName = "";
            ApprovedName = "";
        }

        public object Clone()
        {
            PaperRevisionModel newModel = new PaperRevisionModel();
            newModel.No = No;
            newModel.RevString = RevString;
            newModel.DateString = DateString;
            newModel.Description = Description;
            newModel.PreparedName = PreparedName;
            newModel.CheckedName = CheckedName;
            newModel.ApprovedName = ApprovedName;
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

        private string _RevString;
        public string RevString
        {
            get { return _RevString; }
            set
            {
                _RevString = value;
                OnPropertyChanged(nameof(RevString));
            }
        }

        private string _DateString;
        public string DateString
        {
            get { return _DateString; }
            set
            {
                _DateString = value;
                OnPropertyChanged(nameof(DateString));
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private string _PreparedName;
        public string PreparedName
        {
            get { return _PreparedName; }
            set
            {
                _PreparedName = value;
                OnPropertyChanged(nameof(PreparedName));
            }
        }

        private string _CheckedName;
        public string CheckedName
        {
            get { return _CheckedName; }
            set
            {
                _CheckedName = value;
                OnPropertyChanged(nameof(CheckedName));
            }
        }

        private string _ApprovedName;
        public string ApprovedName
        {
            get { return _ApprovedName; }
            set
            {
                _ApprovedName = value;
                OnPropertyChanged(nameof(ApprovedName));
            }
        }

    }
}
