﻿using AssemblyLib.Utils;using System;using System.Collections.Generic;using System.Collections.ObjectModel;using System.Linq;using System.Text;using System.Threading.Tasks;namespace AssemblyLib.AssemblyModels{	public class NozzleOHFLWNModel : Notifier	{		public NozzleOHFLWNModel()		{					DN = "";			NPS = "";			G = "";			OD = "";			BCD = "";			RRF = "";			RFF = "";			H = "";			B = "";			C = "";			BoltNo = "";			BoltSize1 = "";			BoltSize2 = "";			BoltLength = "";		}				private string _DN;		public string DN			{				get { return _DN; }				set				{					_DN = value;					OnPropertyChanged(nameof(DN));				}			}				private string _NPS;		public string NPS			{				get { return _NPS; }				set				{					_NPS = value;					OnPropertyChanged(nameof(NPS));				}			}				private string _G;		public string G			{				get { return _G; }				set				{					_G = value;					OnPropertyChanged(nameof(G));				}			}				private string _OD;		public string OD			{				get { return _OD; }				set				{					_OD = value;					OnPropertyChanged(nameof(OD));				}			}				private string _BCD;		public string BCD			{				get { return _BCD; }				set				{					_BCD = value;					OnPropertyChanged(nameof(BCD));				}			}				private string _RRF;		public string RRF			{				get { return _RRF; }				set				{					_RRF = value;					OnPropertyChanged(nameof(RRF));				}			}				private string _RFF;		public string RFF			{				get { return _RFF; }				set				{					_RFF = value;					OnPropertyChanged(nameof(RFF));				}			}				private string _H;		public string H			{				get { return _H; }				set				{					_H = value;					OnPropertyChanged(nameof(H));				}			}				private string _B;		public string B			{				get { return _B; }				set				{					_B = value;					OnPropertyChanged(nameof(B));				}			}				private string _C;		public string C			{				get { return _C; }				set				{					_C = value;					OnPropertyChanged(nameof(C));				}			}				private string _BoltNo;		public string BoltNo			{				get { return _BoltNo; }				set				{					_BoltNo = value;					OnPropertyChanged(nameof(BoltNo));				}			}				private string _BoltSize1;		public string BoltSize1			{				get { return _BoltSize1; }				set				{					_BoltSize1 = value;					OnPropertyChanged(nameof(BoltSize1));				}			}				private string _BoltSize2;		public string BoltSize2			{				get { return _BoltSize2; }				set				{					_BoltSize2 = value;					OnPropertyChanged(nameof(BoltSize2));				}			}				private string _BoltLength;		public string BoltLength			{				get { return _BoltLength; }				set				{					_BoltLength = value;					OnPropertyChanged(nameof(BoltLength));				}			}			}}