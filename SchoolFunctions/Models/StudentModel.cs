using Microsoft.WindowsAzure.Storage.Table;

namespace SchoolFunctions.Models
{
    public class StudentModel : TableEntity
    {
        /// <summary>
        /// Student Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Student Surname
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Email of student
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Age of Student
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Course student want to participate in
        /// </summary>
        public string Course { get; set; }

        /// <summary>
        /// Succesfully signed up or not
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Message to student and reason to success or failure of signup
        /// </summary>
        public string Reason { get; set; }
    }
}
