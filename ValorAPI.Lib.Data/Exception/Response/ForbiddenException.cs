namespace ValorAPI.Lib.Data.Exception.Response
{
    public class ForbiddenException : IResponseException
    {
        public override int StatusCode => 403;
    }
}
