using System;

namespace ValorAPI.Lib.Connection.RateLimiter
{
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

        public static void UnsetRateLimiter(this Client client, RateLimiterSettings rateLimiterSettings)
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
