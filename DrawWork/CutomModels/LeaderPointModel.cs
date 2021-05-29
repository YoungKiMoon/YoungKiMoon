using DrawWork.DrawModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.CutomModels
{
    public class LeaderPointModel
    {
        public LeaderPointModel()
        {
            leaderPoint = new CDPoint();
            lineTextList = new List<string>();
            emptyTextList = new List<string>();
            lineLength = 0;
            Position = "";
        }


        public CDPoint leaderPoint
        {
            get { return _leaderPoint; }
            set { _leaderPoint = value; }
        }
        private CDPoint _leaderPoint;


        public List<string> lineTextList
        {
            get { return _lineTextList; }
            set { _lineTextList = value; }
        }
        private List<string> _lineTextList;

        public List<string> emptyTextList
        {
            get { return _emptyTextList; }
            set { _emptyTextList = value; }
        }
        private List<string> _emptyTextList;

        public double lineLength
        {
            get { return _lineLength; }
            set { _lineLength = value; }
        }
        private double _lineLength;

        public string Position
        {
            get { return _Position; }
            set { _Position = value; }
        }
        private string _Position;

    }
}
