﻿using AssemblyLib.Utils;using System;using System.Collections.Generic;using System.Collections.ObjectModel;using System.Linq;using System.Text;using System.Threading.Tasks;namespace AssemblyLib.AssemblyModels{	public class StructureColumnClipShellSideModel : Notifier	{		public StructureColumnClipShellSideModel()		{					SIZE = "";			A = "";			B = "";			C = "";			D = "";			E = "";			F = "";			G = "";		}				private string _SIZE;		public string SIZE			{				get { return _SIZE; }				set				{					_SIZE = value;					OnPropertyChanged(nameof(SIZE));				}			}				private string _A;		public string A			{				get { return _A; }				set				{					_A = value;					OnPropertyChanged(nameof(A));				}			}				private string _B;		public string B			{				get { return _B; }				set				{					_B = value;					OnPropertyChanged(nameof(B));				}			}				private string _C;		public string C			{				get { return _C; }				set				{					_C = value;					OnPropertyChanged(nameof(C));				}			}				private string _D;		public string D			{				get { return _D; }				set				{					_D = value;					OnPropertyChanged(nameof(D));				}			}				private string _E;		public string E			{				get { return _E; }				set				{					_E = value;					OnPropertyChanged(nameof(E));				}			}				private string _F;		public string F			{				get { return _F; }				set				{					_F = value;					OnPropertyChanged(nameof(F));				}			}				private string _G;		public string G			{				get { return _G; }				set				{					_G = value;					OnPropertyChanged(nameof(G));				}			}			}}