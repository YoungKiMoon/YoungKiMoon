using AssemblyLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyLib.AssemblyModels
{
    public class GeneralModel : Notifier
    {
        public GeneralModel()
        {

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
    }
}
