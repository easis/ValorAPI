using System;

namespace ValorAPI.Lib.Data.Endpoint.Attribute
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EndpointNameAttribute : System.Attribute
    {
        private string name;

        public EndpointNameAttribute(string name)
        {
            this.name = name;
        }
    }
}