using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using devDept.Eyeshot;
using devDept.Graphics;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Serialization;
using eyeEnvironment = devDept.Eyeshot.Environment;
using MColor = System.Windows.Media.Color;
using Color = System.Drawing.Color;
using DrawWork.AssemblyModels;

namespace DrawWork.DrawBuilders
{
    public class LogicBuilder : WorkUnit
    {
        private List<Entity> _EntityList;
        private Block _logicBlock;
        

        #region CONSTRUCTOR
        public LogicBuilder()
        {
            _EntityList = null;
        }
        public LogicBuilder(List<Entity> selEntityList)
        {
            _EntityList = selEntityList;
        }
        #endregion


        protected override void DoWork(BackgroundWorker worker, DoWorkEventArgs doWorkEventArgs)
        {

            //initialize progress
            UpdateProgress(0, 100, "Rebuilding Drawing Logic", worker);

            Color structureColor = Color.Red;

            _logicBlock = new Block("LogicBlock", linearUnitsType.Millimeters);

            

            UpdateProgressTo100("Creating Drawing Logic", worker);
        }

        protected override void WorkCompleted(eyeEnvironment environment)
        {
            // 블럭 추가
            // 엔트리 추가
            //Entity[] cc = new Entity[2];
            //cc[0] = new Line(1, 1, 40, 40);
            //cc[1] = new Line(40, 1, 40, 40);
            //environment.Entities.AddRange(cc);

            environment.Entities.AddRange(_EntityList);

            // update top data display
            environment.Entities.Regen();

            // sets trimetric view
            environment.SetView(viewType.Top);

            // fits the model in the viewport
            environment.ZoomFit();
        }
    }
}
