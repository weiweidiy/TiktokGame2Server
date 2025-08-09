using JFramework;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TiktokGame2Server.Others
{
    public class TimedTaskService : BackgroundService
    {
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(5); // 5分钟执行一次，可根据需要调整

        JFramework.ILogger logger;

        public TimedTaskService(JFramework.ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // 在这里写你的定时任务逻辑
                    await DoWorkAsync();

                    // 等待下一个周期
                    await Task.Delay(_interval, stoppingToken);
                }
                catch (Exception ex)
                {
                    // 记录异常日志
                }
            }
        }

        private Task DoWorkAsync()
        {
            logger.Log("定时器执行");
            // 你的定时任务代码
            return Task.CompletedTask;
        }
    }
}
