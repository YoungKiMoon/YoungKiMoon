using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawSettingLib.Commons;
using DrawSettingLib.SettingModels;
using DrawWork.Commons;
using DrawWork.DrawStyleServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MColor = System.Windows.Media.Color;
using Color = System.Drawing.Color;

namespace DrawWork.DrawGridServices
{
    public class DrawPaperGridService
    {
        private StyleFunctionService styleService;
        private LayerStyleService layerService;

        public DrawPaperGridService()
        {
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();
        }


        public DrawPaperGridModel GetPaperGrid(ObservableCollection<DrawPaperGridModel> selGridList,PAPERMAIN_TYPE selDwgName,double pageNumber)
        {
            DrawPaperGridModel newGrid = null;
            foreach (DrawPaperGridModel eachGrid in selGridList)
            {
                if (eachGrid.Name == selDwgName)
                {
                    if (eachGrid.Page == pageNumber)
                    {
                        newGrid = eachGrid;
                        break;
                    }
                    
                }
            }

            return newGrid;
        }

        // size Update
        public void SetGirdSizeUpdate(ObservableCollection<DrawPaperGridModel> selGridList)
        {
            foreach(DrawPaperGridModel selPaperGrid in selGridList)
            {
                double columnRef = selPaperGrid.ColumnDef;
                double rowRef = selPaperGrid.RowDef;

                double width = selPaperGrid.Size.Width;
                double height = selPaperGrid.Size.Height;
                double columnMargin = 4;
                double rowMargin = 10;
                double calWidth = width - ((columnRef - 1) * columnMargin);
                double calHeight = height - ((rowRef - 1) * rowMargin);
                double widthOne = calWidth / columnRef;
                double heightOne = calHeight / rowRef;
                foreach (PaperAreaModel eachArea in SingletonData.PaperArea.AreaList)
                {

                }
            }
        }

        // 한 Page Grid  그리기
        public List<Entity> CreateGridEntity(DrawPaperGridModel selPaperGrid, PAPERMAIN_TYPE selDwgName,double selDwgPage, Drawings singleDraw)
        {
            List<List<Entity>> gridEntity = new List<List<Entity>>();
            double columnRef = selPaperGrid.ColumnDef;
            double rowRef = selPaperGrid.RowDef;

            double width = selPaperGrid.Size.Width;
            double height = selPaperGrid.Size.Height;
            double columnMargin = 4;
            double rowMargin = 10;
            double calWidth = width - ((columnRef- 1) * columnMargin);
            double calHeight=height- ((rowRef- 1) * rowMargin);
            double widthOne = calWidth / columnRef;
            double heightOne = calHeight / rowRef;

            Point3D referencePoint = new Point3D(selPaperGrid.Location.X, selPaperGrid.Location.Y);

            int[,] gridArray = GetGridArray(rowRef, columnRef);
            foreach (PaperAreaModel eachArea in SingletonData.PaperArea.AreaList)
            {
                // 같은 DWG
                if (eachArea.DWGName == selDwgName)
                {
                    if (eachArea.Page == selDwgPage)
                    {
                        if (eachArea.visible)
                        {
                            // arrange
                            Point3D eachPoint = GetGridPoint(gridArray, referencePoint, eachArea, columnRef, rowRef, widthOne, heightOne, columnMargin, rowMargin);
                            eachArea.Location.X = eachPoint.X;
                            eachArea.Location.Y = eachPoint.Y;

                            // Draw
                            gridEntity.Add(GetGridOne(GetSumPoint(eachPoint, 0, 0), eachArea, heightOne, widthOne, columnMargin, rowMargin, singleDraw));
                        }
                    }


                }
            }




            List<Entity> newList = new List<Entity>();
            foreach (List<Entity> eachList in gridEntity)
                newList.AddRange(eachList);
            return newList;
        }
        private int[,] GetGridArray(double rowRef, double columnRef)
        {
            int[,] newArray = new int[Convert.ToInt32(rowRef),Convert.ToInt32(columnRef)];
            for(int i = 0; i < rowRef; i++)
            {
                for (int j = 0; j < columnRef; j++)
                {
                    newArray[i, j] = 0; // Default value
                }
            }

            return newArray;
        }

