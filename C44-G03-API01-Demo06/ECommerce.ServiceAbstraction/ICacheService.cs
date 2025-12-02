using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServicesAbstraction
{
    public interface ICacheService
    {
        Task<string> GetAsync(string Cachekey);
        Task SetAsync(string Cachekey, object CacheValue,TimeSpan TimeToLive);
    }
}
