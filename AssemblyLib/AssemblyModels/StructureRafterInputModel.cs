﻿using AssemblyLib.Utils;using System;using System.Collections.Generic;using System.Collections.ObjectModel;using System.Linq;using System.Text;using System.Threading.Tasks;namespace AssemblyLib.AssemblyModels{	public class StructureRafterInputModel : Notifier	{		public StructureRafterInputModel()		{					RafterInNo = "";			RafterInSize = "";			RafterInQty = "";			RafterInRadius = "";		}				private string _RafterInNo;		public string RafterInNo			{				get { return _RafterInNo; }				set				{					_RafterInNo = value;					OnPropertyChanged(nameof(RafterInNo));				}			}				private string _RafterInSize;		public string RafterInSize			{				get { return _RafterInSize; }				set				{					_RafterInSize = value;					OnPropertyChanged(nameof(RafterInSize));				}			}				private string _RafterInQty;		public string RafterInQty			{				get { return _RafterInQty; }				set				{					_RafterInQty = value;					OnPropertyChanged(nameof(RafterInQty));				}			}				private string _RafterInRadius;		public string RafterInRadius			{				get { return _RafterInRadius; }				set				{					_RafterInRadius = value;					OnPropertyChanged(nameof(RafterInRadius));				}			}			}}