
using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;

namespace SchoolFunctions.MessageHandlers
{
    public interface IMessageHandler
    {
        Task HandleAsync(Message message);
    }
}
