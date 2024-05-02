using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUpDB.Entity;
using TopUpDB.Interface;

namespace TopUpDB.Implementation
{
    public class BeneficiaryImplementation : IBeneficiary
    {
        public AppDBContext _db;
        public BeneficiaryImplementation(AppDBContext db)
        {
            _db = db;
        }

        public async Task<TopUpBeneficiary> AddBeneficiary(TopUpBeneficiary beneficiary)
        {
            var addedEntry = await _db.TopUpBeneficiaries.AddAsync(beneficiary);
            await _db.SaveChangesAsync();
            return addedEntry.Entity;

        }

        public async Task<List<TopUpBeneficiary>> GetActiveBeneficiaries(int userId)
        {
            return await _db.TopUpBeneficiaries.Where(c=> c.UserId== userId && c.IsActive == true).ToListAsync();
        }

        public async Task<int> GetActiveBeneficiariesCount(int userId)
        {
            return await  _db.TopUpBeneficiaries.Where(c => c.UserId == userId && c.IsActive == true).CountAsync();
        }

        public async Task<TopUpBeneficiary> GetBeneficiaryById(int beneficiaryId)
        {
            return await _db.TopUpBeneficiaries.FirstOrDefaultAsync(c => c.Id == beneficiaryId);
        }
    }
}
