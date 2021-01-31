using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaperSetting.Commons
{
    public enum PAPERFORMAT_TYPE
    {
        A0_ISO, 
        A1_ISO, 
        A2_ISO, 
        A3_ISO, 
        A4_ISO, 
        A4_LANDSCAPE_ISO, 
        A_ANSI, 
        A_LANDSCAPE_ANSI, 
        B_ANSI, 
        C_ANSI, 
        D_ANSI, 
        E_ANSI
    }
    
    public enum DOCKPOSITION_TYPE
    {
        TOP,
        LEFT,
        RIGHT,
        BOTTOM,
        FILL,
        FLOATING,
        NONE
    }

    public enum HORIZONTALALIGNMENT_TYPE
    {
        LEFT,
        CENTER,
        RIGHT,
        STRETCH
    }
    public enum VERTICALALIGNMENT_TYPE
    {
        TOP,
        CENTER,
        BOTTOM,
        STRETCH
    }
}
