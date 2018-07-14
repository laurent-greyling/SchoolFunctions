using System;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace SchoolFunctions.MessageHandlers
{
    public class UploadCourseMessageHandler : IMessageHandler
    {
        public Task HandleAsync(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
