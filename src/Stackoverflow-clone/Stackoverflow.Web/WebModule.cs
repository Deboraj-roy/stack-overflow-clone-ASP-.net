﻿using Autofac;
using Stackoverflow.Web.Areas.Admin.Models;
using Stackoverflow.Web.Areas.User.Models;
using Stackoverflow.Web.Models;

namespace Stackoverflow.Web
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PostCreateModel>().AsSelf();
            builder.RegisterType<PostListModel>().AsSelf();
            builder.RegisterType<PostUpdateModel>().AsSelf();
            builder.RegisterType<PostDetailsModel>().AsSelf();
            builder.RegisterType<PostDeleteModel>().AsSelf();
            builder.RegisterType<RegistrationModel>().AsSelf();   
            builder.RegisterType<LoginModel>().AsSelf();
        }
    
    }
}
