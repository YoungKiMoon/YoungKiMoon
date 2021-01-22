using PaperSetting.Models;
using PaperSetting.Utils;
using PaperSetting.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PaperSetting.ViewModel
{
    public class PaperSettingViewModel : Notifier
    {
        private ObservableCollection<PaperDwgModel> _PaperList;
        public ObservableCollection<PaperDwgModel> PaperList
        {
            get { return _PaperList; }
            set
            {
                _PaperList = value;
                OnPropertyChanged(nameof(PaperList));
            }
        }

        private ObservableCollection<PaperDwgModel> _PaperListSelectionColl;
        public ObservableCollection<PaperDwgModel> PaperListSelectionColl
        {
            get { return _PaperListSelectionColl; }
            set
            {
                _PaperListSelectionColl = value;
                OnPropertyChanged(nameof(PaperListSelectionColl));
            }
        }

        private PaperDwgModel _PaperListSelection;
        public PaperDwgModel PaperListSelection
        {
            get { return _PaperListSelection; }
            set
            {
                _PaperListSelection = value;
                OnPropertyChanged(nameof(PaperListSelection));
                PaperListSelectionColl.Clear();
                PaperListSelectionColl.Add(value);
            }
        }


        public PaperSettingViewModel()
        {
            PaperListSelectionColl = new ObservableCollection<PaperDwgModel>();
            PaperListSelection = new PaperDwgModel();

            PaperList = SampleService.CreatePaperSample();
        }

    }
}
