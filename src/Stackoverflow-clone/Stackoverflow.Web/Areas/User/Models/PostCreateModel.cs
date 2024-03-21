using Autofac;
using Stackoverflow.Application.Features.Services;

namespace Stackoverflow.Web.Areas.User.Models
{
    public class PostCreateModel
    {
        private ILifetimeScope _scope;
        private IPostManagementService _postManagementService;

        public string Title { get; set; }
        public string Body { get; set; }

        public PostCreateModel() { }

        public PostCreateModel(IPostManagementService postManagementService)
        {
            _postManagementService = postManagementService;
        }

        internal void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _postManagementService = _scope.Resolve<IPostManagementService>();
        }

        internal async Task CreateCourseAsync()
        {
            await _postManagementService.CreatePostAsync(Title, Body);
        }
    }
}
