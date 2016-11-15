using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(NIMBOLE.UI.Startup))]

namespace NIMBOLE.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}