using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DasWs.Services.ExchangeDataType
{
    public class RequestResult
    {
        public string Pn { get; set; }

        public bool IsSucceed { get; set; }

        public string ErrorCode { get; set; }

        public string ExceptionMsg { get; set; }
    }

}
