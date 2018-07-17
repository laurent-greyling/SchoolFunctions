using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SchoolApp.Models;

namespace SchoolApp.Services
{
    public class AzureService : IAzureService
    {
        private const string AzureWebJobsStorage = "";
        private readonly CloudStorageAccount _account;
        private readonly CloudTableClient _client;
        private readonly CloudTable _table;

        public AzureService(string tableName)
        {
            _account = CloudStorageAccount.Parse(AzureWebJobsStorage);
            _client = _account.CreateCloudTableClient();
            _table = _client.GetTableReference(tableName);
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
    }
}
