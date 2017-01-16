using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FileFinder.Startup))]
namespace FileFinder
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
