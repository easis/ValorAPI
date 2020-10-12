using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ValorAPI.Lib.Connection.Event;
using ValorAPI.Lib.Data.DTO;
using ValorAPI.Lib.Data.Endpoint;

namespace ValorAPI.Lib.Connection.Cache
{
    public static class ClientExtension
    {
        public static async void SetCache(this Client client, string databaseName)
        {
            CacheContext.SetDatabaseName(databaseName);

            using (var cacheContext = new CacheContext())
            {
                await cacheContext.Database.EnsureCreatedAsync();
            }

            client.EnableCache();
        }

        public static void EnableCache(this Client client)
        {
            client.BeforeRequestAsync += OnBeforeRequestAsync;
            client.CompletedRequestAsync += OnCompletedRequestAsync;
        }

        public static void DisableCache(this Client client)
        {
            client.BeforeRequestAsync -= OnBeforeRequestAsync;
            client.CompletedRequestAsync -= OnCompletedRequestAsync;
        }

        public static async Task<T> GetAsyncUncached<T>(this Client client, IEndpoint endpoint) where T : IResponse
        {
            client.DisableCache();
            var response = await client.GetAsync<T>(endpoint);
            client.EnableCache();

            return response;
        }
        private static async Task OnBeforeRequestAsync(object sender, ClientRequestEventArgs e)
        {
            var client = sender as Client;
            var clientRequest = e as ClientRequestEventArgs;

            using var cacheContext = new CacheContext();
            var item = cacheContext
                        .CacheItems
                        .Where(i => i.Url.ToLower() == clientRequest.Url.ToLower())
                        .Select(i => i.Content);

            var content = item.FirstOrDefault();
            if (content == null)
            {
                return;
            }

            var contentText = StringCompressor.DecompressString(content);
            clientRequest.ResponseContent = contentText;
        }

        private static async Task OnCompletedRequestAsync(object sender, ClientRequestEventArgs clientRequest)
        {
            var content = StringCompressor.CompressString(clientRequest.ResponseContent);

            var cacheItem = new CacheItem()
            {
                Url = clientRequest.Url,
                Content = content
            };

            using var cacheContext = new CacheContext();
            await cacheContext.CacheItems.AddAsync(cacheItem);
            await cacheContext.SaveChangesAsync();
        }
    }
}
