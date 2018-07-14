using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolFunctions.Models;
using SchoolFunctions.Telemetry;

namespace SchoolFunctions.MessageHandlers
{
    public class UploadCourseMessageHandler : IMessageHandler
    {
        private readonly IApplicationInsights _applicationInsight;

        public UploadCourseMessageHandler()
        {
            _applicationInsight = new ApplicationInsights();
        }

        public async Task HandleAsync(ManagementModel message)
        {
            var telemetry = _applicationInsight.Create();

            try
            {
                foreach (var item in message.Courses)
                {
                    var appEvent = new Dictionary<string, string>
                        {
                            { "Course", item.Course },
                            { "Lecturer",  item.Lecturer },
                            { "Quantity", item.Quantity }
                        };

                    telemetry.TrackEvent(message.MessageType, appEvent);
                }                
            }
            catch (Exception ex)
            {
                telemetry.TrackException(ex);
            }
            finally
            {
                telemetry.Flush();

                // flush is not blocking so wait a bit
                await Task.Delay(5000);
            }
        }
    }
}
