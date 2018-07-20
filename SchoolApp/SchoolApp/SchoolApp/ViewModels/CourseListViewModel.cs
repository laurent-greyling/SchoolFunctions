using SchoolApp.Helpers;
using SchoolApp.Models;
using SchoolApp.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SchoolApp.ViewModels
{
    public class CourseListViewModel : INotifyPropertyChanged
    {
        public NotifyTaskCompletion<List<CourseModel>> _courses { get; set; }
        private readonly IAzureService _azureService;

        public NotifyTaskCompletion<List<CourseModel>> Courses
        {
            get
            {
                return _courses;
            }
            set
            {
                if (_courses != value)
                {
                    _courses = value;
                    OnPropertyChanged("Courses");
                }
            }
        }

        public CourseListViewModel()
        {
            _azureService = new AzureService(AppConst.UploadCourse);
            Courses = new NotifyTaskCompletion<List<CourseModel>>(_azureService.RetreiveCourseEntities());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
