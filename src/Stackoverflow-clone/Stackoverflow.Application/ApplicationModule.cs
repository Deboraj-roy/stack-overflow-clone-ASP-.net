using Autofac;
using Stackoverflow.Application.Features.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stackoverflow.Application
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PostManagementService>().As<IPostManagementService>()
                .InstancePerLifetimeScope();
        }
    }
}
