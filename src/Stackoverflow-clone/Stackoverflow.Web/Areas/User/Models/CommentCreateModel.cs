using Autofac;
using Stackoverflow.Application.Features.Services;

namespace Stackoverflow.Web.Areas.User.Models
{
    public class CommentCreateModel
    {
        private ILifetimeScope _scope;
        private ICommentManagementService _commentManagementService;
        public Guid userId { get; set; }
        public Guid postId { get; set; }
        public string Body { get; set; }
        //public DateTime CreationDate { get; set; } 
        //public DateTime LastModifiedDate { get; set; }
        //public bool IsDeleted { get; set; }

        public CommentCreateModel() { }

        public CommentCreateModel(ICommentManagementService commentManagementService)
        {
            _commentManagementService = commentManagementService;
        }

        internal void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _commentManagementService = _scope.Resolve<ICommentManagementService>();
        }

        internal async Task CreateCommentAsync()
        {
            await _commentManagementService.CreateCommentAsync(Body, userId, postId);
        }
    }
}
