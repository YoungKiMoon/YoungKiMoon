﻿using AssemblyLib.Utils;using System;using System.Collections.Generic;using System.Collections.ObjectModel;using System.Linq;using System.Text;using System.Threading.Tasks;namespace AssemblyLib.AssemblyModels{	public class NozzleInputModel : Notifier	{		public NozzleInputModel()		{					Position = "";			Mark = "";			Size = "";			Qty = "";			Rating = "";			Facing = "";			R = "";			H = "";			Ort = "";			Description = "";			Remarks = "";			NozzlePosition = "";		}				private string _Position;		public string Position			{				get { return _Position; }				set				{					_Position = value;					OnPropertyChanged(nameof(Position));				}			}				private string _Mark;		public string Mark			{				get { return _Mark; }				set				{					_Mark = value;					OnPropertyChanged(nameof(Mark));				}			}				private string _Size;		public string Size			{				get { return _Size; }				set				{					_Size = value;					OnPropertyChanged(nameof(Size));				}			}				private string _Qty;		public string Qty			{				get { return _Qty; }				set				{					_Qty = value;					OnPropertyChanged(nameof(Qty));				}			}				private string _Rating;		public string Rating			{				get { return _Rating; }				set				{					_Rating = value;					OnPropertyChanged(nameof(Rating));				}			}				private string _Facing;		public string Facing			{				get { return _Facing; }				set				{					_Facing = value;					OnPropertyChanged(nameof(Facing));				}			}				private string _R;		public string R			{				get { return _R; }				set				{					_R = value;					OnPropertyChanged(nameof(R));				}			}				private string _H;		public string H			{				get { return _H; }				set				{					_H = value;					OnPropertyChanged(nameof(H));				}			}				private string _Ort;		public string Ort			{				get { return _Ort; }				set				{					_Ort = value;					OnPropertyChanged(nameof(Ort));				}			}				private string _Description;		public string Description			{				get { return _Description; }				set				{					_Description = value;					OnPropertyChanged(nameof(Description));				}			}				private string _Remarks;		public string Remarks			{				get { return _Remarks; }				set				{					_Remarks = value;					OnPropertyChanged(nameof(Remarks));				}			}				private string _NozzlePosition;		public string NozzlePosition			{				get { return _NozzlePosition; }				set				{					_NozzlePosition = value;					OnPropertyChanged(nameof(NozzlePosition));				}			}			}}