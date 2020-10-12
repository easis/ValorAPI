using System;

namespace ValorAPI.Lib.Data.Endpoint.Attribute
{
    public enum EndpointType
    {
        Query, Path
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class EndpointTypeAttribute : System.Attribute
    {
        private EndpointType endpointType;

        public EndpointTypeAttribute(EndpointType endpointType)
        {
            this.endpointType = endpointType;
        }
    }
}
