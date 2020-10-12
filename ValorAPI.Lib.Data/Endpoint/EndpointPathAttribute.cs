using System;

namespace ValorAPI.Lib.Data.Endpoint
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EndpointPathAttribute : System.Attribute
    {
        public string Path { get; private set; }
        public string Name { get; private set; }

        public EndpointPathAttribute(string path, string name)
        {
            this.Path = path;
            this.Name = name;
        }
    }
}