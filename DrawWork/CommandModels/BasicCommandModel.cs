﻿using DrawWork.DrawModels;
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
            currentPoint = new CDPoint();
            referencePoint = new CDPoint();
        }

        public object Clone()
        {
            BasicCommandModel newModel = new BasicCommandModel();
            newModel.commandList = commandList;
            newModel.commandListTrans = commandListTrans;
            newModel.currentPoint = currentPoint;
            newModel.referencePoint = referencePoint;
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

        private CDPoint _currentPoint;
        public CDPoint currentPoint
        {
            get { return _currentPoint; }
            set { _currentPoint = value; }
        }

        private CDPoint _referencePoint;
        public CDPoint referencePoint
        {
            get { return _referencePoint; }
            set { _referencePoint = value; }
        }

    }
}
