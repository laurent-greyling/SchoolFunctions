using System.Collections.Generic;

namespace SchoolFunctions.Models
{
    public class ManagementModel
    {
        /// <summary>
        /// Message Type to direct messagehandler to correc process
        /// </summary>
        public string MessageType { get; set; }
        public List<CourseModel> Courses { get; set; }

        public StudentModel Details { get; set; }
    }
}
