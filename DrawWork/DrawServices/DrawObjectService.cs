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

using AssemblyLib.AssemblyModels;
using System.Diagnostics;

namespace DrawWork.DrawServices
{
    public class DrawObjectService
    {
        private DrawService drawService;
        private DrawBlockService drawBlockService;
        private DrawNozzleService drawNozzleService;

        private DrawContactPointService cpService;

        private ValueService valueService;

        public DrawObjectService()
        {
            drawService = new DrawService();
            drawBlockService = new DrawBlockService();
            drawNozzleService = new DrawNozzleService();

            cpService = new DrawContactPointService();

            valueService = new ValueService();
        }


        public void DoRefPoint(string[] eachCmd,ref CDPoint refPoint,ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j].ToLower())
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                        {
                            CDPoint newPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            refPoint = newPoint;
                            curPoint.X = refPoint.X;
                            curPoint.Y = refPoint.Y;
                        }
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
                switch (eachCmd[j].ToLower())
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                        {
                            CDPoint newPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
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

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j].ToLower())
                {
                    case "xy1":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "xy2":
                        if (j + 1 <= eachCmd.Length)
                            newPoint2 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
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
            Line customLine= drawService.Draw_Line(newPoint1, newPoint2);

            // Method
            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j].ToLower())
                {
                    case "offset":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newOffsetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            
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

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j].ToLower())
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
                            newDgree = valueService.GetDoubleValue(eachCmd[j + 1]);
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

            Line returnEntity= drawService.Draw_Line(newPoint1, newPoint2, newDgree);
            // Mirror
            if (returnEntity != null)
            {
                for (int j = refIndex; j < eachCmd.Length; j += 2)
                {
                    switch (eachCmd[j].ToLower())
                    {
                        case "mirror":
                            if (j + 1 <= eachCmd.Length)
                            {
                                switch (eachCmd[j + 1].ToLower())
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

                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            break;


                    }
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


            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j].ToLower())
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
            return drawService.Draw_Rectangle(newPoint1, newWidth, newHeight);

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

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j].ToLower())
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
            return drawService.Draw_Arc(newPoint1, newPoint2,newPoint3);

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

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j].ToLower())
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
            return drawService.Draw_Text(newPoint1, newText, newHeight, newAlign);

        }


        // Block
        public Entity[] DoBlockTopAngle(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint, ObservableCollection<EqualAngleSizeModel> selAngleSize)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();

            string newAngleType = "";
            string newAngleSize = "";
            EqualAngleSizeModel newAngle = new EqualAngleSizeModel();

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j].ToLower())
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "type":
                        if (j + 1 <= eachCmd.Length)
                            newAngleType = eachCmd[j + 1];
                        break;

                    case "size":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newAngleSize = eachCmd[j + 1];
                            foreach(EqualAngleSizeModel eachAngle in selAngleSize)
                            {
                                if (eachAngle.Size == newAngleSize)
                                {
                                    newAngle = eachAngle;
                                }
                            }
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
            if (newAngleType!="" && newAngleSize != "")
                returnEntity= drawBlockService.DrawBlock_TopAngle(newPoint1, newAngleType, newAngle);



            // Mirror
            if (returnEntity != null)
            {
                for (int j = refIndex; j < eachCmd.Length; j += 2)
                {
                    switch (eachCmd[j].ToLower())
                    {
                        case "mirror":
                            if (j + 1 <= eachCmd.Length)
                            {
                                switch (eachCmd[j + 1].ToLower())
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

        // Nozzle
        public Entity[] DoNozzle(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint, AssemblyModel selAssembly)
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

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j].ToLower())
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
            if (newNozzleType != "" && newNozzlePosition != "")
                returnEntity = drawNozzleService.DrawNozzle_GA(ref refPoint, newPosition,newNozzleType, newNozzlePosition, newNozzleFontSize, newReaderCircleSize,newMultiColumn, selAssembly);

            return returnEntity;
        }

        // Contact Point
        public void DoContactPoint(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint, AssemblyModel selAssembly)
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
                newPoint1 = cpService.ContactPoint(eachCmd[j].ToLower(), ref refPoint, ref curPoint, selAssembly);
                if (newPoint1 != null)
                {
                    curPoint = (CDPoint)newPoint1.Clone();

                }

            }

        }

        // Dimension
        public Entity[] DoDimension(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
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

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j].ToLower())
                {
                    case "xy1":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "xy2":
                        if (j + 1 <= eachCmd.Length)
                            newPoint2 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "xyh":
                        if (j + 1 <= eachCmd.Length)
                            newPoint3 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "position":
                        if (j + 1 <= eachCmd.Length)
                            newPosition = eachCmd[j + 1];
                        break;

                    case "dh":
                    case "dimheight":
                        if (j + 1 <= eachCmd.Length)
                            newDimHeight = valueService.Evaluate(eachCmd[j + 1]);
                        break;

                    case "th":
                    case "textheight":
                        if (j + 1 <= eachCmd.Length)
                            newTextHeight = valueService.Evaluate(eachCmd[j + 1]);
                        break;

                    case "tg":
                    case "textgap":
                        if (j + 1 <= eachCmd.Length)
                            newTextGap = valueService.Evaluate(eachCmd[j + 1]);
                        break;

                    case "as":
                    case "arrowsize":
                        if (j + 1 <= eachCmd.Length)
                            newArrowSize = valueService.Evaluate(eachCmd[j + 1]);
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

            returnEntity =drawService.Draw_Dimension(newPoint1, newPoint2, newPoint3, newPosition,newDimHeight, newTextHeight, newTextGap, newArrowSize, 0);

            return returnEntity;
        }

    }
}
