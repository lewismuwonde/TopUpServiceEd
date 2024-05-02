using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUpDB.Entity;
using TopUpDB.Utility;

namespace TopUpDB.Seeding
{
    public class BalanceSeeder :IDataSeeder
    {
        public async Task SeedDataAsync(AppDBContext context)
        {
            if (!context.Balances.Any())
            { 
                var balance = new Balance
                {
                     Id = 1,
                     UserId = 1,
                     Amount = 100,
                     UpdatedDate = DateTime.Now  
                };               

                await context.Balances.AddAsync(balance);
                await context.SaveChangesAsync();
            }
        }
    }
}
