using Stackoverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stackoverflow.Application.Features.Services
{
    public class PostManagementService : IPostManagementService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public PostManagementService(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task CreatePostAsync(string title, string body)
        {
            Post post = new Post
            {
                Title = title,
                Body = body
            };

            await _unitOfWork.PostRepository.AddAsync(post);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeletePostAsync(Guid id)
        {
            await _unitOfWork.PostRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Post>> GetAllPostAsync()
        {
            return await _unitOfWork.PostRepository.GetAllAsync();
        }

        public async Task<Post> GetPostAsync(Guid id)
        {
            return await _unitOfWork.PostRepository.GetByIdAsync(id);
        }

        public async Task UpdatePostAsync(Guid id, string title, string body)
        {
            var post = await GetPostAsync(id);
            if (post is not null)
            {
                post.Title = title;
                post.Body = body;
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
