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
            var key = "RGAPI-a6dbecf6-e05b-4ca4-aa82-0eae6e5b666a";
            var databasePath = @"P:\C#\cache.db";

            var client = new Client(region, key);
            client.SetCache(databasePath);

            var rateLimiterSettings = new RateLimiterSettings();
            rateLimiterSettings.AddRateLimit(20, TimeSpan.FromSeconds(1));
            rateLimiterSettings.AddRateLimit(100, TimeSpan.FromMinutes(2));

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

            var contentsEndpoint = new ContentsEndpoint()
            {
                locale = Locale.ES_ES
            };

            var contentResponse = await client.GetAsync<ContentDto>(contentsEndpoint);
            Debug.WriteLine($"First gamemode: {contentResponse.gameModes.First().name}");

            Debug.WriteLine($"[~] {System.AppDomain.CurrentDomain.FriendlyName} - END");
        }
    } 
}
