using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Initilal_YV_Assesment2.Startup))]
namespace Initilal_YV_Assesment2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
