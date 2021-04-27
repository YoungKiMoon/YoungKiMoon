using DrawWork.DrawPaperServices;
using DrawWork.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.ViewModels
{
    public class PreviewWindowViewModel : Notifier
    {
        public PaperPreviewService previewService;

        public ViewPortSettingModel viewPortSet
        {
            get { return _viewPortSet; }
            set
            {
                _viewPortSet = value;
                OnPropertyChanged(nameof(viewPortSet));
            }
        }
        private ViewPortSettingModel _viewPortSet;

        public PreviewWindowViewModel()
        {
            previewService = new PaperPreviewService();
            viewPortSet = new ViewPortSettingModel();

            viewPortSet.Scale = "130";
            viewPortSet.TargetX = "17000";
            viewPortSet.TargetY = "14000";
            viewPortSet.LocationX = "330";
            viewPortSet.LocationY = "360";
            viewPortSet.SizeX = "600";
            viewPortSet.SizeY = "400";
        }
    }
}
