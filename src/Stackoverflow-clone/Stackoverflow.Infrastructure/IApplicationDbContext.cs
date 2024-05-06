using Microsoft.EntityFrameworkCore;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Infrastructure.Membership;

namespace Stackoverflow.Infrastructure
{
    public interface IApplicationDbContext
    {
        DbSet<Post> Posts { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<UserBadge> UserBadges { get; set; }
    }
//dotnet ef migrations add initial --project Stackoverflow.Web --context ApplicationDbContext
}
