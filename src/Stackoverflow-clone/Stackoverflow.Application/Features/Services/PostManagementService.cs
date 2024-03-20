using Stackoverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stackoverflow.Application.Features.Services
{
    internal class PostManagementService : IPostManagementService
    {
        public Task CreatePostAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeletePostAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Post>>? GetAllPostAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Post> GetPostAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePostAsync()
        {
            throw new NotImplementedException();
        }
    }
}
