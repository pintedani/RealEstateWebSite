using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Crosscutting;
using Imobiliare.UI.ScheduledTasks;
using Imobiliare.UI.ScheduledTasks.Jobs;
using Microsoft.Extensions.DependencyInjection;

namespace ScheduledTasks
{
    public class ScheduledTasksBackgroundService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly int _intervalInSeconds = 60; // Default interval
        private readonly IMemoryCache _cache;
        private readonly IEnvironmentService environmentService;

        private AnunturiAdaugateCheckerJob anunturiAdaugateCheckerJob;
        private AnunturiExpirateCheckerJob anunturiExpirateCheckerJob;
        private TrimitereRapoarteAdminJob trimitereRapoarteAdminJob;
        private IServiceScopeFactory scopeFactory;

        public ScheduledTasksBackgroundService(IMemoryCache memoryCache,
            IEnvironmentService environmentService,
            IServiceScopeFactory scopeFactory)
        {
            _cache = memoryCache;
            this.scopeFactory = scopeFactory;
            this.environmentService = environmentService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(_intervalInSeconds));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var now = DateTime.Now;

            using (var scope = scopeFactory.CreateScope())
            {
                // Resolve the scoped service within the scope
                var scopedService = scope.ServiceProvider.GetRequiredService<AnunturiAdaugateCheckerJob>();
                this.anunturiAdaugateCheckerJob = scope.ServiceProvider.GetRequiredService<AnunturiAdaugateCheckerJob>();
                this.anunturiExpirateCheckerJob = scope.ServiceProvider.GetRequiredService<AnunturiExpirateCheckerJob>();
                this.trimitereRapoarteAdminJob = scope.ServiceProvider.GetRequiredService<TrimitereRapoarteAdminJob>();

                // Check and execute Task 1 (every 2 hours)
                if (!IsTaskExecutedRecently("AnunturiAdaugateCheckerJob", TimeSpan.FromHours(1)))
                {
                    Console.WriteLine("Task 1 executed at: " + now);
                    this.anunturiAdaugateCheckerJob.Execute();
                    UpdateLastRunTimestamp("AnunturiAdaugateCheckerJob", now);
                    // Execute Task 1 logic here
                }

                // Check and execute Task 2 (every 8 hours)
                if (!IsTaskExecutedRecently("AnunturiExpirateCheckerJob", TimeSpan.FromHours(8)))
                {
                    Console.WriteLine("Task 2 executed at: " + now);
                    this.anunturiExpirateCheckerJob.Execute();
                    UpdateLastRunTimestamp("AnunturiExpirateCheckerJob", now);
                    // Execute Task 2 logic here
                }

                // Check and execute Task 3 (every 2 days)
                if (!IsTaskExecutedRecently("RapoarteAdminJob", TimeSpan.FromDays(2)))
                {
                    Console.WriteLine("Task 3 executed at: " + now);
                    this.trimitereRapoarteAdminJob.Execute();
                    UpdateLastRunTimestamp("RapoarteAdminJob", now);
                    // Execute Task 3 logic here
                }
            }
        }

        private bool IsTaskExecutedRecently(string taskName, TimeSpan interval)
        {
            if (_cache.TryGetValue<DateTime>(taskName, out var lastRun))
            {
                return (DateTime.Now - lastRun) < interval;
            }

            var filePath = GetFullTaskPath(taskName);
            if (File.Exists(filePath))
            {
                var lastRunFromFile = DateTime.Parse(File.ReadAllText(filePath));
                _cache.Set(taskName, lastRunFromFile, TimeSpan.FromDays(1)); // Cache for 1 day
                return (DateTime.Now - lastRunFromFile) < interval;
            }

            return false;
        }

        private void UpdateLastRunTimestamp(string taskName, DateTime timestamp)
        {
            _cache.Set(taskName, timestamp, TimeSpan.FromDays(1)); // Cache for 1 day
            string filePath = GetFullTaskPath(taskName);
            File.WriteAllText(filePath, timestamp.ToString());
        }

        private string GetFullTaskPath(string taskName)
        {
            // Update file for persistence if needed
            var folderPath = environmentService.GetWebRootPath() + "\\ScheduledTaskData";
            return Path.Combine(folderPath, $"{taskName}.txt");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
