using AutoMapper;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Infrastructure.Membership;
using Stackoverflow.Web.Areas.User.Models;
using Stackoverflow.Web.Models;

namespace Stackoverflow.Web
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<PostUpdateModel, Post>()
                .ReverseMap();

            CreateMap<UserUpdateModel, ApplicationUser>()
                .ReverseMap();
        }
    }
}
