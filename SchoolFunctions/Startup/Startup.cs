using SchoolFunctions.MessageHandlers;
using SchoolFunctions.Models;
using SimpleInjector;
using System;
using System.Threading.Tasks;

namespace SchoolFunctions.Startup
{
    public class Startup : IStartup
    {
        public async Task RunAsync(ManagementModel message)
        {
            var messageType = $"SchoolFunctions.MessageHandlers.{message.MessageType}MessageHandler";
            Type type = Type.GetType(messageType);

            Container container = new Container();
            container.Register(type);
            var instance = (IMessageHandler)container.GetInstance(type);
            await instance.HandleAsync(message);            
        }
    }
}
