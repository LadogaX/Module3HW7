using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Module3HW7.Services.Abstractions;

namespace Module3HW7.Services
{
    public class FileService : IFileService
    {
        private StreamWriter _streamWriter;
        private SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);
        private string _logFolderPath;

        public FileService(IConfigService configService)
        {
            ConfigService = configService;
            _logFolderPath = configService.Config.LogFolderPath;

            if (!Directory.Exists(_logFolderPath))
            {
                Directory.CreateDirectory(_logFolderPath);
            }

            _streamWriter = new StreamWriter(Path.Combine(configService.Config.LogFolderPath, "log.txt"));
        }

        public IConfigService ConfigService { get; set; }

        public async Task WriteAsync(string text)
        {
            await _semaphoreSlim.WaitAsync();

            await _streamWriter.WriteLineAsync(text);
            await _streamWriter.FlushAsync();

            _semaphoreSlim.Release();
        }
    }
}