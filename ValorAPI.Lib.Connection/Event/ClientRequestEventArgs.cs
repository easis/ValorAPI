using System;
using System.Collections.Generic;
using System.Text;
using ValorAPI.Lib.Data.Endpoint;

namespace ValorAPI.Lib.Connection.Event
{
    public class ClientRequestEventArgs : EventArgs
    {
        public string Url { get; set; }
        public string RequestContent { get; set; }
        public string ResponseContent { get; set; }
        public IEndpoint Endpoint { get; set; }
    }
}
