using PaperSetting.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaperSetting.Models
{
    public class PaperNoteModel:Notifier,ICloneable
    {
        public PaperNoteModel()
        {
            No = "";
            Name = "";
            Note = "";
            Location = new PointModel();
            Size = new SizeModel();
        }

        public object Clone()
        {
            PaperNoteModel newModel = new PaperNoteModel();
            newModel.No = No;
            newModel.Name = Name;
            newModel.Note = Note;
            newModel.Location = Location;
            newModel.Size = Size;
            
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

        private string _Note;
        public string Note
        {
            get { return _Note; }
            set
            {
                _Note = value;
                OnPropertyChanged(nameof(Note));
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
    }
}
