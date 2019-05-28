using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DasWs.Services
{
     public class RegisteredDataGenerator:Controller.ILineInfo
    {
        public List<int> getRegisteredLineNos(int WerkNo)
        {
            
            List<int> RegisteredLineNos=new List<int>();
            for(int i=0;i<12;i++){
                RegisteredLineNos.Add(2551+WerkNo*100+i);
            }
            return RegisteredLineNos;
        }

        public int getUMK_Total(int LineNo)
        {
            return 1000;
        }

        public string getAssignment(int LineNo)
        {
            return "xxxxxxx";
        }

        public string getItem(int LineNo) 
        {
            return "xxxxx";
        }

        public string getItemDescription(int LineNo) 
        {
            return "ArtikelBeschreibung";
        }

        public string getManager(int LineNo) 
        {
            return "xxx xxxxxxx";
        }
    }
}
