using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace SchoolFunctions.Functions
{
    public static class UploadCourseMaterialFunction
    {
        [FunctionName("UploadCourseMaterialFunction")]
        public static void Run([QueueTrigger("management", Connection = "")]string myQueueItem, TraceWriter log)
        {
            log.Info($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
