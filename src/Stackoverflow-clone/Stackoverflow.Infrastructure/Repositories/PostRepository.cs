using Microsoft.EntityFrameworkCore;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Stackoverflow.Infrastructure.Repositories
{
    public class PostRepository : Repository<Post, Guid>, IPostRepository
    {
        public PostRepository(IApplicationDbContext context) : base((DbContext)context)
        {
            
        }

        public async Task<(IList<Post> records, int total, int totalDisplay)> 
            GetTableDataAsync(string searchTitle, string orderBy, int pageIndex, int pageSize)
        {
            Expression<Func<Post, bool>> expression = null;
            if(string.IsNullOrWhiteSpace(searchTitle))
            {
                expression = x => x.Title.Contains(searchTitle);
            }

            return await GetDynamicAsync(expression, orderBy, null, pageIndex, pageSize, true);
        }

        public async Task<bool> IsTitleDuplicateAsync(string title, Guid? id = null)
        {
           if(id.HasValue)
            {
                return (await GetCountAsync(x => x.Id != id.Value && x.Title == title)) > 0;
            }
           else
            {
                return (await GetCountAsync(x => x.Title == title)) > 0;
            }
        }
    }
}
