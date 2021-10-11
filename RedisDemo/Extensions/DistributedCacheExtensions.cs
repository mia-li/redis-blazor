using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;


namespace RedisDemo.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache,
            string recordId, 
            T data,
            TimeSpan? absoluteExpireTime=null,
            TimeSpan? unusedExpireTime=null)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
            //if you don't set the expiretime,it'll default set only 1 minute.
            options.SlidingExpiration = unusedExpireTime;
            //if you set an expiretime,but you don't access it in slidingexprietime,then we can delete it and get new data the next time they ask.

            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsonData, options);
        }

        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache,string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);
            
            if(jsonData is null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
