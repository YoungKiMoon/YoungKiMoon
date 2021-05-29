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

using AssemblyLib.AssemblyModels;

using DrawWork.ValueServices;
using DrawWork.DrawModels;
using DrawWork.Commons;
using DrawWork.DrawCustomObjectModels;
using DrawWork.DrawStyleServices;
using DrawWork.CutomModels;
using System.Collections.ObjectModel;

namespace DrawWork.DrawServices
{
    public class DrawLeaderPublicService
    {

        private AssemblyModel assemblyData;

        private ValueService valueService;
        private DrawWorkingPointService workingPointService;
        private StyleFunctionService styleService;
        private LayerStyleService layerService;

        private DrawPublicFunctionService publicFunService;

        private DrawNozzleBlockService nozzleBlock;

        private DrawService drawService;

        public DrawLeaderPublicService()
        {

        }
        public DrawLeaderPublicService(AssemblyModel selAssembly)
        {

            assemblyData = selAssembly;

            valueService = new ValueService();
            workingPointService = new DrawWorkingPointService(selAssembly);
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();

            publicFunService = new DrawPublicFunctionService();

            nozzleBlock = new DrawNozzleBlockService();

            drawService = new DrawService(selAssembly);

        }

        public List<string> GetLeaderLineText(LeaderListCRTInputModel selLeader)
        {
            List<string> newList = new List<string>();
            if (selLeader != null)
            {
                if (selLeader.RowLine01.Trim() != "")
                    newList.Add(selLeader.RowLine01);
                if (selLeader.RowLine02.Trim() != "")
                    newList.Add(selLeader.RowLine02);
                if (selLeader.RowLine03.Trim() != "")
                    newList.Add(selLeader.RowLine03);
                if (selLeader.RowLine04.Trim() != "")
                    newList.Add(selLeader.RowLine04);
            }
            return newList;
        }
        public List<string> GetLeaderEmptyLineText(LeaderListCRTInputModel selLeader)
        {
            List<string> newList = new List<string>();
            if (selLeader != null)
            {
                if (selLeader.RowEmptyLine01.Trim() != "")
                    newList.Add(selLeader.RowEmptyLine01);
                if (selLeader.RowEmptyLine02.Trim() != "")
                    newList.Add(selLeader.RowEmptyLine02);
                if (selLeader.RowEmptyLine03.Trim() != "")
                    newList.Add(selLeader.RowEmptyLine03);
                if (selLeader.RowEmptyLine04.Trim() != "")
                    newList.Add(selLeader.RowEmptyLine04);
            }

            return newList;
        }


        public List<string> GetLeaderLineTextByName(string selName)
        {
            LeaderListCRTInputModel newModel = GetLeaderModel(selName);
            return GetLeaderLineText(newModel);

        }
        public List<string> GetLeaderEmptyLineTextByName(string selName)
        {
            LeaderListCRTInputModel newModel = GetLeaderModel(selName);
            return GetLeaderEmptyLineText(newModel);

        }


        public LeaderListCRTInputModel GetLeaderModel(string selName)
        {
            LeaderListCRTInputModel newModel = null;
            foreach (LeaderListCRTInputModel eachLeader in assemblyData.LeaderListCRTInput)
            {
                if (eachLeader.Part.ToLower() == selName)
                {
                    newModel = eachLeader;
                    break;
                }
            }
            return newModel;
        }
    }
}
