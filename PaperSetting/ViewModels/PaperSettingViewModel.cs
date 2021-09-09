using PaperSetting.Models;
using PaperSetting.Utils;
using PaperSetting.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AssemblyLib.AssemblyModels;
using DrawWork.DrawGridServices;

namespace PaperSetting.ViewModels
{
    public class PaperSettingViewModel : Notifier
    {
        public AssemblyModel newTankData { get; set; }


        public ObservableCollection<PaperDwgModel> PaperList
        {
            get { return _PaperList; }
            set
            {
                _PaperList = value;
                OnPropertyChanged(nameof(PaperList));
            }
        }
        private ObservableCollection<PaperDwgModel> _PaperList;

        public ObservableCollection<PaperDwgModel> PaperListSelectionColl
        {
            get { return _PaperListSelectionColl; }
            set
            {
                _PaperListSelectionColl = value;
                OnPropertyChanged(nameof(PaperListSelectionColl));
            }
        }
        private ObservableCollection<PaperDwgModel> _PaperListSelectionColl;

        public PaperDwgModel PaperListSelection
        {
            get { return _PaperListSelection; }
            set
            {
                _PaperListSelection = value;
                OnPropertyChanged(nameof(PaperListSelection));
                PaperListSelectionColl.Clear();
                if(value!=null)
                    PaperListSelectionColl.Add(value);
            }
        }
        private PaperDwgModel _PaperListSelection;

        // Grid
        public ObservableCollection<DrawPaperGridModel> PaperGridList
        {
            get { return _PaperGridList; }
            set
            {
                _PaperGridList = value;
                OnPropertyChanged(nameof(PaperGridList));
            }
        }
        private ObservableCollection<DrawPaperGridModel> _PaperGridList;

        public bool DWGGridVisible
        {
            get { return _DWGGridVisible; }
            set
            {
                _DWGGridVisible = value;
                OnPropertyChanged(nameof(DWGGridVisible));
            }
        }
        private bool _DWGGridVisible;


        #region Progress
        private string _bContents;
        public string BContents
        {
            get { return _bContents; }
            set
            {
                _bContents = value;
                OnPropertyChanged(nameof(BContents));
            }
        }
        #endregion


        public PaperSettingViewModel()
        {
            PaperListSelectionColl = new ObservableCollection<PaperDwgModel>();
            PaperListSelection = new PaperDwgModel();

            PaperList = new ObservableCollection<PaperDwgModel>();
            PaperGridList = new ObservableCollection<DrawPaperGridModel>();

            DWGGridVisible = true;
            //PaperList = SampleService.CreatePaperSample();


        }

        //public void CreateDrawingList(AssemblyModel selAssembly)
        //{
        //    PaperSettingService newSetting = new PaperSettingService();
        //    PaperList = newSetting.CreateDrawingCRTList(selAssembly);
        //    //PaperList = newSetting.CreateDrawingCRTList();
        //}


    }
}
