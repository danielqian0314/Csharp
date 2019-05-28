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
using System.Web;

namespace DasWs.Controller
{
    public class  LineDetail: DasControllerBase<LineDetailModel, LineDetailSession>
    {



        public LineDetail()
        {
          
        }

        public override void OnLoad()
        {
            base.OnLoad();
            this.Model.MainPage = "LineDetail";

        }

        [AllowAnonymous]
        public ActionRet Main()
        {
            IRealTimeData dataHandel = new RealTimeDataGenerator();
            ILineInfo RegisteredData = new XMLLineInfoReader();

            this.Model.LineNo = GetParameter("lineNo").StrToInt();
            this.SessionData.LineNo = GetParameter("lineNo").StrToInt();
            this.Model.WerkNo = GetParameter("werkNo").StrToInt();
            this.Model.Administrator = RegisteredData.getManager(this.Model.LineNo);
            this.Model.Assignment = RegisteredData.getAssignment(this.Model.LineNo);
            this.Model.Item = RegisteredData.getItem(this.Model.LineNo);
            this.Model.ItemDescription = RegisteredData.getItemDescription(this.Model.LineNo);
            this.Model.UMK_total = RegisteredData.getUMK_Total(this.Model.LineNo);
            
            return View("LineDetail",this.Model);
        }

        [AllowAnonymous]
        public ActionRet AjaxGetUpdateLineData()
        {
            
            AjaxUpdateData data= new AjaxUpdateData();
            IRealTimeData dataHandel = new RealTimeDataGenerator();
            ILineInfo RegisteredData = new XMLLineInfoReader();
            DataRecorder dataRecorder=new DataRecorder();

            
            
            List<int> AllStatus = dataHandel.getAllStatus(this.SessionData.LineNo);
            int FinalStatus = LineStatusIDConvertor.getFinalStatus(AllStatus);
            data.SignalLight = LineStatusIDConvertor.getBigSignalLight(FinalStatus);
            data.StatusDescription = LineStatusIDConvertor.getFinalStatusDsc(FinalStatus);
            data.SignalLights = LineStatusIDConvertor.getSignalLights(AllStatus);
            data.StatusDscs = LineStatusIDConvertor.getStautsDscs(AllStatus);
            data.Speed = dataHandel.getSpeed(this.SessionData.LineNo);           
            data.UMK_current = dataHandel.getUMK_current(this.SessionData.LineNo);
            data.CurrentUMKDiff = data.UMK_current / 100;
            data.Progress = data.UMK_current * 100 / RegisteredData.getUMK_Total(this.SessionData.LineNo);           
            data.CurrentCountDiff = dataHandel.getCurrentCountDiff(this.SessionData.LineNo);
            
            dataRecorder.RecordLineOnConsole(this.SessionData.LineNo, data);
            dataRecorder.RecordLineOnTxtFile(this.SessionData.LineNo, data);
            dataRecorder.RecordLineOnCsvFile(this.SessionData.LineNo, data);
           
            return OutputText(data.ToJSON());

        }
    }




    public class AjaxUpdateData 
    {
        public string SignalLight { get; set; }
        public string StatusDescription { get; set; }
        public List<string> SignalLights { get; set; }
        public List<string> StatusDscs { get; set; }
        public int Progress { get; set; }
        public int Speed { get; set; }
        public int UMK_current { get; set; }
        public int CurrentUMKDiff { get; set; }
        public int CurrentCountDiff { get; set; }
    }
        
    
    public class LineDetailModel : DasBaseModel
        {
            public int WerkNo { get; set; }
            public int LineNo { get; set; }
            public string Administrator { get; set; }
            public string Assignment { get; set; }
            public string Item { get; set; }
            public string ItemDescription { get; set; }           
            public int UMK_total { get; set; }
        }

        

        
        public class LineDetailSession
        {
            public int LineNo { get; set; } 
        }

        



    
}
