using DrawWork.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.CommandModels
{
    public class CommandPropertiyModel : Notifier
    {
        public CommandPropertiyModel()
        {
            Name = "";
            Value = "";
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _Value;
        public string Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

    }
}
