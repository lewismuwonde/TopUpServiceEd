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
    public class TopUpImplementation : ITopUp
    {
        public AppDBContext _db;
        public TopUpImplementation(AppDBContext db)
        {
            _db = db;
        }

        public async Task<decimal> GetTotalTopUpsForUserAndBeneficiaryAndMonth(long userId, long beneficiaryId, int currentMonth)
        {
            try
            {
                var currentYear = DateTime.Now.Year;
                var startDate = new DateTime(currentYear, currentMonth, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var topUps = await _db.TopUpTransactions
                    .Where(t => t.UserId == userId && t.BeneficiaryId == beneficiaryId && t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                    .ToListAsync();

                if (topUps.Any())
                {
                    var totalTopUps = topUps.Sum(t => t.Amount);
                    return totalTopUps;
                }
                else
                {
                    return 0; 
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Exception occurred: {ex.Message}");
            }
        }


        public async Task<decimal> GetTotalTopUpsForUserAndMonth(long userId, int currentMonth)
        {
            try
            {
                var currentYear = DateTime.Now.Year;
                var startDate = new DateTime(currentYear, currentMonth, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var topUps = await _db.TopUpTransactions
                    .Where(t => t.UserId == userId && t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                    .ToListAsync();

                if (topUps.Any())
                {
                    var totalTopUps = topUps.Sum(t => t.Amount);
                    return totalTopUps;
                }
                else
                {
                    return 0; 
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Exception occurred: {ex.Message}");
            }
        }


        public async Task<bool> ProcessTopUp(long userId, long beneficiaryId, decimal amount)
        {
            var topup = new TopUpTransaction
            {
                Amount = amount,
                UserId = userId,
                BeneficiaryId = beneficiaryId,
                TransactionDate = DateTime.Now,

            };
            var addedEntry = await _db.TopUpTransactions.AddAsync(topup);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
