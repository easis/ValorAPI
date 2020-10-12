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

namespace ValorAPI.Lib.Connection
{
    public partial class Client
    {
        /// <summary>
        /// URL base con la cual se construirá la URL para la petición.
        /// </summary>
        public const string BaseUrl = "api.riotgames.com";

        /// <summary>
        /// Nombre corto del juego que se incluirá en la petición.
        /// </summary>
        public const string GameName = "val";

        /// <summary>
        /// Versión de la API a utilizar
        /// </summary>
        public const string Version = "v1";

        /// <summary>
        /// Región hacia donde se realizará la petición.
        /// <seealso cref="ValorAPI.Lib.Data.Constant.Region"/>
        /// </summary>
        public Region Region { get; private set; }

        public string Key { get; private set; }

        public JsonSerializerSettings Serializer { get; set; }
        public Client(Region region, string key)
        {
            this.Region = region;
            this.Key = key;

            this.Serializer = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None
            };
        }

        /// <summary>
        /// Crea la URL usada para la conexión a la API
        /// </summary>
        /// <param name="endpoint">Objeto endpoint con el cual se construirá la URL</param>
        /// <returns>La URL completa del endpoint</returns>
        public string BuildUrl(IEndpoint endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentException("You must supply a valid endpoint", nameof(endpoint));
            }

            if (this.Region == null)
            {
                throw new ArgumentException("You must pass a valid region", nameof(this.Region));
            }

            string url = $"https://{this.Region.Name.ToLower()}.{Client.BaseUrl.ToLower()}/{Client.GameName.ToLower()}/{endpoint.Path.ToLower()}/{Client.Version.ToLower()}/{endpoint.Name.ToLower()}";
            return url;
        }

        private HttpClient HttpClient = new HttpClient();

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

            var requestBody = JsonConvert.SerializeObject(endpoint, this.Serializer);
            if(requestBody == "{}") // json vacío
            {
                requestBody = string.Empty;
            }

            var clientRequest = new ClientRequestEventArgs()
            {
                Url = url,
                Endpoint = endpoint,
                RequestContent = requestBody
            };

            await this.OnBeforeRequestAsync(clientRequest);
            this.OnBeforeRequest(clientRequest);

            string response = string.Empty;

            // un evento ha proporcionado un cuerpo (cache?)
            if (string.IsNullOrEmpty(clientRequest.ResponseContent))
            {

                this.HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Riot-Token", this.Key);

                var httpResponse = await this.HttpClient.GetAsync(url);
                if (httpResponse.IsSuccessStatusCode)
                {
                    response = await httpResponse?.Content?.ReadAsStringAsync();

                    clientRequest.ResponseContent = response;

                    await this.OnCompletedRequestAsync(clientRequest);
                    this.OnCompletedRequest(clientRequest);
                } else
                {
                    response = await httpResponse?.Content?.ReadAsStringAsync();
                    var errorResponseDto = JsonConvert.DeserializeObject<ErrorResponseDto>(response);
                    var errorStatusResponseDto = errorResponseDto.status;

                    var clientRequestError = new ClientRequestErrorEventArgs(errorStatusResponseDto.status_code, errorStatusResponseDto.message);
                    await this.OnErrorRequestAsync(clientRequestError);
                    this.OnErrorRequest(clientRequestError);
                }
            } else
            {
                response = clientRequest.ResponseContent;
            }

            await this.OnSuccessRequestAsync(clientRequest);
            this.OnSuccessRequest(clientRequest);

            var responseDto = JsonConvert.DeserializeObject<T>(response);
            return responseDto;
        }
    }
}
