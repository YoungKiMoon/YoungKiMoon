using ExcelAddIn.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelAddIn.Models
{
    public class TableModel : Notifier
    {
        public string COL01
        {
            get { return _COL01; }
            set
            {
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

        public string COL04
        {
            get { return _COL04; }
            set
            {
                _COL04 = value;
                OnPropertyChanged(nameof(COL04));
            }
        }
        private string _COL04;

        public string COL05
        {
            get { return _COL05; }
            set
            {
                _COL05 = value;
                OnPropertyChanged(nameof(COL05));
            }
        }
        private string _COL05;

        public string COL06
        {
            get { return _COL06; }
            set
            {
                _COL06 = value;
                OnPropertyChanged(nameof(COL06));
            }
        }
        private string _COL06;

        public string COL07
        {
            get { return _COL07; }
            set
            {
                _COL07 = value;
                OnPropertyChanged(nameof(COL07));
            }
        }
        private string _COL07;
    }
}
