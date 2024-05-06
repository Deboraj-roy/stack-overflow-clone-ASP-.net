using Autofac;
using Stackoverflow.Application.Features.Services;

namespace Stackoverflow.Application
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PostManagementService>().As<IPostManagementService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<CommentManagementService>().As<ICommentManagementService>()
                .InstancePerLifetimeScope();
        }
    }
}
