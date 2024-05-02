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
    public class BalanceImplementation : IBalance
    {
        public AppDBContext _context;
        public BalanceImplementation(AppDBContext context)
        {
            _context = context;
        }
        public async Task<decimal> GetUserAvailableBalance(long userId)
        {
            var balance = await _context.Balances.FirstOrDefaultAsync(u => u.Id == userId);
            if (balance!=null)
            {
                return balance.Amount;
            }
            return 0;

        }

        public async Task<bool> TopUpBalance(long userId, decimal amount)
        {
            var balance = await _context.Balances.FirstOrDefaultAsync(u => u.Id == userId);
            if (balance != null)
            {
                balance.Amount = balance.Amount + amount;
                balance.Amount = balance.Amount - 1; //charge of AED 1
                balance.UpdatedDate = DateTime.UtcNow;

                _context.Entry(balance).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return true;
            }
            else
            {
                var userBalance = new Balance();
                userBalance.UserId = userId;
                userBalance.Amount = amount;
                userBalance.Amount = userBalance.Amount - 1; //charge of AED 1
                userBalance.UpdatedDate = DateTime.UtcNow;

                await _context.Balances.AddAsync(userBalance);
                await _context.SaveChangesAsync();

                return true;
            }
          
        }

        public async Task<bool> UpdateBalance(long userId, decimal amount)
        {
            var balance = await _context.Balances.FirstOrDefaultAsync(u => u.Id == userId);
            if (balance!=null)
            {
                balance.Amount = balance.Amount - amount;
                balance.Amount = balance.Amount - 1; //charge of AED 1
                balance.UpdatedDate = DateTime.UtcNow;

                _context.Entry(balance).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }
    }
}
