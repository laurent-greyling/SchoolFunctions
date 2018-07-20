using Microsoft.ApplicationInsights;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SchoolFunctions.Helpers;
using SchoolFunctions.Models;
using SchoolFunctions.Telemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolFunctions.Azure
{
    public class AzureServices : IAzureServices
    {
        CloudTable _table;
        ManagementModel _managementModel;
        TelemetryClient _telemetry;
        CloudStorageAccount _account;
        CloudTableClient _client;

        private readonly IApplicationInsights _applicationInsight;

        public AzureServices(ManagementModel managementModel)
        {
            _managementModel = managementModel;
            _applicationInsight = new ApplicationInsights();

            _account = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
            _client = _account.CreateCloudTableClient();

            _table = _client.GetTableReference(managementModel.MessageType);

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
                    model.Quantity = 0;
                    batchOperation.InsertOrMerge(model);

                    var appEvent = new Dictionary<string, string>
                        {
                            { "Course", model.Course },
                            { "Lecturer",  model.Lecturer },
                            { "Quantity", model.Quantity.ToString() }
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

        public async Task StudentInsertOrMergeAsync()
        {
            var courseCapacity = new CourseModel();
            try
            {
                if (!IsValidEmail(_managementModel.Details.Email))
                {
                    await UpdateSignUpDetails(false, $"Sorry {_managementModel.Details.Name}, please supply a correct Email Address");
                    return;
                }

                var isAlreadySignedUp = await IsStudentAlreadySignedUp(_managementModel.Details.Course);

                if (isAlreadySignedUp)
                {
                    await UpdateSignUpDetails(true, $"Sorry {_managementModel.Details.Name}, but you are already signed up for {_managementModel.Details.Course}");
                    return;
                }

                courseCapacity = await RetreiveCourseTableEntityAsync();

                if (courseCapacity.Quantity == courseCapacity.MaxQuantity)
                {
                    await UpdateSignUpDetails(false, $"Sorry {_managementModel.Details.Name}, but the capacity for {_managementModel.Details.Course} is already reached");
                    return;
                }

                courseCapacity.Quantity++;
                await UpdateCourseTableEntityAsync(courseCapacity);

                await UpdateSignUpDetails(true, $"Success, {_managementModel.Details.Name} you are now signedup for {_managementModel.Details.Course}");

                var appEvent = new Dictionary<string, string>
                        {
                            { "Name",  _managementModel.Details.Name },
                            { "Surname",  _managementModel.Details.Surname },
                            { "Email",  _managementModel.Details.Email },
                            { "Age",  _managementModel.Details.Age.ToString() },
                            { "Course",  _managementModel.Details.Course }
                        };

                _telemetry.TrackEvent(_managementModel.MessageType, appEvent);
            }
            catch (Exception ex)
            {
                //TODO: If email class throw don't come here, give different message
                if (courseCapacity != null)
                {
                    courseCapacity.Quantity--;
                    await UpdateCourseTableEntityAsync(courseCapacity);
                    await UpdateSignUpDetails(false, $"Sorry {_managementModel.Details.Name}, something went wrong when trying to sign you up. Please try again later");
                }
                _telemetry.TrackException(ex);
            }
            finally
            {
                _telemetry.Flush();

                // flush is not blocking so wait a bit
                await Task.Delay(5000);
            }
        }

        #region Helper Methods
        private async Task<CourseModel> RetreiveCourseTableEntityAsync()
        {
            var table = _client.GetTableReference(AppConst.UploadCourse);
            var partitionKey = AppConst.UploadCourse;
            var rowKey = _managementModel.Details.Course.ToLower().Replace(" ", "-");
            var retreiveOperation = TableOperation.Retrieve<CourseModel>(partitionKey, rowKey);
            var result = await table.ExecuteAsync(retreiveOperation);

            return (CourseModel)result.Result;
        }

        private async Task<bool> IsStudentAlreadySignedUp(string course)
        {
            try
            {
                var table = _client.GetTableReference(AppConst.SignUp);
                var partitionKey = AppConst.SignUp;
                var rowKey = $"{_managementModel.Details.Name.ToLower().Replace(" ", "-")}-{_managementModel.Details.Surname.ToLower().Replace(" ", "-")}-{_managementModel.Details.Course.ToLower().Replace(" ", "-")}";

                var query = new TableQuery<StudentModel>()
                    .Where(TableQuery.CombineFilters(
                           TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
                           TableOperators.And,
                           TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey)
                           )
                        );

                var result = await table.ExecuteQuerySegmentedAsync(query, null);

                return result.Any(x => x.Course == course && x.IsSuccess == true);
            }
            catch (Exception)
            {
                return false;
            }            
        }

        private async Task UpdateCourseTableEntityAsync(CourseModel courseDetails)
        {
            var table = _client.GetTableReference(AppConst.UploadCourse);
            var retreiveOperation = TableOperation.InsertOrMerge(courseDetails);
            var result = await table.ExecuteAsync(retreiveOperation);
        }

        private StudentModel MapStudentToEntity(bool isSuccess, string message)
        {
            return new StudentModel
            {
                PartitionKey = _managementModel.MessageType,
                RowKey = $"{_managementModel.Details.Name.ToLower().Replace(" ", "-")}-{_managementModel.Details.Surname.ToLower().Replace(" ", "-")}-{_managementModel.Details.Course.ToLower().Replace(" ", "-")}",
                Name = _managementModel.Details.Name,
                Surname = _managementModel.Details.Surname,
                Email = _managementModel.Details.Email,
                Age = _managementModel.Details.Age,
                Course = _managementModel.Details.Course,
                IsSuccess = isSuccess,
                Reason = message
            };
        }

        private async Task UpdateSignUpDetails(bool isSuccess, string message)
        {
            var emailService = new EmailService();
            var insertMergeOperation = TableOperation.InsertOrMerge(MapStudentToEntity(isSuccess, message));
            await _table.ExecuteAsync(insertMergeOperation);

            if (IsValidEmail(_managementModel.Details.Email))
            {
                emailService.SendMail(MapStudentToEntity(isSuccess, message));
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
