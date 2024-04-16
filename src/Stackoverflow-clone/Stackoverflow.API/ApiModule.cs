using Autofac;
using Stackoverflow.API.RequestHandlers;

namespace Stackoverflow.API
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ViewPostRequestHandler>().AsSelf();
            builder.RegisterType<PostCreateModel>().AsSelf();
            builder.RegisterType<PostUpdateModel>().AsSelf();
            base.Load(builder);
        }
    }
}
