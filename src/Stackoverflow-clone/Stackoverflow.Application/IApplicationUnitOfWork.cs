using Stackoverflow.Domain;
using Stackoverflow.Domain.Repositories;

namespace Stackoverflow.Application
{
    public interface IApplicationUnitOfWork : IUnitOfWork
    {
        IPostRepository PostRepository { get; }
        ICommentRepository CommentRepository { get; }
    }
}
