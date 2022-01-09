using System;
using System.IO;
using System.Threading.Tasks;

namespace Module3HW7.Services.Abstractions
{
    public interface ILogger
    {
        event Action<string> EventBackUp;

        Task Log(LogType logType, StreamWriter streamWriter, string message);
    }
}
