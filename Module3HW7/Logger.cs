using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Module3HW7.Services;
using Module3HW7.Services.Abstractions;

namespace Module3HW7
{
    public class Logger : ILogger
    {
        private readonly IConfigService _configService;
        private readonly StringBuilder _logs;
        private static int _counter = 0;
        private readonly IFileService _fileService;
        private SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);
        public Logger(IFileService fileService, IConfigService configService)
        {
            _configService = configService;
            _fileService = fileService;
            _logs = new StringBuilder();
        }

        public event Func<string, Task> EventBackUp;

        public async Task Log(LogType logType, string message)
        {
            await _semaphoreSlim.WaitAsync();
            _counter++;
            var log = $"{DateTime.UtcNow}: {logType}: {message}";
            Console.WriteLine(log);
            await _fileService.WriteAsync(log);
            _logs.AppendLine(log);
            await ToBackUp();
            _semaphoreSlim.Release();
        }

        public async Task ToBackUp()
        {
            if (_counter % _configService.Config.CountRecordsFlushBackUp == 0)
            {
                await EventBackUp?.Invoke(_logs.AppendLine("============================================").ToString());
            }
        }
    }
}
