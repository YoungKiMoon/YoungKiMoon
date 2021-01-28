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
            SheetSize = new SizeModel();
            Basic = new PaperBasicModel();
            Revisions = new ObservableCollection<PaperRevisionModel>();
            Tables = new ObservableCollection<PaperTableModel>();
            Notes = new ObservableCollection<PaperNoteModel>();
            ViewPorts = new ObservableCollection<PaperViewportModel>();
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
            return newModel;
        }

        private SizeModel _SheetSize;
        public SizeModel SheetSize
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


    }
}
