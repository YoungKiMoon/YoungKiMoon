using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testfunction.Models;

namespace testfunction.Functions
{
    public class ShellFunction
    {
        // 원본
        public List<Shell_Out_Model> Cal(Shell_In_Model in_Model)
        {

            List<Shell_Out_Model> result = new List<Shell_Out_Model>();
            Shell_In_Num_Model inNum = TransDoubleValue(in_Model);




            if (inNum.ID < 0 || inNum.Height < 0)
                return result;
            if (inNum.PLWidth < 0 || inNum.PLHeight < 0)
                return result;


            int crsCnt = Convert.ToInt32(Math.Truncate(inNum.Height / inNum.PLWidth));
            int cMod = Convert.ToInt32(inNum.Height % inNum.PLWidth);
            int crsTopWd = 0;
            if (cMod > 0)
            {
                crsTopWd = cMod;
                crsCnt = crsCnt + 1;
            }

            // Thk
            int[] thkArray = GetThickness(crsCnt);


            double outCcmf = (inNum.ID + thkArray[0] * 2) * Math.PI;
            double plCnt = Math.Round(outCcmf / inNum.PLHeight, 0);

            for(int i = 0; i < crsCnt; i++)
            {
                double plCnt2 = Math.Round(outCcmf / inNum.PLHeight, 0);
                double plLg = Math.Round(outCcmf / plCnt2, 1);
                double plSt = Math.Round(plLg / 3, 1);
                

                Shell_Out_Model newCourse = new Shell_Out_Model();
                newCourse.No = i.ToString();       
                
                newCourse.Course = (i+1).ToString();            // D열
                newCourse.Thickness = thkArray[i].ToString();   // E열

                

                int crsPosition = (i+1) % 3;
                if (crsPosition == 0)
                    newCourse.StartPoint = plLg.ToString();     // F열
                else if (crsPosition == 1)
                    newCourse.StartPoint = plSt.ToString();     // F열
                else if (crsPosition == 2)
                    newCourse.StartPoint = (plSt*2).ToString(); // F열

                if (i == (crsCnt - 1))
                    newCourse.OnePLWidth = crsTopWd.ToString();       // G열
                else
                    newCourse.OnePLWidth = inNum.PLWidth.ToString();  // G열

                newCourse.OnePLHeight = plLg.ToString();       // H열

                newCourse.Count= plCnt.ToString();       

                result.Add(newCourse);
            }

            return result;
        }

        // 원본 1차
        public List<Shell_Out_Model> CalNew(Shell_In_Model in_Model)
        {

            List<Shell_Out_Model> result = new List<Shell_Out_Model>();
            Shell_In_Num_Model inNum = TransDoubleValue(in_Model);

            

            if (inNum.ID < 0 || inNum.Height < 0)
                return result;
            if (inNum.PLWidth < 0 || inNum.PLHeight < 0)
                return result;

            double courseCount = Math.Ceiling(inNum.Height / inNum.PLWidth);  
            double courseMod= inNum.Height % inNum.PLWidth;                   

            // Thk
            int[] thkArray = GetThickness(courseCount);

            double outCourseWidth = (inNum.ID + thkArray[0] * 2) * Math.PI; 
            double plateCount = Math.Ceiling(outCourseWidth / inNum.PLHeight);
            double plateLength = Math.Round(outCourseWidth / plateCount, 1);
            double plateStartPoint = Math.Round(plateLength / 3, 1);

            for (int i = 0; i < courseCount; i++)
            {
                Shell_Out_Model newCourse = new Shell_Out_Model();
                newCourse.No = i.ToString();
                newCourse.Course = (i + 1).ToString();          // D열
                newCourse.Thickness = thkArray[i].ToString();   // E열

                int crsPosition = (i ) % 3;
                if (crsPosition == 0)
                    newCourse.StartPoint = "0";                              // F열
                else if (crsPosition == 1)
                    newCourse.StartPoint = plateStartPoint.ToString();       // F열
                else if (crsPosition == 2)
                    newCourse.StartPoint = (plateStartPoint * 2).ToString(); // F열

                if (i == (courseCount - 1))
                    newCourse.OnePLWidth = courseMod.ToString();      // G열
                else
                    newCourse.OnePLWidth = inNum.PLWidth.ToString();  // G열

                newCourse.OnePLHeight = plateLength.ToString();  // H열

                newCourse.Count = plateCount.ToString();
                result.Add(newCourse);
            }

            return result;
        }

