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
    public class DrawImportBlockService
    {
        public Model singleModel;

        private StyleFunctionService styleSerivce;


        private AssemblyModel assemblyData;

        private ValueService valueService;
        private DrawWorkingPointService workingPointService;
        private StyleFunctionService styleService;
        private LayerStyleService layerService;

        private DrawPublicFunctionService publicFunService;

        private DrawNozzleBlockService nozzleBlock;

        private DrawService drawService;


        private DrawLeaderPublicService leaderDataService;

        public DrawImportBlockService(AssemblyModel selAssembly, Model selModel)
        {
            singleModel = selModel;
            assemblyData = selAssembly;

            valueService = new ValueService();
            workingPointService = new DrawWorkingPointService(selAssembly);
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();

            publicFunService = new DrawPublicFunctionService();

            nozzleBlock = new DrawNozzleBlockService(selAssembly);

            drawService = new DrawService(selAssembly);

            leaderDataService = new DrawLeaderPublicService(selAssembly);

        }

        public BlockReference Draw_ImportBlock(CDPoint selPoint1, string selBlockName, string selLayerName, double scaleFactor=1)
        {
            if (singleModel.Blocks.Contains(selBlockName))
            {
                BlockReference newBlock = new BlockReference(selPoint1.X, selPoint1.Y, 0, selBlockName.ToUpper(), 0);
                newBlock.Scale(scaleFactor);

                newBlock.LayerName = selLayerName;
                //styleSerivce.SetLayer(ref newBlock, selLayerName);
                return newBlock;
            }
            else
            {
                return null;
            }
        }
        public Block GetImportBlock(string selBlockName)
        {
            Block returnBlock = null;
            if (singleModel.Blocks.Contains(selBlockName))
            {
                foreach (Block eachBlock in singleModel.Blocks)
                    if (eachBlock.Name == selBlockName)
                        returnBlock= eachBlock;
            }

            return returnBlock;
        }



        public List<BlockReference> Draw_ImportCustomBlock(CDPoint selPoint1, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            List<BlockReference> newList = new List<BlockReference>();



            double bottomThickness = valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomPlateThickness);
            double shellTopThickness = 0;
            if (assemblyData.ShellOutput.Count > 0)
            {
                shellTopThickness= valueService.GetDoubleValue(assemblyData.ShellOutput[assemblyData.ShellOutput.Count-1].Thickness);
            }
            CDPoint etcBasePoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottom, 0, ref refPoint, ref curPoint);


            if (assemblyData.AppurtenancesInput[0].EarthLug.ToLower() == "yes")
            {
                string earthLugName = "EARTH_LUG";
                BlockReference earthLug = Draw_ImportBlock(GetSumCDPoint(etcBasePoint, 0, 0), earthLugName, layerService.LayerBlock);
                if (earthLug != null)
                {
                    newList.Add(earthLug);

                    // Leader
                    SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                    {
                        leaderPoint = GetSumCDPoint(etcBasePoint, 2500, 300),
                        lineTextList = new List<string>() { assemblyData.AppurtenancesInput[0].ELQty + "-EARTH LUGS" },
                        lineLength = -50,
                        Position = "bottomleft"
                    });
                }
            }


            if (assemblyData.AppurtenancesInput[0].SettlementCheckPiece.ToLower() == "yes")
            {
                string setName = "SETTLEMENT_CHECK_PIECE";

                BlockReference setBlock = Draw_ImportBlock(GetSumCDPoint(etcBasePoint, 0, 0), setName, layerService.LayerBlock);
                if (setBlock != null)
                {
                    newList.Add(setBlock);
                    // Leader
                    SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                    {
                        leaderPoint = GetSumCDPoint(etcBasePoint, 3500, 300),
                        lineTextList = new List<string>() { assemblyData.AppurtenancesInput[0].SCPQty + "-SETTLEMENT CHECK PIECES" },
                        Position = "bottomleft"
                    });
                }

            }


            if (assemblyData.AppurtenancesInput[0].NamePlate.ToLower() == "yes")
            {
                string nameName = "NAME_PLATE";

                BlockReference nameBlock = Draw_ImportBlock(GetSumCDPoint(etcBasePoint, 0, 0), nameName, layerService.LayerBlock);
                if (nameBlock != null)
                {
                    newList.Add(nameBlock);
                    // Leader
                    SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                    {
                        leaderPoint = GetSumCDPoint(etcBasePoint, 1000, 1500),
                        lineTextList = new List<string>() { "NAME PLATE" },
                        Position = "topright"
                    });
                }
            }


            #region Platform & Ladder or Stairway 
            if (SingletonData.TankType == TANK_TYPE.CRT ||
                SingletonData.TankType == TANK_TYPE.DRT ||
                SingletonData.TankType == TANK_TYPE.IFRT)
            {
                if (assemblyData.AccessInput[0].SpiralStairwayVSLadder != "")
                {
                    string stairwayName = assemblyData.AccessInput[0].SpiralStairwayVSLadder.ToLower();

                    string nameName = "";
                    if (stairwayName.Contains("stairway"))
                    {
                        nameName = "SPIRAL_STAIRWAY-1";
                        CDPoint topLeftPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointLeftShellTop, 0, ref refPoint, ref curPoint);

                        BlockReference nameBlock = Draw_ImportBlock(GetSumCDPoint(topLeftPoint, -shellTopThickness, 0), nameName, layerService.LayerBlock);
                        if (nameBlock != null)
                        {
                            newList.Add(nameBlock);
                            // Leader
                            SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                            {
                                leaderPoint = GetSumCDPoint(topLeftPoint, -1473.2, -567.8),
                                lineTextList = new List<string>() { "SPIRAL STAIRWAY", "W/INTERMEDIATE PLATFORM" },
                                Position = "bottomleft"
                            });
                        }

                    }
                    else if (stairwayName.Contains("ladder"))
                    {
                        nameName = "LADDER-1";
                        CDPoint topLeftPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointLeftShellTop, 0, ref refPoint, ref curPoint);

                        BlockReference nameBlock = Draw_ImportBlock(GetSumCDPoint(topLeftPoint, -shellTopThickness, 0), nameName, layerService.LayerBlock);
                        if (nameBlock != null)
                        {
                            newList.Add(nameBlock);
                            // Leader
                            SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                            {
                                leaderPoint = GetSumCDPoint(etcBasePoint, -905, -300),
                                lineTextList = new List<string>() { "EXTERNAL LADDER" },
                                Position = "bottomleft"
                            });
                        }
                    }

                    if (nameName != "")
                    {


                    }

                }
            }
            else if (SingletonData.TankType == TANK_TYPE.EFRTSingle ||
                    SingletonData.TankType == TANK_TYPE.EFRTDouble)
            {
                // Ref Point
                CDPoint refFRTPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointLeftShellTop, 0, ref refPoint, ref curPoint);
                double shellCourseLastThk = 0;
                if (assemblyData.ShellOutput.Count > 0)
                    shellCourseLastThk = valueService.GetDoubleValue(assemblyData.ShellOutput.Last().Thickness);
                Point3D FRTPoint = GetSumPoint(refFRTPoint, -shellCourseLastThk, 0);

                // Platform
                string platformName = "FRT-GAUGING_PLATFORM_&_LADDER";
                //double platformShiftXValue = 500;
                //double platformShiftYValue = 900;
                BlockReference platformBlock = Draw_ImportBlock(GetSumCDPoint(FRTPoint, 0,0), platformName, layerService.LayerBlock);
                if (platformBlock != null)
                {
                    newList.Add(platformBlock);
                    // Leader
                    SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                    {
                        leaderPoint = GetSumCDPoint(FRTPoint, -1165, 1600),
                        lineTextList = new List<string>() {  "W/HANDRAIL", "GAUGING P/F" },
                        lineLength = -50,
                        Position = "topleft"
                    });
                    // Leader
                    SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                    {
                        leaderPoint = GetSumCDPoint(FRTPoint, -910, 250),
                        lineTextList = new List<string>() { "SPIRAL STAIRWAY", "GAUGING P/F" },
                        lineLength = -50,
                        Position = "topleft"
                    });
                }

                // Stairway
                string stairwayName = "FRT-WIND_GIRDER_&_SPIRALSTAIRWAY";
                //double stairwayShiftXValue = 500;
                //double stairwayShiftYValue = 900;
                BlockReference stairwayBlock = Draw_ImportBlock(GetSumCDPoint(FRTPoint, 0 , 0), stairwayName, layerService.LayerBlock);
                if (platformBlock != null)
                {
                    newList.Add(stairwayBlock);
                    // Leader
                    SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                    {
                        leaderPoint = GetSumCDPoint(FRTPoint, -780, -1834),
                        lineTextList = new List<string>() { "W/INTERMEDIATE PLATFORM & HANDIRAIL", "SPIRAL STAIRWAY" },
                        lineLength = -50,
                        Position = "topleft"
                    });
                }
            }
            #endregion


            return newList;
        }


        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
        private Point3D GetSumPoint(CDPoint selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
        private CDPoint GetSumCDPoint(CDPoint selPoint1, double X, double Y, double Z = 0)
        {
            return new CDPoint(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
        private CDPoint GetSumCDPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new CDPoint(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }


    }
}
