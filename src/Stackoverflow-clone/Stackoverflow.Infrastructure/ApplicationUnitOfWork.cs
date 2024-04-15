using Stackoverflow.Application;
using Stackoverflow.Domain.Repositories;
using Stackoverflow.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stackoverflow.Infrastructure
{
	public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
	{
		public IPostRepository PostRepository { get; private set; }

        public ApplicationUnitOfWork(IPostRepository postRepository,
             IApplicationDbContext dbContext) : base((DbContext)dbContext)
        {
            PostRepository = postRepository;
        }

    }
}
