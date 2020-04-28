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

        public async Task<List<string>> GetNameDay() => await _context.NameDays
            .Where(o => o.Date.Day == DateTime.Today.Day && o.Date.Month == DateTime.Today.Month)
            .Select(o => o.Name)
            .OrderBy(o => o)
            .ToListAsync();
    }
}
