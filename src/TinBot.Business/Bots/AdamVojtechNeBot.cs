using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TinBot.DataAccess;

namespace TinBot.Business.Bots
{
    public class AdamVojtechNeBot : IBot
    {
        private readonly ILogger<AdamVojtechNeBot> _logger;
        private readonly TinContext _context;

        public AdamVojtechNeBot(ILogger<AdamVojtechNeBot> logger, TinContext context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Run()
        {
            _logger.LogInformation("Wow");
            var accounts = await _context.Accounts.ToListAsync();
            _logger.LogInformation(accounts.Count.ToString());
        }
    }
}
