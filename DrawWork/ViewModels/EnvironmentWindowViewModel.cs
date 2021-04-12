using DrawWork.Models;
using DrawWork.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using devDept.Eyeshot;
using DrawWork.DrawStyleServices;

namespace DrawWork.ViewModels
{
    public class EnvironmentWindowViewModel : Notifier
    {
        public Model singleModel;
        public EnvironmentStyleService envService;

        public ObservableCollection<CustomLayerModel> CustomLayer
        {
            get { return _CustomLayer; }
            set
            {
                _CustomLayer = value;
                OnPropertyChanged(nameof(CustomLayer));
            }
        }
        private ObservableCollection<CustomLayerModel> _CustomLayer;

        public ObservableCollection<CustomLineModel> CustomLine
        {
            get { return _CustomLine; }
            set
            {
                _CustomLine = value;
                OnPropertyChanged(nameof(CustomLine));
            }
        }
        private ObservableCollection<CustomLineModel> _CustomLine;

        public ObservableCollection<CustomBlockModel> CustomBlock
        {
            get { return _CustomBlock; }
            set
            {
                _CustomBlock = value;
                OnPropertyChanged(nameof(CustomBlock));
            }
        }
        private ObservableCollection<CustomBlockModel> _CustomBlock;

        public ObservableCollection<CustomTextModel> CustomText
        {
            get { return _CustomText; }
            set
            {
                _CustomText = value;
                OnPropertyChanged(nameof(CustomText));
            }
        }
        private ObservableCollection<CustomTextModel> _CustomText;

        public ObservableCollection<CustomEntityModel> CustomEntity
        {
            get { return _CustomEntity; }
            set
            {
                _CustomEntity = value;
                OnPropertyChanged(nameof(CustomEntity));
            }
        }
        private ObservableCollection<CustomEntityModel> _CustomEntity;


        public EnvironmentWindowViewModel()
        {
            CustomLayer = new ObservableCollection<CustomLayerModel>();
            CustomLine = new ObservableCollection<CustomLineModel>();
            CustomBlock = new ObservableCollection<CustomBlockModel>();
            CustomText = new ObservableCollection<CustomTextModel>();
            CustomEntity = new ObservableCollection<CustomEntityModel>();
        }

        public void SetModelEnvironment(Model selModel)
        {
            singleModel = selModel;
            envService = new EnvironmentStyleService(selModel);
        }
        public void CreateEnvironment()
        {
            CustomLayer = envService.GetLayerModelObservableCollection();
            CustomLine = envService.GetLineModelObservableCollection();
            CustomBlock = envService.GetBlockModelObservableCollection();
            CustomText = envService.GetTextModelObservableCollection();
            CustomEntity = envService.GetEntityModelObservableCollection();

        }
    }
}
