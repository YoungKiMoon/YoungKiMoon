using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AssemblyLib.AssemblyModels;
using DrawWork.ValueServices;

namespace DrawWork.CommandServices
{
    public class TranslateDataOutputService
    {
        private ValueService valueService;
        public TranslateDataOutputService()
        {
            valueService = new ValueService();
        }

        public void CreateOutputData(AssemblyModel selAssembly)
        {
            // Auto create output data
            // output Data 자동으로 구성하기 : AC로 표시


            // Input list : first index
            int firstIndex = 0;

            #region Roof : Angle
            // Top angle Output
            selAssembly.RoofAngleOutput.Clear();
            // Type : 아직 고려하지 않음
            string acTopAngleType = valueService.GetAllTrim(selAssembly.RoofInput[firstIndex].TopAngleType);

            string acTopAngleSize = valueService.GetAllTrim(selAssembly.RoofInput[firstIndex].TopAngleSize);
            foreach(AngleSizeModel eachAngle in selAssembly.AngleIList)
            {
                if (valueService.GetAllTrim(eachAngle.SIZE) == acTopAngleSize)
                {
                    selAssembly.RoofAngleOutput.Add(eachAngle);
                    break;
                }
            }

            #endregion


            #region Structure

            // Structure Column Rafter Output
            selAssembly.StructureColumnRafterOutput.Clear();
            string acRafterSize = valueService.GetAllTrim(selAssembly.StructureInput[firstIndex].RafterSize);
            for (int i = 0; i < selAssembly.StructureRafterInput.Count ; i++)
            {
                string rafterSize = valueService.GetAllTrim(selAssembly.StructureRafterInput[i].RafterInSize);
                if (rafterSize != "")
                {
                    foreach (StructureColumnRafterModel eachRafter in selAssembly.StructureColumnRafter)
                    {
                        if (valueService.GetAllTrim(eachRafter.SIZE) == rafterSize)
                        {
                            selAssembly.StructureColumnRafterOutput.Add(eachRafter);
                            break;
                        }
                    }
                }
            }


            // Structure : Clip Shell Side
            selAssembly.StructureColumnClipShellSideOutput.Clear();
            string acRafterSizeLast = valueService.GetAllTrim(selAssembly.StructureRafterInput[selAssembly.StructureRafterInput.Count-1].RafterInSize);
            foreach (StructureColumnClipShellSideModel eachClip in selAssembly.StructureColumnClipShellSide)
            {
                if (valueService.GetAllTrim(eachClip.SIZE) == acRafterSizeLast)
                {
                    selAssembly.StructureColumnClipShellSideOutput.Add(eachClip);
                }
            }


            //int acStructureColumnCount = valueService.GetIntValue(selAssembly.StructureInput[firstIndex].ColumnNo);

            // Structure : Column Center Top Support : One
            // Structure : Column Pipe : Column Count
            // structure : Column Base Support : Column Count
            selAssembly.StructureColumnCenterOutput.Clear();
            selAssembly.StructureColumnPipeOutput.Clear();
            selAssembly.StructureColumnBaseSupportOutput.Clear();
            for (int i = 0; i < selAssembly.StructureColumnInput.Count; i++)
            {
                string eachColumnSize = "";
                string eachRafterSize = "";
                if (selAssembly.StructureColumnInput.Count>i)
                    eachColumnSize = valueService.GetAllTrim(selAssembly.StructureColumnInput[i].ColumnInSize);
                if (selAssembly.StructureRafterInput.Count>i)
                    eachRafterSize = valueService.GetAllTrim(selAssembly.StructureRafterInput[i].RafterInSize);

                if (i == 0 && eachColumnSize != "" && eachRafterSize != "")
                {
                    foreach(StructureColumnCenterModel eachCenter in selAssembly.StructureColumnCenter)
                    {
                        if (valueService.GetAllTrim(eachCenter.COLUMN) == eachColumnSize && valueService.GetAllTrim(eachCenter.SIZE) == eachRafterSize)
                        {
                            selAssembly.StructureColumnCenterOutput.Add(eachCenter);
                            break;
                        }
                    }
                }
                if (eachColumnSize != "")
                {
                    foreach(PipeModel eachPipe in selAssembly.PipeList)
                    {
                        if (eachPipe.NPS == eachColumnSize)
                        {
                            selAssembly.StructureColumnPipeOutput.Add(eachPipe);
                        }
                    }
                    foreach(StructureColumnBaseSupportModel eachBase in selAssembly.StructureColumnBaseSupport)
                    {
                        selAssembly.StructureColumnBaseSupportOutput.Add(eachBase); // 1개만 있음
                    }
                }
            }

            // Structure : Column Side Top Support : Column Count -1
            selAssembly.StructureColumnSideOutput.Clear();
            for(int i = 1; i < selAssembly.StructureRafterInput.Count; i++) // 1 에서 부터 시작
            {
                string rafterSize = valueService.GetAllTrim(selAssembly.StructureRafterInput[i].RafterInSize);
                if (rafterSize !="")
                {
                    foreach (StructureColumnSideModel eachSide in selAssembly.StructureColumnSide)
                    {
                        if (valueService.GetAllTrim(eachSide.SIZE) == rafterSize)
                        {
                            selAssembly.StructureColumnSideOutput.Add(eachSide);
                        }
                    }
                }
            }

            // Structure : Column Girder : Column Count -1
            selAssembly.StructureColumnHBeamOutput.Clear();
            for(int i = 0; i < selAssembly.StructureGirderInput.Count; i++)
            {
                string girderSize = valueService.GetAllTrim(selAssembly.StructureGirderInput[i].GirderInSize);
                if (girderSize != "")
                {
                    foreach(HBeamModel eachHBeam in selAssembly.HBeamList)
                    {
                        if (valueService.GetAllTrim(eachHBeam.SIZE) == girderSize)
                        {
                            selAssembly.StructureColumnHBeamOutput.Add(eachHBeam);
                        }
                    }
                }

            }

            // Structure : Clip Slot Hole
            selAssembly.StructureClipSlotHoleOutput.Clear();
            foreach(ClipSlotHoleModel eachHole in selAssembly.ClipSlotHoleList)
            {
                if (eachHole.Div == "ColumnCenterTop")
                {
                    selAssembly.StructureClipSlotHoleOutput.Add(eachHole);
                }
            }




            #endregion
        }


    }
}
