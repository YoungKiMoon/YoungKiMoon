using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DrawWork.AssemblyModels;
using DrawWork.CommandModels;
using DrawWork.CommandServices;
using DrawWork.DrawBuilders;

namespace DrawWork.ViewModels
{
    public class MainWindowViewModel
    {

        private AssemblyModel TankData;
        private BasicCommandModel commandData;
        private CommandBasicService commandService;

        public MainWindowViewModel()
        {
            TankData = new AssemblyModel();
            commandData = new BasicCommandModel();

            CreateSampleAssembly();
            CreateSampleCommandModel();

            commandService = new CommandBasicService(commandData.commandList, TankData);
        }

        private void CreateSampleAssembly()
        {
            // Input Data
            ShellInputModel newInput = new ShellInputModel();
            newInput.No = "1";
            newInput.ID = "48000";
            newInput.Height = "16400";
            newInput.PLWidth = "10000";
            newInput.PLHeight = "2400";

            TankData.ShellInput.Add(newInput);


            // Output Data
            List<string[]> newCourseList = new List<string[]>();
            string[] newCourseStr1 = new string[7] { "1", "1st", "19", "0", "2400", "9432", "16" };
            string[] newCourseStr2 = new string[7] { "2", "2nd", "17", "3144", "2400", "9432", "16" };
            string[] newCourseStr3 = new string[7] { "3", "3rd", "15", "6288", "2400", "9432", "16" };
            string[] newCourseStr4 = new string[7] { "4", "4th", "12", "0", "2400", "9432", "16" };
            string[] newCourseStr5 = new string[7] { "5", "5th", "10", "3144", "2400", "9432", "16" };
            string[] newCourseStr6 = new string[7] { "6", "6th", "8", "6288", "2400", "9432", "16" };
            string[] newCourseStr7 = new string[7] { "7", "7th", "8", "0", "2000", "9432", "16" };

            newCourseList.Add(newCourseStr1);
            newCourseList.Add(newCourseStr2);
            newCourseList.Add(newCourseStr3);
            newCourseList.Add(newCourseStr4);
            newCourseList.Add(newCourseStr5);
            newCourseList.Add(newCourseStr6);
            newCourseList.Add(newCourseStr7);

            foreach (string[] eachStr in newCourseList)
            {
                ShellOutputModel newCourse = new ShellOutputModel();
                newCourse.No = eachStr[0];
                newCourse.Course = eachStr[1];
                newCourse.Thickness = eachStr[2];
                newCourse.StartPoint = eachStr[3];
                newCourse.OnePLHeight = eachStr[4];
                newCourse.OnePLWidth = eachStr[5];
                newCourse.Count = eachStr[6];

                TankData.ShellOutput.Add(newCourse);
            }

        }

        private void CreateSampleCommandModel()
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
                commandData.commandList.Add(new CommandLineModel { CommandText = eachText });
        }

        private void CreateModel()
        {
            commandService.ExecuteCommand();   
            
        }

        public LogicBuilder GetLogicBuilder()
        {
            LogicBuilder logicB = new LogicBuilder(TankData);

            return logicB;
        }
    }
}