        private Point3D GetGridPoint(int[,] gridArray,Point3D selPoint, PaperAreaModel selArea,
                                     double columnRef, double rowRef,double widthOne, double heightOne,  double columnMargin, double rowMargin)
        {
            Point3D returnPoint = new Point3D();
            double rowFix = selArea.Row;
            double columnFix = selArea.Column;
            double rowSpan = selArea.RowSpan;
            double columnSpan = selArea.ColumnSpan;
            bool assignmentComplete = false;    

            if (selArea.IsFix)
            {
                rowFix = selArea.Row;
                columnFix = selArea.Column;
                for (int rowS = 0; rowS < rowSpan; rowS++)
                {
                    for (int colS = 0; colS < columnSpan; colS++)
                    {
                        int rowAdj = Convert.ToInt32(rowFix - 1 + rowS);
                        int columnAdj = Convert.ToInt32(columnFix - 1 + colS);
                        gridArray[rowAdj, columnAdj] = 1;// Use
                    }
                }

                assignmentComplete = true;
            }
            else
            {
                
                for (int i = 0; i < rowRef; i++)
                {
                    for (int j = 0; j < columnRef; j++)
                    {
                        if(gridArray[i, j] == 0)
                        {
                            assignmentComplete = true;

                            // Assignment
                            rowFix = i+1;
                            columnFix = j+1;
                            for (int rowS = 0; rowS < rowSpan; rowS++)
                            {
                                for (int colS = 0; colS < columnSpan; colS++)
                                {
                                    int rowAdj = Convert.ToInt32(rowFix - 1 + rowS);
                                    int columnAdj = Convert.ToInt32(columnFix - 1 + colS);
                                    if (gridArray[rowAdj, columnAdj] == 1) 
                                    {
                                        assignmentComplete = false;
                                        break;
                                    }

                                }
                            }

                            // Assignment Complete
                            if (assignmentComplete)
                            {
                                for (int rowS = 0; rowS < rowSpan; rowS++)
                                {
                                    for (int colS = 0; colS < columnSpan; colS++)
                                    {
                                        int rowAdj = Convert.ToInt32(rowFix - 1 + rowS);
                                        int columnAdj = Convert.ToInt32(columnFix - 1 + colS);
                                        gridArray[rowAdj, columnAdj] = 1;// Used
                                    }
                                }
                                break;
                            }
                        }

                    }
                    if (assignmentComplete)
                        break;
                }
            }

            if (!assignmentComplete)
            {
                rowFix = 1;
                columnFix = 1;
            }

            // Point : Left Top
            double rowRefAdj = rowRef +1 - rowFix;
            double columnRefAdj = columnFix-1;
            double rowHeight = rowRefAdj *heightOne + ((rowRefAdj-1)*rowMargin);
            double columWidth = columnRefAdj * widthOne + (columnRefAdj*columnMargin);

            returnPoint = GetSumPoint(selPoint, columWidth, rowHeight);
            return returnPoint;
        }

