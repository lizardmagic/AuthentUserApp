using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RCInvigilator.Startup))]
namespace RCInvigilator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
