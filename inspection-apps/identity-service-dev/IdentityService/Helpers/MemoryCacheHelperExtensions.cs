using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Helpers
{
    public static class MemoryCacheHelperExtensions
    {

        public static void SetInt32(this IDistributedCache _distributedCache, string key, int value, DistributedCacheEntryOptions options)
        {
            var bytes = new byte[]
            {
                (byte)(value >> 24),
                (byte)(0xFF & (value >> 16)),
                (byte)(0xFF & (value >> 8)),
                (byte)(0xFF & value)
            };
            _distributedCache.Set(key, bytes, options);
        }

        public static int? GetInt32(this IDistributedCache _distributedCache, string key)
        {
            var data = _distributedCache.Get(key);
            if (data == null || data.Length < 4)
            {
                return null;
            }
            return data[0] << 24 | data[1] << 16 | data[2] << 8 | data[3];
        }
    }
}
