
namespace BalanceAPI.Services
{
    public interface IBalanceService
    {       
        Task<decimal> GetUserCurrentBalance(long userId);
        Task<bool> UpdateBalance(long userId, decimal amount);
        Task<bool> TopUpBalance(long userId, decimal amount);
    }
}
