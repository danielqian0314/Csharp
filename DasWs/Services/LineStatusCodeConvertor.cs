using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DasWs.Services
{
    public static class LineStatusIDConvertor
    {
        
        public static Dictionary<int,string> SignalLightDict = new Dictionary<int, string>()
        {
            {1,"green"},
            {2,"orange"},
            {3,"orange"},
            {4,"red"},            
            {7,"red"},
            {8,"orange"},
            {9,"orange"},
            {12,"green"},
            {13,"red"},
            {14,"red"},
            {15,"red"},
            {16,"red"}
        };


        public static Dictionary<int, string> StatusDscDict = new Dictionary<int, string>()
        {
            {1,"PRODUKTION"},
            {2,"RUESTEN"},
            {3,"ANLAGE NICHT BELEGT"},
            {4,"KEIN MATERIAL"},            
            {7,"KURZZEITSTOERUNG"},
            {8,"REINIGUNG"},
            {9,"WARTUNG/INSTANDHALT."},
            {12,"PACKEN PER HAND"},
            {13,"KEINE VERPACKUNG"},
            {14,"STILLSTAND BAENDER"},
            {15,"STILLSTAND HRL"},
            {16,"BB WECHSEL"},
            
        };

        public static Dictionary<int, string> BigSignalLightDict = new Dictionary<int, string>()
       {
           {0,"red"},
           {1,"green"},
       };

        public static Dictionary<int, string> FinalStatusDscDict = new Dictionary<int, string>()
       {
           {0,"STOERUNG"},
           {1,"PRODUKTION"},
       };
        public static string getBigSignalLight(int finalStatus)
        {
            return BigSignalLightDict[finalStatus];      
        }

        public static string getFinalStatusDsc(int finalStatus)
        {
            return FinalStatusDscDict[finalStatus];      
        }


        public static List<string> getSignalLights(List<int> allStatus)
        {
            List<string> SignalLights = new List<string>();
            for (int i = 0; i < 4; i++) 
            {
                SignalLights.Add(SignalLightDict[allStatus[i]]);
            }
            return SignalLights;
        }

        public static List<string> getStautsDscs(List<int> allStatus)
        {
            List<string> StautsDscs = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                StautsDscs.Add(StatusDscDict[allStatus[i]]);
            }
            return StautsDscs;      
        }

        public static int getFinalStatus(List<int> allStatus) 
        {
            if (allStatus.Contains(1) || allStatus.Contains(12))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }


     }
}
