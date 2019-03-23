using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FundMe.Startup))]
namespace FundMe
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
