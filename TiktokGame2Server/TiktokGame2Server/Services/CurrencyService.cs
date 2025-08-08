
using Microsoft.EntityFrameworkCore;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class CurrencyService : ICurrencyService
    {
        private readonly MyDbContext _dbContext;
        private readonly TiktokConfigService tiktokConfigService;
        public CurrencyService(MyDbContext dbContext, TiktokConfigService tiktokConfigService)
        {
            _dbContext = dbContext;
            this.tiktokConfigService = tiktokConfigService ?? throw new ArgumentNullException(nameof(tiktokConfigService));
        }

        public async Task<Currency> GetCurrency(int playerId, CurrencyType currencyType)
        {
            var currency = await _dbContext.Currencies.FirstOrDefaultAsync(c => c.PlayerId == playerId && c.CurrencyType == currencyType);
            if (currency == null)
            {
                // 如果没有找到，创建一个新的货币记录，初始值为0
                currency = new Currency
                {
                    PlayerId = playerId,
                    CurrencyType = currencyType,
                    Count = 0
                };
                _dbContext.Currencies.Add(currency);
                await _dbContext.SaveChangesAsync();
            }
            return currency;
        }

        public async Task<Currency> AddCurrency(int playerId, CurrencyType currencyType, int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount to add must be positive.");
            var currency = await GetCurrency(playerId, currencyType);
            currency.Count += amount;
            _dbContext.Currencies.Update(currency);
            await _dbContext.SaveChangesAsync();
            return currency;
        }

        public async Task<Currency> SpendCurrency(int playerId, CurrencyType currencyType, int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount to spend must be positive.");
            var currency = await GetCurrency(playerId, currencyType);
            if(currency.Count < amount)
                throw new InvalidOperationException($"Not enough {currencyType.ToString()}");
            currency.Count -= amount;

            _dbContext.Currencies.Update(currency);
            await _dbContext.SaveChangesAsync();
            return currency;
        }

        public async Task<bool> HasEnoughCurrency(int playerId, CurrencyType currencyType, int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount to check must be positive.");
            var currency = await GetCurrency(playerId, currencyType);
            return currency.Count >= amount;
        }
    }
}
