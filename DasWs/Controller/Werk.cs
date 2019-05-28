using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServer.Infrasturcture.MVC;
using Utilities.IO.Logging.Interfaces;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.IO.ExtensionMethods;
using Utilities.Reflection.ExtensionMethods;
using WebServer.Infrasturcture;
using Microsoft.Practices.Unity;
using DasWs.CommonCS;
using WebServer.Infrasturcture.Status;
using System.IO;
using WebServer.Infrasturcture.IO;
using Utilities.IO;
using WebServer.Infrasturcture.Validation;
using Utilities.Serialization;

using DasWs.Services;

using System.Globalization;
using WebServerConfig;
using das_common.WebSocket;
using DasWs.Controller;


namespace DasWs.Controller
{
    public class Werk : DasControllerBase<WerkModel, WerkSession>
    {

        public Werk()
        {

        }

        public override void OnLoad()
        {
            base.OnLoad();
            this.Model.MainPage = "Werk";
        }

        [AllowAnonymous]
        public ActionRet Main()
        {
            this.Model.WerkNo = GetParameter("Werk").StrToInt();
            this.SessionData.WerkNo = GetParameter("Werk").StrToInt();
            this.Model.Lines = new List<Line>();

            ILineInfo RegisteredData = new XMLLineInfoReader();
            foreach (int lineNo in RegisteredData.getRegisteredLineNos(this.Model.WerkNo))
            {
                Line line = new Line(lineNo);
                line.Assignment = RegisteredData.getAssignment(lineNo);
                line.Item = RegisteredData.getItem(lineNo);
                line.ItemDescription = RegisteredData.getItemDescription(lineNo);
                this.Model.Lines.Add(line);
            }
            
            return View("Werk",this.Model);
        }

       [AllowAnonymous]
        public ActionRet AjaxGetUpdateData()
        {           
            List<LineUpdateData> data = new List<LineUpdateData>();
            IRealTimeData dataHandel = new RealTimeDataGenerator();
            ILineInfo RegisteredData = new Services.XMLLineInfoReader();
            
           foreach (int lineNo in RegisteredData.getRegisteredLineNos(this.SessionData.WerkNo))
            {
                LineUpdateData lineUpdateData = new LineUpdateData();
                lineUpdateData.LineNo = lineNo;
                int FinalStatus = LineStatusIDConvertor.getFinalStatus(dataHandel.getAllStatus(lineNo));
                lineUpdateData.LastSignalLight = LineStatusIDConvertor.getBigSignalLight(FinalStatus);
                lineUpdateData.LastStatusDescription = LineStatusIDConvertor.getFinalStatusDsc(FinalStatus);
                lineUpdateData.Speed = dataHandel.getSpeed(lineNo);
                lineUpdateData.Progress = dataHandel.getUMK_current(lineNo) * 100 / RegisteredData.getUMK_Total(lineNo);
                data.Add(lineUpdateData);
            }
            
            return OutputText(data.ToJSON());
        }
       
    }
       
    
    
    public class WerkModel : DasBaseModel
    {
            public int WerkNo { get; set; }
            public List<Line> Lines { get; set;}
           
            public string GetActiveStatusForWerk(int werkNo)
        {
            return werkNo == this.WerkNo ? "class='active'": "";
        }

    }

        

        public class LineUpdateData
        {
            public int LineNo { get; set;}
            public string LastSignalLight { get; set; }
            public string LastStatusDescription { get; set; }
            public int Speed { get; set; }
            public int Progress { get; set; }
        }
        
        public class WerkSession
        {
            public int WerkNo { get; set; }
        }

        public class Line 
        {
            public int LineNo { get; set; }                       
            public string Assignment { get; set; }
            public string Item { get; set; }
            public string ItemDescription { get; set; }
               
            public Line(int lineNo)
            {    
                this.LineNo = lineNo;                              
            }
        }



    
}
