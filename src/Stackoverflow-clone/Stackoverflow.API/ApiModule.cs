using Autofac;
using Stackoverflow.Api.RequestHandlers;

namespace Stackoverflow.API
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ViewPostRequestHandler>().AsSelf();
            base.Load(builder);
        }
    }
}
