using devDept.Eyeshot;
using DrawSettingLib.Commons;
using PaperSetting.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PaperSetting.Models
{
    public class PaperDwgModel : Notifier, ICloneable
    {
        public PaperDwgModel()
        {

            Name = PAPERMAIN_TYPE.NotSet;
            Page = 1;
            ColumnDef = 1;
            RowDef = 1;

            SheetSize = new PaperSizeModel();
            Basic = new PaperBasicModel();
            Revisions = new ObservableCollection<PaperRevisionModel>();
            Tables = new ObservableCollection<PaperTableModel>();
            Notes = new ObservableCollection<PaperNoteModel>();
            ViewPorts = new ObservableCollection<PaperViewportModel>();


            IsFloating = false;

            // Ref
            //PaperSheet = null;
        }

        public object Clone()
        {
            PaperDwgModel newModel = new PaperDwgModel();
            newModel.SheetSize = SheetSize;
            newModel.Basic = Basic;
            newModel.Revisions = Revisions;
            newModel.Tables = Tables;
            newModel.Notes = Notes;
            newModel.ViewPorts = ViewPorts;
            
            newModel.ColumnDef = ColumnDef;
            newModel.RowDef = RowDef;

            newModel.IsFloating = IsFloating;


            newModel.Name = Name;

            //newModel.PaperSheet= PaperSheet;
            return newModel;
        }

        //private Sheet _PaperSheet;
        //public Sheet PaperSheet
        //{
        //    get { return _PaperSheet; }
        //    set
        //    {
        //        _PaperSheet = value;
        //        OnPropertyChanged(nameof(PaperSheet));
        //    }
        //}

        

        private PAPERMAIN_TYPE _Name;
        public PAPERMAIN_TYPE Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private double _Page;
        public double Page
        {
            get { return _Page; }
            set
            {
                _Page = value;
                OnPropertyChanged(nameof(Page));
            }
        }


        private PaperSizeModel _SheetSize;
        public PaperSizeModel SheetSize
        {
            get { return _SheetSize; }
            set
            {
                _SheetSize = value;
                OnPropertyChanged(nameof(SheetSize));
            }
        }

        private PaperBasicModel _Basic;
        public PaperBasicModel Basic
        {
            get { return _Basic; }
            set
            {
                _Basic = value;
                OnPropertyChanged(nameof(Basic));
            }
        }

        private ObservableCollection<PaperRevisionModel> _Revisions;
        public ObservableCollection<PaperRevisionModel> Revisions
        {
            get { return _Revisions; }
            set
            {
                _Revisions = value;
                OnPropertyChanged(nameof(Revisions));
            }
        }

        private ObservableCollection<PaperTableModel> _Tables;
        public ObservableCollection<PaperTableModel> Tables
        {
            get { return _Tables; }
            set
            {
                _Tables = value;
                OnPropertyChanged(nameof(Tables));
            }
        }

        private ObservableCollection<PaperNoteModel> _Notes;
        public ObservableCollection<PaperNoteModel> Notes
        {
            get { return _Notes; }
            set
            {
                _Notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }

        private ObservableCollection<PaperViewportModel> _ViewPorts;
        public ObservableCollection<PaperViewportModel> ViewPorts
        {
            get { return _ViewPorts; }
            set
            {
                _ViewPorts = value;
                OnPropertyChanged(nameof(ViewPorts));
            }
        }

        private double _ColumnDef;
        public double ColumnDef
        {
            get { return _ColumnDef; }
            set
            {
                _ColumnDef = value;
                OnPropertyChanged(nameof(ColumnDef));
            }
        }

        private double _RowDef;
        public double RowDef
        {
            get { return _RowDef; }
            set
            {
                _RowDef = value;
                OnPropertyChanged(nameof(RowDef));
            }
        }

        private bool _IsFloating;
        public bool IsFloating
        {
            get { return _IsFloating; }
            set
            {
                _IsFloating = value;
                OnPropertyChanged(nameof(IsFloating));
            }
        }

    }
}
