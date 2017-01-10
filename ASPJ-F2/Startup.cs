using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ASPJ_F2.Startup))]
namespace ASPJ_F2
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
