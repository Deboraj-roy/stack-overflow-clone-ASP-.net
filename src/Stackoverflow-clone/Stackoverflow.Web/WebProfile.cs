using AutoMapper;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Web.Areas.Admin.Models;

namespace Stackoverflow.Web
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<PostUpdateModel, Post>()
                .ReverseMap();
        }
    }
}
