using Microsoft.EntityFrameworkCore;
using TinBot.Entities;

namespace TinBot.DataAccess
{
    public class TinContext : DbContext
    {
        public TinContext(DbContextOptions<TinContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<NameDay> NameDays{ get; set; }
    }
}