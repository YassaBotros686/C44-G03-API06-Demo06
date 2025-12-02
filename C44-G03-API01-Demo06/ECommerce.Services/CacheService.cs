using ECommerce.Domain.Contracts;
using ECommerce.ServicesAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class CacheService : ICacheService
    {
        private readonly ICacheRepository _cacheRepository;

        public CacheService(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }

        public async Task<string> GetAsync(string Cachekey)
        {
            return await _cacheRepository.GetAsync(Cachekey);
        }

        public async Task SetAsync(string Cachekey, object CacheValue, TimeSpan TimeToLive)
        {
            var Value = JsonSerializer.Serialize(CacheValue);
            await _cacheRepository.SetAsync(Cachekey, Value, TimeToLive);
        }
    }
}
