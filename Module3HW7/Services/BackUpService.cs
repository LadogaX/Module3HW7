using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Module3HW7.Models;
using Module3HW7.Services.Abstractions;

namespace Module3HW7.Services
{
    public class BackUpService : IBackUpService
    {
        private SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);
        private StreamWriter _streamWriter;
        private string _backUpFolderPath;

        public BackUpService()
        {
            Config = new ConfigService().Config;
            _backUpFolderPath = Config.BackUpFolderPath;

            string nameFile = DateTime.UtcNow.ToString("yyyyMMdd_HH_mm_ss_fffff");
            nameFile = Path.Combine(_backUpFolderPath, $"{nameFile}.backup");

            if (!Directory.Exists(_backUpFolderPath))
            {
                Directory.CreateDirectory(_backUpFolderPath);
            }

            _streamWriter = new StreamWriter(nameFile);
        }

        public Config Config { get; set; }

        public async Task CreateBackUpAsync(string text)
        {
            try
            {
                await _semaphoreSlim.WaitAsync();
                await _streamWriter.WriteLineAsync($"{text}");
                await _streamWriter.FlushAsync();
                await _streamWriter.DisposeAsync();
                _semaphoreSlim.Release();
            }
            catch (Exception oex)
            {
                Console.WriteLine(oex.Message);
            }
        }
    }
}