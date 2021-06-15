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
using System.Windows;
using DrawWork.DrawServices;

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
		private Model singleModel2;

		#region CONSTRUCTOR
		public IntergrationService(string selType, AssemblyModel selTankData, object selModel,object selModel2)
        {
			Info = new DesignInformationModel(selType);

			tankData = selTankData;

			singleModel = selModel as Model;

			singleModel2 = selModel2 as Model;

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

		private void SetScaleValue()
        {
            if (singleModel2 != null)
            {
				singleModel2.Entities.Clear();
				singleModel2.Entities.AddRange(commandService.commandEntities);
				singleModel2.Entities.Regen();
			}


			switch (Info.TypeName)
            {
				case "GA":



					SingletonData.GAViewPortSize.X = singleModel2.Entities.BoxSize.X;
					SingletonData.GAViewPortSize.Y = singleModel2.Entities.BoxSize.Y;
					SingletonData.GAViewPortCenter.X = singleModel2.Entities.BoxMin.X;
					SingletonData.GAViewPortCenter.Y = singleModel2.Entities.BoxMin.Y;
					break;
				case "ORIENTATION":

					SingletonData.OrientationViewPortSize.X = singleModel2.Entities.BoxSize.X;
					SingletonData.OrientationViewPortSize.Y = singleModel2.Entities.BoxSize.Y;
					SingletonData.OrientationGAViewPortCenter.X = singleModel2.Entities.BoxMin.X;
					SingletonData.OrientationGAViewPortCenter.Y = singleModel2.Entities.BoxMin.Y;
					break;
			}

			if (singleModel2 != null)
				singleModel2.Entities.Clear();
		}


		public bool CreateLogic(double selScale,string[] selCommandArray, out LogicBuilder outLogicBuilder, bool clearEntities = false)
        {
			outLogicBuilder = null;
			bool returnValue = false;
            try
            {
				LogicBuilder testBuilder = GetLogicBuilder(selScale, selCommandArray);

				//if (clearEntities)
				//	singleModel.Entities.Clear();
				singleModel.Entities.AddRange(testBuilder._EntityList);
				//singleModel.StartWork(testBuilder);
				//outLogicBuilder = testBuilder;

				SetScaleValue();

				returnValue = true;

			}
            catch(Exception ex)
            {
				Console.WriteLine(ex.Message);
				MessageBox.Show(ex.Message);
				returnValue = false;
			}

			return returnValue;
		}

    }
}
