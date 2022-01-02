using System.Threading.Tasks;
using Module3HW7.Models;

namespace Module3HW7.Services.Abstractions
{
    public interface IBackUpService
    {
         Config Config { get; set; }

         Task CreateBackUpAsync(string text);
    }
}
