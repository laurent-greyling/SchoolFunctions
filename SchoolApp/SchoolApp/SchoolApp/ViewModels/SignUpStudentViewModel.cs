using Newtonsoft.Json;
using SchoolApp.Helpers;
using SchoolApp.Models;
using SchoolApp.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SchoolApp.ViewModels
{
    public class SignUpStudentViewModel : INotifyPropertyChanged
    {
        public NotifyTaskCompletion<StudentModel> _student { get; set; }
        public CourseModel _course { get; set; }
        private readonly IAzureService _azureService;

        public CourseModel Course
        {
            get
            {
                return _course;
            }
            set
            {
                if (_course != value)
                {
                    _course = value;
                    OnPropertyChanged("Course");
                }
            }
        }

        public NotifyTaskCompletion<StudentModel> Student
        {
            get
            {
                return _student;
            }
            set
            {
                if (_student != value)
                {
                    _student = value;
                    OnPropertyChanged("Student");
                }
            }
        }

        public SignUpStudentViewModel(ManagementModel mgmModel, CourseModel courseModel, bool signUp)
        {
            Course = courseModel;

            if (signUp)
            {
                var rowKey = $"{mgmModel.Details.Name.ToLower().Replace(" ", "-")}-{mgmModel.Details.Surname.ToLower().Replace(" ", "-")}-{mgmModel.Details.Course.ToLower().Replace(" ", "-")}";
                _azureService = new AzureService(AppConst.SignUp);
                Student = new NotifyTaskCompletion<StudentModel>(_azureService.RetreiveStudentEntity(AppConst.SignUp, rowKey));

                if (Student.Result == null || (Student.Result != null && !Student.Result.IsSuccess))
                {
                    var message = JsonConvert.SerializeObject(mgmModel);
                    var isSent = new NotifyTaskCompletion<bool>(_azureService.SendMessage(message));
                }

                Student = new NotifyTaskCompletion<StudentModel>(_azureService.RetreiveStudentEntity(AppConst.SignUp, rowKey));
            }            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
