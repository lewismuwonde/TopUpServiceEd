using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUpDB.Entity
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TopUpBeneficiary> TopUpBeneficiaries { get; set; }
        public DbSet<TopUpTransaction> TopUpTransactions { get; set; }
        public DbSet<Balance> Balances { get; set; }        

    }
}
