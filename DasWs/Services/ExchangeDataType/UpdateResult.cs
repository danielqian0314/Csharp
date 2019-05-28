using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DasWs.Services.ExchangeDataType
{
    public class UpdateResult
    {
        public bool IsOK { get; set; }

        public string ErrorCode { get; set; }

        public string SoapErrorMsg { get; set; }

        public string LabelUpdateSource { get; set; }

        public string LabelFileName { get; set; }

        public string LabelFileBase64Content { get; set; }

        public CLPBasicData CLPBasicData { get; set; } 
    }
}
