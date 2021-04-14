﻿using System;
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
using DrawWork.DrawModels;

namespace DrawWork.DrawServices
{
    public class DrawImportBlockService
    {
        public Model singleModel;
        public DrawImportBlockService(Model selModel)
        {
            singleModel = selModel;
        }

        public BlockReference Draw_ImportBlock(CDPoint selPoint1, string selBlockName,double scaleFactor=1)
        {
            if (singleModel.Blocks.Contains(selBlockName))
            {
                BlockReference newBlock = new BlockReference(selPoint1.X, selPoint1.Y, 0, selBlockName.ToUpper(), 0);
                newBlock.Scale(scaleFactor);
                return newBlock;
            }
            else
            {
                return null;
            }
        }
    }
}