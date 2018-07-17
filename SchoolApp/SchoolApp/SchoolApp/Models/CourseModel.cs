using Microsoft.WindowsAzure.Storage.Table;

namespace SchoolApp.Models
{
    public class CourseModel : TableEntity
    {
        /// <summary>
        /// Course name 
        /// </summary>
        public string Course { get; set; }

        /// <summary>
        /// Lecturer Name
        /// </summary>
        public string Lecturer { get; set; }
        /// <summary>
        /// Max quantity of students that can partisipate in cours
        /// </summary>
        public int MaxQuantity { get; set; }

        /// <summary>
        /// How many students are already signedup
        /// </summary>
        public int Quantity { get; set; }
    }
}
