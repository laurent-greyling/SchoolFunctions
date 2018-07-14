
using Microsoft.Azure.ServiceBus;
using SchoolFunctions.MessageHandlers;
using SimpleInjector;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolFunctions.Startup
{
    public class Startup
    {
        public async Task RunAsync(Message message)
        {
            var type = message.UserProperties["MessageType"];
            Type d = Type.GetType($"{type}MessageHandler");

            Container dd = new Container();
            dd.Register(d);
            var x = dd.GetAllInstances<IMessageHandler>();

            foreach (var item in x)
            {
                await item.HandleAsync(message);
            }
        }
    }
}
