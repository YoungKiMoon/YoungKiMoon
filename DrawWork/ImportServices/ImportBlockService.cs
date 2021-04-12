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

//using Color = System.Drawing.Color;

namespace DrawWork.ImportServices
{
    public class ImportBlockService
    {
        #region CONSTRUCTOR
        public ImportBlockService()
        {

        }
        #endregion

        public void CreateBlock(ReadFileAsyncWithBlocks selFileData,Model selModel)
        {

            Dictionary<string, List<Entity>> blockDic = new Dictionary<string, List<Entity>>();

            List<LinearPath> boxList = new List<LinearPath>();
            foreach(Entity eachEntity in selFileData.Entities)
            {
                if(eachEntity is LinearPath)
                {
                    boxList.Add((LinearPath)eachEntity);

                }
            }

            List<string> blockNameList = blockDic.Keys.ToList();
            foreach(string eachName in blockNameList)
            {
                Block eachBlock = GetBlock(eachName, blockDic[eachName]);
                selModel.Blocks.Add(eachBlock);
            }
        }

        private Block GetBlock(string selBlockName, List<Entity> selEntityList)
        {
            Block newBlock = new Block(selBlockName);
            // Layer 지정이 필요
            newBlock.Entities.AddRange(selEntityList);

            return newBlock;
        }

    }
}
