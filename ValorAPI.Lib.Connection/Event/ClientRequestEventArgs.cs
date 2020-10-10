using System;
using System.Collections.Generic;
using System.Text;

namespace ValorAPI.Lib.Connection.Event
{
    public class ClientRequestEventArgs : EventArgs
    {
        public string Url { get; set; }
        public string RequestContent { get; set; }
        public string ResponseContent { get; set; }
    }
}
