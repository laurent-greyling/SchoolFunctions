using Microsoft.ApplicationInsights;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SchoolFunctions.Models;
using SchoolFunctions.Telemetry;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolFunctions.Azure
{
    public class AzureServices : IAzureServices
    {
        CloudTable _table;
        ManagementModel _managementModel;
        TelemetryClient _telemetry;
        
        private readonly IApplicationInsights _applicationInsight;

        public AzureServices(ManagementModel managementModel)
        {
            _managementModel = managementModel;
            _applicationInsight = new ApplicationInsights();

            var account = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
            var client = account.CreateCloudTableClient();
            _table = client.GetTableReference(managementModel.MessageType);

            _telemetry = _applicationInsight.Create();
            Task.Run(async () =>
            {
                if (!await _table.ExistsAsync())
                {
                    await _table.CreateIfNotExistsAsync();
                }
            });
        }

        public async Task CoursesInsertOrMergeAsync()
        {
            try
            {
                var batchOperation = new TableBatchOperation();

                foreach (var model in _managementModel.Courses)
                {
                    model.PartitionKey = _managementModel.MessageType;
                    model.RowKey = model.Course.ToLower().Replace(" ", "-");
                    batchOperation.InsertOrMerge(model);

                    var appEvent = new Dictionary<string, string>
                        {
                            { "Course", model.Course },
                            { "Lecturer",  model.Lecturer },
                            { "Quantity", model.Quantity }
                        };

                    _telemetry.TrackEvent(_managementModel.MessageType, appEvent);
                }

                await _table.ExecuteBatchAsync(batchOperation);
            }
            catch (Exception ex)
            {
                _telemetry.TrackException(ex);
            }
            finally
            {
                _telemetry.Flush();

                // flush is not blocking so wait a bit
                await Task.Delay(5000);
            }
        }
    }
}
