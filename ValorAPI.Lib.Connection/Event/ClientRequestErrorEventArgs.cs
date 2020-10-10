using System;
using System.Collections.Generic;
using System.Text;

namespace ValorAPI.Lib.Connection.Event
{
    public class ClientRequestErrorEventArgs : EventArgs
    {
        public int StatusCode { get; private set; }
        public string Message { get; private set; }

        public ClientRequestErrorEventArgs(int statusCode, string message)
        {
            this.StatusCode = statusCode;
            this.Message = message;
        }
    }
}
