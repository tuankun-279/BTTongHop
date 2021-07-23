using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WinStore.Startup))]
namespace WinStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
