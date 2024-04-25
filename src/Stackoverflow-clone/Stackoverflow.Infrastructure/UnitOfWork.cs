using Stackoverflow.Domain;
using Microsoft.EntityFrameworkCore;

namespace Stackoverflow.Infrastructure
{
    public abstract class UnitOfWork : IUnitOfWork, IDisposable, IAsyncDisposable
    {
        private readonly DbContext _dbContext;
        protected IAdoNetUtility AdoNetUtility { get; private set; }

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
            AdoNetUtility = new AdoNetUtility(_dbContext.Database.GetDbConnection());
        }


        public void Dispose() => _dbContext?.Dispose();
        public async ValueTask DisposeAsync() => await _dbContext.DisposeAsync();
        public void Save() => _dbContext?.SaveChanges();
        public async Task SaveAsync() => await _dbContext.SaveChangesAsync();

    }
}
