using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServer.Infrasturcture.MVC;
using Utilities.IO.Logging.Interfaces;
using Utilities.IO.Logging;

namespace DasWs.CommonCS
{
    public abstract class DasControllerBase<T,S> : ControllerBase<T,S>
        where T : ModelBase
        where S : class, new ()
    {


        public override ActionRet ErrorHappenedCallback(Exception exp, string type)
        {
            return ActionRet;
        }
    }
}
