using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using DrawShapeLib.DrawModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DrawShapeLib.DrawStyleServices
{
    public class EnvironmentStyleService
    {
        private Model singleModel;

        public EnvironmentStyleService(Model selModel)
        {
            singleModel = selModel;
        }

        public ObservableCollection<T> GetObservableCollection<T>(List<T> selList)
        {
            ObservableCollection<T> collection = new ObservableCollection<T>(selList);
            return collection;
        }


        public List<CustomLayerModel> GetLayerModel()
        {
            List<CustomLayerModel> newList = new List<CustomLayerModel>();

            if (singleModel != null)
            {
                foreach (Layer eachLayer in singleModel.Layers)
                {
                    CustomLayerModel newLayer = new CustomLayerModel();
                    newLayer.Name = eachLayer.Name;
                    newLayer.LayerColor = eachLayer.Color.ToString();
                    newLayer.LineTypeName = eachLayer.LineTypeName;
                    newLayer.LineWeight = eachLayer.LineWeight.ToString();
                    newList.Add(newLayer);
                }
            }

            return newList;
        }
        public ObservableCollection<CustomLayerModel> GetLayerModelObservableCollection()
        {
            List<CustomLayerModel> newList = GetLayerModel();

            return GetObservableCollection<CustomLayerModel>(newList);
        }

        public List<CustomLineModel> GetLineModel()
        {
            List<CustomLineModel> newList = new List<CustomLineModel>();

            if (singleModel != null)
            {
                foreach (LinePattern eachLine in singleModel.LineTypes)
                {
                    CustomLineModel newLine = new CustomLineModel();
                    newLine.Name = eachLine.Name;
                    newLine.Description = eachLine.Description;
                    if (eachLine.Pattern != null)
                    {
                        newLine.Pattern = string.Join(",", eachLine.Pattern);
                    }
                    newLine.Length = eachLine.Length.ToString();
                    newList.Add(newLine);
                }
            }

            return newList;
        }
        public ObservableCollection<CustomLineModel> GetLineModelObservableCollection()
        {
            List<CustomLineModel> newList = GetLineModel();

            return GetObservableCollection<CustomLineModel>(newList);
        }

        public List<CustomBlockModel> GetBlockModel()
        {
            List<CustomBlockModel> newList = new List<CustomBlockModel>();

            if (singleModel != null)
            {
                List<string> useBlockList = GetBlockReference();

                foreach (Block eachBlock in singleModel.Blocks)
                {
                    CustomBlockModel newBlock = new CustomBlockModel();
                    newBlock.Name = eachBlock.Name;
                    //newBlock.Unit = eachBlock.Units.GetDisplayName();
                    // 2021 버젼 수정
                    newBlock.Unit = eachBlock.Units.ToString();

                    newBlock.BasePoint = eachBlock.BasePoint.ToString();
                    newBlock.InUse = useBlockList.Contains(eachBlock.Name).ToString();
                    newList.Add(newBlock);
                }
            }

            return newList;
        }
        public ObservableCollection<CustomBlockModel> GetBlockModelObservableCollection()
        {
            List<CustomBlockModel> newList = GetBlockModel();

            return GetObservableCollection<CustomBlockModel>(newList);
        }
        private List<string> GetBlockReference()
        {
            List<string> newList = new List<string>();
            foreach (Entity eachEntity in singleModel.Entities)
            {
                if (eachEntity is BlockReference)
                {
                    BlockReference newEntity = eachEntity as BlockReference;
                    if (!newList.Contains(newEntity.BlockName))
                        newList.Add(newEntity.BlockName);
                }
            }
            return newList;
        }


        public List<CustomTextModel> GetTextModel()
        {
            List<CustomTextModel> newList = new List<CustomTextModel>();

            if (singleModel != null)
            {
                foreach (TextStyle eachText in singleModel.TextStyles)
                {
                    CustomTextModel newText = new CustomTextModel();
                    newText.Name = eachText.Name;
                    newText.FontFamily = eachText.FontFamilyName;
                    newText.WidthFactor = eachText.WidthFactor.ToString();
                    newText.FileName = eachText.FileName;
                    newList.Add(newText);
                }
            }

            return newList;
        }
        public ObservableCollection<CustomTextModel> GetTextModelObservableCollection()
        {
            List<CustomTextModel> newList = GetTextModel();

            return GetObservableCollection<CustomTextModel>(newList);
        }

        public List<CustomEntityModel> GetEntityModel()
        {
            List<CustomEntityModel> newList = new List<CustomEntityModel>();

            if (singleModel != null)
            {
                foreach (Entity eachEntity in singleModel.Entities)
                {
                    CustomEntityModel newEntity = new CustomEntityModel();
                    newEntity.Type = eachEntity.GetType().Name.ToString(); ;
                    newList.Add(newEntity);
                }
            }

            return newList;
        }
        public ObservableCollection<CustomEntityModel> GetEntityModelObservableCollection()
        {
            List<CustomEntityModel> newList = GetEntityModel();

            return GetObservableCollection<CustomEntityModel>(newList);
        }
    }
}
