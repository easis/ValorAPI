using Newtonsoft.Json;

namespace ValorAPI.Lib.Data.Endpoint.Content
{
    [EndpointType(EndpointType.Query), EndpointPath("content", "contents")]
    public class ContentsEndpoint : IEndpoint
    {
        public string locale;
    }
}
