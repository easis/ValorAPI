# ValorAPI

## Description
This is an on-going project to build a Valorant API in .NET. The goal is make a flexible API which will even allow to make request to another Riot games endpoints.

## Status
At the moment, only one endpoint is supported (Content) due the API access limitation to only a few entities.

## Features
- Basic requests
- (optional) Cache responses to SQLite database
- (optional) Rate limiter

## Example
Setup a client:
```csharp
var client = new Client(Region.EUROPE, "your api key");
```

Add the cache:
```csharp
client.SetCache(@"cache.db");
```

Set the rate limiters:
```csharp
var rateLimiterSettings = new RateLimiterSettings();
rateLimiterSettings.AddRateLimit(20, TimeSpan.FromSeconds(1));
rateLimiterSettings.AddRateLimit(100, TimeSpan.FromMinutes(2));
rateLimiterSettings.EnableRateLimiter(client);
```

There are 4 events available:
- **BeforeRequest**: fires when _GetAsync_ is called, before the call to the API is made.
- **CompletedRequest**: fires when an actual connection is made to the endpoint.
- **SuccessRequest**: fires when got a response (from connection or any other source, e.g **BeforeRequest** event).
- **ErrorRequest**: fires when the response is an error.

How to setup events:
```csharp
client.CompletedRequest += (object sender, EventArgs e) => {
    var clientRequest = e as ClientRequestEventArgs;
    Debug.WriteLine($"[CompletedRequest] {clientRequest.ResponseContent}");
};

client.ErrorRequest += (object sender, EventArgs e) =>
{
    var clientRequestError = e as ClientRequestErrorEventArgs;
    Debug.WriteLine($"[E] ({clientRequestError.StatusCode}) {clientRequestError.Message}");
};
```

Calling the endpoint:
```csharp
var contentsEndpoint = new ContentsEndpoint()
{
    locale = Locale.ES_ES // optional
};

var contentResponse = await client.GetAsync<ContentDto>(contentsEndpoint);
Debug.WriteLine($"First gamemode: {contentResponse.gameModes.First().name}");
```

Output:
`First gamemode: Estándar`