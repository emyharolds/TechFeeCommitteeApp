using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StudentsTechFeeEvalApp.Startup))]
namespace StudentsTechFeeEvalApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
