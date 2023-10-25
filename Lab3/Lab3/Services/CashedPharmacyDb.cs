using Microsoft.Extensions.Caching.Memory;
using static Lab3.Services.CashedPharmacyDb;

namespace Lab3.Services
{
    public class CashedPharmacyDb
    {
        
        private readonly PharmacyContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        private readonly int _saveTime;

        public CashedPharmacyDb(PharmacyContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
            _saveTime = 3 * 2 + 240;
        }

        public void AddIncomingToCache(CacheKey key, int rowsNumber = 100)
        {
            if (!_memoryCache.TryGetValue(key, out IEnumerable<Incoming> cachedOffice))
            {
                cachedOffice = _dbContext.Incomings.Take(rowsNumber).ToList();

                if (cachedOffice != null)
                {
                    _memoryCache.Set(key, cachedOffice, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_saveTime)
                    });
                }
                Console.WriteLine("Таблица Incoming занесена в кеш");
            }
            else
            {
                Console.WriteLine("Таблица Incoming уже находится в кеше");
            }
        }

        public IEnumerable<Producer> GetEmployee(CacheKey key, int rowsNumber = 20)
        {
            IEnumerable<Producer> employes;
            if (!_memoryCache.TryGetValue(key, out employes))
            {
                employes = _dbContext.Producers.Take(rowsNumber).ToList();
                if (employes != null)
                {
                    _memoryCache.Set(key, employes,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
                }
            }
            return employes;
        }

        public IEnumerable<Incoming> GetSubscription(CacheKey key, int rowsNumber = 20)
        {
            IEnumerable<Incoming> subscriptions;
            if (!_memoryCache.TryGetValue(key, out subscriptions))
            {
                subscriptions = _dbContext.Incomings.Take(rowsNumber).ToList();
                if (subscriptions != null)
                {
                    _memoryCache.Set(key, subscriptions,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
                }
            }
            return subscriptions;
        }
    }
}
