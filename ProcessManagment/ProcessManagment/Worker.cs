using ProcessManagment.Application.Services;

namespace ProcessManagment
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var taskService = scope.ServiceProvider
                       .GetRequiredService<TaskService>();

                    var tasks = await taskService.GetPendingTask();

                    foreach (var task in tasks)
                    {
                        await taskService.ProcessTask(task);
                    }
                }
                await Task.Delay(5000, stoppingToken); // كل 5 ثواني
            }
        }
    }
}
