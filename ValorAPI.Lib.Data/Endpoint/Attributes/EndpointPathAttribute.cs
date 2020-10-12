using System;

namespace ValorAPI.Lib.Data.Endpoint.Attribute
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EndpointPathAttribute : System.Attribute
    {
        private string path;

        public EndpointPathAttribute(string path)
        {
            this.path = path;
        }
    }
}