using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DrawWork.Utils;

namespace DrawWork.AssemblyModels
{
    public class AssemblyModel : Notifier
    {
        public AssemblyModel()
        {
            ShellInput = new ObservableCollection<ShellInputModel>();
            ShellOutput = new ObservableCollection<ShellOutputModel>();
        }

        private ObservableCollection<ShellInputModel> _ShellInput;
        public ObservableCollection<ShellInputModel> ShellInput
        {
            get { return _ShellInput; }
            set
            {
                _ShellInput = value;
                OnPropertyChanged(nameof(ShellInput));
            }
        }

        private ObservableCollection<ShellOutputModel> _ShellOutput;
        public ObservableCollection<ShellOutputModel> ShellOutput
        {
            get { return _ShellOutput; }
            set
            {
                _ShellOutput = value;
                OnPropertyChanged(nameof(ShellOutput));
            }
        }
    }
}
