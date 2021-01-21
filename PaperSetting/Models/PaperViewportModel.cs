using PaperSetting.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaperSetting.Models
{
    public class PaperViewportModel : Notifier, ICloneable
    {
        public PaperViewportModel()
        {
            No = "";
            Name = "";
            AssemblySelection = "";
            ViewPort = new ViewPortModel();
        }

        public object Clone()
        {
            PaperViewportModel newModel = new PaperViewportModel();
            newModel.No = No;
            newModel.Name = Name;
            newModel.AssemblySelection = AssemblySelection;
            newModel.ViewPort = ViewPort;

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

        private string _AssemblySelection;
        public string AssemblySelection
        {
            get { return _AssemblySelection; }
            set
            {
                _AssemblySelection = value;
                OnPropertyChanged(nameof(AssemblySelection));
            }
        }

        private ViewPortModel _ViewPort;
        public ViewPortModel ViewPort
        {
            get { return _ViewPort; }
            set
            {
                _ViewPort = value;
                OnPropertyChanged(nameof(ViewPort));
            }
        }

        
    }
}
