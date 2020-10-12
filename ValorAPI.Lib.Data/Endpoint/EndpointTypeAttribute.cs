using System;

namespace ValorAPI.Lib.Data.Endpoint
{
    public enum EndpointType
    {
        Query, Path
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class EndpointTypeAttribute : System.Attribute
    {
        public EndpointType Type { get; private set; }

        public EndpointTypeAttribute(EndpointType endpointType)
        {
            this.Type = endpointType;
        }
    }
}
