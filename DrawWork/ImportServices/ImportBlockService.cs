using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using devDept.Eyeshot;
using devDept.Graphics;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Serialization;
using MColor = System.Windows.Media.Color;

//using Color = System.Drawing.Color;

namespace DrawWork.ImportServices
{
    public class ImportBlockService
    {

        private string blockLayerName;

        public ImportBlockService()
        {
            blockLayerName = "WP";
        }
        

        public void CreateBlock(ReadFileAsyncWithBlocks selFileData,Model selModel)
        {

            List<LinearPath> boxList = new List<LinearPath>();
            List<Text> textList = new List<Text>();
            List<MultilineText> multitextList = new List<MultilineText>();
            List<Point3D> blockBasePointList = new List<Point3D>();

            List<Entity> blockSpareList = new List<Entity>();

            foreach (Entity eachEntity in selFileData.Entities)
            {
                if (eachEntity.LayerName == blockLayerName)
                {
                    if (eachEntity is LinearPath)
                    {
                        LinearPath tempLinearPath = eachEntity as LinearPath;
                        if (tempLinearPath.IsClosed) // Close
                        {
                            boxList.Add(tempLinearPath);
                        }
                    }
                    else if (eachEntity is Text)
                    {
                        textList.Add(eachEntity as Text);
                    }
                    else if (eachEntity is MultilineText)
                    {
                        multitextList.Add(eachEntity as MultilineText);
                    }
                    else if (eachEntity is Circle)
                    {
                        Circle tempBaseCircle = eachEntity as Circle;
                        blockBasePointList.Add(tempBaseCircle.Center);
                    }
                }
                else
                {
                    blockSpareList.Add(eachEntity);
                }

            }

            // Find Block Name
            List<ImportBlockModel> blockList = GetBlockList(boxList,textList,multitextList);

            // Find Base Point
            foreach (ImportBlockModel eachBlock in blockList)
            {
                Point2D lowerLeftPoint = GetBoxPointOfLowerLeft(eachBlock.BlockArea.Vertices);
                Point2D upperRightPoint = GetBoxPointOfUpperRight(eachBlock.BlockArea.Vertices);

                foreach (Point3D eachPoint in blockBasePointList)
                {
                    if (UtilityEx.PointInRect(eachPoint, lowerLeftPoint, upperRightPoint))
                    {
                        eachBlock.BlockBasePoint = eachPoint;
                        break;
                    }
                }

            }

            // Find Block Entity
            foreach (ImportBlockModel eachBlock in blockList)
            {
                Point2D lowerLeftPoint = GetBoxPointOfLowerLeft(eachBlock.BlockArea.Vertices);
                Point2D upperRightPoint = GetBoxPointOfUpperRight(eachBlock.BlockArea.Vertices);
                
                foreach (Entity eachEntity in blockSpareList)
                {
                    if(eachEntity is Text)
                    {
                        Text eachEntityText = eachEntity as Text;
                        if (UtilityEx.PointInRect(eachEntityText.InsertionPoint, lowerLeftPoint, upperRightPoint))
                        {
                            eachBlock.BlockEntities.Add(eachEntityText);
                        }
                    }
                    else if(eachEntity is Circle)
                    {
                        Circle eachEntityCircle = eachEntity as Circle;
                        if (UtilityEx.PointInRect(eachEntityCircle.StartPoint, lowerLeftPoint, upperRightPoint))
                        {
                            eachBlock.BlockEntities.Add(eachEntityCircle);
                        }
                    }
                    else if (eachEntity is Curve)
                    {
                        Curve eachEntityCurve = eachEntity as Curve;
                        if (UtilityEx.PointInRect(eachEntityCurve.StartPoint, lowerLeftPoint, upperRightPoint))
                        {
                            eachBlock.BlockEntities.Add(eachEntityCurve);
                        }
                    }
                    else if (eachEntity is CompositeCurve)
                    {
                        CompositeCurve eachEntityCom = eachEntity as CompositeCurve;
                        if (UtilityEx.PointInRect(eachEntityCom.StartPoint, lowerLeftPoint, upperRightPoint))
                        {
                            eachBlock.BlockEntities.Add(eachEntityCom);
                        }
                    }
                    else if (eachEntity is Ellipse)
                    {
                        Ellipse eachEntityEll = eachEntity as Ellipse;
                        if (UtilityEx.PointInRect(eachEntityEll.Center, lowerLeftPoint, upperRightPoint))
                        {
                            eachBlock.BlockEntities.Add(eachEntityEll);
                        }
                    }
                    else if (eachEntity is EllipticalArc)
                    {
                        EllipticalArc eachEntityEllArc = eachEntity as EllipticalArc;
                        if (UtilityEx.PointInRect(eachEntityEllArc.Center, lowerLeftPoint, upperRightPoint))
                        {
                            eachBlock.BlockEntities.Add(eachEntityEllArc);
                        }
                    }
                    else if (eachEntity is Text)
                    {
                        Text eachEntityText = eachEntity as Text;
                        if (UtilityEx.PointInRect(eachEntityText.Vertices[0], lowerLeftPoint, upperRightPoint))
                        {
                            eachEntityText.StyleName = "ROMANS";
                            eachBlock.BlockEntities.Add(eachEntityText);
                        }
                    }
                    else if ( eachEntity is MultilineText)
                    {
                        Text eachEntityMText = eachEntity as Text;
                        if (UtilityEx.PointInRect(eachEntityMText.Vertices[0], lowerLeftPoint, upperRightPoint))
                        {
                            eachEntityMText.StyleName = "ROMANS";
                            eachBlock.BlockEntities.Add(eachEntityMText);
                        }
                    }
                    else if (eachEntity is LinearPath)
                    {
                        if (UtilityEx.PointInRect(eachEntity.Vertices[0], lowerLeftPoint, upperRightPoint))
                        {
                            eachBlock.BlockEntities.Add(eachEntity);
                        }
                    }
                    else if(eachEntity is BlockReference)
                    {
                        continue;
                    }
                    else
                    {
                        if (UtilityEx.PointInRect(eachEntity.Vertices[0], lowerLeftPoint, upperRightPoint))
                        {
                            eachBlock.BlockEntities.Add(eachEntity);
                        }
                    }


                }

            }

            // Add Block, Set Base Point
            foreach(ImportBlockModel eachBlock in blockList)
            {
                if (eachBlock.BlockEntities.Count > 0)
                {
                    if(eachBlock.BlockName != "")
                    {
                        Block newBlock = GetBlock(eachBlock.BlockName, eachBlock.BlockEntities, eachBlock.BlockBasePoint);

                        selModel.Blocks.Add(newBlock);
                    }
                }
            }
        }

