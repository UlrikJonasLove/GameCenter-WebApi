using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameCenter.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GameCenter.Services
{
    public class NewlyReleasedGameService : IHostedService, IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private Timer timer;

        public NewlyReleasedGameService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using(var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var today = DateTime.Today;
                var games = await context.Game.Where(x => x.ReleaseDate == today).ToListAsync();
                if(games.Any())
                {
                    foreach(var game in games)
                    {
                        game.NewlyRelease = true;
                    }
                    await context.SaveChangesAsync();
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}