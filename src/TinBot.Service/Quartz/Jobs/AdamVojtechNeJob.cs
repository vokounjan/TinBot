using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;
using TinBot.Business.Bots;

namespace TinBot.Service.Jobs
{
    [DisallowConcurrentExecution]
    public class AdamVojtechNeJob : IJob
    {
        private readonly ILogger<AdamVojtechNeJob> _logger;
        private readonly AdamVojtechNeBot _adamVojtechNeBot;

        public AdamVojtechNeJob(ILogger<AdamVojtechNeJob> logger, AdamVojtechNeBot adamVojtechNeBot)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _adamVojtechNeBot = adamVojtechNeBot ?? throw new ArgumentNullException(nameof(adamVojtechNeBot));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Hello world!");

            await _adamVojtechNeBot.Run();

            _logger.LogInformation("Bye world!");
        }
    }
}
