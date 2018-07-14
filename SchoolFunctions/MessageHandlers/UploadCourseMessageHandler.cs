using System.Threading.Tasks;
using SchoolFunctions.Azure;
using SchoolFunctions.Models;

namespace SchoolFunctions.MessageHandlers
{
    public class UploadCourseMessageHandler : IMessageHandler
    {
        private IAzureServices _azureServices;

        public async Task HandleAsync(ManagementModel message)
        {
            _azureServices = new AzureServices(message);
            await _azureServices.CoursesInsertOrMergeAsync();
        }
    }
}
