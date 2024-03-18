using Autofac;
using Stackoverflow.Web.Models;

namespace Stackoverflow.Web
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<RegistrationModel>().AsSelf();
        }
    
    }
}
