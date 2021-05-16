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

            Description = "";
            DateString = "";
            DRWN = "";
            CHKD = "";
            REVD = "";
            APVD = "";
        }

        public object Clone()
        {
            PaperRevisionModel newModel = new PaperRevisionModel();
            newModel.No = No;
            newModel.RevString = RevString;

            newModel.Description = Description;
            newModel.DateString = DateString;
            newModel.DRWN = DRWN;
            newModel.CHKD = CHKD;
            newModel.REVD = REVD;
            newModel.APVD = APVD;
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

        private string _DRWN;
        public string DRWN
        {
            get { return _DRWN; }
            set
            {
                _DRWN = value;
                OnPropertyChanged(nameof(DRWN));
            }
        }



        private string _CHKD;
        public string CHKD
        {
            get { return _CHKD; }
            set
            {
                _CHKD = value;
                OnPropertyChanged(nameof(CHKD));
            }
        }

        private string _REVD;
        public string REVD
        {
            get { return _REVD; }
            set
            {
                _REVD = value;
                OnPropertyChanged(nameof(REVD));
            }
        }

        private string _APVD;
        public string APVD
        {
            get { return _APVD; }
            set
            {
                _APVD = value;
                OnPropertyChanged(nameof(APVD));
            }
        }

    }
}
