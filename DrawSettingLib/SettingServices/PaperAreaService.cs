
using DrawSettingLib.Commons;
using DrawSettingLib.SettingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingServices
{
    public class PaperAreaService
    {

        public PaperAreaService()
        {
        }

        public List<PaperAreaModel> GetPaperAreaData()
        {
            List<PaperAreaModel> newAreaList = new List<PaperAreaModel>();

            PaperAreaModel GAView = new PaperAreaModel();
            GAView.Name = PAPERMAIN_TYPE.GA;
            GAView.Location.X = 334;
            GAView.Location.Y = 363;
            GAView.Size.X = 600;
            GAView.Size.Y = 400;
            newAreaList.Add(GAView);

            PaperAreaModel Orientation = new PaperAreaModel();
            Orientation.Name = PAPERMAIN_TYPE.ORIENTATION;
            Orientation.Location.X = 280;
            Orientation.Location.Y = 297;
            Orientation.Size.X = 500;
            Orientation.Size.Y = 570;
            newAreaList.Add(Orientation);

            PaperAreaModel Detail01 = new PaperAreaModel();
            Detail01.Name = PAPERMAIN_TYPE.DETAIL;
            Detail01.SubName = PAPERSUB_TYPE.HORIZONTALJOINT;
            Detail01.Location.X = 280;
            Detail01.Location.Y = 297;
            Detail01.Size.X = 500;
            Detail01.Size.Y = 570;
            newAreaList.Add(Detail01);

            PaperAreaModel Detail02 = new PaperAreaModel();
            Detail02.Name = PAPERMAIN_TYPE.DETAIL;
            Detail02.SubName = PAPERSUB_TYPE.ONECOURSESHELLPLATE;
            Detail02.Location.X = 280;
            Detail02.Location.Y = 297;
            Detail02.Size.X = 22 + 598 + 22;
            Detail02.Size.Y = 185 + 185 + 185;
            newAreaList.Add(Detail02);


            // Jang : Dev
            PaperAreaModel DetailJang01 = new PaperAreaModel();
            DetailJang01.Name = PAPERMAIN_TYPE.DETAIL;
            DetailJang01.SubName = PAPERSUB_TYPE.BOTTOMPLATEJOINT;
            DetailJang01.Location.X = 280;
            DetailJang01.Location.Y = 297;
            DetailJang01.Size.X = 22 + 598 + 22;
            DetailJang01.Size.Y = 185 + 185 + 185;
            newAreaList.Add(DetailJang01);



            return newAreaList;
        }




        public PaperAreaModel GetPaperAreaModel(PAPERMAIN_TYPE mainName, PAPERSUB_TYPE subName, List<PaperAreaModel> selList)
        {
            PaperAreaModel returnModel = new PaperAreaModel();


            // Paper
            foreach (PaperAreaModel eachArea in selList)
            {
                if (eachArea.Name == mainName)
                    if (eachArea.SubName == subName)
                    {
                        returnModel = eachArea;
                        break;
                    }
            }

            return returnModel;
        }

        public double GetPaperScaleValue(PAPERMAIN_TYPE mainName, PAPERSUB_TYPE subName, List<PaperAreaModel> selList)
        {
            double returnScale = 1;


            // Paper
            foreach (PaperAreaModel eachArea in selList)
            {
                if (eachArea.Name == mainName)
                    if(eachArea.SubName== subName)
                    {
                        returnScale = eachArea.ScaleValue;
                        break;
                    }
            }

            return returnScale;
        }

        public PAPERMAIN_TYPE GetPaperMainType(string selName)
        {
            PAPERMAIN_TYPE returnValue = PAPERMAIN_TYPE.NotSet;
            switch (selName)
            {
                case "ga":
                    returnValue = PAPERMAIN_TYPE.GA;
                    break;
                case "orientation":
                    returnValue = PAPERMAIN_TYPE.ORIENTATION;
                    break;
                case "detail":
                    returnValue = PAPERMAIN_TYPE.DETAIL;
                    break;
            }
            return returnValue;
        }

        public PAPERSUB_TYPE GetPaperSubType(string selName)
        {
            PAPERSUB_TYPE returnValue = PAPERSUB_TYPE.NotSet;
            switch (selName)
            {
                case "horizontaljoint":
                    returnValue = PAPERSUB_TYPE.HORIZONTALJOINT;
                    break;

                case "onecourseshellplate":
                    returnValue = PAPERSUB_TYPE.ONECOURSESHELLPLATE;
                    break;

                case "bottomplatejoint":
                    returnValue = PAPERSUB_TYPE.BOTTOMPLATEJOINT;
                    break;

            }
            return returnValue;
        }
    }
}