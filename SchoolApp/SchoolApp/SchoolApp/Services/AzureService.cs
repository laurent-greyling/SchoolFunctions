using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using SchoolApp.Models;

namespace SchoolApp.Services
{
    public class AzureService : IAzureService
    {
        private const string AzureWebJobsStorage = "";
        private readonly CloudStorageAccount _account;
        private readonly CloudTableClient _client;
        private readonly CloudQueueClient _queueClient;
        private readonly CloudTable _table;
        private readonly CloudQueue _queue;

        public AzureService(string tableName)
        {
            _account = CloudStorageAccount.Parse(AzureWebJobsStorage);
            _client = _account.CreateCloudTableClient();
            _queueClient = _account.CreateCloudQueueClient();
            _table = _client.GetTableReference(tableName);
            _queue = _queueClient.GetQueueReference(AppConst.ManagementQueue);
        }

        public async Task<List<CourseModel>> RetreiveCourseEntities()
        {
            TableContinuationToken token = null;
            var entities = new List<CourseModel>();

            do
            {
                try
                {
                    var query = new TableQuery<CourseModel>();
                    var queryResult = await _table.ExecuteQuerySegmentedAsync(query, token).ConfigureAwait(false);
                    entities.AddRange(queryResult.Results);
                    token = queryResult.ContinuationToken;
                }
                catch (System.Exception)
                {
                    throw;
                }
            } while (token != null);

            return entities;
        }

        public async Task<StudentModel> RetreiveStudentEntity(string partitionKey, string rowKey)
        {
            var operation = TableOperation.Retrieve<StudentModel>(partitionKey, rowKey);
            var entity = await _table.ExecuteAsync(operation).ConfigureAwait(false);

            return (StudentModel)entity.Result;
        }

        public async Task<bool> SendMessage(string message)
        {
            try
            {
                await _queue.AddMessageAsync(new CloudQueueMessage(message));
                return true;
                
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }
    }
}
