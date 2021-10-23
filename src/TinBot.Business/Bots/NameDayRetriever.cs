using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinBot.DataAccess;

namespace TinBot.Business.Bots
{
    public class NameDayRetriever
    {
        private readonly TinContext _context;

        public NameDayRetriever(TinContext context) => _context = context;

        public async Task<string> GetNameDay()
        {
            var nameDays = await _context.NameDays.ToListAsync();

            var todayNameDays = nameDays
                .Where(o => o.Date.Day == DateTime.Today.Day && o.Date.Month == DateTime.Today.Month)
                .Select(o => o.Name)
                .OrderBy(o => o)
                .ToList();

            var merged = todayNameDays switch
            {
                { Count: 1 } => todayNameDays.First(),
                { Count: > 1 } => string.Join(", ", todayNameDays.Take(todayNameDays.Count - 1)) + " & " + todayNameDays.Last(),
                _ => "nikdo",
            };

            return merged;
        }
    }
}