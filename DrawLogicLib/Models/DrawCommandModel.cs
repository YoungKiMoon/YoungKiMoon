using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawLogicLib.Models
{
    public class DrawCommandModel
    {
        public DrawCommandModel()
        {
            Name = "";
            Command = new List<string>();
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Name;

        public List<string> Command
        {
            get { return _Command; }
            set { _Command = value; }
        }
        private List<string> _Command;
    }
}
