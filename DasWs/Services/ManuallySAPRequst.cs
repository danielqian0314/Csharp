using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using das_settings;
using das_common.SOAP;
using DasWs.Services.ExchangeDataType;
using Utilities.IO.Logging.Interfaces;

namespace DasWs.Services
{
    public class ManuallySAPRequst
    {
        private readonly Isap_push_to_das sapPushSettings;
        private readonly SoapClient soapClient;
        private readonly SoapRetSaveSvc saveSvc;
        private readonly ILog logger;

        public ManuallySAPRequst(Isap_push_to_das sapPushSettings, SoapClient soapClient, SoapRetSaveSvc saveSvc,ILog logger)
        {
            this.logger = logger;
            this.saveSvc = saveSvc;
            this.soapClient = soapClient;
            this.sapPushSettings = sapPushSettings;
        }

        public RequestResult DoRequest(string pn)
        {
            SoapLabelPrintResult soapRet = null;
            Exception soapExp = null;

            try
            {
                soapRet = soapClient.GetLabelPrintResult(pn);
            }
            catch (Exception exp)
            {
                soapExp = exp;
                return SoapRequestException(pn,exp);
            }

            if (soapRet != null && soapRet.IsNoError)
            {
                this.saveSvc.SaveToLocal(soapRet);                
            }
    
            return GenRequestResultFromSoapResult(pn, soapRet);           
        }

        private RequestResult SoapRequestException(string pn, Exception exp)
        {
            this.logger.LogException(exp, LogType.Error, "SAP");

            return new RequestResult 
            {
                Pn = pn,
                IsSucceed = false,
                ErrorCode = "SOAP_ERROR",
                ExceptionMsg = exp.ToString()
            };
        }

        private RequestResult GenRequestResultFromSoapResult(string pn, SoapLabelPrintResult soapRet)
        {
            RequestResult ret = new RequestResult();

            ret.Pn = pn;

            ret.IsSucceed = soapRet.IsNoError;

            ret.ErrorCode = soapRet.ErrorType;
            ret.ExceptionMsg = soapRet.ErrorMessage;

            return ret;
        }

      
    }
}
