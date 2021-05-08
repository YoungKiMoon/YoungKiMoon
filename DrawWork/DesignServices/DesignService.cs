using AssemblyLib.AssemblyModels;
using DesignLib.DesignModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DesignServices
{
    public class DesignService
    {

        // CRTModel
        public DesignStepModel CreateDesignCRTModel(AssemblyModel selAssembly)
        {
            // 0 : CRT
            DesignStepModel newStepModel = new DesignStepModel();

            #region Shell
            // 1 : 
            DesignStepModel newStepShell = new DesignStepModel("SHELL", "",true);       // Multi
            newStepModel.Options.Add(newStepShell);
            // 1 : 2 
            DesignStepModel newStepWindGirder = new DesignStepModel("WIND GIRDER", "", false);
            newStepShell.Options.Add(newStepWindGirder);
            // 1 : 2 : 3 
            newStepWindGirder.Options.Add(new DesignStepModel("TYPE-1 (SINGLE ANGLE)", "", false));
            newStepWindGirder.Options.Add(new DesignStepModel("TYPE-1 (DOUBLE ANGLE)", "", false));
            // 1 : 2
            DesignStepModel newStepInsulation = new DesignStepModel("INSULATION", "", false);
            newStepShell.Options.Add(newStepInsulation);
            #endregion

            #region Roof
            // 1 :
            DesignStepModel newStepRoof = new DesignStepModel("ROOF", "", true);        // Multi
            newStepModel.Options.Add(newStepRoof);
            // 1 : 2 
            DesignStepModel newStepTopJoint = new DesignStepModel("TOP JOINT", "", false);
            newStepRoof.Options.Add(newStepTopJoint);
            // 1 : 2 : 3 : 
            newStepTopJoint.Options.Add(new DesignStepModel("TOP ANGLE", "", false));
            newStepTopJoint.Options.Add(new DesignStepModel("SHELL COMPRESSION RING", "", false));
            newStepTopJoint.Options.Add(new DesignStepModel("ROOF COMPRESSION RING", "", false));
            // 1 : 2 
            DesignStepModel newStepRoofInsulation = new DesignStepModel("INSULATION", "", false);
            newStepRoof.Options.Add(newStepRoofInsulation);
            #endregion

            #region Bottom
            // 1 :
            DesignStepModel newStepBottom = new DesignStepModel("BOTTOM", "", true);        // Multi
            newStepModel.Options.Add(newStepBottom);
            // 1 : 2 
            DesignStepModel newStepBottomType = new DesignStepModel("BOTTOM TYPE", "", false);
            newStepBottom.Options.Add(newStepBottomType);
            // 1 : 2 : 3 : 
            newStepBottomType.Options.Add(new DesignStepModel("BOTTOM PLATE", "", false));
            newStepBottomType.Options.Add(new DesignStepModel("ANNULAR PLATE", "", false));
            // 1 : 2 
            DesignStepModel newStepAnchorChar = new DesignStepModel("ANCHOR CHAR", "", false);
            newStepBottom.Options.Add(newStepAnchorChar);
            #endregion

            #region Structure
            // 1 :
            DesignStepModel newStepStructure = new DesignStepModel("STRUCTURE", "", false);
            newStepModel.Options.Add(newStepStructure);
            // 1 : 2 
            DesignStepModel newStepRafterCenterRing = new DesignStepModel("RAFTER CENTER RING", "", true);  // Multi
            newStepStructure.Options.Add(newStepRafterCenterRing);
            // 1 : 2 : 3 : 
            newStepRafterCenterRing.Options.Add(new DesignStepModel("INT. TYPE", "", false));
            newStepRafterCenterRing.Options.Add(new DesignStepModel("EXT. TYPE", "", false));
            newStepRafterCenterRing.Options.Add(new DesignStepModel("PURLIN", "", false));
            // 1 : 2 
            DesignStepModel newStepRafterColumn = new DesignStepModel("RAFTER COLUMN", "", false);
            newStepStructure.Options.Add(newStepRafterColumn);
            // 1 : 2 : 3 : 
            newStepRafterCenterRing.Options.Add(new DesignStepModel("1st COLUMN", "", false));
            newStepRafterCenterRing.Options.Add(new DesignStepModel("2nd COLUMN + GIRDER", "", false));
            #endregion

            #region Nozzle
            // 1 :
            DesignStepModel newStepNozzle = new DesignStepModel("Nozzle", "", false);
            newStepModel.Options.Add(newStepNozzle);
            // 1 : 2 
            DesignStepModel newStepNozzleShell = new DesignStepModel("SHELL", "", false);
            newStepNozzle.Options.Add(newStepNozzleShell);
            // 1 : 2 : 3
            DesignStepModel newStepNozzleShellInlet = new DesignStepModel("INLET", "", false);
            newStepNozzleShell.Options.Add(newStepNozzleShellInlet);
            // 1 : 2 : 3 : 4
            newStepNozzleShellInlet.Options.Add(new DesignStepModel("NORMAL", "", false));
            newStepNozzleShellInlet.Options.Add(new DesignStepModel("INT. PIPE", "", false));
            // 1 : 2 : 3
            DesignStepModel newStepNozzleShellOutlet = new DesignStepModel("OUTLET", "", true);        // Multi
            newStepNozzleShell.Options.Add(newStepNozzleShellOutlet);
            // 1 : 2 : 3 : 4
            newStepNozzleShellOutlet.Options.Add(new DesignStepModel("NORNAL", "", false));
            newStepNozzleShellOutlet.Options.Add(new DesignStepModel("VORTEX BRK.", "", false));
            newStepNozzleShellOutlet.Options.Add(new DesignStepModel("INT. PIPE", "", false));
            // 1 : 2 : 3
            DesignStepModel newStepNozzleShellDrain = new DesignStepModel("DRAIN", "", false);        
            newStepNozzleShell.Options.Add(newStepNozzleShellDrain);
            // 1 : 2 : 3 : 4
            newStepNozzleShellDrain.Options.Add(new DesignStepModel("NORNAL", "", false));
            newStepNozzleShellDrain.Options.Add(new DesignStepModel("SUMP", "", false));
            newStepNozzleShellDrain.Options.Add(new DesignStepModel("LOW", "", false));
            // 1 : 2 : 3
            DesignStepModel newStepNozzleShellManhole = new DesignStepModel("MANHOLE", "", false);
            newStepNozzleShell.Options.Add(newStepNozzleShellManhole);
            // 1 : 2 : 3 : 4
            newStepNozzleShellManhole.Options.Add(new DesignStepModel("DAVIT", "", false));
            newStepNozzleShellManhole.Options.Add(new DesignStepModel("HINGE", "", false));
            // 1 : 2 : 3
            DesignStepModel newStepNozzleShellVent = new DesignStepModel("VENT", "", true);        // Multi
            newStepNozzleShell.Options.Add(newStepNozzleShellVent);
            // 1 : 2 : 3 : 4
            newStepNozzleShellVent.Options.Add(new DesignStepModel("OPEN VENT", "", false));
            newStepNozzleShellVent.Options.Add(new DesignStepModel("FLAME ARRESTOR", "", false));
            newStepNozzleShellVent.Options.Add(new DesignStepModel("BREATHER VALVE", "", false));
            newStepNozzleShellVent.Options.Add(new DesignStepModel("FLAME ARRESTOR & BREATHER VALVE", "", false));
            newStepNozzleShellVent.Options.Add(new DesignStepModel("VACUUM RELIEF VALVE", "", false));
            // 1 : 2 : 3
            DesignStepModel newStepNozzleShellEtc = new DesignStepModel("계장 및 기타 NOZZLE", "", true);        // Multi
            newStepNozzleShell.Options.Add(newStepNozzleShellEtc);
            // 1 : 2 : 3 : 4
            newStepNozzleShellEtc.Options.Add(new DesignStepModel("NORMAL", "", false));
            newStepNozzleShellEtc.Options.Add(new DesignStepModel("W.S RISER PIPE", "", false));
            newStepNozzleShellEtc.Options.Add(new DesignStepModel("SPRAY NOZZLE", "", false));

            // 1 : 2 
            DesignStepModel newStepNozzleRoof = new DesignStepModel("ROOF", "", false);
            newStepNozzle.Options.Add(newStepNozzleRoof);
            // 1 : 2 : 3 : 
            DesignStepModel newStepNozzleRoofInlet = new DesignStepModel("INLET", "", false);
            newStepNozzleRoof.Options.Add(newStepNozzleRoofInlet);
            // 1 : 2 : 3 : 4
            newStepNozzleRoofInlet.Options.Add(new DesignStepModel("NORMAL", "", false));
            newStepNozzleRoofInlet.Options.Add(new DesignStepModel("INT. PIPE", "", false));
            // 1 : 2 : 3 : 
            DesignStepModel newStepNozzleRoofManhole = new DesignStepModel("MANHOLE", "", true);        // Multi
            newStepNozzleRoof.Options.Add(newStepNozzleRoofManhole);
            // 1 : 2 : 3 : 4
            newStepNozzleRoofManhole.Options.Add(new DesignStepModel("HINGE", "", false));
            newStepNozzleRoofManhole.Options.Add(new DesignStepModel("EMERGENCY VENT", "", false));
            // 1 : 2 : 3 : 
            DesignStepModel newStepNozzleRoofEtc = new DesignStepModel("계장 및 기타 NOZZLE", "", true);        // Multi
            newStepNozzleRoof.Options.Add(newStepNozzleRoofEtc);
            // 1 : 2 : 3 : 4
            newStepNozzleRoofEtc.Options.Add(new DesignStepModel("NORMAL", "", false));
            newStepNozzleRoofEtc.Options.Add(new DesignStepModel("INT. PIPE", "", false));

            #endregion

            #region Access
            // 1 :
            DesignStepModel newStepAccess = new DesignStepModel("ACCESS", "", true);        //Multi
            newStepModel.Options.Add(newStepAccess);
            // 1 : 2 
            DesignStepModel newStepAccessShell = new DesignStepModel("SHELL", "", true);  // Multi
            newStepAccess.Options.Add(newStepAccessShell);
            // 1 : 2 : 3 : 
            DesignStepModel newStepAccessShellLadder = new DesignStepModel("LADDER", "", false);
            newStepAccessShell.Options.Add(newStepAccessShellLadder);
            // 1 : 2 : 3 : 4
            newStepAccessShellLadder.Options.Add(new DesignStepModel("INT. TYPE", "", false));
            newStepAccessShellLadder.Options.Add(new DesignStepModel("EXT. TYPE", "", false));
            // 1 : 2 : 3 : 
            newStepAccessShell.Options.Add(new DesignStepModel("SPIRAL STAIRWAY", "", false));
            newStepAccessShell.Options.Add(new DesignStepModel("FOAM MAINTENANCE PLATFORM", "", false));
            // 1 : 2 
            DesignStepModel newStepAccessRoof = new DesignStepModel("ROOF", "", false);
            newStepAccess.Options.Add(newStepAccessRoof);
            // 1 : 2 : 3 : 
            newStepAccessRoof.Options.Add(new DesignStepModel("TOP PLATFORM", "", false));
            #endregion

            #region Appurtenances
            // 1 :
            DesignStepModel newStepAppurtenances = new DesignStepModel("APPURTENANCES", "", false);
            newStepModel.Options.Add(newStepAppurtenances);
            // 1 : 2
            DesignStepModel newStepAppurtenancesShell = new DesignStepModel("SHELL", "", false);
            newStepAppurtenances.Options.Add(newStepAppurtenancesShell);
            // 1 : 2 : 3
            newStepAppurtenancesShell.Options.Add(new DesignStepModel("NAME PLATE", "", false));
            newStepAppurtenancesShell.Options.Add(new DesignStepModel("EARTH LUG", "", false));
            newStepAppurtenancesShell.Options.Add(new DesignStepModel("SETTLEMENT CHECK PIECE", "", false));
            // 1 : 2
            DesignStepModel newStepAppurtenancesRoof = new DesignStepModel("ROOF", "", false);
            newStepAppurtenances.Options.Add(newStepAppurtenancesRoof);
            // 1 : 2 : 3
            newStepAppurtenancesRoof.Options.Add(new DesignStepModel("SCAFFOLD CABLE SUPPORT", "", false));
            #endregion


            return newStepModel;
        }
    }
}
