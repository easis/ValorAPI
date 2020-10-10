using System;

namespace ValorAPI.Lib.Data.Endpoint
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RequiredAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field)]
    public class OptionalAttribute : Attribute { }
}
