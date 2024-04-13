using Stackoverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stackoverflow.Domain.Repositories
{
    public interface IPostRepository : IRepositoryBase<Post, Guid>
    {
        Task<bool> IsTitleDuplicateAsync(string title, Guid? id = null);

        Task<(IList<Post> records, int total, int totalDisplay)>
            GetTableDataAsync(string searchTitle, string orderBy, int pageIndex, int pageSize);
    }
}
