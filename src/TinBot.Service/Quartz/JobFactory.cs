using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Concurrent;

namespace TwitterBot.Service
{
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private ConcurrentDictionary<Type, IServiceScope> _scopes = new ConcurrentDictionary<Type, IServiceScope>();

        public JobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var scope = _serviceProvider.CreateScope();

            var job = scope.ServiceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
            _scopes[bundle.JobDetail.JobType] = scope;

            return job;
        }

        public void ReturnJob(IJob job)
        {
            _scopes[job.GetType()].Dispose();
        }
    }
}
