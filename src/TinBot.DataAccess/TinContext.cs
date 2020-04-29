using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using TinBot.Entities;

namespace TinBot.DataAccess
{
    public class TinContext : DbContext
    {
        private const string format = "dd.MM.yyyy";
        public TinContext(DbContextOptions<TinContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NameDay>()
                .Property(e => e.Date)
                .HasConversion(v => v.ToString(format), v => DateTime.ParseExact(v, format, CultureInfo.InvariantCulture));
        }

        public DbSet<NameDay> NameDays{ get; set; }
    }
}