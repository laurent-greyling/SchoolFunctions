using SchoolApp.ViewModels;
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
	}
}
