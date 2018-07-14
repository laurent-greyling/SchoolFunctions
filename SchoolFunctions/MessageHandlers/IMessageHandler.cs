using Microsoft.Azure.ServiceBus;
using SchoolFunctions.Models;
using System.Threading.Tasks;

namespace SchoolFunctions.MessageHandlers
{
    public interface IMessageHandler
    {
        Task HandleAsync(ManagementModel message);
    }
}
