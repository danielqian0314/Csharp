using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DasWs.Services
{
     public class RealTimeDataGenerator:Controller.IRealTimeData
    {
         public List<int> StatusList = new List<int> { 1, 2, 3, 4, 7, 8, 9, 12, 13, 14, 15, 16 };
        
        public List<int> getAllStatus(int lineNo)
        {
           Random ro = new Random(DateTime.Now.Second+lineNo);
           
           List<int> AllStatus=new List<int>();
           for (int i = 0; i < 4; i++)
           {
               int status = this.StatusList[ro.Next(12)];
               AllStatus.Add(status);
           }
           return AllStatus;
        }

       

       

        public int getSpeed(int lineNo) 
        {
            Random ro = new Random(DateTime.Now.Second + lineNo);
            int speed = ro.Next(120);
            return speed;
        }

       

        public int getUMK_current(int lineNo) 
        {
            Random ro = new Random(DateTime.Now.Second + lineNo);
            int UMK_Current = ro.Next(1000);
            return UMK_Current;
        }

       

        public int getCurrentCountDiff(int lineNo)
        {
            Random ro = new Random(DateTime.Now.Second + lineNo);
            int CurrentCountDiff = ro.Next(120);
            return CurrentCountDiff;
        }

        public List<int> getRegisteredlineNos(int WerkNo)
        {
            
            List<int> RegisteredlineNos=new List<int>();
            for(int i=0;i<12;i++){
                RegisteredlineNos.Add(WerkNo*100+i);
            }
            return RegisteredlineNos;
        }
    }
}
