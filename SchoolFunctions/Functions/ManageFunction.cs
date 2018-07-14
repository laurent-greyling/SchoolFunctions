using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using SchoolFunctions.Models;
using SchoolFunctions.Startup;

namespace SchoolFunctions.Functions
{
    public static class ManageFunction
    {
        private static IStartup _startup;

        [FunctionName("ManageFunction")]
        public static async Task Run([QueueTrigger("management")]string queueMessage, TraceWriter log)
        {
            var message = JsonConvert.DeserializeObject<ManagementModel>(queueMessage);
            _startup = new Startup.Startup();
            await _startup.RunAsync(message);
        }
    }
}
