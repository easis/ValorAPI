namespace ValorAPI.Lib.Data.Exception.Response
{
    public class UnauthorizedException : IResponseException
    {
        public override int StatusCode => 401;
    }
}
