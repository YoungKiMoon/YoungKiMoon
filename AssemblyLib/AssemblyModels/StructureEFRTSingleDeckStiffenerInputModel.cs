﻿using AssemblyLib.Utils;using System;using System.Collections.Generic;using System.Collections.ObjectModel;using System.Linq;using System.Text;using System.Threading.Tasks;namespace AssemblyLib.AssemblyModels{	public class StructureEFRTSingleDeckStiffenerInputModel : Notifier	{		public StructureEFRTSingleDeckStiffenerInputModel()		{					Material = "";			UpperOuterRimStiffenerType = "";			UpperOuterRimStiffenerSize = "";			UpperMiddle01Type = "";			UpperMiddle01Size = "";			UpperMiddle02Type = "";			UpperMiddle02Size = "";			UpperPontoonRafter01Type = "";			UpperPontoonRafter01Size = "";			UpperPontoonRafter02Type = "";			UpperPontoonRafter02Size = "";			UpperPontoonRafter03Type = "";			UpperPontoonRafter03Size = "";			MiddleOuterRimStiffenerType = "";			MiddleOuterRimStiffenerSize = "";			MiddleTruss01Type = "";			MiddleTruss01Size = "";			MiddleTruss02Type = "";			MiddleTruss02Size = "";			MiddleInnerRimStiffenerType = "";			MiddleInnerRimStiffenerSize = "";			MiddleInnerMiddleRimStiffenerType = "";			MiddleInnerMiddleRimStiffenerSize = "";			MiddleBulkHeadStiffenerType = "";			MiddleBulkHeadStiffenerSize = "";			LowerStiffener01Type = "";			LowerStiffener01Size = "";			LowerStiffener0101Type = "";			LowerStiffener0101Size = "";			LowerStiffener02Type = "";			LowerStiffener02Size = "";			LowerStiffener0201Type = "";			LowerStiffener0201Size = "";		}				private string _Material;		public string Material			{				get { return _Material; }				set				{					_Material = value;					OnPropertyChanged(nameof(Material));				}			}				private string _UpperOuterRimStiffenerType;		public string UpperOuterRimStiffenerType			{				get { return _UpperOuterRimStiffenerType; }				set				{					_UpperOuterRimStiffenerType = value;					OnPropertyChanged(nameof(UpperOuterRimStiffenerType));				}			}				private string _UpperOuterRimStiffenerSize;		public string UpperOuterRimStiffenerSize			{				get { return _UpperOuterRimStiffenerSize; }				set				{					_UpperOuterRimStiffenerSize = value;					OnPropertyChanged(nameof(UpperOuterRimStiffenerSize));				}			}				private string _UpperMiddle01Type;		public string UpperMiddle01Type			{				get { return _UpperMiddle01Type; }				set				{					_UpperMiddle01Type = value;					OnPropertyChanged(nameof(UpperMiddle01Type));				}			}				private string _UpperMiddle01Size;		public string UpperMiddle01Size			{				get { return _UpperMiddle01Size; }				set				{					_UpperMiddle01Size = value;					OnPropertyChanged(nameof(UpperMiddle01Size));				}			}				private string _UpperMiddle02Type;		public string UpperMiddle02Type			{				get { return _UpperMiddle02Type; }				set				{					_UpperMiddle02Type = value;					OnPropertyChanged(nameof(UpperMiddle02Type));				}			}				private string _UpperMiddle02Size;		public string UpperMiddle02Size			{				get { return _UpperMiddle02Size; }				set				{					_UpperMiddle02Size = value;					OnPropertyChanged(nameof(UpperMiddle02Size));				}			}				private string _UpperPontoonRafter01Type;		public string UpperPontoonRafter01Type			{				get { return _UpperPontoonRafter01Type; }				set				{					_UpperPontoonRafter01Type = value;					OnPropertyChanged(nameof(UpperPontoonRafter01Type));				}			}				private string _UpperPontoonRafter01Size;		public string UpperPontoonRafter01Size			{				get { return _UpperPontoonRafter01Size; }				set				{					_UpperPontoonRafter01Size = value;					OnPropertyChanged(nameof(UpperPontoonRafter01Size));				}			}				private string _UpperPontoonRafter02Type;		public string UpperPontoonRafter02Type			{				get { return _UpperPontoonRafter02Type; }				set				{					_UpperPontoonRafter02Type = value;					OnPropertyChanged(nameof(UpperPontoonRafter02Type));				}			}				private string _UpperPontoonRafter02Size;		public string UpperPontoonRafter02Size			{				get { return _UpperPontoonRafter02Size; }				set				{					_UpperPontoonRafter02Size = value;					OnPropertyChanged(nameof(UpperPontoonRafter02Size));				}			}				private string _UpperPontoonRafter03Type;		public string UpperPontoonRafter03Type			{				get { return _UpperPontoonRafter03Type; }				set				{					_UpperPontoonRafter03Type = value;					OnPropertyChanged(nameof(UpperPontoonRafter03Type));				}			}				private string _UpperPontoonRafter03Size;		public string UpperPontoonRafter03Size			{				get { return _UpperPontoonRafter03Size; }				set				{					_UpperPontoonRafter03Size = value;					OnPropertyChanged(nameof(UpperPontoonRafter03Size));				}			}				private string _MiddleOuterRimStiffenerType;		public string MiddleOuterRimStiffenerType			{				get { return _MiddleOuterRimStiffenerType; }				set				{					_MiddleOuterRimStiffenerType = value;					OnPropertyChanged(nameof(MiddleOuterRimStiffenerType));				}			}				private string _MiddleOuterRimStiffenerSize;		public string MiddleOuterRimStiffenerSize			{				get { return _MiddleOuterRimStiffenerSize; }				set				{					_MiddleOuterRimStiffenerSize = value;					OnPropertyChanged(nameof(MiddleOuterRimStiffenerSize));				}			}				private string _MiddleTruss01Type;		public string MiddleTruss01Type			{				get { return _MiddleTruss01Type; }				set				{					_MiddleTruss01Type = value;					OnPropertyChanged(nameof(MiddleTruss01Type));				}			}				private string _MiddleTruss01Size;		public string MiddleTruss01Size			{				get { return _MiddleTruss01Size; }				set				{					_MiddleTruss01Size = value;					OnPropertyChanged(nameof(MiddleTruss01Size));				}			}				private string _MiddleTruss02Type;		public string MiddleTruss02Type			{				get { return _MiddleTruss02Type; }				set				{					_MiddleTruss02Type = value;					OnPropertyChanged(nameof(MiddleTruss02Type));				}			}				private string _MiddleTruss02Size;		public string MiddleTruss02Size			{				get { return _MiddleTruss02Size; }				set				{					_MiddleTruss02Size = value;					OnPropertyChanged(nameof(MiddleTruss02Size));				}			}				private string _MiddleInnerRimStiffenerType;		public string MiddleInnerRimStiffenerType			{				get { return _MiddleInnerRimStiffenerType; }				set				{					_MiddleInnerRimStiffenerType = value;					OnPropertyChanged(nameof(MiddleInnerRimStiffenerType));				}			}				private string _MiddleInnerRimStiffenerSize;		public string MiddleInnerRimStiffenerSize			{				get { return _MiddleInnerRimStiffenerSize; }				set				{					_MiddleInnerRimStiffenerSize = value;					OnPropertyChanged(nameof(MiddleInnerRimStiffenerSize));				}			}				private string _MiddleInnerMiddleRimStiffenerType;		public string MiddleInnerMiddleRimStiffenerType			{				get { return _MiddleInnerMiddleRimStiffenerType; }				set				{					_MiddleInnerMiddleRimStiffenerType = value;					OnPropertyChanged(nameof(MiddleInnerMiddleRimStiffenerType));				}			}				private string _MiddleInnerMiddleRimStiffenerSize;		public string MiddleInnerMiddleRimStiffenerSize			{				get { return _MiddleInnerMiddleRimStiffenerSize; }				set				{					_MiddleInnerMiddleRimStiffenerSize = value;					OnPropertyChanged(nameof(MiddleInnerMiddleRimStiffenerSize));				}			}				private string _MiddleBulkHeadStiffenerType;		public string MiddleBulkHeadStiffenerType			{				get { return _MiddleBulkHeadStiffenerType; }				set				{					_MiddleBulkHeadStiffenerType = value;					OnPropertyChanged(nameof(MiddleBulkHeadStiffenerType));				}			}				private string _MiddleBulkHeadStiffenerSize;		public string MiddleBulkHeadStiffenerSize			{				get { return _MiddleBulkHeadStiffenerSize; }				set				{					_MiddleBulkHeadStiffenerSize = value;					OnPropertyChanged(nameof(MiddleBulkHeadStiffenerSize));				}			}				private string _LowerStiffener01Type;		public string LowerStiffener01Type			{				get { return _LowerStiffener01Type; }				set				{					_LowerStiffener01Type = value;					OnPropertyChanged(nameof(LowerStiffener01Type));				}			}				private string _LowerStiffener01Size;		public string LowerStiffener01Size			{				get { return _LowerStiffener01Size; }				set				{					_LowerStiffener01Size = value;					OnPropertyChanged(nameof(LowerStiffener01Size));				}			}				private string _LowerStiffener0101Type;		public string LowerStiffener0101Type			{				get { return _LowerStiffener0101Type; }				set				{					_LowerStiffener0101Type = value;					OnPropertyChanged(nameof(LowerStiffener0101Type));				}			}				private string _LowerStiffener0101Size;		public string LowerStiffener0101Size			{				get { return _LowerStiffener0101Size; }				set				{					_LowerStiffener0101Size = value;					OnPropertyChanged(nameof(LowerStiffener0101Size));				}			}				private string _LowerStiffener02Type;		public string LowerStiffener02Type			{				get { return _LowerStiffener02Type; }				set				{					_LowerStiffener02Type = value;					OnPropertyChanged(nameof(LowerStiffener02Type));				}			}				private string _LowerStiffener02Size;		public string LowerStiffener02Size			{				get { return _LowerStiffener02Size; }				set				{					_LowerStiffener02Size = value;					OnPropertyChanged(nameof(LowerStiffener02Size));				}			}				private string _LowerStiffener0201Type;		public string LowerStiffener0201Type			{				get { return _LowerStiffener0201Type; }				set				{					_LowerStiffener0201Type = value;					OnPropertyChanged(nameof(LowerStiffener0201Type));				}			}				private string _LowerStiffener0201Size;		public string LowerStiffener0201Size			{				get { return _LowerStiffener0201Size; }				set				{					_LowerStiffener0201Size = value;					OnPropertyChanged(nameof(LowerStiffener0201Size));				}			}			}}