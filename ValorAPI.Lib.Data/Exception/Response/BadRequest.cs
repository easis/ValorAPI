namespace ValorAPI.Lib.Data.Exception.Response
{
    public class BadRequest : IResponseException
    {
        public override int StatusCode => 400;
    }
}
