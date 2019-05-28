using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DasWs.Controller
{
    interface ILineInfo
    {
        List<int> getRegisteredLineNos(int WerkNo);
        
        int getUMK_Total(int LineNo);

        string getAssignment(int LineNo);
        
        string getItem(int LineNo);
        
        string getItemDescription(int LineNo);

        string getManager(int LineNo);
        

    }
}
