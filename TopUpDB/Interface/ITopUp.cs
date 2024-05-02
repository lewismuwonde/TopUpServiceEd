using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUpDB.Interface
{
    public interface ITopUp
    {        
        Task<decimal> GetTotalTopUpsForUserAndBeneficiaryAndMonth(long userId, long beneficiaryId, int currentMonth);
        Task<decimal> GetTotalTopUpsForUserAndMonth(long userId, int currentMonth);
        Task<bool> ProcessTopUp(long userId, long beneficiaryId, decimal amount);
    }
}
