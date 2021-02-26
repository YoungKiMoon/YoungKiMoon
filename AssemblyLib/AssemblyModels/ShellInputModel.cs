using AssemblyLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyLib.AssemblyModels
{
    public class ShellInputModel : Notifier
    {
        public ShellInputModel()
        {
            No = "";
            ID = "";
            Height = "";
            PLWidth = "";
            PLHeight = "";
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

        private string _ID;
        public string ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        private string _Height;
        public string Height
        {
            get { return _Height; }
            set
            {
                _Height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        private string _PLWidth;
        public string PLWidth
        {
            get { return _PLWidth; }
            set
            {
                _PLWidth = value;
                OnPropertyChanged(nameof(PLWidth));
            }
        }

        private string _PLHeight;
        public string PLHeight
        {
            get { return _PLHeight; }
            set
            {
                _PLHeight = value;
                OnPropertyChanged(nameof(PLHeight));
            }
        }

    }
}
