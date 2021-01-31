using PaperSetting.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaperSetting.Models
{
    public class PaperTableModel:Notifier,ICloneable
    {
        public PaperTableModel()
        {
            No = "";
            Name = "";
            TableSelection = "";
            TableList = new List<string[]>();
            Location = new PointModel();
            Size = new SizeModel();
            Dock = new DockModel();
        }

        public object Clone()
        {
            PaperTableModel newModel = new PaperTableModel();
            newModel.No = No;
            newModel.Name = Name;
            newModel.TableSelection = TableSelection;
            newModel.TableList = TableList;
            newModel.Location = Location;
            newModel.Size = Size;
            newModel.Dock = Dock;

            return newModel;
        }


        private string _No;
        public string No
        {
            get { return _No; }
            set
            {
                _No = value;
                OnPropertyChanged(nameof(No));
            }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _TableSelection;
        public string TableSelection
        {
            get { return _TableSelection; }
            set
            {
                _TableSelection = value;
                OnPropertyChanged(nameof(TableSelection));
            }
        }

        private List<string[]> _TableList;
        public List<string[]> TableList
        {
            get { return _TableList; }
            set
            {
                _TableList = value;
                OnPropertyChanged(nameof(TableList));
            }
        }

        private PointModel _Location;
        public PointModel Location
        {
            get { return _Location; }
            set
            {
                _Location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        private SizeModel _Size;
        public SizeModel Size
        {
            get { return _Size; }
            set
            {
                _Size = value;
                OnPropertyChanged(nameof(Size));
            }
        }

        private DockModel _Dock;
        public DockModel Dock
        {
            get { return _Dock; }
            set
            {
                _Dock = value;
                OnPropertyChanged(nameof(Dock));
            }
        }
    }
}
