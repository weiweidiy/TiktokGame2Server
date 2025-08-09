using JFramework;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TiktokGame2Server.Others
{
    public class TimedTaskService : BackgroundService
    {
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(5); // 5����ִ��һ�Σ��ɸ�����Ҫ����

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
                    // ������д��Ķ�ʱ�����߼�
                    await DoWorkAsync();

                    // �ȴ���һ������
                    await Task.Delay(_interval, stoppingToken);
                }
                catch (Exception ex)
                {
                    // ��¼�쳣��־
                }
            }
        }

        private Task DoWorkAsync()
        {
            logger.Log("��ʱ��ִ��");
            // ��Ķ�ʱ�������
            return Task.CompletedTask;
        }
    }
}
