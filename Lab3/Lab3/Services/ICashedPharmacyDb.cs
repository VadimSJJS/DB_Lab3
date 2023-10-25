using Lab3.View;

namespace Lab3.Services
{
    public interface ICashedPharmacyDb
    {
        void AddIncomingToCache(CacheKey key, int rowsNumber = 100);
        IEnumerable<Producer> GetIncoming(CacheKey key, int rowsNumber = 100);
    }
}
