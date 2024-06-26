﻿using Stackoverflow.Domain.Entities;

namespace Stackoverflow.Application.Features.Services
{
    public class PostManagementService : IPostManagementService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public PostManagementService(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task CreatePostAsync(string title, string body, Guid userId)
        {
            Post post = new Post
            {
                Title = title,
                Body = body,
                UserId = userId
            };

            await _unitOfWork.PostRepository.AddAsync(post);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeletePostAsync(Guid id)
        {
            await _unitOfWork.PostRepository.RemoveAsync(id);
            //_unitOfWork.PostRepository.Remove(id);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeletePostAPIAsync(Guid id)
        {
            _unitOfWork.PostRepository.Remove(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<(IList<Post> records, int total, int totalDisplay)> GetPagedPostsAsync(int pageIndex, int pageSize, string searchTitle, string sortBy)
        {
            return await _unitOfWork.PostRepository.GetTableDataAsync(searchTitle, sortBy, pageIndex, pageSize);
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

        public async Task<IList<Post>>? GetPostAsync()
        {
            return await _unitOfWork.PostRepository.GetAllAsync();
        }
        
    }
}
