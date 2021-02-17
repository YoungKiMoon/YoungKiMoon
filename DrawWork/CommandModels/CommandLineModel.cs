using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.CommandModels
{
    public class CommandLineModel
    {
        public CommandLineModel()
        {
            CommandText = "";
        }

        private string _CommandText;
        public string CommandText
        {
            get { return _CommandText; }
            set {_CommandText = value;}
        }
    }
}
