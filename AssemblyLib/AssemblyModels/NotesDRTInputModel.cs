﻿using AssemblyLib.Utils;using System;using System.Collections.Generic;using System.Collections.ObjectModel;using System.Linq;using System.Text;using System.Threading.Tasks;namespace AssemblyLib.AssemblyModels{	public class NotesDRTInputModel : Notifier	{		public NotesDRTInputModel()		{					PaperName = "";			NoteName = "";			Description = "";			Name = "";			CellAddress = "";		}				private string _PaperName;		public string PaperName			{				get { return _PaperName; }				set				{					_PaperName = value;					OnPropertyChanged(nameof(PaperName));				}			}				private string _NoteName;		public string NoteName			{				get { return _NoteName; }				set				{					_NoteName = value;					OnPropertyChanged(nameof(NoteName));				}			}				private string _Description;		public string Description			{				get { return _Description; }				set				{					_Description = value;					OnPropertyChanged(nameof(Description));				}			}				private string _Name;		public string Name			{				get { return _Name; }				set				{					_Name = value;					OnPropertyChanged(nameof(Name));				}			}				private string _CellAddress;		public string CellAddress			{				get { return _CellAddress; }				set				{					_CellAddress = value;					OnPropertyChanged(nameof(CellAddress));				}			}			}}