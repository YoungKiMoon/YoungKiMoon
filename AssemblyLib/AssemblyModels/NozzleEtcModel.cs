﻿using AssemblyLib.Utils;using System;using System.Collections.Generic;using System.Collections.ObjectModel;using System.Linq;using System.Text;using System.Threading.Tasks;namespace AssemblyLib.AssemblyModels{	public class NozzleEtcModel : Notifier	{		public NozzleEtcModel()		{					GasketThickness = "";		}				private string _GasketThickness;		public string GasketThickness			{				get { return _GasketThickness; }				set				{					_GasketThickness = value;					OnPropertyChanged(nameof(GasketThickness));				}			}			}}