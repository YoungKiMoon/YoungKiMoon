﻿using AssemblyLib.Utils;using System;using System.Collections.Generic;using System.Collections.ObjectModel;using System.Linq;using System.Text;using System.Threading.Tasks;namespace AssemblyLib.AssemblyModels{	public class StructureColumnRafterModel : Notifier	{		public StructureColumnRafterModel()		{					SIZE = "";			Rafter1 = "";			Rafter2 = "";			Rafter3 = "";			Rafter4 = "";			Rafter5 = "";			Rafter6 = "";			A = "";			B = "";			C = "";			D = "";			E = "";			BoltHole = "";			R1 = "";			R2 = "";		}				private string _SIZE;		public string SIZE			{				get { return _SIZE; }				set				{					_SIZE = value;					OnPropertyChanged(nameof(SIZE));				}			}				private string _Rafter1;		public string Rafter1			{				get { return _Rafter1; }				set				{					_Rafter1 = value;					OnPropertyChanged(nameof(Rafter1));				}			}				private string _Rafter2;		public string Rafter2			{				get { return _Rafter2; }				set				{					_Rafter2 = value;					OnPropertyChanged(nameof(Rafter2));				}			}				private string _Rafter3;		public string Rafter3			{				get { return _Rafter3; }				set				{					_Rafter3 = value;					OnPropertyChanged(nameof(Rafter3));				}			}				private string _Rafter4;		public string Rafter4			{				get { return _Rafter4; }				set				{					_Rafter4 = value;					OnPropertyChanged(nameof(Rafter4));				}			}				private string _Rafter5;		public string Rafter5			{				get { return _Rafter5; }				set				{					_Rafter5 = value;					OnPropertyChanged(nameof(Rafter5));				}			}				private string _Rafter6;		public string Rafter6			{				get { return _Rafter6; }				set				{					_Rafter6 = value;					OnPropertyChanged(nameof(Rafter6));				}			}				private string _A;		public string A			{				get { return _A; }				set				{					_A = value;					OnPropertyChanged(nameof(A));				}			}				private string _B;		public string B			{				get { return _B; }				set				{					_B = value;					OnPropertyChanged(nameof(B));				}			}				private string _C;		public string C			{				get { return _C; }				set				{					_C = value;					OnPropertyChanged(nameof(C));				}			}				private string _D;		public string D			{				get { return _D; }				set				{					_D = value;					OnPropertyChanged(nameof(D));				}			}				private string _E;		public string E			{				get { return _E; }				set				{					_E = value;					OnPropertyChanged(nameof(E));				}			}				private string _BoltHole;		public string BoltHole			{				get { return _BoltHole; }				set				{					_BoltHole = value;					OnPropertyChanged(nameof(BoltHole));				}			}				private string _R1;		public string R1			{				get { return _R1; }				set				{					_R1 = value;					OnPropertyChanged(nameof(R1));				}			}				private string _R2;		public string R2			{				get { return _R2; }				set				{					_R2 = value;					OnPropertyChanged(nameof(R2));				}			}			}}