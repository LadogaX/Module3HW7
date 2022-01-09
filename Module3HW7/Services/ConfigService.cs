using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Module3HW7.Models;
using Module3HW7.Services.Abstractions;
using Newtonsoft.Json;

namespace Module3HW7.Services
{
    public class ConfigService : IConfigService
    {
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);
        public ConfigService()
        {
            Config = new Config();
            LoadConfig().GetAwaiter().GetResult();
        }

        public Config Config { get; set; }

        public async Task LoadConfig()
        {
            if (!File.Exists("config.json"))
            {
                var streamWriter = new StreamWriter("config.json");
                streamWriter.WriteLine("{}");
                streamWriter.Close();
            }

            var configFile = File.ReadAllText("config.json");
            Config = JsonConvert.DeserializeObject<Config>(configFile);

            await Task.Run(async () =>
             {
                 if (Config.CountRecordsFlushBackUp == 0)
                 {
                     Config.CountRecordsFlushBackUp = 10;
                     await SaveConfigAsync();
                 }
             });

            await Task.Run(async () =>
             {
                 if (Config.LogFolderPath == null)
                 {
                     Config.LogFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
                     await SaveConfigAsync();
                 }
             });

            await Task.Run(async () =>
            {
                if (Config.BackUpFolderPath == null)
                {
                    Config.BackUpFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "BackUp");
                    await SaveConfigAsync();
                }
            });
            await Task.Run(async () =>
            {
                if (Config.LogNameFile == null)
                {
                    Config.LogNameFile = "Log.txt";
                    await SaveConfigAsync();
                }
            });
        }

        public async Task SaveConfigAsync()
        {
            await _semaphoreSlim.WaitAsync();

            var json = JsonConvert.SerializeObject(Config, Formatting.Indented);
            await File.WriteAllTextAsync("config.json", json);

            _semaphoreSlim.Release();
        }
    }
}
