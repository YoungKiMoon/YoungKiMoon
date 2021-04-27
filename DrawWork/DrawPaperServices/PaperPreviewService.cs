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
using Color = System.Drawing.Color;
using System.Collections.ObjectModel;
using DrawWork.ValueServices;
using DrawWork.DrawSacleServices;

namespace DrawWork.DrawPaperServices
{
    public class PaperPreviewService
    {
        ValueService valueService;
        DrawScaleService scaleService;

        private Model singleModel = null;
        private Drawings singleDraw=null;

        private int selCount=0;
        public PaperPreviewService()
        {
            valueService = new ValueService();
            scaleService = new DrawScaleService();


        }
        public void SetModelObject(Model selModel)
        {
            singleModel = selModel;
        }
        public void SetDrawingsObject(Drawings selDraw)
        {
            singleDraw = selDraw;
        }

        public void CreateVectorView(ViewPortSettingModel selViewPort)
        {

            bool first = false;
            singleDraw.Sheets.Clear();
            singleDraw.Entities.Clear();
            singleDraw.Blocks.Remove("newView");
            if (singleDraw.Sheets.Count == 0)
            {
                CreateSheet();
                first = true;
            }



            if(first)
            {
                Sheet selSheet = singleDraw.Sheets[0];

                Camera newCamera = new Camera();

                newCamera.FocalLength = 0;
                newCamera.ProjectionMode = projectionType.Orthographic;

                newCamera.Rotation.X = 0.5;
                newCamera.Rotation.Y = 0.5;
                newCamera.Rotation.Z = 0.5;
                newCamera.Rotation.W = 0.5;

                // 표적 중앙
                newCamera.Target.X = valueService.GetDoubleValue(selViewPort.TargetX);
                newCamera.Target.Y = valueService.GetDoubleValue(selViewPort.TargetY);
                newCamera.Target.Z = 0;


                
                double extensionAmount = Math.Min(selSheet.Width, selSheet.Height) / 400;

                VectorView newView = new VectorView(valueService.GetDoubleValue(selViewPort.LocationX),
                                                    valueService.GetDoubleValue(selViewPort.LocationY),
                                                    newCamera,
                                                    valueService.GetDoubleValue(selViewPort.Scale),
                                                    "newView",
                                                    valueService.GetDoubleValue(selViewPort.SizeX),
                                                    valueService.GetDoubleValue(selViewPort.SizeY));
                
                newView.CenterlinesExtensionAmount = extensionAmount;
                //newView.CenterlinesExtensionAmount = 0;
                //newView.FillRegions = false;
                //newView.FillTexts = false;

                //selSheet.AddViewPlaceHolder(newView, singleModel, singleDraw, "GAPlaceHolder");

                Camera newnewCamera = new Camera(new Point3D(valueService.GetDoubleValue(selViewPort.TargetX), valueService.GetDoubleValue(selViewPort.TargetY), 0),
                                0,
                                Viewport.GetCameraRotation(viewType.Top),
                                projectionType.Orthographic,
                                0,
                                1);
                VectorView newnewView = new VectorView(valueService.GetDoubleValue(selViewPort.LocationX),
                                                    valueService.GetDoubleValue(selViewPort.LocationY),
                                                    newnewCamera,
                                                    scaleService.GetViewScale( valueService.GetDoubleValue(selViewPort.Scale)),
                                                    "newView",
                                                    valueService.GetDoubleValue(selViewPort.SizeX),
                                                    valueService.GetDoubleValue(selViewPort.SizeY)
                                                    );
                newnewView.CenterlinesExtensionAmount = extensionAmount;

                selSheet.AddViewPlaceHolder(newnewView, singleModel, singleDraw, "GAPlaceHolder");

                //selCount++;
                //selSheet.Entities.Clear();

            }
            else
            {
                Sheet newSheet = singleDraw.Sheets[0];
                VectorView newView = newSheet.Entities[0] as VectorView;
                newView.Camera.Target.X = valueService.GetDoubleValue(selViewPort.TargetX);
                newView.Camera.Target.Y = valueService.GetDoubleValue(selViewPort.TargetY);
                newView.Scale = valueService.GetDoubleValue(selViewPort.Scale);
                //newView.X= valueService.GetDoubleValue(selViewPort.LocationX);
                //newView.Y = valueService.GetDoubleValue(selViewPort.LocationY);
                //newView.Window.Width= Convert.ToSingle( valueService.GetDoubleValue(selViewPort.SizeX));
                //newView.Width = valueService.GetDoubleValue(selViewPort.SizeX);

            }




            //ViewBuilder _viewBuilder = new ViewBuilder(singleModel, singleDraw, true, ViewBuilder.operatingType.Queue);
            //_viewBuilder.AddToQueue(singleDraw, true, selSheet);
            //singleDraw.StartWork(_viewBuilder);

            singleDraw.UpdateBoundingBox();
            singleDraw.Invalidate();
            

            singleDraw.Rebuild(singleModel);
            singleDraw.ActiveSheet = singleDraw.Sheets[0];
            singleDraw.Invalidate();
            //singleDraw.ActiveSheet = selSheet;

            singleDraw.ZoomFit();
            singleDraw.ActionMode = actionType.SelectByPick;
            singleDraw.Invalidate();


            


        }

        private void CreateSheet()
        {
            // A3
            selCount++;
            //Sheet newSheet = new Sheet(linearUnitsType.Millimeters, 420, 297, "PreviewGA" + selCount.ToString());
            Sheet newSheet = new Sheet(linearUnitsType.Millimeters, 841, 594, "PreviewGA" + selCount.ToString());
            singleDraw.Sheets.Add(newSheet);
            singleDraw.Invalidate();
        }
    }
}
