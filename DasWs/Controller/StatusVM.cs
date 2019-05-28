using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DasWs.Controller
{
    public class RegisterVM
    {
        public RegisterVM()
        {

        }


        public bool IsStatusOK { get; set; }

        private bool isValueUnknown = true;
        public bool IsValueUnknown { get { return HandleMsg == "Unknown"; } set { isValueUnknown = value; } }

        public uint HandleNo { get; set; }

        public string Name { get; set; }

        public string HandleValueType { get; set; }

        public string ValueUnit { get; set; }

        public string HandleMsg { get; set; }


    }
}
