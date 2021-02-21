using DrawWork.DrawModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.CommandModels
{
    public class BasicCommandModel : ICloneable
    {
        public BasicCommandModel()
        {
            commandList = new List<CommandLineModel>();
            commandListTrans = new List<string[]>();
            drawPoint = new DrawPointModel();
        }

        public object Clone()
        {
            BasicCommandModel newModel = new BasicCommandModel();
            newModel.commandList = commandList;
            newModel.commandListTrans = commandListTrans;
            newModel.drawPoint = drawPoint.Clone() as DrawPointModel;
            return newModel;
        }

        private List<CommandLineModel> _commandList;
        public List<CommandLineModel> commandList
        {
            get { return _commandList; }
            set { _commandList = value; }
        }

        private List<string[]> _commandListTrans;
        public List<string[]> commandListTrans
        {
            get { return _commandListTrans; }
            set { _commandListTrans = value; }
        }

        private DrawPointModel _drawPoint;
        public DrawPointModel drawPoint
        {
            get { return _drawPoint; }
            set { _drawPoint = value; }
        }


    }
}
