using Microsoft.EntityFrameworkCore;
using TwitterBot.Entities;

namespace TwitterBot.DataAccess
{
    public class TwitterContext : DbContext
    {
        public TwitterContext(DbContextOptions<TwitterContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
    }
}