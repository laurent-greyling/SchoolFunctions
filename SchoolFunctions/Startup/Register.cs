
using SimpleInjector;
using System;
using System.Threading.Tasks;

namespace SchoolFunctions.Startup
{
    public class Register : IStartup
    {
        readonly Container _container = new Container();
        public Task RunAsync()
        {
            Type d = Type.GetType("AzureServices");
            _container.Register(d);

            return Task.CompletedTask;
        }
    }
}
