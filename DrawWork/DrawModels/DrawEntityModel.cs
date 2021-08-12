using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using devDept.Eyeshot.Entities;

namespace DrawWork.DrawModels
{
    public class DrawEntityModel
    {
        public DrawEntityModel()
        {
            // Assembly
            outlineList = new List<Entity>();
            centerlineList = new List<Entity>();

            // Dimension
            dimlineList = new List<Entity>();
            dimTextList = new List<Entity>();
            dimlineExtList = new List<Entity>();
            dimArrowList = new List<Entity>();

            // Leader
            leaderlineList = new List<Entity>();
            leaderTextList = new List<Entity>();
            leaderArrowList = new List<Entity>();

            // Nozzle
            nozzlelineList = new List<Entity>();
            nozzleMarkList = new List<Entity>();
            nozzleTextList = new List<Entity>();

            // Block
            blockList = new List<Entity>();
        }

        public void AddDrawEntity(DrawEntityModel selEntity)
        {
            if (selEntity.outlineList.Count > 0)
                outlineList.AddRange(selEntity.outlineList);
            if (selEntity.centerlineList.Count > 0)
                centerlineList.AddRange(selEntity.centerlineList);

            if (selEntity.dimlineList.Count > 0)
                dimlineList.AddRange(selEntity.dimlineList);
            if (selEntity.dimTextList.Count > 0)
                dimTextList.AddRange(selEntity.dimTextList);
            if (selEntity.dimlineExtList.Count > 0)
                dimlineExtList.AddRange(selEntity.dimlineExtList);
            if (selEntity.dimArrowList.Count > 0)
                dimArrowList.AddRange(selEntity.dimArrowList);

            if (selEntity.leaderlineList.Count > 0)
                leaderlineList.AddRange(selEntity.leaderlineList);
            if (selEntity.leaderTextList.Count > 0)
                leaderTextList.AddRange(selEntity.leaderTextList);
            if (selEntity.leaderArrowList.Count > 0)
                leaderArrowList.AddRange(selEntity.leaderArrowList);

            if (selEntity.nozzlelineList.Count > 0)
                nozzlelineList.AddRange(selEntity.nozzlelineList);
            if (selEntity.nozzleMarkList.Count > 0)
                nozzleMarkList.AddRange(selEntity.nozzleMarkList);
            if (selEntity.nozzleTextList.Count > 0)
                nozzleTextList.AddRange(selEntity.nozzleTextList);

            if (selEntity.blockList.Count > 0)
                blockList.AddRange(selEntity.blockList);

        }

        public List<Entity> GetDrawEntity()
        {
            List<Entity> newList = new List<Entity>();
            newList.AddRange(outlineList);
            newList.AddRange(centerlineList);

            newList.AddRange(dimlineList);
            newList.AddRange(dimTextList);
            newList.AddRange(dimlineExtList);
            newList.AddRange(dimArrowList);

            newList.AddRange(leaderlineList);
            newList.AddRange(leaderTextList);
            newList.AddRange(leaderArrowList);

            newList.AddRange(nozzlelineList);
            newList.AddRange(nozzleMarkList);
            newList.AddRange(nozzleTextList);

            newList.AddRange(blockList);

            return newList;
        }


        private List<Entity> _outlineList;
        public List<Entity> outlineList
        {
            get { return _outlineList; }
            set { _outlineList = value; }
        }
        private List<Entity> _centerlineList;
        public List<Entity> centerlineList
        {
            get { return _centerlineList; }
            set { _centerlineList = value; }
        }
        private List<Entity> _dimlineList;
        public List<Entity> dimlineList
        {
            get { return _dimlineList; }
            set { _dimlineList = value; }
        }
        private List<Entity> _dimTextList;
        public List<Entity> dimTextList
        {
            get { return _dimTextList; }
            set { _dimTextList = value; }
        }
        private List<Entity> _dimlineExtList;
        public List<Entity> dimlineExtList
        {
            get { return _dimlineExtList; }
            set { _dimlineExtList = value; }
        }
        private List<Entity> _dimArrowList;
        public List<Entity> dimArrowList
        {
            get { return _dimArrowList; }
            set { _dimArrowList = value; }
        }
        private List<Entity> _leaderlineList;
        public List<Entity> leaderlineList
        {
            get { return _leaderlineList; }
            set { _leaderlineList = value; }
        }
        private List<Entity> _leaderTextList;
        public List<Entity> leaderTextList
        {
            get { return _leaderTextList; }
            set { _leaderTextList = value; }
        }
        private List<Entity> _leaderArrowList;
        public List<Entity> leaderArrowList
        {
            get { return _leaderArrowList; }
            set { _leaderArrowList = value; }
        }
        private List<Entity> _nozzlelineList;
        public List<Entity> nozzlelineList
        {
            get { return _nozzlelineList; }
            set { _nozzlelineList = value; }
        }
        private List<Entity> _nozzleMarkList;
        public List<Entity> nozzleMarkList
        {
            get { return _nozzleMarkList; }
            set { _nozzleMarkList = value; }
        }
        private List<Entity> _nozzleTextList;
        public List<Entity> nozzleTextList
        {
            get { return _nozzleTextList; }
            set { _nozzleTextList = value; }
        }

        public List<Entity> blockList
        {
            get { return _blockList; }
            set { _blockList = value; }
        }
        private List<Entity> _blockList;

    }
}
