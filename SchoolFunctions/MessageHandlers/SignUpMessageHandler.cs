using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SchoolFunctions.Azure;
using SchoolFunctions.Models;

namespace SchoolFunctions.MessageHandlers
{
    public class SignUpMessageHandler : IMessageHandler
    {
        private IAzureServices _azureServices;

        public async Task HandleAsync(ManagementModel message)
        {
            _azureServices = new AzureServices(message);
            await _azureServices.StudentInsertOrMergeAsync();
        }
    }
}
