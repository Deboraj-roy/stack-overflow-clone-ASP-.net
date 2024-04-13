using Autofac;
using AutoMapper;
using Stackoverflow.Application.Features.Services;
using Stackoverflow.Domain.Entities;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Stackoverflow.Web.Areas.Admin.Models
{
    public class PostUpdateModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required, Range(0, 50000, ErrorMessage = "Fees should be between 0 & 50000")]

        public string body { get; set; }

        private IPostManagementService _postService;
        private IMapper _mapper;

        public PostUpdateModel()
        {

        }

        public PostUpdateModel(IPostManagementService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        internal void Resolve(ILifetimeScope scope)
        {
            _postService = scope.Resolve<IPostManagementService>();
            _mapper = scope.Resolve<IMapper>();
        }

        internal async Task LoadAsync(Guid id)
        {
            Post post = await _postService.GetPostAsync(id);
            if (post != null)
            {
                _mapper.Map(post, this);
            }
        }

        internal async Task UpdateCourseAsync()
        {
            if (!string.IsNullOrWhiteSpace(Title))
            {
                await _postService.UpdatePostAsync(Id, Title, body);
            }
        }
    }
}
