using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DasWs.Services
{
    public  class DataRecorder
    {
        public void RecordLineOnConsole(int lineNo, Controller.AjaxUpdateData data) 
        {
            Console.WriteLine("Linie" + lineNo);
            Console.WriteLine("Status:" + data.StatusDescription);          
            Console.WriteLine("Taktzahl:" + data.Speed + "/min");           
            Console.WriteLine("Progress:" + data.Progress + "%");            
            Console.WriteLine("Count:" + data.CurrentCountDiff);
            Console.WriteLine("Timestamp:" + DateTime.Now.ToString());
        }

        public void RecordLineOnTxtFile(int lineNo, Controller.AjaxUpdateData data) 
        {
            
            System.IO.StreamWriter file = new System.IO.StreamWriter("ProductionRecord/txt_record/Linie"+lineNo+".txt",true);

            file.WriteLine("Status:" + data.StatusDescription);
            file.WriteLine("Taktzahl:" + data.Speed + "/min");
            file.WriteLine("Progress:" + data.Progress + "%");
            file.WriteLine("Count:" + data.CurrentCountDiff);
            file.WriteLine("Timestamp:" + DateTime.Now.ToString());
            file.WriteLine("");
            file.Close();
        }

        public void RecordLineOnCsvFile(int lineNo, Controller.AjaxUpdateData data)
        {
            if(!System.IO.File.Exists("Linie"+lineNo+".csv"))
            {
                System.IO.StreamWriter fileInitiate = new System.IO.StreamWriter("ProductionRecord/csv_record/Linie" + lineNo + ".csv", true);
                fileInitiate.WriteLine("Status,Speed(/min),Progress,Count(Beutel),Timestamp");
                fileInitiate.Close();
            }
            System.IO.StreamWriter file = new System.IO.StreamWriter("ProductionRecord/csv_record/Linie" + lineNo + ".csv", true);

            file.WriteLine(data.StatusDescription + "," + data.Speed + "," + data.Progress + "%" + "," + data.CurrentCountDiff + "," + DateTime.Now.ToString());
            
            file.Close();
        }

        
    }
}
