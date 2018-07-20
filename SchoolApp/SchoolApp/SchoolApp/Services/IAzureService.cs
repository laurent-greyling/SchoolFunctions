using SchoolApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolApp.Services
{
    public interface IAzureService
    {
        Task<List<CourseModel>> RetreiveCourseEntities();

        Task<StudentModel> RetreiveStudentEntity(string partitionKey, string rowKey);

        Task<bool> SendMessage(string message);
    }
}
