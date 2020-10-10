using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Security.Cryptography;
using System.Text;
using ValorAPI.Lib.Connection.Exception;
using ValorAPI.Lib.Data;
using ValorAPI.Lib.Data.Constant;

namespace ValorAPI.Lib.Connection
{
    public class Keyring
    {
        public static bool ValidateKeys = true;
        private Dictionary<string, Dictionary<string, Key>> keys = new Dictionary<string, Dictionary<string, Key>>();

        public Keyring() { }

        public Keyring(params Key[] keys)
        {
            if(keys == null || keys.Length == 0)
            {
                throw new ArgumentException("You must supply a valid region-key tuple", nameof(keys));
            }

            foreach(var key in keys)
            {
                if(key.Region == null || string.IsNullOrEmpty(key.Region.Name))
                {
                    continue;
                }

                if(!this.keys.ContainsKey(key.Region.Name))
                {
                    this.keys[key.Region.Name] = new Dictionary<string, Key>();
                }

                this.keys[key.Region.Name][key.ApiKey] = key;
            }
        }

        public bool AddKey(Key key)
        {
            if(key == null)
            {
                throw new ArgumentException("You must supply a valid key", nameof(key));
            }

            return this.AddKey(key.Region, key.ApiKey);
        }

        public bool AddKey(Region region, string apiKey)
        {
            if(region == null || string.IsNullOrEmpty(region.Name))
            {
                throw new ArgumentException("You must supply a valid region", nameof(region));
            }

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentException("You must supply a valid API key", nameof(apiKey));
            }

            var regionName = region.Name;
            if(Keyring.ValidateKeys && !Key.IsValid(apiKey))
            {
                throw new InvalidApiKeyException();
            }

            if (!this.keys.ContainsKey(regionName))
            {
                this.keys[regionName] = new Dictionary<string, Key>();
            } else if(this.keys[regionName].ContainsKey(apiKey))
            {
                return false;
            }

            var key = new Key(region, apiKey);
            this.keys[regionName][apiKey] = key;

            return true;
        }

        public int Count()
        {
            int total = 0;

            foreach(var kvp in this.keys)
            {
                total += kvp.Value.Count;
            }

            return total;
        }

        public List<Key> GetKeysFromRegion(Region region)
        {
            if(region == null || string.IsNullOrEmpty(region.Name))
            {
                throw new ArgumentException("You must supply a valid region", nameof(region));
            }

            if(!this.keys.ContainsKey(region.Name))
            {
                throw new RegionNotFoundException();
            }

            var keys = new List<Key>();
            foreach (var k in this.keys[region.Name])
            {
                keys.Add(k.Value);
            }

            return keys;
        }
    }
}
