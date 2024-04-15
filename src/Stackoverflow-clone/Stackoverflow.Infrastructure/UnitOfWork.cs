using Stackoverflow.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Stackoverflow.Infrastructure
{
    //public abstract class UnitOfWork : IUnitOfWork
    //{
    //    private readonly DbContext _dbContext;
    //    protected IAdoNetUtility AdoNetUtility { get; private set; }

    //    public UnitOfWork(DbContext dbContext)
    //    {
    //        _dbContext = dbContext;
    //        AdoNetUtility = new AdoNetUtility(_dbContext.Database.GetDbConnection());
    //    }

    //    public void Dispose() => _dbContext?.Dispose();
    //    public async ValueTask DisposeAsync() => await _dbContext.DisposeAsync();
    //    public void Save() => _dbContext?.SaveChanges();
    //    public async Task SaveAsync() => await _dbContext.SaveChangesAsync();
    //}

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
