using System;
using System.Threading.Tasks;
using ValorAPI.Lib.Data.Endpoint;
using ValorAPI.Lib.Connection.Error;
using Newtonsoft.Json;
using System.Net;
using ValorAPI.Lib.Data.DTO;
using ValorAPI.Lib.Data.Constant;
using System.Net.Http;
using ValorAPI.Lib.Connection.Event;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;

namespace ValorAPI.Lib.Connection
{
    public partial class Client
    {
        /// <summary>
        /// Base URL of the request.
        /// </summary>
        public string BaseUrl { get; set; } = "api.riotgames.com";

        /// <summary>
        /// Short name of the game, or the first part of the host.
        /// </summary>
        public string GameName { get; set; } = Data.Constant.GameName.VALORANT;

        /// <summary>
        /// Version of API
        /// </summary>
        public string Version { get; set; } = "v1";

        /// <summary>
        /// Region that will be requested.
        /// <seealso cref="ValorAPI.Lib.Data.Constant.Region"/>
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// API key to be used in the request.
        /// </summary>
        public string Key 
        {  
            set => this.HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Riot-Token", value);
        }

        /// <summary>
        /// HttpClient object that will make the requests. If you want to set a proxy, 
        /// change any header or something you can do it right after the constructor.
        /// </summary>
        public HttpClient HttpClient { get; private set; }

        /// <summary>
        /// Creates an instance of <see cref="Client"/>.
        /// </summary>
        /// <param name="region"><see cref="Region"/></param>
        /// <param name="key"><see cref="Key"/></param>
        public Client(string region, string key)
        {
            this.HttpClient = new HttpClient();
            this.HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "application/x-www-form-urlencoded; charset=UTF-8");
            this.HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate, br");
            this.HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "https://github.com/easis/ValorAPI :)");

            this.Region = region;
            this.Key = key;
        }

        /// <summary>
        /// Build the URL used in the API request.
        /// </summary>
        /// <param name="endpoint">Endpoint object containing the information to build the request, as the endpoint path, endpoint name and possible queries.</param>
        /// <returns>The complete URL</returns>
        public string BuildUrl(IEndpoint endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentException("You must supply a valid endpoint", nameof(endpoint));
            }

            if(string.IsNullOrEmpty(this.BaseUrl))
            {
                throw new ArgumentException("You must supply a valid base URL", nameof(this.BaseUrl));
            }

            var endpointPathAttribute = (EndpointPathAttribute) Attribute.GetCustomAttribute(endpoint.GetType(), typeof(EndpointPathAttribute));
            var endpointTypeAttribute = (EndpointTypeAttribute)Attribute.GetCustomAttribute(endpoint.GetType(), typeof(EndpointTypeAttribute));

            var uri = new UriBuilder();
            uri.Scheme = "https"; // Always HTTPS for extra security. Not negotiable.
            uri.Host = string.Join('.', this.Region, this.BaseUrl); // e.g: eu.api.riotgames.com

            var queries = new List<string>();
            var pathParts = new List<string>();

            pathParts.AddRange(new string[] {
                this.GameName, endpointPathAttribute.Path ?? string.Empty, this.Version, endpointPathAttribute.Name ?? string.Empty
            });

            var properties = endpoint.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(endpoint)?.ToString();

                // if the property have the QueryNameAttribute then it will be necessarily a query parameter
                var queryNameAttribute = (QueryNameAttribute)Attribute.GetCustomAttribute(property.GetType(), typeof(QueryNameAttribute));
                if(queryNameAttribute != null && !string.IsNullOrEmpty(queryNameAttribute.Name))
                {
                    propertyName = queryNameAttribute.Name;
                }

                // if no endpoint type is specified we default to type Path (most common type)
                if(endpointTypeAttribute == null || endpointTypeAttribute.Type == EndpointType.Path)
                {
                    pathParts.Add(propertyValue);
                } else if(endpointTypeAttribute.Type == EndpointType.Query)
                {
                    queries.Add($"{propertyName}={propertyValue}");
                }
            }

            uri.Path = string.Join('/', pathParts.ToArray() ); // e.g: val/contents/v1/content
            uri.Query = string.Join('&', queries.ToArray()); // e.g: locale=es-ES&foo=bar

            return uri.ToString();
        }

        /// <summary>
        /// Creates and execute a request against the API, building the URL based on the endpoint.
        /// </summary>
        /// <typeparam name="T">Class type inherited from <see cref="IResponse"/> to which will be convert the response</typeparam>
        /// <param name="endpoint"><see cref="IEndpoint"/> object from which will built the request</param>
        /// <returns>An object of type <typeparamref name="T"/> filled with response values</returns>
        public async Task<T> GetAsync<T>(IEndpoint endpoint) where T : IResponse
        {
            if (endpoint == null)
            {
                throw new ArgumentException("You must supply a valid endpoint", nameof(endpoint));
            }

            var url = this.BuildUrl(endpoint);
            if (string.IsNullOrEmpty(url))
            {
                throw new InvalidUrlException();
            }

            var clientRequest = new ClientRequestEventArgs()
            {
                Url = url,
                Endpoint = endpoint
            };

            // fire a BeforeRequest event
            await this.OnBeforeRequestAsync(clientRequest);
            this.OnBeforeRequest(clientRequest);

            string response = string.Empty;
            if (string.IsNullOrEmpty(clientRequest.ResponseContent))
            {
                var httpResponse = await this.HttpClient.GetAsync(url);

                response = await httpResponse?.Content?.ReadAsStringAsync();
                clientRequest.ResponseContent = response;

                if (httpResponse.IsSuccessStatusCode)
                {
                    // fire a CompletedRequest event
                    await this.OnCompletedRequestAsync(clientRequest);
                    this.OnCompletedRequest(clientRequest);
                } else
                {
                    // if a non success status code (200) is returned then try to deserialize the error
                    var errorResponseDto = JsonConvert.DeserializeObject<ErrorResponseDto>(response);
                    var errorStatusResponseDto = errorResponseDto.status;

                    var clientRequestError = new ClientRequestErrorEventArgs(errorStatusResponseDto.status_code, errorStatusResponseDto.message);

                    // fire a ErrorRequest event
                    await this.OnErrorRequestAsync(clientRequestError);
                    this.OnErrorRequest(clientRequestError);

                    return default(T);
                }
            }
            else // an event supplied the response body (a cache?)
            {
                response = clientRequest.ResponseContent;
            }

            // fire a SuccessRequest event
            await this.OnSuccessRequestAsync(clientRequest);
            this.OnSuccessRequest(clientRequest);

            var responseDto = JsonConvert.DeserializeObject<T>(response);
            return responseDto;
        }
    }
}