        // 추천 드리는 것
        public List<Shell_Out_Model> CalNewNew(Shell_In_Model in_Model)
        {

            List<Shell_Out_Model> result = new List<Shell_Out_Model>();
            Shell_In_Num_Model inNum = TransDoubleValue(in_Model);
            

            if (inNum.ID < 0 || inNum.Height < 0)
                return result;
            if (inNum.PLWidth < 0 || inNum.PLHeight < 0)
                return result;

            double courseCount = Math.Ceiling(inNum.Height / inNum.PLWidth);    // 올림
            double courseMod = inNum.Height % inNum.PLWidth;                    // 나머지

            // Thk
            int[] thkArray = GetThickness(courseCount);

            double outCourseWidth = (inNum.ID + thkArray[0] * 2) * Math.PI; 
            double plateCount = Math.Ceiling(outCourseWidth / inNum.PLHeight);
            double plateLength = Math.Round(outCourseWidth / plateCount, 1);
            double plateStartPoint = Math.Round(plateLength / 3, 1);

            
            for (int i = 0; i < courseCount; i++)
            {
                Shell_Out_Model newCourse = new Shell_Out_Model();
                newCourse.No = i.ToString();
                newCourse.Course = (i + 1).ToString();                               // D열
                newCourse.Thickness = thkArray[i].ToString();                        // E열
                newCourse.StartPoint = GetStartPoint(i, plateStartPoint).ToString(); // F열
                newCourse.OnePLWidth = inNum.PLWidth.ToString();                     // G열
                newCourse.OnePLHeight = plateLength.ToString();                      // H열
                newCourse.Count = plateCount.ToString();                             // I열
                newCourse.CountA = "임시";

                if (i == (courseCount - 1))
                    newCourse.OnePLWidth = courseMod.ToString();  // G열 // 마지막
 
                result.Add(newCourse);
            }

            return result;
        }

        private double GetStartPoint(int i,double plateStartPoint)
        {
            double result = 0;
            int crsPosition = (i) % 3;
            if (crsPosition == 0)
                result = 0;
            else if (crsPosition == 1)
                result = plateStartPoint;
            else if (crsPosition == 2)
                result = plateStartPoint * 2;
            return result;
        }




        private int[] GetThickness(double courseCount)
        {
            // Thk
            int[] thkArray = new int[Convert.ToInt32(courseCount)];
            if (thkArray.Length > 0) thkArray[0] = 28;
            if (thkArray.Length > 1) thkArray[1] = 24;
            if (thkArray.Length > 2) thkArray[2] = 21;
            if (thkArray.Length > 3) thkArray[3] = 17;
            if (thkArray.Length > 4) thkArray[4] = 13;
            if (thkArray.Length > 5) thkArray[5] = 10;
            if (thkArray.Length > 6) thkArray[6] = 7;
            if (thkArray.Length > 7) thkArray[7] = 7;

            return thkArray;
        }
        private Shell_In_Num_Model TransDoubleValue(Shell_In_Model InModel)
        {
            Shell_In_Num_Model result = new Shell_In_Num_Model();

            result.No = GetDoubleValue(InModel.No);
            result.ID = GetDoubleValue(InModel.ID);
            result.Height = GetDoubleValue(InModel.Height);
            result.PLHeight = GetDoubleValue(InModel.PLHeight);
            result.PLWidth = GetDoubleValue(InModel.PLWidth);

            return result;
        }
        private double GetDoubleValue(string selValue)
        {
            double doubleValue = 0;
            if (!double.TryParse(selValue, out doubleValue))
                doubleValue = 0;
            return doubleValue;
        }
    }
}
