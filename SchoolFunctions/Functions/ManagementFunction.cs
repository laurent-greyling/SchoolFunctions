using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SchoolFunctions.Azure;
using SimpleInjector;
using System;
using System.Threading.Tasks;

namespace SchoolFunctions.Functions
{
    public static class ManagementFunction
    {
        [FunctionName("ManagementFunction")]
        public static async Task Run([ServiceBusTrigger("management")]Message message, TraceWriter log)
        {

            await new Startup.Startup().RunAsync(message);
            log.Info($"C# ServiceBus queue trigger function processed message: {message}");
        }
    }
}
