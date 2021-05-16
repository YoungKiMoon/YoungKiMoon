using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawLogicLib.Models
{
    public class DrawLogicModel
    {
        public DrawLogicModel()
        {
            UsingList = new List<string>();
            ReferencePointList = new List<string>();
            CommandList = new List<DrawCommandModel>();
        }

        public List<string> UsingList
        {
            get { return _UsingList; }
            set { _UsingList = value; }
        }
        private List<string> _UsingList;

        public List<string> ReferencePointList
        {
            get { return _ReferencePointList; }
            set { _ReferencePointList = value; }
        }
        private List<string> _ReferencePointList;

        public List<DrawCommandModel> CommandList
        {
            get { return _CommandList; }
            set { _CommandList = value; }
        }
        private List<DrawCommandModel> _CommandList;
    }
}
