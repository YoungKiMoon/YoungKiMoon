﻿using AssemblyLib.Utils;using System;using System.Collections.Generic;using System.Collections.ObjectModel;using System.Linq;using System.Text;using System.Threading.Tasks;namespace AssemblyLib.AssemblyModels{	public class NozzleOHFSeriesAModel : Notifier	{		public NozzleOHFSeriesAModel()		{					DN = "";			NPS = "";			G = "";			OD = "";			BCD = "";			RRF = "";			RFF = "";			H = "";			A = "";			BWN = "";			BBF = "";			C = "";			BoltNo = "";			BoltSize1 = "";			BoltSize2 = "";			BoltLengthWNBF = "";			BoltLengthWNWN = "";		}				private string _DN;		public string DN			{				get { return _DN; }				set				{					_DN = value;					OnPropertyChanged(nameof(DN));				}			}				private string _NPS;		public string NPS			{				get { return _NPS; }				set				{					_NPS = value;					OnPropertyChanged(nameof(NPS));				}			}				private string _G;		public string G			{				get { return _G; }				set				{					_G = value;					OnPropertyChanged(nameof(G));				}			}				private string _OD;		public string OD			{				get { return _OD; }				set				{					_OD = value;					OnPropertyChanged(nameof(OD));				}			}				private string _BCD;		public string BCD			{				get { return _BCD; }				set				{					_BCD = value;					OnPropertyChanged(nameof(BCD));				}			}				private string _RRF;		public string RRF			{				get { return _RRF; }				set				{					_RRF = value;					OnPropertyChanged(nameof(RRF));				}			}				private string _RFF;		public string RFF			{				get { return _RFF; }				set				{					_RFF = value;					OnPropertyChanged(nameof(RFF));				}			}				private string _H;		public string H			{				get { return _H; }				set				{					_H = value;					OnPropertyChanged(nameof(H));				}			}				private string _A;		public string A			{				get { return _A; }				set				{					_A = value;					OnPropertyChanged(nameof(A));				}			}				private string _BWN;		public string BWN			{				get { return _BWN; }				set				{					_BWN = value;					OnPropertyChanged(nameof(BWN));				}			}				private string _BBF;		public string BBF			{				get { return _BBF; }				set				{					_BBF = value;					OnPropertyChanged(nameof(BBF));				}			}				private string _C;		public string C			{				get { return _C; }				set				{					_C = value;					OnPropertyChanged(nameof(C));				}			}				private string _BoltNo;		public string BoltNo			{				get { return _BoltNo; }				set				{					_BoltNo = value;					OnPropertyChanged(nameof(BoltNo));				}			}				private string _BoltSize1;		public string BoltSize1			{				get { return _BoltSize1; }				set				{					_BoltSize1 = value;					OnPropertyChanged(nameof(BoltSize1));				}			}				private string _BoltSize2;		public string BoltSize2			{				get { return _BoltSize2; }				set				{					_BoltSize2 = value;					OnPropertyChanged(nameof(BoltSize2));				}			}				private string _BoltLengthWNBF;		public string BoltLengthWNBF			{				get { return _BoltLengthWNBF; }				set				{					_BoltLengthWNBF = value;					OnPropertyChanged(nameof(BoltLengthWNBF));				}			}				private string _BoltLengthWNWN;		public string BoltLengthWNWN			{				get { return _BoltLengthWNWN; }				set				{					_BoltLengthWNWN = value;					OnPropertyChanged(nameof(BoltLengthWNWN));				}			}			}}