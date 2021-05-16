using AssemblyLib.AssemblyModels;
using DesignLib.DesignModels;
using DrawWork.CommandModels;
using DrawWork.CommandServices;
using DrawWork.DrawBuilders;
using DrawWork.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using devDept.Eyeshot;
using DrawWork.Commons;

namespace DrawWork.DesignServices
{
    public class IntergrationService : Notifier
    {
		// Information
		public DesignInformationModel Info
		{
			get { return _Info; }
			set
			{
				_Info = value;
				OnPropertyChanged(nameof(Info));
			}
		}
		private DesignInformationModel _Info;

		// Data
		public AssemblyModel tankData
		{
			get { return _tankData; }
			set
			{
				_tankData = value;
				OnPropertyChanged(nameof(tankData));
			}
		}
		private AssemblyModel _tankData;

		// Service
		private CommandBasicService commandService;

		// Model
		private Model singleModel;

		#region CONSTRUCTOR
		public IntergrationService(string selType, AssemblyModel selTankData, object selModel)
        {
			Info = new DesignInformationModel(selType);

			tankData = selTankData;

			singleModel = selModel as Model;

			commandService = new CommandBasicService(tankData, selModel);

			SetDrawSettingInformation();

		}
		#endregion

		// Only GA
		private void SetDrawSettingInformation()
		{
			SingletonData.GAArea.Dimension.Length = "8000";
			SingletonData.GAArea.NozzleLeader.Length = "5000";
			SingletonData.GAArea.ShellCourse.Length = "5000";

			// Assembly : course 2500 : Dim Area 500 : Nozzle Area 5000
		}

		private LogicBuilder GetLogicBuilder(double selScale, string[] selCommandArray)
        {
			// Scale
			commandService.SetScaleData(selScale);
			// Command
			commandService.SetCommandData(commandService.CommandTextToLower(selCommandArray));
			// Createt Entity
			commandService.ExecuteCommand();
			LogicBuilder logicB = new LogicBuilder(commandService.commandEntities);

			return logicB;
		}


		public bool CreateLogic(double selScale,string[] selCommandArray , bool clearEntities = true)
        {
			bool returnValue = false;
            try
            {
				LogicBuilder testBuilder = GetLogicBuilder(selScale, selCommandArray);

				if (clearEntities)
					singleModel.Entities.Clear();

				singleModel.StartWork(testBuilder);

				returnValue = true;

			}
            catch(Exception ex)
            {
				Console.WriteLine(ex.Message);
				returnValue = false;
			}

			return returnValue;
		}

    }
}