        private Block GetBlock(string selBlockName, List<Entity> selEntityList,Point3D selBasePoint)
        {
            Block newBlock = new Block(selBlockName);
            // Layer 지정이 필요
            foreach(Entity eachEntity in selEntityList)
            {
                eachEntity.LayerName = "LayerBlock";
                eachEntity.ColorMethod = colorMethodType.byLayer;
                newBlock.Entities.Add(eachEntity);
            }
            newBlock.BasePoint = selBasePoint;

            return newBlock;
        }

        private List<ImportBlockModel> GetBlockList(List<LinearPath> selBoxList, List<Text> selTextList, List<MultilineText> selMultiTextList)
        {
            //double blockNameHeight = 10;
            double blockNameHeight = 400;

            List<ImportBlockModel> newList = new List<ImportBlockModel>();
            List<LinearPath> nameList = new List<LinearPath>();

            // Area : One
            foreach (LinearPath eachBox in selBoxList)
            {
                int verticesCount = eachBox.Vertices.Length;
                if (verticesCount > 4)
                {
                    double calHeight = eachBox.Vertices[3].DistanceTo(eachBox.Vertices[4]);
                    calHeight = Math.Round(calHeight, 1, MidpointRounding.AwayFromZero);
                    if (calHeight == blockNameHeight)
                    {
                        nameList.Add(eachBox);
                    }
                    else
                    {
                        ImportBlockModel newBlock = new ImportBlockModel();
                        newBlock.BlockArea = eachBox;
                        newList.Add(newBlock);
                    }
                }

            }

            foreach (ImportBlockModel eachArea in newList)
            {
                List<LinearPath> eachAreaNameList = new List<LinearPath>();
                SetBlockNameList(ref eachAreaNameList, ref nameList, GetBoxPointValueOfLowerLeft(eachArea.BlockArea.Vertices));
                if (eachAreaNameList.Count > 0)
                {
                    eachArea.BlockNameList = eachAreaNameList;
                    eachArea.BlockName = GetBlockName(eachAreaNameList[0],ref selTextList,ref selMultiTextList);
                }
            }

            return newList;
        }

