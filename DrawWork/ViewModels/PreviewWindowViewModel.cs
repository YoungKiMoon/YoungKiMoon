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
            viewPortSet.TargetX = "16000";
            viewPortSet.TargetY = "13000";
            viewPortSet.LocationX = "300";
            viewPortSet.LocationY = "400";
            viewPortSet.SizeX = "500";
            viewPortSet.SizeY = "300";
        }
    }
}
