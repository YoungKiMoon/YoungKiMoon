﻿using AssemblyLib.AssemblyModels;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using DrawWork.DrawModels;
using DrawWork.DrawStyleServices;
using DrawWork.ValueServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawDetailServices
{
    public class DrawDetailService
    {
        private Model singleModel;
        private AssemblyModel assemblyData;




        public  DrawDetailShellService detailShellService;

        public DrawDetailTempService detailTempService;
        public DrawDetailService(AssemblyModel selAssembly,object selModel)
        {
            singleModel = selModel as Model;
            assemblyData = selAssembly;




            detailShellService = new DrawDetailShellService(selAssembly,selModel);
            detailTempService = new DrawDetailTempService(selAssembly, selModel);
        }


    }
}
