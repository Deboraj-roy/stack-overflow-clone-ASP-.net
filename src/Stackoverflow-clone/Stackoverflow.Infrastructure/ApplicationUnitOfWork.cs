using Stackoverflow.Application;
using Stackoverflow.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Stackoverflow.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
	{
		public IPostRepository PostRepository { get; private set; }
        public ICommentRepository CommentRepository { get; private set; }

        public ApplicationUnitOfWork(IPostRepository postRepository,
            ICommentRepository commentRepository,
            IApplicationDbContext dbContext) : base((DbContext)dbContext)
        {
            PostRepository = postRepository;
            CommentRepository = commentRepository;
        }

    }
}
