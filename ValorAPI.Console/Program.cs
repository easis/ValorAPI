using RateLimiter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ValorAPI.Lib.Connection;
using ValorAPI.Lib.Connection.Cache;
using ValorAPI.Lib.Connection.Event;
using ValorAPI.Lib.Connection.RateLimiter;
using ValorAPI.Lib.Data.Constant;
using ValorAPI.Lib.Data.DTO.Content;
using ValorAPI.Lib.Data.Endpoint.Content;

namespace ValorAPI.Console
{
    class Program
    {
        async static Task Main(string[] args)
        {
            Debug.WriteLine($"[~] {System.AppDomain.CurrentDomain.FriendlyName} - START");

            var region = Region.EUROPE;
            var key = "RGAPI-0b24e053-e97d-47b9-a725-d3cd9c9bf3a8";

            var client = new Client(region, key);
            client.SetCache(@"P:\C#\cache.db");

            var rateLimiterSettings = new RateLimiterSettings();
            rateLimiterSettings.AddRateLimit(
                (IAwaitableConstraint) TimeLimiter.GetFromMaxCountByInterval(20, TimeSpan.FromSeconds(1)),
                (IAwaitableConstraint)TimeLimiter.GetFromMaxCountByInterval(100, TimeSpan.FromMinutes(2))
            );
            rateLimiterSettings.EnableRateLimiter(client);

            Debug.WriteLine($"[I] Current region: {region}");

            client.CompletedRequest += (object sender, EventArgs e) => {
                var clientRequest = e as ClientRequestEventArgs;
                Debug.WriteLine($"[CompletedRequest] {clientRequest.ResponseContent}");
            };

            client.ErrorRequest += (object sender, EventArgs e) =>
            {
                var clientRequestError = e as ClientRequestErrorEventArgs;
                Debug.WriteLine($"[E] ({clientRequestError.StatusCode}) {clientRequestError.Message}");
            };

            var contentsEndpoint = new ContentsEndpoint();
            var contentResponse = await client.GetAsync<ContentDto>(contentsEndpoint);
            Debug.WriteLine($"version: {contentResponse.version}");

            Debug.WriteLine($"[~] {System.AppDomain.CurrentDomain.FriendlyName} - END");
        }
    } 
}
