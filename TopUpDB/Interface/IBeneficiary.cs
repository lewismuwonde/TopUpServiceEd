using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUpDB.Entity;

namespace TopUpDB.Interface
{
    public interface IBeneficiary
    {     
        public Task<List<TopUpBeneficiary>> GetActiveBeneficiaries(int userId);
        public Task<TopUpBeneficiary> AddBeneficiary(TopUpBeneficiary request);
        public Task<int> GetActiveBeneficiariesCount(int userId);
        public Task<TopUpBeneficiary> GetBeneficiaryById(int beneficiaryId);
    }
}
