using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using TinBot.Service.Jobs;

namespace TinBot.Service
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBusinessServices(configuration);

            // Add Quartz services
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            services.AddTransient<RefreshBioJob>();
            services.AddSingleton(new JobSchedule(typeof(RefreshBioJob), configuration.GetValue<string>("Cron")));

            return services;
        }
    }
}