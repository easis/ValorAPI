using System;

namespace ValorAPI.Lib.Data.Endpoint
{
    [AttributeUsage(AttributeTargets.Class)]
    public class QueryNameAttribute : System.Attribute
    {
        public string Name { get; private set; }

        public QueryNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}