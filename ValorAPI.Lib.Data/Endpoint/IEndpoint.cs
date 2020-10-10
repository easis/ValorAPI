using System;

namespace ValorAPI.Lib.Data.Endpoint
{
    public abstract class IEndpoint
    {
        public abstract string Path { get; }

        public abstract string Name { get; }
    }
}
