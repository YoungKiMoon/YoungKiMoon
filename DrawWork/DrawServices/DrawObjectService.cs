using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using devDept.Eyeshot;
using devDept.Graphics;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Serialization;

using DrawWork.DrawModels;
using DrawWork.ValueServices;
using DrawWork.CommandModels;
using DrawWork.DrawDetailServices;

using AssemblyLib.AssemblyModels;
using System.Diagnostics;
using DrawWork.Commons;
using DrawWork.DrawStyleServices;
using DrawWork.CutomModels;
using DrawSettingLib.SettingServices;
using DrawSettingLib.Commons;
using DrawSettingLib.SettingModels;

namespace DrawWork.DrawServices
{
    public class DrawObjectService
    {
        private Model singleModel;

        private AssemblyModel assemblyData;


        private DrawService drawService;
        private DrawNozzleService drawNozzleService;
        private DrawNozzleOrientationService drawNozzleOrientationService;

        private DrawLogicBlockService drawLogicBlockService;
        private DrawImportBlockService drawImportBlockService;
        private DrawReferenceBlockService drawReferenceBlockService;

        public  DrawWorkingPointService workingPointService;

        private ValueService valueService;
        private LayerStyleService layerStyle;

        private DrawLeaderService drawLeaderService;

        private PaperAreaService paperAreaService;

        private DrawDetailService detailService;
        public DrawObjectService(AssemblyModel selAssembly,Object selModel)
        {
            singleModel = selModel as Model;

            assemblyData = selAssembly;

            drawService = new DrawService(selAssembly);

            drawNozzleService = new DrawNozzleService(selAssembly,selModel);
            drawNozzleOrientationService = new DrawNozzleOrientationService(selAssembly, selModel);


            drawLogicBlockService = new DrawLogicBlockService(selAssembly, selModel);
            drawImportBlockService = new DrawImportBlockService(selAssembly,singleModel);
            drawReferenceBlockService = new DrawReferenceBlockService(selAssembly);

            workingPointService = new DrawWorkingPointService(selAssembly);

            valueService = new ValueService();
            layerStyle = new LayerStyleService();


            drawLeaderService = new DrawLeaderService(selAssembly, selModel);

            paperAreaService = new PaperAreaService();


            detailService = new DrawDetailService(selAssembly,selModel);
        }

        public DrawAssyModel GetDrawAssyModel(string[] eachCmd)
        {
            DrawAssyModel newAssy = new DrawAssyModel();
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "name":
                    case "mainname":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newAssy.mainName = paperAreaService.GetPaperMainType( eachCmd[j + 1].Trim().ToLower());
                        }
                        goto case "always";

                    case "subname":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newAssy.subName = paperAreaService.GetPaperSubType( eachCmd[j + 1].Trim().ToLower());
                        }
                        goto case "always";

