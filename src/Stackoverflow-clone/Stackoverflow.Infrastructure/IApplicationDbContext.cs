using Microsoft.EntityFrameworkCore;
using Stackoverflow.Domain.Entities;

namespace Stackoverflow.Infrastructure
{
    public interface IApplicationDbContext
    {
        DbSet<Post> posts { get; set; }
    }
//dotnet ef migrations add initial --project Stackoverflow.Web --context ApplicationDbContext
}
