using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Module3HW7.Services;
using Module3HW7.Services.Abstractions;

namespace Module3HW7
{
    public class Starter
    {
        private TaskCompletionSource _tsc = new TaskCompletionSource();
        private TaskCompletionSource _tsc2 = new TaskCompletionSource();
        public Starter(ILogger logger)
        {
            Logger = logger;
        }

        public ILogger Logger { get; }
        public void Run()
        {
            Logger.EventBackUp += CreateBackUpAsync;

            Task.Run(async () => await LogGenerator(50, "Thread 1", _tsc));
            Task.Run(async () => await LogGenerator(50, "Thread 2", _tsc2));

            _tsc.Task.GetAwaiter().GetResult();
            _tsc2.Task.GetAwaiter().GetResult();
        }

        public async Task LogGenerator(int countLogs, string message, TaskCompletionSource tsc)
        {
            var rnd = new Random();

            for (var i = 0; i < countLogs; i++)
            {
                await Logger.Log((LogType)rnd.Next(3), $"{message}-{i}");
            }

            tsc.SetResult();
        }

        public async Task CreateBackUpAsync(string text)
        {
            await new BackUpService().CreateBackUpAsync(text);
        }
    }
}