                    case "always":
                        break;
                }
            }

            return newAssy;
        }

        public DrawPointModel GetDrawPoint(string[] eachCmd)
        {
            DrawPointModel newDrawPoint = new DrawPointModel();

            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "":
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                        {
                            CDPoint refPoint = new CDPoint();
                            CDPoint curPoint = new CDPoint();
                            CDPoint newPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            refPoint = newPoint;
                            curPoint.X = refPoint.X;
                            curPoint.Y = refPoint.Y;

                            newDrawPoint.referencePoint = refPoint;
                            newDrawPoint.currentPoint = curPoint;
                        }
                        goto case "always";

                    case "always":
                        break;
                }
            }
            return newDrawPoint;
        }

        public void DoRefPoint(string[] eachCmd,ref CDPoint refPoint,ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "":
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                        {
                            CDPoint newPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            refPoint = newPoint;
                            curPoint.X = refPoint.X;
                            curPoint.Y = refPoint.Y;
                        }
                        goto case "always";

                    case "always":
                        break;
                }
            }

        }


        public void DoPoint(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                string newValue = "";
                string newName = eachCmd[j];
                if (j + 1 <= eachCmd.Length)
                    newValue = eachCmd[j + 1];

                switch (newName)
                {
                    case "":
                    case "xy":
                        if (newValue !="")
                        {
                            CDPoint newPoint = drawService.GetDrawPoint(newValue, ref refPoint, ref curPoint);
                            curPoint = newPoint;
                        }
                        break;
                }
            }

        }

        public Line DoLine(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();
            CDPoint newOffsetPoint = new CDPoint();
            string layerName = layerStyle.LayerOutLine;

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy1":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "xy2":
                        if (j + 1 <= eachCmd.Length)
                            newPoint2 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "layer":
                    case "layername":
                        if (j + 1 <= eachCmd.Length)
                            layerName = eachCmd[j + 1];
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;

                }
            }

            // Create Line
            Line customLine= drawService.Draw_Line(newPoint1, newPoint2,layerName);

            // Method
            Line customLineMethod = null;
            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "mirror":
                        if (j + 1 <= eachCmd.Length)
                        {
                            switch (eachCmd[j + 1])
                            {
                                case "right":

                                    Plane pl1 = Plane.YZ;
                                    pl1.Origin.X = newPoint2.X;
                                    pl1.Origin.Y = newPoint2.Y;
                                    Mirror customMirror = new Mirror(pl1);
                                    customLine.TransformBy(customMirror);
                                    break;
                            }
                        }

                        break;

                    case "offset":
                        if (j + 1 <= eachCmd.Length)
                        {
                            double offsetDistance = valueService.GetDoubleValue(eachCmd[j + 1]);
                            customLineMethod = customLine.Offset(offsetDistance, Vector3D.AxisZ) as Line;
                        }
                            
                        break;

                    case "rotater":
                    case "rotateradian":
                        if (j + 1 <= eachCmd.Length)
                        {
                            customLine.Rotate(valueService.GetDegreeOfSlope(eachCmd[j+1]), Vector3D.AxisZ,customLine.StartPoint);
                        }
                        break;

                    case "rotated":
                    case "rotatedegree":
                        if (j + 1 <= eachCmd.Length)
                        {
                            double rotateDegree = valueService.GetDoubleValue(eachCmd[j + 1]);
                            customLine.Rotate(UtilityEx.DegToRad(rotateDegree), Vector3D.AxisZ,customLine.StartPoint);
                        }
                        break;
                }
            }
            if (customLineMethod != null)
                customLine = customLineMethod;

            // Working Point
            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "wp":
                    case "workingpoint":
                        if (j + 1 <= eachCmd.Length)
                        {
                            if(eachCmd[j + 1] == "start")
                            {
                                curPoint.X = customLine.StartPoint.X;
                                curPoint.Y = customLine.StartPoint.Y;
                            }
                            else if (eachCmd[j + 1] == "end")
                            {
                                curPoint.X = customLine.EndPoint.X;
                                curPoint.Y = customLine.EndPoint.Y;
                            }
                        }
                        break;

                }
            }

            return customLine;
        }
        public Line DoLineDgree(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();
            double newDgree = 0;
            string newDirection = "x";
            string layerName = layerStyle.LayerOutLine;

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy1":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "xy2":
                        if (j + 1 <= eachCmd.Length)
                            newPoint2 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "degree":
                        if (j + 1 <= eachCmd.Length)
                            newDgree = valueService.GetWidthOfSlope(eachCmd[j + 1]);
                        break;

                    case "direction":
                        if (j + 1 <= eachCmd.Length)
                            newDirection = eachCmd[j + 1];
                        break;

                    case "layer":
                    case "layername":
                        if (j + 1 <= eachCmd.Length)
                            layerName = eachCmd[j + 1];
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            Line returnEntity= drawService.Draw_Line(newPoint1, newPoint2, newDgree, newDirection,layerName);

            // Method
            Line customLineMethod = null;
            if (returnEntity != null)
            {
                for (int j = refIndex; j < eachCmd.Length; j += 2)
                {
                    switch (eachCmd[j])
                    {
                        case "mirror":
                            if (j + 1 <= eachCmd.Length)
                            {
                                switch (eachCmd[j + 1])
                                {
                                    case "right":

                                        Plane pl1 = Plane.YZ;
                                        pl1.Origin.X = newPoint2.X;
                                        pl1.Origin.Y = newPoint2.Y;
                                        Mirror customMirror = new Mirror(pl1);
                                        returnEntity.TransformBy(customMirror);
                                        break;
                                }
                            }

                            break;

                        case "offset":
                            if (j + 1 <= eachCmd.Length)
                            {
                                double offsetDistance = valueService.GetDoubleValue(eachCmd[j + 1]);
                                customLineMethod = returnEntity.Offset(offsetDistance, Vector3D.AxisZ) as Line;
                            }

                            break;

                        case "rotater":
                        case "rotateradian":
                            if (j + 1 <= eachCmd.Length)
                            {

                                returnEntity.Rotate(valueService.GetDegreeOfSlope(eachCmd[j + 1]), Vector3D.AxisZ, returnEntity.StartPoint);
                            }
                            break;

                        case "rotated":
                        case "rotatedegree":
                            if (j + 1 <= eachCmd.Length)
                            {
                                double rotateDegree = valueService.GetDoubleValue(eachCmd[j + 1]);
                                returnEntity.Rotate(UtilityEx.DegToRad(rotateDegree), Vector3D.AxisZ, returnEntity.StartPoint);
                            }
                            break;

                    }
                }
            }
            if (customLineMethod != null)
                returnEntity = customLineMethod;

            // Working Point
            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "wp":
                    case "workingpoint":
                        if (j + 1 <= eachCmd.Length)
                        {
                            if (eachCmd[j + 1] == "start")
                            {
                                curPoint.X = returnEntity.StartPoint.X;
                                curPoint.Y = returnEntity.StartPoint.Y;
                            }
                            else if (eachCmd[j + 1] == "end")
                            {
                                curPoint.X = returnEntity.EndPoint.X;
                                curPoint.Y = returnEntity.EndPoint.Y;
                            }
                        }
                        break;

                }
            }

            return returnEntity;

        }

        public Line[] DoRectangle(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;



            CDPoint newPoint1 = new CDPoint();
            double newWidth = 0;
            double newHeight = 0;
            CDPoint newSetPoint = new CDPoint();
            string layerName = layerStyle.LayerOutLine;


            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "w":
                        if (j + 1 <= eachCmd.Length)
                            newWidth = valueService.Evaluate(eachCmd[j + 1]);
                        break;

                    case "h":
                        if (j + 1 <= eachCmd.Length)
                            newHeight = valueService.Evaluate(eachCmd[j + 1]);
                        break;

                    case "layer":
                    case "layername":
                        if (j + 1 <= eachCmd.Length)
                            layerName = eachCmd[j + 1];
                        break;


                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }

            }

            // Create Line
            return drawService.Draw_Rectangle(newPoint1, newWidth, newHeight,layerName);

        }

        public Arc DoArc(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();
            string layerName = layerStyle.LayerOutLine;

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy1":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "xy2":
                        if (j + 1 <= eachCmd.Length)
                            newPoint2 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "xy3":
                        if (j + 1 <= eachCmd.Length)
                            newPoint3 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "layer":
                    case "layername":
                        if (j + 1 <= eachCmd.Length)
                            layerName = eachCmd[j + 1];
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            // Create Line
            return drawService.Draw_Arc(newPoint1, newPoint2,newPoint3, layerName);

        }

        public Text DoText(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;



            CDPoint newPoint1 = new CDPoint();
            string newText = "";
            double newHeight = 0;
            CDPoint newSetPoint = new CDPoint();
            string newAlign = "lb";
            string layerName = layerStyle.LayerOutLine;


            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "t":
                        if (j + 1 <= eachCmd.Length)
                            newText = eachCmd[j + 1];
                        break;

                    case "h":
                        if (j + 1 <= eachCmd.Length)
                            newHeight = valueService.Evaluate(eachCmd[j + 1]);
                        break;

                    case "a":
                        if (j + 1 <= eachCmd.Length)
                            newAlign = eachCmd[j + 1];
                        break;

                    case "layer":
                    case "layername":
                        if (j + 1 <= eachCmd.Length)
                            layerName = eachCmd[j + 1];
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }


            }

            // Create Line
            return drawService.Draw_Text(newPoint1, newText, newHeight, newAlign, layerName);

        }

        // Improt :Block
        public BlockReference DoBlockImport(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();

            string blockName = "";
            double scaleFactor = 1;

            string layerName = layerStyle.LayerOutLine;


            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "name":
                        if (j + 1 <= eachCmd.Length)
                            blockName = eachCmd[j + 1];
                        break;

                    case "scale":
                        if (j + 1 <= eachCmd.Length)
                            scaleFactor = valueService.GetDoubleValue(eachCmd[j + 1]);
                        break;

                    case "layer":
                    case "layername":
                        if (j + 1 <= eachCmd.Length)
                            layerName = eachCmd[j + 1];
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            BlockReference returnBlock = null;
            // Create Block
            if(blockName!="")
                returnBlock = drawImportBlockService.Draw_ImportBlock(newPoint1, blockName, layerName, scaleFactor);
            

            return returnBlock;


        }

        public List<BlockReference> DoBlockCustomImport(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();

            string blockName = "";
            double scaleFactor = 1;

            string layerName = layerStyle.LayerOutLine;


            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "name":
                        if (j + 1 <= eachCmd.Length)
                            blockName = eachCmd[j + 1];
                        break;

                    case "scale":
                        if (j + 1 <= eachCmd.Length)
                            scaleFactor = valueService.GetDoubleValue(eachCmd[j + 1]);
                        break;

                    case "layer":
                    case "layername":
                        if (j + 1 <= eachCmd.Length)
                            layerName = eachCmd[j + 1];
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            List<BlockReference> returnBlock = new List<BlockReference>();
            // Create Block
            //if (blockName != "")
                returnBlock.AddRange(drawImportBlockService.Draw_ImportCustomBlock(newPoint1,ref refPoint,ref curPoint));


            return returnBlock;


        }



        // Nozzle
        public DrawEntityModel DoNozzle(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint,double scaleValue)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();

            string newPosition = "";
            string newNozzleType = "";
            string newNozzlePosition = "";
            string newNozzleFontSize = "";
            string newReaderCircleSize = "";
            string newMultiColumn = "";
            NozzleInputModel newNozzle = new NozzleInputModel();
            string layerName = layerStyle.LayerDimension;


            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {

                    case "type":
                        if (j + 1 <= eachCmd.Length)
                            newNozzleType = eachCmd[j + 1];
                        break;

                    case "position":
                        if (j + 1 <= eachCmd.Length)
                            newPosition = eachCmd[j + 1];
                        break;

                    case "nozzleposition":
                        if (j + 1 <= eachCmd.Length)
                            newNozzlePosition = eachCmd[j + 1];
                        break;

                    case "fontsize":
                        if (j + 1 <= eachCmd.Length)
                            newNozzleFontSize = eachCmd[j + 1];
                        break;

                    case "leadercirclesize":
                        if (j + 1 <= eachCmd.Length)
                            newReaderCircleSize = eachCmd[j + 1];
                        break;

                    case "multicolumn":
                        if (j + 1 <= eachCmd.Length)
                            newMultiColumn = eachCmd[j + 1];
                        break;

                    case "layer":
                    case "layername":
                        if (j + 1 <= eachCmd.Length)
                            layerName = eachCmd[j + 1];
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            DrawEntityModel returnEntity = new DrawEntityModel();
            // Create Line
            if (newNozzleType != "" && newNozzlePosition != "")
                returnEntity = drawNozzleService.DrawNozzle_GA(ref refPoint, newPosition,newNozzleType, newNozzlePosition, newNozzleFontSize, newReaderCircleSize,newMultiColumn,layerName,scaleValue);

            return returnEntity;
        }

        public DrawEntityModel DoNozzleOrientation(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();

            string newPosition = "";
            string newNozzleType = "";
            string newNozzlePosition = "";
            string newNozzleFontSize = "";
            string newReaderCircleSize = "";
            string newMultiColumn = "";
            NozzleInputModel newNozzle = new NozzleInputModel();
            string layerName = layerStyle.LayerDimension;


            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {

                    case "type":
                        if (j + 1 <= eachCmd.Length)
                            newNozzleType = eachCmd[j + 1];
                        break;

                    case "position":
                        if (j + 1 <= eachCmd.Length)
                            newPosition = eachCmd[j + 1];
                        break;

                    case "nozzleposition":
                        if (j + 1 <= eachCmd.Length)
                            newNozzlePosition = eachCmd[j + 1];
                        break;

                    case "fontsize":
                        if (j + 1 <= eachCmd.Length)
                            newNozzleFontSize = eachCmd[j + 1];
                        break;

                    case "leadercirclesize":
                        if (j + 1 <= eachCmd.Length)
                            newReaderCircleSize = eachCmd[j + 1];
                        break;

                    case "multicolumn":
                        if (j + 1 <= eachCmd.Length)
                            newMultiColumn = eachCmd[j + 1];
                        break;

                    case "layer":
                    case "layername":
                        if (j + 1 <= eachCmd.Length)
                            layerName = eachCmd[j + 1];
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            DrawEntityModel returnEntity = new DrawEntityModel();
            // Create Line
            //if (newNozzleType != "" && newNozzlePosition != "")
                returnEntity = drawNozzleOrientationService.DrawNozzle_Orientation(ref refPoint, scaleValue);

            return returnEntity;
        }

        // Working Point
        public void DoWorkingPoint(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                if (j + 1 < eachCmd.Length)
                {
                    newPoint1 = workingPointService.WorkingPoint(eachCmd[j], eachCmd[j + 1], ref refPoint, ref curPoint);
                }
                else
                {
                    newPoint1 = workingPointService.WorkingPoint(eachCmd[j], ref refPoint, ref curPoint);
                }
                if (newPoint1 != null)
                {
                    curPoint = (CDPoint)newPoint1.Clone();

                }

            }

        }

        // Dimension
        public DrawEntityModel DoDimension(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint,out List<CommandPropertiyModel> selCmdFunctionList, double scaleValue)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();

            string newPosition = "top";
            double newDimHeight = 100;
            double newTextHeight = -1;
            double newTextGap = 1;
            double newArrowSize = 2.5;

            string newPrefix = "";
            string newSuffix = "";
            string newText = "";

            string layerName = layerStyle.LayerDimension;

            List<CommandPropertiyModel> newCmdFunctionList = new List<CommandPropertiyModel>();

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                string newValue = "";
                string newName = eachCmd[j];
                if (j + 1 <= eachCmd.Length)
                    newValue = eachCmd[j + 1];
                
                switch (newName)
                {
                    case "xy1":
                        newPoint1 = drawService.GetDrawPoint(newValue, ref refPoint, ref curPoint);
                        goto case "always";

                    case "xy2":
                        newPoint2 = drawService.GetDrawPoint(newValue, ref refPoint, ref curPoint);
                        goto case "always";

                    case "xyh":
                        newPoint3 = drawService.GetDrawPoint(newValue, ref refPoint, ref curPoint);
                        goto case "always";

                    case "position":
                        newPosition = newValue;
                        goto case "always";

                    case "dh":
                    case "dimheight":
                        newDimHeight = valueService.Evaluate(newValue);
                        goto case "always";

                    case "th":
                    case "textheight":
                        newTextHeight = valueService.Evaluate(newValue);
                        goto case "always";

                    case "tg":
                    case "textgap":
                        newTextGap = valueService.Evaluate(newValue);
                        break;

                    case "as":
                    case "arrowsize":
                        newArrowSize = valueService.Evaluate(newValue);
                        goto case "always";

                    case "pretext":
                    case "prefix":
                        newPrefix = newValue;
                        goto case "always";

                    case "suftext":
                    case "suffix":
                        newSuffix = newValue;
                        goto case "always";

                    case "text":
                        newText = newValue;
                        goto case "always";

                    case "layer":
                    case "layername":
                        if (j + 1 <= eachCmd.Length)
                            layerName = eachCmd[j + 1];
                        break;

                    case "sp":
                        newSetPoint = drawService.GetDrawPoint(newValue, ref refPoint, ref curPoint);
                        curPoint.X = newSetPoint.X;
                        curPoint.Y = newSetPoint.Y;
                        goto case "always";

                    case "always":
                        newCmdFunctionList.Add(new CommandPropertiyModel() { Name = newName,Value=newValue });
                        break;
                }
            }

            selCmdFunctionList= newCmdFunctionList;


            DrawEntityModel returnEntity = drawService.Draw_Dimension(newPoint1, newPoint2, newPoint3, newPosition,newDimHeight, newTextHeight, newTextGap, newArrowSize,newPrefix,newSuffix,newText, 0, scaleValue, layerName);

            return returnEntity;
        }

        public DrawEntityModel DoDimensionCustom(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint,  double scaleValue)
        {


            return  drawLogicBlockService.DrawBlock_CustomDimension(ref refPoint, ref curPoint, scaleValue);


        }
        public Dictionary<string, List<Entity>> DoDimensionExtra(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {


            return drawLogicBlockService.DrawBlock_EtraDimension(ref refPoint, ref curPoint, scaleValue);


        }

        // Leader
        public DrawEntityModel DoLeader(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();

            string newPosition = "";
            string newLeaderType = "";
            string newLength = "";
            string newFontSize = "";
            string newLayerHeight = "7";
            List<string> newText = new List<string>();
            List<string> newTextSub = new List<string>();

            string layerName = layerStyle.LayerDimension;

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                    case "xy1":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "type":
                        if (j + 1 <= eachCmd.Length)
                            newLeaderType = eachCmd[j + 1];
                        break;

                    case "position":
                        if (j + 1 <= eachCmd.Length)
                            newPosition = eachCmd[j + 1];
                        break;

                    case "length":
                        if (j + 1 <= eachCmd.Length)
                            newLength = eachCmd[j + 1];
                        break;

                    case "textheight":
                        if (j + 1 <= eachCmd.Length)
                            newFontSize = eachCmd[j + 1];
                        break;

                    case "layerheight":
                        if (j + 1 <= eachCmd.Length)
                            newLayerHeight = eachCmd[j + 1];
                        break;

                    case "text1":
                    case "text01":
                        if (j + 1 <= eachCmd.Length)
                            newText.Add(eachCmd[j + 1]);
                        break;
                    case "text2":
                    case "text02":
                        if (j + 1 <= eachCmd.Length)
                            newText.Add(eachCmd[j + 1]);
                        break;
                    case "text3":
                    case "text03":
                        if (j + 1 <= eachCmd.Length)
                            newText.Add(eachCmd[j + 1]);
                        break;
                    case "text4":
                    case "text04":
                        if (j + 1 <= eachCmd.Length)
                            newText.Add(eachCmd[j + 1]);
                        break;

                    case "textsub1":
                    case "textsub01":
                        if (j + 1 <= eachCmd.Length)
                            newTextSub.Add(eachCmd[j + 1]);
                        break;
                    case "textsub2":
                    case "textsub02":
                        if (j + 1 <= eachCmd.Length)
                            newTextSub.Add(eachCmd[j + 1]);
                        break;
                    case "textsub3":
                    case "textsub03":
                        if (j + 1 <= eachCmd.Length)
                            newTextSub.Add(eachCmd[j + 1]);
                        break;
                    case "textsub4":
                    case "textsub04":
                        if (j + 1 <= eachCmd.Length)
                            newTextSub.Add(eachCmd[j + 1]);
                        break;

                    case "layer":
                    case "layername":
                        if (j + 1 <= eachCmd.Length)
                            layerName = eachCmd[j + 1];
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }



            DrawEntityModel returnEntity = drawService.Draw_Leader(newPoint1, newLength,newPosition,newFontSize,newLayerHeight,newText,newTextSub, selModel as Model, scaleValue,layerName);

            return returnEntity;
        }

        public DrawEntityModel DoLeaderList(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();


            string newPosition = "";
            string newLeaderType = "";
            string newLength = "";
            string newFontSize = "";
            string newLayerHeight = "7";
            List<string> newText = new List<string>();
            List<string> newTextSub = new List<string>();

            string layerName = layerStyle.LayerDimension;

            


            DrawEntityModel returnEntity = drawLeaderService.GetLeaderList(eachCmd, ref refPoint, ref curPoint, selModel, scaleValue);

            return returnEntity;
        }



        // Drawing Logic : Block
        // Layer 개별로 지정해야 한다.
        public Entity[] DoBlockLogic(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint,double scaleValue)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;

            string logicBlockName = "";

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "name":
                        if (j + 1 <= eachCmd.Length)
                            logicBlockName = eachCmd[j + 1];
                        break;

                }
            }

            Entity[] returnEntity = null;

            // Drawing Logic Block
            switch (logicBlockName)
            {
                case "topangle":
                    returnEntity = (DoBlockTopAngle(eachCmd, ref refPoint, ref curPoint));
                    goto case "allways";

                case "topanglerightouter":
                    returnEntity = (DoBlockTopAngleRightOuter(eachCmd, ref refPoint, ref curPoint));
                    goto case "allways";

                case "hbeam":
                    returnEntity = (DoBlockHBeam(eachCmd, ref refPoint, ref curPoint));
                    goto case "allways";

                case "structure":
                    returnEntity = (DoBlockStructure(eachCmd, ref refPoint, ref curPoint,scaleValue));
                    goto case "allways";

                case "bottom":
                    returnEntity = (DoBlockBottom(eachCmd, ref refPoint, ref curPoint));
                    goto case "allways";

                case "roof":
                    returnEntity = (DoBlockRoof(eachCmd, ref refPoint, ref curPoint,scaleValue));
                    goto case "allways";

                //case "frt":
                //    returnEntity = (DoBlockFrt(eachCmd, ref refPoint, ref curPoint,scaleValue));
                //    goto case "allways";

                case "shell":
                    returnEntity = (DoBlockShell(eachCmd, ref refPoint, ref curPoint));
                    goto case "allways";
                case "shellrightouter":
                    returnEntity = (DoBlockShellRightOuter(eachCmd, ref refPoint, ref curPoint));
                    goto case "allways";

                case "windgirder":
                    returnEntity = (DoBlockWindGirder(eachCmd, ref refPoint, ref curPoint));
                    goto case "allways";
                case "windgirderrightouter":
                    returnEntity = (DoBlockWindGirderRightOuter(eachCmd, ref refPoint, ref curPoint));
                    goto case "allways";

                case "insulationroof":
                    returnEntity = (DoBlockInsulationRoof(eachCmd, ref refPoint, ref curPoint,scaleValue));
                    goto case "allways";

                case "insulationshell":
                    returnEntity = (DoBlockInsulationShell(eachCmd, ref refPoint, ref curPoint,scaleValue));
                    goto case "allways";

                case "anchorange":
                    returnEntity = (DoBlockAnchorChair(eachCmd, ref refPoint, ref curPoint, scaleValue));
                    goto case "allways";

                case "readerline":
                    returnEntity = (DoBlockLeader(eachCmd, ref refPoint, ref curPoint, scaleValue));
                    goto case "allways";

                case "centerline":
                    returnEntity = (DoBlockCenterLine(eachCmd, ref refPoint, ref curPoint, scaleValue));
                    goto case "allways";


                case "etcblock":
                    returnEntity = (DoBlockEtcBlock(eachCmd, ref refPoint, ref curPoint, scaleValue));
                    goto case "allways";

                case "etctext":
                    returnEntity = (DoBlockEtcText(eachCmd, ref refPoint, ref curPoint, scaleValue));
                    goto case "allways";


                // allways
                case "allways":
                    break;
            }


            return returnEntity;
        }

        public Entity[] DoBlockOrientation(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint,double scaleValue)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();
            CDPoint newMirrorPoint = new CDPoint();


            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;



                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            Entity[] returnEntity = null;
            // Create Line
            returnEntity = drawLogicBlockService.DrawBlock_Orientation(newPoint1, ref refPoint, ref curPoint,scaleValue);



            // Mirror
            if (returnEntity != null)
            {
                for (int j = refIndex; j < eachCmd.Length; j += 2)
                {
                    switch (eachCmd[j])
                    {
                        case "mirror":

                            newMirrorPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottomUp, ref refPoint, ref curPoint);
                            if (j + 1 <= eachCmd.Length)
                            {
                                switch (eachCmd[j + 1])
                                {
                                    case "right":

                                        Plane pl1 = Plane.YZ;
                                        pl1.Origin.X = newMirrorPoint.X;
                                        pl1.Origin.Y = newMirrorPoint.Y;
                                        Mirror customMirror = new Mirror(pl1);
                                        foreach (Entity eachEntity in returnEntity)
                                        {
                                            eachEntity.TransformBy(customMirror);
                                        }
                                        break;
                                }
                            }

                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            break;


                    }
                }
            }
            return returnEntity;
        }

        public Entity[] DoBlockTopAngle(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();
            CDPoint newMirrorPoint = new CDPoint();


            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;



                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            Entity[] returnEntity = null;
            // Create Line
            returnEntity = drawLogicBlockService.DrawBlock_TopAngle(newPoint1,ref refPoint, ref curPoint);



            // Mirror
            if (returnEntity != null)
            {
                for (int j = refIndex; j < eachCmd.Length; j += 2)
                {
                    switch (eachCmd[j])
                    {
                        case "mirror":

                            newMirrorPoint= workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottomUp, ref refPoint, ref curPoint);
                            if (j + 1 <= eachCmd.Length)
                            {
                                switch (eachCmd[j + 1])
                                {
                                    case "right":

                                        Plane pl1 = Plane.YZ;
                                        pl1.Origin.X = newMirrorPoint.X;
                                        pl1.Origin.Y = newMirrorPoint.Y;
                                        Mirror customMirror = new Mirror(pl1);
                                        foreach (Entity eachEntity in returnEntity)
                                        {
                                            eachEntity.TransformBy(customMirror);
                                        }
                                        break;
                                }
                            }

                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            break;


                    }
                }
            }
            return returnEntity;
        }
        public Entity[] DoBlockTopAngleRightOuter(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();
            CDPoint newMirrorPoint = new CDPoint();


            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;



                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            Entity[] returnEntity = null;
            // Create Line
            returnEntity = drawLogicBlockService.DrawBlock_TopAngleRightOuter(newPoint1, ref refPoint, ref curPoint);



            // Mirror
            if (returnEntity != null)
            {
                for (int j = refIndex; j < eachCmd.Length; j += 2)
                {
                    switch (eachCmd[j])
                    {
                        case "mirror":

                            newMirrorPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottomUp, ref refPoint, ref curPoint);
                            if (j + 1 <= eachCmd.Length)
                            {
                                switch (eachCmd[j + 1])
                                {
                                    case "right":

                                        Plane pl1 = Plane.YZ;
                                        pl1.Origin.X = newMirrorPoint.X;
                                        pl1.Origin.Y = newMirrorPoint.Y;
                                        Mirror customMirror = new Mirror(pl1);
                                        foreach (Entity eachEntity in returnEntity)
                                        {
                                            eachEntity.TransformBy(customMirror);
                                        }
                                        break;
                                }
                            }

                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            break;


                    }
                }
            }
            return returnEntity;
        }
        public Entity[] DoBlockHBeam(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();

            string newHBeamSize = "";
            HBeamModel newHBeam = new HBeamModel();

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "size":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newHBeamSize = eachCmd[j + 1];
                            //foreach (HBeamModel eachHBeam in assemblyData.HBeamList)
                            //{
                            //    if (eachHBeam.SIZE == newHBeamSize)
                            //    {
                            //        newHBeam = eachHBeam;
                            //        break;
                            //    }
                            //}
                        }
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            Entity[] returnEntity = null;
            // Create Line
            if (newHBeamSize != "")
                returnEntity = drawReferenceBlockService.DrawReference_HBeam(newPoint1, newHBeamSize);



            // Mirror
            if (returnEntity != null)
            {
                for (int j = refIndex; j < eachCmd.Length; j += 2)
                {
                    switch (eachCmd[j])
                    {
                        case "mirror":
                            if (j + 1 <= eachCmd.Length)
                            {
                                switch (eachCmd[j + 1])
                                {
                                    case "right":

                                        Plane pl1 = Plane.YZ;
                                        pl1.Origin.X = newPoint1.X;
                                        pl1.Origin.Y = newPoint1.Y;
                                        Mirror customMirror = new Mirror(pl1);
                                        foreach (Entity eachEntity in returnEntity)
                                        {
                                            eachEntity.TransformBy(customMirror);
                                        }
                                        break;
                                }
                            }

                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            break;


                    }
                }
            }
            return returnEntity;
        }
        public Entity[] DoBlockStructure(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint,double scaleValue)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            Entity[] returnEntity = null;
            // Create Line
            returnEntity = drawLogicBlockService.DrawBlock_Structure(newPoint1,scaleValue);



            // Mirror
            if (returnEntity != null)
            {
                for (int j = refIndex; j < eachCmd.Length; j += 2)
                {
                    switch (eachCmd[j])
                    {
                        case "mirror":
                            if (j + 1 <= eachCmd.Length)
                            {
                                switch (eachCmd[j + 1])
                                {
                                    case "right":

                                        Plane pl1 = Plane.YZ;
                                        pl1.Origin.X = newPoint1.X;
                                        pl1.Origin.Y = newPoint1.Y;
                                        Mirror customMirror = new Mirror(pl1);
                                        foreach (Entity eachEntity in returnEntity)
                                        {
                                            eachEntity.TransformBy(customMirror);
                                        }
                                        break;
                                }
                            }

                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            break;


                    }
                }
            }
            return returnEntity;
        }

        public Entity[] DoBlockBottom(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            Entity[] returnEntity = null;
            // Create Line
            returnEntity = drawLogicBlockService.DrawBlock_Bottom(newPoint1, ref refPoint, ref curPoint);



            // Mirror
            if (returnEntity != null)
            {
                for (int j = refIndex; j < eachCmd.Length; j += 2)
                {
                    switch (eachCmd[j])
                    {
                        case "mirror":
                            if (j + 1 <= eachCmd.Length)
                            {
                                switch (eachCmd[j + 1])
                                {
                                    case "right":

                                        Plane pl1 = Plane.YZ;
                                        pl1.Origin.X = newPoint1.X;
                                        pl1.Origin.Y = newPoint1.Y;
                                        Mirror customMirror = new Mirror(pl1);
                                        foreach (Entity eachEntity in returnEntity)
                                        {
                                            eachEntity.TransformBy(customMirror);
                                        }
                                        break;
                                }
                            }

                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            break;


                    }
                }
            }
            return returnEntity;
        }

        public Entity[] DoBlockRoof(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint,double selScaleValue)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            Entity[] returnEntity = null;
            // Create Line
            returnEntity = drawLogicBlockService.DrawBlock_Roof(newPoint1, ref refPoint, ref curPoint,selScaleValue);



            // Mirror
            if (returnEntity != null)
            {
                for (int j = refIndex; j < eachCmd.Length; j += 2)
                {
                    switch (eachCmd[j])
                    {
                        case "mirror":
                            if (j + 1 <= eachCmd.Length)
                            {
                                switch (eachCmd[j + 1])
                                {
                                    case "right":

                                        Plane pl1 = Plane.YZ;
                                        pl1.Origin.X = newPoint1.X;
                                        pl1.Origin.Y = newPoint1.Y;
                                        Mirror customMirror = new Mirror(pl1);
                                        foreach (Entity eachEntity in returnEntity)
                                        {
                                            eachEntity.TransformBy(customMirror);
                                        }
                                        break;
                                }
                            }

                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            break;


                    }
                }
            }
            return returnEntity;
        }
        public Entity[] DoBlockFrt(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint,double selScaleValue)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            Entity[] returnEntity = null;
            // Create Line
            //returnEntity = drawLogicBlockService.DrawBlock_Frt(newPoint1, ref refPoint, ref curPoint,selScaleValue);



            // Mirror
            if (returnEntity != null)
            {
                for (int j = refIndex; j < eachCmd.Length; j += 2)
                {
                    switch (eachCmd[j])
                    {
                        case "mirror":
                            if (j + 1 <= eachCmd.Length)
                            {
                                switch (eachCmd[j + 1])
                                {
                                    case "right":

                                        Plane pl1 = Plane.YZ;
                                        pl1.Origin.X = newPoint1.X;
                                        pl1.Origin.Y = newPoint1.Y;
                                        Mirror customMirror = new Mirror(pl1);
                                        foreach (Entity eachEntity in returnEntity)
                                        {
                                            eachEntity.TransformBy(customMirror);
                                        }
                                        break;
                                }
                            }

                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            break;


                    }
                }
            }
            return returnEntity;
        }

        public Entity[] DoBlockShell(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            Entity[] returnEntity = null;
            // Create Line
            returnEntity = drawLogicBlockService.DrawBlock_Shell(newPoint1, ref refPoint, ref curPoint);



            // Mirror
            if (returnEntity != null)
            {
                for (int j = refIndex; j < eachCmd.Length; j += 2)
                {
                    switch (eachCmd[j])
                    {
                        case "mirror":
                            if (j + 1 <= eachCmd.Length)
                            {
                                switch (eachCmd[j + 1])
                                {
                                    case "right":

                                        Plane pl1 = Plane.YZ;
                                        pl1.Origin.X = newPoint1.X;
                                        pl1.Origin.Y = newPoint1.Y;
                                        Mirror customMirror = new Mirror(pl1);
                                        foreach (Entity eachEntity in returnEntity)
                                        {
                                            eachEntity.TransformBy(customMirror);
                                        }
                                        break;
                                }
                            }

                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            break;


                    }
                }
            }
            return returnEntity;
        }
        public Entity[] DoBlockShellRightOuter(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            Entity[] returnEntity = null;
            // Create Line
            returnEntity = drawLogicBlockService.DrawBlock_ShellRightOuter(newPoint1, ref refPoint, ref curPoint);



            // Mirror
            if (returnEntity != null)
            {
                for (int j = refIndex; j < eachCmd.Length; j += 2)
                {
                    switch (eachCmd[j])
                    {
                        case "mirror":
                            if (j + 1 <= eachCmd.Length)
                            {
                                switch (eachCmd[j + 1])
                                {
                                    case "right":

                                        Plane pl1 = Plane.YZ;
                                        pl1.Origin.X = newPoint1.X;
                                        pl1.Origin.Y = newPoint1.Y;
                                        Mirror customMirror = new Mirror(pl1);
                                        foreach (Entity eachEntity in returnEntity)
                                        {
                                            eachEntity.TransformBy(customMirror);
                                        }
                                        break;
                                }
                            }

                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            break;


                    }
                }
            }
            return returnEntity;
        }

        public Entity[] DoBlockWindGirder(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();
            CDPoint newMirrorPoint = new CDPoint();


            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            Entity[] returnEntity = null;

            // Mirror Sign
            string mirrorSign = "";

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "mirror":
                        mirrorSign = "right";
                        break;
                }
            }




                            // Create Line
                            returnEntity = drawLogicBlockService.DrawBlock_WindGirder(newPoint1,ref refPoint,ref curPoint, mirrorSign);



            // Mirror
            if (returnEntity != null)
            {
                for (int j = refIndex; j < eachCmd.Length; j += 2)
                {
                    switch (eachCmd[j])
                    {
                        case "mirror":

                            newMirrorPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottomUp, ref refPoint, ref curPoint);

                            if (j + 1 <= eachCmd.Length)
                            {
                                switch (eachCmd[j + 1])
                                {
                                    case "right":

                                        Plane pl1 = Plane.YZ;
                                        pl1.Origin.X = newMirrorPoint.X;
                                        pl1.Origin.Y = newMirrorPoint.Y;
                                        Mirror customMirror = new Mirror(pl1);
                                        foreach (Entity eachEntity in returnEntity)
                                        {
                                            eachEntity.TransformBy(customMirror);
                                        }
                                        break;
                                }
                            }

                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            break;


                    }
                }
            }
            return returnEntity;
        }
        public Entity[] DoBlockWindGirderRightOuter(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();
            CDPoint newMirrorPoint = new CDPoint();


            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            Entity[] returnEntity = null;
            // Create Line
            returnEntity = drawLogicBlockService.DrawBlock_WindGirderRightOuter(newPoint1, ref refPoint, ref curPoint);



            // Mirror
            if (returnEntity != null)
            {
                for (int j = refIndex; j < eachCmd.Length; j += 2)
                {
                    switch (eachCmd[j])
                    {
                        case "mirror":

                            newMirrorPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottomUp, ref refPoint, ref curPoint);

                            if (j + 1 <= eachCmd.Length)
                            {
                                switch (eachCmd[j + 1])
                                {
                                    case "right":

                                        Plane pl1 = Plane.YZ;
                                        pl1.Origin.X = newMirrorPoint.X;
                                        pl1.Origin.Y = newMirrorPoint.Y;
                                        Mirror customMirror = new Mirror(pl1);
                                        foreach (Entity eachEntity in returnEntity)
                                        {
                                            eachEntity.TransformBy(customMirror);
                                        }
                                        break;
                                }
                            }

                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            break;


                    }
                }
            }
            return returnEntity;
        }

        public Entity[] DoBlockInsulationRoof(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint,double scaleValue)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();
            CDPoint newMirrorPoint = new CDPoint();


            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            Entity[] returnEntity = null;
            // Create Line
            returnEntity = drawLogicBlockService.DrawBlock_InsulationRoof(newPoint1, ref refPoint, ref curPoint,scaleValue);



            // Mirror
            if (returnEntity != null)
            {
                for (int j = refIndex; j < eachCmd.Length; j += 2)
                {
                    switch (eachCmd[j])
                    {
                        case "mirror":

                            newMirrorPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottomUp, ref refPoint, ref curPoint);

                            if (j + 1 <= eachCmd.Length)
                            {
                                switch (eachCmd[j + 1])
                                {
                                    case "right":

                                        Plane pl1 = Plane.YZ;
                                        pl1.Origin.X = newMirrorPoint.X;
                                        pl1.Origin.Y = newMirrorPoint.Y;
                                        Mirror customMirror = new Mirror(pl1);
                                        foreach (Entity eachEntity in returnEntity)
                                        {
                                            eachEntity.TransformBy(customMirror);
                                        }
                                        break;
                                }
                            }

                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            break;


                    }
                }
            }
            return returnEntity;
        }
        public Entity[] DoBlockInsulationShell(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint,double scaleValue)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();
            CDPoint newMirrorPoint = new CDPoint();


            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            Entity[] returnEntity = null;
            // Create Line
            returnEntity = drawLogicBlockService.DrawBlock_InsulationShell(newPoint1, ref refPoint, ref curPoint, scaleValue);



            // Mirror
            if (returnEntity != null)
            {
                for (int j = refIndex; j < eachCmd.Length; j += 2)
                {
                    switch (eachCmd[j])
                    {
                        case "mirror":

                            newMirrorPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottomUp, ref refPoint, ref curPoint);

                            if (j + 1 <= eachCmd.Length)
                            {
                                switch (eachCmd[j + 1])
                                {
                                    case "right":

                                        Plane pl1 = Plane.YZ;
                                        pl1.Origin.X = newMirrorPoint.X;
                                        pl1.Origin.Y = newMirrorPoint.Y;
                                        Mirror customMirror = new Mirror(pl1);
                                        foreach (Entity eachEntity in returnEntity)
                                        {
                                            eachEntity.TransformBy(customMirror);
                                        }
                                        break;
                                }
                            }

                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            break;


                    }
                }
            }
            return returnEntity;
        }

        public Entity[] DoBlockAnchorChair(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();
            CDPoint newMirrorPoint = new CDPoint();


            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            Entity[] returnEntity = null;
            // Create Line
            returnEntity = drawLogicBlockService.DrawBlock_AnchorChair(newPoint1, ref refPoint, ref curPoint, scaleValue);



            // Mirror
            if (returnEntity != null)
            {
                for (int j = refIndex; j < eachCmd.Length; j += 2)
                {
                    switch (eachCmd[j])
                    {
                        case "mirror":

                            newMirrorPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottomUp, ref refPoint, ref curPoint);

                            if (j + 1 <= eachCmd.Length)
                            {
                                switch (eachCmd[j + 1])
                                {
                                    case "right":

                                        Plane pl1 = Plane.YZ;
                                        pl1.Origin.X = newMirrorPoint.X;
                                        pl1.Origin.Y = newMirrorPoint.Y;
                                        Mirror customMirror = new Mirror(pl1);
                                        foreach (Entity eachEntity in returnEntity)
                                        {
                                            eachEntity.TransformBy(customMirror);
                                        }
                                        break;
                                }
                            }

                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            break;


                    }
                }
            }
            return returnEntity;
        }


        public Entity[] DoBlockLeader(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();
            CDPoint newMirrorPoint = new CDPoint();


            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            Entity[] returnEntity = null;
            // Create Line
            returnEntity = drawLogicBlockService.DrawBlock_Leader(newPoint1, ref refPoint, ref curPoint, scaleValue);



            // Mirror
            if (returnEntity != null)
            {
                for (int j = refIndex; j < eachCmd.Length; j += 2)
                {
                    switch (eachCmd[j])
                    {
                        case "mirror":

                            newMirrorPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottomUp, ref refPoint, ref curPoint);

                            if (j + 1 <= eachCmd.Length)
                            {
                                switch (eachCmd[j + 1])
                                {
                                    case "right":

                                        Plane pl1 = Plane.YZ;
                                        pl1.Origin.X = newMirrorPoint.X;
                                        pl1.Origin.Y = newMirrorPoint.Y;
                                        Mirror customMirror = new Mirror(pl1);
                                        foreach (Entity eachEntity in returnEntity)
                                        {
                                            eachEntity.TransformBy(customMirror);
                                        }
                                        break;
                                }
                            }

                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            break;


                    }
                }
            }
            return returnEntity;
        }


        public Entity[] DoBlockCenterLine(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();
            CDPoint newMirrorPoint = new CDPoint();


            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            Entity[] returnEntity = null;
            // Create Line
            returnEntity = drawLogicBlockService.DrawBlock_CenterLine(newPoint1, ref refPoint, ref curPoint, scaleValue);

            

            // Mirror
            if (returnEntity != null)
            {
                for (int j = refIndex; j < eachCmd.Length; j += 2)
                {
                    switch (eachCmd[j])
                    {
                        case "mirror":

                            newMirrorPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottomUp, ref refPoint, ref curPoint);

                            if (j + 1 <= eachCmd.Length)
                            {
                                switch (eachCmd[j + 1])
                                {
                                    case "right":

                                        Plane pl1 = Plane.YZ;
                                        pl1.Origin.X = newMirrorPoint.X;
                                        pl1.Origin.Y = newMirrorPoint.Y;
                                        Mirror customMirror = new Mirror(pl1);
                                        foreach (Entity eachEntity in returnEntity)
                                        {
                                            eachEntity.TransformBy(customMirror);
                                        }
                                        break;
                                }
                            }

                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            break;


                    }
                }
            }
            return returnEntity;
        }

        public Entity[] DoBlockEtcBlock(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();
            CDPoint newMirrorPoint = new CDPoint();


            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            Entity[] returnEntity = null;
            // Create Line
            returnEntity = drawLogicBlockService.DrawBlock_EtcBlock(newPoint1, ref refPoint, ref curPoint, scaleValue);



            // Mirror
            if (returnEntity != null)
            {
                for (int j = refIndex; j < eachCmd.Length; j += 2)
                {
                    switch (eachCmd[j])
                    {
                        case "mirror":

                            newMirrorPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottomUp, ref refPoint, ref curPoint);

                            if (j + 1 <= eachCmd.Length)
                            {
                                switch (eachCmd[j + 1])
                                {
                                    case "right":

                                        Plane pl1 = Plane.YZ;
                                        pl1.Origin.X = newMirrorPoint.X;
                                        pl1.Origin.Y = newMirrorPoint.Y;
                                        Mirror customMirror = new Mirror(pl1);
                                        foreach (Entity eachEntity in returnEntity)
                                        {
                                            eachEntity.TransformBy(customMirror);
                                        }
                                        break;
                                }
                            }

                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            break;


                    }
                }
            }
            return returnEntity;
        }

        
        public Entity[] DoBlockEtcText(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();
            CDPoint newMirrorPoint = new CDPoint();


            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            Entity[] returnEntity = null;
            // Create Line
            returnEntity = drawLogicBlockService.DrawBlock_EtcText(newPoint1, ref refPoint, ref curPoint, scaleValue);



            // Mirror
            if (returnEntity != null)
            {
                for (int j = refIndex; j < eachCmd.Length; j += 2)
                {
                    switch (eachCmd[j])
                    {
                        case "mirror":

                            newMirrorPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottomUp, ref refPoint, ref curPoint);

                            if (j + 1 <= eachCmd.Length)
                            {
                                switch (eachCmd[j + 1])
                                {
                                    case "right":

                                        Plane pl1 = Plane.YZ;
                                        pl1.Origin.X = newMirrorPoint.X;
                                        pl1.Origin.Y = newMirrorPoint.Y;
                                        Mirror customMirror = new Mirror(pl1);
                                        foreach (Entity eachEntity in returnEntity)
                                        {
                                            eachEntity.TransformBy(customMirror);
                                        }
                                        break;
                                }
                            }

                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            break;


                    }
                }
            }
            return returnEntity;
        }




        public DrawEntityModel DoBlockDetail(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint,object selModel, double scaleValue, PaperAreaModel selPaperAreaModel)
        {


            // ScaleValue : 강제적용
            //scaleValue = 10;


            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;

            string drawDetailName = "";

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "name":
                        if (j + 1 <= eachCmd.Length)
                            drawDetailName = eachCmd[j + 1];
                        break;

                }
            }


            DrawEntityModel returnEntity = new DrawEntityModel();

            // Drawing Logic Block
            switch (drawDetailName)
            {
                case "horizontaljoint": // 2021-09-01
                    returnEntity = detailService.detailShellService.GetShellHorizontalJoint(ref refPoint, ref curPoint, selModel,scaleValue);
                    goto case "allways";

                case "onecourseshellplate":
                    returnEntity = detailService.detailShellService.GetOneCourseShellPlate(ref refPoint, ref curPoint, selModel, scaleValue);
                    goto case "allways";


                case "comring":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawCompressorRing(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";
                case "topringcuttingplan":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawCuttingPlan_TOP_Ring(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";
                case "comringcuttingplan":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawCuttingPlan_COMP_Ring(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";


                case "anchorchair":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawAnchorChair(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";
                case "anchordetail":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawAnchorB2N2W_Detail_Type1A_Type3(ref refPoint, ref curPoint, selModel, scaleValue,2));
                    goto case "allways";


                case "topanglejoint": // 2021-09-01
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawTopAngleJointDetail_View_B(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";
                case "windgirderjoint":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawTopAngleJointDetail_View_C(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";


                case "sectiondd":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawSection_D_D(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";


                case "vertjointdetail":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawVertJointDetail_Set(ref refPoint, ref curPoint, selModel, scaleValue,1,2,1,0));
                    goto case "allways";



                case "dimensionsforcutting":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawPlanTable(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";
                case "tolerancelimit":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawToleranceLimit(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";
                case "shellplatechordlength":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawShellPlateChordLength_SectionA(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";


                case "nameplatebracket":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawNamePlateBracket(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";
                case "earthlug":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawEarthLug(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";
                case "settlementcheckpiece":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawSettlementCheckPiece(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";



                case "shellplatearrangement":
                    returnEntity.AddDrawEntity(detailService.detailTempService.ArrangeShellPlate(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";



                // Bottom
                case "bottomplatearrangement":
                    returnEntity = detailService.detailRoofBottomService.GetBottomArrangement(ref refPoint, ref curPoint,  scaleValue);
                    goto case "allways";


                case "bottomplatejointdetail":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawBottomPlateJointDetail(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";

                case "bottomplatejointannulardetail":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawBottomPlateAnnularJointDetail(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";

                case "bottomplateweldingdetailc":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawBottomPlateWeldingDetailC(ref refPoint, ref curPoint, selModel, scaleValue,selPaperAreaModel));
                    goto case "allways";
                case "bottomplateweldingdetaild":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawBottomPlateWeldingDetailD(ref refPoint, ref curPoint, selModel, scaleValue, selPaperAreaModel));
                    goto case "allways";
                case "bottomplateweldingdetailbb":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawBottomPlateWeldingDetailBB(ref refPoint, ref curPoint, selModel, scaleValue, selPaperAreaModel));
                    goto case "allways";
                case "backingstripweldingdetail":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawBackingStrip_Welding_Detail(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";
                case "bottomplateshelljointdetail":
                    // 2.5D
                    //returnEntity.AddDrawEntity(detailService.detailTempService.ArrangeShellPlate(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";


                case "bottomplatecuttingplan":
                    returnEntity.AddDrawEntity(detailService.detailCuttingPlanService.DrawBottomCuttingPlan(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";

                case "annularplatecuttingplan":
                    returnEntity.AddDrawEntity(detailService.detailCuttingPlanService.DrawBottomAnnularCuttingPlan(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";

                case "backingstrip":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawBackingStrip(ref refPoint, ref curPoint, selModel, scaleValue, selPaperAreaModel));
                    goto case "allways";




                // Roof
                case "roofplatearrangement":
                    returnEntity = detailService.detailRoofBottomService.GetRoofArrangement(ref refPoint, ref curPoint, scaleValue);
                    goto case "allways";




                case "roofcompressionringjointdetail":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawTopAngle_Detail_ALL(ref refPoint, ref curPoint, selModel, scaleValue, selPaperAreaModel));
                    goto case "allways";

                case "roofplateweldingdetailc":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawBottomPlateWeldingDetailC(ref refPoint, ref curPoint, selModel, scaleValue, selPaperAreaModel));
                    goto case "allways";

                case "roofplateweldingdetaild":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawBottomPlateWeldingDetailD(ref refPoint, ref curPoint, selModel, scaleValue, selPaperAreaModel));
                    goto case "allways";
                case "roofplateweldingdetaildd":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawBottomPlateWeldingDetailBB(ref refPoint, ref curPoint, selModel, scaleValue, selPaperAreaModel));
                    goto case "allways";
                case "roofcompressionweldingdetail":
                    returnEntity.AddDrawEntity(detailService.detailTempService.DrawRoofCompressionRing_WeldingDetail(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";
                case "roofplatecuttingplan":
                    returnEntity.AddDrawEntity(detailService.detailCuttingPlanService.DrawRoofCuttingPlan(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";
                case "roofcompressionringcuttingplan":
                    returnEntity.AddDrawEntity(detailService.detailCuttingPlanService.DrawRoofComRingCuttingPlan(ref refPoint, ref curPoint, selModel, scaleValue));
                    goto case "allways";

















                case "roofarrange":
                    // 차장님 새
                    returnEntity = detailService.detailRoofBottomService.GetRoofSampleArrange(ref refPoint, ref curPoint,  scaleValue);
                    goto case "allways";

                // allways
                case "allways":
                    break;
            }


            return returnEntity;
        }

        private CDPoint GetSumCDPoint(CDPoint selPoint1, double X, double Y, double Z = 0)
        {
            return new CDPoint(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
    }
}
