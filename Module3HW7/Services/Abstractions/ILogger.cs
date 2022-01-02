using System;
using System.Threading.Tasks;

namespace Module3HW7.Services.Abstractions
{
    public interface ILogger
    {
        event Func<string, Task> EventBackUp;

        Task Log(LogType logType, string message);

        Task ToBackUp();
    }
}
