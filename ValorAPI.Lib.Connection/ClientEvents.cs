using AsyncEvent;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ValorAPI.Lib.Connection
{

    public class ClientRequestEventArgs : EventArgs
    {
        public string Url { get; set; }
        public string Body { get; set; }
    }

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

    public partial class Client
    {
        #region BeforeRequest
        public event AsyncEventHandler<ClientRequestEventArgs> BeforeRequestAsync;
        private async Task OnBeforeRequestAsync(ClientRequestEventArgs e) => BeforeRequestAsync?.InvokeAsync(this, e);

        public event EventHandler BeforeRequest;
        protected virtual void OnBeforeRequest(ClientRequestEventArgs e)
        {
            BeforeRequest?.Invoke(this, e);
        }
        #endregion

        #region SuccessRequest
        public event AsyncEventHandler<ClientRequestEventArgs> SuccessRequestAsync;
        async private Task OnSuccessRequestAsync(ClientRequestEventArgs e)
        {
            SuccessRequestAsync?.InvokeAsync(this, e);
        }

        public event EventHandler SuccessRequest;
        protected virtual void OnSuccessRequest(ClientRequestEventArgs e)
        {
            SuccessRequest?.Invoke(this, e);
        }
        #endregion

        #region ErrorRequest
        public event AsyncEventHandler<ClientRequestErrorEventArgs> ErrorRequestAsync;
        async private Task OnErrorRequestAsync(ClientRequestErrorEventArgs e)
        {
            ErrorRequestAsync?.InvokeAsync(this, e);
        }

        public event EventHandler ErrorRequest;
        protected virtual void OnErrorRequest(ClientRequestErrorEventArgs e)
        {
            ErrorRequest?.Invoke(this, e);
        }
        #endregion

        #region CompletedRequest
        public event AsyncEventHandler<ClientRequestEventArgs> CompletedRequestAsync;
        async private Task OnCompletedRequestAsync(ClientRequestEventArgs e)
        {
            CompletedRequestAsync?.InvokeAsync(this, e);
        }

        public event EventHandler CompletedRequest;
        protected virtual void OnCompletedRequest(ClientRequestEventArgs e)
        {
            CompletedRequest?.Invoke(this, e);
        }
        #endregion
    }
}
