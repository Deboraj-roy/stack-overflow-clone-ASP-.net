using Stackoverflow.Domain.Entities;

namespace Stackoverflow.Application.Features.Services
{
    public interface ICommentManagementService
    {
        Task CreateCommentAsync(string body, Guid userId, Guid postId);
        Task DeleteCommentAsync(Guid id);
        Task DeleteCommentAPIAsync(Guid id);
        Task<Comment> GetCommentAsync(Guid id);
        Task UpdateCommentAsync(Guid id, string body);
        Task<IList<Comment>>? GetCommentListAsync();
    }
}
