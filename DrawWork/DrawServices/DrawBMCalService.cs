using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawServices
{
    public class DrawBMCalService
    {
        public DrawBMCalService()
        {

        }



        // Shell Length 
        public double GetCalShellLength(double ID, double courseThk, double qty)
        {
            double returnValue = 0;
            // 필요한 주석
            double tempCal01 = Math.PI * (ID + courseThk) / qty;



            returnValue = tempCal01;



            return returnValue;
        }



        // Shell X
        public double GetCalShellX(double Wd, double Lg)
        {
            double returnValue = 0;
            // 필요한 주석
            double TempCal01 = Math.Pow(Wd, 2) + Math.Pow(Lg, 2);



            double TempCal02 = Math.Sqrt(TempCal01);



            returnValue = TempCal02;



            return returnValue;
        }



        // Shell Qty
        public double GetCalShellQty(double Id, double Thk)
        {
            double returnValue = 0;
            // 
            double TempCal01 = Math.PI * (Id + Thk) / 10000;



            double TempCal02 = Math.Ceiling(TempCal01);



            returnValue = TempCal02;



            return returnValue;
        }



        // shell Weight
        public double GetCalShellWt(double Thk, double Wd, double Lg, double Den = 7.85) // Den(Density) -> CS = 7.85, SS = 7.93
        {
            double returnValue = 0;



            double TempCal01 = (Thk * Wd * Lg * Den) / 1000000;



            returnValue = TempCal01;



            return returnValue;
        }




        // Wind Girder
        public double GetCalWindGirder(double Id, double Thk, double Dist)
        {
            double returnValue = 0;



            double TempCal01 = Math.PI * (Id + Thk * 2 + Dist * 2);



            returnValue = TempCal01;



            return returnValue;
        }




        // Wind Girder Weight
        public  double GetCalWindGirderWt(double Thk, double Lg, double Dist, double cir, double Qty, double Den = 7.85) // Den(Density) -> CS = 7.85, SS = 7.93
        {
            double returnValue = 0;



            double TempCal01 = (Thk * Lg * Dist * cir * Qty * Den) / 1000000;



            returnValue = TempCal01;



            return returnValue;
        }





        // Anchor Chair TopPlate Weight
        public double GetCalAnchorChairTopPlateWt(double Thk, double B, double F, double Qty, double Den = 7.85) // Den(Density) -> CS = 7.85, SS = 7.93
        {
            double returnValue = 0;



            double TempCal01 = (Thk * B * F * Qty * Den) / 1000000;



            returnValue = TempCal01;



            return returnValue;
        }




        // Anchor Chair GUSSET PLATE Weight
        public  double GetCalAnchorChairGussetPlateWt(double Thk, double Wd, double Lg, double Qty, double Den = 7.85) // Den(Density) -> CS = 7.85, SS = 7.93
        {
            double returnValue = 0;



            double TempCal01 = (Thk * Wd * Lg * Qty * Den) / 1000000;



            returnValue = TempCal01;



            return returnValue;
        }





        // Anchor bolt/2N Weight
        //bolt Weight + Nut Weight



        public  double GetCalAnchorBolt2NWt(double BoltKgm, double Lg, double NutKgea, double Qty, double Den = 7.85) // Den(Density) -> CS = 7.85, SS = 7.93
        //Qty = Sets 값으로 bolt 1, Nut 2 가 1Set / Nut의 갯수는 Qty * 2 
        {
            double returnValue = 0;



            //TempCal01 = bolt Weight
            double TempCal01 = BoltKgm * (Lg + 500) * Qty;



            //Nut Weight
            double TempCal02 = NutKgea * Qty * 2;



            returnValue = TempCal02;



            return returnValue;
        }





        // Anchor Chair Washer Plate
        public  double GetCalAnchorChairWasherPlateWt(double Thk, double Wd, double Lg, double Qty, double Den = 7.85) // Den(Density) -> CS = 7.85, SS = 7.93
        {
            double returnValue = 0;



            double TempCal01 = (Thk * Wd * Lg * Qty * Den) / 1000000;



            returnValue = TempCal01;



            return returnValue;
        }





        // Earth Lug Weight
        public  double GetCalEarthLugWt(double Thk, double Wd, double Lg, double Qty, double Den = 7.85) // Den(Density) -> CS = 7.85, SS = 7.93
        {
            double returnValue = 0;



            double TempCal01 = (Thk * Wd * Lg * Qty * Den) / 1000000;



            returnValue = TempCal01;



            return returnValue;
        }





        // SETTLEMENT CHECK PIECE Weight
        public  double GetCalSettlementCheckPieceWt(double Wd, double Qty, double Den = 7.85, double Thk = 8, double Si = 65) // Den(Density) -> CS = 7.85, SS = 7.93
                                                                                                                                    //Size 값 default Thk = 8, Si = 65 
        {
            double returnValue = 0;



            double TempCal01 = (Thk * Si * 2 * Wd * Qty * Den) / 1000000;



            returnValue = TempCal01;



            return returnValue;
        }



        // 명판 브라켓 무게
        // Name Plate Bracket Weight
        public  double GetCalNamePlateBracketWt(double Wd, double InThk, double Lg, double Qty = 1, double Den = 7.85, double Thk = 4.5)
        // Den(Density) -> CS = 7.85, SS = 7.93
        // Default 값 Thk = 4.5, Qty = 1
        // InThk = Shell sheet> Shell > insulation > thk.
        {
            double returnValue = 0;



            double TempCal01 = Wd + (30 + InThk * 2);



            double TempCal02 = Thk * TempCal01 * Lg * Qty * Den / 1000000;



            returnValue = TempCal02;



            return returnValue;
        }

    }
}
