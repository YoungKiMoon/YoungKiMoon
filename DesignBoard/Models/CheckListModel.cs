using DesignBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignBoard.Models
{
    public class CheckListModel : Notifier
    {
        public CheckListModel()
        {
            AMETankCheck = new CheckModel();
            DataInput = new CheckModel();
            NozzleInput = new CheckModel();
            DataCheck = new CheckModel();
        }

        public CheckModel AMETankCheck
        {
            get { return _AMETankCheck; }
            set
            {
                _AMETankCheck = value;
                OnPropertyChanged(nameof(AMETankCheck));
            }
        }
        private CheckModel _AMETankCheck;

        public CheckModel DataInput
        {
            get { return _DataInput; }
            set
            {
                _DataInput = value;
                OnPropertyChanged(nameof(DataInput));
            }
        }
        private CheckModel _DataInput;

        public CheckModel NozzleInput
        {
            get { return _NozzleInput; }
            set
            {
                _NozzleInput = value;
                OnPropertyChanged(nameof(NozzleInput));
            }
        }
        private CheckModel _NozzleInput;

        public CheckModel DataCheck
        {
            get { return _DataCheck; }
            set
            {
                _DataCheck = value;
                OnPropertyChanged(nameof(DataCheck));
            }
        }
        private CheckModel _DataCheck;
    }
}