        private List<Entity> GetGridOne(Point3D selPoint, PaperAreaModel selArea, double heightOne, double widthOne, double columnMargin, double rowMargin, Drawings singleDraw)
        {
            List<Entity> newList = new List<Entity>();
            List<Entity> gridList = new List<Entity>();
            List<Entity> gridTitleList = new List<Entity>();


            double rowHeight = (heightOne * selArea.RowSpan) + ((selArea.RowSpan-1)*rowMargin);
            double columHeight = widthOne * selArea.ColumnSpan+((selArea.ColumnSpan-1)*columnMargin);

            double titleGap = 4;
            double nameHeight = 5.5 + 3.5 + 1 + 1;
            double viewHeight = rowHeight - titleGap - nameHeight;


            // Set Data
            selArea.Size.X = columHeight;
            selArea.Size.Y = viewHeight;

            // Ref Point : Left Top

            // View Space
            Point3D viewPoint = GetSumPoint(selPoint, 0, 0);
            Line viewTopLine = new Line(GetSumPoint(viewPoint, 0, 0), GetSumPoint(viewPoint, columHeight, 0));
            Line viewRightLine = new Line(GetSumPoint(viewPoint, columHeight, 0), GetSumPoint(viewPoint, columHeight, -viewHeight));
            Line viewBottomLine = new Line(GetSumPoint(viewPoint, columHeight, -viewHeight), GetSumPoint(viewPoint, 0, -viewHeight));
            Line viewLeftLine = new Line(GetSumPoint(viewPoint, 0, -viewHeight), GetSumPoint(viewPoint, 0, 0));
            gridList.AddRange(new Entity[] { viewTopLine, viewRightLine, viewBottomLine, viewLeftLine });

            // Name Sapce
            Point3D namePoint = GetSumPoint(selPoint, 0, -viewHeight-titleGap);
            Line nameTopLine = new Line(GetSumPoint(namePoint, 0, 0), GetSumPoint(namePoint, columHeight, 0));
            Line nameRightLine = new Line(GetSumPoint(namePoint, columHeight, 0), GetSumPoint(namePoint, columHeight, -nameHeight));
            Line nameBottomLine = new Line(GetSumPoint(namePoint, columHeight, -nameHeight), GetSumPoint(namePoint, 0, -nameHeight));
            Line nameLeftLine = new Line(GetSumPoint(namePoint, 0, -nameHeight), GetSumPoint(namePoint, 0, 0));
            gridTitleList.AddRange(new Entity[] {nameTopLine,nameRightLine,nameBottomLine,nameLeftLine });

            styleService.SetLayerListEntity(ref gridList, layerService.LayerDimension);
            styleService.SetLayerListEntity(ref gridTitleList, layerService.LayerDimension);

            // Text
            double titleHeight = 4;
            double titleSubHeight = 2.5;
            string titleStr = selArea.TitleName;
            string titleSubStr = "";
            if (selArea.TitleSubName !="")
                titleSubStr="(" + selArea.TitleSubName + ")";


            Point3D titlePoint = GetSumPoint(namePoint, columHeight / 2, -1);
            Text titleText = new Text(GetSumPoint(titlePoint, 0, 0), selArea.TitleName, titleHeight) { Alignment = Text.alignmentType.TopCenter };
            titleText.Regen(new RegenParams(0, singleDraw));
            double titleWidth = titleText.BoxSize.X;
            //styleService.SetLayer(ref titleText, layerService.LayerDimension);
            titleText.ColorMethod = colorMethodType.byEntity;
            titleText.Color = Color.FromArgb(255, 255,0);

            Line titleUnderLine1 = new Line(GetSumPoint(titlePoint, -titleWidth / 2, -titleHeight - 1), GetSumPoint(titlePoint, titleWidth / 2, -titleHeight - 1));
            Line titleUnderLine2 = new Line(GetSumPoint(titlePoint, -titleWidth / 2, -titleHeight - 2), GetSumPoint(titlePoint, titleWidth / 2, -titleHeight - 2));
            styleService.SetLayer(ref titleUnderLine1, layerService.LayerPaper);
            //styleService.SetLayer(ref titleUnderLine2, layerService.LayerDimension);
            titleUnderLine2.ColorMethod = colorMethodType.byEntity;
            titleUnderLine2.Color = Color.FromArgb(255, 255, 0);

            Text titleSubText = new Text(GetSumPoint(titlePoint, 0, -titleHeight - 3), titleSubStr, titleSubHeight) { Alignment = Text.alignmentType.TopCenter };
            styleService.SetLayer(ref titleSubText, layerService.LayerDimension);
            if (titleStr != "")
            {
                newList.Add(titleText);
                newList.Add(titleUnderLine1);
                newList.Add(titleUnderLine2);
            }
            if(titleSubStr != "")
            {
                newList.Add(titleSubText);
            }

            newList.AddRange(gridTitleList);

            if (SingletonData.PaperGridVisible)
            {
                newList.AddRange(gridList);
            }

            return newList;
        }

        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }

        public Point3D GetRefPoint(PaperAreaModel selArea, PointModel selLocation, double widthOne, double heightOne)
        {
            double refX = 0;
            double refY = 0;

            if (selArea.IsFix)
            {

            }
            else
            {

            }

            return new Point3D(refX, refY);
        }

    }
}
