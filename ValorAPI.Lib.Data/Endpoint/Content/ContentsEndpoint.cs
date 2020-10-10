using System;
using ValorAPI.Lib.Data.Constant;
using ValorAPI.Lib.Data.DTO.Content;

namespace ValorAPI.Lib.Data.Endpoint.Content
{
    public class ContentsEndpoint : IEndpoint
    {
        public override string Name => "contents";
        public override string Path => "content";

        [Optional]
        public Locale locale;
    }
}
