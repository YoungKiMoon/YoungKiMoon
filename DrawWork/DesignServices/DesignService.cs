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
            newStepWindGirder.Options.Add(new DesignStepModel("detail C", "", false));
            newStepWindGirder.Options.Add(new DesignStepModel("detail d", "", false));
            // 1 : 2
            DesignStepModel newStepInsulation = new DesignStepModel("INSULATION", "", false);
            newStepShell.Options.Add(newStepInsulation);
            newStepInsulation.Options.Add(new DesignStepModel("Insulation", "", false));
            #endregion

            #region Roof
            // 1 :
            DesignStepModel newStepRoof = new DesignStepModel("ROOF", "", true);        // Multi
            newStepModel.Options.Add(newStepRoof);
            // 1 : 2 
            DesignStepModel newStepTopJoint = new DesignStepModel("TOP JOINT", "", false,true);
            newStepRoof.Options.Add(newStepTopJoint);
            // 1 : 2 : 3 : 
            newStepTopJoint.Options.Add(new DesignStepModel("detail b", "", false));
            newStepTopJoint.Options.Add(new DesignStepModel("detail d", "", false));
            newStepTopJoint.Options.Add(new DesignStepModel("detail e", "", false));
            newStepTopJoint.Options.Add(new DesignStepModel("detail i", "", false));
            newStepTopJoint.Options.Add(new DesignStepModel("detail k", "", false));
            // 1 : 2 
            DesignStepModel newStepRoofInsulation = new DesignStepModel("INSULATION", "", false);
            newStepRoof.Options.Add(newStepRoofInsulation);
            newStepRoofInsulation.Options.Add(new DesignStepModel("INSULATION", "", false));
            #endregion

            #region Bottom
            // 1 :
            DesignStepModel newStepBottom = new DesignStepModel("BOTTOM", "", true);        // Multi
            newStepModel.Options.Add(newStepBottom);
            // 1 : 2 
            DesignStepModel newStepAnnularPlate = new DesignStepModel("ANNULAR PLATE", "", false);
            newStepBottom.Options.Add(newStepAnnularPlate);
            // 1 : 2 : 3 : 
            newStepAnnularPlate.Options.Add(new DesignStepModel("ANNULAR PLATE", "", false));
            // 1 : 2 
            DesignStepModel newStepDripRing = new DesignStepModel("DRIP RING", "", false);
            newStepBottom.Options.Add(newStepDripRing);
            // 1 : 2 : 3 : 
            newStepDripRing.Options.Add(new DesignStepModel("DRIP RING", "", false));
            // 1 : 2 
            DesignStepModel newStepAnchorChar = new DesignStepModel("ANCHOR CHAR", "", false);
            newStepBottom.Options.Add(newStepAnchorChar);
            // 1 : 2 : 3 : 
            newStepAnchorChar.Options.Add(new DesignStepModel("ANCHOR CHAR", "", false));
            #endregion

            #region Structure
            // 1 :
            DesignStepModel newStepStructure = new DesignStepModel("STRUCTURE", "", false);
            newStepModel.Options.Add(newStepStructure);
            // 1 : 2 
            DesignStepModel newStepStructureType = new DesignStepModel("STRUCTURE TYPE", "", false,true);  // Multi
            newStepStructure.Options.Add(newStepStructureType);
            // 1 : 2 : 3 : 
            newStepStructureType.Options.Add(new DesignStepModel("Self-Supporting", "", false));
            newStepStructureType.Options.Add(new DesignStepModel("Rafter Only(internal)", "", false));
            newStepStructureType.Options.Add(new DesignStepModel("Rafter Only(external)", "", false));
            newStepStructureType.Options.Add(new DesignStepModel("Rafter w/Column", "", false));
            newStepStructureType.Options.Add(new DesignStepModel("Rafter w/Column & Girder", "", false));
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
            DesignStepModel newStepAccessSSL = new DesignStepModel("Spiral Stairway vs Ladder", "", false,true);
            newStepAccess.Options.Add(newStepAccessSSL);
            // 1 : 2 : 3 : 
            newStepAccessSSL.Options.Add(new DesignStepModel("LADDER", "", false));
            newStepAccessSSL.Options.Add(new DesignStepModel("SPIRAL STAIRWAY", "", false));
            // 1 : 2 
            DesignStepModel newStepAccessRoofPlatform = new DesignStepModel("ROOF PLATFORM", "", false);
            newStepAccess.Options.Add(newStepAccessRoofPlatform);
            // 1 : 2 : 3 : 
            newStepAccessRoofPlatform.Options.Add(new DesignStepModel("ROOF PLATFORM", "", false));
            // 1 : 2 
            DesignStepModel newStepAccessFMP = new DesignStepModel("FOAM MAINTENANCE PLATFORM", "", false);
            newStepAccess.Options.Add(newStepAccessFMP);
            // 1 : 2 : 3 : 
            newStepAccessFMP.Options.Add(new DesignStepModel("FOAM MAINTENANCE PLATFORM", "", false));
            #endregion

            #region Appurtenances
            // 1 :
            DesignStepModel newStepAppurtenances = new DesignStepModel("APPURTENANCES", "", false);
            newStepModel.Options.Add(newStepAppurtenances);
            // 1 : 2
            DesignStepModel newStepAppurtenancesNamePlate = new DesignStepModel("NAME PLATE", "", false,true);
            newStepAppurtenances.Options.Add(newStepAppurtenancesNamePlate);
            // 1 : 2 : 3
            newStepAppurtenancesNamePlate.Options.Add(new DesignStepModel("NAME PLATE", "", false));
            // 1 : 2
            DesignStepModel newStepAppurtenancesEarthLug = new DesignStepModel("EARTH LUG", "", false, true);
            newStepAppurtenances.Options.Add(newStepAppurtenancesEarthLug);
            // 1 : 2 : 3
            newStepAppurtenancesEarthLug.Options.Add(new DesignStepModel("EARTH LUG", "", false));
            // 1 : 2
            DesignStepModel newStepAppurtenancesSCP = new DesignStepModel("SETTLEMENT CHECK PIECE", "", false, true);
            newStepAppurtenances.Options.Add(newStepAppurtenancesSCP);
            // 1 : 2 : 3
            newStepAppurtenancesSCP.Options.Add(new DesignStepModel("SETTLEMENT CHECK PIECE", "", false));
            // 1 : 2
            DesignStepModel newStepAppurtenancesSCS = new DesignStepModel("NAME PLATE", "", false, true);
            newStepAppurtenances.Options.Add(newStepAppurtenancesSCS);
            // 1 : 2 : 3
            newStepAppurtenancesSCS.Options.Add(new DesignStepModel("NAME PLATE", "", false));

            #endregion


            return newStepModel;
        }
    }
}
