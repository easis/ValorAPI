using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ValorAPI.Lib.Connection.Event;

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

        public static async void EnableCache(this Client client)
        {
            client.BeforeRequestAsync += OnBeforeRequestAsync;
            client.CompletedRequestAsync += OnCompletedRequestAsync;
        }

        public static void DisableCache(this Client client)
        {
            client.BeforeRequestAsync -= OnBeforeRequestAsync;
            client.CompletedRequestAsync -= OnCompletedRequestAsync;
        }

        private static async Task OnBeforeRequestAsync(object sender, ClientRequestEventArgs e)
        {
            var clientRequest = e as ClientRequestEventArgs;

            using var cacheContext = new CacheContext();
            var item = cacheContext
                        .CacheItems
                        .Where(i => i.Url == clientRequest.Url)
                        .Select(i => i.Content)
                        .AsSingleQuery();


            var content = item.FirstOrDefault();
            if(content == null)
            {
                return;
            }

            //var contentText = await Compression.DecompressAsync(content);
            var contentText = StringCompressor.DecompressString(content);
            clientRequest.ResponseContent = contentText;
        }

        private static async Task OnCompletedRequestAsync(object sender, ClientRequestEventArgs clientRequest)
        {
            //var content = await Compression.CompressAsync(clientRequest.ResponseContent);
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
