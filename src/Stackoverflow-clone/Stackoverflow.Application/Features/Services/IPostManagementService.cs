using Stackoverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stackoverflow.Application.Features.Services
{
    public interface IPostManagementService
    {
        Task CreatePostAsync(string title, string body);
        Task DeletePostAsync(Guid id);
        Task<Post> GetPostAsync(Guid id);
        Task UpdatePostAsync(Guid id, string title, string body);
        Task<(IList<Post> records, int total, int totalDisplay)>
            GetPagedPostsAsync(int pageIndex, int pageSize, string searchTitle,
            string sortBy);
        Task<IList<Post>>? GetPostAsync();
    }
}
