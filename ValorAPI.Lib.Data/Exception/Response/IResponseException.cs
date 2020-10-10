namespace ValorAPI.Lib.Data.Exception
{
    public abstract class IResponseException : System.Exception
    {
        public abstract int StatusCode { get; }

        public IResponseException() { }
        public IResponseException(string message) : base(message) { }
        public IResponseException(string message, System.Exception innerException) : base(message, innerException) { }
    }
}
