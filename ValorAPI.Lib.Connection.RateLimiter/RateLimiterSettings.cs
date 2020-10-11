using ComposableAsync;
using RateLimiter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ValorAPI.Lib.Connection.Event;

namespace ValorAPI.Lib.Connection.RateLimiter
{
    public class RateLimiterSettings
    {
        private List<IAwaitableConstraint> timeLimiters;

        public RateLimiterSettings()
        {
            this.timeLimiters = new List<IAwaitableConstraint>();
        }
        
        public void AddRateLimit(IAwaitableConstraint constraint)
        {
            if(constraint == null)
            {
                throw new ArgumentException("You must supply a valid constraint", nameof(constraint));
            }

            this.timeLimiters.Add(constraint);
        }

        public void AddRateLimit(int amount, TimeSpan time)
        {
            if(amount < 0)
            {
                throw new ArgumentException("You must supply a valid amount", nameof(amount));
            }

            if(time == null)
            {
                throw new ArgumentException("You must supply a valid time", nameof(time));
            }

            var constraint = new CountByIntervalAwaitableConstraint(amount, time);
            this.timeLimiters.Add(constraint);
        }

        public void RemoveRateLimit(IAwaitableConstraint constraint)
        {
            if(constraint == null)
            {
                throw new ArgumentException("You must supply a valid constraint", nameof(constraint));
            }

            this.timeLimiters.Remove(constraint);
        }

        public void ClearRateLimit()
        {
            this.timeLimiters.Clear();
        }

        public void EnableRateLimiter(Client client)
        {
            if(client == null)
            {
                throw new ArgumentException("You must supply a valid client", nameof(client));
            }

            client.BeforeRequestAsync += this.OnBeforeRequestAsync;
        }

        public void DisableRateLimiter(Client client)
        {
            if (client == null)
            {
                throw new ArgumentException("You must supply a valid client", nameof(client));
            }

            client.BeforeRequestAsync -= this.OnBeforeRequestAsync;
        }

        private async Task OnBeforeRequestAsync(object sender, EventArgs e)
        {
            if(sender == null || e == null)
            {
                return;
            }

            var clientRequest = e as ClientRequestEventArgs;
            if (string.IsNullOrEmpty(clientRequest.ResponseContent)) // la respuesta no está cacheada
            {
                await TimeLimiter.Compose(this.timeLimiters.ToArray());
            }
        }
    }

    public static class ClientExtension
    {
        public static void SetRateLimiter(this Client client, RateLimiterSettings rateLimiterSettings)
        {
            if (client == null)
            {
                throw new ArgumentException("You must supply a valid client", nameof(client));
            }

            if (rateLimiterSettings == null)
            {
                throw new ArgumentException("You must supply a valid RateLimiterSettings", nameof(rateLimiterSettings));
            }

            rateLimiterSettings.EnableRateLimiter(client);
        }

        public static void nRateLimiter(this Client client, RateLimiterSettings rateLimiterSettings)
        {
            if (client == null)
            {
                throw new ArgumentException("You must supply a valid client", nameof(client));
            }

            if (rateLimiterSettings == null)
            {
                throw new ArgumentException("You must supply a valid RateLimiterSettings", nameof(rateLimiterSettings));
            }

            rateLimiterSettings.DisableRateLimiter(client);
        }
    }
}
