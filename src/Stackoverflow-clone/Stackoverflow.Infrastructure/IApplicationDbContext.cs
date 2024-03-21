using Microsoft.EntityFrameworkCore;
using Stackoverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stackoverflow.Infrastructure
{
    public interface IApplicationDbContext
    {
        DbSet<Post> posts { get; set; }
    }
//dotnet ef migrations add initial --project Stackoverflow.Web --context ApplicationDbContext
}
