using Newtonsoft.Json;
using ValorAPI.Lib.Data.Endpoint.Attribute;

namespace ValorAPI.Lib.Data.Endpoint.Content
{
    [EndpointType(EndpointType.Query), EndpointPath("content"), EndpointName("contents")]
    public class ContentsEndpoint : IEndpoint
    {
        public string locale;
    }
}
