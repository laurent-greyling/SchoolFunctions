using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolFunctions.Models
{
    public class CourseModel : TableEntity
    {
        public string Course { get; set; }
        public string Lecturer { get; set; }
        public string Quantity { get; set; }
    }
}
