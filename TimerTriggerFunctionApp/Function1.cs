using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ServerlessDemo.Functions
{
    public class CacheCleanupFunction
    {
        private readonly ILogger<CacheCleanupFunction> _logger;

        public CacheCleanupFunction(ILogger<CacheCleanupFunction> logger)
        {
            _logger = logger;
        }

        [Function("StaleCacheCleaner")]
        public void Run([TimerTrigger("%CleanupSchedule%")] TimerInfo timerInfo)
        {
            DateTime currentRunTime = DateTime.UtcNow;
            _logger.LogInformation("--- Timer Trigger Fired at: {Time:HH:mm:ss} UTC ---", currentRunTime);

            // 1. Inspect Missed Schedule Flag
            if (timerInfo.IsPastDue)
            {
                _logger.LogWarning("[ALERT] Trigger execution is running late! Recovering missed schedule.");
            }

            // 2. Read NCRONTAB Metadata
            if (timerInfo.ScheduleStatus is not null)
            {
                _logger.LogInformation("Previous Run : {Last:HH:mm:ss}", timerInfo.ScheduleStatus.Last);
                _logger.LogInformation("Next Run     : {Next:HH:mm:ss}", timerInfo.ScheduleStatus.Next);
            }

            // 3. Simulated Workload
            CleanStaleMemoryCache();
        }

        private void CleanStaleMemoryCache()
        {
            _logger.LogInformation("Scanning and clearing expired cache keys... Done.");
        }
    }
}