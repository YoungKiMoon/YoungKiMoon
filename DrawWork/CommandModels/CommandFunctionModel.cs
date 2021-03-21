using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DrawWork.Utils;

namespace DrawWork.CommandModels
{
    public class CommandFunctionModel : Notifier
    {
        public CommandFunctionModel()
        {
            Name = "";
            Properties = new List<CommandPropertiyModel>();
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

        private List<CommandPropertiyModel> _Properties;
        public List<CommandPropertiyModel> Properties
        {
            get { return _Properties; }
            set
            {
                _Properties = value;
                OnPropertyChanged(nameof(Properties));
            }
        }

    }
}