        private void SetBlockNameList(ref List<LinearPath> selAreaNameList, ref List<LinearPath> selNameList,double[] selPoint)
        {

            foreach (LinearPath eachName in selNameList)
            {
                double[] refNewPoint = GetBoxPointValueOfUpperLeft(eachName.Vertices);
                if (selPoint[0] == refNewPoint[0] && selPoint[1] == refNewPoint[1])
                {
                    selAreaNameList.Add(eachName);
                    SetBlockNameList(ref selAreaNameList, ref selNameList, GetBoxPointValueOfLowerLeft(eachName.Vertices));
                    break;
                }
            }
        }
        private string GetBlockName(LinearPath selBlockName, ref List<Text> selTextList, ref List<MultilineText> selMultiTextList)
        {
            string newName = "";
            Point2D lowerLeftPoint = GetBoxPointOfLowerLeft(selBlockName.Vertices);
            Point2D upperRightPoint = GetBoxPointOfUpperRight(selBlockName.Vertices);
            
            foreach (Text eachText in selTextList)
            {
                if(UtilityEx.PointInRect(eachText.InsertionPoint, lowerLeftPoint, upperRightPoint))
                {
                    newName = eachText.TextString;
                    break;
                }
            }
            if(newName == "")
            {
                foreach (MultilineText eachText in selMultiTextList)
                {
                    if (UtilityEx.PointInRect(eachText.InsertionPoint, lowerLeftPoint, upperRightPoint))
                    {
                        newName = eachText.TextString;
                        break;
                    }
                }
            }

            return newName;
        }

        private double[] GetBoxPointValueOfLowerLeft(Point3D[] selPoints)
        {
            double newX = 9999999;
            double newY = 9999999;

            // X : Min Value
            // Y : Min Value
            foreach(Point3D eachPoint in selPoints)
            {
                double refNewPointX = Math.Round(eachPoint.X, 1, MidpointRounding.AwayFromZero);
                double refNewPointY = Math.Round(eachPoint.Y, 1, MidpointRounding.AwayFromZero);
                if (newX > refNewPointX)
                    newX = refNewPointX;
                if (newY > refNewPointY)
                    newY = refNewPointY;
            }

            return new double[] { newX, newY };
        }
        private double[] GetBoxPointValueOfUpperLeft(Point3D[] selPoints)
        {
            double newX = 9999999;
            double newY = -9999999;

            // X : Min Value
            // Y : Max Value
            foreach (Point3D eachPoint in selPoints)
            {
                double refNewPointX = Math.Round(eachPoint.X, 1, MidpointRounding.AwayFromZero);
                double refNewPointY = Math.Round(eachPoint.Y, 1, MidpointRounding.AwayFromZero);
                if (newX > refNewPointX)
                    newX = refNewPointX;
                if (newY < refNewPointY)
                    newY = refNewPointY;
            }

            return new double[] { newX, newY };
        }
        private double[] GetBoxPointValueOfUpperRight(Point3D[] selPoints)
        {
            double newX = -9999999;
            double newY = -9999999;

            // X : Max Value
            // Y : Max Value
            foreach (Point3D eachPoint in selPoints)
            {
                double refNewPointX = Math.Round(eachPoint.X, 1, MidpointRounding.AwayFromZero);
                double refNewPointY = Math.Round(eachPoint.Y, 1, MidpointRounding.AwayFromZero);
                if (newX < refNewPointX)
                    newX = refNewPointX;
                if (newY < refNewPointY)
                    newY = refNewPointY;
            }

            return new double[] { newX, newY };
        }
        private double[] GetBoxPointValueOfLowerRight(Point3D[] selPoints)
        {
            double newX = -9999999;
            double newY = 9999999;

            // X : Max Value
            // Y : Min Value
            foreach (Point3D eachPoint in selPoints)
            {
                double refNewPointX = Math.Round(eachPoint.X, 1, MidpointRounding.AwayFromZero);
                double refNewPointY = Math.Round(eachPoint.Y, 1, MidpointRounding.AwayFromZero);
                if (newX < refNewPointX)
                    newX = refNewPointX;
                if (newY > refNewPointY)
                    newY = refNewPointY;
            }

            return new double[] { newX, newY };
        }

        private Point2D GetBoxPointOfLowerLeft(Point3D[] selPoints)
        {
            double[] newValue = GetBoxPointValueOfLowerLeft(selPoints);
            return new Point2D(newValue[0], newValue[1]);
        }
        private Point2D GetBoxPointOfUpperRight(Point3D[] selPoints)
        {
            double[] newValue = GetBoxPointValueOfUpperRight(selPoints);
            return new Point2D(newValue[0], newValue[1]);
        }
    }
}
