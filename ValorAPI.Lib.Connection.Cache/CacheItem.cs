using System;
using System.Collections.Generic;
using System.Text;

namespace ValorAPI.Lib.Connection.Cache
{
    public class CacheItem
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public byte[] Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
