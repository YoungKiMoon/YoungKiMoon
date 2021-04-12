using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using devDept.Eyeshot;
using devDept.Graphics;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Serialization;
using MColor = System.Windows.Media.Color;

namespace DrawWork.ImportServices
{
    public class ImportBlockModel
    {
        public ImportBlockModel()
        {
            BlockName = "";
            BlockArea = null;
            BlockNameList = new List<LinearPath>();
            BlockEntities = new List<Entity>();
        }

        public LinearPath BlockArea
        {
            get { return _BlockArea; }
            set { _BlockArea = value;}
        }
        private LinearPath _BlockArea;
        public List<LinearPath> BlockNameList
        {
            get { return _BlockNameList; }
            set { _BlockNameList = value; }
        }
        private List<LinearPath> _BlockNameList;

        public List<Entity> BlockEntities
        {
            get { return _BlockEntities; }
            set { _BlockEntities = value; }
        }
        private List<Entity> _BlockEntities;

        public string BlockName
        {
            get { return _BlockName; }
            set { _BlockName = value; }
        }
        private string _BlockName;
    }
}
