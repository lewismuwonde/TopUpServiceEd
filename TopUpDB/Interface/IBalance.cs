using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUpDB.Interface
{
    public interface IBalance
    {
        Task<bool> UpdateBalance(long userId, decimal amount);
        Task<decimal> GetUserAvailableBalance(long userId);
        Task<bool> TopUpBalance(long userId, decimal amount);
    }
}
