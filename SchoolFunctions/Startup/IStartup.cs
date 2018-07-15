using SchoolFunctions.Models;
using System.Threading.Tasks;

namespace SchoolFunctions.Startup
{
    public interface IStartup
    {
        /// <summary>
        /// Go to correct process
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task RunAsync(ManagementModel message);
    }
}
