using Autofac;
using Stackoverflow.Application;
using Stackoverflow.Application.Utilities;
using Stackoverflow.Domain.Repositories;
using Stackoverflow.Infrastructure.Email;
using Stackoverflow.Infrastructure.Membership;
using Stackoverflow.Infrastructure.Repositories;

namespace Stackoverflow.Infrastructure
{
    public class InfrastructureModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;
        public InfrastructureModule(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContext>().AsSelf()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssembly", _migrationAssembly)
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationDbContext>().As<IApplicationDbContext>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssembly", _migrationAssembly)
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationUnitOfWork>().As<IApplicationUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PostRepository>().As<IPostRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<HtmlEmailService>().As<IEmailService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TokenService>().As<ITokenService>()
               .InstancePerLifetimeScope();

            builder.RegisterType<CommentRepository>().As<ICommentRepository>()
                .InstancePerLifetimeScope();
        }
    }
}
