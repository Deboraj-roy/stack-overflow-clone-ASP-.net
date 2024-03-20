using Microsoft.EntityFrameworkCore;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stackoverflow.Infrastructure.Repositories
{
    public class PostRepository : Repository<Post, Guid>, IPostRepository
    {
        public PostRepository(IApplicationDbContext context) : base((DbContext)context)
        {
            
        }
    }
}
