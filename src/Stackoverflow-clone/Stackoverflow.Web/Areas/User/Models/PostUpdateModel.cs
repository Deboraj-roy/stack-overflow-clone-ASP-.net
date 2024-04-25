using Autofac;
using AutoMapper;
using Stackoverflow.Application.Features.Services;
using Stackoverflow.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Stackoverflow.Web.Areas.User.Models
{
    public class PostUpdateModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }

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
            if (_mapper == null)
            {
                throw new Exception("Mapper is not initialized");
            }

            Post post = await _postService.GetPostAsync(id);
            if (post != null)
            {
                _mapper.Map(post, this);
            }
        }

        internal async Task UpdatePostAsync()
        {
            if (!string.IsNullOrWhiteSpace(Title))
            {
                await _postService.UpdatePostAsync(Id, Title, Body);
            }
        }
    }
}
