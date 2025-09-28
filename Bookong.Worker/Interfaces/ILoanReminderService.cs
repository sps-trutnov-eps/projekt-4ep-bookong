using Microsoft.Extensions.Hosting;

namespace Bookong.Worker.Interfaces
{
    public interface ILoanReminderService : IHostedService
    {
        protected Task ExecuteAsync(CancellationToken stoppingToken);
    }
}
