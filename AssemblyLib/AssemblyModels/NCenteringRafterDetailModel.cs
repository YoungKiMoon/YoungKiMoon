﻿using AssemblyLib.Utils;using System;using System.Collections.Generic;using System.Collections.ObjectModel;using System.Linq;using System.Text;using System.Threading.Tasks;namespace AssemblyLib.AssemblyModels{	public class NCenteringRafterDetailModel : Notifier	{		public NCenteringRafterDetailModel()		{					CenteringOD = "";			Scale = "";		}				private string _CenteringOD;		public string CenteringOD			{				get { return _CenteringOD; }				set				{					_CenteringOD = value;					OnPropertyChanged(nameof(CenteringOD));				}			}				private string _Scale;		public string Scale			{				get { return _Scale; }				set				{					_Scale = value;					OnPropertyChanged(nameof(Scale));				}			}			}}