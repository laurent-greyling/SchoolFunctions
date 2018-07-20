using Newtonsoft.Json;
using SchoolApp.Models;
using SchoolApp.ViewModels;
using System;
using System.Text.RegularExpressions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SchoolApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CourseSignupView : ContentPage
	{
        public CourseModel _courseModel;
        public SignUpStudentViewModel StudentDetails { get; set; }

        private const string _emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

        public CourseSignupView (CourseModel courseModel)
		{
			InitializeComponent();
            _courseModel = courseModel;
            StudentDetails = new SignUpStudentViewModel(null, _courseModel, false);

            BindingContext = StudentDetails;
		}

        private void Validate_Email(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                StudentEmail.Placeholder = "Enter Email Here";
                StudentEmail.TextColor = Color.Red;
            }
            else
            {
                var isValid = (Regex.IsMatch(e.NewTextValue, _emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
                ((Entry)sender).TextColor = isValid ? Color.Green : Color.Red;
                StudentEmail.Text = e.NewTextValue;
            }
        }

        private void Sign_Up(object sender, EventArgs e)
        {
            if (ValidateStudentInfo())
            {
                var mgmModel = new ManagementModel
                {
                    MessageType = AppConst.SignUp,
                    Details = new StudentModel
                    {
                        Name = StudentName.Text,
                        Surname = StudentSurname.Text,
                        Email = StudentEmail.Text,
                        Age = int.Parse(StudentAge.Text),
                        Course = StudentDetails.Course.Course
                    }
                };

                StudentDetails = new SignUpStudentViewModel(mgmModel, _courseModel, true);

                BindingContext = StudentDetails;
            }
        }

        private bool ValidateStudentInfo()
        {
            if (string.IsNullOrEmpty(StudentName.Text))
            {
                DisplayAlert("Missing Info", "Please enter your name", "Ok");
                return false;
            }

            if (string.IsNullOrEmpty(StudentSurname.Text))
            {
                DisplayAlert("Missing Info", "Please enter your surname", "Ok");
                return false;
            }

            if (string.IsNullOrEmpty(StudentEmail.Text))
            {
                DisplayAlert("Missing Info", "Please enter your email", "Ok");
                return false;
            }

            if (!Regex.IsMatch(StudentEmail.Text, _emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)))
            {
                DisplayAlert("Incorrect Info", "Email is not valid", "Ok");
                return false;
            }

            if (string.IsNullOrEmpty(StudentAge.Text))
            {
                DisplayAlert("Missing Info", "Please enter your age", "Ok");
                return false;
            }

            return true;
        }
    }
}