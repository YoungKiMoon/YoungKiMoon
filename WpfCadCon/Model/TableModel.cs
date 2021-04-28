using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfCadCon.Utils;

namespace WpfCadCon.Model
{
    public class TableModel : Notifier
    {
        public string COL01
        {
            get { return _COL01; }
            set { 
                _COL01 = value;
                OnPropertyChanged(nameof(COL01));
                 }
        }
        private string _COL01;

        public string COL02
        {
            get { return _COL02; }
            set
            {
                _COL02 = value;
                OnPropertyChanged(nameof(COL02));
            }
        }
        private string _COL02;
        public string COL03
        {
            get { return _COL03; }
            set
            {
                _COL03 = value;
                OnPropertyChanged(nameof(COL03));
            }
        }
        private string _COL03;
    }
}
