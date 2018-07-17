using SchoolApp.Models;
using SchoolApp.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SchoolApp
{
	public partial class MainPage : ContentPage
	{
        public CourseListViewModel Courses { get; set; }

        public MainPage()
		{
            Courses = new CourseListViewModel();
            InitializeComponent();

            BindingContext = Courses;
        }

        public async Task Sign_Up(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                await DisplayAlert("Oeps", "Something went wrong", "Ok");
            }
            var selectedCourse = e.Item as CourseModel;

            await Navigation.PushModalAsync(new CourseSignupView(selectedCourse));
        }

    }
}
