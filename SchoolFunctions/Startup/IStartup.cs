using SchoolFunctions.Models;
using System.Threading.Tasks;

namespace SchoolFunctions.Startup
{
    public interface IStartup
    {
        Task RunAsync(ManagementModel message);
    }
}
