using Stackoverflow.Domain.Entities;

namespace Stackoverflow.Application.Features.Services
{
    public class CommentManagementService : ICommentManagementService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public CommentManagementService(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task CreateCommentAsync(string body, Guid userId)
        {
            Comment Comment = new Comment
            {
                Body = body,
                UserId = userId
            };

            await _unitOfWork.CommentRepository.AddAsync(Comment);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteCommentAsync(Guid id)
        {
            await _unitOfWork.CommentRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteCommentAPIAsync(Guid id)
        {
            _unitOfWork.CommentRepository.Remove(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<Comment> GetCommentAsync(Guid id)
        {
            return await _unitOfWork.CommentRepository.GetByIdAsync(id);
        }

        public async Task UpdateCommentAsync(Guid id, string body)
        {
            var Comment = await GetCommentAsync(id);
            if (Comment is not null)
            {
                Comment.Body = body;
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task<IList<Comment>>? GetCommentAsync()
        {
            return await _unitOfWork.CommentRepository.GetAllAsync();
        }
    }
}
