using System.Threading.Tasks;

namespace SchoolFunctions.Azure
{
    public interface IAzureServices
    {
        /// <summary>
        /// Upload or Update course material 
        /// </summary>
        /// <returns></returns>
        Task CoursesInsertOrMergeAsync();

        /// <summary>
        /// Upload or Update Student Details
        /// </summary>
        /// <returns></returns>
        Task StudentInsertOrMergeAsync();
    }
}
