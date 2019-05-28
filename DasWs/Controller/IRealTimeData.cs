using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DasWs.Controller
{
    public interface IRealTimeData
    {
        List<int> getAllStatus(int lineNo);

         int getSpeed(int LinieNo);

         int getUMK_current(int LinieNo);        

         int getCurrentCountDiff(int LinieNo);
        
    }
}
