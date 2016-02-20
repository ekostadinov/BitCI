using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BitCI.Startup))]
namespace BitCI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
