using Stackoverflow.Domain;
using Stackoverflow.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stackoverflow.Application
{
    public interface IApplicationUnitOfWork : IUnitOfWork
    {
        IPostRepository PostRepository { get; }
    }
}
