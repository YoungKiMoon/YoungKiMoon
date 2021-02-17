using DrawWork.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.Models
{
    public class CommandModel : Notifier
    {
        public CommandModel()
        {
            CommandText = "";
        }

        private string _CommandText;
        public string CommandText
        {
            get { return _CommandText; }
            set
            {
                _CommandText = value;
                OnPropertyChanged(nameof(CommandText));
            }
        }
    }
}
