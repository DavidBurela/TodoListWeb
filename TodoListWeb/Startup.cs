using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TodoListWeb.Startup))]
namespace TodoListWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
