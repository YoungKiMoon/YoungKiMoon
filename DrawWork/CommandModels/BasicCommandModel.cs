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
            commandListTransFunciton = new List<string[]>();
            drawPoint = new DrawPointModel();
        }

        public object Clone()
        {
            BasicCommandModel newModel = new BasicCommandModel();
            newModel.commandList = commandList;
            newModel.commandListTrans = commandListTrans;
            newModel.commandListTransFunciton = commandListTransFunciton;
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

        private List<string[]> _commandListTransFunciton;
        public List<string[]> commandListTransFunciton
        {
            get { return _commandListTransFunciton; }
            set { _commandListTransFunciton = value; }
        }

        private DrawPointModel _drawPoint;
        public DrawPointModel drawPoint
        {
            get { return _drawPoint; }
            set { _drawPoint = value; }
        }



        #region Sample Data
        public void CreateSampleCommandModel()
        {
            List<string> newComData = new List<string>();

            newComData.Add("refpoint xy 0,0");
            newComData.Add("rec xy @startpoint[1],@0 w OnePLWidth[1] h OnePLHeight[1] sp @OnePLWidth[1],@0 rep Count[1]");
            newComData.Add("point xy refpointx,@0");
            newComData.Add("text xy @startpoint[1]+OnePLWidth[1]/2,@0+OnePLHeight[1]/2 t S-1 h 400 a c sp @OnePLWidth[1],@0 rep Count[1]");

            newComData.Add("point xy refpointx,@OnePLHeight[1]");
            newComData.Add("rec xy @startpoint[2],@0 w OnePLWidth[2] h OnePLHeight[2] sp @OnePLWidth[2],@0 rep Count[2]");
            newComData.Add("point xy refpointx,@0");
            newComData.Add("text xy @startpoint[2]+OnePLWidth[2]/2,@0+OnePLHeight[2]/2 t S-2 h 400 a c sp @OnePLWidth[2],@0 rep Count[2]");

            newComData.Add("point xy refpointx,@OnePLHeight[2]");
            newComData.Add("rec xy @startpoint[3],@0 w OnePLWidth[3] h OnePLHeight[3] sp @OnePLWidth[3],@0 rep Count[3]");
            newComData.Add("point xy refpointx,@0");
            newComData.Add("text xy @startpoint[3]+OnePLWidth[3]/2,@0+OnePLHeight[3]/2 t S-3 h 400 a c sp @OnePLWidth[3],@0 rep Count[3]");

            newComData.Add("point xy refpointx,@OnePLHeight[3]");
            newComData.Add("rec xy @startpoint[4],@0 w OnePLWidth[4] h OnePLHeight[4] sp @OnePLWidth[4],@0 rep Count[4]");
            newComData.Add("point xy refpointx,@0");
            newComData.Add("text xy @startpoint[4]+OnePLWidth[4]/2,@0+OnePLHeight[4]/2 t S-4 h 400 a c sp @OnePLWidth[4],@0 rep Count[4]");

            newComData.Add("point xy refpointx,@OnePLHeight[4]");
            newComData.Add("rec xy @startpoint[5],@0 w OnePLWidth[5] h OnePLHeight[5] sp @OnePLWidth[5],@0 rep Count[5]");
            newComData.Add("point xy refpointx,@0");
            newComData.Add("text xy @startpoint[5]+OnePLWidth[5]/2,@0+OnePLHeight[5]/2 t S-5 h 400 a c sp @OnePLWidth[5],@0 rep Count[5]");

            newComData.Add("point xy refpointx,@OnePLHeight[5]");
            newComData.Add("rec xy @startpoint[6],@0 w OnePLWidth[6] h OnePLHeight[6] sp @OnePLWidth[4],@0 rep Count[6]");
            newComData.Add("point xy refpointx,@0");
            newComData.Add("text xy @startpoint[6]+OnePLWidth[6]/2,@0+OnePLHeight[6]/2 t S-6 h 400 a c sp @OnePLWidth[6],@0 rep Count[6]");

            newComData.Add("point xy refpointx,@OnePLHeight[6]");
            newComData.Add("rec xy @startpoint[7],@0 w OnePLWidth[7] h OnePLHeight[7] sp @OnePLWidth[7],@0 rep Count[7]");
            newComData.Add("point xy refpointx,@0");
            newComData.Add("text xy @startpoint[7]+OnePLWidth[7]/2,@0+OnePLHeight[7]/2 t S-7 h 400 a c sp @OnePLWidth[7],@0 rep Count[7]");



            newComData.Add("point xy refpointx,refpointy");
            newComData.Add("dimline xy1 refpointx,refpointy xy2 refpointx+OnePLWidth[1],refpointy xyh 0,OnePLHeight[1]*-0.8 th 350 tg 150 ah 400");

            newComData.Add("dimline xy1 refpointx,refpointy+OnePLHeight[1] xy2 refpointx+startpoint[2],refpointy+OnePLHeight[1]+OnePLHeight[2] xyh @0,@0+OnePLHeight[1]+OnePLHeight[2]+OnePLHeight[3]*0.5 th 350 tg 150 ah 400");
            newComData.Add("dimline xy1 refpointx+startpoint[2],refpointy+OnePLHeight[1]+OnePLHeight[2] xy2 refpointx+startpoint[3],refpointy+OnePLHeight[1]+OnePLHeight[2]+OnePLHeight[3]*0.5 xyh @0,@0+OnePLHeight[1]+OnePLHeight[2]+OnePLHeight[3]*0.5 th 350 tg 150 ah 400");

            newComData.Add("line xy1 @0,@0 xy2 @0,@0-OnePLHeight[1]*2");
            newComData.Add("text xy @0,@0-OnePLHeight[1]*2+OnePLHeight[1]/2+160 t GAUGE_LINE_OF h 400");
            newComData.Add("text xy @0,@0-OnePLHeight[1]*2+160 t 1st_COURSE_SHEELL_PLATE h 400");
            newComData.Add("line xy1 @0,@0-OnePLHeight[1]*2+OnePLHeight[1]/2 xy2 @0+OnePLWidth[1]/2,@0-OnePLHeight[1]*2+OnePLHeight[1]/2");
            newComData.Add("line xy1 @0,@0-OnePLHeight[1]*2 xy2 @0+OnePLWidth[1]*0.9,@0-OnePLHeight[1]*2");
            newComData.Add("text xy @0+OnePLWidth[1]*Count[1]/2,@0-OnePLHeight[1]*2+340 t SHELL_PLATE_ARRANGEMENT h 800 ");
            newComData.Add("line xy1 @0+OnePLWidth[1]*Count[1]/2,@0-OnePLHeight[1]*2 xy2 OnePLWidth[1]*Count[1]/2+17000,@0-OnePLHeight[1]*2");













            newComData.Add("");
            newComData.Add("");
            newComData.Add("");
            newComData.Add("");
            newComData.Add("");

            newComData.Add("refpoint xy 0,40000");
            newComData.Add("rec xy @startpoint[1],@0 w OnePLWidth[1] h OnePLHeight[1] sp @OnePLWidth[1],@0 rep Count[1]/2");
            newComData.Add("point xy refpointx,@0");
            newComData.Add("text xy @startpoint[1]+OnePLWidth[1]/2,@0+OnePLHeight[1]/2 t S-1 h 400 a c sp @OnePLWidth[1],@0 rep Count[1]/2");

            newComData.Add("point xy refpointx,@OnePLHeight[1]");
            newComData.Add("rec xy @startpoint[2],@0 w OnePLWidth[2] h OnePLHeight[2] sp @OnePLWidth[2],@0 rep Count[2]/2");
            newComData.Add("point xy refpointx,@0");
            newComData.Add("text xy @startpoint[2]+OnePLWidth[2]/2,@0+OnePLHeight[2]/2 t S-2 h 400 a c sp @OnePLWidth[2],@0 rep Count[2]/2");

            newComData.Add("point xy refpointx,@OnePLHeight[2]");
            newComData.Add("rec xy @startpoint[3],@0 w OnePLWidth[3] h OnePLHeight[3] sp @OnePLWidth[3],@0 rep Count[3]/2");
            newComData.Add("point xy refpointx,@0");
            newComData.Add("text xy @startpoint[3]+OnePLWidth[3]/2,@0+OnePLHeight[3]/2 t S-3 h 400 a c sp @OnePLWidth[3],@0 rep Count[3]/2");

            newComData.Add("point xy refpointx,@OnePLHeight[3]");
            newComData.Add("rec xy @startpoint[4],@0 w OnePLWidth[4] h OnePLHeight[4] sp @OnePLWidth[4],@0 rep Count[4]/2");
            newComData.Add("point xy refpointx,@0");
            newComData.Add("text xy @startpoint[4]+OnePLWidth[4]/2,@0+OnePLHeight[4]/2 t S-4 h 400 a c sp @OnePLWidth[4],@0 rep Count[4]/2");

            newComData.Add("point xy refpointx,@OnePLHeight[4]");
            newComData.Add("rec xy @startpoint[5],@0 w OnePLWidth[5] h OnePLHeight[5] sp @OnePLWidth[5],@0 rep Count[5]/2");
            newComData.Add("point xy refpointx,@0");
            newComData.Add("text xy @startpoint[5]+OnePLWidth[5]/2,@0+OnePLHeight[5]/2 t S-5 h 400 a c sp @OnePLWidth[5],@0 rep Count[5]/2");

            newComData.Add("point xy refpointx,@OnePLHeight[5]");
            newComData.Add("rec xy @startpoint[6],@0 w OnePLWidth[6] h OnePLHeight[6] sp @OnePLWidth[4],@0 rep Count[6]/2");
            newComData.Add("point xy refpointx,@0");
            newComData.Add("text xy @startpoint[6]+OnePLWidth[6]/2,@0+OnePLHeight[6]/2 t S-6 h 400 a c sp @OnePLWidth[6],@0 rep Count[6]/2");

            newComData.Add("point xy refpointx,@OnePLHeight[6]");
            newComData.Add("rec xy @startpoint[7],@0 w OnePLWidth[7] h OnePLHeight[7] sp @OnePLWidth[7],@0 rep Count[7]/2");
            newComData.Add("point xy refpointx,@0");
            newComData.Add("text xy @startpoint[7]+OnePLWidth[7]/2,@0+OnePLHeight[7]/2 t S-7 h 400 a c sp @OnePLWidth[7],@0 rep Count[7]/2");



            newComData.Add("point xy refpointx,refpointy");
            newComData.Add("dimline xy1 refpointx,refpointy xy2 refpointx+OnePLWidth[1],refpointy xyh @0,@0+OnePLHeight[1]*-0.8 th 350 tg 150 ah 400");

            newComData.Add("dimline xy1 refpointx,refpointy+OnePLHeight[1] xy2 refpointx+startpoint[2],refpointy+OnePLHeight[1]+OnePLHeight[2] xyh @0,@0+OnePLHeight[1]+OnePLHeight[2]+OnePLHeight[3]*0.5 th 350 tg 150 ah 400");
            newComData.Add("dimline xy1 refpointx+startpoint[2],refpointy+OnePLHeight[1]+OnePLHeight[2] xy2 refpointx+startpoint[3],refpointy+OnePLHeight[1]+OnePLHeight[2]+OnePLHeight[3]*0.5 xyh @0,@0+OnePLHeight[1]+OnePLHeight[2]+OnePLHeight[3]*0.5 th 350 tg 150 ah 400");

            newComData.Add("line xy1 @0,@0 xy2 @0,@0-OnePLHeight[1]*2");
            newComData.Add("text xy @0,@0-OnePLHeight[1]*2+OnePLHeight[1]/2+160 t GAUGE_LINE_OF h 400");
            newComData.Add("text xy @0,@0-OnePLHeight[1]*2+160 t 1st_COURSE_SHEELL_PLATE h 400");
            newComData.Add("line xy1 @0,@0-OnePLHeight[1]*2+OnePLHeight[1]/2 xy2 @0+OnePLWidth[1]/2,@0-OnePLHeight[1]*2+OnePLHeight[1]/2");
            newComData.Add("line xy1 @0,@0-OnePLHeight[1]*2 xy2 @0+OnePLWidth[1]*0.9,@0-OnePLHeight[1]*2");
            newComData.Add("text xy @0+OnePLWidth[1]*Count[1]/4,@0-OnePLHeight[1]*2+340 t SHELL_PLATE_ARRANGEMENT h 800 ");
            newComData.Add("line xy1 @0+OnePLWidth[1]*Count[1]/4,@0-OnePLHeight[1]*2 xy2 OnePLWidth[1]*Count[1]/4+17000,@0-OnePLHeight[1]*2");


            newComData.Add("rectangle xy @0+OnePLWidth[1]*Count[1]/4,@0-1000 w 1000 h Height[1]*1.3 r -30");

            foreach (string eachText in newComData)
                commandList.Add(new CommandLineModel { CommandText = eachText });
        }
        #endregion
    }
}
