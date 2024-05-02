
using TopUpDB.Interface;

namespace BalanceAPI.Services
{
    public class BalanceService : IBalanceService
    {
                private readonly IBalance _iBalance;
        
        public BalanceService(IBalance iBalance)
        {
            _iBalance = iBalance;
        }
        public async Task<decimal> GetUserCurrentBalance(long userId)
        {
            return await _iBalance.GetUserAvailableBalance(userId);
        }

        public async Task<bool> TopUpBalance(long userId, decimal amount)
        {
            return await _iBalance.TopUpBalance(userId, amount);
        }

        public async Task<bool> UpdateBalance(long userId, decimal amount)
        {
            return await _iBalance.UpdateBalance(userId,amount);
        }
    }
}
