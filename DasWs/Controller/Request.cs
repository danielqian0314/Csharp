using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DasWs.CommonCS;
using Microsoft.Practices.Unity;
using das_settings;
using das_common.WebSocket;
using DasWs.Services;
using WebServer.Infrasturcture;
using WebServer.Infrasturcture.MVC;
using System.Threading;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.IO.ExtensionMethods;
using Utilities.Reflection.ExtensionMethods;
using Utilities.Math.ExtensionMethods;
using System.Diagnostics;
using das_common.Utility;
using DasWs.Services.ExchangeDataType;
using System.Net;

namespace DasWs.Controller
{
    public class Request : DasControllerBase<RequestModel, RequestSession>
    {

        private static RequestStatus CurrentStatus = new RequestStatus ();
        private static string LastRequestPns = "";

        private static Dictionary<RequestState, string> StateViewMapping = null;

        private IUnityContainer container;
        private readonly DataExchangeLogSvc dataExchangeLogSvc;
        private readonly Icbb_http_web_server serverSetting;
        private readonly ManuallySAPRequst manuallySAPRequst;

        static Request()
        {
            StateViewMapping = new Dictionary<RequestState, string>() 
                            {
                                    { RequestState.Idle,        "RequestInput"  },
                                    { RequestState.Requesting,  "RequestingStatus"},
                                    { RequestState.Finished,    "RequestResult"  },
                                    { RequestState.Failed,      "RequestResult" }
                            };
            DebugInitialize();
        }

        [Conditional("DEBUG")]
        private static void DebugInitialize()
        {
            LastRequestPns = "PN0001" + Environment.NewLine + "PN0002" + Environment.NewLine + "PN0003" + Environment.NewLine + "PN0004" + Environment.NewLine + "PN0005" + Environment.NewLine + "PN0006";
        }

        public Request(IUnityContainer container,
                      DataExchangeLogSvc dataExchangeLogSvc,
                      Icbb_http_web_server serverSetting,
                      ManuallySAPRequst manuallySAPRequst
            )
        {
            this.manuallySAPRequst = manuallySAPRequst;
            this.serverSetting = serverSetting;
            this.dataExchangeLogSvc = dataExchangeLogSvc;
            this.container = container;
        }

        public override void OnLoad()
        {
            base.OnLoad();
            this.Model.MainPage = "Request";
            this.Model.SessionData = SessionData;          
            this.Model.CurrentStatus = CurrentStatus;

            DebugSetLastRequestPns();

        }

        [Conditional("DEBUG")]
        private void DebugSetLastRequestPns()
        {
            SessionData.LastRequestPns = LastRequestPns;
        }

        [AllowAnonymous]
        public ActionRet Main()
        {
            return View(StateViewMapping[Request.CurrentStatus.State], this.Model);          
        }

        [AllowAnonymous]
        public ActionRet StartRequest()
        {
            string pns = GetParameter("pn");
            this.SessionData.LastRequestPns = pns;            
            var list = pns.SplitEx();

            CurrentStatus.SetStart(list.Count);

            var logdict = new Dictionary<string, string> { { "pn", string.Join(";", list) } };
            var Ip = IPAddress.Parse("127.0.0.1");

            ThreadPool.QueueUserWorkItem((x) =>
            {
                this.dataExchangeLogSvc.LogRequest(Ip, logdict,"DAS_REQUEST");
                foreach (var item in list)
                {
                    CurrentStatus.CurrentProcessingItem = item;
                    Thread.Sleep(400);                    
                    CurrentStatus.ProcessedCount++;
                   
                    var result = this.manuallySAPRequst.DoRequest(item);
                    CurrentStatus.AddRequestResult(result);
                }

                CurrentStatus.SetFinished();
            });

            return Redirect(Main);
        }    

        [AllowAnonymous]
        public ActionRet AjaxRequstProcess()
        {
            return OutputText(CurrentStatus.ToJSON());
        }

        [AllowAnonymous]
        public ActionRet Reset()
        {
            CurrentStatus.State = RequestState.Idle;
            return Redirect(Main);
        }

    }

   


    public class RequestStatus
    {
        public RequestState State { get; set; }

        public string StateText { get { return State.ToString(); } set { } }

        public int TotalCount { get; set; }

        public int ProcessedCount { get; set; }

        public string CurrentProcessingItem { get; set; }

        public DateTime RequestTime { get; set; }

        public string RequestTimeUI { get { return RequestTime.ToString("dd.MM.yyyy HH:mm:ss"); } }

        public RequestResultSet RequestResultSet { get; set; }

        public void SetStart(int totalCount)
        {
            State = RequestState.Requesting;
            this.TotalCount = totalCount;
            this.ProcessedCount = 0;
            this.RequestTime = DateTime.Now;
            this.RequestResultSet = new RequestResultSet();
        }

        public void AddRequestResult(RequestResult input)
        {
            if (input.IsSucceed)
            {
                RequestResultSet.SucceedList.Add(input);
            }
            else
            {
                RequestResultSet.FailedList.Add(input);
            }
        }

        public void SetFinished()
        {
            State = RequestState.Finished;
        }
    }


    public class RequestResultSet
    {
        public List<RequestResult> SucceedList { get; set; }

        public List<RequestResult> FailedList { get; set; }

        public RequestResultSet()
        {
            SucceedList = new List<RequestResult>();
            FailedList = new List<RequestResult>();
        }
    }


    public class RequestModel : DasBaseModel
    {
        public RequestSession SessionData { get; set; }

        public RequestStatus CurrentStatus { get; set; }
    }

    public class RequestSession
    {
        public string LastRequestPns { get; set; }
    }

    public enum RequestState
    {
        
        Idle       ,
        Requesting ,
        Finished   ,
        Failed     ,
    }
}
