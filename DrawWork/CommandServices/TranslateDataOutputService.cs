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
            string acTopAngleType = GetAllTrim(selAssembly.RoofInput[firstIndex].TopAngleType);

            string acTopAngleSize = GetAllTrim(selAssembly.RoofInput[firstIndex].TopAngleSize);
            foreach(EqualAngleSizeModel eachAngle in selAssembly.AngleIList)
            {
                if (GetAllTrim(eachAngle.SIZE) == acTopAngleSize)
                {
                    selAssembly.RoofAngleOutput.Add(eachAngle);
                    break;
                }
            }

            #endregion


            #region Structure

            // Structure Column Rafter Output : Only 1 Row
            selAssembly.StructureColumnRafterOutput.Clear();
            string acRafterSize = GetAllTrim(selAssembly.StructureInput[firstIndex].RafterSize);
            foreach(StructureColumnRafterModel eachRafter in selAssembly.StructureColumnRafter)
            {
                if (GetAllTrim(eachRafter.SIZE)== acRafterSize)
                {
                    selAssembly.StructureColumnRafterOutput.Add(eachRafter);
                    break;
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
                    eachColumnSize = selAssembly.StructureColumnInput[i].Size;
                if (selAssembly.StructureRafterInput.Count>i)
                    eachRafterSize = selAssembly.StructureRafterInput[i].Size;

                if (i == 0 && eachColumnSize != "" && eachRafterSize != "")
                {
                    foreach(StructureColumnCenterModel eachCenter in selAssembly.StructureColumnCenter)
                    {
                        if (eachCenter.COLUMN == eachColumnSize && eachCenter.SIZE == eachRafterSize)
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
            // Structure : Column Girder : Column Count -1
            selAssembly.StructureColumnSideOutput.Clear();
            selAssembly.StructureColumnHBeamOutput.Clear();
            for(int i = 0; i < selAssembly.StructureGirderInput.Count; i++)
            {
                string girderSize = selAssembly.StructureGirderInput[i].Size;
                if (girderSize != "")
                {
                    foreach(StructureColumnSideModel eachSide in selAssembly.StructureColumnSide)
                    {
                        if (eachSide.SIZE == girderSize)
                        {
                            selAssembly.StructureColumnSideOutput.Add(eachSide);
                        }
                    }
                    foreach(HBeamModel eachHBeam in selAssembly.HBeamList)
                    {
                        if (eachHBeam.SIZE == girderSize)
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

        private string GetAllTrim(string selStr)
        {
            return selStr.Replace(" ", "");
        }
    }
}
