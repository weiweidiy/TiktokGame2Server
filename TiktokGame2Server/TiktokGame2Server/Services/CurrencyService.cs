
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

        public async Task<Currency> GetCurrency(int playerId)
        {
            var currency = await _dbContext.Currencies.FirstOrDefaultAsync(c => c.PlayerId == playerId);
            if (currency == null)
            {
                // 如果没有找到，创建一个新的货币记录，初始值为0
                currency = new Currency
                {
                    PlayerId = playerId,
                    Coin = 0,
                    Pan = 0
                };
                _dbContext.Currencies.Add(currency);
                await _dbContext.SaveChangesAsync();
            }
            return currency;
        }

        public async Task<Currency> AddCurrency(int playerId, CurrencyType currencyType, int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount to add must be positive.");
            var currency = await GetCurrency(playerId);
            switch (currencyType)
            {
                case CurrencyType.Coin:
                    currency.Coin += amount;
                    break;
                case CurrencyType.Pan:
                    currency.Pan += amount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currencyType), "Invalid currency type.");
            }
            _dbContext.Currencies.Update(currency);
            await _dbContext.SaveChangesAsync();
            return currency;
        }

        public async Task<Currency> SpendCurrency(int playerId, CurrencyType currencyType, int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount to spend must be positive.");
            var currency = await GetCurrency(playerId);
            switch (currencyType)
            {
                case CurrencyType.Coin:
                    if (currency.Coin < amount)
                        throw new InvalidOperationException("Not enough coins.");
                    currency.Coin -= amount;
                    break;
                case CurrencyType.Pan:
                    if (currency.Pan < amount)
                        throw new InvalidOperationException("Not enough pans.");
                    currency.Pan -= amount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currencyType), "Invalid currency type.");
            }
            _dbContext.Currencies.Update(currency);
            await _dbContext.SaveChangesAsync();
            return currency;
        }

        public async Task<bool> HasEnoughCurrency(int playerId, CurrencyType currencyType, int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount to check must be positive.");
            var currency = await GetCurrency(playerId);
            return currencyType switch
            {
                CurrencyType.Coin => currency.Coin >= amount,
                CurrencyType.Pan => currency.Pan >= amount,
                _ => throw new ArgumentOutOfRangeException(nameof(currencyType), "Invalid currency type."),
            };
        }
    }
}
