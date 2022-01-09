using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module3HW7.Services.Abstractions
{
    public interface IFileService
    {
        Task WriteAsync(StreamWriter streamWriter, string text);
    }
}
