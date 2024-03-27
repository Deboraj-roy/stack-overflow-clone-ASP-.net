using Autofac;
using Stackoverflow.Web.Areas.User.Models;
using Stackoverflow.Web.Models;

namespace Stackoverflow.Web
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PostCreateModel>().AsSelf();
            builder.RegisterType<RegistrationModel>().AsSelf();   
            builder.RegisterType<LoginModel>().AsSelf();
        }
    
    }
}
