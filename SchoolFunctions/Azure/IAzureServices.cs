using Microsoft.WindowsAzure.Storage.Table;
using SchoolFunctions.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchoolFunctions.Azure
{
    public interface IAzureServices
    {
        Task CoursesInsertOrMergeAsync();
    }
}
