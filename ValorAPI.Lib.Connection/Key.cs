using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ValorAPI.Lib.Data.Constant;

namespace ValorAPI.Lib.Connection
{
    public class Key
    {
        public readonly Region Region;
        public readonly string ApiKey;
        public DateTime LastUsed= DateTime.MinValue;

        public Key(Region region, string apiKey)
        {
            this.Region = region;
            this.ApiKey = apiKey;
        }

        public static bool IsValid(string apiKey)
        {
            if(string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentException("You must supply a valid API key", nameof(apiKey));
            }

            var regex = new Regex(@"^RGAPI-[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$", RegexOptions.Compiled);
            var match = regex.Match(apiKey);

            return match.Success;
        }
    }
}
